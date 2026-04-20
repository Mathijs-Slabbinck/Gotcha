"use strict";

let isBlocked = false;
let cropper = null;
let croppedBlob = null;

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

    // Image file input — opens the crop modal
    const inpSignUpImageInput = document.getElementById("signUpImageInput");
    const cropImage = document.getElementById("cropImage");
    const cropConfirmBtn = document.getElementById("cropConfirmBtn");
    const cropModal = document.getElementById("cropModal");
    const imagePreviewContainer = document.getElementById("imagePreviewContainer");
    const imagePreview = document.getElementById("imagePreview");
    const removeImageBtn = document.getElementById("removeImageBtn");

    inpSignUpImageInput.addEventListener("change", function () {
        var file = inpSignUpImageInput.files[0];

        if (!file) {
            return;
        }

        // Client-side pre-validation (extension + size)
        var result = validateImageFile(file);

        if (!result.isValid) {
            inpSignUpImageInput.value = "";
            showInfoModal("Invalid Image", result.errorMessage);
            return;
        }

        // Load the image into the crop modal
        var reader = new FileReader();

        reader.onload = function (e) {
            cropImage.src = e.target.result;

            var modal = new bootstrap.Modal(cropModal);
            modal.show();
        };

        reader.readAsDataURL(file);
    });

    // Initialize Cropper.js when the crop modal opens
    cropModal.addEventListener("shown.bs.modal", function () {
        // Destroy previous cropper instance if it exists
        if (cropper) {
            cropper.destroy();
        }

        cropper = new Cropper(cropImage, {
            aspectRatio: 1,         // square crop (1:1)
            viewMode: 1,            // restrict crop box to canvas
            dragMode: "move",       // move the image, not the crop box
            cropBoxResizable: true,
            cropBoxMovable: true,
            autoCropArea: 0.8,      // initial crop area = 80% of the image
            responsive: true,
            background: false,
        });
    });

    // Clean up cropper when modal closes
    cropModal.addEventListener("hidden.bs.modal", function () {
        if (cropper) {
            cropper.destroy();
            cropper = null;
        }

        // Reset file input so the user can pick the same file again
        inpSignUpImageInput.value = "";
    });

    // Confirm crop — get the cropped canvas as a Blob
    cropConfirmBtn.addEventListener("click", function () {
        if (!cropper) {
            return;
        }

        var canvas = cropper.getCroppedCanvas({
            width: 1500,
            height: 1500,
            imageSmoothingEnabled: true,
            imageSmoothingQuality: "high",
        });

        canvas.toBlob(function (blob) {
            croppedBlob = blob;

            // Show the preview
            imagePreview.src = URL.createObjectURL(blob);
            imagePreviewContainer.classList.remove("d-none");

            // Close the modal
            var modal = bootstrap.Modal.getInstance(cropModal);
            modal.hide();
        }, "image/jpeg", 0.9);
    });

    // Remove image button
    removeImageBtn.addEventListener("click", function () {
        croppedBlob = null;
        imagePreviewContainer.classList.add("d-none");
        imagePreview.src = "";
        inpSignUpImageInput.value = "";
    });

    // Intercept form submission — validate client-side, then attach cropped image
    var form = document.getElementById("signUpForm");

    form.addEventListener("submit", function (e) {
        // Run client-side validation first
        if (!validateSignUpForm(form)) {
            e.preventDefault();
            return;
        }

        if (!croppedBlob) {
            // No image selected — let the form submit normally
            return;
        }

        e.preventDefault();

        var formData = new FormData(form);

        // Remove the original file input (it's empty after cropping)
        formData.delete("ProfileImage");

        // Append the cropped blob as a file
        formData.append("ProfileImage", croppedBlob, "profile.jpg");

        // Submit via fetch, then follow the response location
        fetch(form.action, {
            method: "POST",
            body: formData,
            redirect: "follow",
        }).then(function (response) {
            // Whether success (redirect to CheckEmail) or error (re-rendered form),
            // navigate to the response URL so the browser renders it normally
            window.location.href = response.url;
        });
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
