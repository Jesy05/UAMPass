using Microsoft.AspNetCore.Mvc;

namespace UAMPass.Controllers
{
    public class AdministracionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
