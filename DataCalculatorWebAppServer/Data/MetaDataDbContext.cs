﻿using System;
using System.IO;
using MongoDB.Bson;
using MongoDB.Driver;
using DataCalculatorWebAppServer.Models;
using DataCalculatorWebAppServer.Pages;
using DataCalculatorWebAppServer.Parser;


namespace DataCalculatorWebAppServer.Data
{
    public class MetaDataDbContext
    {
        MongoClient dbclient = new MongoClient("mongodb+srv://rpatel1:Yash2001@cluster0.fbfor.mongodb.net/NDVData?retryWrites=true&w=majority");
        List<object> storedData = new List<object>();
        public void UploadMetaData(FileSendData file)
        {
            var database = dbclient.GetDatabase("NDVData");
            var collection = database.GetCollection<BsonDocument>("Test1");

            string path = @file.Path;

            BsonDateTime uploadDate = new BsonDateTime(DateTime.Now);

            var document = new BsonDocument
            {
                {"file_Name", file.FileName},
                {"file_type", file.Type },
                {"Time_of_Upload", uploadDate },
                {"file_size", file.Size},
                {"year", file.Year }
            };

            collection.InsertOne(document);

        }

        public void UploadMetricsMetaData(DateTime start, DateTime end)
        {
            var database = dbclient.GetDatabase("NDVData");
            var collection = database.GetCollection<BsonDocument>("Test2");

            BsonDateTime startTime = new BsonDateTime(start);
            BsonDateTime endTime = new BsonDateTime(end);

            var document = new BsonDocument
            {
                {"start_time", startTime},
                {"end_time", endTime }
            };

            collection.InsertOne(document);
        }

        public void QueryMetricsMetaData(DateTime start, DateTime end)
        {
            var database = dbclient.GetDatabase("NDVData");
            var collection = database.GetCollection<BsonDocument>("Test3");

            BsonDateTime startTime = new BsonDateTime(start);
            BsonDateTime endTime = new BsonDateTime(end);

            var document = new BsonDocument
            {
                {"start_time", startTime},
                {"end_time",  endTime},
                {"top10_vul", new BsonArray(storedData.Select(i => i.ToBsonDocument())) }
            };

            collection.InsertOne(document);
            
        }

        public void StoreQueryResults(List<object> data)
        {
            storedData = data;
        }
    }
}
