"use strict";

const ALLOWED_EXTENSIONS = [".jpg", ".jpeg", ".png", ".webp"];
const MAX_FILE_SIZE_MB = 5;
const MAX_FILE_SIZE_BYTES = MAX_FILE_SIZE_MB * 1024 * 1024;

function validateImageFile(file) {
    if (!file) {
        return { isValid: false, errorMessage: "No file was selected." };
    }

    // Check file extension
    const fileName = file.name.toLowerCase();
    let hasValidExtension = false;

    for (let i = 0; i < ALLOWED_EXTENSIONS.length; i++) {
        if (fileName.endsWith(ALLOWED_EXTENSIONS[i])) {
            hasValidExtension = true;
            break;
        }
    }

    if (!hasValidExtension) {
        return {
            isValid: false,
            errorMessage: `This file type is not supported. Please upload a JPG, PNG, or WebP image.`
        };
    }

    // Check file size
    if (file.size > MAX_FILE_SIZE_BYTES) {
        const fileSizeMB = (file.size / (1024 * 1024)).toFixed(1);
        return {
            isValid: false,
            errorMessage: `This image is too large (${fileSizeMB} MB). The maximum file size is ${MAX_FILE_SIZE_MB} MB.`
        };
    }

    return { isValid: true, errorMessage: "" };
}
