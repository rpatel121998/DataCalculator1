using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataCalculatorWebAppServer.Models
{
    public class FileStorageData
    {
        [Key]
        [ForeignKey("FileSendData")]
        public int FileSendDataId { get; set; }
        public FileSendData FileSendData { get; set; }
        public string FileName { get; set; }

        [DataType(DataType.Url)]
        public string FileUri { get; set; }
    }
}
