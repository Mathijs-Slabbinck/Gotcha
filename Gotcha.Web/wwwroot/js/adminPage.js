var pendingAction = "";
var pendingPlayerName = "";
var pendingNotifyType = "";

document.addEventListener("DOMContentLoaded", function () {

    var confirmTitle = document.querySelector('.confirmActionTitle');
    var confirmText = document.querySelector('.confirmActionText');
    var confirmBtn = document.querySelector('.confirmActionBtn');

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
            confirmBtn.className = 'btn confirmActionBtn confirmRemoveBtn';

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

            var modal = bootstrap.Modal.getInstance(document.getElementById('confirmActionModal'));
            modal.hide();

            pendingAction = "";
            pendingPlayerName = "";
            pendingNotifyType = "";
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
