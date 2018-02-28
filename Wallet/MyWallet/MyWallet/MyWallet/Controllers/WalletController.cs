namespace Wallet.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Wallet.Models.ViewModels;
    using Wallet.Services.Interfaces;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;

    public class WalletController : Controller
    {
        private readonly ITransactionsService service;

        public WalletController(ITransactionsService service)
        {
            this.service = service;
        }

        public IActionResult CreateNewWallet()
        {
            CreateNewWalletVm model = new CreateNewWalletVm();
            return View(model);
        }

        public IActionResult Create()
        {           
            CreateNewWalletVm model = this.service.RandomPrivateKeyToAddress();
            //Set serializable objects to Session
            HttpContext.Session.SetString(model.Address, JsonConvert.SerializeObject(model)); 
            return View("CreateNewWallet", model);
        }

        public IActionResult OpenExistingWallet()
        {
            var model = new OpenExistingWalletVm();
            return View(model);
        }

        public IActionResult Open(string privateKey)
        {
            OpenExistingWalletVm model = 
                 this.service.ExistingPrivateKeyToAddress(privateKey);

            //Set serializable objects to Session
            HttpContext.Session.SetString(model.Address, JsonConvert.SerializeObject(model));
            return View("OpenExistingWallet", model);
        }
    }
}