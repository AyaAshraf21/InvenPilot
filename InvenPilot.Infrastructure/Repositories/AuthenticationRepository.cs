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
            return await userManager.CreateAsync(user, password);
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser user , string password)
        {
            return await userManager.CheckPasswordAsync(user , password);
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            return await userManager.FindByEmailAsync(email);
        }
    }
}
