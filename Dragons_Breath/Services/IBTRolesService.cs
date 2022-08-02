using Dragons_Breath.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dragons_Breath.Services
{
    public interface IBTRolesService
    {
        public Task<bool> AddUserToRole(BTUser user, string roleName);

        public Task<bool> IsUserInRole(BTUser user, string roleName);

        public Task<IEnumerable<string>> ListUserRoles(BTUser user);

        public Task<bool> RemoveUserFromRole(BTUser user, string roleName);

        public Task<IEnumerable<BTUser>> UsersInRole(string roleName);

        public Task<IEnumerable<BTUser>> UsersNotInRole(string roleName);

        public IEnumerable<string> NonDemoRoles();
    }
}
