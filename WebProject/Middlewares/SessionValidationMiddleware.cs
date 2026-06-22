using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using WebProject.ExternalServices.Interfaces;

namespace WebProject.Middlewares;

public class SessionValidationMiddleware(RequestDelegate _next)
{
    public async Task InvokeAsync(HttpContext context, ISessionService sessionService)
    {
        if(context.User.Identity?.IsAuthenticated == true)
        {
            string userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception("İstifadəçi adı Claims tapılmadı.");
            
            string sessionToken = context.User.FindFirstValue("SessionToken") ?? throw new Exception($"Bu ID-yə malik istifadəçi üçün SessionToken tapılmadı: {userId}");

            if(!await sessionService.IsValidAsync(userId, sessionToken))
            {
                if (context.Response.HasStarted)
                {
                    await _next(context);
                    return;
                }
                await context.SignOutAsync();
                context.Response.Redirect("/Auth/Login");
                return;
            }
        }

        await _next(context);
    }

}
