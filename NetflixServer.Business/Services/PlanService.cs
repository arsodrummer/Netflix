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
    public class PlanService : IPlanService
    {
        public UserRepository _userRepository;
        public PlanRepository _planRepository;
        public MessageService _messageService;

        public PlanService(PlanRepository planRepository, UserRepository userRepository, MessageService messageService)
        {
            _userRepository = userRepository;
            _planRepository = planRepository;
            _messageService = messageService;
        }

        public async Task CreatePlanAsync(Plan plan, CancellationToken cancellationToken)
        {
            await _planRepository.InsertPlanAsync(plan.Name, plan.Price, plan.Description, plan.ExpirationDate);
        }

        public async Task<PlanByIdResponse> GetPlanByIdAsync(long planId, CancellationToken cancellationToken)
        {
            var planEntity = await _planRepository.GetPlanByIdAsync(planId);

            if (planEntity == null)
            {
                return null;
            }

            return new PlanByIdResponse
            {
                PlanId = planEntity.PlanId,
                Description = planEntity.Description,
                Name = planEntity.Name,
                Price = planEntity.Price,
            };
        }

        public async Task<List<PlanByIdResponse>> GetPlanListAsync(CancellationToken cancellationToken)
        {
            var res = await _planRepository.GetPlanListAsync();

            List<PlanByIdResponse> listOfPlans = new List<PlanByIdResponse>();

            foreach (var item in res)
            {
                listOfPlans.Add(new PlanByIdResponse
                {
                    PlanId = item.PlanId,
                    Description = item.Description,
                    Name = item.Name,
                    Price = item.Price,
                });
            }

            return listOfPlans;
        }

        public async Task<PlanByIdResponse> UpdatePlanById(long planId, decimal price, CancellationToken cancellationToken)
        {
            var planEntity = await _planRepository.GetPlanByIdAsync(planId);

            if (planEntity == null)
            {
                return null;
            }

            planEntity.Price = price;
            await _planRepository.UpdatePlanByIdAsync(planEntity);

            //{
            //    await _messageService
            //        .SendAsync(General.EndpointNameReceiver,
            //            new NotificationCommand
            //            {
            //                Id = userEntity.UserId,
            //                Email = userEntity.Email,
            //                UserName = userEntity.UserName,
            //                Active = userEntity.Active,
            //                SubscriptionPlanPrice = planEntity.Price,
            //                SubscriptionPlanDescription = planEntity.Description,
            //                SubscriptionPlanName = planEntity.Name,
            //                NotificationType = NotificationType.SubscriptionPlanUpdated,
            //            });
            //}

            return new PlanByIdResponse
            {
                Description = planEntity.Description,
                Name = planEntity.Name,
                PlanId = planEntity.PlanId,
            };
        }
    }
}
