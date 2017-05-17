using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SMAS.Data;
using SMAS.Entities;

namespace SMAS.Services.Identity
{
    public class UserService : ServiceBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        
        public UserService(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager, IDataContextFactory dataContextFactory) : base(dataContextFactory)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IReadOnlyCollection<User>> GetUsers()
        {
            return await _userManager.Users.ToListAsync();

            //using (var dc = DataContext())
            //{
            //    return await ((IdentityDbContext<User>) dc).Users
            //        .ToListAsync();
            //}
        }

        public T GetUsers<T>(string search, string role, Func<IQueryable<User>, T> processQueryable)
        {
            using (var dc = DataContext())
            {
                var users = ((IdentityDbContext<User, IdentityRole<int>, int>)dc).Users.AsQueryable();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    users = users.Where(i =>
                        i.FirstName.Contains(search) ||
                        i.LastName.Contains(search) ||
                        i.Email.Contains(search));
                }

                if (!string.IsNullOrEmpty(role))
                {
                    var identityRole = _roleManager.Roles.SingleOrDefault(i => i.Name == role);
                    if (identityRole == null)
                    {
                        throw new ArgumentException("Cannot find role '" + role + "'.");
                    }

                    users = users.Where(i => i.Roles.Select(r => r.RoleId).Contains(identityRole.Id));
                }

                return processQueryable(users);
            }
        }

        public async Task<User> GetUserById(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<User> GetUserByUserName(string userName)
        {
            return await _userManager.FindByEmailAsync(userName);
        }

        public async Task<IReadOnlyCollection<IdentityRole<int>>> GetRoles()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task<IReadOnlyCollection<string>> GetRolesForUser(User user)
        {
            return (await _userManager.GetRolesAsync(user)).ToList();
        }

        public async Task<IdentityResult> CreateUser(User user, string password)
        {
            user.UserName = user.UserName ?? user.Email;
            user.CreatedUtc = DateTime.UtcNow;

            return await _userManager.CreateAsync(user, password);
        }

        public async Task SetRoles(User user, IEnumerable<string> roles)
        {
            var allRoles = await _roleManager.Roles.ToListAsync();
            var unassignedRoles = allRoles.Where(i => !roles.Contains(i.Name)).Select(i => i.Name);

            foreach (var unassignedRole in unassignedRoles)
            {
                await _userManager.RemoveFromRoleAsync(user, unassignedRole);
            }

            foreach (var role in roles)
            {
                await _userManager.AddToRoleAsync(user, role);
            }

            // TODO: This doesn't seem to work
            //await _userManager.RemoveFromRolesAsync(user, unassignedRoles);
            //await _userManager.AddToRolesAsync(user, roles);
        }

        public async Task SaveUser(User user)
        {
            await _userManager.UpdateAsync(user);
        }
    }
}
