using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using ProgettoIUM.Services;
using ProgettoIUM.Services.Shared;
using ProgettoIUM.Services.Shared.Segnalazione;
using ProgettoIUM.Web.Features.Compilazione;
using System;
using System.IO;
using System.Threading.Tasks;



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

        [HttpPost("Conferma")]
        public virtual async Task<IActionResult> Conferma(CompilazioneViewModel model)
        {
            if (!ModelState.IsValid) return View("~/Features/Compilazione/Index.cshtml", model);

            if (model.Allegato != null)
            {
                
                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsPath)) Directory.CreateDirectory(uploadsPath);

                
                var uniqueName = Guid.NewGuid().ToString() + Path.GetExtension(model.NomeFileGiaCaricato);
                var filePath = Path.Combine(uploadsPath, uniqueName);

                
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Allegato.CopyToAsync(stream);
                }

                
                model.NomeFileGiaCaricato = model.Allegato.FileName;
                model.PathFileGiaCaricato = "/uploads/" + uniqueName;
            }

            return View("~/Features/Compilazione/Riepilogo.cshtml", model);
        }

        [HttpPost("InviaDefinitivo")]
        public virtual async Task<IActionResult> InviaDefinitivo(CompilazioneViewModel model)
        {
            string codice = GenerateCodiceUnivoco();

            var segnalazione = new Segnalazione
            {
                Id = Guid.NewGuid(),
                CodiceUnivoco = codice,
                DataInvio = model.DataInvio,
                Categoria = model.Categoria,
                Luogo = model.Luogo,
                Reparto = model.Reparto,
                Descrizione = model.Descrizione,

                
                StatoAttuale = "Nuova - In attesa di verifica",
                Priorità = "Non Definita",
                Esito = "",

                
                NomeFile = model.NomeFileGiaCaricato,
                PathFile = model.PathFileGiaCaricato
            };

            _dbContext.Segnalazioni.Add(segnalazione);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Successo", new { codice = codice });
        }


        private string GenerateCodiceUnivoco()
        {
            return $"SK-{DateTime.Now.Year}-{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}";
        }


        [HttpGet]
        public virtual IActionResult Successo(string codice)
        {
            ViewBag.CodiceUnivoco = codice ?? "SK-ERROR-000";
            return View("~/Features/Compilazione/Successo.cshtml");
        }
    }
}
