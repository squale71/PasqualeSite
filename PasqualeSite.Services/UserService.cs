using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Reflection;
using PasqualeSite.Data.Identity;

namespace PasqualeSite.Services
{
    public class UserService : DisposableService
    {
        public async Task<AppUser> GetUser(string userId)
        {
            var user = await db.Users.AsNoTracking().Where(x => x.Id == userId).FirstOrDefaultAsync();
            return user;
        }

        public async Task<List<AppUser>> GetAllUsers()
        {
            var users = await db.Users.Include(x => x.Roles).ToListAsync();
            return users;
        }

        public string GetRoleName(IdentityUserRole role)
        {
            var userRole = db.Roles.Where(x => x.Id == role.RoleId).FirstOrDefault();
            return userRole.Name;
        }

        public async Task<string> AddRoleToUser(string userId, string roleName)
        {
            var userManager = new UserManager<AppUser>(new UserStore<AppUser>(db));
            await userManager.AddToRoleAsync(userId, roleName);
            return userId;
        }

        public async Task<string> RemoveRoleFromUser(string userId, string roleName)
        {
            var userManager = new UserManager<AppUser>(new UserStore<AppUser>(db));
            await userManager.RemoveFromRoleAsync(userId, roleName);
            return userId;
        }

        public async Task<bool> DeactivateUser(string id)
        {
            var userManager = new UserManager<AppUser>(new UserStore<AppUser>(db));
            await userManager.RemoveFromRoleAsync(id, "Active");

            await db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateUser(AppUser user)
        {
            var userToUpdate = await db.Users.Where(x => x.Id == user.Id).FirstOrDefaultAsync();
            if (userToUpdate != null)
            {
                db.Entry(userToUpdate).CurrentValues.SetValues(user);
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<AppUser> UpdateUserFields(AppUser user)
        {
            var currentUser = await db.Users.Where(x => x.Id == user.Id).FirstOrDefaultAsync();
            currentUser.FirstName = user.FirstName;
            currentUser.LastName = user.LastName;
            currentUser.Email = user.Email;
            if (!String.IsNullOrEmpty(user.PasswordHash))
                currentUser.PasswordHash = user.PasswordHash;
            await db.SaveChangesAsync();
            return user;
        }

        public async Task<bool> CheckIfEmailExists(string userId, string email)
        {
            return await db.Users.Where(x => x.Email.Replace("\r\n", "").Trim().ToLower() == email.Replace("\r\n", "").Trim().ToLower() && x.Id != userId).AnyAsync();
        }
    }
}