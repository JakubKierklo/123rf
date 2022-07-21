using Microsoft.Win32;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;



namespace _123rf
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        static string downloadPath = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders", "{374DE290-123F-4565-9164-39C4925E467B}", String.Empty).ToString();
       
        public static string nameOfLoadedFile;
        //public static strings nameOfLoadedFile;
        
        
        public MainWindow()
        {
            InitializeComponent();
        }

        public void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Plik CSV (*.csv)|*.csv";
            openFileDialog.InitialDirectory = downloadPath;

            if (openFileDialog.ShowDialog() == true)
            {
                nameOfLoadedFile = openFileDialog.FileName;
                FilePathTextBox.Text = nameOfLoadedFile;
               
            }
            else
            {
                MessageBox.Show("Nie udało się wczytać pliku");
            }
        }

       


        private void FilePathTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {


        }

        private void SaveLoginDataToXml_Click(object sender, RoutedEventArgs e)
        {
            XMLfile xmlconfig = new XMLfile();
            xmlconfig.CreateConfigXML(LoginTextBox.Text, PasswordPasswordBox.Password);
        }

        private void readXmlButton_Click(object sender, RoutedEventArgs e)
        {
            XMLfile xmlFile = new XMLfile();
            xmlFile.readXmlFile();
            //LoginTextBox.Text = xmlFile.login;
            LoginTextBox.Text = xmlFile.login;
            PasswordPasswordBox.Password = xmlFile.pass;



        }

        private void LoginTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
          
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            bool? stan = DebugModeUserControl.DebugMode.IsChecked;
            //if (DebugModeUserControl.DebugMode.IsChecked==true)
            //{
            //    MessageBox.Show("mw zatrue");
            //}
            //else
            //{
            //    MessageBox.Show("falseee");
            //}
            BrowserControl browserControl = new BrowserControl();
            
            browserControl.StartApp();
        }

        private void PathOfDownloadedPhotos_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void WczytajSciezkeZapisywanychZdjec_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Plik CSV (*.csv)|*.csv";
            openFileDialog.InitialDirectory = downloadPath;

            if (openFileDialog.ShowDialog() == true)
            {
                nameOfLoadedFile = openFileDialog.FileName;
                FilePathTextBox.Text = nameOfLoadedFile;
            }
            else
            {
                MessageBox.Show("Nie udało się wczytać pliku");
            }
        }

        private void trol_Click(object sender, RoutedEventArgs e)
        {
           ChromeDriver chrome = new ChromeDriver();
           //chrome.Close();
           var a= chrome.Capabilities.GetCapability("browserVersion");
           File.WriteAllText(@"E:\aaa\aa.txt", a.ToString());  
        }

        
    }
}
