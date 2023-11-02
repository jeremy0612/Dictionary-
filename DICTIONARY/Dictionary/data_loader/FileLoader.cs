using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using Newtonsoft.Json;


namespace data_loader
{
    public abstract class FileProcessor
    {
        protected string FilePath;

        public FileProcessor(string filePath)
        {
            FilePath = filePath;
        }

        public abstract List<Word> ReadFile();
        public abstract void WriteToFile();
    }

    public class CsvFileProcessor : FileProcessor
    {
        public CsvFileProcessor(string filePath) : base(filePath)
        { }

        public override List<Word> ReadFile()
        {
            using (var reader = new StreamReader(FilePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                var records = new List<Word>();
                while (csv.Read())
                {
                    var record = new Word
                    {
                        Vocab = csv.GetField(0),
                        Meaning = csv.GetField(1),
                        Example = csv.GetField(2)
                    };
                    records.Add(record);
                }
                return records;
            }
        }


        public override void WriteToFile() // cái này để test
        {
            var data = new List<Word>
            {
                new Word { Vocab = "Mango", Meaning = "Xoai", Example = "Tao an xoai" }
            };

            using (var writer = new StreamWriter(FilePath))
            using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                csv.WriteRecords(data);
            }
        }
    }
    public class JsonFileProcessor : FileProcessor
    {
        public JsonFileProcessor(string filePath) : base(filePath)
        {}
        public override List<Word>? ReadFile()
        {
            try
            {
                using (StreamReader reader = new StreamReader(FilePath))
                {
                    string json = reader.ReadToEnd();
                    // Deserialize the JSON string into a list of Word objects.
                    List<Word> words = JsonConvert.DeserializeObject<List<Word>>(json);
                    return words;
                }
            }
            catch (IOException e)
            {
                return null;
            }
        }
        public override void WriteToFile()
        {
            var data = new Word 
            {};
            try
            {
                string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                using (StreamWriter writer = new StreamWriter(FilePath))
                {
                    writer.Write(json);
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Error writing the JSON file: " + e.Message);
            }
        }
    }
    public class TextFileProcessor : FileProcessor
    {
        public TextFileProcessor(string filePath) : base(filePath)
        {}

        public override List<Word>? ReadFile()
        {
            try
            {
                using (StreamReader reader = new StreamReader(FilePath))
                {
                    string text = reader.ReadToEnd();
                    return new List<Word> { };
                }
            }
            catch (IOException e)
            {
                return null;
            }
        }

        public override void WriteToFile()
        {
            string text = "This is a sample text.";
            try
            {
                using (StreamWriter writer = new StreamWriter(FilePath))
                {
                    writer.Write(text);
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Error writing the file: " + e.Message);
            }
        }
    }
}
