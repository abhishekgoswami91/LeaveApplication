$(document).ready(function () {
    console.clear();
});

toastr.options.preventDuplicates = true;
var loader = {
    start: function () {
        $('html').find('.loader-div').remove();
        $('html').append('<div class="loader-div"><div class="loader"></div></div>');

    },
    stop: function () {
        $('html').find('.loader-div').remove();
    },
    auto: function (time) {

    }
}

var doScroll = {
    top: function () {
        var $container = $("html");
        $container.animate({
            scrollTop: 0
        }, 300); 
        //window.Animation.scrollTo(0, 0);
    },
    to: function (from, to, value) {
        var $container = $(from);
        var $scrollTo = $(to);
        $container.animate({
            scrollTop: $scrollTo.offset().top - $container.offset().top + $container.scrollTop() + value,
            scrollLeft: 0
        }, 300); 
    }
}

var getUrlParameter = function getUrlParameter(sParam) {
    var sPageURL = window.location.search.substring(1),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return typeof sParameterName[1] === undefined ? true : decodeURIComponent(sParameterName[1]);
        }
    }
    return false;
};

function getUserProfileData() {
    loader.start();
    $.ajax({
        url: '/User/GetUserProfile',
        type: "GET",
        async: true,
        success: function (response) {
            if (response.Data) {
                $("#profileImage").attr('src', response.Data.ProfileImage);
            }
            loader.stop();
        },
        error: function (ex) {
            loader.stop();
        }
    });
}