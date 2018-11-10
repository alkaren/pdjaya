using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PDJaya.Service.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class DefaultController : Controller
    {
        [Route("/")]
        //[Route("/docs")]
        //[Route("/swagger")]
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }
    }
}