using System;
using System.Collections.Generic;
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
            if (context.Segnalazioni.Any())
            {
                return;
            }

            // Lista categorie fornite
            var categorie = new List<string>
            {
                "Frode e Corruzione", "Conflitto di Interesse", "Violazione della Privacy / GDPR",
                "Abusi e Molestie", "Discriminazione", "Sicurezza sul Lavoro",
                "Sicurezza Informatica / Cybersecurity", "Violazione delle Norme Ambientali",
                "Violazione delle Norme Antitrust", "Irregolarità Contabili o Finanziarie",
                "Violazione del Codice Etico", "Gestione Inappropriata di Risorse",
                "Abuso di Potere", "Ostacolo alle Indagini / Ritorsioni", "Altro"
            };

            var luoghi = new[] { "Sede Cesena", "Filiale Forlì", "Magazzino Rimini", "Ufficio Bologna", "Sito Produttivo Savignano" };
            var reparti = new[] { "IT", "HR", "Logistica", "Produzione", "Amministrazione", "Marketing" };
            var stati = new[] { "Nuova - In attesa di verifica", "In lavorazione", "Chiusa" };
            var prioritaOptions = new[] { "Alta", "Media", "Bassa" };

            var random = new Random();
            var segnalazioniList = new List<Segnalazione>();

            // 1. Aggiungiamo le tue segnalazioni specifiche per i test manuali
            segnalazioniList.Add(new Segnalazione
            {
                Id = Guid.NewGuid(),
                CodiceUnivoco = "MT-9942-RS",
                DataInvio = DateTime.Now.AddDays(-2),
                Categoria = "Violazione della Privacy / GDPR",
                Luogo = "ONIT SANITA Cesena",
                Reparto = "Produzione",
                Descrizione = "Rilevato guasto meccanico alla linea 3.",
                StatoAttuale = "In lavorazione",
                Priorità = "Media",
                Esito = ""
            });

            segnalazioniList.Add(new Segnalazione
            {
                Id = Guid.NewGuid(),
                CodiceUnivoco = "SI-1023-QA",
                DataInvio = DateTime.Now.AddHours(-5),
                Categoria = "Sicurezza sul Lavoro",
                Luogo = "Forli VEM",
                Reparto = "Logistica",
                Descrizione = "Uscita di emergenza ostruita da imballaggi.",
                StatoAttuale = "Nuova - In attesa di verifica",
                Priorità = "Non Definita",
                Esito = ""
            });

           
            for (int i = 1; i <= 45; i++)
            {
                var stato = stati[random.Next(stati.Length)];
                var cat = categorie[random.Next(categorie.Count)];

                segnalazioniList.Add(new Segnalazione
                {
                    Id = Guid.NewGuid(),
                    CodiceUnivoco = $"ID-{random.Next(1000, 9999)}-{i}",
                    DataInvio = DateTime.Now.AddDays(-random.Next(0, 30)).AddMinutes(-random.Next(0, 1440)),
                    Categoria = cat,
                    Luogo = luoghi[random.Next(luoghi.Length)],
                    Reparto = reparti[random.Next(reparti.Length)],
                    Descrizione = $"Segnalazione di test automatica numero {i}. Dettagli relativi a {cat}.",
                    StatoAttuale = stato,
                   
                    Priorità = (stato == "Nuova - In attesa di verifica") ? "Non Definita" : prioritaOptions[random.Next(prioritaOptions.Length)],
                    Esito = (stato == "Chiusa") ? "Verificata e risolta" : ""
                });
            }

            context.Segnalazioni.AddRange(segnalazioniList);
            context.SaveChanges();
        }
    }
}