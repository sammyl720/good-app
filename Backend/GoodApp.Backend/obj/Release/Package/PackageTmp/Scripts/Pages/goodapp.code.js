$(document).ready(function () {
    $("td #lnkDelete").each(function () {
        $(this).click(function (e) {
            e.preventDefault();
            var thisHref = $(this).attr('href');
            bootbox.confirm("Are you sure to delete the Group Code?", function (result) {
                if (!result) {
                    e.preventDefault();
                } else {
                    window.location = thisHref;
                }
            });
        });
    });
});