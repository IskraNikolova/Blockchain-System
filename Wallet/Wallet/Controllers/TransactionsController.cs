namespace Wallet.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using System;
    using Wallet.Models.ViewModels;
    using Wallet.Services.Interfaces;

    public class TransactionsController : Controller
    {
        private readonly ITransactionsService service;

        public TransactionsController(ITransactionsService service)
        {
            this.service = service;
        }

        public IActionResult SendTransaction()
        {
            SignTransactionVm model = new SignTransactionVm();

            return View(model);
        }

        [HttpPost]
        public IActionResult SendTransaction(string info, string url)
        {
            var model = JsonConvert.DeserializeObject<SignTransactionVm>(info);
            model.Info = info;
            var result = this.GetModel(model);
            return View("SendTransaction", result);
        }

        public IActionResult SignTransaction(SignTransactionVm model)
        {
            var result = this.GetModel(model);
            return View("SendTransaction", result);
        }

        private SignTransactionVm GetModel(SignTransactionVm model)
        {
            var obj = HttpContext.Session.GetString(model.From);
            var value = JsonConvert.DeserializeObject<OpenExistingWalletVm>(obj);

            SignTransactionVm result = new SignTransactionVm
            {
                From = model.From,
                To = model.To,
                Value = model.Value,
                Fee = 10,
                DateCreated = DateTime.Now,//todo ISO
                SenderPubKey = value.PublicKey,
                Info = ""
            };

            string info = JsonConvert.SerializeObject(result);
            result.Info = info.Substring(0, info.LastIndexOf(',')) + "}";

            return result;
        }
    }
}