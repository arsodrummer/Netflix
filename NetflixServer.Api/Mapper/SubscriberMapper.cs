using NetflixServer.Business.Domain;
using NetflixServer.Models.Requests;

namespace NetflixServer.Api.Mapper
{
    public static class UserMapper
    {
        public static User ToUser(this NewUserRequest newUserRequest) =>
            new User
            {
                UserName = newUserRequest.UserName,
                Email = newUserRequest.Email,
                Active = true,
            };
    }
}
