// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
    $("#selectAll").click(function () {
        $("input[type=checkbox]").prop('checked', $(this).prop('checked'));
    });
    $("input[type=checkbox]").click(function () {
        if (!$(this).prop("checked")) {
            $("#selectAll").prop("checked", false);
        }
    });
});