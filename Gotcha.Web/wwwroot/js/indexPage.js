"use strict";


window.addEventListener("load", initialize);

function initialize() {
    const eye = document.getElementById("eye-login");

    eye.addEventListener("click", function () {
        const inpPasswordInput = document.getElementById("loginPasswordInput");
        showPassword(inpPasswordInput);
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