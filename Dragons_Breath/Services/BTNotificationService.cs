using Dragons_Breath.Data;
using Dragons_Breath.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Dragons_Breath.Services
{
    public class BTNotificationService : IBTNotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BTUser> _userManager;
        private readonly IBTTicketService _ticketService;

        public BTNotificationService(
            ApplicationDbContext context,
            UserManager<BTUser> userManager,
            IBTTicketService ticketService)
        {
            _context = context;
            _userManager = userManager;
            _ticketService = ticketService;
        }

        //Methods to create notifications for demo users while logged in
        public async Task CheckNotificationsAsync(ClaimsPrincipal user, bool isLogin)
        {
            BTUser currentUser = await _userManager.GetUserAsync(user);

            //If the user logs in and this method hasn't run for more than 1 minute -> prevents abuse
            if (isLogin && currentUser.LastNotificationCheck < DateTimeOffset.Now.AddMinutes(-1))
            {
                //Set up the information we will need as we run these methods 

                //this let's us select a random ticket the user has access to
                Random random = new Random();

                var userTickets = (await _ticketService.ListUserTickets()).ToList();
                int ticketCount = userTickets.Count;
                //Generates a number between 0 and 1 less than the total number of tickets the user has
                int num1 = random.Next(ticketCount);
                int num2 = random.Next(ticketCount);
                int num3 = random.Next(ticketCount);

                int ticketId1 = userTickets[num1].Id;
                int ticketId2 = userTickets[num2].Id;
                int ticketId3 = userTickets[num3].Id;
                //Create multiple notifications so the user has something to look at
                await CreateNotification(ticketId1, "Demo Notification Click to visit a ticket", currentUser.Id, currentUser.Id);
                await CreateNotification(ticketId2, "Demo Notification Click to visit a ticket", currentUser.Id, currentUser.Id);
                await CreateNotification(ticketId3, "Demo Notification Click to visit a ticket", currentUser.Id, currentUser.Id);

                currentUser.LastNotificationCheck = DateTimeOffset.Now;
                await _context.SaveChangesAsync();

            }
            //Runs every two minutes after log in
            else if (currentUser.LastNotificationCheck < DateTimeOffset.Now.AddMinutes(-2))
            {
                //Set up the information we will need as we run these methods 

                //this let's us select a random ticket the user has access to
                Random random = new Random();

                var userTickets = (await _ticketService.ListUserTickets()).ToList();
                int ticketCount = userTickets.Count;
                //Generates a number between 0 and 1 less than the total number of tickets the user has
                int num1 = random.Next(ticketCount);
                //Uses that number to get a random ticketId
                int ticketId1 = userTickets[num1].Id;
                await CreateNotification(ticketId1, "Demo Notification Click to visit a ticket", currentUser.Id, currentUser.Id);

                currentUser.LastNotificationCheck = DateTimeOffset.Now;
                await _context.SaveChangesAsync();
            }

        }

        public async Task CreateNotification(int ticketId, string description, string recipientId, string senderId)
        {
            Notification notification = new Notification
            {
                TicketId = ticketId,
                Description = description,
                Created = DateTimeOffset.Now,
                RecipientId = recipientId,
                SenderId = senderId,
                IsRead = false
            };
            await _context.AddAsync(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Notification>> GetUnreadNotifications(ClaimsPrincipal user)
        {
            var userId = _userManager.GetUserId(user);

            return await _context.Notifications.Where(n => n.RecipientId == userId && !n.IsRead).Include(n => n.Sender).ToListAsync();
        }

        public async Task<int> MoreInfoTicketCount(ClaimsPrincipal currentUser)
        {
            var user = await _userManager.GetUserAsync(currentUser);
            var roles = await _userManager.GetRolesAsync(user);
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
                    return await _context.Tickets.Where(t => t.TicketStatusId == moreInfoId).CountAsync();
                case "ProjectManager":
                    int myTickets = _context.Tickets.Where(t => t.OwnerUserId == user.Id && t.TicketStatusId == moreInfoId).Count();
                    var myRecords = await _context.ProjectUsers.Where(p => p.UserId == user.Id).ToListAsync();
                    foreach(var record in myRecords)
                    {
                        myTickets += _context.Tickets.Where(t => t.ProjectId == record.ProjectId && t.TicketStatusId == moreInfoId).Count();
                    }
                    return myTickets;
                case "Developer":
                    return await _context.Tickets.Where(t => t.TicketStatusId == moreInfoId && (t.DeveloperUserId == user.Id || t.OwnerUserId == user.Id)).CountAsync();
                case "Submitter":
                    return await _context.Tickets.Where(t => t.TicketStatusId == moreInfoId && t.OwnerUserId == user.Id).CountAsync();
                default:
                    return 0;
            }

        }

        public async Task<int> UnassignedTicketCount(ClaimsPrincipal currentUser)
        {
            var user = await _userManager.GetUserAsync(currentUser);
            var roles = await _userManager.GetRolesAsync(user);
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
                    return await _context.Tickets.Where(t => t.TicketStatusId == unassignedId).CountAsync();
                case "ProjectManager":
                    int myTickets = _context.Tickets.Where(t => t.OwnerUserId == user.Id && t.TicketStatusId == unassignedId).Count();
                    var myRecords = await _context.ProjectUsers.Where(p => p.UserId == user.Id).ToListAsync();
                    foreach (var record in myRecords)
                    {
                        myTickets += _context.Tickets.Where(t => t.ProjectId == record.ProjectId && t.TicketStatusId == unassignedId).Count();
                    }
                    return myTickets;
                default:
                    return 0;
            }
        }

        public async Task<int> CriticalTicketCount(ClaimsPrincipal currentUser)
        {
            var user = await _userManager.GetUserAsync(currentUser);
            var roles = await _userManager.GetRolesAsync(user);
            var urgentId = _context.TicketPriorities.FirstOrDefault(t => t.Name == "Critical").Id;
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
                    return await _context.Tickets.Where(t => t.TicketPriorityId == urgentId).CountAsync();
                case "ProjectManager":
                    int myTickets = _context.Tickets.Where(t => t.OwnerUserId == user.Id && t.TicketPriorityId == urgentId).Count();
                    var myRecords = await _context.ProjectUsers.Where(p => p.UserId == user.Id).ToListAsync();
                    foreach (var record in myRecords)
                    {
                        myTickets += _context.Tickets.Where(t => t.ProjectId == record.ProjectId && t.TicketPriorityId == urgentId).Count();
                    }
                    return myTickets;
                case "Developer":
                    return await _context.Tickets.Where(t => t.TicketPriorityId == urgentId && (t.DeveloperUserId == user.Id || t.OwnerUserId == user.Id)).CountAsync();
                case "Submitter":
                    return await _context.Tickets.Where(t => t.TicketPriorityId == urgentId && t.OwnerUserId == user.Id).CountAsync();
                default:
                    return 0;
            }
        }

        public async Task<int> NewTicketCount(ClaimsPrincipal currentUser)
        {
            var user = await _userManager.GetUserAsync(currentUser);
            var roles = await _userManager.GetRolesAsync(user);
            var role = "";
            var dateCheck = DateTimeOffset.Now.AddDays(-7);
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
                    return await _context.Tickets.Where(t => t.Created > dateCheck).CountAsync();
                case "ProjectManager":
                    int myTickets = _context.Tickets.Where(t => t.OwnerUserId == user.Id && t.Created > dateCheck).Count();
                    var myRecords = await _context.ProjectUsers.Where(p => p.UserId == user.Id).ToListAsync();
                    foreach (var record in myRecords)
                    {
                        myTickets += _context.Tickets.Where(t => t.ProjectId == record.ProjectId && t.Created > dateCheck).Count();
                    }
                    return myTickets;
                case "Developer":
                    return await _context.Tickets.Where(t => t.Created > dateCheck && (t.DeveloperUserId == user.Id || t.OwnerUserId == user.Id)).CountAsync();
                case "Submitter":
                    return await _context.Tickets.Where(t => t.Created > dateCheck && t.OwnerUserId == user.Id).CountAsync();
                default:
                    return 0;
            }

        }

        public async Task<int> ResolvedTicketCount(ClaimsPrincipal currentUser)
        {
            var user = await _userManager.GetUserAsync(currentUser);
            var roles = await _userManager.GetRolesAsync(user);
            var role = "";
            var dateCheck = DateTimeOffset.Now.AddDays(-7);
            var resolvedId = _context.TicketStatuses.FirstOrDefault(t => t.Name == "Resolved").Id;

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
                    return await _context.Tickets.Where(t => t.Created > dateCheck && t.TicketStatusId == resolvedId).CountAsync();
                case "ProjectManager":
                    int myTickets = _context.Tickets.Where(t => t.OwnerUserId == user.Id && t.TicketStatusId == resolvedId && t.Created > dateCheck).Count();
                    var myRecords = await _context.ProjectUsers.Where(p => p.UserId == user.Id).ToListAsync();
                    foreach (var record in myRecords)
                    {
                        myTickets += _context.Tickets.Where(t => t.ProjectId == record.ProjectId && t.TicketStatusId == resolvedId && t.Created > dateCheck).Count();
                    }
                    return myTickets;
                case "Developer":
                    return await _context.Tickets.Where(t => t.Created > dateCheck && t.TicketStatusId == resolvedId && (t.DeveloperUserId == user.Id || t.OwnerUserId == user.Id)).CountAsync();
                case "Submitter":
                    return await _context.Tickets.Where(t => t.Created > dateCheck && t.TicketStatusId == resolvedId && t.OwnerUserId == user.Id).CountAsync();
                default:
                    return 0;
            }
        }
    }
}
