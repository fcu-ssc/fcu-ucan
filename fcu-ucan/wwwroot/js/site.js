﻿$(document).ready(function() {
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
});