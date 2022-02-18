using NetflixServer.NServiceBus.Sagas.SagaDatas;
using NetflixServer.Shared;
using NServiceBus;
using System;
using System.Threading.Tasks;

namespace NetflixServer.NServiceBus.Sagas
{
    internal class NotificationSaga :
                    Saga<NotificationSagaData>,
                    IAmStartedByMessages<NotificationCommand>,
                    IHandleTimeouts<NotificationCommand>
    //IHandleMessages<LogMessageResponse>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<NotificationSagaData> mapper)
        {
            mapper.ConfigureMapping<NotificationCommand>(message => $"{message.Id}")
                    .ToSaga(saga => saga.CorrelationId);
        }

        public async Task Handle(NotificationCommand message, IMessageHandlerContext context)
        {
            await RequestTimeout(context, TimeSpan.FromSeconds(2), message);

            // ...
            // construct email and send it
            // ...

            
            //return Task.CompletedTask;
        }

        public Task Timeout(NotificationCommand state, IMessageHandlerContext context)
        {
            // ...
            // construct email and send it
            // ...

            MarkAsComplete();
            return Task.CompletedTask;
        }
    }

    //internal class MyCustomTimeout { }
}
