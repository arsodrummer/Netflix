using NetflixServer.Business.Domain;
using NetflixServer.Models.Requests;

namespace NetflixServer.Api.Mapper
{
    public static class SubscriberMapper
    {
        public static Subscriber ToSubscriber(this NewSubscriberRequest newSubscriberRequest) =>
            new Subscriber
            {
                UserName = newSubscriberRequest.UserName,
                Email = newSubscriberRequest.Email,
                Active = true,
            };
    }
}
