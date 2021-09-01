document.querySelector('#upload-button-1').addEventListener('click', function(e) {
    const modal = Bulma('#upload-modal').modal();
    modal.open();
});
document.querySelector('#upload-button-2').addEventListener('click', function(e) {
    const modal = Bulma('#upload-modal').modal();
    modal.open();
});
document.querySelector('#cancel-button').addEventListener('click', function(e) {
    const modal = Bulma('#upload-modal').modal();
    modal.close();
});