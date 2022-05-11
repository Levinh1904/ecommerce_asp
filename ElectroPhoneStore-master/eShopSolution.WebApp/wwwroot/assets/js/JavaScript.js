﻿var ChatController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
    }
        (function () {

            $("#Send").click(
                function () {

                    var message = $("#message").val();

                    var stringUrl = "api/Chat";

                    $.ajax({
                        type: "POST",
                        url: stringUrl,
                        async: false,
                        data: { message: message },

                        success: function (data) {

                            $("#displaymessage").append(" >> Me : " + message + "\n");
                            $("#displaymessage").append(data.reply + "\n");

                            $("#message").val("")
                        }
                    });


                }
            );

        });
}