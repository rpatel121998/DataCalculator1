using BlazorInputFile;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataCalculatorWebAppServer.Models
{
    public class FileSendData
    {
        public int Id { get; set; } // Label

        public string Type { get; set; } // Type

        public string FileName { get; set; } // Name of File
        public DateTime Date { get; set; } // Date of Storage
        public int Size { get; set; } // Size of Data


        [NotMapped]
        public IFileListEntry File { get; set; }
    }
}
