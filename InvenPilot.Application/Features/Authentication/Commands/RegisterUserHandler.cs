using InvenPilot.Application.Features.Authentication.DTO;
using InvenPilot.Application.Interfaces;
using InvenPilot.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Authentication.Commands
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand>
    {
        private readonly IAuthenticationRepository authenticationRepository;

        public RegisterUserHandler(IAuthenticationRepository authenticationRepository)
        {
            this.authenticationRepository = authenticationRepository;
        }

        public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = new ApplicationUser
            {
                UserName = request.registerDto.Name,
                Email = request.registerDto.Email
            };
            IdentityResult identityResult =  await authenticationRepository.RegisterAsync(user, request.registerDto.Password);
            if (!identityResult.Succeeded)
            {
                throw new Exception(identityResult.Errors.First().Description);
            }
        }
    }
}
