namespace Gotcha.Core.Services.Payment
{
    public class PayPalSettings
    {
        public string ClientId { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = "https://api-m.sandbox.paypal.com";
    }
}
