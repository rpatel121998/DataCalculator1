using System;
using System.IO;
using MongoDB.Bson;
using MongoDB.Driver;
using DataCalculatorWebAppServer.Models;

namespace DataCalculatorWebAppServer.Data
{
    public class MetaDataDbContext
    {
        MongoClient dbclient = new MongoClient("mongodb+srv://rpatel1:Yash2001@cluster0.fbfor.mongodb.net/NDVData?retryWrites=true&w=majority");

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
    }
}
