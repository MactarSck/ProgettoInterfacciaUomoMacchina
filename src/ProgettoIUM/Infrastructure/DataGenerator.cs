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
                    Id = Guid.Parse("3de6883f-9a0b-4667-aa53-0fbc52c4d300"), // Forced to specific Guid for tests
                    Email = "email1@test.it",
                    Password = "M0Cuk9OsrcS/rTLGf5SY6DUPqU2rGc1wwV2IL88GVGo=", // SHA-256 of text "Prova"
                    FirstName = "Nome1",
                    LastName = "Cognome1",
                    
                },
                new User
                {
                    Id = Guid.Parse("a030ee81-31c7-47d0-9309-408cb5ac0ac7"), // Forced to specific Guid for tests
                    Email = "email2@test.it",
                    Password = "Uy6qvZV0iA2/drm4zACDLCCm7BE9aCKZVQ16bg80XiU=", // SHA-256 of text "Test"
                    FirstName = "Nome2",
                    LastName = "Cognome2",
                    
                },
                new User
                {
                    Id = Guid.Parse("bfdef48b-c7ea-4227-8333-c635af267354"), // Forced to specific Guid for tests
                    Email = "email3@test.it",
                    Password = "Uy6qvZV0iA2/drm4zACDLCCm7BE9aCKZVQ16bg80XiU=", // SHA-256 of text "Test"
                    FirstName = "Nome3",
                    LastName = "Cognome3",
                    
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
                    Luogo = "Cesena",
                    Reparto = "Produzione",
                    Descrizione = "Rilevato guasto meccanico alla linea 3.",
                    StatoAttuale = "Aperta",
                    Priorità = "Media",
                    Esito = "-"
                },
                new Segnalazione
                {
                    Id = Guid.NewGuid(),
                    CodiceUnivoco = "SI-1023-QA",
                    DataInvio = DateTime.Now.AddHours(-5),
                    Categoria = "Sicurezza Informatica",
                    Luogo = "Rimini",
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
                    Luogo = "Savignano",
                    Reparto = "IT",
                    Descrizione = "Problemi di connessione al server centrale.",
                    StatoAttuale = "In Lavorazione",
                    Priorità = "Alta",
                    Esito = "-"
                },
                new Segnalazione
                {
                    Id = Guid.NewGuid(),
                    CodiceUnivoco = "MQ-3242-R3",
                    DataInvio = DateTime.Now.AddDays(-3),
                    Categoria = "Altro",
                    Luogo = "forli",
                    Reparto = "HR",
                    Descrizione = "Rilevato guasto meccanico alla linea 3.",
                    StatoAttuale = "Aperta",
                    Priorità = "Media",
                    Esito = "-"
                }



            );

            context.SaveChanges();
        }
    }
}
