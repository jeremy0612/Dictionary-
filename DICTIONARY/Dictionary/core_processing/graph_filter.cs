using System;
using System.IO;
using System.Collections.Generic;
using algorithm;
using data_loader;

namespace Program
{
    public class Filter
    {
        public WordNode? head;
        private WordNode? tail;
        private int size;
        public string display = ""; 
        public Filter()
        {
            head = null;
            tail = null;
            size = 0;
        }

        public void addLast(string vocab, int occur)
        {
            WordNode newest = new WordNode(vocab);
            newest.occurence = occur;
            if (isEmpty())
                head = newest;
            else
                tail.right = newest;
            tail = newest;
            size = size + 1;
        }
        public bool isEmpty()
        {
            return size == 0;
        }
        public void Forward(Dict dict)
        {
            if(display != "")
                display = "";
            WordNode current = head;
            while (current != null)
            {
                WordNode check_word = dict.FindWord(current.word);
                if(check_word != null)
                {
                    display += "====================================================================================================================================================================\n";
                    display += "Vocab: " + current.word + "\nMeaning: " + check_word.meaning + "\n";
                    display += "Case study: " + check_word.example + "\nOccurence: " + current.occurence + "\n\n\n";
                }
                // Console.WriteLine(current.word + ": " + current.occurence);
                current = current.right;
            }
        }
    }
    public class Paragrapp : Dict 
    {
        
        protected WordNode? root;
        protected string? dictText;
        public Filter filter = new Filter();
        public string? display ="Word occurence after filter:\n";
        public void  Add(string word)
        {
            WordNode newNode = new WordNode(word);

            if (root == null)
            {
                root = newNode;
            }
            else
            {
                WordNode currentNode = root;
                bool isDuplicate = false;

                while (currentNode != null)
                {
                    int comparisonResult = string.Compare(word, currentNode.word);

                    if (comparisonResult < 0)
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
                    else if (comparisonResult > 0)
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
                        // Word is a duplicate, don't add it and set the flag
                        isDuplicate = true;
                        break;
                    }
                }
                // After adding the word to the tree, perform KMP search on the word in the dictionary
                if (!isDuplicate)
                {
                    KMP.LinkedList Result = new KMP.LinkedList();
                    KMP kmp = new KMP();
                    kmp.Search(word, dictText, Result);
                    
                    if (Result.isEmpty() == false)
                    {
                        KMP.Node current = Result.head;
                        while (current != null)
                        {
                            current = current.next;
                        }
                    }
                    filter.addLast(word,Result.length());
                    display += "====================================================================================================================================================================\n"; 
                    display += "Vocab:" + word + "\nOccurence:" + Result.length() + "\n\n";
                }
            }
        }
        public void Forward(string fileName)
        {
            // Load words from a text file
            string dictFileName = fileName;
            dictText = File.ReadAllText(dictFileName);
            string[] wordsInDict = dictText.Replace(",","").Replace("\n"," ").Split(' ');
            foreach(string word in wordsInDict)
            {
                Add(word);
            }
        }
        public void Filt(Dict dict)
        {

        }
    }
}