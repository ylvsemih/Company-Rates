$(document).ready(function () {
    let company = {};
    showLoggedInUser();
    let items = getStorageItems();

    let searchParams = new URLSearchParams(window.location.search);
    if (searchParams.has('companyid')) {
        let param = searchParams.get('companyid');
        if (items.isCompany == true) {
            $('#div_addReview').hide();
        } else if (items.isAdmin == true) {
            $('#btn_removeCompany').removeClass('hidden');
        }

        $.ajax({
            url: "https://companyratesapi.azurewebsites.net/api/companies/" + param,
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                company = data;
                $('#lbl_name').html(`<a href="Profile.html?userid=${data.User_FK}">${data.Name}</a>`);
                $('#lbl_address').text(data.Address + " " + data.City + " " + data.Country);
                $('#lbl_site').text(data.Website);
                let verified;
                if (data.Verified === true) {
                    verified = '<span class="glyphicon glyphicon-ok"></span>';
                }
                else {
                    verified = '<span class="glyphicon glyphicon-remove"></span>';
                }
                $('#lbl_verified').html(verified);
                $('#lbl_rating').text(data.TotalRating);
                $("#logo-img").attr("src", data.LogoUrl);
                if (data.Verified == false) {
                    if (items.isAdmin == true) {
                        $('#btn_approve').removeClass("hidden");

                    }
                }


                $.ajax({
                    url: "https://companyratesapi.azurewebsites.net/api/reviews/" + param,
                    type: 'GET',
                    dataType: 'json',
                    success: function (data) {

                        for (let i in data) {
                            if (items.isAdmin == true) {
                                var tablerow = `<tr><td><p>${data[i].Text}</p></td><td><p>Entered:<br/><strong><a href="Profile.html?userid=${data[i].User_FK}">${data[i].UserFK.Email}</a></strong></p></td><td><p>Category:<br /><strong>${data[i].Category}</strong></p></td><td><p>Date:<br /> <strong>${data[i].DateTimeAdded}</strong> </p> </td> <td> <button OnClick="ThumbsUpReview(this);" value="${data[i].ReviewID}" class="btn btn_thumbsup"><i class="far fa-thumbs-up"></i></button> <button OnClick="ThumbsDownReview(this);" value="${data[i].ReviewID}"class="btn btn_thumbsdown"><i class="far fa-thumbs-down"></i></button> </td> <td> <h5> <b>Total rating:</b> </h5> <p>${data[i].TotalRating}</p> </td> <td class="removeReview"> <button OnClick="RemoveReview(this);" value="${data[i].ReviewID}" class="btn"><i class="far fa-trash-alt"></i></button></td> </tr>`;

                            }
                            else {
                                var tablerow = `<tr><td><p>${data[i].Text}</p></td><td><p>Entered:<br/><strong><a href="Profile.html?userid=${data[i].User_FK}">${data[i].UserFK.Email}</a></strong></p></td><td><p>Category:<br /><strong>${data[i].Category}</strong></p></td><td><p>Date:<br /> <strong>${data[i].DateTimeAdded}</strong> </p> </td> <td> <button OnClick="ThumbsUpReview(this);" value="${data[i].ReviewID}" class="btn btn_thumbsup"><i class="far fa-thumbs-up"></i></button> <button OnClick="ThumbsDownReview(this);" value="${data[i].ReviewID}"class="btn btn_thumbsdown"><i class="far fa-thumbs-down"></i></button> </td> <td> <h5> <b>Total rating:</b> </h5> <p>${data[i].TotalRating}</p> </td></tr>`;

                            }

                            $('#table_reviews tbody').append(tablerow);
                        }
                    },
                    error: function () {
                        console.log('error');
                    }
                });

            },
            error: function () {
                console.log('error');
            }
        });

        $("#btn_compnay_thumbsup").click(function () {
            changeLocationIfNotLoggedIn();

            let companyThumbDownData = {
                User_FK: items.UserID,
                Company_FK: param,
                Value: 1
            };

            $.ajax({
                url: "https://companyratesapi.azurewebsites.net/api/votecompanies?sessionkey=" + items.SessionKey,
                type: 'POST',
                dataType: 'json',
                data: companyThumbDownData,
                success: function (data) {
                    if (data == false) {
                        alert("You have voted already!");
                    }
                    else {
                        window.location.reload(true);
                    }

                },
                error: function (err) {
                    if (err.status == 401) {
                        alert("You are not allowed to vote!");
                    }
                    console.log(err);

                }
            });

        });

        $("#btn_company_thumbsdown").click(function () {
            changeLocationIfNotLoggedIn();

            let companyThumbUpData = {
                User_FK: items.UserID,
                Company_FK: param,
                Value: -1
            };
            $.ajax({
                url: "https://companyratesapi.azurewebsites.net/api/votecompanies?sessionkey=" + items.SessionKey,
                type: 'POST',
                dataType: 'json',
                data: companyThumbUpData,
                success: function (data) {

                    if (data == false) {
                        alert("You have voted already!");
                    }
                    else {
                        window.location.reload(true);
                    }

                },
                error: function (err) {
                    if (err.status == 401) {
                        alert("You are not allowed to vote!");
                    }
                    console.log('error');

                }
            });

        });

        $("#btn_addReview").click(function () {



            let category = $('#DDL_category').val();
            let text = $("#txt_review").val();
            let user_fk = items.UserID;
            let company_fk = param;
            let date = new Date();
            let month = parseInt(date.getMonth()) + 1;
            let dateadded = date.getFullYear() + "-" + month + "-" + date.getDate() + " " + date.getHours() + ":" + date.getMinutes();

            let reviewData = {
                Category: category,
                Text: text,
                User_FK: user_fk,
                Company_FK: company_fk,
                DateTimeAdded: dateadded
            }

            $.ajax({
                url: "https://companyratesapi.azurewebsites.net/api/reviews?sessionkey=" + items.SessionKey,
                type: 'POST',
                dataType: 'json',
                data: reviewData,
                success: function (data) {
                    $("#myModal").modal('hide');

                    location.reload();

                },
                error: function (err) {
                    console.log('error');

                }
            });
        });

        $("#btn_approve").click(function () {
            if (items.isAdmin == true) {
                let object = {
                    CompanyID: company.CompanyID,
                    Name: company.Name,
                    Website: company.Website,
                    LogoUrl: company.LogoUrl,
                    Country: company.Country,
                    Address: company.Address,
                    City: company.City,
                    Verified: true,
                    TotalRating: company.TotalRating
                };
                $.ajax({
                    url: `https://companyratesapi.azurewebsites.net/api/Companies/${company.CompanyID}?sessionkey=${items.SessionKey}`,
                    type: 'PUT',
                    dataType: 'json',
                    data: object,
                    success: function (data) {
                        location.reload();
                    },
                    error: function (err) {
                        console.log(err);
                    }

                })
            }
        });

        $("#btn_removeCompany").click(function () {
            if (items.isAdmin == true) {
                $.ajax({
                    url: "https://companyratesapi.azurewebsites.net/api/companies/" + param + "?sessionkey=" + items.SessionKey,
                    type: 'DELETE',
                    dataType: 'json',

                    success: function (data) {
                        console.log(data);
                        if (data) {
                            alert("Successfuly deleted!")
                            window.location = "index.html";
                        }
                    },
                    error: function (err) {
                        console.log(err);
                    }
                });
            }
        });

        $("#btn_openModalAddReview").click(function () {
            changeLocationIfNotLoggedIn();

        });


    }
    else {
        window.location = "index.html";
    }


});

function ThumbsDownReview(el) {


    var id = $(el).attr('value');
    changeLocationIfNotLoggedIn();
    let items = getStorageItems();
    let ReviewThumbDownData = {
        User_FK: items.UserID,
        Review_FK: id,
        Value: -1
    };

    $.ajax({
        url: "https://companyratesapi.azurewebsites.net/api/votereviews?sessionkey=" + items.SessionKey,
        type: 'POST',
        dataType: 'json',
        data: ReviewThumbDownData,
        success: function (data) {
            if (data == false) {
                alert("You have voted already!");
            }
            else {
                window.location.reload(true);
            }
        },
        error: function (err) {

            if (err.status == 401) {
                alert("You are not allowed to vote!");
            }
        }
    });

}

function ThumbsUpReview(el) {


    var id = $(el).attr('value');
    changeLocationIfNotLoggedIn();
    let items = getStorageItems();
    let ReviewThumbDownData = {
        User_FK: items.UserID,
        Review_FK: id,
        Value: 1
    };

    $.ajax({
        url: "https://companyratesapi.azurewebsites.net/api/votereviews?sessionkey=" + items.SessionKey,
        type: 'POST',
        dataType: 'json',
        data: ReviewThumbDownData,
        success: function (data) {
            if (data == false) {
                alert("You have voted already!");
            }
            else {
                window.location.reload(true);
            }
        },
        error: function (err) {
            if (err.status == 401) {
                alert("You are not allowed to vote!");
            }
        }
    });
}

function RemoveReview(el) {


    var id = $(el).attr('value');
    changeLocationIfNotLoggedIn();
    let items = getStorageItems();
    if (items.isAdmin == true) {
        $.ajax({
            url: "https://companyratesapi.azurewebsites.net/api/reviews/" + id + "?sessionkey=" + items.SessionKey,
            type: 'DELETE',
            dataType: 'json',

            success: function (data) {
                console.log(data);
                if (data) {
                    alert("Successfuly deleted!")
                    location.reload();
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
    }


}