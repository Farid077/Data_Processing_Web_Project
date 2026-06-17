using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using WebProject.Models;

namespace WebProject.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class AuthorizePermissionAttribute(Pages p, PageAccess acc = PageAccess.Read) : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        if (user.Identity?.IsAuthenticated != true)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        int access = (int)acc;
        byte page = (byte)p;

        string permissions = user.FindFirstValue("Permission" + page.ToString()) ?? "0";

        //bool isAllowed = permissions.Any(perm => (perm & page) == page && (perm & access) == access);
        bool isAllowed = ((int.TryParse(permissions, out int perm) ? perm : 0) & access) == access;

        if (!isAllowed)
            context.Result = new ForbidResult();
    }
}
