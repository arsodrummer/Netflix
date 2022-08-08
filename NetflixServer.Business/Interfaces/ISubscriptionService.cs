using System;
using System.Threading;
using System.Threading.Tasks;

namespace NetflixServer.Business.Interfaces
{
    public interface ISubscriptionService
    {
        Task CreateSubscriptionAsync(long userId, long planId, DateTime expirationDate, CancellationToken cancellationToken);
    }
}
