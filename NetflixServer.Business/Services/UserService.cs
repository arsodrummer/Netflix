using NetflixServer.Business.Domain;
using NetflixServer.Business.Interfaces;
using NetflixServer.Business.Models.Responses;
using NetflixServer.Resources.Repositories;
using NetflixServer.Resources.Services;
using NetflixServer.Shared;
using NetflixServer.Shared.Commands;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NetflixServer.Business.Services
{
    public class UserService : IUserService
    {
        public UserRepository _userRepository;
        public PlanRepository _planRepository;
        public MessageService _messageService;

        public UserService(UserRepository userRepository, PlanRepository planRepository, MessageService messageService)
        {
            _userRepository = userRepository;
            _planRepository = planRepository;
            _messageService = messageService;
        }

        public async Task CreateUserAsync(User user, CancellationToken cancellationToken)
        {
            var userId = await _userRepository.InsertUserAsync(user.Email, user.UserName, user.Active);

            await _messageService
                    .SendAsync(General.EndpointNameReceiver,
                        new UserNotificationCommand
                        {
                            Id = userId,
                            Email = user.Email,
                            UserName = user.UserName,
                            NotificationType = NotificationType.UserCreated,
                        });
        }

        public async Task<UserByIdResponse> GetUserByIdAsync(long userId, CancellationToken cancellationToken)
        {
            var userEntity = await _userRepository.GetUserByIdAsync(userId);

            if (userEntity == null)
                return null;

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
            var users = await _userRepository.GetUserListAsync();

            List<UserByIdResponse> listOfUsers = new List<UserByIdResponse>();

            foreach (var user in users)
            {
                listOfUsers.Add(new UserByIdResponse
                {
                    UserId = user.UserId,
                    Email = user.Email,
                    UserName = user.UserName,
                    Active = user.Active,
                });
            }

            return listOfUsers;
        }

        public async Task<UserByIdResponse> UpdateUserByIdAsync(long userId, bool isActiveUser, CancellationToken cancellationToken)
        {
            var userEntity = await _userRepository.GetUserByIdAsync(userId);

            if (userEntity == null)
                return null;

            userEntity.Active = isActiveUser;

            await _userRepository.UpdateUserByIdAsync(userEntity);

            await _messageService
                    .SendAsync(General.EndpointNameReceiver,
                        new UserNotificationCommand
                        {
                            Id = userId,
                            Email = userEntity.Email,
                            UserName = userEntity.UserName,
                            NotificationType = isActiveUser ? NotificationType.UserActivated : NotificationType.UserDeactivated,
                        });

            return new UserByIdResponse
            {
                UserId = userEntity.UserId,
                Email = userEntity.Email,
                UserName = userEntity.UserName,
                Active = userEntity.Active,
            };
        }
    }
}
