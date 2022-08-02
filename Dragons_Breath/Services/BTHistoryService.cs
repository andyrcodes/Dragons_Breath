using Dragons_Breath.Data;
using Dragons_Breath.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dragons_Breath.Services
{
    public class BTHistoryService : IBTHistoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IBTNotificationService _notificationService;
        private readonly IEmailSender emailSender;

        public BTHistoryService(ApplicationDbContext context, IBTNotificationService notificationService, IEmailSender emailSender)
        {
            _context = context;
            _notificationService = notificationService;
            this.emailSender = emailSender;
        }

        public async Task AddHistory(Ticket oldTicket, Ticket newTicket, string userId)
        {
            if (oldTicket.Title != newTicket.Title)
            {
                await CreateHistory(newTicket.Id, "Title", oldTicket.Title, newTicket.Title, userId);
            };
            if (oldTicket.Description != newTicket.Description)
            {
                await CreateHistory(newTicket.Id, "Description", oldTicket.Description, newTicket.Description, userId);
            };
            if (oldTicket.TicketTypeId != newTicket.TicketTypeId)
            {
                await CreateHistory(newTicket.Id, "Type", oldTicket.TicketType.Name, newTicket.TicketType.Name, userId);
            };
            if (oldTicket.TicketStatusId != newTicket.TicketStatusId)
            {
                await CreateHistory(newTicket.Id, "Status", oldTicket.TicketStatus.Name, newTicket.TicketStatus.Name, userId);
            };
            if (oldTicket.TicketPriorityId != newTicket.TicketPriorityId)
            {
                await CreateHistory(newTicket.Id, "Priority", oldTicket.TicketPriority.Name, newTicket.TicketPriority.Name, userId);
            };
            if (oldTicket.DeveloperUserId != newTicket.DeveloperUserId)
            {
                if (string.IsNullOrEmpty(oldTicket.DeveloperUserId))
                {
                    await CreateHistory(newTicket.Id, "Developer", "No Developer Assigned", newTicket.DeveloperUser.FullName, userId);
                    await _notificationService.CreateNotification(newTicket.Id, "You have been assigned a new ticket", newTicket.DeveloperUserId, userId);
                }
                else if (string.IsNullOrEmpty(newTicket.DeveloperUserId))
                {
                    await CreateHistory(newTicket.Id, "Developer", oldTicket.DeveloperUser.FullName, "No Developer Assigned", userId);
                    await _notificationService.CreateNotification(newTicket.Id, "You have been removed from a ticket", oldTicket.DeveloperUserId, userId);
                }
                else
                {
                    await CreateHistory(newTicket.Id, "Developer", oldTicket.DeveloperUser.FullName, newTicket.DeveloperUser.FullName, userId);
                    await _notificationService.CreateNotification(newTicket.Id, "You have been assigned a new ticket", newTicket.DeveloperUserId, userId);
                    await _notificationService.CreateNotification(newTicket.Id, "You have been removed from a ticket", oldTicket.DeveloperUserId, userId);
                };
            };
        }

        private async Task CreateHistory(int ticketId, string property, string oldValue, string newValue, string userId)
        {
            TicketHistory history = new TicketHistory
            {
                TicketId = ticketId,
                Property = property,
                OldValue = oldValue,
                NewValue = newValue,
                UserId = userId,
                Created = DateTimeOffset.Now
            };
            await _context.AddAsync(history);
            await _context.SaveChangesAsync();
        }

    }
}
