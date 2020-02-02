$(document).ready(function () {
    showLoggedInUser();
    $("#btn_sendMessege").click(async function () {
        let email = {};
        email.From = $("#txt_email").val();
        email.Reason = $("#DDL_reason").val();
        email.Text = $("#txt_text").val();

        $.ajax({
            url: "https://companyratesapi.azurewebsites.net/api/emails",
            type: 'POST',
            data: email,
            dataType: 'json',
            success: function (data) {


            },
            error: function () {
                console.log('error');
            }
        });
        alert("Messege has been sent. We will reach back to you soon!");

    });


});

