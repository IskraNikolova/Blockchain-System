namespace Wallet.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Wallet.Models.BindingModels;
    using Wallet.Services.Interfaces;

    public class BalanceController : Controller
    {
        private readonly IHttpRequestService httpRequestService;

        public BalanceController(IHttpRequestService httpRequestService)
        {
            this.httpRequestService = httpRequestService;
        }

        [HttpGet]
        public IActionResult AccountBalance()
        {
            AcountBalanceBm bm = new AcountBalanceBm();
            return View(bm);
        }

        [HttpPost]
        public IActionResult AccountBalance(AcountBalanceBm bm)
        {
            var address = bm.Address;
            var blockChainNode = bm.BlockChainNode;
            string resUrl = $"{blockChainNode}/balance/{address}/confirmations/6";
            var responce = this.httpRequestService.Get<object>(resUrl);
             bm.Info = responce + "";
             return View(bm);
        }
    }
}