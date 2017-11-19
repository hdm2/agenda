$body = $("body");

$(document).on({
    ajaxStart: function () { $body.addClass("loading"); },
    ajaxStop: function () { $body.removeClass("loading"); }
});

$(document).ajaxError(function (xhr, ajaxOptions, thrownError) {
    $.smallBox({
        title: "ERRO!",
        content: "<i class='fa fa-times'></i> <i>Falha ao tentar executar processamento em segundo plano.</i>",
        color: "#C46A69",
        iconSmall: "fa fa-times fa-2x fadeInRight animated",
        timeout: 4000
    });
});

function stickyFooter() {
    var mFoo = $("#footer");
    if ((($(document.body).height() + mFoo.outerHeight()) <= $(window).height())
        || ($(document.body).height() <= $(window).height())) {
        mFoo.css({ bottom: "0px" });
    } else {
        mFoo.css({ bottom: "" });
    }
}

$(document).ready(function () {
    stickyFooter();
    $(window).scroll(stickyFooter);
    $(window).resize(stickyFooter);
    $(window).load(stickyFooter);
});
