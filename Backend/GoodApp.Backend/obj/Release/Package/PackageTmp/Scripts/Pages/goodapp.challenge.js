$(document).ready(function () {
    $("td #lnkDelete").each(function () {
        $(this).click(function (e) {
            e.preventDefault();
            var thisHref = $(this).attr('href');
            bootbox.confirm("Are you sure to delete the Challenge?", function (result) {
                if (!result) {
                    e.preventDefault();
                } else {
                    window.location = thisHref;
                }
            });
        });
    });
    if ($("#orderBy").val().toUpperCase() == "ORDER") {
        $("table.table").tableDnD({
            onDragClass: "myDragClass",
            onDrop: function (table, row) {
                var challengeId = row.id;
                var originIndex = $("#" + row.id).attr("data-id");
                var currentIndex = 0;
                var rows = table.tBodies[0].rows;
                for (var i = 0; i < rows.length; i++) {
                    if (rows[i].id == row.id) {
                        currentIndex = i + 1;
                    }
                    $("#" + rows[i].id).attr("data-id",i+1);
                }
                console.log(challengeId + "," + (originIndex - currentIndex));
                $.post("/challenge/reorder?challengeId="+challengeId + "&diff=" + (originIndex-currentIndex) + "&isAsc=" + ($("#isAsc").val().length>0?'true':'false'));
            }
        });
    }
});