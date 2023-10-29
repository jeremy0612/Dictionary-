using System;
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
using Program;

namespace Dictionary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dict dict = new Dict();
        string choice = "";
        public MainWindow()
        {
            InitializeComponent();
            dict.LoadFromFile("../../../Data/Dictionary.txt");
            var resultObject = new { Result = "Hello, world!" };
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
                Input.Visibility = Visibility.Collapsed;
            }
            else 
            {
                choice = tag;
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
                case "Occurence":
                    PathAttribute.Visibility = Visibility.Collapsed;
                    WordAttribute.Visibility = Visibility.Visible;
                    break;
            }
        }
        
        private void Apply(object sender, RoutedEventArgs e)
        {
            if (ValidateAndParseString(Word.Text, out string word) &&
                ValidateAndParseString(Meaning.Text, out string meaning) &&
                ValidateAndParseString(Example.Text, out string example) )
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
                        dict.SaveToFile("../../../Data/Dict.txt");
                        break;
                    case "Load":
                        dict.LoadFromFile("../../../Data/Dictionary.txt");
                        ResultContentControl.DataContext = new { Result = dict.LoadFromFile("../../../Data/Dictionary.txt") };
                        break;
                    case "Occurence":
                        try
                        {
                            string algorithm = Microsoft.VisualBasic.Interaction.InputBox("Choose algorithm (KMP or rabinkarp):", "Algorithm choosing");
                            string result = dict.FindOccurence(word,algorithm);
                            MessageBox.Show(result, "Message Box");
                            ResultContentControl.DataContext = new { Result = result };
                        }
                        catch (Exception ex)
                        {
                            // Log the exception
                            Console.WriteLine(ex.ToString());
                            // Show an error message to the user
                            MessageBox.Show("An error occurred: " + ex.Message, "Error");
                        }
                        break;
                }
            }
        }                
    }
}
