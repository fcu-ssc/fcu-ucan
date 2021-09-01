document.querySelector('#contact-link').addEventListener('click', function(e) {
    e.preventDefault()
    Bulma().alert({
        type: 'success',
        title: '聯絡我們',
        body: '<div class="content m-3"><p class="title is-5"><i class="fas fa-map-marker-alt"></i><span class="ml-2">成就學生中心 育樂館106</span></p><p class="title is-5"><i class="far fa-clock"></i><span class="ml-2">週一至週五 8:30-17:30</span></p><p class="title is-5"><i class="fas fa-phone-alt"></i><span class="ml-2">04-24517250 ext.2271</span></p></div>',
        confirm: '關閉'
    });
});