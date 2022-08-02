using Dragons_Breath.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Dragons_Breath.Services
{
    public interface IBTNotificationService
    {
        public Task CheckNotificationsAsync(ClaimsPrincipal user, bool isLogin);

        public Task CreateNotification(int ticketId, string description, string recipientId, string senderId);

        public Task<IEnumerable<Notification>> GetUnreadNotifications(ClaimsPrincipal user);

        public Task<int> UnassignedTicketCount(ClaimsPrincipal currentUser);
        public Task<int> CriticalTicketCount(ClaimsPrincipal currentUser);
        public Task<int> MoreInfoTicketCount(ClaimsPrincipal currentUser);
        public Task<int> NewTicketCount(ClaimsPrincipal currentUser);
        public Task<int> ResolvedTicketCount(ClaimsPrincipal currentUser);
    }
}
