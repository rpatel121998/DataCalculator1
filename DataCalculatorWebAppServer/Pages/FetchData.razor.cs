using BlazorInputFile;
using DataCalculatorWebAppServer.Models;
using DataCalculatorWebAppServer.Data;
using DataCalculatorWebAppServer.Services;
using C1.Chart;
using C1.Blazor.Chart;
using DataCalculatorWebAppServer.Parser;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Diagnostics;


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

        string errorMessage = "";
        string timeToUpload = "";
        string QueryTimeMessage = "";
        string QuerySizeMessage = "";
        string g_QueryTimeMessage = "";
        string g_QuerySizeMessage = "";

        private List<BsonDocument> queryHistory = new List<BsonDocument>();
        private List<TimeSpan> queryTimeHistory = new List<TimeSpan>();
        private List<TimeSpan> currentQueryTimes = new List<TimeSpan>();

        DateTime substart;
        DateTime subend;


        List<List<object>> displays = new List<List<object>>();
        List<string> fileNames = new List<string>();
        DataParser p = new DataParser();

        private void ParseData(List<string> filenames)
        {
            string path = @"C:.\Downloads\";
            int i = 0;
            foreach (string name in filenames)
            {
                substart = DateTime.Now;
                path += name;
                List<object> data = new List<object>();
                data = p.QueryFile(path);
                displays.Add(data);
                _md_context.StoreQueryResults(data);
                subend = DateTime.Now;
                _md_context.QueryMetricsMetaData(substart, subend, name);
                currentQueryTimes.Add(subend.Subtract(substart));
                path = @"C:.\Downloads\";
            }

            state = 1;
        }

        private async Task MetaDataQuery(string startingQueryDate, string endingQueryDate)
        {
            try
            {
                state = 0;
                message = "";
                QueryTimeMessage = "";
                errorMessage = "";

                fileNames.Clear();
                displays.Clear();

                message = "Querying Data...";
                //Stopwatch stopwatch = new Stopwatch();
                //stopwatch.Start();
                DateTime start = DateTime.Now;

                MongoClient dbclient = new MongoClient("mongodb+srv://rpatel1:Yash2001@cluster0.fbfor.mongodb.net/NDVData?retryWrites=true&w=majority");
                var database = dbclient.GetDatabase("NDVData");
                var collection = database.GetCollection<BsonDocument>("Test1");
                var collection3 = database.GetCollection<BsonDocument>("Test3");

                int span = Math.Abs(int.Parse(startingQueryDate) - int.Parse(endingQueryDate));


                int qYear;
                long fileSize = 0;
                //int queryCount = 0;

                
                // Meta Data Query goes here
                for (int i = 0; i <= span; i++)
                {
                    var filter = Builders<BsonDocument>.Filter.Eq("year", startingQueryDate);
                    var result = collection.Find(filter).ToList();
                    string name = result[result.Count - 1]["file_Name"].AsString;
                    fileSize += result[result.Count - 1]["file_size"].AsInt64;


                    fileNames.Add(name);

                    qYear = int.Parse(startingQueryDate) + 1;
                    startingQueryDate = qYear.ToString();
                   
                }

                for (int i = 0; i < fileNames.Count; i++)
                {
                    var filter = Builders<BsonDocument>.Filter.Eq("file_name", fileNames[i]);
                    var result3 = collection3.Find(filter).Sort(Builders<BsonDocument>.Sort.Descending("_id")).Limit(1).ToList();
                    queryTimeHistory.Add(subtractTime(result3[0]["end_time"].AsBsonDateTime, result3[0]["start_time"].AsBsonDateTime));
                }

                await DownloadFile(fileNames);
                ParseData(fileNames);
                DateTime end = DateTime.Now;
                TimeSpan ts = end.Subtract(start);

                message = "Data has queried";
                QueryTimeMessage = $"Time to query is: {ts.Minutes} minute(s) and {ts.Seconds}.{ts.Milliseconds} seconds";
                QuerySizeMessage = $"{fileSize / (long)1000000} MB worth of data has been queried";
            }
           
            catch (Exception ex)
            {
                message = "Date range is invalid";
            }

        }

        private async Task DownloadFile(List<string> fileList)
        {
            foreach (var fiName in fileList)
            {
                if (File.Exists($@"C:.\Downloads\{fiName}") == true)
                    continue;
                try
                {
                    var file = await _fileHandler.DownloadFileAsync(fiName);
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
            string Name = @"C:.\Downloads\";

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

        public string GetYear(int i)
        {
            char[] delimiterChars = { '-' };
            string[] fileNameArray = fileNames[i].Split(delimiterChars);
            fileNameArray = fileNameArray[2].Split('.');
            return fileNameArray[0];
        }

        public TimeSpan subtractTime(BsonDateTime endTime, BsonDateTime startTime)
        {
            DateTime conv_endTime = Convert.ToDateTime(endTime);
            DateTime conv_startTime = Convert.ToDateTime(startTime);

            return conv_endTime.Subtract(conv_startTime);
        }
    }
}
