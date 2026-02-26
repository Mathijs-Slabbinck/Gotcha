document.addEventListener("DOMContentLoaded", function () {

    // Info icons — show shared modal with data from clicked icon
    var infoIcons = document.querySelectorAll('.settingsInfoIcon');
    var modalTitle = document.querySelector('#settingsInfoModal .infoModalTitle');
    var modalText = document.querySelector('#settingsInfoModal .infoModalText');

    infoIcons.forEach(function (icon) {
        icon.addEventListener("click", function () {
            modalTitle.textContent = icon.dataset.title;
            modalText.textContent = icon.dataset.info;

            var modal = new bootstrap.Modal(document.getElementById('settingsInfoModal'));
            modal.show();
        });
    });

});
