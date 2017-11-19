function showNotification(msg) {
    $(".notification-box .box-content").html(msg);
    $(".notification-box").animate({
        opacity: 1.0,
        top: "+=40",
        height: "toggle"
    }, 1000, function () {
        setTimeout(function () {
            hideNotification();
        }, 4000);
    });
}

function hideNotification() {
    $(".notification-box").animate({
        opacity: 0.0,
        top: "-=40",
        height: "toggle"
    }, 1000, function () {
        $(".notification-box .box-content").html("");
    });
}