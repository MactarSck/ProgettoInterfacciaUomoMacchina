using System;
using System.Linq;
using ProgettoIUM.Services;
using ProgettoIUM.Services.Shared;
using ProgettoIUM.Services.Shared.Segnalazione;

namespace ProgettoIUM.Infrastructure
{
    public class DataGenerator
    {
        public static void InitializeUsers(ProgettoIUMDbContext context)
        {
            if (context.Users.Any())
            {
                return;   // Data was already seeded
            }

            context.Users.AddRange(
                new User
                {
                    Id = Guid.Parse("3de6883f-9a0b-4667-aa53-0fbc52c4d300"), 
                    Email = "gestore@test.it",
                    Password = "M0Cuk9OsrcS/rTLGf5SY6DUPqU2rGc1wwV2IL88GVGo=", // SHA-256 of text "Prova"
                    FirstName = "Gestione",
                    LastName = "Cognome1",
                    
                });

            context.SaveChanges();
        }

        public static void InitializeSegnalazioni(ProgettoIUMDbContext context)
        {
            // Verifica se esistono già segnalazioni per evitare duplicati
            if (context.Segnalazioni.Any())
            {
                return;
            }
            context.Segnalazioni.AddRange(
                new Segnalazione
                {
                    Id = Guid.NewGuid(),
                    CodiceUnivoco = "MT-9942-RS",
                    DataInvio = DateTime.Now.AddDays(-2),
                    Categoria = "Manutenzione",
                    Luogo = "ONIT SANITA Cesena",
                    Reparto = "Produzione",
                    Descrizione = "Rilevato guasto meccanico alla linea 3.",
                    StatoAttuale = "Aperta",
                    Priorità = "Media",
                    Esito = ""
                },
                new Segnalazione
                {
                    Id = Guid.NewGuid(),
                    CodiceUnivoco = "SI-1023-QA",
                    DataInvio = DateTime.Now.AddHours(-5),
                    Categoria = "Sicurezza Informatica",
                    Luogo = "Forli VEM",
                    Reparto = "Logistica",
                    Descrizione = "Uscita di emergenza ostruita da imballaggi.",
                    StatoAttuale = "Chiusa",
                    Priorità = "Bassa",
                    Esito = "Risolto - Area sgomberata"
                },
                new Segnalazione
                {
                    Id = Guid.NewGuid(),
                    CodiceUnivoco = "IT-5512-LB",
                    DataInvio = DateTime.Now.AddDays(-1),
                    Categoria = "Sicurezza Informatica",
                    Luogo = "MAGGIOLI Savignano",
                    Reparto = "IT",
                    Descrizione = "Problemi di connessione al server centrale.",
                    StatoAttuale = "In Lavorazione",
                    Priorità = "Alta",
                    Esito = ""
                },
                new Segnalazione
                {
                    Id = Guid.NewGuid(),
                    CodiceUnivoco = "MQ-3242-R3",
                    DataInvio = DateTime.Now.AddDays(-3),
                    Categoria = "Altro",
                    Luogo = "VEM forli",
                    Reparto = "HR",
                    Descrizione = "Rilevato guasto meccanico alla linea 3.",
                    StatoAttuale = "Aperta",
                    Priorità = "Media",
                    Esito = ""
                }



            );

            context.SaveChanges();
        }
    }
}
