using System.Security.Claims;
using WebProject.Models;

namespace WebProject.ExternalServices.Extentions;

public static class AuthorizePermission
{
    public static bool HasAccess(this ClaimsPrincipal user, Pages page, PageAccess access = PageAccess.Read)
    {
        //byte page = byte.TryParse(depot, out byte p) ? p : (byte)Pages.AllDepos;

        string permissions = user.FindFirstValue("Permission" + ((byte)page).ToString()) ?? "0";
 
        bool isAllowed = ((int.TryParse(permissions, out int perm) ? perm : 0) & (int)access) == (int)access;
        
        return isAllowed;
    }
}
