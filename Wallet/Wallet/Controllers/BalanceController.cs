namespace Wallet.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using Wallet.Models.BindingModels;
    using Wallet.Models.ViewModels;

    public class BalanceController : Controller
    {
        [HttpGet]
        public IActionResult AccountBalance()
        {
            AcountBalanceBm bm = new AcountBalanceBm();
            return View(bm);
        }

        [HttpPost]
        public IActionResult AccountBalance(AcountBalanceBm bm)
        {
            bm.Info = bm.Address;
            return View(bm);
        }
    }
}