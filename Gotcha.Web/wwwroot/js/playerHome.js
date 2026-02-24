document.addEventListener("DOMContentLoaded", function () {
    const dayElement = document.getElementById("dayTimer");
    const hourElement = document.getElementById("hourTimer");
    const minuteElement = document.getElementById("minuteTimer");
    const secondElement = document.getElementById("secondTimer");

    // If the timer elements don't exist on the page, do nothing
    if (!dayElement || !hourElement || !minuteElement || !secondElement) {
        return;
    }

    let days = parseInt(dayElement.textContent);
    let hours = parseInt(hourElement.textContent);
    let minutes = parseInt(minuteElement.textContent);
    let seconds = parseInt(secondElement.textContent);

    // Convert everything to total seconds so we have one number to count down
    let totalSeconds = (days * 86400) + (hours * 3600) + (minutes * 60) + seconds;

    function updateTimerDisplay() {
        // If timer has reached zero, stop counting
        if (totalSeconds <= 0) {
            totalSeconds = 0;
            dayElement.textContent = "0";
            hourElement.textContent = "0";
            minuteElement.textContent = "0";
            secondElement.textContent = "0";
            clearInterval(timerInterval);
            return;
        }

        totalSeconds--;

        // Break total seconds back into days, hours, minutes, seconds
        let remainingSeconds = totalSeconds;

        let displayDays = Math.floor(remainingSeconds / 86400);
        remainingSeconds = remainingSeconds - (displayDays * 86400);

        let displayHours = Math.floor(remainingSeconds / 3600);
        remainingSeconds = remainingSeconds - (displayHours * 3600);

        let displayMinutes = Math.floor(remainingSeconds / 60);
        let displaySeconds = remainingSeconds - (displayMinutes * 60);

        dayElement.textContent = displayDays;
        hourElement.textContent = displayHours;
        minuteElement.textContent = displayMinutes;
        secondElement.textContent = displaySeconds;
    }

    let timerInterval = setInterval(updateTimerDisplay, 1000);
});
