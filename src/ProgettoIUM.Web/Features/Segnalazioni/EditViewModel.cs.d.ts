//#hash:xyIkkCtDAbs=
declare module Features.Segnalazioni.Server {
	interface editViewModel {
		id: any;
		codiceUnivoco: string;
		dataInvio: Date;
		categoria: string;
		luogo: string;
		reparto: string;
		descrizione: string;
		statoAttuale: string;
		priorità: string;
		nomeFile: string;
		pathFile: string;
		esito: string;
		dataRisoluzionePrevista?: Date;
	}
}
