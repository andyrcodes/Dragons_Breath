using Dragons_Breath.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dragons_Breath.Services
{
    public interface IBTTicketService
    {
        public Task<bool> IsUserOnTicket(Ticket ticket);

        public Task<IEnumerable<Ticket>> ListUserTickets();

        public Task<IEnumerable<Ticket>> GetUnassignedTicketsAsync();

        public Task<IEnumerable<Ticket>> GetCriticalTicketsAsync();

        public Task<IEnumerable<Ticket>> GetMoreInfoTicketsAsync();
    }
}
