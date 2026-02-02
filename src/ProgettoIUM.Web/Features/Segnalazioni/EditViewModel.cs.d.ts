//#hash:r9ofhld0zGY=
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
		nomeFileGiaCaricato: string;
		pathFileGiaCaricato: string;
		esito: string;
		dataRisoluzionePrevista?: Date;
		storicoStati: server.storicoStato[];
		chatMessaggi: server.comunicazione[];
	}
}
