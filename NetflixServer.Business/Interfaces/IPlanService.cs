using NetflixServer.Business.Domain;
using NetflixServer.Business.Models.Responses;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NetflixServer.Business.Interfaces
{
    public interface IPlanService
    {
        Task CreatePlanAsync(Plan plan, CancellationToken cancellationToken);

        Task<PlanByIdResponse> GetPlanByIdAsync(long planId, CancellationToken cancellationToken);

        Task<List<PlanByIdResponse>> GetPlanListAsync(CancellationToken cancellationToken);

        Task<PlanByIdResponse> UpdatePlanById(long planId, long userId, decimal price, CancellationToken cancellationToken);
    }
}
