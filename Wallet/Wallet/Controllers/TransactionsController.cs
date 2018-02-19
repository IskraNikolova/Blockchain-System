namespace Wallet.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Wallet.Models.BindingModels;
    using Wallet.Services.Interfaces;

    public class TransactionsController : Controller
    {
        private readonly ITransactionsService service;

        public TransactionsController(ITransactionsService service)
        {
            this.service = service;
        }

        [HttpGet]
        public IActionResult SignTransaction()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendTransaction(SendTransactionBm bm)
        {
            return PartialView("_SendTransaction", bm);
        }
    }
}