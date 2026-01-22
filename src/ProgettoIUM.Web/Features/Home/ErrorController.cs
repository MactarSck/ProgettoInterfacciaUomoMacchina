using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using ProgettoIUM.Services;
using ProgettoIUM.Services.Shared;
using ProgettoIUM.Services.Shared.Segnalazione;
using ProgettoIUM.Web.Features.Segnalazioni;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProgettoIUM.Web.Features.Home
{
    public partial class ErrorController : Controller
    {

        [Route("Error/{statusCode}")]
        public virtual IActionResult HttpStatusCodeHandler(int statusCode)
        {
            if (statusCode == 404)
            {
                ViewBag.ErrorMessage = "Spiacenti, la pagina richiesta non è stata trovata.";
            }


            return View("NotFound");
        }

        [Route("Home/NotFound")]
        public virtual IActionResult NotFound()
        {
            return View();
        }
    }
}
