var Features;
(function (Features) {
    var Segnalazioni;
    (function (Segnalazioni) {
        class editVueModel {
            constructor() {
                // Proprietà per la chat
                this.nuoviMessaggi = [];
                this.messaggioCorrente = "";
            }
            scrollChatBottom() {
                const container = document.getElementById("chatContainer");
                if (container)
                    container.scrollTop = container.scrollHeight;
            }
        }
        Segnalazioni.editVueModel = editVueModel;
    })(Segnalazioni = Features.Segnalazioni || (Features.Segnalazioni = {}));
})(Features || (Features = {}));
//# sourceMappingURL=Edit.js.map