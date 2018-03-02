namespace BlockExplorer.Controllers
{
    using System.Diagnostics;
    using Microsoft.AspNetCore.Mvc;
    using BlockExplorer.Models;
    using BlockExplorer.Services.Interfaces;
    using System;

    public class HomeController : Controller
    {
        private readonly IHttpRequestService httpRequestService;

        public HomeController(IHttpRequestService httpRequestService)
        {
            this.httpRequestService = httpRequestService;
        }
        public IActionResult Index()
        {
            BlocksInfoVm model = this.httpRequestService.Get<BlocksInfoVm>("http://localhost:5555/info");
            Array.Reverse(model.Blocks);
            return View(model);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
