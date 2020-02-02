$(document).ready(function () {

    changeLocationIfLoggedIn();
    showLoggedInUser();

    $("#btn_login").click(async function () {
        let passHash = await sha256($("#txt_pass").val());
        let user = {};
        user.email = $("#txt_email").val();
        user.passwordHash = passHash;

        $.ajax({
            url: "https://companyratesapi.azurewebsites.net/api/accounts/login",
            type: 'POST',
            data: user,
            dataType: 'json',
            success: function (data) {
                console.log(data);
                window.location = "index.html";
                sessionStorage.setItem("SessionKey", data.SessionKey);
                sessionStorage.setItem("ValidTo", data.ValidTo);
                sessionStorage.setItem("UserID", data.User_FK);
                sessionStorage.setItem("isAdmin", data.UserFK.isAdmin);
                sessionStorage.setItem("isCompany", data.UserFK.isCompany);
                sessionStorage.setItem("Email", data.UserFK.Email);

            },
            error: function () {
                console.log('error');
                alert("User does not exists!");
            }
        });
    });



    // TO-DO:
    // SEARCH BY STRING

});

