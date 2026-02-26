"use strict";


let isBlocked = false;

window.addEventListener("load", initialize);

function initialize() {
    const slcSignUpGenderInput = document.getElementById("signUpGenderInput");
    const inpSignUpPasswordInput = document.getElementById("signUpPasswordInput");
    const inpSignUpRepeatPasswordInput = document.getElementById("signUpRepeatPasswordInput");
    const eyeIcon1 = document.getElementById("eye-signUp-1");
    const eyeIcon2 = document.getElementById("eye-signUp-2");

    slcSignUpGenderInput.addEventListener("change", function () {
        handleChangedSelection(slcSignUpGenderInput);
    });

    eyeIcon1.addEventListener("click", function () {
        showPassword(inpSignUpPasswordInput);
    });

    eyeIcon2.addEventListener("click", function () {
        showPassword(inpSignUpRepeatPasswordInput);
    });

    // Info icons — show shared modal with data from clicked icon
    var infoIcons = document.querySelectorAll('.infoIcon');
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
}

function showPassword(inputField) {
    if (inputField.type === "password") {
        inputField.type = "text";
    }
    else {
        inputField.type = "password";
    }
}

function handleChangedSelection(select) {
    if (isBlocked) {
        return;
    }

    select.remove(0);
    select.classList.remove("text-secondary");
    select.style.color = "white";
    isBlocked = true;
}
