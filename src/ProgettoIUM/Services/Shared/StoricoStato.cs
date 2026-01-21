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
        public Guid SegnalazioneId { get; set; }
        public string StatoPrecedente { get; set; }
        public string StatoNuovo { get; set; }
        public DateTime DataCambio { get; set; }

       
    }
}
