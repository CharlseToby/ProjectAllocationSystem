function sendMessage() {
    if ($(".message_input").val() !== '' || $(".message_input").val() !== null) {

        $.ajax({
            url: '/Chat/SendChat',
            type: "POST",
            data: {
                nodeId: $("#nodeId").val(),
                message: $(".message_input").val()
            },
            success: function (data) {
                message: $(".message_input").val("");
                renderMessages();
            },
            error: function () { alert('Error Sending Message'); }
        });
    }
}

function renderMessages() {
    $.ajax({
        url: '/Chat/GetChats',
        type: "GET",
        data: {
            nodeId: $("#nodeId").val()
        },
        success: function (data) {
            $(".messages").html(data);
        },
        error: function () { alert('Error Loading Messages'); }
    });
}

$(window).on('load', function () {
    renderMessages();
});