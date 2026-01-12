using Microsoft.AspNetCore.SignalR;
using ProgettoIUM.Web.SignalR.Hubs;
using ProgettoIUM.Web.SignalR.Hubs.Events;
using System;
using System.Threading.Tasks;

namespace ProgettoIUM.Web.SignalR
{
    public class SignalrPublishDomainEvents : IPublishDomainEvents
    {
        IHubContext<ProgettoIUMHub, IProgettoIUMClientEvent> _ProgettoIUMHub;

        public SignalrPublishDomainEvents(IHubContext<ProgettoIUMHub, IProgettoIUMClientEvent> ProgettoIUMHub)
        {
            _ProgettoIUMHub = ProgettoIUMHub;
        }

        private IProgettoIUMClientEvent GetProgettoIUMGroup(Guid id)
        {
            return _ProgettoIUMHub.Clients.Group(id.ToString());
        }

        public Task Publish(object evnt)
        {
            try
            {
                return ((dynamic)this).When((dynamic)evnt);
            }
            catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
            {
                return Task.CompletedTask;
            }
        }

        public Task When(NewMessageEvent e)
        {
            return GetProgettoIUMGroup(e.IdGroup).NewMessage(e.IdUser, e.IdMessage);
        }
    }
}
