namespace Gotcha.Web.Areas.User.ViewModels
{
    public class CreateOrderRequest
    {
        public string Feature { get; set; } = string.Empty;
    }

    public class CaptureOrderRequest
    {
        public string Feature { get; set; } = string.Empty;
        public string OrderId { get; set; } = string.Empty;
    }
}
