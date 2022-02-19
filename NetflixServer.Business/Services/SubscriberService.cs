using NetflixServer.Business.Domain;
using NetflixServer.Business.Interfaces;
using NetflixServer.Business.Models.Responses;
using NetflixServer.Resources.Repositories;
using NetflixServer.Resources.Services;
using NetflixServer.Shared;
using System.Threading;
using System.Threading.Tasks;


namespace NetflixServer.Business.Services
{
    public class SubscriberService : ISubscriberService
    {
        public SubscriberRepository _subscriberRepository;
        public SubscriptionPlanRepository _subscriptionPlanRepository;
        public MessageService _messageService;

        public SubscriberService(SubscriberRepository subscriberRepository, SubscriptionPlanRepository subscriptionPlanRepository, MessageService messageService)
        {
            _subscriberRepository = subscriberRepository;
            _subscriptionPlanRepository = subscriptionPlanRepository;
            _messageService = messageService;
        }

        public async Task CreateSubscriberAsync(Subscriber subscriber, CancellationToken cancellationToken)
        {
            var subscriberId = await _subscriberRepository.InsertSubscriberAsync(subscriber.Email, subscriber.UserName, subscriber.Active);

            await _messageService
                    .SendAsync("NServiceBus", 
                        new NotificationCommand
                        {
                            Id = subscriberId,
                            Email = subscriber.Email,
                            UserName = subscriber.UserName,
                            NotificationType = NotificationType.SubscriberCreated,
                        });
        }

        public async Task<SubscriberByIdResponse> GetSubscriberByIdAsync(long subscriberId, CancellationToken cancellationToken)
        {
            var res = await _subscriberRepository.GetSubscriberByIdAsync(subscriberId);
            
            if (res == null)
            {
                return null;
            }

            var subscriptionPlan = await _subscriptionPlanRepository.GetSubscriptionPlanByIdAsync(res.SubscriptionPlanId.Value);

            return new SubscriberByIdResponse
            {
                SubscriberId = res.SubscriberId,
                SubscriptionPlanId = res.SubscriptionPlanId,
                Email = res.Email,
                UserName = res.UserName,
                Description = subscriptionPlan.Description,
                Name = subscriptionPlan.Name,
                Price = subscriptionPlan.Price,
                Active = res.Active,
                ExpirationDate = subscriptionPlan.ExpirationDate,
            };
        }

        public async Task<SubscriberByIdResponse> UpdateSubscriberByIdAsync(long subscriberId, long subscriptionPlanId, CancellationToken cancellationToken)
        {
            var subscriber = await _subscriberRepository.GetSubscriberByIdAsync(subscriberId);
            var subscriptionPlan = await _subscriptionPlanRepository.GetSubscriptionPlanByIdAsync(subscriptionPlanId);
            
            if (subscriptionPlan == null)
                return null;

            subscriber.SubscriptionPlanId = subscriptionPlanId;

            await _subscriberRepository.UpdateSubscriberByIdAsync(subscriber);

            await _messageService
                    .SendAsync("NServiceBus",
                        new NotificationCommand
                        {
                            Id = subscriber.SubscriberId,
                            Email = subscriber.Email,
                            UserName = subscriber.UserName,
                            Active = subscriber.Active,
                            SubscriptionPlanPrice = subscriptionPlan.Price,
                            SubscriptionPlanDescription = subscriptionPlan.Description,
                            SubscriptionPlanName = subscriptionPlan.Name,
                            NotificationType = NotificationType.SubscriberActivated,
                        });

            return new SubscriberByIdResponse
            {
                SubscriberId = subscriber.SubscriberId,
                SubscriptionPlanId = subscriber.SubscriptionPlanId.Value,
                Email = subscriber.Email,
                UserName = subscriber.UserName,
            };
        }
    }
}
