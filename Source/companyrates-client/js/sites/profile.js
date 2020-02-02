let company = {};
$(document).ready(function () {
    changeLocationIfNotLoggedIn();

    showLoggedInUser();

    let items = getStorageItems();

    let searchParams = new URLSearchParams(window.location.search);
    if (searchParams.has('userid')) {
        let param = searchParams.get('userid')
        if (param == items.UserID) {
            $("#lbl_user").text(items.Email);
            $('#btn_approve').hide();
            if (items.isCompany == true) {
                $('#my_reviews').hide();
                $("#div_pass").hide();
                $("#div_passRe").hide();

                $.ajax({
                    url: "https://companyratesapi.azurewebsites.net/api/companies",
                    type: 'GET',
                    dataType: 'json',
                    success: function (data) {
                        company = data.find(x => x.User_FK == items.UserID);
                        $('#txt_name').val(company.Name);
                        $('#txt_country').val(company.Country);
                        $('#txt_address').val(company.Address);
                        $('#txt_city').val(company.City);
                        $('#txt_website').val(company.Website);
                        $('#txt_logoUrl').val(company.LogoUrl);


                    },
                    error: function () {
                        console.log('error');
                    }
                });

            }
            else {
                $('#div_name').hide();
                $('#div_country').hide();
                $('#div_address').hide();
                $('#div_city').hide();
                $('#div_website').hide();
                $('#div_logoUrl').hide();
            }



            $.ajax({
                url: "https://companyratesapi.azurewebsites.net/api/reviews/",
                type: 'GET',
                dataType: 'json',
                success: function (data) {

                    for (let i in data) {

                        if (data[i].User_FK == items.UserID) {
                            var tablerow = `<tr><td><p>${data[i].Text}</p></td><td><p>Category:<br /><strong>${data[i].Category}</strong></p></td><td><p>Date:<br /> <strong>${data[i].DateTimeAdded}</strong> </p> </td><td> <h5> <b>Total rating:</b> </h5> <p>${data[i].TotalRating}</p> </td><td> <button OnClick="goToCompany(this);" value="${data[i].Company_FK}"class="btn"><i class="far fa-comment"></i></button> </td>  </tr>`;
                            $('#LV_reviews tbody').append(tablerow);
                        }


                    }
                },
                error: function () {
                    console.log('error');
                }
            });
        }
        else {
            $('#btn_logout').hide();
            $('#whose_reviews').text("Reviews");
            $('#div_details').hide();
            $.ajax({
                url: `https://companyratesapi.azurewebsites.net/api/Users/${param}?sessionkey=${items.SessionKey}`,
                type: 'GET',
                dataType: 'json',
                success: function (data) {
                    $("#lbl_user").text(data.Email);
                    if (data.isCompany == true) {
                        window.location = "index.html";
                    }
                    if (!data.isCompany && !data.isAdmin && items.isAdmin) {

                        user = data;
                    }

                },
                error: function () {
                    console.log('error');
                }
            });

            $.ajax({
                url: "https://companyratesapi.azurewebsites.net/api/reviews/",
                type: 'GET',
                dataType: 'json',
                success: function (data) {

                    for (let i in data) {

                        if (data[i].User_FK == param) {
                            var tablerow = `<tr><td><p>${data[i].Text}</p></td><td><p>Category:<br /><strong>${data[i].Category}</strong></p></td><td><p>Date:<br /> <strong>${data[i].DateTimeAdded}</strong> </p> </td><td> <h5> <b>Total rating:</b> </h5> <p>${data[i].TotalRating}</p> </td><td> <button OnClick="goToCompany(this);" value="${data[i].Company_FK}"class="btn"><i class="far fa-comment"></i></button> </td>  </tr>`;
                            $('#LV_reviews tbody').append(tablerow);
                        }


                    }
                },
                error: function () {
                    console.log('error');
                }
            });
        }
    }
    else {
        window.location = "index.html";
    }


    $("#btn_logout").click(function () {
        let key = {};
        key.SessionKey = items.SessionKey;

        $.ajax({
            url: "https://companyratesapi.azurewebsites.net/api/accounts/logout",
            type: 'POST',
            dataType: 'json',
            data: key,
            success: function (data) {
                sessionStorage.clear();
                window.location = "Login.html";


            },
            error: function (err) {
                console.log(err);
            }

        })
    })

    $("#btn_update").click(async function () {

        if (items.isCompany == true) {
            let name = $("#txt_name").val();
            let website = $("#txt_website").val();
            let logourl = $("#txt_logoUrl").val();
            let country = $("#txt_country").val();
            let address = $("#txt_address").val();
            let city = $("#txt_city").val();
            let object = {
                CompanyID: company.CompanyID,
                Name: name,
                Website: website,
                LogoUrl: logourl,
                Country: country,
                Address: address,
                City: city,
                Verified: company.Verified,
                TotalRating: company.TotalRating
            };
            $.ajax({
                url: `https://companyratesapi.azurewebsites.net/api/Companies/${company.CompanyID}?sessionkey=${items.SessionKey}`,
                type: 'PUT',
                dataType: 'json',
                data: object,
                success: function (data) {
                    alert("Data changed!");

                },
                error: function (err) {
                    console.log(err);
                }

            })
        }
        else {
            let pass = $("#txt_pass").val();
            let passRe = $("#txt_passRe").val();
            if (pass != "" && pass == passRe) {
                let passHash = await sha256(pass);

                let object = {
                    UserID: items.UserID,
                    PasswordHash: passHash,
                    isAdmin: items.isAdmin
                };
                $.ajax({

                    url: `https://companyratesapi.azurewebsites.net/api/Users/${items.UserID}?sessionkey=${items.SessionKey}`,
                    type: 'PUT',
                    dataType: 'json',
                    data: object,
                    success: function (data) {
                        alert("Data changed!");
                        $("#txt_pass").val("");
                        $("#txt_passRe").val("");

                    },
                    error: function (err) {
                        console.log(err);
                    }

                })
            }
            else {
                alert("Password is not the same!");
            }

        }

    })

});

function goToCompany(el) {
    var id = $(el).attr('value');
    window.location = "SelectedCompany.html?companyid=" + id;
}

