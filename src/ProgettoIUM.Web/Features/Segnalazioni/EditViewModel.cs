using ProgettoIUM.Services.Shared;
using ProgettoIUM.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace ProgettoIUM.Web.Features.Segnalazioni
{
    [TypeScriptModule("Features.Segnalazioni.Server")]
    public class EditViewModel
    {
        public EditViewModel()
        {
        }

        public Guid Id { get; set; }
        public string CodiceUnivoco { get; set; }
        public DateTime DataInvio { get; set; }

        [Display(Name = "Categoria")]
        public string Categoria { get; set; }
        [Display(Name = "Luogo")]
        public string Luogo { get; set; }
        [Display(Name = "Reparto")]
        public string Reparto { get; set; }
        [Display(Name = "Descrizione")]
        public string Descrizione { get; set; }
        [Display(Name = "Stato Attuale")]
        public string StatoAttuale { get; set; }
        [Display(Name = "Priorità")]
        public string Priorità { get; set; }
        public string? NomeFile { get; set; }
        public string? PathFile { get; set; }
        [Display(Name = "Esito")]
        public string? Esito { get; set; }

        public DateTime? DataRisoluzionePrevista { get; set; }

        public List<StoricoStato> StoricoStati { get; set; }



        public string ToJson()
        {
            return Infrastructure.JsonSerializer.ToJsonCamelCase(this);
        }

        public void SetSegnalazioni(SegnalazioneDetailDTO dto)
        {
            if (dto == null) return;

            Id = dto.Id;
            CodiceUnivoco = dto.CodiceUnivoco;
            DataInvio = dto.DataInvio;
            Categoria = dto.Categoria;
            Luogo = dto.Luogo;
            Reparto = dto.Reparto;
            Descrizione = dto.Descrizione;
            StatoAttuale = dto.StatoAttuale;
            Priorità = dto.Priorità;
            Esito = dto.Esito;
            DataRisoluzionePrevista = dto.DataRisoluzionePrevista;
            NomeFile = dto.NomeFile;
            PathFile = dto.PathFile;
            StoricoStati = dto.StoricoStati?
                   .OrderByDescending(x => x.DataCambio)
                   .ToList() ?? new List<StoricoStato>();
        }


        public AddOrUpdateSegnalazioneCommand ToAddOrUpdateSegnalazioneCommand()
        {
            return new AddOrUpdateSegnalazioneCommand
            {
                Id = Id,
                StatoAttuale = StatoAttuale,
                Esito = Esito,
                Descrizione = Descrizione,
                DataInvio = DataInvio,
                Categoria = Categoria,
                Priorità = Priorità,
                Luogo = Luogo,
                Reparto = Reparto,
                DataRisoluzionePrevista = DataRisoluzionePrevista
                
            };
        }

    }

}
