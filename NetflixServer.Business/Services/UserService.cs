using NetflixServer.Business.Domain;
using NetflixServer.Business.Interfaces;
using NetflixServer.Business.Models.Responses;
using NetflixServer.Resources.Repositories;
using NetflixServer.Resources.Services;
using NetflixServer.Shared;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace NetflixServer.Business.Services
{
    public class UserService : IUserService
    {
        public UserRepository _userRepository;
        public PlanRepository _subscriptionPlanRepository;
        public MessageService _messageService;

        public UserService(UserRepository userRepository, PlanRepository subscriptionPlanRepository, MessageService messageService)
        {
            _userRepository = userRepository;
            _subscriptionPlanRepository = subscriptionPlanRepository;
            _messageService = messageService;
        }

        public async Task CreateUserAsync(User user, CancellationToken cancellationToken)
        {
            var userId = await _userRepository.InsertUserAsync(user.Email, user.UserName, user.Active);

            //await _messageService
            //        .SendAsync(General.EndpointNameReceiver,
            //            new NotificationCommand
            //            {
            //                Id = userId,
            //                Email = user.Email,
            //                UserName = user.UserName,
            //                NotificationType = NotificationType.UserCreated,
            //            });
        }

        public async Task<UserByIdResponse> GetUserByIdAsync(long userId, CancellationToken cancellationToken)
        {
            var userEntity = await _userRepository.GetUserByIdAsync(userId);

            if (userEntity == null)
            {
                return null;
            }

            //if (!userEntity.SubscriptionPlanId.HasValue)
            //{
            //    return new UserByIdResponse
            //    {
            //        UserId = userEntity.UserId,
            //        SubscriptionPlanId = userEntity.SubscriptionPlanId,
            //        Email = userEntity.Email,
            //        UserName = userEntity.UserName,
            //        Active = userEntity.Active,
            //    };
            //}

            //var subscriptionPlan = await _subscriptionPlanRepository.GetSubscriptionPlanByIdAsync(userEntity.SubscriptionPlanId.Value);

            return new UserByIdResponse
            {
                UserId = userEntity.UserId,
                Email = userEntity.Email,
                UserName = userEntity.UserName,
                Active = userEntity.Active,
            };
        }

        public async Task<List<UserByIdResponse>> GetUserListAsync(CancellationToken cancellationToken)
        {
            var res = await _userRepository.GetUserListAsync();

            List<UserByIdResponse> listOfSubscriptionPlans = new List<UserByIdResponse>();

            foreach (var item in res)
            {
                listOfSubscriptionPlans.Add(new UserByIdResponse
                {
                    UserId = item.UserId,
                    Email = item.Email,
                    UserName = item.UserName,
                    Active = item.Active,
                });
            }

            return listOfSubscriptionPlans;
        }

        public async Task<UserByIdResponse> UpdateUserByIdAsync(long userId, bool isActiveSubsciber, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            object subscriptionPlan = null;//await _subscriptionPlanRepository.GetSubscriptionPlanByIdAsync(subscriptionPlanId);

            if (subscriptionPlan == null || user == null)
                return null;

            //user.SubscriptionPlanId = subscriptionPlanId;
            user.Active = isActiveSubsciber;

            await _userRepository.UpdateUserByIdAsync(user);

            if (!isActiveSubsciber)
            {
                await _messageService
                                    .SendAsync(General.EndpointNameReceiver,
                                        new NotificationCommand
                                        {
                                            Id = user.UserId,
                                            Email = user.Email,
                                            UserName = user.UserName,
                                            Active = user.Active,
                                            //SubscriptionPlanPrice = subscriptionPlan.Price,
                                            //SubscriptionPlanDescription = subscriptionPlan.Description,
                                            //SubscriptionPlanName = subscriptionPlan.Name,
                                            NotificationType = NotificationType.UserDeactivated,
                                            SubscriptionPlanExpirationDate = null,
                                        });
            }
            else
            {
                await _messageService
                                .SendAsync(General.EndpointNameReceiver,
                                    new NotificationCommand
                                    {
                                        Id = user.UserId,
                                        Email = user.Email,
                                        UserName = user.UserName,
                                        Active = user.Active,
                                        //SubscriptionPlanPrice = subscriptionPlan.Price,
                                        //SubscriptionPlanDescription = subscriptionPlan.Description,
                                        //SubscriptionPlanName = subscriptionPlan.Name,
                                        NotificationType = NotificationType.UserActivated,
                                        //SubscriptionPlanExpirationDate = expirationDate,
                                    });
            }

            return new UserByIdResponse
            {
                UserId = user.UserId,
                //SubscriptionPlanId = user.SubscriptionPlanId.Value,
                Email = user.Email,
                UserName = user.UserName,
            };
        }
    }
}
