"use strict";


let isBlocked = false;

window.addEventListener("load", initialize);

function initialize() {
    const slcSignUpGenderInput = document.getElementById("signUpGenderInput");
    const pInfoIcons = document.getElementsByClassName("infoIcon");
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

    for (let i = 0; i < pInfoIcons.length; i++) {
        const selectedInfoIcon = pInfoIcons[i];

        selectedInfoIcon.addEventListener("click", function () {
            const infoIconId = selectedInfoIcon.id;
            const modalToShow = infoIconId.split("-")[1];
            showInfoModal(modalToShow);
        });
    }
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

function showInfoModal(modalToShow) {
    const divInfoModal = new bootstrap.Modal(document.getElementById("infoModal"));
    const infoModalTitle = document.getElementsByClassName("infoModalTitle")[0];
    const infoModalText = document.getElementsByClassName("infoModalText")[0];
    divInfoModal.show();

    switch (modalToShow) {
        case "general":
            infoModalTitle.textContent = "General";
            infoModalText.innerHTML = `We will only use this data in game.<br />None of your info will be sold and/or used outside the app.<br />All data is stored safely and your privacy is respected!`;
            break;
        case "firstName":
            infoModalTitle.textContent = "First Name";
            infoModalText.textContent = "We ask for your first name because, unless disabled, your name will be shown in game.";
            break;
        case "lastName":
            infoModalTitle.textContent = "Last Name";
            infoModalText.textContent = "We ask for your last name because, unless disabled, your name will be shown in game.";
            break;
        case "username":
            infoModalTitle.textContent = "Username";
            infoModalText.textContent = "We ask for your username because if real names are disabled, this username will be shown by default (can be changed per game).";
            break;
        case "emailAddress":
            infoModalTitle.textContent = "Email Address";
            infoModalText.innerHTML = "We ask for your email address for account verification.<br />We will not be sending you any emails.";
            break;
        case "password":
            infoModalTitle.textContent = "Password";
            infoModalText.innerHTML = "Your password is encrypted and kept safe.<br/>We take cyber security serious<br />(but we suggest using a unique password to be extra safe)!";
            break;
        case "repeatPassword":
            infoModalTitle.textContent = "Repeat Password";
            infoModalText.textContent = "We ask to repeat your password to be certain that there were no typos.";
            break;
        case "birthday":
            infoModalTitle.textContent = "Birthday";
            infoModalText.textContent = "If this in game setting is enabled, players will be able to see your age to get a better idea of their target.";
            break;
        case "gender":
            infoModalTitle.textContent = "Gender";
            infoModalText.innerHTML = "If images are disabled we will show a template<br />(different for each gender).<br/>If enabled, we will also show your gender in game to give players a better idea of their target.";
            break;
        case "picture":
            infoModalTitle.textContent = "Picture";
            infoModalText.textContent = "If enabled (enabled by default) we will show player images in game to give players an idea of what their target looks like.";
            break;
        default:
            infoModalTitle.textContent = "Error";
            infoModalText.textContent = "An error has occurred, please try again later.";
            break;
    }
}