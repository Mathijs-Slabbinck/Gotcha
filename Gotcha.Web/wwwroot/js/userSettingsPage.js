document.addEventListener("DOMContentLoaded", function () {

    // Image file validation
    var settingsImageInput = document.getElementById("settingsImageInput");

    settingsImageInput.addEventListener("change", function () {
        var file = settingsImageInput.files[0];

        if (!file) {
            return;
        }

        var result = validateImageFile(file);

        if (!result.isValid) {
            settingsImageInput.value = "";
            showSettingsInfoModal("Invalid Image", result.errorMessage);
        }
    });

    // Info icons — show shared modal with data from clicked icon
    var infoIcons = document.querySelectorAll('.settingsInfoIcon');

    infoIcons.forEach(function (icon) {
        icon.addEventListener("click", function () {
            showSettingsInfoModal(icon.dataset.title, icon.dataset.info);
        });
    });

});

function showSettingsInfoModal(title, text) {
    var modalTitle = document.querySelector('#settingsInfoModal .infoModalTitle');
    var modalText = document.querySelector('#settingsInfoModal .infoModalText');

    modalTitle.textContent = title;
    modalText.textContent = text;

    var modal = new bootstrap.Modal(document.getElementById('settingsInfoModal'));
    modal.show();
}
