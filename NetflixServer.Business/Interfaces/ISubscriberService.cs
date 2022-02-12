using NetflixServer.Business.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace NetflixServer.Business.Interfaces
{
    public interface ISubscriberService
    {
        Task CreateSubscriberAsync(Subscriber subscriber, CancellationToken cancellationToken);
    }
}
