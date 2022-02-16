using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace queryjson
{
    public class JsonQuery
    {

        public JsonQuery()
        {


        }


        public static void QueryFile(string path)
        {
            StreamReader r = new StreamReader(path);
            string jsonString = r.ReadToEnd();



            JObject nvd = JObject.Parse(jsonString);

            var cweValues =
               from c in nvd["CVE_Items"].SelectMany(i => i["cve"]["problemtype"]["problemtype_data"][0]["description"][0]).Values<string>()
               group c by c
               into g
               orderby g.Count() descending
               select new { CWE = g.Key, Count = g.Count() };

           

            

            List<string> cweNames = new List<string>();
            List<int> cweCounts = new List<int>();
            int i = 0;

            foreach (var c in cweValues)
            {
                if (c.CWE.Equals("CWE-XXX") == false && c.CWE.Equals("NVD-CWE-Other") == false && c.CWE.Equals("NVD-CWE-noinfo") == false && c.CWE.Equals("en") == false)
                {
                    Console.WriteLine(c.CWE + " - Count: " + c.Count);
                    
                    cweNames.Add(c.CWE);
                    cweCounts.Add(c.Count);
                    if (i < 9)
                        i++;
                    else
                        break;

                }

                
            }
            
            /*
            foreach (var item in cweNames)
            {
                Console.WriteLine(item);

                
            }

            foreach (var item in cweCounts)
            {
                Console.WriteLine(item);

               
            }
            */
        }
    }
}
