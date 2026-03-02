"use strict";


let isBlocked = false;

window.addEventListener("load", initialize);

function initialize() {
    const slcReasonSelector = document.getElementById("contactReason");

    slcReasonSelector.addEventListener("change", function () {
        handleChangedSelection(slcReasonSelector);
    });

    // Pre-fill from server-side TempData (used by the Gotcha page redirect)
    const preFillElement = document.getElementById("contactPreFill");
    const reasonParam = preFillElement.dataset.reason;
    const messageParam = preFillElement.dataset.message;

    if (reasonParam) {
        const options = slcReasonSelector.options;

        for (let i = 0; i < options.length; i++) {
            if (options[i].text === reasonParam) {
                slcReasonSelector.selectedIndex = i;
                handleChangedSelection(slcReasonSelector);
                break;
            }
        }
    }

    if (messageParam) {
        const messageField = document.getElementById("messageField");
        messageField.value = messageParam;
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