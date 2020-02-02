$(document).ready(function () {
    changeLocationIfLoggedIn();
    showLoggedInUser();

    $("#btn_register").click(async function () {
        let email = $("#txt_email").val();
        let pass = $("#txt_pass").val();
        let passre = $("#txt_passre").val();

        if (pass === passre) {
            let passHash = await sha256(pass);
            let data = {
                Email: email,
                PasswordHash: passHash,
                isCompany: false
            }
            $.ajax({
                url: "https://companyratesapi.azurewebsites.net/api/users/",
                type: 'POST',
                dataType: 'json',
                data: data,
                success: function (data) {
                    window.location = "Login.html";

                },
                error: function () {
                    console.log('error');
                    alert("error!");
                }

            })
        }
        else {
            alert("Password does not match!");
        }
    });
});