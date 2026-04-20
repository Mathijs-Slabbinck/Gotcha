"use strict";

// --- Password rules (mirrors Identity config in Program.cs) ---

const MIN_PASSWORD_LENGTH = 12;
const MIN_UNIQUE_CHARS = 4;

// --- Age rules (mirrors UserValidationService) ---

const MIN_AGE = 13;
const MAX_AGE = 150;

// --- Validation functions ---

function validatePassword(password) {
    if (!password) {
        return null;
    }

    let errors = [];

    if (password.length < MIN_PASSWORD_LENGTH) {
        errors.push("at least " + MIN_PASSWORD_LENGTH + " characters");
    }

    if (!/[0-9]/.test(password)) {
        errors.push("a digit");
    }

    if (!/[a-z]/.test(password)) {
        errors.push("a lowercase letter");
    }

    if (!/[A-Z]/.test(password)) {
        errors.push("an uppercase letter");
    }

    if (!/[^a-zA-Z0-9]/.test(password)) {
        errors.push("a special character");
    }

    // Count unique characters
    let uniqueChars = [];

    for (let i = 0; i < password.length; i++) {
        if (uniqueChars.indexOf(password[i]) === -1) {
            uniqueChars.push(password[i]);
        }
    }

    if (uniqueChars.length < MIN_UNIQUE_CHARS) {
        errors.push("at least " + MIN_UNIQUE_CHARS + " unique characters");
    }

    if (errors.length > 0) {
        return "Password must contain " + errors.join(", ") + ".";
    }

    return null;
}

function validateBirthDay(dateString) {
    if (!dateString) {
        return null;
    }

    const birthDate = new Date(dateString);

    if (isNaN(birthDate.getTime())) {
        return "Please enter a valid date.";
    }

    const today = new Date();
    today.setHours(0, 0, 0, 0);

    if (birthDate > today) {
        return "Birthday cannot be in the future.";
    }

    let age = today.getFullYear() - birthDate.getFullYear();
    const monthDiff = today.getMonth() - birthDate.getMonth();
    const dayDiff = today.getDate() - birthDate.getDate();

    if (monthDiff < 0 || (monthDiff === 0 && dayDiff < 0)) {
        age = age - 1;
    }

    if (age < MIN_AGE) {
        return "You must be at least " + MIN_AGE + " years old to sign up.";
    }

    if (age > MAX_AGE) {
        return "Please enter a valid date of birth.";
    }

    return null;
}

function validateEmail(email) {
    if (!email || email.trim().length === 0) {
        return null;
    }

    // Basic email format check (not exhaustive — backend does the real validation)
    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    if (!emailPattern.test(email.trim())) {
        return "Please enter a valid email address.";
    }

    return null;
}

// --- Main form validation ---

function validateSignUpForm(form) {
    let isValid = true;

    // Clear all previous client-side errors
    const errorSpans = form.querySelectorAll(".client-error");

    errorSpans.forEach(function (span) {
        span.textContent = "";
    });

    // Required fields
    const requiredFields = [
        { id: "signUpFirstNameInput", error: "signUpFirstNameError", message: "Please enter your first name!" },
        { id: "signUpLastNameInput", error: "signUpLastNameError", message: "Please enter your last name!" },
        { id: "signUpUsernameInput", error: "signUpUsernameError", message: "Please enter your username!" },
        { id: "signUpEmailInput", error: "signUpEmailError", message: "Please enter your email!" },
        { id: "signUpPasswordInput", error: "signUpPasswordError", message: "Please enter a password!" },
        { id: "signUpRepeatPasswordInput", error: "signUpRepeatPasswordError", message: "Please repeat your password!" },
        { id: "signUpBirthDayInput", error: "signUpBirthDayError", message: "Please enter your birthday!" },
    ];

    for (let i = 0; i < requiredFields.length; i++) {
        const field = requiredFields[i];
        const input = document.getElementById(field.id);
        const errorSpan = document.getElementById(field.error);

        if (input && errorSpan && (!input.value || input.value.trim().length === 0)) {
            errorSpan.textContent = field.message;
            isValid = false;
        }
    }

    // Gender — check if a valid option is selected
    const genderSelect = document.getElementById("signUpGenderInput");
    const genderError = document.getElementById("signUpGenderError");

    if (genderSelect && genderError) {
        if (!genderSelect.value || genderSelect.value === "") {
            genderError.textContent = "Please select your gender!";
            isValid = false;
        }
    }

    // Email validation
    const emailInput = document.getElementById("signUpEmailInput");
    const emailError = document.getElementById("signUpEmailError");

    if (emailInput && emailError && emailInput.value) {
        const emailErr = validateEmail(emailInput.value);

        if (emailErr) {
            emailError.textContent = emailErr;
            isValid = false;
        }
    }

    // Password validation
    const passwordInput = document.getElementById("signUpPasswordInput");
    const passwordError = document.getElementById("signUpPasswordError");

    if (passwordInput && passwordError && passwordInput.value) {
        const passwordErr = validatePassword(passwordInput.value);

        if (passwordErr) {
            passwordError.textContent = passwordErr;
            isValid = false;
        }
    }

    // Repeat password — must match
    const repeatPasswordInput = document.getElementById("signUpRepeatPasswordInput");
    const repeatPasswordError = document.getElementById("signUpRepeatPasswordError");

    if (repeatPasswordInput && repeatPasswordError && repeatPasswordInput.value && passwordInput.value) {
        if (repeatPasswordInput.value !== passwordInput.value) {
            repeatPasswordError.textContent = "Passwords do not match!";
            isValid = false;
        }
    }

    // Birthday validation (age)
    const birthDayInput = document.getElementById("signUpBirthDayInput");
    const birthDayError = document.getElementById("signUpBirthDayError");

    if (birthDayInput && birthDayError && birthDayInput.value) {
        const birthDayErr = validateBirthDay(birthDayInput.value);

        if (birthDayErr) {
            birthDayError.textContent = birthDayErr;
            isValid = false;
        }
    }

    // Guardian email — required if under 16
    const guardianSection = document.getElementById("guardianEmailSection");

    if (guardianSection && !guardianSection.classList.contains("d-none")) {
        const guardianInput = document.getElementById("signUpGuardianEmailInput");
        const guardianError = document.getElementById("signUpGuardianEmailError");

        if (guardianInput && guardianError) {
            if (!guardianInput.value || guardianInput.value.trim().length === 0) {
                guardianError.textContent = "Please enter a guardian email address!";
                isValid = false;
            }
            else {
                const guardianEmailErr = validateEmail(guardianInput.value);

                if (guardianEmailErr) {
                    guardianError.textContent = guardianEmailErr;
                    isValid = false;
                }
            }
        }
    }

    return isValid;
}
