using BlazorInputFile;
using DataCalculatorWebAppServer.Models;
using DataCalculatorWebAppServer.Data;
using DataCalculatorWebAppServer.Services;
using System.Diagnostics;
using DataCalculatorWebAppServer.Parser;
using MongoDB.Driver;
using MongoDB.Bson;
using Amazon.S3.Model;


namespace DataCalculatorWebAppServer.Pages
{
    public partial class Counter
    {

        private List<FileSendData> data = new List<FileSendData>();

        private List<string> fileNames = new List<string>();

        private List<BsonDocument> uploadHistory = new List<BsonDocument>();

        private List<object> uploadTimeHistory = new List<object>();

        private List<object> gUploadTimeHistory = new List<object>();
        
        private List<TimeSpan> currentUploadTimes = new List<TimeSpan>();

        private List<TimeSpan> gCurrentUploadTimes = new List<TimeSpan>();

        private List<long> fileHistorySize = new List<long>();

        private string message;

        private string message2;

        private string UploadTimeMessage;

        private string timeToUpload;

        private long totalFileSize = 0;

        private string totalFileSizeMessage;

        private int state = 0;

        private int g_state = 0;

        private string GlueMessage;

        private string GlueMessage2;

        private string GlueMessage3;

        private string gUploadTimeMessage;

        private long gTotalFileSize  = 0;

        private string gTotalFileSizeMessage;

        private string bucketParadise = "rawjsondatanvd";

        private string bucketGlue = "seconddatacalc/read";

        private async Task HandleValidSubmit()
        {
            if (data.Count != 0)
            {
                //Stopwatch stopwatch = new Stopwatch();

                message = "Uploading to Amazon S3";

                MongoClient dbclient = new MongoClient("mongodb+srv://rpatel1:Yash2001@cluster0.fbfor.mongodb.net/NDVData?retryWrites=true&w=majority");
                var database = dbclient.GetDatabase("NDVData");
                var collection = database.GetCollection<BsonDocument>("Test1");
                var collection2 = database.GetCollection<BsonDocument>("Test2");
                //uploadHistory = await collection2.Find(new BsonDocument()).Sort(Builders<BsonDocument>.Sort.Descending("_id")).ToListAsync();
                
                for (int i = 0; i < data.Count; i++)
                {
                    var filter = Builders<BsonDocument>.Filter.Eq("file_name", data[i].FileName);
                    var result2 = collection2.Find(filter).Sort(Builders<BsonDocument>.Sort.Descending("_id")).Limit(1).ToList();
                    if (result2.Count == 0)
                    {
                        continue;
                    }

                    uploadTimeHistory.Add(subtractTime(result2[0]["end_time"].AsBsonDateTime, result2[0]["start_time"].AsBsonDateTime));
                }

                

                //stopwatch.Start();
                DateTime start = DateTime.Now;
                foreach (FileSendData datum in data)
                {
                    DateTime substart = DateTime.Now;
                    _md_context.UploadMetaData(datum);
                    await _fileHandler.UploadFileAsync(datum, bucketParadise);
                    totalFileSize += datum.Size;
                    DateTime subend = DateTime.Now;
                    _md_context.UploadMetricsMetaData(substart, subend, datum.FileName, datum.Size, bucketParadise);
                    TimeSpan ts_sub = subend.Subtract(substart);
                    currentUploadTimes.Add(ts_sub);
                }

                //stopwatch.Stop();
                DateTime end = DateTime.Now;
                TimeSpan ts = end.Subtract(start);

                
                //TimeSpan ts = stopwatch.Elapsed;
                //_md_context.UploadMetricsMetaData(start, end, fileNames);
                message = "All files are now uploaded to Paradise";
                message2 = "All Metadata of the all files are now uploaded to Paradise";

                UploadTimeMessage = $"Time to upload and process is: {ts.Minutes} minute(s) and {ts.Seconds}.{ts.Milliseconds} seconds";
                totalFileSizeMessage = $"{(float)totalFileSize / (float)1000000} MB has been uploaded";



                ChangeState(bucketParadise);
                base.StateHasChanged();
            }

            else
            {
                message = "You did not upload anything. Please try again.";
            }


        }

        private void HandleFileUpload(IFileListEntry[] file)
        {
            Random rnd = new Random();

            for (int i = 0; i < file.Length; i++)
            {
                data.Add(new FileSendData());

                if (file[i] != null)
                {
                    data[i].File = file[i];
                    data[i].Id = rnd.Next(201);
                    data[i].FileName = data[i].File.Name;
                    fileNames.Add(data[i].FileName);
                    data[i].Type = data[i].File.Type;
                    data[i].Size = data[i].File.Size;
                    data[i].Date = data[i].File.LastModified;
                    data[i].Year = data[i].GetYear();
                    data[i].Path = data[i].File.RelativePath;
                }

                else
                {
                    message = "Error: A Null file exists";
                }
            }
        }

        private void ChangeState(string stateBucket)
        {
            if (stateBucket == bucketParadise)
            {
                if (state == 0)
                {
                    state = 1;
                }
            }
            
            else
            {
                if (g_state == 0)
                {
                    g_state = 1;
                }
            }
        }

        public void onClick()
        {
            NavigationManager.NavigateTo("/fetchdata");
        }

        public void onClickGlue()
        {
            NavigationManager.NavigateTo("/fetchdata");
        }

        public TimeSpan subtractTime(BsonDateTime endTime, BsonDateTime startTime)
        {
            DateTime conv_endTime = Convert.ToDateTime(endTime);
            DateTime conv_startTime = Convert.ToDateTime(startTime);

            return conv_endTime.Subtract(conv_startTime);
        }

        private async Task HandleValidSubmitGlue()
        {
            List<string> fileYears = new List<string>();
            if (data.Count != 0)
            {
                //Stopwatch stopwatch = new Stopwatch();

                bool etlCheck = false;
                GlueMessage = "Uploading to Amazon S3";

                MongoClient dbclient = new MongoClient("mongodb+srv://rpatel1:Yash2001@cluster0.fbfor.mongodb.net/NDVData?retryWrites=true&w=majority");
                var database = dbclient.GetDatabase("NDVData");
                var collection = database.GetCollection<BsonDocument>("Test1");
                var collection4 = database.GetCollection<BsonDocument>("Test4");
                //uploadHistory = await collection2.Find(new BsonDocument()).Sort(Builders<BsonDocument>.Sort.Descending("_id")).ToListAsync();

                for (int i = 0; i < data.Count; i++)
                {
                    var filter = Builders<BsonDocument>.Filter.Eq("file_name", data[i].FileName);
                    var result4 = collection4.Find(filter).Sort(Builders<BsonDocument>.Sort.Descending("_id")).Limit(1).ToList();
                    if (result4.Count == 0)
                    {
                        gUploadTimeHistory.Add("newly uploaded");
                        continue;
                    }
                    else
                    {
                        gUploadTimeHistory.Add(subtractTime(result4[0]["end_time"].AsBsonDateTime, result4[0]["start_time"].AsBsonDateTime));
                    }
                   
                }



                //stopwatch.Start();
                DateTime start = DateTime.Now;
                foreach (FileSendData datum in data)
                {
                    DateTime substart = DateTime.Now;
                    _md_context.UploadMetaData(datum);
                    await _fileHandler.UploadFileAsync(datum, bucketGlue);
                    gTotalFileSize += datum.Size;
                    DateTime subend = DateTime.Now;
                    _md_context.UploadMetricsMetaData(substart, subend, datum.FileName, datum.Size, bucketGlue);
                    TimeSpan ts_sub = subend.Subtract(substart);
                    gCurrentUploadTimes.Add(ts_sub);
                    fileYears.Add(datum.Year);
                    //etlCheck = await ifExists(datum.Year);

                }
                string lastYear = fileYears.Last();
                etlCheck = await ifExists(lastYear);

                DateTime end = DateTime.Now;

                if (etlCheck)
                {
                    end = DateTime.Now;
                }

                
                while (etlCheck == false)
                {
                    if (etlCheck)
                    {
                        end = DateTime.Now;
                    }

                }
                //stopwatch.Stop();

                TimeSpan ts = end.Subtract(start);


                //TimeSpan ts = stopwatch.Elapsed;
                //_md_context.UploadMetricsMetaData(start, end, fileNames);
                GlueMessage = "All files are now uploaded to Glue";
                GlueMessage2 = "All Metadata of the all files are now uploaded to Glue";

                gUploadTimeMessage = $"Time to upload and process is: {ts.Minutes} minute(s) and {ts.Seconds}.{ts.Milliseconds} seconds";
                gTotalFileSizeMessage = $"{(float)gTotalFileSize / (float)1000000} MB has been uploaded";



                ChangeState(bucketGlue);
                base.StateHasChanged();
            }

            else
            {
                GlueMessage = "You did not upload anything. Please try again.";
            }


        }

        private async Task<bool> ifExists(string fileYear)
        {
            List<string> fileNames = await getGlueResults();

            List<string> newlist = fileNames.Where(item => item.Contains(fileYear)).ToList();

            GlueMessage = "Performing ETL in AWS GLue...";

            while (!newlist.Any())
            {
                //Console.WriteLine("checking...");
                fileNames = await getGlueResults();
                newlist = fileNames.Where(item => item.Contains(fileYear)).ToList();
            }

            Console.WriteLine($"{fileYear} exists!");
            GlueMessage3 = "All files have underwent ETL!";
            return true;

        }

        private async Task<List<string>> getGlueResults()
        {
            List<S3Object> li = await _fileHandler.ListObjectAsync("seconddatacalc");
            li.RemoveAt(0);

            List<string> filenames = li.Select(s => s.Key).ToList();
            return filenames;

        }
    }
}
