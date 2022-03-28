using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using DataCalculatorWebAppServer.Models;

namespace DataCalculatorWebAppServer.Parser
{
    public class DataParser
    {
        public List<string> cweNames = new List<string>();
        public List<int> cweCounts = new List<int>();

        public List<object> cweValues { get; set; }
        public List<object> QueryFile(string path)
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

            int i = 0;


            var data = new List<object>();

            foreach (var c in cweValues)
            {
                if (c.CWE.Equals("CWE-XXX") == false && c.CWE.Equals("NVD-CWE-Other") == false && c.CWE.Equals("NVD-CWE-noinfo") == false && c.CWE.Equals("en") == false)
                {
                    Console.WriteLine(c.CWE + " - Count: " + c.Count);

                    cweNames.Add(c.CWE);
                    cweCounts.Add(c.Count);

                    var name = c.CWE;
                    var count = c.Count; 
                    data.Add(new
                    {
                        name = c.CWE,
                        count = c.Count
                    });
               
                    if (i < 9)
                        i++;
                    else
                        break;

                }


            }

            return data;

        }
    }
}
