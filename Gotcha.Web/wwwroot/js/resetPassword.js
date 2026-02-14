"use strict";


let isBlocked = false;

window.addEventListener("load", initialize);

function initialize() {
    const eyeReset1 = document.getElementById("eye-reset-1");
    const eyeReset2 = document.getElementById("eye-reset-2");

    eyeReset1.addEventListener("click", function () {
        const inpResetPasswordInput = document.getElementById("resetPassword");
        showPassword(inpResetPasswordInput);
    });

    eyeReset2.addEventListener("click", function () {
        const inpResetRepeatPasswordInput = document.getElementById("repeatResetPassword");
        showPassword(inpResetRepeatPasswordInput);
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