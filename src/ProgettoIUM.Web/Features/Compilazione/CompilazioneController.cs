using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProgettoIUM.Services;
using ProgettoIUM.Services.Shared.Segnalazione;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProgettoIUM.Web.Features.Compilazione
{
    public partial class CompilazioneController : Controller
    {
        private readonly ProgettoIUMDbContext _dbContext;
        private readonly IDataProtector _protector;


        public CompilazioneController(ProgettoIUMDbContext context, IDataProtectionProvider provider)
        {
            _dbContext = context;
          
            _protector = provider.CreateProtector("Segnalazione.Codice.Sicurezza.v1");
        }

        [HttpGet]
        public virtual IActionResult NuovaSegnalazione(
          string Categoria,
          string Luogo,
          string Reparto,
          string Descrizione,
          DateTime DataInvio,
          string NomeFileGiaCaricato,
          string PathFileGiaCaricato)
            {
                var model = new CompilazioneViewModel
                {
                    Categoria = Categoria,
                    Luogo = Luogo,
                    Reparto = Reparto,
                    Descrizione = Descrizione,
                    DataInvio = DataInvio,
                    NomeFileGiaCaricato = NomeFileGiaCaricato,
                    PathFileGiaCaricato = PathFileGiaCaricato
                };

            return View("~/Features/Compilazione/Index.cshtml", model);
        }


        [HttpPost("Conferma")]
        public virtual async Task<IActionResult> Conferma(CompilazioneViewModel model)
        {
            if (!ModelState.IsValid) return View("~/Features/Compilazione/Index.cshtml", model);

            if (model.Allegato != null)
            {
                var allowed = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
                var ext = Path.GetExtension(model.Allegato.FileName).ToLower();

                if (!allowed.Contains(ext))
                {
                    ModelState.AddModelError("Allegato", "Formato file non valido, Quelli consentiti sono  pdf , jpg , jpeg , png ");
                    return View("~/Features/Compilazione/Index.cshtml", model);
                }

   
                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsPath)) Directory.CreateDirectory(uploadsPath);

                var uniqueName = Guid.NewGuid().ToString() + ext;
                var filePath = Path.Combine(uploadsPath, uniqueName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Allegato.CopyToAsync(stream);
                }

                model.NomeFileGiaCaricato = Path.GetFileName(model.Allegato.FileName);
                model.PathFileGiaCaricato = "/uploads/" + uniqueName;
            }
            else
            {
       
            }


            return View("~/Features/Compilazione/Riepilogo.cshtml", model);
        }

        [HttpPost("InviaDefinitivo")]
        public virtual async Task<IActionResult> InviaDefinitivo(CompilazioneViewModel model)
        {
            string codiceOriginale = GenerateCodiceUnivoco();

            var segnalazione = new Segnalazione
            {
                Id = Guid.NewGuid(),
                CodiceUnivoco = codiceOriginale,
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

            
            string codiceProtetto = _protector.Protect(codiceOriginale);

            return RedirectToAction("Successo", new { id = codiceProtetto });
        }

        [HttpGet]
        public virtual IActionResult Successo(string id)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction("Index", "Home");

            try
            {
               
                string codiceChiaro = _protector.Unprotect(id);

                ViewBag.CodiceInChiaro = codiceChiaro; 
                ViewBag.CodiceProtetto = id;           
            }
            catch
            {
                
                return RedirectToAction("Index", "Home");
            }

            return View("~/Features/Compilazione/Successo.cshtml");
        }

        private string GenerateCodiceUnivoco()
        {
            return $"SK-{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}-{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}";
        }
    }
}