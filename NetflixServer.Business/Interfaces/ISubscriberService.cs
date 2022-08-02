using NetflixServer.Business.Domain;
using NetflixServer.Business.Models.Responses;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NetflixServer.Business.Interfaces
{
    public interface IUserService
    {
        Task CreateUserAsync(User user, CancellationToken cancellationToken);

        Task<UserByIdResponse> GetUserByIdAsync(long userId, CancellationToken cancellationToken);

        Task<List<UserByIdResponse>> GetUserListAsync(CancellationToken cancellationToken);

        Task<UserByIdResponse> UpdateUserByIdAsync(long userId, long subscriptionPlanId, DateTime? expirationDate, bool isActiveSubsciber, CancellationToken cancellationToken);
    }
}
