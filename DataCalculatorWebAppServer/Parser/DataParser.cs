using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DataCalculatorWebAppServer.Models;

namespace DataCalculatorWebAppServer.Parser
{
    public class DataParser
    {

        public List<string> cweNames = new List<string>();
        public List<int> cweCounts = new List<int>();

        public List<object> cweValues { get; set; }

        public void FormatParadise(string path)
        {
            //path = ""; //input path
            StreamReader r = new StreamReader(path);
            string nvdString = r.ReadToEnd();

            r.Close();

            File.Delete(path);

            //nvdString = nvdString.Remove(0, 15);

            string[] words = nvdString.Split('{');

            string jsonString = "{\"cve_items\":[";

            if (words[1].Contains("\"cve_items\""))
            {
                System.IO.File.WriteAllText(path, nvdString);
            }

            else
            {
                foreach (var word in words)
                {

                    //jsonString += "{" + word;
                    //jsonString = jsonString.Remove(jsonString.Length - 1, 1);

                    jsonString += ",{" + word;

                }

                jsonString += "]}";
                jsonString = jsonString.Remove(14, 3);

                string fileName = Path.GetFileNameWithoutExtension(path);

                string strResultJSON = JsonConvert.SerializeObject(jsonString);
                string newFilePath = @"C:.\Downloads\paradiseDownloads\";

                System.IO.File.WriteAllText(@"" + newFilePath + fileName + ".json", jsonString);
            }
           
        }

        public void FormatGlue(string path)
        {
            //path = ""; //input path
            StreamReader r = new StreamReader(path);
            string nvdString = r.ReadToEnd();

            //nvdString = nvdString.Remove(0, 15);

            string[] words = nvdString.Split('{');
            string jsonString = "{\"cve_items\":[";

            foreach (var word in words)
            {

                //jsonString += "{" + word;
                //jsonString = jsonString.Remove(jsonString.Length - 1, 1);

                jsonString += ",{" + word;

            }

            jsonString += "]}";
            jsonString = jsonString.Remove(14, 3);

            string fileName = Path.GetFileNameWithoutExtension(path);

            string strResultJSON = JsonConvert.SerializeObject(jsonString);
            string newFilePath = @"C:.\Downloads\glueDownloads\";

            System.IO.File.WriteAllText(@"" + newFilePath + fileName + ".json", jsonString);
        }
        public List<string[]> QueryFile(string path)
        {

            StreamReader r = new StreamReader(path);
            string jsonString = r.ReadToEnd();



            JObject nvd = JObject.Parse(jsonString);

            JArray CVE_Items = (JArray)nvd["cve_items"];



            var cweValues =
                from c in nvd["cve_items"]
                select (string)c["cwe"];

            List<string> cwes = new List<string>();

            foreach (var item in cweValues)
            {
                cwes.Add(item);
            }

            
            var cweQuery =
                from cwe in cwes
                group cwe by cwe into g
                select new { CWE = g.Key, Count = g.Count() };


            List<string> cweNames = new List<string>();
            List<int> cweCounts = new List<int>();
            List<string[]> data = new List<string[]>();
            int i = 0;

            foreach (var cwe in cweQuery)
            {
                //Console.WriteLine(cwe.CWE + " - Count: " + cwe.Count);
                cweNames.Add(cwe.CWE);
                cweCounts.Add(cwe.Count);
                data.Add(new string[]
                   {
                      cwe.CWE.ToString(),
                      cwe.Count.ToString()
                   });
                if (i < 9)
                    i++;
                else
                    break;

            }


            foreach (var item in cweNames)
            {
                Console.WriteLine(item);


            }

            foreach (var item in cweCounts)
            {
                Console.WriteLine(item);


            }

            return data;
        }
    }
}
