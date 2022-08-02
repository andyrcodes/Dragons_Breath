using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dragons_Breath.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        #region Ticket Settings
        public IEnumerable<TicketPriority> Priorities { get; set; }

        public IEnumerable<TicketStatus> Statuses { get; set; }

        public IEnumerable<TicketType> Types { get; set; }
        #endregion

        #region Project Creation Wizard
        public IEnumerable<BTUser> DevUsers { get; set; }

        public IEnumerable<BTUser> SubUsers { get; set; }

        public IEnumerable<BTUser> ProjectPM { get; set; }
        #endregion

        #region User Roles and Management
        public IEnumerable<BTUser> AllUsers { get; set; }

        public IEnumerable<string> Roles { get; set; }
        #endregion    
    }
}
