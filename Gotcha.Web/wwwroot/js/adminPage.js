var pendingNotifyPlayerName = "";
var pendingNotifyType = "";

document.addEventListener("DOMContentLoaded", function () {

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
            var playerName = button.dataset.player;
            var notifyType = button.dataset.type;

            pendingNotifyPlayerName = playerName;
            pendingNotifyType = notifyType;

            var titleElement = document.querySelector('.confirmNotifyTitle');
            var textElement = document.querySelector('.confirmNotifyText');

            if (notifyType === 'image') {
                titleElement.textContent = 'Send Image Request';
                textElement.textContent = 'Are you sure you want to send ' + playerName + ' a request to update their profile image?';
            }
            else {
                titleElement.textContent = 'Send Username Request';
                textElement.textContent = 'Are you sure you want to send ' + playerName + ' a request to change their username?';
            }

            var modal = new bootstrap.Modal(document.getElementById('confirmNotifyModal'));
            modal.show();
        });
    });

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

    // Toggle chaos timer when chaos mode checkbox changes
    var chaosModeCheckbox = document.getElementById('chaosMode');
    var chaosTimerInput = document.getElementById('chaosTimer');
    if (chaosModeCheckbox && chaosTimerInput) {
        chaosModeCheckbox.addEventListener("change", function () {
            if (chaosModeCheckbox.checked) {
                chaosTimerInput.disabled = false;
            }
            else {
                chaosTimerInput.disabled = true;
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

    // Confirm notify button in modal
    var confirmBtn = document.querySelector('.confirmNotifyBtn');
    if (confirmBtn) {
        confirmBtn.addEventListener("click", function () {
            // TODO: send actual request to backend
            var modal = bootstrap.Modal.getInstance(document.getElementById('confirmNotifyModal'));
            modal.hide();

            pendingNotifyPlayerName = "";
            pendingNotifyType = "";
        });
    }

});
