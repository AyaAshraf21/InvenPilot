using InvenPilot.Application.Exceptions;
using InvenPilot.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenPilot.Application.Features.Authentication.Commands
{
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, string>
    {
        private readonly IAuthenticationRepository authenticationRepository;
        private readonly IJwtRepository jwtRepository;

        public LoginUserHandler(IAuthenticationRepository authenticationRepository, IJwtRepository jwtRepository)
        {
            this.authenticationRepository = authenticationRepository;
            this.jwtRepository = jwtRepository;
        }

        public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user =  await authenticationRepository.GetUserByEmailAsync(request.loginDTO.Email);
            if (user == null)
            {
                throw new InvalidCredentialsException();
            }
            bool check = await authenticationRepository.CheckPasswordAsync(user, request.loginDTO.Password);
            if (!check)
            {
                throw new InvalidCredentialsException();
            }
            return await jwtRepository.GenerateToken(user);
        }
    }
}
