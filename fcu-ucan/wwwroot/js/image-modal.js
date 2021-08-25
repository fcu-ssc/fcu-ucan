$(".image-modal").click(function() {
    const target = $(this).data("target");
    $("html").addClass("is-clipped");
    $(target).addClass("is-active");
});
$(".modal-close").click(function() {
    $("html").removeClass("is-clipped");
    $(this).parent().removeClass("is-active");
});