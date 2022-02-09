namespace DataCalculatorWebAppServer.Models
{
    public class TransferFile // Used for S3
    {
        public string Name { get; set; }
        public byte[] Content { get; set; }
    }
}
