using BlazorInputFile;
using DataCalculatorWebAppServer.Models;
using DataCalculatorWebAppServer.Data;
using DataCalculatorWebAppServer.Services;
using DataCalculatorWebAppServer.Chart;
using DataCalculatorWebAppServer.Parser;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Diagnostics;
using Amazon.S3.Model;



namespace DataCalculatorWebAppServer.Pages
{
    public partial class FetchData
    {
        string startDate = "";
        string endDate = "";

        string g_startDate = "";
        string g_endDate = "";

        string message = "";
        string g_message = "";

        int state = 0;
        int g_state = 0;
        int c_state = 0;

        string errorMessage = "";
        string g_errorMessage = "";
        string timeToUpload = "";
        string QueryTimeMessage = "";
        string QuerySizeMessage = "";
        string g_QueryTimeMessage = "";
        string g_QuerySizeMessage = "";
        string db_bucket = "";

        string bucketParadise = "rawjsondatanvd";
        string bucketGlue = "seconddatacalc/write";

        private List<BsonDocument> queryHistory = new List<BsonDocument>();
        private List<object> queryTimeHistory = new List<object>();
        private List<object> g_QueryTimeHistory = new List<object>();
        private List<TimeSpan> g_currentQueryTimes = new List<TimeSpan>();
        private List<TimeSpan> currentQueryTimes = new List<TimeSpan>();
        private List<long> fileSizes = new List<long>();

        DateTime substart;
        DateTime subend;


        List<List<string[]>> displays = new List<List<string[]>>();
        List<List<string[]>> g_Displays = new List<List<string[]>>();
        List<string> fileNames = new List<string>();
        DataParser p = new DataParser();

        // Glue Variables 
        public string listObjects;


        private void ParseData(List<string> filenames, string bucketState)
        {
            string path;
            if (bucketState == bucketParadise)
            {
                path = @"C:.\Downloads\paradiseDownloads\";
                foreach (string name in filenames)
                {
                    
                    p.FormatParadise(path + name);
                    
                   
                }
            }
                
            else
            {
                path = @"C:.\Downloads\glueDownloads\";
                foreach (string name in filenames)
                {
                    string ex_name = Path.GetFileNameWithoutExtension(path + name);
                    p.FormatGlue(path + ex_name);
                }
            }
                

            int i = 0;
            foreach (string name in filenames)
            {
                substart = DateTime.Now;
                string full_path = path + name;
                List<string[]> data = new List<string[]>();
                data = p.QueryFile(full_path);
                displays.Add(data);
                
                subend = DateTime.Now;
                _md_context.QueryMetricsMetaData(substart, subend, name, bucketState);

                if (bucketState != bucketParadise)
                {
                    g_currentQueryTimes.Add(subend.Subtract(substart));
                }

                else
                {
                    currentQueryTimes.Add(subend.Subtract(substart));
                }
               
            }

            if (bucketState == bucketParadise)
            {
                state = 1;
            }

            else
            {
                g_state = 1;
            }

            if (c_state == 1)
            {
               
            }
            
            else
            {
                c_state = 1;
            }
        }

        private async Task MetaDataQuery(string startingQueryDate, string endingQueryDate, string stateBucket)
 
        {
            string tmpStartDate = startingQueryDate;
            string tmpEndDate = endingQueryDate;
            try
            {
                if (stateBucket == bucketParadise)
                {
                    state = 0;
                    message = "";
                    QueryTimeMessage = "";
                    errorMessage = "";
                    db_bucket = "Test3";
                    displays.Clear();

                }

                else
                {
                    g_state = 0;
                    g_message = "";
                    g_QueryTimeMessage = "";
                    g_errorMessage = "";
                    db_bucket = "Test5";
                    g_Displays.Clear();
                }

                

                fileNames.Clear();
               
                if (stateBucket == bucketGlue)
                {
                    g_message = "Querying Data...";
                }

                else
                {
                    message = "Querying Data...";
                }
                DateTime start = DateTime.Now;

                MongoClient dbclient = new MongoClient("mongodb+srv://rpatel1:Yash2001@cluster0.fbfor.mongodb.net/NDVData?retryWrites=true&w=majority");
                var database = dbclient.GetDatabase("NDVData");
                var collection = database.GetCollection<BsonDocument>("Test1");
                var collection3 = database.GetCollection<BsonDocument>(db_bucket);

                if (int.Parse(startingQueryDate) > int.Parse(endingQueryDate))
                {
                    string tmp_startingQueryDate = startingQueryDate;
                    startingQueryDate = endingQueryDate;
                    endingQueryDate = tmp_startingQueryDate;
                }
                    
                int span = Math.Abs(int.Parse(startingQueryDate) - int.Parse(endingQueryDate));


                int qYear;
                long fileSize = 0;
                //int queryCount = 0;

                
                // Meta Data Query goes here
                for (int i = 0; i <= span; i++)
                {
                    var filter = Builders<BsonDocument>.Filter.Eq("year", startingQueryDate);
                    var result = collection.Find(filter).Sort(Builders<BsonDocument>.Sort.Descending("_id")).Limit(1).ToList();
                    string name = result[0]["file_Name"].AsString;
                    fileSize += result[0]["file_size"].AsInt64;
                    fileSizes.Add(result[0]["file_size"].AsInt64);


                    fileNames.Add(name);

                    qYear = int.Parse(startingQueryDate) + 1;
                    startingQueryDate = qYear.ToString();
                   
                }

                for (int i = 0; i < fileNames.Count; i++)
                {
                    var filter = Builders<BsonDocument>.Filter.Eq("file_name", fileNames[i]);
                    var result3 = collection3.Find(filter).Sort(Builders<BsonDocument>.Sort.Descending("_id")).Limit(1).ToList();
                    if (stateBucket == bucketParadise)
                    {
                        if (result3.Count == 0)
                        {
                            queryTimeHistory.Add("Newly Uploaded");
                            continue;
                        }

                        else
                        {
                            queryTimeHistory.Add(subtractTime(result3[0]["end_time"].AsBsonDateTime, result3[0]["start_time"].AsBsonDateTime));
                        }
                    }

                    else
                    {
                        if (result3.Count == 0)
                        {
                            g_QueryTimeHistory.Add("Newly Uploaded");
                            continue;
                        }

                        else
                        {
                            g_QueryTimeHistory.Add(subtractTime(result3[0]["end_time"].AsBsonDateTime, result3[0]["start_time"].AsBsonDateTime));
                        }
                    }
                    
                   
                }

                if (stateBucket == bucketParadise)
                {
                    await DownloadFile(fileNames); 
                }
                    
                else
                {
                    ListObj(tmpStartDate, tmpEndDate);
                }
                   
                ParseData(fileNames, stateBucket);
                DateTime end = DateTime.Now;
                TimeSpan ts = end.Subtract(start);

                if (stateBucket == bucketParadise)
                {
                    message = "Data has queried";
                    QueryTimeMessage = $"Time to query is: {ts.Minutes} minute(s) and {ts.Seconds}.{ts.Milliseconds} seconds";
                    QuerySizeMessage = $"{(float)fileSize / (float)1000000} MB worth of data has been queried";
                }

                else
                {
                    g_message = "Data has queried";
                    g_QueryTimeMessage = $"Time to query is: {ts.Minutes} minute(s) and {ts.Seconds}.{ts.Milliseconds} seconds";
                    g_QuerySizeMessage = $"{(float)fileSize / (float)1000000} MB worth of data has been queried";
                }
                

            }

            catch (FormatException fex)
            {
                string fex_message = "Invalid input. Please try again.";

                if (stateBucket == bucketParadise)
                    message = fex_message;
                else
                    g_message = fex_message;
            }

            catch (ArgumentOutOfRangeException iex)
            {
                string iex_message = "Invalid year input. Please try again.";

                if (stateBucket == bucketParadise)
                    message = iex_message;
                else
                    g_message = iex_message;
            }
           
            catch (Exception ex)
            {
                //string e_message = "Data range is invalid";
                string exType = ex.ToString();
                string e_message = ex.Message;
                if (stateBucket == bucketParadise)
                {
                    message = exType;
                }

                else
                {
                    g_message = e_message;
                }
            }

        }

        private async Task DownloadFile(List<string> fileList) // Paradise
        {
            
            foreach (var fiName in fileList)
            {
           
                if (File.Exists($@"C:.\Downloads\paradiseDownloads\{fiName}") == true)
                    continue;
               
                try
                {
                    var file = await _fileHandler.DownloadFileAsync(fiName, bucketParadise);
                    SaveData(file);
                }

                catch
                {
                    continue;
                }
            }
        }

        protected bool SaveData(TransferFile byteFile)
        {
            BinaryWriter Writer = null;
            string Name = @"C:.\Downloads\paradiseDownloads\";


            try
            {
                Writer = new BinaryWriter(File.OpenWrite(Name + byteFile.Name));
                Writer.Write(byteFile.Content);
                Writer.Flush();
                Writer.Close();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async void ListObj(string startDate, string endDate)
        {
            string fileName;
            int queriedYear;
            int startDateInt = Int32.Parse(startDate);
            int endDateInt = Int32.Parse(endDate);
            List<string> glueFileNames = new List<string>();

            List<S3Object> li = await _fileHandler.ListObjectAsync("seconddatacalc");
            li.RemoveAt(0);

            foreach (S3Object s3 in li)
            {
                fileName = s3.Key;
                listObjects += fileName;
                string fileYear = fileName.Substring(13, 4);
                queriedYear = Int32.Parse(fileYear);
                if (queriedYear >= startDateInt && queriedYear <= endDateInt)
                {
                    glueFileNames.Add(s3.Key);
                }
                else
                {

                }
            }
            DownloadGlue(glueFileNames);
        }

        protected bool SaveData(TransferFile byteFile, string fileKey)
        {
            BinaryWriter Writer = null;
            string Name = $@"C:.\Downloads\glueDownloads\";
            byteFile.Name = fileKey.Replace("id=CVE", "nvdcve");
            byteFile.Name = byteFile.Name;

            try
            {
                Writer = new BinaryWriter(File.OpenWrite(Name + byteFile.Name));
                Writer.Write(byteFile.Content);
                Writer.Flush();
                Writer.Close();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async void DownloadGlue(List<string> glueFileNames)
        {
            string keyName;

            glueFileNames = glueFileNames.Select(s => s.Remove(0, 6)).ToList();
            foreach (string file in glueFileNames)
            {
                keyName = file.Substring(0, 11);
                string fi = file.Remove(0, 12);
                var savedFile = await _fileHandler.DownloadFileAsync(fi, $"seconddatacalc/write/{keyName}");
                SaveData(savedFile, keyName);

            }
        }

        public string GetYear(int i)
        {
            char[] delimiterChars = { '-' };
            string[] fileNameArray = fileNames[i].Split(delimiterChars);
            fileNameArray = fileNameArray[1].Split('.');
            string year = fileNameArray[0];
            return year;
        }

        public TimeSpan subtractTime(BsonDateTime endTime, BsonDateTime startTime)
        {
            DateTime conv_endTime = Convert.ToDateTime(endTime);
            DateTime conv_startTime = Convert.ToDateTime(startTime);

            return conv_endTime.Subtract(conv_startTime);
        }

    }
}
