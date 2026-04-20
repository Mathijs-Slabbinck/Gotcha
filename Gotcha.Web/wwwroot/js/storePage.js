document.addEventListener("DOMContentLoaded", function () {

    // ===== INFO MODAL =====
    var infoIcons = document.querySelectorAll('.storeInfoIcon');
    var modalTitle = document.querySelector('#infoModal .infoModalTitle');
    var modalText = document.querySelector('#infoModal .infoModalText');

    infoIcons.forEach(function (icon) {
        icon.addEventListener("click", function () {
            modalTitle.textContent = icon.dataset.title;
            modalText.textContent = icon.dataset.info;

            var modal = new bootstrap.Modal(document.getElementById('infoModal'));
            modal.show();
        });
    });

    // ===== PAYPAL BUTTONS =====
    var featureIds = [
        "AssassinMode",
        "ChaosMode",
        "TimedKills",
        "CustomKillMethods",
        "Lobby100",
        "Lobby150",
        "Lobby500",
        "Lobby10000"
    ];

    featureIds.forEach(function (feature) {
        var containerId = "#paypal-btn-" + feature;
        var container = document.querySelector(containerId);

        // Only render button if the container exists (feature not already owned)
        if (!container) {
            return;
        }

        paypal.Buttons({
            style: {
                layout: "horizontal",
                color: "gold",
                shape: "rect",
                label: "pay",
                height: 35,
                tagline: false
            },

            createOrder: function () {
                return fetch("/User/Store/CreateOrder", {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify({ feature: feature })
                })
                .then(function (response) {
                    if (!response.ok) {
                        throw new Error("Failed to create order.");
                    }
                    return response.json();
                })
                .then(function (data) {
                    return data.orderId;
                });
            },

            onApprove: function (data) {
                return fetch("/User/Store/CaptureOrder", {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify({
                        orderId: data.orderID,
                        feature: feature
                    })
                })
                .then(function (response) {
                    if (!response.ok) {
                        throw new Error("Failed to capture payment.");
                    }
                    return response.json();
                })
                .then(function (result) {
                    if (result.success) {
                        window.location.reload();
                    }
                });
            },

            onError: function (err) {
                console.error("PayPal error:", err);
                alert("Something went wrong with the payment. Please try again.");
            }

        }).render(containerId);
    });

});
