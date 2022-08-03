using NetflixServer.Business.Domain;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NetflixServer.Business.Interfaces
{
    public interface IPlanService
    {
        Task CreatePlanAsync(Plan plan, CancellationToken cancellationToken);

        Task<Plan> GetPlanByIdAsync(long planId, CancellationToken cancellationToken);

        Task<List<Plan>> GetPlanListAsync(CancellationToken cancellationToken);

        Task<Plan> UpdatePlanById(long planId, decimal price, DateTime? expirationDate, string name, CancellationToken cancellationToken);
    }
}
