using Dragons_Breath.Data;
using Dragons_Breath.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Dragons_Breath.Services
{
    public class BTProjectService : IBTProjectService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BTUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;

        public BTProjectService(
            ApplicationDbContext context,
            UserManager<BTUser> userManager,
            IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
        }

        public async Task<bool> IsUserOnProject(string userId, int projectId)
        {
            if (await _userManager.IsInRoleAsync(await _userManager.FindByIdAsync(userId), "Admin"))
            {
                return true;
            }
            return _context.ProjectUsers.Where(pu => pu.UserId == userId && pu.ProjectId == projectId).Any();
        }

        public async Task<IEnumerable<Project>> ListUserProjects(string userId)
        {
            if (_contextAccessor.HttpContext.User.IsInRole("Admin"))
            {
                return await _context.Projects.ToListAsync();
            }
            BTUser user = await _context.Users.Include(p => p.ProjectUsers).ThenInclude(p => p.Project).FirstOrDefaultAsync(p => p.Id == userId);
            List<Project> projects = user.ProjectUsers.Select(p => p.Project).ToList();
            return projects;
        }

        public async Task AddUserToProject(string userId, int projectId)
        {
            if (!await IsUserOnProject(userId, projectId))
            {
                try
                {
                    ProjectUser projectUser = new ProjectUser
                    {
                        UserId = userId,
                        ProjectId = projectId
                    };


                    await _context.AddAsync(projectUser);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("*********** An Error Occurred When Adding the User to the Project **************");
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        public async Task RemoveUserFromProject(string userId, int projectId)
        {
            if (await IsUserOnProject(userId, projectId))
            {
                try
                {
                    ProjectUser projectUser = await _context.ProjectUsers
                        .Where(pu => pu.UserId == userId && pu.ProjectId == projectId)
                        .FirstOrDefaultAsync();
                    _context.ProjectUsers.Remove(projectUser);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("*********** An Error Occurred When Removing the User from the Project **************");
                    Debug.WriteLine(ex.Message);
                }

            }
        }

        public async Task<IEnumerable<BTUser>> UsersNotOnProject(int projectId)
        {

            var output = new List<BTUser>();
            foreach (var user in await _context.Users.ToListAsync())
            {
                if (!await IsUserOnProject(user.Id, projectId))
                {
                    output.Add(user);
                }
            }
            return output;

            //return await _context.Users.Where(u => IsUserOnProject(u.Id, projectId) == false).ToListAsync();

            //Needs to be cleaned up to match OnProject

            //Project project = await _context.Projects.Include(p => p.ProjectUsers).ThenInclude(p => p.User).FirstOrDefaultAsync(p => p.Id == projectId);
            //List<BTUser> users = project.ProjectUsers.SelectMany(p => (IEnumerable<BTUser>)p.User).ToList();
            //return users;

        }

        public async Task<IEnumerable<BTUser>> UsersOnProject(int projectId)
        {
            var output = new List<BTUser>();
            foreach(var user in await _context.Users.ToListAsync())
            {
                if(await IsUserOnProject(user.Id, projectId))
                {
                    output.Add(user);
                }
            }
            return output;

        }

        public async Task<BTUser> ProjectManagerOnProject(int projectId)
        {
            var projectManagers = await _userManager.GetUsersInRoleAsync("ProjectManager");
            var onProject = await UsersOnProject(projectId);
            var projectManager = projectManagers.Intersect(onProject).FirstOrDefault();
            return projectManager;
        }
 
        public async Task<IEnumerable<BTUser>> DevelopersOnProject(int projectId)
        {
            var developers = await _userManager.GetUsersInRoleAsync("Developer");
            var onProject = await UsersOnProject(projectId);
            var devsOnProject = new List<BTUser>();
            devsOnProject.AddRange(developers.Intersect(onProject).ToList());
            return devsOnProject;
        }

        public async Task<IEnumerable<BTUser>> SubmittersOnProject(int projectId)
        {
            var submitters = await _userManager.GetUsersInRoleAsync("Submitter");
            var onProject = await UsersOnProject(projectId);
            var subsOnProject = new List<BTUser>();
            subsOnProject.AddRange(submitters.Intersect(onProject).ToList());
            return subsOnProject;
        }
    }
}
