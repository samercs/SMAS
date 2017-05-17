(function ($) {
    $("input[data-type='date']").datepicker({
        "dateFormat": "yy-mm-dd"
    });
    $(".date-picker").datepicker({
        "dateFormat": "yy-mm-dd"
    });
    $(function () {
        $(".form-search").on("change", "select", function () {
            $(this).parents("form").submit();
        });
    });
})(jQuery);
//# sourceMappingURL=site.js.map