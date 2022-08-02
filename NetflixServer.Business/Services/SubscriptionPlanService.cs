using NetflixServer.Business.Domain;
using NetflixServer.Business.Interfaces;
using NetflixServer.Resources.Repositories;
using NetflixServer.Resources.Services;
using NetflixServer.Shared;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NetflixServer.Business.Services
{
    public class SubscriptionPlanService : ISubscriptionPlanService
    {
        public UserRepository _userRepository;
        public SubscriptionPlanRepository _subscriptionPlanRepository;
        public MessageService _messageService;

        public SubscriptionPlanService(SubscriptionPlanRepository subscriptionPlanRepository, UserRepository userRepository, MessageService messageService)
        {
            _userRepository = userRepository;
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

        public async Task<List<SubscriptionPlan>> GetSubscriptionPlanListAsync(CancellationToken cancellationToken)
        {
            var res = await _subscriptionPlanRepository.GetSubscriptionPlanListAsync();

            List<SubscriptionPlan> listOfSubscriptionPlans = new List<SubscriptionPlan>();

            foreach (var item in res)
            {
                listOfSubscriptionPlans.Add(new SubscriptionPlan
                {
                    SubscriptionPlanId = item.SubscriptionPlanId,
                    Description = item.Description,
                    Name = item.Name,
                    Price = item.Price,
                    ExpirationDate = item.ExpirationDate,
                });
            }

            return listOfSubscriptionPlans;
        }

        public async Task<SubscriptionPlan> UpdateSubscriptionPlanById(long subscriptionPlanId, decimal price, DateTime? expirationDate, string name, CancellationToken cancellationToken)
        {
            var subscriptionPlanEntity = await _subscriptionPlanRepository.GetSubscriptionPlanByIdAsync(subscriptionPlanId);
            var userEntity = await _userRepository.GetUserByIdAsync(subscriptionPlanId);

            if (subscriptionPlanEntity == null)
            {
                return null;
            }
            else if(subscriptionPlanEntity != null && userEntity == null)
            {
                subscriptionPlanEntity.Price = price;
                subscriptionPlanEntity.ExpirationDate = expirationDate;
                subscriptionPlanEntity.Name = name;

                await _subscriptionPlanRepository.UpdateSubscriptionPlanByIdAsync(subscriptionPlanEntity);
            }
            else
            {
                await _messageService
                    .SendAsync(General.EndpointNameReceiver,
                        new NotificationCommand
                        {
                            Id = userEntity.UserId,
                            Email = userEntity.Email,
                            UserName = userEntity.UserName,
                            Active = userEntity.Active,
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
