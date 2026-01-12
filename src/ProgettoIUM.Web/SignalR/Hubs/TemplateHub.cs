using Microsoft.AspNetCore.SignalR;
using System;

namespace ProgettoIUM.Web.SignalR.Hubs
{
    public interface IProgettoIUMClientEvent
    {
        public System.Threading.Tasks.Task NewMessage(Guid idUser, Guid idMessage);
    }

    [Microsoft.AspNetCore.Authorization.Authorize] // Makes the hub usable only by authenticated users
    public class ProgettoIUMHub : Hub<IProgettoIUMClientEvent>
    {
        private readonly IPublishDomainEvents _publisher;

        public ProgettoIUMHub(IPublishDomainEvents publisher)
        {
            _publisher = publisher;
        }

        public async System.Threading.Tasks.Task JoinGroup(Guid idGroup)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, idGroup.ToString());
        }
        public async System.Threading.Tasks.Task LeaveGroup(Guid idGroup)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, idGroup.ToString());
        }
    }
}
