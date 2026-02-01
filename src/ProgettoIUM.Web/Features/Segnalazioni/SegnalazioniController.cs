using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using ProgettoIUM.Services;
using ProgettoIUM.Services.Shared;
using ProgettoIUM.Web.Features.Compilazione;
using ProgettoIUM.Web.Infrastructure;
using ProgettoIUM.Web.SignalR;
using ProgettoIUM.Web.SignalR.Hubs.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgettoIUM.Web.Features.Segnalazioni
{
    [Authorize]
    public partial class SegnalazioniController : Controller
    {
        private readonly SharedService _sharedService;
        private readonly IPublishDomainEvents _publisher;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;
        private readonly ProgettoIUMDbContext _dbContext;

        public SegnalazioniController(SharedService sharedService, IPublishDomainEvents publisher, IStringLocalizer<SharedResource> sharedLocalizer, ProgettoIUMDbContext context)
        {
            _sharedService = sharedService;
            _publisher = publisher;
            _sharedLocalizer = sharedLocalizer;
            _dbContext = context;

            ModelUnbinderHelpers.ModelUnbinders.Add(typeof(IndexViewModel), new SimplePropertyModelUnbinder());
        }

        [HttpGet]
        public virtual async Task<IActionResult> Index(IndexViewModel model)
        {
    
            var query = model.ToSegnalazioniIndexQuery();

         
            var s = await _sharedService.Query(query);

            var compilazioneVm = new CompilazioneViewModel();

            ViewBag.Categorie = compilazioneVm.CategorieDisponibili;


            model.SetSegnalazioni(s);

            return View(model);
        }

        [HttpGet]
        public virtual async Task<IActionResult> Edit(Guid? id)
        {
            var model = new EditViewModel();

            if (id.HasValue)
            {
                var dto = await _sharedService.Query(new SegnalazioniDetailQuery
                {
                    Id = id.Value,
                });

                model.SetSegnalazioni(dto);

                var storico = dto.StoricoStati?
             .OrderByDescending(x => x.DataCambio)
             .ToList() ?? new List<StoricoStato>();

             
                if (!storico.Any(s => s.StatoNuovo == "Segnalazione creata"))
                {
                    storico.Add(new StoricoStato
                    {
                        DataCambio = dto.DataInvio,
                        StatoNuovo = "Segnalazione creata"
                    });
                }

                model.StoricoStati = storico
                    .OrderByDescending(x => x.DataCambio)
                    .ToList();

            }

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public virtual async Task<IActionResult> Edit(EditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                Alerts.AddError(this, "Errore nei dati inseriti");
                return View(model);
            }

            if (model.DataRisoluzionePrevista.HasValue && model.DataRisoluzionePrevista.Value.Date < DateTime.Today)
            {
                TempData["ErrorMessage"] = "Inserire una data di risoluzione valida";
                return RedirectToAction(nameof(Edit), new { id = model.Id });
            }

            if (!string.IsNullOrWhiteSpace(model.Esito) && model.StatoAttuale != "Chiusa")
            {
                TempData["ErrorMessage"] = "Per inserire un esito lo stato della segnalazione deve essere chiusa";
                return RedirectToAction(nameof(Edit), new { id = model.Id });
            }

            if (string.IsNullOrWhiteSpace(model.Esito) && model.StatoAttuale == "Chiusa")
            {
                TempData["ErrorMessage"] = "Inserire un esito prima di chiudere la segnalazione";
                return RedirectToAction(nameof(Edit), new { id = model.Id });
            }

            try
            {
                await _sharedService.Handle(model.ToAddOrUpdateSegnalazioneCommand());

                var segnalazione = await _dbContext.Segnalazioni
                    .Include(s => s.StoricoStati)
                    .FirstOrDefaultAsync(s => s.Id == model.Id);

                if (segnalazione != null)
                {
                    var ultimoStato = segnalazione.StoricoStati
                        .OrderByDescending(s => s.DataCambio)
                        .FirstOrDefault()?.StatoNuovo;

                    if (ultimoStato != model.StatoAttuale)
                    {
                        segnalazione.StoricoStati.Add(new StoricoStato
                        {
                            StatoNuovo = model.StatoAttuale,
                            DataCambio = DateTimeOffset.Now
                        });

                        await _dbContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                Alerts.AddError(this, "Errore durante il salvataggio");
                return View(model);
            }

            TempData["SuccessMessage"] = "Informazioni aggiornate correttamente";
            return RedirectToAction(nameof(Edit), new { id = model.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> AddMessage(Guid segnalazioneId, string testo)
        {
            if (string.IsNullOrWhiteSpace(testo))
            {
                TempData["ErrorMessage"] = "Il messaggio non può essere vuoto.";
                return RedirectToAction("Edit", new { id = segnalazioneId });
            }

            try
            {
                await _sharedService.Handle(new AddComunicazioneCommand
                {
                    SegnalazioneId = segnalazioneId,
                    Testo = testo,
                    IsOperatore = true
                });

                TempData["SuccessMessage"] = "Messaggio inviato con successo!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Errore nell'invio del messaggio: " + ex.Message;
            }

            return RedirectToAction("Edit", new { id = segnalazioneId });
        }


     

    }





}



