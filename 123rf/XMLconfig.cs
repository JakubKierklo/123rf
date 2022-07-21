using System;
using System.IO;
using System.Windows;
using System.Xml;

namespace _123rf
{
    class XMLfile : MainWindow
    {
        public string login { get; set; }
        public string pass { get; set; }
        //string appDataPath = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders", "Local AppData", String.Empty).ToString();
        string xmlFilePath = @"c:\TestTest\test.xml";
        string xmlFolderPath = @"c:\TestTest";
        string appData = Environment.GetEnvironmentVariable("LocalAppData");

        public bool CreateConfigXML(string login, string password)
        {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Indent = true;  //Funkcja, która przenosi elementy na niższe linie
            try
            {
                if (Directory.Exists($@"{appData}\123rf"))
                {

                }
                else
                {
                    DirectoryInfo di = Directory.CreateDirectory($@"{appData}\123rf");
                }


                using (XmlWriter xmlWriter = XmlWriter.Create($@"{appData}\123rf\config123rf.xml", xmlWriterSettings))
                {
                    xmlWriter.WriteStartElement("Danelogowania");
                    xmlWriter.WriteElementString("Login", login);
                    xmlWriter.WriteElementString("Password", password);
                    xmlWriter.WriteEndElement();
                }
                return true;
            }
            catch (Exception)
            {
                MessageBox.Show("Nie udało się utworzyć pliku konfiguracyjnego");
                return false;
            }

        }

        public void readXmlFile()
        {

            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
            xmlReaderSettings.Async = true;
            XmlReader reader = XmlReader.Create($@"{appData}\123rf\config123rf.xml", xmlReaderSettings);

            if (reader.Read())
            {
                reader.ReadToFollowing("Login");
                var loginStrona = reader.ReadElementContentAsString();
                //MessageBox.Show(loginStrona); //działa
                login = loginStrona;

                reader.ReadToFollowing("Password");
                var passStrona = reader.ReadElementContentAsString();
                pass = passStrona;
            }

        }
    }
}
