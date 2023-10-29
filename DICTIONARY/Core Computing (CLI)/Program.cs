using System;
using System.IO;


namespace Program
{
    class MGB
    {
        class WordNode
        {
            public string word;
            public string meaning;
            public string example;
            public WordNode left;
            public WordNode right;

            public WordNode(string word, string meaning, string example)
            {
                this.word = word;
                this.meaning = meaning;
                this.example = example;
                this.left = null;
                this.right = null;
            }
        }

        class Dictionary
        {
            public WordNode root;

            public Dictionary()
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

            private WordNode DeleteRec(WordNode currentNode, string word)
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

            public void LoadFromFile(string fileName)
            {
                StreamReader reader = new StreamReader(fileName);
                Console.WriteLine("List Vocabulary:");
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] parts = line.Split(':');

                    string word = parts[0];
                    string meaning = parts[1];
                    string example = parts[2];
                    Console.Write("--> ");
                    Console.WriteLine(word);
                    Add(word, meaning, example);
                }

                reader.Close();
            }

            private WordNode FindMinNode(WordNode node)
            {
                while (node.left != null)
                {
                    node = node.left;
                }
                return node;
            }

            public void FindWord(string word)
            {
                WordNode currentNode = root;

                while (currentNode != null)
                {
                    if (word == currentNode.word)
                    {
                        Console.WriteLine(currentNode.meaning);
                        Console.WriteLine(currentNode.example);
                        return;
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
                Console.WriteLine("Word not found.");
            }

            public void FindOccurence(string algorithm)
            {
                if (algorithm=="KMP")
                {
                    Console.Write("Enter patterns (seperated by a space !): ");
                    
                    string Pattern = Console.ReadLine();
                    string[] Patterns = Pattern.Split(' ');

                    StreamReader reader = new StreamReader("Text.txt");
                    string Text = "";
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        Text += line;
                    }
                    
                    for(int i=0; i< Patterns.Length;++i)
                    {
                        try
                        {
                            KMP.LinkedList Result = new KMP.LinkedList();
                            KMP kmp = new KMP();
                            kmp.KMPSearch(Patterns[i], Text, Result);
                            Console.WriteLine("Pattern: "+Patterns[i]);
                            Console.Write("-->Occurences: ");
                            Console.WriteLine(Result.length());
                        }
                        catch
                        {
                            break;
                        }
                    }
                }
                else if (algorithm == "rabinkarp")
                {
                    Console.Write("Enter patterns (seperated by a space !): ");
                    
                    string Pattern = Console.ReadLine();
                    string[] Patterns = Pattern.Split(' ');

                    StreamReader reader = new StreamReader("Text.txt");
                    string Text = "";
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        Text += line;
                    }
                    for(int i=0; i< Patterns.Length;++i)
                    {
                        Search search = new Search();
                        Matches Result = search.RabinKarp(Text,Patterns[i]);
                        Console.WriteLine("Pattern: "+Patterns[i]);
                        Console.Write("-->Occurences: ");
                        Console.WriteLine(Result.count);
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            Dictionary dict = new Dictionary();
            dict.LoadFromFile("dictionary.txt");
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
                        dict.SaveToFile("dictionary.txt");
                        break;

                    case 5:
                        dict.LoadFromFile("dictionary.txt");
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
    }
}
