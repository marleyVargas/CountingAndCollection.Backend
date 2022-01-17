using Application.PrincipalContext.Interfaces.Notification;
using Application.PrincipalContext.Interfaces.Orchestrator;
using Application.PrincipalContext.Interfaces.SchedulerManager;
using Application.PrincipalContext.Interfaces.Transactional;
using DistributedServices.SchedulerManagerServices.Interfaces;
using DistributedServices.SchedulerManagerServices.Services;
using Domain.Nucleus.Entities.Transactional;
using Domain.Nucleus.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Transversal.DTOs.Notifications;
using Transversal.DTOs.Orchestrator.Correspondence;
using Transversal.DTOs.Transactional.Request;
using static Infraestructure.Transversal.Enumerations.Enumerations;

namespace DistributedServices.SchedulerManagerServices.Tasks
{
    public class ProcessTransaction : CronJobService
    {

        private readonly ILogger<ProcessTransaction> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private static Object _lock = new Object();

        public ProcessTransaction(ISchedulerConfig<ProcessTransaction> config, ILogger<ProcessTransaction> logger, IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(ProcessTransaction)} is starting.");
            return base.StartAsync(cancellationToken);
        }

        public override async Task DoWorkAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(ProcessTransaction)} is working.");

            if (!cancellationToken.IsCancellationRequested)
            {
                if (bool.Parse(_configuration["RunParallel"]))
                {
                    await DoWorkParallelAsync();
                }
            }

        }

        private async Task DoWorkParallelAsync()
        {

            try
            {
                lock (_lock)
                {
                    Thread thVerifyPendingTransactions = new Thread(() => VerifyPendingTransactions());
                    thVerifyPendingTransactions.Start();

                    thVerifyPendingTransactions.Join();

                    Thread thSendTransactionsConfirmation = new Thread(() => SendTransactionsConfirmation());
                    thSendTransactionsConfirmation.Start();

                    thSendTransactionsConfirmation.Join();

                    Thread thCorrespondenceReverseTransactions = new Thread(() => CorrespondenceReverseTransactions());
                    thCorrespondenceReverseTransactions.Start();

                    thCorrespondenceReverseTransactions.Join();

                    Thread thCorrespondenceDesistedTransactions = new Thread(() => CorrespondenceDesistedTransactions());
                    thCorrespondenceDesistedTransactions.Start();

                    thCorrespondenceDesistedTransactions.Join();

                    Thread thCorrespondenceCorrespondTransaction = new Thread(() => CorrespondenceCorrespondTransaction());
                    thCorrespondenceCorrespondTransaction.Start();

                    thCorrespondenceCorrespondTransaction.Join();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Failed to verify pending transactions: {0}", ex.Message), GetType());

                throw;
            }
        }

        private void VerifyPendingTransactions()
        {
            try
            {
                _logger.LogDebug("Start the thread to verify pending transactions.", GetType());

                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    using var _unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();

                    var parameters = _unitOfWork.ParametersRepository.GetAll();

                    DateTime createdDate = DateTime.Now.AddMinutes(-Convert.ToInt32(parameters.Where(x => x.Code == "APP_TIME_INTERVAL_MINUTES_CONSULTATION_PENDING").FirstOrDefault().Value));

                    Expression<Func<Collection, bool>> query = q =>
                    (
                        (q.StatusId == StatusType.Pending.GetHashCode()
                        || q.StatusId == StatusType.Validated.GetHashCode()
                        || q.StatusId == StatusType.Authorized.GetHashCode())
                        && q.RegenerateOTPSign == false
                        && q.CreatedDate <= createdDate
                        && q.IsDeleted == false
                    );

                    _logger.LogDebug("Querying pending transactions ...", GetType());

                    var transactions = _unitOfWork.TransactionRepository.GetByFilter(query).ToList();

                    _logger.LogDebug(string.Format("There are {0} pending transactions.", transactions.Count), GetType());

                    if (transactions.Any())
                    {
                        var threadsQuantity = int.Parse(_configuration["MaximumQuantityThreads"]);

                        Parallel.ForEach(transactions, new ParallelOptions { MaxDegreeOfParallelism = threadsQuantity }, async transaction =>
                        {

                            using (var scopeParallel = _serviceScopeFactory.CreateScope())
                            {
                                using var _transactionService = scopeParallel.ServiceProvider.GetService<Application.PrincipalContext.Interfaces.Transactional.IVehicleService>();

                                _logger.LogInformation($"Processing transaction: {transaction.GUID} in {DateTime.Now:hh:mm:ss}");

                                var requestFinalizeTransactionDto = new RequestFinalizeTransactionDto
                                {
                                    GUID = transaction.GUID,
                                    StatusId = transaction.StatusId
                                };

                                await _transactionService.FinalizeTransactionScheduler(requestFinalizeTransactionDto);
                            }

                        });
                    }
                    else
                    {
                        _logger.LogDebug(string.Format("They did not find any transactions to process."), GetType());
                    }

                    _logger.LogDebug("End the thread for verification of pending transactions.", GetType());

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Failed to verify pending transactions: {0}", ex.Message), GetType());

                throw;
            }
        }

        private void SendTransactionsConfirmation()
        {
            try
            {
                _logger.LogDebug("Start the thread to send transaction confirmation.", GetType());

                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    using var _unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();

                    var parameters = _unitOfWork.ParametersRepository.GetAll();

                    DateTime createdDate = DateTime.Now.AddMinutes(-Convert.ToInt32(parameters.Where(x => x.Code == "APP_TIME_INTERVAL_MINUTES_SEND_TRANSACTION_CONFIRMATION").FirstOrDefault().Value));

                    Expression<Func<Collection, bool>> query = q =>
                    (
                        q.ToConfirm == true 
                        && q.IsConfirmed != "200" 
                        && q.CreatedDate <= createdDate
                        && q.IsDeleted == false
                    );

                    _logger.LogDebug("Consulting pending transactions to be confirmed...", GetType());

                    var transactions = _unitOfWork.TransactionRepository.GetByFilter(query).ToList();

                    _logger.LogDebug(string.Format("There are {0} pending transactions to be confirmed.", transactions.Count), GetType());

                    if (transactions.Any())
                    {
                        var threadsQuantity = int.Parse(_configuration["MaximumQuantityThreads"]);

                        Parallel.ForEach(transactions, new ParallelOptions { MaxDegreeOfParallelism = threadsQuantity }, async transaction =>
                        {

                            using (var scopeParallel = _serviceScopeFactory.CreateScope())
                            {

                                using var _notificationService = scopeParallel.ServiceProvider.GetService<INotificationService>();

                                _logger.LogInformation($"Processing transaction: {transaction.GUID} in {DateTime.Now:hh:mm:ss}");

                                ResponsePaymentConfirmationDto confirmationDto = new ResponsePaymentConfirmationDto();

                                await _notificationService.SendPaymentConfirmation(confirmationDto, transaction, null);
                            }

                        });
                    }
                    else
                    {
                        _logger.LogDebug(string.Format("They did not find any transactions to process."), GetType());
                    }

                    _logger.LogDebug("End the thread to send transaction confirmation.", GetType());

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Could not verify pending transactions to be confirmed: {0}", ex.Message), GetType());

                throw;
            }
        }

        private void CorrespondenceReverseTransactions()
        {
            try
            {
                _logger.LogDebug("Starts the thread to reverse correspondence transactions.", GetType());

                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    using var _unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();

                    var parameters = _unitOfWork.ParametersRepository.GetAll();

                    DateTime createdDate = DateTime.Now.AddMinutes(-Convert.ToInt32(parameters.Where(x => x.Code == "APP_TIME_INTERVAL_MINUTES_CONSULTATION_CORRESPONDENCE").FirstOrDefault().Value));

                    Expression<Func<Collection, bool>> query = q =>
                    (
                        q.ToReverse == true 
                        && q.CreatedDate <= createdDate
                        && q.IsDeleted == false
                    );

                    _logger.LogDebug("Consulting pending transactions to reverse by correspondence...", GetType());

                    var transactions = _unitOfWork.TransactionRepository.GetByFilter(query).ToList();

                    _logger.LogDebug(string.Format("There are {0} pending transactions to be reversed by correspondence.", transactions.Count), GetType());

                    if (transactions.Any())
                    {
                        var threadsQuantity = int.Parse(_configuration["MaximumQuantityThreads"]);

                        Parallel.ForEach(transactions, new ParallelOptions { MaxDegreeOfParallelism = threadsQuantity }, async transaction =>
                        {

                            using (var scopeParallel = _serviceScopeFactory.CreateScope())
                            {

                                using var _correspondenceService = scopeParallel.ServiceProvider.GetService<ICorrespondenceService>();

                                _logger.LogInformation($"Processing transaction: {transaction.GUID} in {DateTime.Now:hh:mm:ss}");

                                var requestTransactionCorrespondenceDto = new RequestTransactionCorrespondenceDto
                                {
                                    GUID = transaction.GUID,
                                    CorrespondenceIndicator = CorrespondenceIndicatorType.Reverse,
                                    DescriptionCorrespondence = CorrespondenceIndicatorType.Reverse.GetDescription()
                                };

                                await _correspondenceService.TransactionCorrespondence(requestTransactionCorrespondenceDto);

                            }

                        });
                    }
                    else
                    {
                        _logger.LogDebug(string.Format("They did not find any transactions to process."), GetType());
                    }

                    _logger.LogDebug("End the thread to reverse correspondence transactions.", GetType());

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Pending transactions to be reversed could not be verified by correspondence: {0}", ex.Message), GetType());

                throw;
            }
        }

        private void CorrespondenceDesistedTransactions()
        {
            try
            {
                _logger.LogDebug("Starts the thread to desisted correspondence transactions.", GetType());

                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    using var _unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();

                    var parameters = _unitOfWork.ParametersRepository.GetAll();

                    DateTime createdDate = DateTime.Now.AddMinutes(-Convert.ToInt32(parameters.Where(x => x.Code == "APP_TIME_INTERVAL_MINUTES_CONSULTATION_CORRESPONDENCE").FirstOrDefault().Value));

                    Expression<Func<Collection, bool>> query = q =>
                    (
                        q.ToCorrespond == true 
                        && q.StatusId == StatusType.Desisted.GetHashCode() 
                        && q.CreatedDate <= createdDate
                        && q.IsDeleted == false
                    );

                    _logger.LogDebug("Consulting pending transactions to desisted by correspondence...", GetType());

                    var transactions = _unitOfWork.TransactionRepository.GetByFilter(query).ToList();

                    _logger.LogDebug(string.Format("There are {0} pending transactions to be desisted by correspondence.", transactions.Count), GetType());

                    if (transactions.Any())
                    {
                        var threadsQuantity = int.Parse(_configuration["MaximumQuantityThreads"]);

                        Parallel.ForEach(transactions, new ParallelOptions { MaxDegreeOfParallelism = threadsQuantity }, async transaction =>
                        {

                            using (var scopeParallel = _serviceScopeFactory.CreateScope())
                            {

                                using var _correspondenceService = scopeParallel.ServiceProvider.GetService<ICorrespondenceService>();

                                _logger.LogInformation($"Processing transaction: {transaction.GUID} in {DateTime.Now:hh:mm:ss}");

                                var requestTransactionCorrespondenceDto = new RequestTransactionCorrespondenceDto
                                {
                                    GUID = transaction.GUID,
                                    CorrespondenceIndicator = CorrespondenceIndicatorType.Desist,
                                    DescriptionCorrespondence = CorrespondenceIndicatorType.Desist.GetDescription()
                                };

                                await _correspondenceService.TransactionCorrespondence(requestTransactionCorrespondenceDto);

                            }

                        });
                    }
                    else
                    {
                        _logger.LogDebug(string.Format("They did not find any transactions to process."), GetType());
                    }

                    _logger.LogDebug("End the thread to desisted correspondence transactions.", GetType());

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Pending transactions to be desisted could not be verified by correspondence: {0}", ex.Message), GetType());

                throw;
            }
        }

        private void CorrespondenceCorrespondTransaction()
        {
            try
            {
                _logger.LogDebug("Starts the thread to correspond correspondence transactions.", GetType());

                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    using var _unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();

                    var parameters = _unitOfWork.ParametersRepository.GetAll();

                    DateTime createdDate = DateTime.Now.AddMinutes(-Convert.ToInt32(parameters.Where(x => x.Code == "APP_TIME_INTERVAL_MINUTES_CONSULTATION_CORRESPONDENCE").FirstOrDefault().Value));

                    Expression<Func<Collection, bool>> query = q =>
                    (
                        q.ToCorrespond == true 
                        && q.StatusId == StatusType.Approved.GetHashCode() 
                        && q.CreatedDate <= createdDate
                        && q.IsDeleted == false
                    );

                    _logger.LogDebug("Consulting pending transactions to correspond by correspondence...", GetType());

                    var transactions = _unitOfWork.TransactionRepository.GetByFilter(query).ToList();

                    _logger.LogDebug(string.Format("There are {0} pending transactions to be correspond by correspondence.", transactions.Count), GetType());

                    if (transactions.Any())
                    {
                        var threadsQuantity = int.Parse(_configuration["MaximumQuantityThreads"]);

                        Parallel.ForEach(transactions, new ParallelOptions { MaxDegreeOfParallelism = threadsQuantity }, async transaction =>
                        {

                            using (var scopeParallel = _serviceScopeFactory.CreateScope())
                            {

                                using var _correspondenceService = scopeParallel.ServiceProvider.GetService<ICorrespondenceService>();

                                _logger.LogInformation($"Processing transaction: {transaction.GUID} in {DateTime.Now:hh:mm:ss}");

                                var requestTransactionCorrespondenceDto = new RequestTransactionCorrespondenceDto
                                {
                                    GUID = transaction.GUID,
                                    CorrespondenceIndicator = CorrespondenceIndicatorType.Correspond,
                                    DescriptionCorrespondence = CorrespondenceIndicatorType.Correspond.GetDescription()
                                };

                                await _correspondenceService.TransactionCorrespondence(requestTransactionCorrespondenceDto);

                            }

                        });
                    }
                    else
                    {
                        _logger.LogDebug(string.Format("They did not find any transactions to process."), GetType());
                    }

                    _logger.LogDebug("End the thread to correspond correspondence transactions.", GetType());

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Pending transactions to be correspond could not be verified by correspondence: {0}", ex.Message), GetType());

                throw;
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(ProcessTransaction)} is stopping.");
            return base.StopAsync(cancellationToken);
        }

    }
}
