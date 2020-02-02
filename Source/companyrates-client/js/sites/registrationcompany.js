$(document).ready(function () {

    changeLocationIfLoggedIn();
    showLoggedInUser();

    $("#btn_register").click(async function () {
        let email = $("#txt_email").val();
        let pass = $("#txt_pass").val();
        let passre = $("#txt_passre").val();
        let name = $("#txt_name").val();
        let address = $("#txt_address").val();
        let website = $("#txt_website").val();
        let logourl = $("#txt_logourl").val();
        let country = $("#txt_country").val();
        let city = $("#txt_city").val();

        if (pass === passre) {
            let passHash = await sha256(pass);
            let data = {
                Email: email,
                PasswordHash: passHash,
                isCompany: true
            }
            $.ajax({
                url: "https://companyratesapi.azurewebsites.net/api/users/",
                type: 'POST',
                dataType: 'json',
                data: data,
                success: function (responseUser) {

                    $.ajax({
                        url: "https://companyratesapi.azurewebsites.net/api/accounts/login",
                        type: 'POST',
                        data: data,
                        dataType: 'json',
                        success: function (responseLogin) {

                            sessionStorage.setItem("SessionKey", responseLogin.SessionKey);
                            sessionStorage.setItem("ValidTo", responseLogin.ValidTo);
                            sessionStorage.setItem("UserID", responseLogin.User_FK);
                            sessionStorage.setItem("isAdmin", "false");
                            sessionStorage.setItem("isCompany", "true");
                            sessionStorage.setItem("Email", email.toLowerCase());


                            let companyData = {

                                Name: name,
                                Address: address,
                                City: city,
                                Country: country,
                                Website: website,
                                LogoUrl: logourl,
                                User_FK: responseUser.UserID

                            }

                            $.ajax({
                                url: "https://companyratesapi.azurewebsites.net/api/companies?sessionkey=" + responseLogin.SessionKey,
                                type: 'POST',
                                dataType: 'json',
                                data: companyData,
                                success: function (responseCompany) {


                                    window.location = "index.html";


                                },
                                error: function () {
                                    console.log('error');
                                }

                            })

                        },
                        error: function () {
                            console.log('error');
                        }
                    });




                },
                error: function () {
                    console.log('error');
                }

            })
        }
    });
});