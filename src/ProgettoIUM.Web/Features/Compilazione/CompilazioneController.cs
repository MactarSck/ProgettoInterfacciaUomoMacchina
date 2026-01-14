using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using ProgettoIUM.Services;
using ProgettoIUM.Services.Shared;
using ProgettoIUM.Web.Features.Compilazione;
using System;



namespace ProgettoIUM.Web.Features.Compilazione
{
    public partial class CompilazioneController :Controller
    {
        private readonly SharedService _sharedService;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;
        private readonly ProgettoIUMDbContext _dbContext;
        public CompilazioneController(ProgettoIUMDbContext context)
        {
            _dbContext = context;
        }


        [HttpGet]
        public virtual IActionResult NuovaSegnalazione()
        {
            var model = new CompilazioneViewModel();
            return View("~/Features/Compilazione/Index.cshtml", model);
        }
    }
}
