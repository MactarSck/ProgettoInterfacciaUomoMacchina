using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProgettoIUM.Services.Shared;
using ProgettoIUM.Web.Areas.Example.Users;
using ProgettoIUM.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace ProgettoIUM.Web.Features.Segnalazioni
{
    public class IndexViewModel : PagingViewModel
    {

        public IndexViewModel()
        {
            OrderBy = nameof(SegnalazioniIndexViewModel.DataInvio);
            OrderByDescending = true;
            Segnalazioni = Array.Empty<SegnalazioniIndexViewModel>();
        }

        [Display(Name = "Cerca")]
        public string Filter { get; set; }
        public DateTime? Dal { get; set; }
        public string FStato{ get; set; }
        public string FPriorità { get; set; }
        public string Fcategoria { get; set; }
        public string Fluogo { get; set; }
        public string Fesito { get; set; }


        internal void SetSegnalazioni(SegnalazioniIndexDTO segnalazioniIndexDTO)
        {
            Segnalazioni = segnalazioniIndexDTO.Segnalazioni.Select(x => new SegnalazioniIndexViewModel(x)).ToArray();
            TotalItems = segnalazioniIndexDTO.Count;
        }


        public SegnalazioneIndexQuery ToSegnalazioniIndexQuery()
        {
           
            return new SegnalazioneIndexQuery
            {
                Filter = Filter,
                Dal = Dal,
                Fesito = Fesito,
                Fcategoria = Fcategoria,
                FPriorità = FPriorità,
                FStato = FStato,
                Fluogo = Fluogo,
                Paging = new ProgettoIUM.Infrastructure.Paging
                {
                    OrderBy = OrderBy,
                    OrderByDescending = OrderByDescending,
                    Page = Page,
                    PageSize = PageSize
                }
            };
        }

        public IEnumerable<SegnalazioniIndexViewModel> Segnalazioni { get; set; }

        public override IActionResult GetRoute() => MVC.Segnalazioni.Index(this).GetAwaiter().GetResult();

        public string OrderbyUrl<TProperty>(IUrlHelper url, System.Linq.Expressions.Expression<Func<SegnalazioniIndexViewModel, TProperty>> expression) => base.OrderbyUrl(url, expression);

        public string OrderbyCss<TProperty>(HttpContext context, System.Linq.Expressions.Expression<Func<SegnalazioniIndexViewModel, TProperty>> expression) => base.OrderbyCss(context, expression);

        public string ToJson()
        {
            return JsonSerializer.ToJsonCamelCase(this);
        }

    }

    public class SegnalazioniIndexViewModel
    {
        public SegnalazioniIndexViewModel(SegnalazioniIndexDTO.Segnalazione segnalazioniIndexDTO)
        {
            this.Id = segnalazioniIndexDTO.Id;
            this.Priorità = segnalazioniIndexDTO.Priorità;
            this.Stato = segnalazioniIndexDTO.Stato;
            this.Luogo = segnalazioniIndexDTO.Luogo;
            this.Categoria = segnalazioniIndexDTO.Categoria;
            this.DataInvio = segnalazioniIndexDTO.DataInvio;
            this.Esito = segnalazioniIndexDTO.Esito;
                
            
        }

        public Guid Id { get; set; }
        public string Priorità { get; set; }
        public string Stato { get; set; }
        public string Categoria { get; set; }
        public string Luogo { get; set; }
        public DateTime DataInvio { get; set; }
        public string Esito { get; set; }
    }
}
