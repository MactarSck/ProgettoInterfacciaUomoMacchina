using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoIUM.Services.Shared.Segnalazione
{
    public class Segnalazione
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public Guid Id { get; set; }
        public string CodiceUnivoco { get; set; }
        public DateTime DataInvio { get; set; }
        public string Categoria { get; set; }
        public string Luogo { get; set; }
        public string Reparto { get; set; }
        public string Descrizione { get; set; }
        public string StatoAttuale { get; set; }
        public string Priorità { get; set; }
        public string? NomeFile { get; set; } 
        public string? PathFile { get; set; } 
        public string? Esito { get; set; }
        public DateTime? DataRisoluzionePrevista { get; set; }







    }
}
