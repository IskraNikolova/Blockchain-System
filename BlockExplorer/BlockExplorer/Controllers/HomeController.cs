namespace BlockExplorer.Controllers
{
    using System.Diagnostics;
    using Microsoft.AspNetCore.Mvc;
    using BlockExplorer.Models;

    public class HomeController : Controller
    {
        public IActionResult Index(BlocksInfoVm model)
        {
            return View(model);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
