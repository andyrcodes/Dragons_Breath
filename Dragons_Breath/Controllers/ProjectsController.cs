using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dragons_Breath.Data;
using Dragons_Breath.Models;
using Dragons_Breath.Services;
using Microsoft.AspNetCore.Identity;
using Dragons_Breath.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Dragons_Breath.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly IBTProjectService _projectService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BTUser> _userManager;
        private readonly IBTAttachmentService _attachmentService;
        private readonly IBTRolesService _rolesService;
        private readonly IHttpContextAccessor _httpContext;

        public ProjectsController(
            ApplicationDbContext context,
            IBTProjectService projectService,
            UserManager<BTUser> userManager,
            IBTAttachmentService attachmentService,
            IBTRolesService rolesService,
            IHttpContextAccessor httpContext)
        {
            _context = context;
            _projectService = projectService;
            _userManager = userManager;
            _attachmentService = attachmentService;
            _rolesService = rolesService;
            _httpContext = httpContext;
        }


        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .FirstOrDefaultAsync(m => m.Id == id);

            if (project == null)
            {
                return NotFound();
            }

            var model = new ProjectDetailsViewModel();

            model.Project = project;

            model.Tickets = await _context.Tickets.Where(t => t.ProjectId == id)
                .Include(t => t.DeveloperUser)
                .Include(t => t.OwnerUser)
                .Include(t => t.TicketPriority)
                .Include(t => t.TicketStatus)
                .Include(t => t.TicketType)
                .ToListAsync();

            var developerIds = new List<string>();
            foreach (var dev in await _projectService.DevelopersOnProject(project.Id))
            {
                developerIds.Add(dev.Id);
            };
            var submitterIds = new List<string>();
            foreach (var sub in await _projectService.SubmittersOnProject(project.Id))
            {
                submitterIds.Add(sub.Id);
            };

            var pmId = (await _projectService.ProjectManagerOnProject(project.Id)) != null ? (await _projectService.ProjectManagerOnProject(project.Id)).Id : null;

            ViewData["ProjectManagerId"] = new SelectList(await _rolesService.UsersInRole("ProjectManager"), "Id", "FullName", pmId);
            ViewData["DeveloperIds"] = new MultiSelectList(await _rolesService.UsersInRole("Developer"), "Id", "FullName", developerIds);
            ViewData["SubmitterIds"] = new MultiSelectList(await _rolesService.UsersInRole("Submitter"), "Id", "FullName", submitterIds);

            return View(model);
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,FormFile")] Project project, string projectManagerId, List<string> developerIds, List<string> submitterIds)
        {
            //if (!User.IsInRole("Demo"))
            //{
            if (ModelState.IsValid)
            {
                if (project.FormFile != null)
                {
                    project.ImagePath = project.FormFile.FileName;
                    project.ImageData = await _attachmentService.EncodeAttachment(project.FormFile);
                }
                _context.Add(project);
                await _context.SaveChangesAsync();
                if (User.IsInRole("Admin"))
                {
                    await _projectService.AddUserToProject(projectManagerId, project.Id);
                }
                else
                {
                    var userId = _userManager.GetUserId(_httpContext.HttpContext.User);
                    await _projectService.AddUserToProject(userId, project.Id);

                }
                foreach (var developerId in developerIds)
                {
                    await _projectService.AddUserToProject(developerId, project.Id);
                }
                foreach (var submitterId in submitterIds)
                {
                    await _projectService.AddUserToProject(submitterId, project.Id);
                }

                return RedirectToAction("Details", "Projects", new { id = project.Id });
            }
            TempData["ErrorMessage"] = "Something went wrong creating the project!";
            return RedirectToAction("Dashboard", "Admin");
            //}
            //else
            //{
            //    return RedirectToAction("Dashboard", "Admin");
            //}

        }


        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,FormFile")] Project project)
        {
            if (id != project.Id)
            {
                return NotFound();
            }
            if (!User.IsInRole("Demo"))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var updated = await _context.Projects.FindAsync(id);
                        updated.Name = project.Name;
                        if (project.FormFile != null)
                        {
                            updated.ImageData = await _attachmentService.EncodeAttachment(project.FormFile);
                            updated.ImagePath = project.FormFile.FileName;
                        }
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ProjectExists(project.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(project);
            }
            else
            {
                return RedirectToAction("Details", "Projects", new { id });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignUsers(int id, string projectManagerId, List<string> developerIds, List<string> submitterIds)
        {
            var oldPM = await _projectService.ProjectManagerOnProject(id);
            if (oldPM != null && projectManagerId != oldPM.Id)
            {
                await _projectService.RemoveUserFromProject(oldPM.Id, id);
            }
            await _projectService.AddUserToProject(projectManagerId, id);
            foreach (var dev in developerIds)
            {
                await _projectService.AddUserToProject(dev, id);
            }
            foreach (var sub in submitterIds)
            {
                await _projectService.AddUserToProject(sub, id);
            }

            var allSubIds = new List<string>();
            foreach (var sub in await _projectService.SubmittersOnProject(id))
            {
                allSubIds.Add(sub.Id);
            }
            var removeSubIds = allSubIds.Except(submitterIds);
            foreach (var subId in removeSubIds)
            {
                await _projectService.RemoveUserFromProject(subId, id);
            }

            var allDevIds = new List<string>();
            foreach (var dev in await _projectService.DevelopersOnProject(id))
            {
                allDevIds.Add(dev.Id);
            }
            var removeDevIds = allDevIds.Except(developerIds);
            foreach (var devId in removeDevIds)
            {
                await _projectService.RemoveUserFromProject(devId, id);
            }

            return RedirectToAction("Details", new { id });
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
    }
}
