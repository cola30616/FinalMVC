document.addEventListener('DOMContentLoaded', function () {
    new lc_select(document.querySelector('.liveCityLcSelect'), {
        // options here
        max_opts: 3,
        addit_classes: ['lcslt-light']
    });
    new lc_select(document.querySelector('.wishField'), {
        // options here
        max_opts: 3,
        addit_classes: ['lcslt-light']
    });
});