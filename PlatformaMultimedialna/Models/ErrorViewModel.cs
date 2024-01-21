namespace PlatformaMultimedialna.Models
{
    public class ErrorViewModel
    {
        public int ID { get; set; }
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}