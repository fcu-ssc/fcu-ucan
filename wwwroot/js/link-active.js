$(document).ready(function() {
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
    if (path[1] === 'manage') {
        $("#manage-link").addClass('has-text-link');
    } else {
        $("#manage-link").removeClass('has-text-link');
    }
});