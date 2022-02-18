using NServiceBus;
using System.Threading.Tasks;

namespace NetflixServer.Resources.Services
{
    public class MessageService
    {
        private IMessageSession _messageSession;

        public MessageService(IMessageSession messageSession)
        {
            _messageSession = messageSession;
        }

        public Task Publish(object @event) => _messageSession.Publish(@event);

        public Task SendAsync(string destination, object message) => _messageSession.Send(destination, message);
    }
}
