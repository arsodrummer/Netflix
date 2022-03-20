using NetflixServer.Business.Domain;
using NetflixServer.Business.Interfaces;
using NetflixServer.Resources.Repositories;
using NetflixServer.Resources.Services;
using NetflixServer.Shared;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NetflixServer.Business.Services
{
    public class SubscriptionPlanService : ISubscriptionPlanService
    {
        public SubscriberRepository _subscriberRepository;
        public SubscriptionPlanRepository _subscriptionPlanRepository;
        public MessageService _messageService;

        public SubscriptionPlanService(SubscriptionPlanRepository subscriptionPlanRepository, SubscriberRepository subscriberRepository, MessageService messageService)
        {
            _subscriberRepository = subscriberRepository;
            _subscriptionPlanRepository = subscriptionPlanRepository;
            _messageService = messageService;
        }

        public async Task CreateSubscriptionPlanAsync(SubscriptionPlan subscriptionPlan, CancellationToken cancellationToken)
        {
            await _subscriptionPlanRepository.InsertSubscriptionPlanAsync(subscriptionPlan.Name, subscriptionPlan.Price, subscriptionPlan.Description, subscriptionPlan.ExpirationDate);
        }

        public async Task<SubscriptionPlan> GetSubscriptionPlanByIdAsync(long subscriptionPlanId, CancellationToken cancellationToken)
        {
            var subscriptionPlanEntity = await _subscriptionPlanRepository.GetSubscriptionPlanByIdAsync(subscriptionPlanId);

            if (subscriptionPlanEntity == null)
            {
                return null;
            }

            return new SubscriptionPlan
            {
                SubscriptionPlanId = subscriptionPlanEntity.SubscriptionPlanId,
                Description = subscriptionPlanEntity.Description,
                Name = subscriptionPlanEntity.Name,
                Price = subscriptionPlanEntity.Price,
                ExpirationDate = subscriptionPlanEntity.ExpirationDate,
            };
        }

        public async Task<SubscriptionPlan> UpdateSubscriptionPlanById(long subscriptionPlanId, decimal price, DateTime? expirationDate, CancellationToken cancellationToken)
        {
            var subscriptionPlanEntity = await _subscriptionPlanRepository.GetSubscriptionPlanByIdAsync(subscriptionPlanId);
            var subscriberEntity = await _subscriberRepository.GetSubscriberByIdAsync(subscriptionPlanId);

            if (subscriptionPlanEntity == null)
            {
                return null;
            }
            else if(subscriptionPlanEntity != null && subscriberEntity == null)
            {
                subscriptionPlanEntity.Price = price;
                subscriptionPlanEntity.ExpirationDate = expirationDate;

                await _subscriptionPlanRepository.UpdateSubscriptionPlanByIdAsync(subscriptionPlanEntity);
            }
            else
            {
                await _messageService
                    .SendAsync("NServiceBus",
                        new NotificationCommand
                        {
                            Id = subscriberEntity.SubscriberId,
                            Email = subscriberEntity.Email,
                            UserName = subscriberEntity.UserName,
                            Active = subscriberEntity.Active,
                            SubscriptionPlanPrice = subscriptionPlanEntity.Price,
                            SubscriptionPlanDescription = subscriptionPlanEntity.Description,
                            SubscriptionPlanName = subscriptionPlanEntity.Name,
                            NotificationType = NotificationType.SubscriptionPlanUpdated,
                        });
            }

            return new SubscriptionPlan
            {
                Price = price,
                ExpirationDate = expirationDate,
                Description = subscriptionPlanEntity.Description,
                Name = subscriptionPlanEntity.Name,
                SubscriptionPlanId = subscriptionPlanEntity.SubscriptionPlanId,
            };
        }
    }
}
