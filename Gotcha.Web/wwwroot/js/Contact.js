"use strict";


let isBlocked = false;

window.addEventListener("load", initialize);

function initialize() {
    const slcReasonSelector = document.getElementById("contactReason");

    slcReasonSelector.addEventListener("change", function () {
        handleChangedSelection(slcReasonSelector);
    });
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