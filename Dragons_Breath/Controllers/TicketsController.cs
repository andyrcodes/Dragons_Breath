using Dragons_Breath.Data;
using Dragons_Breath.Models;
using Dragons_Breath.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Dragons_Breath.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BTUser> _userManager;
        private readonly IBTHistoryService _historyService;
        private readonly IBTAccessService _accessService;
        private readonly IBTProjectService _projectService;
        private readonly IBTAttachmentService _attachmentService;
        private readonly IBTRolesService _rolesService;
        private readonly IHttpContextAccessor _httpContext;

        public TicketsController(ApplicationDbContext context,
            UserManager<BTUser> userManager,
            IBTHistoryService historyService,
            IBTAccessService accessService,
            IBTProjectService projectService,
            IBTAttachmentService attachmentService,
            IBTRolesService rolesService,
            IHttpContextAccessor httpContext)
        {
            _context = context;
            _userManager = userManager;
            _historyService = historyService;
            _accessService = accessService;
            _projectService = projectService;
            _attachmentService = attachmentService;
            _rolesService = rolesService;
            _httpContext = httpContext;
        }


        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var ticket = await _context.Tickets
                .Include(t => t.DeveloperUser)
                .Include(t => t.OwnerUser)
                .Include(t => t.Project)
                .Include(t => t.TicketPriority)
                .Include(t => t.TicketStatus)
                .Include(t => t.TicketType)
                .Include(t => t.Attachments)
                .Include(t => t.Histories)
                .Include(t => t.Comments)
                .ThenInclude(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", ticket.ProjectId);
            ViewData["DeveloperUserId"] = new SelectList(await _projectService.DevelopersOnProject(ticket.ProjectId), "Id", "FullName", ticket.DeveloperUserId);
            return View(ticket);

        }

        public async Task<IActionResult> GoToTicket(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var notification = await _context.Notifications.FirstOrDefaultAsync(m => m.Id == id);
            if (notification == null)
            {
                return NotFound();
            }
            notification.IsRead = true;
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = notification.TicketId });
        }

        // GET: Tickets/Create
        public async Task<IActionResult> Create(int? id)
        {
            if (id == null)
            {
                if (User.IsInRole("Admin") || User.IsInRole("ProjectManager"))
                {
                    ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Name");
                    ViewData["DeveloperUserId"] = new SelectList(_context.Users, "Id", "FullName");
                }
                ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name");
                ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Name");
                ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Name");
                return View();
            }
            else
            {
                var model = new Ticket
                {
                    ProjectId = (int)id
                };
                if (User.IsInRole("Admin") || User.IsInRole("ProjectManager"))
                {
                    ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Name");
                    ViewData["DeveloperUserId"] = new SelectList(await _projectService.DevelopersOnProject((int)id), "Id", "FullName");
                }
                ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Name");
                ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Name");
                return View(model);
            }
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,DeveloperUserId")] Ticket ticket)
        {
            //if (!User.IsInRole("Demo"))
            //{
            if (ModelState.IsValid)
            {
                if (ticket.TicketStatusId == 0)
                {
                    ticket.TicketStatusId = _context.TicketStatuses.Where(ts => ts.Name == "Unassigned").FirstOrDefault().Id;
                }
                ticket.OwnerUserId = _userManager.GetUserId(User);
                ticket.Created = DateTimeOffset.Now;
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = ticket.Id });
            }
            if (User.IsInRole("Admin") || User.IsInRole("ProjectManager"))
            {
                ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Name");
                ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Name");
            }
            if (User.IsInRole("Admin"))
            {
                ViewData["DeveloperUserId"] = new SelectList(_context.Users, "Id", "FullName");
            }
            if (User.IsInRole("ProjectManager"))
            {
                ViewData["DeveloperUserId"] = new SelectList(await _rolesService.UsersInRole("Developer"), "Id", "FullName");
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name");
            ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Name");
            return View(ticket);
            //}
            //else
            //{
            //    TempData["DemoLockout"] = "Your changes have not been saved. To make changes to the database please log in as a full user.";
            //    return RedirectToAction("Dashboard", "Home");
            //}

        }

    
        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,DeveloperUserId,Created")] Ticket ticket)
        {
            if (!User.IsInRole("Demo"))
            {
                if (id != ticket.Id)
                {
                    return NotFound();
                }

                Ticket oldTicket = await _context.Tickets
                .Include(t => t.DeveloperUser)
                .Include(t => t.OwnerUser)
                .Include(t => t.TicketPriority)
                .Include(t => t.TicketStatus)
                .Include(t => t.TicketType)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == ticket.Id);

                if (ModelState.IsValid)
                {
                    try
                    {
                        ticket.Updated = DateTime.Now;
                        _context.Update(ticket);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!TicketExists(ticket.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }

                    Ticket newTicket = await _context.Tickets
                    .Include(t => t.DeveloperUser)
                    .Include(t => t.OwnerUser)
                    .Include(t => t.TicketPriority)
                    .Include(t => t.TicketStatus)
                    .Include(t => t.TicketType)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.Id == ticket.Id);

                    string userId = _userManager.GetUserId(User);

                    await _historyService.AddHistory(oldTicket, newTicket, userId);

                    return RedirectToAction("Details", "Tickets", new { id = ticket.Id });
                }
                ViewData["DeveloperUserId"] = new SelectList(_context.Users, "Id", "Id", ticket.DeveloperUserId);
                ViewData["OwnerUserId"] = new SelectList(_context.Users, "Id", "Id", ticket.OwnerUserId);
                ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", ticket.ProjectId);
                ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Id", ticket.TicketPriorityId);
                ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Id", ticket.TicketStatusId);
                ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Id", ticket.TicketTypeId);
                return View(ticket);
            }
            else
            {
                TempData["DemoLockout"] = "Your changes have not been saved. To make changes to the database please log in as a full user.";
                return RedirectToAction("Details", "Tickets", new { id = ticket.Id });
            }
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }

        public async Task<FileResult> DownloadFile(int? id)
        {
            if (id == null)
            {
                return null;
            }
            TicketAttachment attachment = await _context.TicketAttachments.FirstOrDefaultAsync(t => t.Id == id);
            if (attachment == null)
            {
                return null;
            }
            return File(attachment.FileData, attachment.ContentType);
        }


        #region Ticket Adjacent Models
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAttachment([Bind("FormFile, Description, TicketId")] TicketAttachment ticketAttachment)
        {
            if (ModelState.IsValid)
            {
                ticketAttachment.ContentType = ticketAttachment.FormFile.ContentType;
                ticketAttachment.FileData = await _attachmentService.EncodeAttachment(ticketAttachment.FormFile);
                ticketAttachment.FileName = ticketAttachment.FormFile.FileName;
                ticketAttachment.Created = DateTimeOffset.Now;
                ticketAttachment.UserId = _userManager.GetUserId(User);

                try
                {
                    _context.TicketAttachments.Add(ticketAttachment);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.Message;
                }
            }
            return RedirectToAction("Details", new { id = ticketAttachment.TicketId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateComment(int ticketId, string comment)
        {
            TicketComment ticketComment = new TicketComment
            {
                Comment = comment,
                TicketId = ticketId,
                Created = DateTimeOffset.Now,
                UserId = _userManager.GetUserId(User)
            };
            try
            {
                _context.TicketComments.Add(ticketComment);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Details", new { id = ticketId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditComment(int commentId, string comment)
        {
            TicketComment ticketComment = await _context.TicketComments.FindAsync(commentId);
            var user = await _userManager.GetUserAsync(User);
            string editNotice = $"Edited by: {user.FullName} <br />On: {DateTime.Now:MMM/dd/yyyy}<br />";
            ticketComment.Comment = editNotice + comment;

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Tickets", new { id = ticketComment.TicketId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAttachment(int attachmentId)
        {
            TicketAttachment attachment = await _context.TicketAttachments.FindAsync(attachmentId);
            int ticketId = attachment.TicketId;
            _context.TicketAttachments.Remove(attachment);

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Tickets", new { id = ticketId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            TicketComment comment = await _context.TicketComments.FindAsync(commentId);
            int ticketId = comment.TicketId;
            _context.TicketComments.Remove(comment);

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Tickets", new { id = ticketId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task MarkAsRead(int id)
        {
            var notification = _context.Notifications.Find(id);
            notification.IsRead = true;
            await _context.SaveChangesAsync();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task MarkAllAsRead()
        {
            var userId = _userManager.GetUserId(User);
            var notifications = await _context.Notifications.Where(n => n.RecipientId == userId).ToListAsync();
            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }
            await _context.SaveChangesAsync();
        }
        #endregion
    }
}
