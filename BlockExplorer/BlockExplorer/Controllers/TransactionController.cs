namespace BlockExplorer.Controllers
{
    using BlockExplorer.Models;
    using BlockExplorer.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    public class TransactionController : Controller
    {
        private readonly IHttpRequestService httpRequestService;

        public TransactionController(IHttpRequestService httpRequestService)
        {
            this.httpRequestService = httpRequestService;
        }

        public IActionResult Index(int index)
        {
            Block block = this.httpRequestService.Get<Block>($"http://localhost:5555/blocks/{index}");
            var model = block.Transactions;

            return View(model);
        }


        public IActionResult Details([FromQuery]string tranHash)
        {
            var transaction = 
                this.httpRequestService.Get<Transaction>($"http://localhost:5555/transactions/{tranHash}/info");

            return View(transaction);
        }
    }
}