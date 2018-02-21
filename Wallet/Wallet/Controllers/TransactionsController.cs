namespace Wallet.Controllers
{
    using System;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using Wallet.Models.ViewModels;
    using Wallet.Services.Interfaces;
    using System.IO;
    using Newtonsoft.Json.Linq;
    using System.Text;

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
            SignTransactionVm model = new SignTransactionVm();

            return View(model);
        }


        [HttpPost]
        public IActionResult SendTransaction(string info, string url)
        {
            var model = JsonConvert.DeserializeObject<SignTransactionVm>(info);       
            
            //Take private key from session
            var obj = HttpContext.Session.GetString(model.From);
            var jsonModel = JsonConvert.DeserializeObject<OpenExistingWalletVm>(obj);
            var privateKey = jsonModel.PrivateKey;

            string dateCreated = (model.DateCreated).ToString("o");

            //Create signature for request
            SendTransactionBody bodyData = this.transactionService.CreateAndSignTransaction(model.To, model.Value, 
                                                                         model.Fee, dateCreated, privateKey);
            //Post to node and take responce
            //var response = this.httpRequestService.Pots<ResponseSentTransactionVm>(url, bodyData);
            JObject dataObject = JObject.FromObject(new
            {
                from = bodyData.From,
                to = bodyData.To,
                value = bodyData.Value,
                fee = bodyData.Fee,
                dateCreated = bodyData.DateCreated,
                senderPubKey = bodyData.SenderPubKey,
                senderSignature = bodyData.SenderSignature
            });

            byte[] data1 = Encoding.ASCII.GetBytes(dataObject.ToString());
            var response = HttpServices.PostRequest.Pots<ResponseSentTransactionVm>(url, data1);
            //Populate model with info and sent to view
            model.Info = info;
            model.Response = $"{response.Message}\nTransaction Hash: {response.TransactionHash}";
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
                DateCreated = DateTime.UtcNow,
                SenderPubKey = value.PublicKey,
                Info = "",
                Response = model.Response == "" ? "" : model.Response
            };

            string info = JsonConvert.SerializeObject(result);
            result.Info = info.Substring(0, info.LastIndexOf(','));
            result.Info = result.Info.Substring(0, result.Info.LastIndexOf(',')) + "}";

            return result;
        }
    }
}