"use strict";

var ALLOWED_EXTENSIONS = [".jpg", ".jpeg", ".png", ".webp"];
var MAX_FILE_SIZE_MB = 5;
var MAX_FILE_SIZE_BYTES = MAX_FILE_SIZE_MB * 1024 * 1024;

function validateImageFile(file) {
    if (!file) {
        return { isValid: false, errorMessage: "No file was selected." };
    }

    // Check file extension
    var fileName = file.name.toLowerCase();
    var hasValidExtension = false;

    for (var i = 0; i < ALLOWED_EXTENSIONS.length; i++) {
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
        var fileSizeMB = (file.size / (1024 * 1024)).toFixed(1);
        return {
            isValid: false,
            errorMessage: `This image is too large (${fileSizeMB} MB). The maximum file size is ${MAX_FILE_SIZE_MB} MB.`
        };
    }

    return { isValid: true, errorMessage: "" };
}
