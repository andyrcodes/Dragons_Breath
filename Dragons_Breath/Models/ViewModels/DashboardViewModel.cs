using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dragons_Breath.Models.ViewModels
{
    public class DashboardViewModel
    {
        public DashboardViewModel()
        {
            Tickets = new List<Ticket>();
            Projects = new List<Project>();
            UnassignedTickets = new List<Ticket>();
            CriticalTickets = new List<Ticket>();
            MoreInfoTickets = new List<Ticket>();
        }
        public IEnumerable<Ticket> Tickets { get; set; }

        public IEnumerable<Project> Projects { get; set; }

        public IEnumerable<Ticket> UnassignedTickets { get; set; }

        public IEnumerable<Ticket> CriticalTickets { get; set; }

        public IEnumerable<Ticket> MoreInfoTickets { get; set; }

    }
}
