var pendingAction = "";
var pendingPlayerName = "";
var pendingNotifyType = "";
var pendingButton = null;

document.addEventListener("DOMContentLoaded", function () {

    var confirmTitle = document.querySelector('.confirmActionTitle');
    var confirmText = document.querySelector('.confirmActionText');
    var confirmBtn = document.querySelector('.confirmActionBtn');

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

    // Notify buttons (image & username)
    var notifyButtons = document.querySelectorAll('.notifyBtn');
    notifyButtons.forEach(function (button) {
        button.addEventListener("click", function () {
            pendingAction = "notify";
            pendingPlayerName = button.dataset.player;
            pendingNotifyType = button.dataset.type;

            if (pendingNotifyType === 'image') {
                confirmTitle.textContent = 'Send Image Request';
                confirmText.textContent = 'Are you sure you want to send ' + pendingPlayerName + ' a request to update their profile image?';
            }
            else {
                confirmTitle.textContent = 'Send Username Request';
                confirmText.textContent = 'Are you sure you want to send ' + pendingPlayerName + ' a request to change their username?';
            }

            confirmBtn.textContent = 'Yes, Send';
            confirmBtn.className = 'btn btn-1 confirmActionBtn';

            var modal = new bootstrap.Modal(document.getElementById('confirmActionModal'));
            modal.show();
        });
    });

    // Remove player buttons
    var removeButtons = document.querySelectorAll('.removePlayerBtn');
    removeButtons.forEach(function (button) {
        button.addEventListener("click", function () {
            pendingAction = "remove";
            pendingPlayerName = button.dataset.player;
            pendingNotifyType = "";

            confirmTitle.textContent = 'Remove Player';
            confirmText.textContent = 'Are you sure you want to remove ' + pendingPlayerName + ' from the game?';
            confirmBtn.textContent = 'Yes, Remove';
            confirmBtn.className = 'btn btn-1 confirmActionBtn confirmRemoveBtn';

            var modal = new bootstrap.Modal(document.getElementById('confirmActionModal'));
            modal.show();
        });
    });

    // Admin toggle buttons
    var adminToggleButtons = document.querySelectorAll('.adminToggleBtn');
    adminToggleButtons.forEach(function (button) {
        button.addEventListener("click", function () {
            pendingPlayerName = button.dataset.player;
            pendingNotifyType = "";
            pendingButton = button;

            if (button.classList.contains('addAdminBtn')) {
                pendingAction = "addAdmin";
                confirmTitle.textContent = 'Grant Admin Access';
                confirmText.textContent = `Are you sure you want to give ${pendingPlayerName} admin access?`;
                confirmBtn.textContent = 'Yes, Grant';
                confirmBtn.className = 'btn btn-1 confirmActionBtn';
            }
            else {
                pendingAction = "removeAdmin";
                confirmTitle.textContent = 'Remove Admin Access';
                confirmText.textContent = `Are you sure you want to remove admin access from ${pendingPlayerName}?`;
                confirmBtn.textContent = 'Yes, Remove';
                confirmBtn.className = 'btn btn-1 confirmActionBtn confirmRemoveBtn';
            }

            var modal = new bootstrap.Modal(document.getElementById('confirmActionModal'));
            modal.show();
        });
    });

    // Spectator toggle buttons
    var spectatorToggleButtons = document.querySelectorAll('.spectatorToggleBtn');
    spectatorToggleButtons.forEach(function (button) {
        button.addEventListener("click", function () {
            pendingPlayerName = button.dataset.player;
            pendingNotifyType = "";
            pendingButton = button;

            if (button.dataset.spectator === "false") {
                pendingAction = "enableSpectator";
                confirmTitle.textContent = 'Enable Spectator Mode';
                confirmText.textContent = `Are you sure you want to put ${pendingPlayerName} in spectator mode? They will no longer be assigned targets or participate in the game.`;
                confirmBtn.textContent = 'Yes, Enable';
                confirmBtn.className = 'btn btn-1 confirmActionBtn';
            }
            else {
                pendingAction = "disableSpectator";
                confirmTitle.textContent = 'Disable Spectator Mode';
                confirmText.textContent = `Are you sure you want to take ${pendingPlayerName} out of spectator mode? They will be included in the game again.`;
                confirmBtn.textContent = 'Yes, Disable';
                confirmBtn.className = 'btn btn-1 confirmActionBtn';
            }

            var modal = new bootstrap.Modal(document.getElementById('confirmActionModal'));
            modal.show();
        });
    });

    // Confirm action button in modal
    if (confirmBtn) {
        confirmBtn.addEventListener("click", function () {
            if (pendingAction === "notify") {
                // TODO: send actual notify request to backend
            }
            else if (pendingAction === "remove") {
                // TODO: send actual remove request to backend
            }
            else if (pendingAction === "addAdmin") {
                // TODO: send actual add admin request to backend
                if (pendingButton) {
                    pendingButton.textContent = 'remove admin';
                    pendingButton.title = 'Remove admin access';
                    pendingButton.classList.remove('addAdminBtn');
                    pendingButton.classList.add('removeAdminBtn');
                }
            }
            else if (pendingAction === "removeAdmin") {
                // TODO: send actual remove admin request to backend
                if (pendingButton) {
                    pendingButton.textContent = 'add admin';
                    pendingButton.title = 'Grant admin access';
                    pendingButton.classList.remove('removeAdminBtn');
                    pendingButton.classList.add('addAdminBtn');
                }
            }
            else if (pendingAction === "enableSpectator") {
                // TODO: send actual enable spectator request to backend
                if (pendingButton) {
                    pendingButton.innerHTML = 'spectator: <span class="spectatorOnText">on</span>';
                    pendingButton.title = 'Remove spectator mode';
                    pendingButton.dataset.spectator = 'true';
                }
            }
            else if (pendingAction === "disableSpectator") {
                // TODO: send actual disable spectator request to backend
                if (pendingButton) {
                    pendingButton.textContent = 'spectator: off';
                    pendingButton.title = 'Enable spectator mode';
                    pendingButton.dataset.spectator = 'false';
                }
            }

            var modal = bootstrap.Modal.getInstance(document.getElementById('confirmActionModal'));
            modal.hide();

            pendingAction = "";
            pendingPlayerName = "";
            pendingNotifyType = "";
            pendingButton = null;
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
