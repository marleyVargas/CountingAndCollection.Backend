using System;
using System.Threading.Tasks;

namespace Application.PrincipalContext.Interfaces.Orchestrator
{
    public interface IOrchestratorAPIService : IDisposable
    {
        Task<string> VehicleCounting(DateTime consultationDate);

        Task<string> VehicleCollection(DateTime consultationDate);

    }
}
