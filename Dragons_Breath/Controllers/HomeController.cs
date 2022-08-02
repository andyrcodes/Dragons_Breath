using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Dragons_Breath.Models;
using Microsoft.AspNetCore.Authorization;
using Dragons_Breath.Services;
using Dragons_Breath.Models.ViewModels;
using Dragons_Breath.Data;
using Microsoft.EntityFrameworkCore;

namespace Dragons_Breath.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBTNotificationService _notificationService;
        private readonly ApplicationDbContext _context;
        private readonly IBTTicketService _ticketService;

        public HomeController(
            ILogger<HomeController> logger,
            IBTNotificationService notificationService,
            ApplicationDbContext context,
            IBTTicketService ticketService)
        {
            _logger = logger;
            _notificationService = notificationService;
            _context = context;
            _ticketService = ticketService;
        }

        //General access to the application
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        //My dashboard for after login
        public async Task<IActionResult> Dashboard(bool? fromLogin, string filter)
        {
            if (fromLogin != null)
            {
                if (fromLogin.Value)
                {
                    await _notificationService.CheckNotificationsAsync(User, true);
                }

            }

            var model = new DashboardViewModel
            {
                Tickets = await _context.Tickets.Include(t => t.DeveloperUser)
                    .Include(t => t.OwnerUser)
                    .Include(t => t.TicketPriority)
                    .Include(t => t.TicketStatus)
                    .Include(t => t.TicketType).ToListAsync(),
                Projects = await _context.Projects.ToListAsync()
            };

            switch (filter)
            {
                case "unassigned":
                    model.UnassignedTickets = await _ticketService.GetUnassignedTicketsAsync();
                    break;
                case "critical":
                    model.CriticalTickets = await _ticketService.GetCriticalTicketsAsync();
                    break;
                case "moreInfo":
                    model.MoreInfoTickets = await _ticketService.GetMoreInfoTicketsAsync();
                    break;
                default:
                    break;
            }
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
