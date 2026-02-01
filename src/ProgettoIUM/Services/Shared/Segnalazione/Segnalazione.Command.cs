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

        public DateTime? DataRisoluzionePrevista { get; set; }

    }

    public class AddComunicazioneCommand
    {
        public Guid SegnalazioneId { get; set; }
        public string Testo { get; set; }
        public bool IsOperatore { get; set; }
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
            if (s.StatoAttuale != cmd.StatoAttuale)
            {
                var storico = new StoricoStato
                {
                    SegnalazioneId = s.Id,
                    StatoPrecedente = s.StatoAttuale,
                    StatoNuovo = cmd.StatoAttuale,
                    DataCambio = DateTime.UtcNow
                };
             

                _dbContext.StoricoStati.Add(storico);
            }
            s.StatoAttuale = cmd.StatoAttuale;
            s.Esito = cmd.Esito;
            s.DataRisoluzionePrevista = cmd.DataRisoluzionePrevista;
            s.Priorità = cmd.Priorità;
      

            await _dbContext.SaveChangesAsync();

            return s.Id;
        }

        public async Task<Guid> Handle(AddComunicazioneCommand cmd)
        {
            var msg = new Comunicazione
            {
                SegnalazioneId = cmd.SegnalazioneId,
                Testo = cmd.Testo,
                IsOperatore = cmd.IsOperatore,
                DataInvio = DateTime.UtcNow
            };

            _dbContext.Comunicazioni.Add(msg);
            await _dbContext.SaveChangesAsync();

            return msg.Id;
        }

    }
}
