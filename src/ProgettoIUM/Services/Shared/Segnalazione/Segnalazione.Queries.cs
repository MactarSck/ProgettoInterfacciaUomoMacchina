using Microsoft.EntityFrameworkCore;
using ProgettoIUM.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoIUM.Services.Shared
{
 
    public class SegnalazioneSelectQuery
    {
        public Guid IdCurrentSegnalazione { get; set; }
        public string Filter { get; set; }
    }

    public class SegnalazioneSelectDTO
    {
        public IEnumerable<Segnalazione> Segnalazioni { get; set; }
        public int Count { get; set; }

        public class Segnalazione
        {
            public Guid Id { get; set; }
            public string Categoria { get; set; }
        }
    }

    public class SegnalazioneIndexQuery
    {
        public Guid IdCurrentSegnalazione { get; set; }
        public string Filter { get; set; }
        public DateTime? Dal { get; set; }
        public string FStato { get; set; }
        public string FPriorità { get; set; }
        public string Fcategoria { get; set; }
        public string Fluogo { get; set; }
        public string Fesito { get; set; }
        public Paging Paging { get; set; }
    }

    public class SegnalazioneDetailDTO
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
        public List<StoricoStato> StoricoStati { get; set; }

    }

    public class SegnalazioniIndexDTO
    {
        public IEnumerable<Segnalazione> Segnalazioni { get; set; }
        public int Count { get; set; }

        public class Segnalazione
        {
            public Guid Id { get; set; }
            public string Priorità { get; set; }
            public string Stato { get; set; }
            public string Luogo { get; set; }
            public string Categoria { get; set; }
            public DateTime DataInvio { get; set; }
            public string Esito { get; set; }
        }
    }
    public class SegnalazioniDetailQuery
    {
        public Guid Id { get; set; }
    }

    public class CheckLoginCredentialsQuery2
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }


    public partial class SharedService
    {
        /// <summary>
        /// Returns users for a select field
        /// </summary>
        /// <param name="qry"></param>
        /// <returns></returns>
        public async Task<SegnalazioneSelectDTO> Query(SegnalazioneSelectQuery qry)
        {
            var queryable = _dbContext.Segnalazioni
                .Where(x => x.Id != qry.IdCurrentSegnalazione);

            if (string.IsNullOrWhiteSpace(qry.Filter) == false)
            {
                queryable = queryable.Where(x => x.Luogo.Contains(qry.Filter, StringComparison.OrdinalIgnoreCase));
            }

            return new SegnalazioneSelectDTO
            {
                Segnalazioni = await queryable
                .Select(x => new SegnalazioneSelectDTO.Segnalazione
                {
                    Id = x.Id,
                    Categoria = x.Categoria
                })
                .ToArrayAsync(),
                Count = await queryable.CountAsync(),
            };
        }

        /// <summary>
        /// Returns users for an index page
        /// </summary>
        /// <param name="qry"></param>
        /// <returns></returns>
        public async Task<SegnalazioniIndexDTO> Query(SegnalazioneIndexQuery qry)
        {
            var queryable = _dbContext.Segnalazioni
                .Where(x => x.Id != qry.IdCurrentSegnalazione);

            // 1. FILTRO GENERALE
            if (!string.IsNullOrWhiteSpace(qry.Filter))
            {
                var filter = qry.Filter.Trim();
                queryable = queryable.Where(x =>
                    x.Categoria.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                    x.Luogo.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                    x.StatoAttuale.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                    x.Priorità.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                    x.Esito.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                    x.Id.Contains(filter, StringComparison.OrdinalIgnoreCase)
                );
            }

            // 2. FILTRI SPECIFICI
            if (qry.Dal.HasValue)
            {
                queryable = queryable.Where(x => x.DataInvio.Date >= qry.Dal.Value.Date);
            }

            if (!string.IsNullOrWhiteSpace(qry.Fluogo))
            {
                queryable = queryable.Where(x => x.Luogo.Contains(qry.Fluogo, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(qry.FPriorità))
            {
                queryable = queryable.Where(x => x.Priorità == qry.FPriorità);
            }

            if (!string.IsNullOrWhiteSpace(qry.Fcategoria))
            {
                queryable = queryable.Where(x => x.Categoria == qry.Fcategoria);
            }

            if (!string.IsNullOrWhiteSpace(qry.FStato))
            {
                queryable = queryable.Where(x => x.StatoAttuale == qry.FStato);
            }

            if (!string.IsNullOrWhiteSpace(qry.Fesito))
            {
                queryable = queryable.Where(x => x.Esito.Contains(qry.Fesito, StringComparison.OrdinalIgnoreCase));
            }

            // 3. ESECUZIONE (Sempre fuori dagli IF)
            var count = await queryable.CountAsync();

            var items = await queryable
                .ApplyPaging(qry.Paging)
                .Select(x => new SegnalazioniIndexDTO.Segnalazione
                {
                    Id = x.Id,
                    Stato = x.StatoAttuale,
                    Priorità = x.Priorità,
                    Luogo = x.Luogo,
                    Categoria = x.Categoria,
                    DataInvio = x.DataInvio,
                    Esito = x.Esito
                })
                .ToArrayAsync();

            return new SegnalazioniIndexDTO
            {
                Segnalazioni = items,
                Count = count
            };
        }




        /// <summary>
        /// Returns the detail of the user who matches the Id passed in the qry parameter
        /// </summary>
        /// <param name="qry"></param>
        /// <returns></returns>
        public async Task<SegnalazioneDetailDTO> Query(SegnalazioniDetailQuery qry)
        {
            return await _dbContext.Segnalazioni
                .Where(x => x.Id == qry.Id)
                .Select(x => new SegnalazioneDetailDTO
                {
                    Id = x.Id,
                    DataInvio = x.DataInvio,
                    Categoria = x.Categoria,
                    Luogo = x.Luogo,
                    Reparto = x.Reparto,
                    Descrizione = x.Descrizione,
                    StatoAttuale = x.StatoAttuale,
                    Priorità = x.Priorità,
                    NomeFile = x.NomeFile, 
                    PathFile = x.PathFile,
                    Esito = x.Esito,
                    DataRisoluzionePrevista = x.DataRisoluzionePrevista,
                    StoricoStati = x.StoricoStati
                            .OrderByDescending(s => s.DataCambio)
                            .ToList()

                })
                .FirstOrDefaultAsync();
        }

        
    }
}
