using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace USF_Health_MVC_EF.Controllers
{
    public class LoginController : Controller
    {

        public IActionResult Index()
        {

            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();


    }

    }
}
