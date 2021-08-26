$(document).ready(function() {
    // Check for click events on the navbar burger icon
    $(".navbar-burger").click(function() {
        // Toggle the "is-active" class on both the "navbar-burger" and the "navbar-menu"
        $(".navbar-burger").toggleClass("is-active");
        $(".navbar-menu").toggleClass("is-active");
    });
    $(".navbar-item.has-dropdown").click(function(e) {
        if ($(".navbar-burger").is(':visible')) {
            $(this).toggleClass("is-active");
        }
    });
    $(".navbar-item > .navbar-link").click(function(e) {
        if ($(".navbar-burger").is(':visible')) {
            e.preventDefault();
        }
    });
    $(window).resize(function(e) {
        if (!$(".navbar-burger").is(':visible') && $(".navbar-item.has-dropdown.is-active").length) {
            $(".navbar-item.has-dropdown.is-active").removeClass('is-active');
        }
    });
    const path = window.location.pathname.split('/');
    if (path[1] === 'intro') {
        $("#intro-link").addClass('has-text-link');
    } else {
        $("#intro-link").removeClass('has-text-link');
    }
    if (path[2] === 'common') {
        $("#common-link").addClass('has-text-link');
    } else {
        $("#common-link").removeClass('has-text-link');
    }
    if (path[2] === 'professional') {
        $("#professional-link").addClass('has-text-link');
    } else {
        $("#professional-link").removeClass('has-text-link');
    }
    if (path[1] === 'privacy') {
        $("#privacy-link").addClass('has-text-link');
    } else {
        $("#privacy-link").removeClass('has-text-link');
    }
});