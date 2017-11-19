function ajax(url, successFunction) {
    console.log("GenericAjax()");
    $.ajax({
        url: url,
        type: 'POST',
        success: successFunction
    });
}