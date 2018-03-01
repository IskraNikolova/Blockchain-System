namespace Wallet.Controllers
{
    using System;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
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

        [HttpGet]
        public IActionResult SendTransaction()
        {
            TransactionModel model = new TransactionModel();

            return View(model);
        }

        public IActionResult SignTransaction(TransactionModel model)
        {
            var result = model;
            if (ModelState.IsValid)
            {
                try
                {
                    result = this.GetModel(model);
                }
                catch
                {
                    TempData["errorMessage"] = "Invalid Sender Address!";
                }

            }

            return View("SendTransaction", result);
        }

        [HttpPost]
        public IActionResult SendTransaction(string info, string url)
        {
            var model = JsonConvert.DeserializeObject<TransactionModel>(info);

            //Take private key from session
            var obj = HttpContext.Session.GetString(model.From);
            var jsonModel = JsonConvert.DeserializeObject<OpenExistingWalletVm>(obj);
            var privateKey = jsonModel.PrivateKey;
            var address = jsonModel.Address;

            string resUrl = $"http://localhost:5555/balance/{address}/confirmations/6";
            AcountBalanceVm responce = this.httpRequestService.Get<AcountBalanceVm>(resUrl);

            var balance = responce.ConfirmedBalance.Balance;
            if(balance < model.Value)
            {
                TempData["errorMessage"] = "Insufficient balance!!!";
                return View("SendTransaction", model);
            }

            string dateCreated = (model.DateCreated).ToString("o");

            //Create signature for request
            SendTransactionBody bodyData = this.transactionService.CreateAndSignTransaction(model.To, model.Value,
                                                                            model.Fee, dateCreated, privateKey);
            //Post to node and take responce
            var response = this.httpRequestService.Post(url, bodyData);

            //Populate model with info and sent to view
            model.Info = info;
            model.Response = $"{response.Message}\nTransaction Hash: {response.TransactionHash}";
            var result = this.GetModel(model);

            return View("SendTransaction", result);
        }

        private TransactionModel GetModel(TransactionModel model)
        {
            var obj = HttpContext.Session.GetString(model.From);
            var value = JsonConvert.DeserializeObject<OpenExistingWalletVm>(obj);

            var  result = new TransactionModel
            {
                From = model.From,
                To = model.To,
                Value = model.Value,
                Fee = 10,
                DateCreated = DateTime.UtcNow,
                SenderPubKey = value.PublicKey,
                Info = "",
                Response = model.Response == "" ? "" : model.Response
            };

            string info = JsonConvert.SerializeObject(result);
            result.Info = info.Substring(0, info.LastIndexOf(','));
            result.Info = result.Info.Substring(0, result.Info.LastIndexOf(',')) + "}";
            
            return result;
        }//todo
    }
}