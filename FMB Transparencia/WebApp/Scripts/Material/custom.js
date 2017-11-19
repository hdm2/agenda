function toggleAllButtons() {
    $(":button").each(function () {
        $(this).attr("disabled", !Boolean($(this).attr("disabled")));
    });

    $("a").each(function () {
        $(this).attr("disabled", !Boolean($(this).attr("disabled")));
    });
}