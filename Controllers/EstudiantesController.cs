using Microsoft.AspNetCore.Mvc;

namespace UAMPass.Controllers
{
    public class EstudiantesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
