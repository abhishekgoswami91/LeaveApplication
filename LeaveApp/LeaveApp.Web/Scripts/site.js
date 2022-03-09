$(document).ready(function () {
    //console.clear();
});

toastr.options.preventDuplicates = true;
var loader = {
    start: function () {
        $('html').find('.loader-div').remove();
        $('html').find('.loader').remove();
        $('html').append('<div class="loader-div"></div>');
        $('html').append('<div class="loader"></div>');
    },
    stop: function () {
        $('html').find('.loader-div').remove();
        $('html').find('.loader').remove();
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
                if (response.Data.ProfileImage == null) { response.Data.ProfileImage = '/Content/images/avtar-b.jpg';}
                $("#layoutProfileImage").attr('src', response.Data.ProfileImage);
                $("#profileId").attr('href', '/User/EmployeeDetail?Id=' + response.Data.ProfileId);
            } 
            else { $("#layoutProfileImage").attr('src', '/Content/images/avtar-b.jpg'); }
            loader.stop();
        },
        error: function (ex) {
            loader.stop();
        }
    });
}

var fileService = {
    getKb: function (fileData) {
        base64String = "data:image/jpeg;base64......";
        base64String = fileData;
        var stringLength = base64String.length - 'data:image/png;base64,'.length;

        var sizeInBytes = 4 * Math.ceil((stringLength / 3)) * 0.5624896334383812;
        var sizeInKb = sizeInBytes / 1000;
        return sizeInKb;
    }
}
