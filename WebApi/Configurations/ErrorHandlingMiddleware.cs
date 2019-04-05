using BLL.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace WebApi.Configurations
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Status 500 if unexpected exception.
            var code = HttpStatusCode.InternalServerError;

            if (exception is InvalidPasswordException) code = HttpStatusCode.BadRequest;
            else if (exception is ManagerNotFoundException) code = HttpStatusCode.NotFound;
            else if (exception is WorkerNotFoundException) code = HttpStatusCode.NotFound;
            else if (exception is PersonNotFoundException) code = HttpStatusCode.NotFound;
            else if (exception is PriorityNotFoundException) code = HttpStatusCode.BadRequest;
            else if (exception is RefreshTokenNotFoundException) code = HttpStatusCode.NotFound;
            else if (exception is RoleException) code = HttpStatusCode.BadRequest;
            else if (exception is StatusNotFoundException) code = HttpStatusCode.NotFound;
            else if (exception is StatuskAccessException) code = HttpStatusCode.BadRequest;
            else if (exception is TaskAccessException) code = HttpStatusCode.BadRequest;
            else if (exception is TaskNotFoundException) code = HttpStatusCode.NotFound;
            else if (exception is TeamExistsException) code = HttpStatusCode.BadRequest;
            else if (exception is TeamNotFoundException) code = HttpStatusCode.NotFound;
            else if (exception is UserNotFoundException) code = HttpStatusCode.NotFound;

            var result = JsonConvert.SerializeObject(new { error = exception.Message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
    }
}
