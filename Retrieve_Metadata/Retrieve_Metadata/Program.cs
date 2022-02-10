using System;
using System.IO;


namespace Retrieve_Metadata
{
    class Program
    {
        static void Main(string[] args)
        {
            //path of file
            string path = @"";

            //variables for metadata components of file (name, type, size, upload date, id, year)
            string fileName;
            string type;
            DateTime uploadDate; 
            long size;
            int id;
            string year;

            //retrieves file name from path
            fileName = Path.GetFileNameWithoutExtension(path);
            Console.WriteLine("File name: {0}", fileName);
            
            //retrieves type from path
            type = Path.GetExtension(path);
            Console.WriteLine("File type:{0}", type);

            //records date and time of upload
            uploadDate = DateTime.Now; 
            Console.WriteLine("Upload date: {0}", uploadDate);

            //retrieves size of file in bytes
            size = new System.IO.FileInfo(path).Length;
            Console.WriteLine("File size: {0} bytes", size);

            //id of file
            id = 0001;
            Console.WriteLine("ID: {0}", id);

            /*
             * method that retrieves the year of the nvd file.
             * This method splits the file name into an array of strings in which the 2 position contains the year.
             * */

            void GetYear()
            {
                char[] delimiterChars = { '-' };
                string[] fileNameArray = fileName.Split(delimiterChars);
                year = fileNameArray[2];
            }

            GetYear();
            Console.WriteLine("Year: {0}", year);
        }
    }

    
}
