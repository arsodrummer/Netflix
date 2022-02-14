using NetflixServer.Business.Domain;
using NetflixServer.Business.Models.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace NetflixServer.Business.Interfaces
{
    public interface ISubscriberService
    {
        Task CreateSubscriberAsync(Subscriber subscriber, CancellationToken cancellationToken);

        Task<SubscriberByIdResponse> GetSubscriberByIdAsync(string subscriberId, CancellationToken cancellationToken);
    }
}
