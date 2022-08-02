using Dragons_Breath.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dragons_Breath.Services
{
    public interface IBTHistoryService
    {
        public Task AddHistory(Ticket oldTicket, Ticket newTicker, string userId);
    }
}
