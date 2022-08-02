
using Dragons_Breath.Models;
using System.Threading.Tasks;

namespace Dragons_Breath.Services
{
    public interface IBTAccessService
    {
        public Task<bool> CanInteractProject(int projectId);

        public Task<bool> CanInteractTicket(int ticketId);

        public Task<bool> CanInteractAttachment(TicketAttachment attachment);

        public Task<bool> CanInteractComment(TicketComment comment);

    }
}
