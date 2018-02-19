namespace Wallet.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Wallet.Models.BindingModels;

    public class BalanceController : Controller
    {
        [HttpGet]
        public IActionResult AccountBalance()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AccountBalance(AcountBalanceBm bm)
        {
            return Content($"{bm.Address} {bm.BlockChainNode}");
        }
    }
}