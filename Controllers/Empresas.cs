using Microsoft.AspNetCore.Mvc;

namespace UAMPass.Controllers
{
    public class EmpresasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
