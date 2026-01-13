using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoIUM.Services.Shared
{
    public class Comunicazione
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public int SegnalazioneId { get; set; }
        public string testo { get; set; } 
        public DateTime DataInvio { get; set; }

        public string Mittente { get; set; }
        
        public bool isOperatore { get; set; }

    }
}
