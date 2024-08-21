btn = document.getElementById("view-source");
window.onscroll = function() {
    if (document.body.scrollTop > 200 || document.documentElement.scrollTop > 700) {
        btn.classList.remove("is-hidden");
    } else {
        btn.classList.add("is-hidden");
    }
};
function toTop() {
    document.body.scrollTop = 0; // For Safari
    document.documentElement.scrollTop = 0; // For Chrome, Firefox, IE and Opera
}