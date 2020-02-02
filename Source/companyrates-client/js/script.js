$(document).ready(function () {

    $('.star').on('click', function () {
        $(this).toggleClass('star-checked');
    });

    $('.ckbox label').on('click', function () {
        $(this).parents('tr').toggleClass('selected');
    });

    $('.btn-filter').on('click', function () {
        var $target = $(this).data('target');
        if ($target != 'all') {
            $('.table tr').css('display', 'none');
            $('.table tr[data-status="' + $target + '"]').fadeIn('slow');
        } else {
            $('.table tr').css('display', 'none').fadeIn('slow');
        }
    });
    $("#btn_openModalAddReview").click(function () {
        var id = $('#MyID').val();
        if (id == "0") {
            window.location.replace("Login.html");
        }
        else {
            $("#myModal").modal('show');
        }


    });


    $('#myModal').on('shown.bs.modal', function (e) {
        var id = $('#MyID').val();
        if (id == "0") {
        }
        else {
            $("#CB_anonymous").removeAttr("disabled");
        }
    })





});

