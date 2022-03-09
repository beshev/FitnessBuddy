// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

hoverShadow();
dashboardCounter();


function hoverShadow() {
    $(document).ready(() => {
        $(`.hover-shadow`).hover(hoverIn, hoverOut);
    })

    var hoverIn = (event) => {
        $(event.currentTarget).addClass("shadow");
    }

    var hoverOut = (event) => {
        $(event.currentTarget).removeClass("shadow");
    }
}

function dashboardCounter() {
    $(document).ready(function () {

        $('.counter').each(function () {
            $(this).prop('Counter', 0).animate({
                Counter: $(this).text()
            }, {
                duration: 4000,
                easing: 'swing',
                step: function (now) {
                    $(this).text(Math.ceil(now));
                }
            });
        });
    });
}