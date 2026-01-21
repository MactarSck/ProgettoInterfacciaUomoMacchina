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
        public virtual IActionResult Index()
        {
            var model = new HomeViewModel();
            return View(model);
        }

        [HttpGet]
        public virtual IActionResult DettaglioUtente(string codiceUnivoco)
        {
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            if (string.IsNullOrWhiteSpace(codiceUnivoco))
            {
                return RedirectToAction("Index");
            }

            string codiceReale = codiceUnivoco;

            // Tentativo di decriptazione
            try
            {
                codiceReale = _protector.Unprotect(codiceUnivoco);
            }
            catch
            {
                // se non è criptato, va bene così
            }

            var segnalazione = _dbContext.Segnalazioni
                .Include(s => s.StoricoStati)
                .FirstOrDefault(s => s.CodiceUnivoco == codiceReale);

            if (segnalazione == null)
            {
                TempData["Error"] = "Codice univoco errato.";
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

            // ---- VIEWMODEL ----
            var viewModel = new EditViewModel
            {
                Id = segnalazione.Id,
                CodiceUnivoco = segnalazione.CodiceUnivoco,
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