using System.Threading.Tasks;

namespace ProgettoIUM.Web.SignalR
{
    public interface IPublishDomainEvents
    {
        Task Publish(object evnt);
    }
}
