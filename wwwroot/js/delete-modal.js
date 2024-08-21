document.querySelector('#delete-button').addEventListener('click', function(e) {
    const modal = Bulma('#delete-modal').modal();
    modal.open();
});
document.querySelector('#cancel-button').addEventListener('click', function(e) {
    const modal = Bulma('#delete-modal').modal();
    modal.close();
});