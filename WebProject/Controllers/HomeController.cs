using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebProject.Attributes;
using WebProject.Models;
using WebProject.ViewModels;

namespace WebProject.Controllers;

//[AuthorizePermission((int)Pages.Dashboard, (int)PageAccess.Read)]
[Authorize]
public class HomeController : Controller
{
    [Authorize]
    public async Task<IActionResult> Index()
    {
        return View();
    }


    [Authorize]
    public IActionResult DepotData(int depot)
    {
        var permissions = User.FindAll("Permission").Select(perm => int.Parse(perm.Value));

        var page = Enum.GetValues<Pages>().FirstOrDefault(x => x.ToString() == "Depo" + depot);
        bool isAllowed = permissions.Any(perm => (perm & (int)page) == (int)page && (perm & (int)PageAccess.Read) == (int)PageAccess.Read);
        if (!isAllowed)
            return new ForbidResult();

        return View(depot);
    }
    

    [AuthorizePermission((int)Pages.AllDepos, (int)PageAccess.Read)]
    public IActionResult AllDepotsData() => View();



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(string errorMessage = "Error!")
    {
        return View(new ErrorVM { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, ErrorMessage = errorMessage ?? "Error!" });
    }
}
