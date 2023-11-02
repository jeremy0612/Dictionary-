using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;

namespace data_loader
{
    public class MongoDb
    {
        public MongoClient client = new MongoClient();

        public MongoDb()
        {
            const string connectionUri = "mongodb+srv://Admin:j3r3my0612@cluster0.aj4lkgr.mongodb.net/?retryWrites=true&w=majority";
            var settings = MongoClientSettings.FromConnectionString(connectionUri);
            // Set the ServerApi field of the settings object to Stable API version 1
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            // Create a new client and connect to the server
            this.client = new MongoClient(settings);
            // Send a ping to confirm a successful connection
            try {
            var result = client.GetDatabase("admin").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
            Console.WriteLine("Pinged your deployment. You successfully connected to MongoDB!");
            } catch (Exception ex) {
            Console.WriteLine(ex);
            }
        }
        public List<Word> Forward()
        {
            IMongoDatabase database = this.client.GetDatabase("DictionaryDB");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("technology");

            List<BsonDocument> documents = collection.Find(new BsonDocument()).ToList();
            List<Word> words = new List<Word>();
            foreach (BsonDocument document in documents)
            {
                Word temp = new Word();
                temp.Vocab = document["vocab"].AsString;
                temp.Meaning = document["meaning"].AsString;
                temp.Example = document["example"].AsString;
                words.Add(temp);
            }
            return words;
        }

    }
    public class Word
    {
        public string? Vocab { get; set; }
        public string? Meaning { get; set; }
        public string? Example { get; set; }
    }
}
