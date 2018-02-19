namespace Wallet.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Wallet.Models.ViewModels;
    using Wallet.Services.Interfaces;

    public class WalletController : Controller
    {
        private readonly ITransactionsService service;

        public WalletController(ITransactionsService service)
        {
            this.service = service;
        }

        public IActionResult CreateNewWallet()
        {
            CreateNewWalletVm model = new CreateNewWalletVm
            {
                PrivateKey = "",
                PublicKey = "",
                Address = "",
                Info = ""
            };
            return View(model);
        }

        public IActionResult Create()
        {           
            CreateNewWalletVm model = this.service.RandomPrivateKeyToAddress();
            //todo write data to storage
            return View("CreateNewWallet", model);
        }
    }
}