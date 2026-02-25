document.addEventListener("DOMContentLoaded", function () {

    var infoIcons = document.querySelectorAll('.storeInfoIcon');
    var modalTitle = document.querySelector('#infoModal .infoModalTitle');
    var modalText = document.querySelector('#infoModal .infoModalText');

    infoIcons.forEach(function (icon) {
        icon.addEventListener("click", function () {
            modalTitle.textContent = icon.dataset.title;
            modalText.textContent = icon.dataset.info;

            var modal = new bootstrap.Modal(document.getElementById('infoModal'));
            modal.show();
        });
    });

});
