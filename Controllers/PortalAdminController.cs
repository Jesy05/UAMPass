using Microsoft.AspNetCore.Mvc;

namespace UAMPass.Controllers.Admin
{
    public class PortalAdminController : Controller
    {
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("AdminId") == null)
                return RedirectToAction("Login", "AdminAuth");

            return View();
        }
    }
}
