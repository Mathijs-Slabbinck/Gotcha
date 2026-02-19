"use strict";


window.addEventListener("load", initialize);

function initialize() {
    const username = document.getElementById("username").innerText;
    const title = document.getElementsByClassName("typeWriter")[0];
    typeWriter(username, title);
}

function typeWriter(text, field, speed = 80, i = 0) {
    if (i === 0) {
        field.innerHTML = '<span class="cursor"></span>';
    }
    if (i < text.length) {
        field.innerHTML = text.slice(0, i + 1) + '<span class="cursor"></span>';
        setTimeout(() => typeWriter(text, field, speed, i + 1), speed);
    }
    else {
        setTimeout(function () {
            field.innerHTML = field.innerHTML.replace('<span class="cursor"></span>', '');
        }, 1500);
    }
}
