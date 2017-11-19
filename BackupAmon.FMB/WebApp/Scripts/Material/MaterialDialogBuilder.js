//O parâmetro labels deve ser um array contendo os textos a serem exibidos nos botões da caixa de diálogo.
function buildDialog(title, content, labels, actions) {
    
    if (!(labels instanceof Array) || !(actions instanceof Array) || labels.length !== actions.length) {
        alert("Os labels e actions dos botões devem ser arrays de mesmo tamanho.");
        return;
    }

    $(".mdl-dialog > .mdl-dialog__title").html(title);
    $(".mdl-dialog > .mdl-dialog__content > p").html(content);

    $("dialog > .mdl-dialog__actions > button").remove();

    var primary = $("dialog > .mdl-dialog__actions > button").length == 0; //Verifica se existe pelo menos um botão adicionado

    for (var i = 0; i < labels.length; i++) {
        if (primary) {
            $("dialog > .mdl-dialog__actions").append("<button class='primary-action'></button>");
            $("dialog > .mdl-dialog__actions > button").addClass("mdl-button--accent");
        }
        else {
            $("dialog > .mdl-dialog__actions > button:last").after("<button class='secondary-action'></button>")
            $("dialog > .mdl-dialog__actions > button:last").addClass("mdl-button--primary");
        }

        $("dialog > .mdl-dialog__actions > button").addClass("mdl-button mdl-js-button mdl-button--raised mdl-js-ripple-effect");
        $("dialog > .mdl-dialog__actions > button:last").html(labels[i]);
        $("dialog > .mdl-dialog__actions > button:last").attr("onclick", actions[i]);
        primary = false;
    }

    dialog.showModal();
}