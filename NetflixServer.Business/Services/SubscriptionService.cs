using NetflixServer.Business.Interfaces;
using NetflixServer.Business.Models.Responses;
using NetflixServer.Resources.Repositories;
using NetflixServer.Resources.Services;
using NetflixServer.Shared;
using NetflixServer.Shared.Commands;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NetflixServer.Business.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        public SubscriptionRepository _subscriptionRepository;
        public UserRepository _userRepository;
        public PlanRepository _planRepository;
        public MessageService _messageService;

        public SubscriptionService(SubscriptionRepository subscriptionRepository,
            PlanRepository planRepository,
            UserRepository userRepository,
            MessageService messageService)
        {
            _subscriptionRepository = subscriptionRepository;
            _planRepository = planRepository;
            _userRepository = userRepository;
            _messageService = messageService;
        }

        public async Task CreateSubscriptionAsync(long userId, long planId, DateTime expirationDate, CancellationToken cancellationToken)
        {
            var subscriptionId = await _subscriptionRepository.InsertSubscriptionAsync(userId, planId, expirationDate);

            var planEntity = await _planRepository.GetPlanByIdAsync(planId);

            var userEntity = await _userRepository.GetUserByIdAsync(userId);

            if (userEntity != null && planEntity != null)
                await _messageService
                        .SendAsync(General.EndpointNameReceiver,
                            new SubscriptionNotificationCommand
                            {
                                Id = subscriptionId,
                                PlanName = planEntity.Name,
                                PlanPrice = planEntity.Price,
                                UserEmail = userEntity.Email,
                                UserName = userEntity.UserName,
                                ExpirationDate = expirationDate,
                                NotificationType = NotificationType.SubscriptionActivated,
                            });
        }

        public async Task<List<SubscriptionByIdResponse>> GetSubscriptionListAsync(CancellationToken cancellationToken)
        {
            var subscriptions = await _subscriptionRepository.GetSubscriptionListAsync();

            List<SubscriptionByIdResponse> listOfSubscriptions = new List<SubscriptionByIdResponse>();

            foreach (var subscription in subscriptions)
            {
                listOfSubscriptions.Add(new SubscriptionByIdResponse
                {
                    Id = subscription.SubscriptionId,
                    UserId = subscription.UserId,
                    PlanId = subscription.PlanId,
                    ExpirationDate = subscription.ExpirationDate,
                });
            }

            return listOfSubscriptions;
        }

        public async Task DeleteSubscriptionByIdAsync(long planId, CancellationToken cancellationToken)
        {
            var subscriptionEntity = await _subscriptionRepository.GetSubscriptionByIdAsync(planId);

            await _subscriptionRepository.DeleteSubscriptionAsync(planId);

            var planEntity = await _planRepository.GetPlanByIdAsync(subscriptionEntity.PlanId);

            var userEntity = await _userRepository.GetUserByIdAsync(subscriptionEntity.UserId);

            if (subscriptionEntity != null && userEntity != null && planEntity != null)
                await _messageService
                        .SendAsync(General.EndpointNameReceiver,
                            new SubscriptionNotificationCommand
                            {
                                Id = subscriptionEntity.SubscriptionId,
                                PlanName = planEntity.Name,
                                PlanPrice = planEntity.Price,
                                UserEmail = userEntity.Email,
                                UserName = userEntity.UserName,
                                NotificationType = NotificationType.SubscriptionDeactivated,
                            });
        }
    }
}
