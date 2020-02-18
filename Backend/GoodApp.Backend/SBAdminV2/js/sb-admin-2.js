$(function() {

    $('#side-menu').metisMenu();

});

//Loads the correct sidebar on window load,
//collapses the sidebar on window resize.
// Sets the min-height of #page-wrapper to window size
$(function() {
    $(window).bind("load resize", function() {
        topOffset = 50;
        width = (this.window.innerWidth > 0) ? this.window.innerWidth : this.screen.width;
        if (width < 768) {
            $('div.navbar-collapse').addClass('collapse');
            topOffset = 100; // 2-row-menu
        } else {
            $('div.navbar-collapse').removeClass('collapse');
        }

        height = (this.window.innerHeight > 0) ? this.window.innerHeight : this.screen.height;
        height = height - topOffset;
        if (height < 1) height = 1;
        if (height > topOffset) {
            $("#page-wrapper").css("min-height", (height) + "px");
        }
    });
});

$(function() {
    $('[data-toggle="tooltip"]').tooltip();
    $('[data-toggle="popover"]').popover('toggle');
    $('select[data-multi="multiselect"]').each(function () {
        var multivals = $(this).data('multivals');
        if (multivals != undefined && multivals.length>0) {
            $(this).val(multivals.split(','));
        }
        $(this).multiselect({
            disabledIfEmpty: true,
            maxHeight: 300,
            includeSelectAllOption: true,
            allSelectedText: 'All',
            enableFiltering: true
        });
        //$.each(vals, function(key, value) {
        //    $('option[value='+value+']', $(this)).prop('selected', true);
        //});
        //$(this).multiselect('refresh');
    });
});
