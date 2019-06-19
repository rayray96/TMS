using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System.Linq;

namespace WebApi.Filters
{
    public class UserValidationActionFilterAttribute : ActionFilterAttribute
    {
        // This filter have to use with actions where optional "Id" is UserId of the current User in a system.
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var requestUserId = context.ActionArguments["id"];
            if (requestUserId != null)
            {
                var contextUserId = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                if (contextUserId != null)
                {
                    if (requestUserId as string != contextUserId)
                    {
                        Log.Error($"Request UserId: {requestUserId} is not matching with Context UserId: {contextUserId}");
                        context.Result = new BadRequestObjectResult(requestUserId);
                    }
                }
            }
        }
    }
}
