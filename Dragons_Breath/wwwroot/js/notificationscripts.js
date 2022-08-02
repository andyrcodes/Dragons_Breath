
function submitMarkAsRead(itemId) {
    var sectionId = "#notification-" + itemId;
    $(sectionId).hide();
    var formId = "#markAsRead-" + itemId;
    var valdata = $(formId).serialize();
    $.ajax({
        url: $(formId).attr("action"),
        type: "POST",
        dataType: 'json',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: valdata
    });
}
function submitMarkAllAsRead() {
    var valdata = $('#markAllAsReadForm').serialize();
    //to get alert popup
    $.ajax({
        url: '/Tickets/MarkAllAsRead',
        type: "POST",
        dataType: 'json',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: valdata
    });
}

$("#navbarNotification").click(function () {
    var icon = $('#dropdownIcon')
    if (icon.hasClass("fa-arrow-alt-circle-down")) {
        icon.removeClass("fa-arrow-alt-circle-down");
        icon.addClass('fa-arrow-alt-circle-up')
    }
    else
    {
        icon.removeClass('fa-arrow-alt-circle-up')
        icon.addClass("fa-arrow-alt-circle-down");
    }
})
