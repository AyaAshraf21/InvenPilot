using InvenPilot.Application.Features.Authentication.Commands;
using InvenPilot.Application.Interfaces;
using InvenPilot.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly InvenPilotContext context;

        public UnitOfWork(InvenPilotContext context)
        {
            this.context = context;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}
