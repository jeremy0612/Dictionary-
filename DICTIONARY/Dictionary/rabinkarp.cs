using System;
using System.Collections.Generic;

class Position //Khai bao note
{
    public int pos;
    public Position next;
    public Position(int e)
    {
        pos = e;
        next = null;
    }
}
public class Matches
{
    Position head;
    public int count;

    public Matches()
    {
        head = null;
        count = 0;
    }
    public int Search(int data)
    {
        Position temp = head;
        int index = 0;
        while (temp.next != null)
        {
            if (temp.pos == data)
                return index;
            temp = temp.next;
            index++;
        }
        return -1;
    }
    public void AddnodeToEnd(int data)
    //them mot node vao cuoi list
    {
        Position newNode = new Position(data);

        if (head == null)
        {
            head = newNode;
        }
        else
        {
            Position current = head;
            while (current.next != null)
            {
                current = current.next;
            }
            current.next = newNode;
        }
        count++;
    }
    public void Inspect()
    {
        Position current = head;
        //bool i = true;
        int flag = 1;
        while (true)
        {
            flag = int.Parse(Console.ReadLine());
            if (flag != 1)
                break;
            else
            {

                Console.WriteLine(current.pos);
                current = current.next;
                if (current == null)
                    break;
                continue;
            }
        }
    }
    public void Printlist()
    {
        Position runner = head;
        while (runner != null)
        {
            Console.Write(runner.pos + " ");
            runner = runner.next;
        }
    }
    public string Showlist()
    {
        string result = "Head -->";
        Position runner = head;
        while (runner != null)
        {
            result += runner.pos + " ";
            runner = runner.next;
        }
        result += "<-- Tail";
        return result;
    }
}

public class Search
{
    public static void naive(string s, string finds)
    {
        for (int i = 0; i < (s.Length - finds.Length + 1); i++)
        {
            int j;
            for (j = 0; j < finds.Length; j++)
            {
                if (s[i + j] != finds[j])
                {
                    break;
                }
            }
            if (j == finds.Length)
            {
                /* Console.WriteLine("Find at position " + i);*/
            }
        }
    }

    public Matches RabinKarp(string text, string pattern)
    {
        const int prime = 101; // A prime number used for the hash function
        const int radix = 256; // The size of the alphabet used for the hash function

        int textLength = text.Length;
        int patternLength = pattern.Length;
        int patternHash = 0; // The hash value of the pattern
        int textHash = 0; // The hash value of the current text window
        int h = 1; // The radix^(patternLength-1) value used for rolling hash calculation

        // Calculate the hash value of the pattern and the initial hash value of the text window
        for (int i = 0; i < patternLength; i++)
        {
            patternHash = (radix * patternHash + pattern[i]) % prime;
            textHash = (radix * textHash + text[i]) % prime;
        }

        // Calculate the value of h
        for (int i = 0; i < patternLength - 1; i++)
        {
            h = (h * radix) % prime;
        }

        Matches matches = new Matches();

        // Slide the text window over the text and check for matches
        for (int i = 0; i <= textLength - patternLength; i++)
        {
            // If the hash values match, check if the pattern and the text window match
            if (patternHash == textHash)
            {
                bool match = true;
                for (int j = 0; j < patternLength; j++)
                {
                    if (pattern[j] != text[i + j])
                    {
                        match = false;
                        break;
                    }
                }
                if (match)
                {
                    matches.AddnodeToEnd(i);
                }
            }

            // Calculate the hash value of the next text window
            if (i < textLength - patternLength)
            {
                textHash = (radix * (textHash - text[i] * h) + text[i + patternLength]) % prime;
                if (textHash < 0)
                {
                    textHash += prime;
                }
            }
        }
        return matches;
    }
    /*
    public static void Main(string[] args)
    {
        Console.WriteLine("nhap text");
        string s = Console.ReadLine();
        string s1 = "";
        while (s1 == "")
        {
            Console.WriteLine("Nhập chữ muốn tìm:");
            s1 = Console.ReadLine();
        }
        naive(s, s1);

        Matches matches = RabinKarp(s, s1);
        //Console.WriteLine("Rabin-Karp matches: " + string.Join(", ", matches));
        matches.Printlist();

    }
    */
}
