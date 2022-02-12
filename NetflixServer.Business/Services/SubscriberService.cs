using NetflixServer.Business.Domain;
using NetflixServer.Business.Interfaces;
using NetflixServer.Resources.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace NetflixServer.Business.Services
{
    public class SubscriberService : ISubscriberService
    {
        public SubscriberRepository _subscriberRepository;

        public SubscriberService(SubscriberRepository subscriberRepository)
        {
            _subscriberRepository = subscriberRepository;
        }

        public async Task CreateSubscriberAsync(Subscriber subscriber, CancellationToken cancellationToken)
        {
            await _subscriberRepository.InsertSubscriberAsync(subscriber.Email, subscriber.UserName);
        }
    }
}
