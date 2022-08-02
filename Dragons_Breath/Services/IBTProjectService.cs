using Dragons_Breath.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dragons_Breath.Services
{
    public interface IBTProjectService
    {
        public Task<bool> IsUserOnProject(string userId, int projectId);

        public Task<IEnumerable<Project>> ListUserProjects(string userId);

        public Task AddUserToProject(string userId, int projectId);

        public Task RemoveUserFromProject(string userId, int projectId);

        public Task<IEnumerable<BTUser>> UsersOnProject(int projectId);

        public Task<IEnumerable<BTUser>> UsersNotOnProject(int projectId);

        public Task<IEnumerable<BTUser>> DevelopersOnProject(int projectId);
        
        public Task<IEnumerable<BTUser>> SubmittersOnProject(int projectId);

        public Task<BTUser> ProjectManagerOnProject(int projectId);

    }
}
