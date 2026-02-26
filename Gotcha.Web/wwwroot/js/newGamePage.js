document.addEventListener("DOMContentLoaded", function () {

    // Setting info icons — show shared modal with data from clicked icon
    var settingInfoIcons = document.querySelectorAll('.settingInfoIcon');
    var settingsModalTitle = document.querySelector('#settingsInfoModal .infoModalTitle');
    var settingsModalText = document.querySelector('#settingsInfoModal .infoModalText');

    settingInfoIcons.forEach(function (icon) {
        icon.addEventListener("click", function () {
            settingsModalTitle.textContent = icon.dataset.title;
            settingsModalText.textContent = icon.dataset.info;

            var modal = new bootstrap.Modal(document.getElementById('settingsInfoModal'));
            modal.show();
        });
    });

    // Copy invite link button
    var copyBtn = document.querySelector('.copyLinkBtn');
    if (copyBtn) {
        copyBtn.addEventListener("click", function () {
            var linkInput = document.querySelector('.inviteLinkInput');
            linkInput.select();
            navigator.clipboard.writeText(linkInput.value);

            var originalText = copyBtn.textContent;
            copyBtn.textContent = 'Copied!';

            setTimeout(function () {
                copyBtn.textContent = originalText;
            }, 2000);
        });
    }

    // Show real names and show usernames cannot both be off at the same time
    var showRealNamesCheckbox = document.getElementById('showRealNames');
    var showUsernamesCheckbox = document.getElementById('showUsernames');
    if (showRealNamesCheckbox && showUsernamesCheckbox) {
        showRealNamesCheckbox.addEventListener("change", function () {
            if (!showRealNamesCheckbox.checked && !showUsernamesCheckbox.checked) {
                showUsernamesCheckbox.checked = true;
            }
        });

        showUsernamesCheckbox.addEventListener("change", function () {
            if (!showUsernamesCheckbox.checked && !showRealNamesCheckbox.checked) {
                showRealNamesCheckbox.checked = true;
            }
        });
    }

    // When show living player names is toggled on, also toggle on show living player names to dead
    var showLivingPlayerNamesCheckbox = document.getElementById('showLivingPlayerNames');
    var showLivingPlayerNamesToDeadCheckbox = document.getElementById('showLivingPlayerNamesToDead');
    if (showLivingPlayerNamesCheckbox && showLivingPlayerNamesToDeadCheckbox) {
        showLivingPlayerNamesCheckbox.addEventListener("change", function () {
            if (showLivingPlayerNamesCheckbox.checked) {
                showLivingPlayerNamesToDeadCheckbox.checked = true;
            }
        });
    }

    // When Enforce Player Images is toggled on, disable Show Gender (everyone has an image, so gender placeholders are irrelevant)
    var enforcePlayerImagesCheckbox = document.getElementById('enforcePlayerImages');
    var showGenderCheckbox = document.getElementById('showGender');
    if (enforcePlayerImagesCheckbox && showGenderCheckbox) {
        // Set initial state on page load
        if (enforcePlayerImagesCheckbox.checked) {
            showGenderCheckbox.checked = false;
            showGenderCheckbox.disabled = true;
        }

        enforcePlayerImagesCheckbox.addEventListener("change", function () {
            if (enforcePlayerImagesCheckbox.checked) {
                showGenderCheckbox.checked = false;
                showGenderCheckbox.disabled = true;
            }
            else {
                showGenderCheckbox.disabled = false;
            }
        });
    }

    // Toggle kill methods input when custom kill methods checkbox changes
    var customKillMethodsCheckbox = document.getElementById('customKillMethods');
    var killMethodsInput = document.getElementById('killMethods');
    if (customKillMethodsCheckbox && killMethodsInput) {
        customKillMethodsCheckbox.addEventListener("change", function () {
            if (customKillMethodsCheckbox.checked) {
                killMethodsInput.disabled = false;
            }
            else {
                killMethodsInput.disabled = true;
            }
        });
    }

    // Toggle Show Hunter when Assassin Mode checkbox changes
    var assassinModeCheckbox = document.getElementById('assassinMode');
    var showHunterCheckbox = document.getElementById('showHunter');
    if (assassinModeCheckbox && showHunterCheckbox) {
        assassinModeCheckbox.addEventListener("change", function () {
            if (assassinModeCheckbox.checked) {
                showHunterCheckbox.disabled = false;
            }
            else {
                showHunterCheckbox.checked = false;
                showHunterCheckbox.disabled = true;
            }
        });
    }

    // Toggle chaos timer inputs when chaos mode checkbox changes
    var chaosModeCheckbox = document.getElementById('chaosMode');
    var chaosTimerMinInput = document.getElementById('chaosTimerMin');
    var chaosTimerMaxInput = document.getElementById('chaosTimerMax');
    if (chaosModeCheckbox && chaosTimerMinInput && chaosTimerMaxInput) {
        chaosModeCheckbox.addEventListener("change", function () {
            if (chaosModeCheckbox.checked) {
                chaosTimerMinInput.disabled = false;
                chaosTimerMaxInput.disabled = false;
            }
            else {
                chaosTimerMinInput.disabled = true;
                chaosTimerMaxInput.disabled = true;
            }
        });
    }

    // Toggle target timeout when timed kills checkbox changes
    var timedKillsCheckbox = document.getElementById('timedKills');
    var targetTimeoutInput = document.getElementById('targetTimeout');
    if (timedKillsCheckbox && targetTimeoutInput) {
        timedKillsCheckbox.addEventListener("change", function () {
            if (timedKillsCheckbox.checked) {
                targetTimeoutInput.disabled = false;
            }
            else {
                targetTimeoutInput.disabled = true;
            }
        });
    }

});
