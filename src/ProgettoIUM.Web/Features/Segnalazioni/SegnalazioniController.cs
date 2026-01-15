using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using ProgettoIUM.Web.Infrastructure;
using ProgettoIUM.Web.SignalR;
using ProgettoIUM.Web.SignalR.Hubs.Events;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ProgettoIUM.Services.Shared;

namespace ProgettoIUM.Web.Features.Segnalazioni
{
    [Authorize]
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

        [HttpGet]
        public virtual async Task<IActionResult> Edit(Guid? id)
        {
            var model = new EditViewModel();

            if (id.HasValue)
            {
                model.SetSegnalazioni(await _sharedService.Query(new SegnalazioniDetailQuery
                {
                    Id = id.Value,
                }));
            }



            return View(model);
        }


        [HttpPost]
        public virtual async Task<IActionResult> Edit(EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.Id = await _sharedService.Handle(model.ToAddOrUpdateSegnalazioneCommand());

                    Alerts.AddSuccess(this, "Informazioni aggiornate");

                   
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                }
            }

            if (ModelState.IsValid == false)
            {
                Alerts.AddError(this, "Errore in aggiornamento");
            }

            return RedirectToAction(Actions.Edit(model.Id));
        }

    }

       
}
