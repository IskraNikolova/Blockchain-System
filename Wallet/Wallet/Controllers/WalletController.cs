namespace Wallet.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Wallet.Models.ViewModels;
    using Wallet.Services.Interfaces;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;
    using System.Threading.Tasks;
    using System;

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

            //Set serializable objects to Session
            HttpContext.Session.SetString(model.Address, JsonConvert.SerializeObject(model));
            ViewData["Login"] = "Yes";
            return View("CreateNewWallet", model);
        }

        public IActionResult OpenExistingWallet()
        {
            var model = new OpenExistingWalletVm
            {
                PrivateKey = "",
                PublicKey = "",
                Address = "",
                Info = ""
            };

            return View(model);
        }

        public IActionResult Open(string privateKey)
        {
            OpenExistingWalletVm model = 
                 this.service.ExistingPrivateKeyToAddress(privateKey);

            //Set serializable objects to Session
            HttpContext.Session.SetString(model.Address, JsonConvert.SerializeObject(model));
            ViewData["Login"] = "Yes";
            return View("OpenExistingWallet", model);
        }
    }
}