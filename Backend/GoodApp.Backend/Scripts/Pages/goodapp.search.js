$(document).ready(function () {
   $("#btnSearch").click(function (e) {
        e.preventDefault();
        var text = $("#txtSearch").val();
        var searchUrl = $("#hidSearchUrl").val();
        if (text == '') {
            location.href = searchUrl.replace("{search}", "");
        } else {
            location.href = searchUrl.replace("{search}", text.replace(/[^\w]+/g, ""));
        }
    });
});