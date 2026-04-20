using Gotcha.Core.Enums;

namespace Gotcha.Core.Services.Payment
{
    public interface IPayPalService
    {
        /// <summary>
        /// Creates a PayPal order for a one-time store purchase.
        /// Returns the PayPal order ID.
        /// </summary>
        Task<string?> CreateOrder(StoreItem item);

        /// <summary>
        /// Captures (finalizes) a PayPal order after the user approves it.
        /// Returns true if the payment was successfully captured.
        /// </summary>
        Task<bool> CaptureOrder(string orderId);
    }
}
