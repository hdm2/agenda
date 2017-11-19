//Este tipo de arquivo deverá conter as ações dos botões da página que não sejam a submissão de um formulário.
//Requisições AJAX deverão ser implementadas aqui.

//Contexto: PlanoDeContasController


//visualização
function visualized() {
    dialog.close();
}

//exclusão
function cancel() {
    dialog.close();
}

//exclusão de itens da lista
function confirm(urlExclusao, lineId) {
    toggleAllButtons();
    dialog.close();
    //$("#table-spinner").addClass("is-active").show("fade", 500);
    $.ajax({
        url: urlExclusao,
        type: 'POST',
        success: function () {
            showNotification("Operação realizada com sucesso!");
            //$("#filtered-table tr#" + lineId).hide("fade", 1000);

            $("#filtered-table tr#" + lineId).animate({
                backgroundColor: '#000',
                opacity: 0.0
            }, 1200, 
            function(){
                $(this).remove();
            });

            toggleAllButtons();
            //$("#table-spinner").removeClass("is-active").hide("fade", 500);
        }
    });
}

function preparaFormEdicao(changeActionTo, getDataAjaxUrl) {
    console.log("Change action to: " + changeActionTo + "\nLoad form action: " + getDataAjaxUrl);

    $("#form-principal").hide("fade", 400).attr("action", changeActionTo);
    $("#form-principal .form-title").html("Editar Plano de Contas");
    $(".mdl-card__actions > .primary-action").html("Salvar alterações");
    $(".mdl-card__actions > .secondary-action").html("Cancelar").attr("type", "reset").attr("onclick", "cancelarEdicao_Invoke()");
    $(".mdl-card__actions > .simple-link").hide();

    $.ajax({
        url: getDataAjaxUrl,
        type: 'POST',
        success: function (response) {
            console.log("AJAX: sucesso!");
            var planoEmEdicao = response.planoEmEdicao;

            //Definicao dos campos do formulário
            $("#obj_CodCli").val(planoEmEdicao.CodCli).parent().addClass("is-dirty");
            $("#obj_CodContas").val(planoEmEdicao.CodContas).parent().addClass("is-dirty");
            $("#obj_ContaReduz").val(planoEmEdicao.ContaReduz).parent().addClass("is-dirty");
            $("#cb1").val(planoEmEdicao.Analitica);
            var aux = $("#cb1").parent();
            if ($("#cb1").val() === "true")
                aux.addClass("is-checked");
            else
                aux.removeClass("is-checked");

            //rolar a página para o topo

            $("#form-principal").show("fade", 300);
        }
    });
}

function cancelarEdicao(changeActionTo) {
    $.ajax({
        success: function (response) {
            console.log("cancelarEdicao() -> Nova ação: " + changeActionTo);
            $("#form-principal").hide().attr("action", changeActionTo);
            $("#form-principal .form-title").html("Consulta Planos de Contas");
            $(".mdl-card__actions > .primary-action").html("Principal");
            $(".mdl-card__actions > .secondary-action").removeAttr("type").removeAttr("onclick").html("Listar todos");
            $(".mdl-card__actions > .simple-link").show();
            $("#form-principal input, #form-principal select").parent().removeClass("is-dirty");
            $("#form-principal").show("fade", 500);
        }
    });
}