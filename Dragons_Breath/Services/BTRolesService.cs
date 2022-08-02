using Dragons_Breath.Data;
using Dragons_Breath.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dragons_Breath.Services
{
    public class BTRolesService : IBTRolesService
    {
        private readonly UserManager<BTUser> _userManager;
        private readonly ApplicationDbContext _context;

        public BTRolesService(
            UserManager<BTUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<bool> AddUserToRole(BTUser user, string roleName)
        {
            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded;
        }

        public async Task<bool> IsUserInRole(BTUser user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        public async Task<IEnumerable<string>> ListUserRoles(BTUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public IEnumerable<string> NonDemoRoles()
        {

            var roles = _context.Roles;
            var output = new List<string>();
            foreach(var role in roles)
            {
                if(role.Name != "Demo")
                {
                    output.Add(role.Name);
                }
            }
            return output;
        }

        public async Task<string> NonDemoUserRole(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);
            var output = "";
            foreach (var role in roles)
            {
                if (role != Roles.Demo.ToString())
                {
                    output = role;
                }
            }
            return output;
        }

        public async Task<bool> RemoveUserFromRole(BTUser user, string roleName)
        {
            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            return result.Succeeded;
        }

        public async Task<IEnumerable<BTUser>> UsersInRole(string roleName)
        {
            return await _userManager.GetUsersInRoleAsync(roleName);
        }

        public async Task<IEnumerable<BTUser>> UsersNotInRole(string roleName)
        {
            var inRole = await _userManager.GetUsersInRoleAsync(roleName);
            var users = await _userManager.Users.ToListAsync();
            return users.Except(inRole);
        }
    }
}