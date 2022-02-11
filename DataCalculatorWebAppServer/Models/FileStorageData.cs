using System.ComponentModel.DataAnnotations;

namespace DataCalculatorWebAppServer.Models
{
    public class FileStorageData
    {
        [Key]
        [ForeignKey("FileSendData")]
        public int FileSendDataId { get; set; }
        public FileSendData FileSendData { get; set; }

        [DataType(DataType.Url)]
        public string FileUri { get; set; }
    }
}
