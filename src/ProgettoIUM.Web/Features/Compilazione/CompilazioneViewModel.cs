using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProgettoIUM.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProgettoIUM.Web.Features.Compilazione
{
    [TypeScriptModule("Features.Compilazione.Server")]
    public class CompilazioneViewModel
    {
        [Required(ErrorMessage = "Seleziona una {0}")]
        [Display(Name = "Categoria")]
        public string Categoria { get; set; }

        [Required(ErrorMessage = "Inserisci il {0}")]
        [Display(Name = "Luogo dell'accaduto")]
        public string Luogo { get; set; }

        [Required(ErrorMessage = "Specificare il {0}")]
        [Display(Name = "Reparto")]
        public string Reparto { get; set; }

        [Required(ErrorMessage = "Inserisci una {0}")]
        [Display(Name = "Descrizione")]
        [MinLength(20, ErrorMessage = "La descrizione deve essere di almeno 20 caratteri")]
        [MaxLength(2000, ErrorMessage = "La descrizione non può superare i 2000 caratteri")]
        public string Descrizione { get; set; }

        public DateTime DataInvio { get; set; } = DateTime.Now;

       
        public IFormFile? Allegato { get; set; }

        
        public string? NomeFileGiaCaricato { get; set; }
        public string? PathFileGiaCaricato { get; set; }

        public List<string> CategorieDisponibili { get; set; } = new()
        {
            "Frode e Corruzione",
            "Conflitto di Interesse",
            "Violazione della Privacy / GDPR",
            "Abusi e Molestie",
            "Discriminazione",
            "Sicurezza sul Lavoro",
            "Sicurezza Informatica / Cybersecurity",
            "Violazione delle Norme Ambientali",
            "Violazione delle Norme Antitrust",
            "Irregolarità Contabili o Finanziarie",
            "Violazione del Codice Etico",
            "Gestione Inappropriata di Risorse",
            "Abuso di Potere",
            "Ostacolo alle Indagini / Ritorsioni",
            "Altro"
        };

    }
}