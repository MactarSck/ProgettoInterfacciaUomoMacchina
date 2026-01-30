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
        private readonly ProgettoIUMDbContext _dbContext;
        private readonly IDataProtector _protector;

        public HomeController(ProgettoIUMDbContext context, IDataProtectionProvider provider)
        {
            _dbContext = context;
            _protector = provider.CreateProtector("Segnalazione.Codice.Sicurezza.v1");
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
        public virtual IActionResult DettaglioUtente(string token)
        {
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            if (string.IsNullOrWhiteSpace(token))
            {
                TempData["Error"] = "Link non valido.";
                return RedirectToAction("Index");
            }

            string codiceReale;


            try
            {
                codiceReale = _protector.Unprotect(token);
            }
            catch
            {
                TempData["Error"] = "Link non valido o scaduto.";
                return RedirectToAction("Index");
            }

            var segnalazione = _dbContext.Segnalazioni
                .Include(s => s.StoricoStati)
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

            storico = storico
                .OrderByDescending(s => s.DataCambio)
                .ToList();

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
                StoricoStati = storico
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
