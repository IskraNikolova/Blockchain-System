namespace BlockExplorer.Controllers
{
    using BlockExplorer.Models;
    using BlockExplorer.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    public class BlockController : Controller
    {
        private readonly IHttpRequestService httpRequestService;

        public BlockController(IHttpRequestService httpRequestService)
        {
            this.httpRequestService = httpRequestService;
        }

        public IActionResult Index(int index)
        {
            Block block = this.httpRequestService.Get<Block>($"http://localhost:5555/blocks/{index}");
            return View(block);
        }
    }
}