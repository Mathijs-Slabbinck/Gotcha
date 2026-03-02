"use strict";


let isBlocked = false;

window.addEventListener("load", initialize);

function initialize() {
    const slcSignUpGenderInput = document.getElementById("signUpGenderInput");
    const inpSignUpPasswordInput = document.getElementById("signUpPasswordInput");
    const inpSignUpRepeatPasswordInput = document.getElementById("signUpRepeatPasswordInput");
    const eyeIcon1 = document.getElementById("eye-signUp-1");
    const eyeIcon2 = document.getElementById("eye-signUp-2");
    const inpSignUpBirthDayInput = document.getElementById("signUpBirthDayInput");

    slcSignUpGenderInput.addEventListener("change", function () {
        handleChangedSelection(slcSignUpGenderInput);
    });

    inpSignUpBirthDayInput.addEventListener("change", function () {
        handleBirthDayChange(inpSignUpBirthDayInput);
    });

    eyeIcon1.addEventListener("click", function () {
        showPassword(inpSignUpPasswordInput);
    });

    eyeIcon2.addEventListener("click", function () {
        showPassword(inpSignUpRepeatPasswordInput);
    });

    // Image file validation
    const inpSignUpImageInput = document.getElementById("signUpImageInput");

    inpSignUpImageInput.addEventListener("change", function () {
        var file = inpSignUpImageInput.files[0];

        if (!file) {
            return;
        }

        var result = validateImageFile(file);

        if (!result.isValid) {
            inpSignUpImageInput.value = "";
            showInfoModal("Invalid Image", result.errorMessage);
        }
    });

    // Info icons — show shared modal with data from clicked icon
    var infoIcons = document.querySelectorAll('.infoIcon');

    infoIcons.forEach(function (icon) {
        icon.addEventListener("click", function () {
            showInfoModal(icon.dataset.title, icon.dataset.info);
        });
    });
}

function showInfoModal(title, text) {
    var modalTitle = document.querySelector('#infoModal .infoModalTitle');
    var modalText = document.querySelector('#infoModal .infoModalText');

    modalTitle.textContent = title;
    modalText.textContent = text;

    var modal = new bootstrap.Modal(document.getElementById('infoModal'));
    modal.show();
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

function handleBirthDayChange(birthdayInput) {
    const guardianSection = document.getElementById("guardianEmailSection");
    const divBottomFieldsRow = document.getElementById("bottomFieldsRow");

    const birthDate = new Date(birthdayInput.value);
    const age = calculateAge(birthDate);

    if (age > 0 && age < 16) {
        guardianSection.classList.remove("d-none");
        divBottomFieldsRow.classList.remove("justify-content-center");
    }
    else {
        guardianSection.classList.add("d-none");
        divBottomFieldsRow.classList.add("justify-content-center");
    }
}

function calculateAge(birthDate) {
    const today = new Date();

    let age = today.getFullYear() - birthDate.getFullYear();
    const monthDiff = today.getMonth() - birthDate.getMonth();
    const dayDiff = today.getDate() - birthDate.getDate();

    // If the birthday hasn't happened yet this year, subtract 1
    if (monthDiff < 0 || (monthDiff === 0 && dayDiff < 0)) {
        age = age - 1;
    }

    return age;
}
