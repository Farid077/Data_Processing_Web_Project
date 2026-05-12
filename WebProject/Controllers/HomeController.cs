using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebProject.Attributes;
using WebProject.DataAccess;
using WebProject.Models;
using WebProject.ViewModels;

namespace WebProject.Controllers;

//[AuthorizePermission((int)Pages.Dashboard, (int)PageAccess.Read)]
[Authorize]
public class HomeController : Controller
{
    //[AuthorizePermission((int)Pages.Dashboard, (int)PageAccess.Read)]
    public async Task<IActionResult> Index()
    {
        return View();
    }

    public IActionResult Products() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(string errorMessage = "Error!")
    {
        return View(new ErrorVM { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, ErrorMessage = errorMessage ?? "Error!" });
    }
}
