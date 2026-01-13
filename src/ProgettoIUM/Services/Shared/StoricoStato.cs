using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoIUM.Services.Shared
{
    public class StoricoStato
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public int SegnalazioneId { get; set; }
        public string Stato { get; set; }
        public DateTime DataAggiornamento { get; set; }
        public string Note { get; set; }

        public int UtenteOperatatoreId { get; set; }

        public virtual User Operatore { get; set; }
    }
}
