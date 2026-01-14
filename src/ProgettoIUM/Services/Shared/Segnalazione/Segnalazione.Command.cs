using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProgettoIUM.Services.Shared.Segnalazione;

namespace ProgettoIUM.Services.Shared
{
    public class AddOrUpdateSegnalazioneCommand
    {
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
    }

    public partial class SharedService
    {
        public async Task<Guid> Handle(AddOrUpdateSegnalazioneCommand cmd)
        {
            var s = await _dbContext.Segnalazioni
                .Where(x => x.Id == cmd.Id)
                .FirstOrDefaultAsync();

            if (s == null)
            {
                s = new Segnalazione.Segnalazione
                {
                   CodiceUnivoco = cmd.CodiceUnivoco,
                };
                _dbContext.Segnalazioni.Add(s);
            }

            s.StatoAttuale = cmd.StatoAttuale;
            s.DataInvio = cmd.DataInvio;
            s.Descrizione = cmd.Descrizione;
            s.Esito = s.Esito;
            s.Reparto = s.Reparto;
            s.Categoria = s.Categoria;
            s.Luogo = s.Luogo;
            s.Reparto = s.Reparto;
            s.Luogo = s.Luogo;
      

            await _dbContext.SaveChangesAsync();

            return s.Id;
        }
    }
}
