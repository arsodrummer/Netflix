using NetflixServer.Business.Models.Responses;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NetflixServer.Business.Interfaces
{
    public interface ISubscriptionService
    {
        Task CreateSubscriptionAsync(long userId, long planId, DateTime expirationDate, CancellationToken cancellationToken);

        Task<List<SubscriptionByIdResponse>> GetSubscriptionListAsync(CancellationToken cancellationToken);

        Task DeleteSubscriptionByIdAsync(long id, CancellationToken cancellationToken);
    }
}
