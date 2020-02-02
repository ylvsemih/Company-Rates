$(document).ready(function () {

    let companies = [];
    showLoggedInUser();
    $.ajax({
        url: "https://companyratesapi.azurewebsites.net/api/companies",
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            let num = 1;
            for (let i in data) {
                let verified = '';
                companies = data;
                if (data[i].Verified === true) {
                    verified = '<span class="glyphicon glyphicon-ok"></span>';
                }
                else {
                    verified = '<span class="glyphicon glyphicon-remove"></span>';
                }
                var tablerow = '<tr><th scope="row">' + num + '</th><td>' + data[i].Name + '</td><td>' + verified + '</td><td><a href="' + data[i].Website + '">' + data[i].Website + '</a></td><td>' + data[i].TotalRating + '</td><td><a href="SelectedCompany.html?companyid=' + data[i].CompanyID + '"><i class="far fa-comments"></i></a></td></tr>';

                $('#table_companies tbody').append(tablerow);
                num++;
            }

        },
        error: function () {
            console.log('error');
        }
    });

    $("#btn_search").click(function () {
        let searchString = $("#txt_search").val();
        let data = [];
        for (var i in companies) {
            if (companies[i].Name.includes(searchString)) {
                data.push(companies[i]);
            }
        }

        $("#table_companies tbody tr").remove();
        let num = 1;
        for (let i in data) {
            let verified = '';
            if (data[i].Verified === true) {
                verified = '<span class="glyphicon glyphicon-ok"></span>';
            }
            else {
                verified = '<span class="glyphicon glyphicon-remove"></span>';
            }
            var tablerow = '<tr><th scope="row">' + num + '</th><td>' + data[i].Name + '</td><td>' + verified + '</td><td><a href="' + data[i].Website + '">' + data[i].Website + '</a></td><td>' + data[i].TotalRating + '</td><td><a href="SelectedCompany.html?companyid=' + data[i].CompanyID + '"><i class="far fa-comments"></i></a></td></tr>';

            $('#table_companies tbody').append(tablerow);
            num++;
        }
    });
});


