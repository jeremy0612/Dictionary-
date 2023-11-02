using System;
using System.IO;
using data_loader;
using algorithm;
using System.Collections.Generic;

namespace Program
{
    public class WordNode
    {
        public string word;
        public string meaning;
        public string example;
        public int occurence;
        public WordNode? left;
        public WordNode? right;

        public WordNode(string word, string meaning, string example)
        {
            this.word = word;
            this.meaning = meaning;
            this.example = example;
            this.left = null;
            this.right = null;
        }
        public WordNode(string word)
        {
            this.word = word;
            this.left = null;
            this.right = null;
        }
    }

    public class Dict
    {
        public WordNode? root;

        public Dict()
        {
            this.root = null;
        }
        public void Add(string word, string meaning, string example)
        {
            WordNode newNode = new WordNode(word, meaning, example);

            if (root == null)
            {
                root = newNode;
            }
            else
            {
                WordNode currentNode = root;

                while (currentNode != null)
                {
                    if (string.Compare(word, currentNode.word) < 0)
                    {
                        if (currentNode.left == null)
                        {
                            currentNode.left = newNode;
                            break;
                        }
                        else
                        {
                            currentNode = currentNode.left;
                        }
                    }
                    else if (string.Compare(word, currentNode.word) > 0)
                    {
                        if (currentNode.right == null)
                        {
                            currentNode.right = newNode;
                            break;
                        }
                        else
                        {
                            currentNode = currentNode.right;
                        }
                    }
                    else
                    {
                        // If the word already exists, replace its meaning and example
                        currentNode.meaning = meaning;
                        currentNode.example = example;
                        break;
                    }
                }
            }
        }

        public void Delete(string word)
        {
            root = DeleteRec(root, word);
        }

        private WordNode? DeleteRec(WordNode? currentNode, string word)
        {
            if (currentNode == null)
            {
                return null;
            }
            if (string.Compare(word, currentNode.word) < 0)
            {
                currentNode.left = DeleteRec(currentNode.left, word);
            }
            else if (string.Compare(word, currentNode.word) > 0)
            {
                currentNode.right = DeleteRec(currentNode.right, word);
            }
            else
            {
                if (currentNode.left == null && currentNode.right == null)
                {
                    currentNode = null;
                }
                else if (currentNode.left == null)
                {
                    currentNode = currentNode.right;
                }
                else if (currentNode.right == null)
                {
                    currentNode = currentNode.left;
                }
                else
                {
                    WordNode minRightNode = FindMinNode(currentNode.right);
                    currentNode.word = minRightNode.word;
                    currentNode.meaning = minRightNode.meaning;
                    currentNode.example = minRightNode.example;
                    currentNode.right = DeleteRec(currentNode.right, minRightNode.word);
                }
            }
            return currentNode;
        }

        public void SaveToFile(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                SaveNodeToFile(root, writer);
            }
        }

        private void SaveNodeToFile(WordNode node, StreamWriter writer)
        {
            if (node == null)
            {
                return;
            }

            SaveNodeToFile(node.left, writer);
            writer.WriteLine(
                String.Format("{0}:{1}:{2}", node.word, node.meaning, node.example)
            );
            SaveNodeToFile(node.right, writer);
        }

        public string LoadFromFile(string fileName,string mode)
        {
            string display = "List Vocabulary:\n\n\n";
            if(mode == "csv")
            {
                CsvFileProcessor reader = new CsvFileProcessor(fileName);
                List<Word> words = reader.ReadFile();
                for(int i = 0; i < words.Count; i++)
                {
                    string word = words[i].Vocab;
                    string meaning = words[i].Meaning;
                    string example = words[i].Example;
                    display += "====================================================================================================================================================================\n";
                    display += "Vocab: " + word + "\nMeaning: " + meaning + "\nExample: " + example + "\n\n";
                    Add(word, meaning, example);
                }
            }
            else if(mode == "json")
            {
                JsonFileProcessor reader = new JsonFileProcessor(fileName);
                List<Word>? words = reader.ReadFile();
                for(int i = 0; i < words.Count; i++)
                {
                    string word = words[i].Vocab;
                    string meaning = words[i].Meaning;
                    string example = words[i].Example;
                    display += "====================================================================================================================================================================\n";
                    display += "Vocab: " + word + "\nMeaning: " + meaning + "\nExample: " + example + "\n\n";
                    Add(word, meaning, example);
                }
            }
            return display;
        }
        public string LoadFromMongo()
        {
            string display = "List Vocabulary:\n";
            MongoDb source = new MongoDb();
            List<Word> data = source.Forward();
            foreach (Word word in data)
            {
                string vocab = word.Vocab;
                string meaning =word.Meaning;
                string example = word.Example;
                display += "====================================================================================================================================================================\n";
                display += "Vocab: " + vocab + "\nMeaning: " + meaning + "\nExample: " + example + "\n\n";
                Add(vocab,meaning,example);
            }             
            return display;
        }
        private WordNode FindMinNode(WordNode node)
        {
            while (node.left != null)
            {
                node = node.left;
            }
            return node;
        }

        public WordNode? FindWord(string word)
        {
            WordNode? currentNode = root;

            while (currentNode != null)
            {
                if (word == currentNode.word)
                {
                    return currentNode;
                }
                else if (string.Compare(word, currentNode.word) < 0)
                {
                    currentNode = currentNode.left;
                }
                else
                {
                    currentNode = currentNode.right;
                }
            }
            return currentNode;
        }

        public string FindOccurence(string Pattern, string algorithm)
        {
            if (algorithm=="KMP")
            {
                string display = "";
        
                return display;
            }
            else if (algorithm == "rabinkarp")
            {
                Console.Write("Enter patterns (seperated by a space !): ");

                string[] Patterns = Pattern.Split(' ');

                StreamReader reader = new StreamReader("../../../Data/Text.txt");
                string Text = "";
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    Text += line;
                }
                string display = "";
                for(int i=0; i< Patterns.Length;++i)
                {
                    Search search = new Search();
                    Matches Result = search.RabinKarp(Text,Patterns[i]);
                    display += "Pattern: "+Patterns[i];
                    display += "-->Occurences: " + Result.count.ToString();
                    display += "\n";
                }
                return display;
            }
            else 
                return "";
        }
    }

    /*
    static void Main(string[] args)
    {
        Dict dict = new Dict();
        dict.LoadFromFile("Dict.txt");
        int choice = -1;
        while (choice != 0)
        {
            Console.WriteLine("---------------Menu--------------");
            Console.WriteLine("1. Them tu vung");
            Console.WriteLine("2. Xoa tu vung");
            Console.WriteLine("3. Tra cuu tu vung");
            Console.WriteLine("4. Luu vao file");
            Console.WriteLine("5. Doc tu file");
            Console.WriteLine("6. Tim so lan xuat hien");
            Console.WriteLine("0. Thoat");
            Console.WriteLine("---------------END---------------");

            // yêu cầu người dùng nhập lựa chọn
            Console.Write("Nhap lua chon cua ban: ");
            string input = Console.ReadLine();
            // chuyển đổi lựa chọn thành kiểu int
            if (!int.TryParse(input, out choice))
            {
                Console.WriteLine("Lua chon khong hop le.");
                continue; // quay lại vòng lặp để yêu cầu người dùng nhập lại lựa chọn
            }

            switch (choice)
            {
                case 1:
                    Console.WriteLine("Nhap tu can them: ");
                    dict.Add(Console.ReadLine(), Console.ReadLine(), Console.ReadLine());
                    break;

                case 2:
                    Console.WriteLine("Nhap tu can xoa: ");
                    dict.Delete(Console.ReadLine());
                    break;

                case 3:
                    Console.WriteLine("Nhap tu can tra cuu: ");
                    dict.FindWord(Console.ReadLine());
                    break;

                case 4:
                    dict.SaveToFile("Dict.txt");
                    break;

                case 5:
                    dict.LoadFromFile("Dict.txt");
                    break;
                case 6:
                    Console.Write("Enter the algorithm (KMP or rabinkart):");
                    string algorithm = Console.ReadLine();
                    dict.FindOccurence(algorithm);
                    break;

                case 0:
                    break;
                default:
                    Console.WriteLine("Lua chon khong hop le.");
                    break;
            }
        }
    }
    */    
}
