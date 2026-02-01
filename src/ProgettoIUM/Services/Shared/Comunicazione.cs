using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoIUM.Services.Shared.Segnalazione
{
    public class Comunicazione
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid SegnalazioneId { get; set; }
        [ForeignKey(nameof(SegnalazioneId))]
        public Segnalazione Segnalazione { get; set; }
        public string Testo { get; set; } 
        public DateTime DataInvio { get; set; }
        
        public bool IsOperatore { get; set; }

    }
}
