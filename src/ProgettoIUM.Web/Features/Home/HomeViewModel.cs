using Microsoft.AspNetCore.Mvc;
using ProgettoIUM.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ProgettoIUM.Web.Features.Home
{
    [TypeScriptModule("Features.Home.Server")]
    public class HomeViewModel
    {

        public string CodiceUnivoco { get; set; }
        public string ErrorMessage { get; set; }
    }
}
