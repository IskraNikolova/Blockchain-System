namespace Wallet.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Logout()
        {
            //todo
            return this.RedirectToAction("Index", "Home");
        }
    }
}
