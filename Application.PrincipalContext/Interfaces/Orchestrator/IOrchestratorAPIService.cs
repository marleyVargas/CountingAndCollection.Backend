using System;
using System.Threading.Tasks;

namespace Application.PrincipalContext.Interfaces.Orchestrator
{
    public interface IOrchestratorAPIService : IDisposable
    {
        Task<string> VehicleCounting(DateTime queryDate);

        Task<string> VehicleCollection(DateTime queryDate);

    }
}
