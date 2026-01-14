using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProgettoIUM.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace ProgettoIUM.Web.Features.Compilazione
{
    [TypeScriptModule("Features.Compilazione.Server")]
    public class CompilazioneViewModel
    {
        [Required(ErrorMessage = "Il campo '{0}' è obbligatorio")]
        public string Categoria { get; set; }

        [Required(ErrorMessage = "Il campo '{0}' è obbligatorio")]
        public string Luogo { get; set; }

        [Required(ErrorMessage = "Il campo '{0}' è obbligatorio")]
        public string Reparto { get; set; }
        [Required(ErrorMessage = "Il campo '{0}' è obbligatorio")]
        public string Descrizione { get; set; }
        public DateTime DataInvio { get; set; } = DateTime.Now;

        public IFormFile? Allegato { get; set; }

        public List<string> CategorieDisponibili { get; set; } = new()
        {
            "Frode", "Corruzione", "Molestie", "Sicurezza Informatica", "Altro"
        };

    }
}
