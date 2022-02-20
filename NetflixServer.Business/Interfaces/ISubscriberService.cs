using NetflixServer.Business.Domain;
using NetflixServer.Business.Models.Responses;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NetflixServer.Business.Interfaces
{
    public interface ISubscriberService
    {
        Task CreateSubscriberAsync(Subscriber subscriber, CancellationToken cancellationToken);

        Task<SubscriberByIdResponse> GetSubscriberByIdAsync(long subscriberId, CancellationToken cancellationToken);

        Task<SubscriberByIdResponse> UpdateSubscriberByIdAsync(long subscriberId, long subscriptionPlanId, DateTime? expirationDate, CancellationToken cancellationToken);
    }
}
