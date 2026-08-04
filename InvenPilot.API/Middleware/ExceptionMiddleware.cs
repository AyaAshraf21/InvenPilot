using FluentValidation;
using InvenPilot.API.Models;
using InvenPilot.Application.Exceptions;
using System.Security.Authentication;

namespace InvenPilot.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            try
            {
                await next(context);
            }
            catch(ValidationException ex)
            {
                var response = new ErrorResponse
                {
                    StatusCode = 400,
                    Message = "Validation failed.",
                    Errors = ex.Errors
                       .GroupBy(x => x.PropertyName)
                       .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToList())
                };

                await context.Response.WriteAsJsonAsync(response);
            }
            catch(EmailAlreadyExistsException ex)
            {
                var response = new ErrorResponse
                {
                    StatusCode = 409,
                    Message = ex.Message
                };
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(response);
            }
            catch(InvalidCredentialsException ex)
            {
                var response = new ErrorResponse
                {
                    StatusCode = 401,
                    Message = ex.Message
                };
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(response);
            }
            catch(NotFoundException ex)
            {
                var response = new ErrorResponse
                {
                    StatusCode = 404,
                    Message = ex.Message
                };
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsJsonAsync(response);
            }

            catch (Exception ex)
            {
                var response = new ErrorResponse
                {
                    StatusCode = 500,
                    Message = "An unexpected error occurred."
                };
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
