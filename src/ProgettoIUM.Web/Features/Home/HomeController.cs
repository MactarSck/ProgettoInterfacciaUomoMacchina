using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
using System.Threading.Tasks;

namespace ProgettoIUM.Web.Features.Home
{
    public partial class HomeController : Controller
    {
        private readonly SharedService _sharedService;
        private readonly ProgettoIUMDbContext _dbContext;
        private readonly IDataProtector _protector;

        public HomeController(ProgettoIUMDbContext context, IDataProtectionProvider provider, SharedService sharedService)
        {
            _dbContext = context;
            _protector = provider.CreateProtector("Segnalazione.Codice.Sicurezza.v1");
            _sharedService = sharedService;
        }

        [OutputCache(NoStore = true, Duration = 0)]
        public virtual async Task<IActionResult> Index()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            ViewData["ForceUnauthenticated"] = true;
            var model = new HomeViewModel();
            return View(model);
        }

        [HttpGet]
        [HttpGet]
        public virtual IActionResult DettaglioUtente(string token, Guid? id)
        {
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            string codiceReale = null;

            if (!string.IsNullOrWhiteSpace(token))
            {
                try
                {
                    codiceReale = _protector.Unprotect(token);
                }
                catch
                {
                    TempData["Error"] = "Link non valido o scaduto.";
                    return RedirectToAction("Index");
                }
            }
            else if (id.HasValue)
            {
                var seg = _dbContext.Segnalazioni.FirstOrDefault(s => s.Id == id.Value);
                if (seg == null)
                {
                    TempData["Error"] = "Segnalazione non trovata.";
                    return RedirectToAction("Index");
                }
                codiceReale = seg.CodiceUnivoco;
            }
            else
            {
                TempData["Error"] = "Link non valido.";
                return RedirectToAction("Index");
            }

            // Carico la segnalazione con storico stati e comunicazioni
            var segnalazione = _dbContext.Segnalazioni
                .Include(s => s.StoricoStati)
                .Include(s => s.ChatMessaggi) // <-- aggiunto per visualizzare i messaggi
                .FirstOrDefault(s => s.CodiceUnivoco == codiceReale);

            if (segnalazione == null)
            {
                TempData["Error"] = "Segnalazione non trovata.";
                return RedirectToAction("Index");
            }

            var storico = segnalazione.StoricoStati?
                .OrderByDescending(s => s.DataCambio)
                .ToList() ?? new List<StoricoStato>();

            if (!storico.Any(s => s.StatoNuovo == "Segnalazione creata"))
            {
                storico.Add(new StoricoStato
                {
                    DataCambio = segnalazione.DataInvio,
                    StatoNuovo = "Segnalazione creata"
                });
            }

            storico = storico.OrderByDescending(s => s.DataCambio).ToList();

            var viewModel = new EditViewModel
            {
                Id = segnalazione.Id,
                CodiceUnivoco = codiceReale,
                StatoAttuale = segnalazione.StatoAttuale,
                DataRisoluzionePrevista = segnalazione.DataRisoluzionePrevista,
                Categoria = segnalazione.Categoria,
                DataInvio = segnalazione.DataInvio,
                Luogo = segnalazione.Luogo,
                Reparto = segnalazione.Reparto,
                Descrizione = segnalazione.Descrizione,
                PathFile = segnalazione.PathFile,
                NomeFile = segnalazione.NomeFile,
                Esito = segnalazione.Esito,
                StoricoStati = storico,
                ChatMessaggi = segnalazione.ChatMessaggi
                    .OrderBy(c => c.DataInvio)
                    .ToList() 
            };

            return View("~/Features/Segnalazioni/DettaglioUtente.cshtml", viewModel);
        }



        [HttpPost]
        public virtual IActionResult DettaglioUtentePost(string codiceUnivoco)
        {
            if (string.IsNullOrWhiteSpace(codiceUnivoco))
            {
                TempData["Error"] = "Inserisci un codice valido.";
                return RedirectToAction("Index");
            }

            
            bool esiste = _dbContext.Segnalazioni.Any(s => s.CodiceUnivoco == codiceUnivoco);

            if (!esiste)
            {
                TempData["Error"] = "Codice non valido.";
                return RedirectToAction("Index");
            }

            
            string token = _protector.Protect(codiceUnivoco);


            return RedirectToAction("DettaglioUtente", new { token });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> AddMessageUtente(Guid segnalazioneId, string testo)
        {
            if (string.IsNullOrWhiteSpace(testo))
            {
                TempData["ErrorMessage"] = "Il messaggio non può essere vuoto.";
                return RedirectToAction("DettaglioUtente", new { id = segnalazioneId });
            }

            try
            {
                await _sharedService.Handle(new AddComunicazioneCommand
                {
                    SegnalazioneId = segnalazioneId,
                    Testo = testo,
                    IsOperatore = false
                });

                TempData["SuccessMessage"] = "Messaggio inviato con successo!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Errore nell'invio del messaggio: " + ex.Message;
            }

            // Redirect alla stessa pagina usando ID invece del token
            return RedirectToAction("DettaglioUtente", new { id = segnalazioneId });
        }


        [HttpPost]
        public virtual IActionResult ChangeLanguageTo(string cultureName)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(cultureName)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), Secure = true }
            );

            return Redirect(Request.GetTypedHeaders().Referer?.ToString() ?? "~/");
        }

        public virtual IActionResult Privacy()
        {
            return View();
        }


    }
}
