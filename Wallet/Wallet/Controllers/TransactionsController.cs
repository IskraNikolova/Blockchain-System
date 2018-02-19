namespace Wallet.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using System;
    using System.Threading.Tasks;
    using Wallet.Models.ViewModels;
    using Wallet.Services.Interfaces;

    public class TransactionsController : Controller
    {
        private readonly ITransactionsService transactionService;
        private readonly IHttpRequestService httpRequestService;

        public TransactionsController(ITransactionsService transactionService, 
                                      IHttpRequestService httpRequestService)
        {
            this.transactionService = transactionService;
            this.httpRequestService = httpRequestService;
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
            string to = model.To;
            string dateCreated = (model.DateCreated).ToString("o");
            int value = model.Value;
            int fee = model.Fee;
            var obj = HttpContext.Session.GetString(model.From);
            var jsonModel = JsonConvert.DeserializeObject<OpenExistingWalletVm>(obj);
            string jsonResult = this.transactionService.CreateAndSignTransaction(to,
                value,
                fee,
                dateCreated,
                jsonModel.PrivateKey);

            var response = this.httpRequestService.Pots<ResponseSentTransactionVm>(url, jsonResult);
            return Content(response.Message);
            //return Content(jsonResult);TODO SEND REQUEST!!!
            //model.Info = info;
            //var result = this.GetModel(model);
            //return View("SendTransaction", result);
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
                DateCreated = DateTime.UtcNow,
                SenderPubKey = value.PublicKey,
                Info = ""
            };

            string info = JsonConvert.SerializeObject(result);
            result.Info = info.Substring(0, info.LastIndexOf(',')) + "}";

            return result;
        }
    }
}