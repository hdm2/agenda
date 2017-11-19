$(function () {
    var masks = ['(00) 00000-0000', '(00) 0000-00009'],
        maskBehavior = function (val, e, field, options) {
            return val.length > 14 ? masks[0] : masks[1];
        };

    $('.telefone').mask(maskBehavior, {
        onKeyPress:
            function (val, e, field, options) {
                field.mask(maskBehavior(val, e, field, options), options);
            }
    });
    
    $.extend($.validator.messages, {
        required: "Este campo &eacute; requerido.",
        remote: "Por favor, corrija este campo.",
        email: "Por favor, forne&ccedil;a um endere&ccedil;o de email v&aacute;lido.",
        url: "Por favor, forne&ccedil;a uma URL v&aacute;lida.",
        date: "Por favor, forne&ccedil;a uma data v&aacute;lida.",
        dateISO: "Por favor, forne&ccedil;a uma data v&aacute;lida (ISO).",
        number: "Por favor, forne&ccedil;a um n&uacute;mero v&aacute;lido.",
        digits: "Por favor, forne&ccedil;a somente d&iacute;gitos.",
        creditcard: "Por favor, forne&ccedil;a um cart&atilde;o de cr&eacute;dito v&aacute;lido.",
        equalTo: "Por favor, forne&ccedil;a o mesmo valor novamente.",
        extension: "Por favor, forne&ccedil;a um valor com uma extens&atilde;o v&aacute;lida.",
        maxlength: $.validator.format("Por favor, forne&ccedil;a n&atilde;o mais que {0} caracteres."),
        minlength: $.validator.format("Por favor, forne&ccedil;a ao menos {0} caracteres."),
        rangelength: $.validator.format("Por favor, forne&ccedil;a um valor entre {0} e {1} caracteres de comprimento."),
        range: $.validator.format("Por favor, forne&ccedil;a um valor entre {0} e {1}."),
        max: $.validator.format("Por favor, forne&ccedil;a um valor menor ou igual a {0}."),
        min: $.validator.format("Por favor, forne&ccedil;a um valor maior ou igual a {0}."),
        nifES: "Por favor, forne&ccedil;a um NIF v&aacute;lido.",
        nieES: "Por favor, forne&ccedil;a um NIE v&aacute;lido.",
        cifEE: "Por favor, forne&ccedil;a um CIF v&aacute;lido."
    });
});

$('*[data-bind-semmask]').each(function () {
    var entrada = $(this);
    var destino = $(entrada.attr('data-bind-semmask'));
    entrada.change(function () {
        destino.val(entrada.cleanVal());
    });
});

$('*[data-bind]').each(function () {
    var entrada = $(this);
    var destino = $(entrada.attr('data-bind'));
    entrada.change(function () {
        destino.val(entrada.cleanVal());
    });
});

$('.date-picker').datepicker({ autoclose: true }).next().on("click", function () {
    $(this).prev().focus();
});