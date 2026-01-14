using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using ProgettoIUM.Web.Infrastructure;
using ProgettoIUM.Web.SignalR;
using ProgettoIUM.Web.SignalR.Hubs.Events;
using System;
using System.Threading.Tasks;
using ProgettoIUM.Services.Shared;

namespace ProgettoIUM.Web.Features.Segnalazioni
{
    public partial class SegnalazioniController : Controller
    {
        private readonly SharedService _sharedService;
        private readonly IPublishDomainEvents _publisher;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;

        public SegnalazioniController(SharedService sharedService, IPublishDomainEvents publisher, IStringLocalizer<SharedResource> sharedLocalizer)
        {
            _sharedService = sharedService;
            _publisher = publisher;
            _sharedLocalizer = sharedLocalizer;

            ModelUnbinderHelpers.ModelUnbinders.Add(typeof(IndexViewModel), new SimplePropertyModelUnbinder());
        }

        [HttpGet]
        public virtual async Task<IActionResult> Index(IndexViewModel model)
        {
            var s = await _sharedService.Query(model.ToSegnalazioniIndexQuery());
            model.SetSegnalazioni(s);

            return View(model);
        }

    }

       
}
