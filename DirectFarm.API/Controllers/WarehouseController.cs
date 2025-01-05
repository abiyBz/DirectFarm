using Microsoft.AspNetCore.Mvc;

namespace DirectFarm.API.Controllers
{
    public class WarehouseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
