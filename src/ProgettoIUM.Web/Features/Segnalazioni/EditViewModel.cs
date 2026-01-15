using ProgettoIUM.Web.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;
using ProgettoIUM.Services.Shared;


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

        public DateOnly DataRisoluzionePrevista { get; set; }


        public string ToJson()
        {
            return Infrastructure.JsonSerializer.ToJsonCamelCase(this);
        }

        public void SetSegnalazioni(SegnalazioneDetailDTO segnalazioneDetailDTO)
        {
            if (segnalazioneDetailDTO != null)
            {
                Id = segnalazioneDetailDTO.Id;
                DataInvio = segnalazioneDetailDTO.DataInvio;
                Categoria = segnalazioneDetailDTO.Categoria;
                Luogo = segnalazioneDetailDTO.Luogo;
                Esito = segnalazioneDetailDTO.Esito;
                Descrizione = segnalazioneDetailDTO.Descrizione;
                Priorità = segnalazioneDetailDTO.Priorità;
                DataRisoluzionePrevista = segnalazioneDetailDTO.DataRisoluzionePrevista;
                

                

            }
        }

        public AddOrUpdateSegnalazioneCommand ToAddOrUpdateSegnalazioneCommand()
        {
            return new AddOrUpdateSegnalazioneCommand
            {
                Id = Id,
                Esito = Esito,
                Descrizione = Descrizione,
                DataInvio = DataInvio,
                Categoria = Categoria,
                Priorità = Priorità,
                Luogo = Luogo,
                DataRisoluzionePrevista = DataRisoluzionePrevista,
            };
        }
    }

}
