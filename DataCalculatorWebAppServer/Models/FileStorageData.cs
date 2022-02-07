using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataCalculatorWebAppServer.Models
{
    public class FileStorageData
    {
        [Key]
        [ForeignKey("FileSendData")]
        public int FileSendDataId { get; set; } // Label of the file
        public FileSendData FileSendData { get; set; } // Actual data of the file

        public string FileUri { get; set; }

    }
}
