//Máscaras aplicáveis

$(function () {
    $(".cpf").mask("999.999.999-99");
    $(".tel").mask("(00) 0000-0000");
    $(".cel").mask("(00) 00000-0000");
    $(".cep").mask("00000-000");
});



/*
function aplicaMascara(objeto, funcao) {

}

//Aplicação de máscaras por classe

//cpf
var cpf = $(".mascara-cpf").html();
$(".mascara-cpf").html(mascaraCpf(cpf));

//celular
var cel = $(".mascara-tel").html();
$(".mascara-tel").html(mascaraCelular(cel));

//Funções para aplicação de máscaras ================================

//CPF:
function mascaraCpf(valor) {
    return valor.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/g, "\$1.\$2.\$3\-\$4");
}

//Telefone:
function mascaraTelefone(){

}

//Celular
function mascaraCelular(v) {
    v = v.replace(/\D/g, "");                  //Remove tudo o que não é dígito
    var l = v.length;
    v = v.replace(/^(\d{2})(\d)/g, "($1) $2"); //Coloca parênteses em volta dos dois primeiros dígitos
    if (l === 6) {
        v = v.replace(/(\d)(\d{4})$/, "$1-$2");    //Coloca hífen entre o quarto e o quinto dígitos
    }
    else if(l === 7){
        v = v.replace(/(\d)(\d{5})$/, "$1-$2");    //Coloca hífen entre o quinto e o sexto dígitos
    }
    return v;
}
*/