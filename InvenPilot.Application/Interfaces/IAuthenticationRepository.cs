using InvenPilot.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Interfaces
{
    public interface IAuthenticationRepository
    {
        public Task<IdentityResult> RegisterAsync(ApplicationUser user , string password);
        public Task<bool> CheckPasswordAsync(ApplicationUser user , string password);
        public Task<ApplicationUser> GetUserByEmailAsync(string email);
        public Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);
    }
}
