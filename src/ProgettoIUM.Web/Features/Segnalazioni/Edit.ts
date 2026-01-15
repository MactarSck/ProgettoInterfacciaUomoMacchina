module Features.Segnalazioni {
    export class editVueModel {
        // Proprietà per la chat
        public nuoviMessaggi: any[] = [];
        public messaggioCorrente: string = "";

       

        private scrollChatBottom() {
            const container = document.getElementById("chatContainer");
            if (container) container.scrollTop = container.scrollHeight;
        }
    }
}