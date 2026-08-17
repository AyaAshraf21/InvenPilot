using Azure.Core;
using InvenPilot.Application.Interfaces;
using InvenPilot.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Infrastructure.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly UserManager<ApplicationUser> userManager;

        public AuthenticationRepository(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IdentityResult> RegisterAsync(ApplicationUser user, string password)
        {
            var result = await userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                return result;
            }

            if (user.Email == "admin@invenpilot.com" &&
                password == "Admin@123")
            {
                await userManager.AddToRoleAsync(user, "Admin");
            }
            else
            {
                await userManager.AddToRoleAsync(user, "Employee");
            }

            return result;
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser user , string password)
        {
            return await userManager.CheckPasswordAsync(user , password);
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            return await userManager.FindByEmailAsync(email);
        }

        public async Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role)
        {
            return await userManager.AddToRoleAsync(user, role);
        }
    }
}
