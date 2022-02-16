using BlazorInputFile;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DataCalculatorWebAppServer.Models
{
    public class FileSendData // File Object. Helpful for Metadata
    {
        public int Id { get; set; } // Label

        public string Type { get; set; } // Type

        public string FileName { get; set; } // Name of File
        public DateTime Date { get; set; } // Date of Storage/Time of Upload
        public long Size { get; set; } // Size of Data

        public string Year { get; set; } // Year

        public string Path { get; set; } // File Path

        [NotMapped]
        public IFileListEntry File { get; set; }

        public string GetYear()
        {
            char[] delimiterChars = { '-' };
            string[] fileNameArray = FileName.Split(delimiterChars);
            fileNameArray = fileNameArray[2].Split('.');
            return fileNameArray[0];
        }
    }

   
}
