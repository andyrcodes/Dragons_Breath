using Dragons_Breath.Data;
using Dragons_Breath.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dragons_Breath.Services
{
    public class BTTicketService : IBTTicketService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BTUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IBTProjectService _projectService;
        private readonly BTUser _user;

        public BTTicketService(
            ApplicationDbContext context,
            UserManager<BTUser> userManager,
            IHttpContextAccessor contextAccessor,
            IBTProjectService projectService)
        {
            _context = context;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
            _projectService = projectService;
            _user = _context.Users.Find(_userManager.GetUserId(_contextAccessor.HttpContext.User));
        }

        public async Task<bool> IsUserOnTicket(Ticket ticket)
        {
            if (_contextAccessor.HttpContext.User.IsInRole("Admin"))
            {
                return true;
            }
            else if (_contextAccessor.HttpContext.User.IsInRole("ProjectManager"))
            {
                if (await _projectService.IsUserOnProject(_user.Id, ticket.ProjectId) || ticket.OwnerUserId == _user.Id)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return _context.Tickets.Where(t => (t.DeveloperUserId == _user.Id || t.OwnerUserId == _user.Id) && t.Id == ticket.Id).Any();
        }

        public async Task<IEnumerable<Ticket>> ListUserTickets()
        {
            if (_contextAccessor.HttpContext.User.IsInRole("Admin"))
            {
                return await _context.Tickets.ToListAsync();
            }
            else if (_contextAccessor.HttpContext.User.IsInRole("ProjectManager"))
            {
                BTUser user = await _context.Users.Include(p => p.ProjectUsers).ThenInclude(p => p.Project).ThenInclude(p => p.Tickets).FirstOrDefaultAsync(p => p.Id == _user.Id);
                var data = user.ProjectUsers.Select(p => p.Project).Select(p => p.Tickets);
                List<Ticket> tickets = new List<Ticket>();
                foreach (var collection in data)
                {
                    tickets.AddRange(collection);
                }
                return tickets;
            }
            return await _context.Tickets.Where(t => t.DeveloperUserId == _user.Id || t.OwnerUserId == _user.Id).ToListAsync();

            //var tickets = _context.Tickets.ToList();
            //var myTickets = new List<Ticket>();
            //foreach(var ticket in tickets)
            //{
            //    if(await IsUserOnTicket(ticket))
            //    {
            //        myTickets.Add(ticket);
            //    }
            //}
            //return myTickets;
        }

        public async Task<IEnumerable<Ticket>> GetCriticalTicketsAsync()
        {
            var roles = await _userManager.GetRolesAsync(_user);
            var criticalId = _context.TicketPriorities.FirstOrDefault(t => t.Name == "Critical").Id;
            var role = "";
            if (roles.Count > 1)
            {
                foreach (var name in roles)
                {
                    if (name != "Demo")
                    {
                        role = name;
                    }
                }
            }
            else
            {
                role = roles.FirstOrDefault();
            }
            switch (role)
            {
                case "Admin":
                    return await _context.Tickets.Where(t => t.TicketPriorityId == criticalId).ToListAsync();
                case "ProjectManager":
                    var output = new List<Ticket>();
                    var dataPacket = await _context.ProjectUsers.Where(p => p.UserId == _user.Id).Select(p => p.Project).Select(p => p.Tickets.Where(t => (t.TicketPriorityId == criticalId) || (t.OwnerUserId == _user.Id))).ToListAsync();
                    foreach (var projectSet in dataPacket)
                    {
                        foreach (var ticket in projectSet)
                        {
                            output.Add(ticket);
                        }
                    }
                    return output;
                //return (IEnumerable<Ticket>)_context.ProjectUsers.Where(p => p.UserId == _user.Id).Select(p => p.Project).Select(p => p.Tickets.Where(t => (t.TicketPriorityId == criticalId) || (t.OwnerUserId == _user.Id))).ToList();
                case "Developer":
                    return await _context.Tickets.Where(t => t.TicketPriorityId == criticalId && (t.DeveloperUserId == _user.Id || t.OwnerUserId == _user.Id)).ToListAsync();
                case "Submitter":
                    return await _context.Tickets.Where(t => t.TicketPriorityId == criticalId && t.OwnerUserId == _user.Id).ToListAsync();
                default:
                    return new List<Ticket>();
            }

        }

        public async Task<IEnumerable<Ticket>> GetMoreInfoTicketsAsync()
        {
            var roles = await _userManager.GetRolesAsync(_user);
            var moreInfoId = _context.TicketStatuses.FirstOrDefault(t => t.Name == "More Info Needed").Id;
            var role = "";
            if (roles.Count > 1)
            {
                foreach (var name in roles)
                {
                    if (name != "Demo")
                    {
                        role = name;
                    }
                }
            }
            else
            {
                role = roles.FirstOrDefault();
            }
            switch (role)
            {
                case "Admin":
                    return await _context.Tickets.Where(t => t.TicketStatusId == moreInfoId).ToListAsync();
                case "ProjectManager":
                    var output = new List<Ticket>();
                    var dataPacket = await _context.ProjectUsers.Where(p => p.UserId == _user.Id).Select(p => p.Project).Select(p => p.Tickets.Where(t => (t.TicketStatusId == moreInfoId) || (t.OwnerUserId == _user.Id))).ToListAsync();
                    foreach(var projectSet in dataPacket)
                    {
                        foreach(var ticket in projectSet)
                        {
                            output.Add(ticket);
                        }
                    }
                    return output;
                    //return (IEnumerable<Ticket>)_context.ProjectUsers.Where(p => p.UserId == _user.Id).Select(p => p.Project).Select(p => p.Tickets.Where(t => (t.TicketStatusId == moreInfoId) || (t.OwnerUserId == _user.Id))).ToList();
                case "Developer":
                    return await _context.Tickets.Where(t => t.TicketStatusId == moreInfoId && (t.DeveloperUserId == _user.Id || t.OwnerUserId == _user.Id)).ToListAsync();
                case "Submitter":
                    return await _context.Tickets.Where(t => t.TicketStatusId == moreInfoId && t.OwnerUserId == _user.Id).ToListAsync();
                default:
                    return new List<Ticket>();
            }

        }

        public async Task<IEnumerable<Ticket>> GetUnassignedTicketsAsync()
        {
            var roles = await _userManager.GetRolesAsync(_user);
            var unassignedId = _context.TicketStatuses.FirstOrDefault(t => t.Name == "Unassigned").Id;
            var role = "";
            if (roles.Count > 1)
            {
                foreach (var name in roles)
                {
                    if (name != "Demo")
                    {
                        role = name;
                    }
                }
            }
            else
            {
                role = roles.FirstOrDefault();
            }
            switch (role)
            {
                case "Admin":
                    return await _context.Tickets.Where(t => t.TicketStatusId == unassignedId).ToListAsync();
                case "ProjectManager":
                    var output = new List<Ticket>();
                    var dataPacket = await _context.ProjectUsers.Where(p => p.UserId == _user.Id).Select(p => p.Project).Select(p => p.Tickets.Where(t => (t.TicketStatusId == unassignedId) || (t.OwnerUserId == _user.Id))).ToListAsync();
                    foreach (var projectSet in dataPacket)
                    {
                        foreach (var ticket in projectSet)
                        {
                            output.Add(ticket);
                        }
                    }
                    return output;
                //return (IEnumerable<Ticket>)_context.ProjectUsers.Where(p => p.UserId == _user.Id).Select(p => p.Project).Select(p => p.Tickets.Where(t => (t.TicketStatusId == unassignedId) || (t.OwnerUserId == _user.Id))).ToList();
                case "Developer":
                    return await _context.Tickets.Where(t => t.TicketStatusId == unassignedId && (t.DeveloperUserId == _user.Id || t.OwnerUserId == _user.Id)).ToListAsync();
                case "Submitter":
                    return await _context.Tickets.Where(t => t.TicketStatusId == unassignedId && t.OwnerUserId == _user.Id).ToListAsync();
                default:
                    return new List<Ticket>();
            }

        }
    }
}
