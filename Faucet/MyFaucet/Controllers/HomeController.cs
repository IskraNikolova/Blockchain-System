namespace MyFaucet.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using Microsoft.AspNetCore.Mvc;
    using MyFaucet.Models;
    using Newtonsoft.Json.Linq;
    using Microsoft.AspNetCore.Http;
    using MyFaucet.Services.Interfaces;

    public class HomeController : Controller
    {
        private readonly ITransactionsService transactionService;
        private readonly IHttpRequestService httpRequestService;

        public HomeController(ITransactionsService transactionService,
                                      IHttpRequestService httpRequestService)
        {
            this.transactionService = transactionService;
            this.httpRequestService = httpRequestService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FormSubmit(TransactionModel model)
        {
            //Validate google recaptcha here

            var status = GetStatusFromCaptcha();
            ViewBag.Message = status ? "Google reCaptcha validation success" : "Google reCaptcha validation failed";

            if (ModelState.IsValid && status)
            {
                var address = model.Address;
                var url = model.Url;
                DateTime date = new DateTime();
                string dateCreated = (date).ToString("o");
                string faucetPrivateKey = "7e4670ae70c98d24f3662c172dc510a085578b9ccc717e6c2f4e547edd960a34";

                //Create signature for request
                SendTransactionBody bodyData = this.transactionService.CreateAndSignTransaction(address, 1,
                                     0, dateCreated, faucetPrivateKey);

                ResponseModel responseTransaction = this.httpRequestService.Post($"{url}/transactions/send", bodyData);
                responseTransaction.Address = address;

                return View("SuccessTransaction", responseTransaction);
            }

            return View("Index", model);
        }

        public IActionResult SuccessTransaction(ResponseModel vm)
        {
            return View(vm);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private bool GetStatusFromCaptcha()
        {
            var response = Request.Form["g-recaptcha-response"];
            string secretKey = "6LfNnEkUAAAAAMylPxH87SmX9lNTxmf9XG0PfJh4";
            var client = new WebClient();
            var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));
            var obj = JObject.Parse(result);
            var status = (bool)obj.SelectToken("success");

            return status;
        }
    }
}
