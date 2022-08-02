using Dragons_Breath.Data;
using Dragons_Breath.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Dragons_Breath.Services
{
    public class BTAccessService : IBTAccessService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContext;
        private readonly UserManager<BTUser> _userManager;
        private readonly BTUser _user;

        public BTAccessService(ApplicationDbContext context, IHttpContextAccessor httpContext, UserManager<BTUser> userManager)
        {
            _context = context;
            _httpContext = httpContext;
            _userManager = userManager;
            _user = _context.Users.Find(_userManager.GetUserId(_httpContext.HttpContext.User));
        }

        public async Task<bool> CanInteractAttachment(TicketAttachment attachment)
        {
            var roles = await _userManager.GetRolesAsync(_user);
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
            bool result = false;
            switch (role)
            {
                case "Admin":
                    result = true;
                    break;
                case "ProjectManager":
                    var projectId = (await _context.Tickets.FindAsync(attachment.TicketId)).ProjectId;
                    if (await _context.ProjectUsers.Where(pu => pu.UserId == _user.Id && pu.ProjectId == projectId).AnyAsync() || await _context.Tickets.Where(t => t.OwnerUserId == _user.Id && t.Id == attachment.TicketId).AnyAsync() || attachment.UserId == _user.Id)
                    {
                        result = true;
                    }
                    break;
                case "Developer":
                    if (attachment.UserId == _user.Id)
                    {
                        result = true;
                    }
                    break;
                case "Submitter":
                    if (attachment.UserId == _user.Id)
                    {
                        result = true;
                    }
                    break;
                default:
                    break;
            }
            return result;
        }

        public async Task<bool> CanInteractComment(TicketComment comment)
        {
            var roles = await _userManager.GetRolesAsync(_user);
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
            bool result = false;
            switch (role)
            {
                case "Admin":
                    result = true;
                    break;
                case "ProjectManager":
                    var projectId = (await _context.Tickets.FindAsync(comment.TicketId)).ProjectId;
                    if (await _context.ProjectUsers.Where(pu => pu.UserId == _user.Id && pu.ProjectId == projectId).AnyAsync() || await _context.Tickets.Where(t => t.OwnerUserId == _user.Id && t.Id == comment.TicketId).AnyAsync() || comment.UserId == _user.Id)
                    {
                        result = true;
                    }
                    break;
                case "Developer":
                    if (comment.UserId == _user.Id)
                    {
                        result = true;
                    }
                    break;
                case "Submitter":
                    if (comment.UserId == _user.Id)
                    {
                        result = true;
                    }
                    break;
                default:
                    break;
            }
            return result;
        }

        public async Task<bool> CanInteractProject(int projectId)
        {
            var roles = await _userManager.GetRolesAsync(_user);
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
                    return true;
                case "ProjectManager":
                    if(await _context.ProjectUsers.Where(pu => pu.UserId == _user.Id && pu.ProjectId == projectId).AnyAsync())
                    {
                        return true;
                    }
                    return false;
                default:
                    return false;
            }
        }

        public async Task<bool> CanInteractTicket(int ticketId)
        {
            var roles = await _userManager.GetRolesAsync(_user);
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
            bool result = false;
            switch (role)
            {
                case "Admin":
                    result = true;
                    break;
                case "ProjectManager":
                    var projectId = (await _context.Tickets.FindAsync(ticketId)).ProjectId;
                    if (await _context.ProjectUsers.Where(pu => pu.UserId == _user.Id && pu.ProjectId == projectId).AnyAsync() || await _context.Tickets.Where(t => t.OwnerUserId == _user.Id && t.Id == ticketId).AnyAsync())
                    {
                        result = true;
                    }
                    break;
                case "Developer":
                    if (await _context.Tickets.Where(t => (t.DeveloperUserId == _user.Id || t.OwnerUserId == _user.Id) && t.Id == ticketId).AnyAsync())
                    {
                        result = true;
                    }
                    break;
                case "Submitter":
                    if (await _context.Tickets.Where(t => t.OwnerUserId == _user.Id && t.Id == ticketId).AnyAsync())
                    {
                        result = true;
                    }
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}
