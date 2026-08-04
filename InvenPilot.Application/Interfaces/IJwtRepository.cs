using InvenPilot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Interfaces
{
    public interface IJwtRepository
    {
        public Task<string> GenerateToken(ApplicationUser user);
    }
}
