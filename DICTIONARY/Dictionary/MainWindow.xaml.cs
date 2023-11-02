using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using Program;

namespace Dictionary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        protected Dict dict = new Dict(); // Data loaded from source
        protected Paragrapp para = new Paragrapp(); // paragraph from user
        string choice = "";
        public MainWindow()
        {
            InitializeComponent();
            var resultObject = new { Result = "Hello, welcome to our Dictionary!" };
            ResultContentControl.DataContext = resultObject;
        }
        private bool ValidateAndParseString(string input, out string result)
        {
            if (!string.IsNullOrEmpty(input))
            {
                result = input;
                return true;
            }
            else
            {
                MessageBox.Show("Please enter a valid value!");
                result = null;
                return false;
            }
        }
        private void UserChoice(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string tag = button.Tag.ToString();  
            if(choice == tag && Input.Visibility == Visibility.Visible)
            {
                choice = "";
                //Input.Visibility = Visibility.Collapsed;
            }
            else 
            {
                choice = tag;
                if (tag=="Save")
                    FileType.Content = "Sink type";
                else
                    FileType.Content = "Source type";
                Input.Visibility = Visibility.Visible;
            }  
            switch(tag)
            {
                case "Load":
                case "Save":
                    PathAttribute.Visibility = Visibility.Visible;
                    WordAttribute.Visibility = Visibility.Collapsed;
                    break;
                case "Add":
                case "Delete":
                case "Search":
                    PathAttribute.Visibility = Visibility.Collapsed;
                    WordAttribute.Visibility = Visibility.Visible;
                    break;
                case "Occurence":
                    Input.Visibility = Visibility.Collapsed;
                    if(dict.root == null)
                    {
                        MessageBox.Show("Please load the dictionary first !", "Message Box");
                        break;
                    }
                    try
                    {
                        OpenFileDialog openFileDialog = new OpenFileDialog();
                        openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                        if(openFileDialog.ShowDialog() == true)
                        {  
                            string fileName = openFileDialog.FileName;
                            para = new Paragrapp();
                            para.Forward(fileName);
                            ResultContentControl.Content = "debug";
                            
                            para.filter.Forward(dict);
                            string filt = para.filter.display;
                            //string algorithm = Microsoft.VisualBasic.Interaction.InputBox("Choose algorithm (KMP or rabinkarp):", "Algorithm choosing");
                            //string result = dict.FindOccurence(word,algorithm);
                            MessageBox.Show("Successfull filtering !!!", "Message Box");
                            ResultContentControl.Content = filt ;
                        }    
                    }
                    catch (Exception ex)
                    {
                        // Show an error message to the user
                        MessageBox.Show("An error occurred: " + ex.Message, "Error");
                    }
                    break;
            }
        }
        
        private void Apply(object sender, RoutedEventArgs e)
        {
            if (ValidateAndParseString(Word.Text, out string word) &&
                ValidateAndParseString(Meaning.Text, out string meaning) &&
                ValidateAndParseString(Example.Text, out string example) &&
                ValidateAndParseString(Source.Text, out string source) )
            {
                // All input values are valid and parsed successfully
                switch (choice)
                {
                    case "Add":
                        try
                        {
                            dict.Add(word,meaning,example);
                        }
                        catch (Exception ex)
                        {
                            // Log the exception
                            Console.WriteLine(ex.ToString());
                            // Show an error message to the user
                            MessageBox.Show("An error occurred: " + ex.Message, "Error");
                        }
                        ResultContentControl.DataContext = new { Result = "Added"};
                        break;
                    case "Delete":
                        dict.Delete(word);
                        ResultContentControl.DataContext = new { Result = "Deleted"};
                        break;
                    case "Search":
                        try
                        {
                            WordNode found = dict.FindWord(word);
                            if (found != null)
                            {
                                MessageBox.Show("Word Found", "Message Box");
                                try
                                {
                                    string data = found.word + ": " + found.meaning + "\n Eg: " + found.example;
                                    ResultContentControl.DataContext = new { Result = data };
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.ToString());
                                    MessageBox.Show("An error occurred while loading data from file: " + ex.Message, "Error");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Word not found", "Message Box");
                            }
                        }
                        catch (Exception ex)
                        {
                            // Log the exception
                            Console.WriteLine(ex.ToString());
                            // Show an error message to the user
                            MessageBox.Show("An error occurred: " + ex.Message, "Error");
                        }
                        
                        break;
                    case "Save":
                        Input.Visibility = Visibility.Collapsed;
                        para.SaveToFile("../../../data/Dict.txt");
                        break;
                    case "Load":
                        if(source == "csv")
                        {
                            dict.LoadFromFile("../../../data/it_topic.csv","csv");
                            ResultContentControl.DataContext = new { Result = dict.LoadFromFile("../../../data/it_topic.csv","csv") };
                        }
                        else if(source == "json")
                        {
                            dict.LoadFromFile("../../../data/it_topic.json","json");
                            ResultContentControl.DataContext = new { Result = dict.LoadFromFile("../../../data/it_topic.json","json") };
                        }
                        else if(source == "mongo")
                        {
                            dict.LoadFromMongo();
                            ResultContentControl.DataContext = new { Result = dict.LoadFromMongo() };
                        }
                        break;
                }
            }
        }                
    }
}
