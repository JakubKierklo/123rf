using Microsoft.Win32;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Linq;
using System.IO;
using System.Threading;
using System.Windows;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace _123rf
{
    class BrowserControl :MainWindow
    {
        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        WebDriver driver;       
        //IWebDriver driver;               
        readonly string urlName = "https://www.123rf.com/";
        ChromeOptions chromeOptions = new ChromeOptions();
        readonly static string downloadPath = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders", "{374DE290-123F-4565-9164-39C4925E467B}", String.Empty).ToString();
        static string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        XMLfile xmlfile = new XMLfile();
        string appData = Environment.GetEnvironmentVariable("LocalAppData");

        public void StartApp()
        {
           
            try
            {
                if (DebugModeUserControl.DebugModeVar == 1)
                {
                    _logger.Info("Application started...");
                }
               
            
            
            //Ogólne//////////////////////////////////////////////////////////////////////////////////////////////////
            FileStream fileStream = new FileStream(nameOfLoadedFile, FileMode.OpenOrCreate);
            StreamReader streamReader = new StreamReader(fileStream);
            string tekstZPliku = streamReader.ReadToEnd();
            string[] rozdzielenieNowaLinia = tekstZPliku.Split('\n');
            chromeOptions.AddUserProfilePreference("safebrowsing.enabled", true);
            chromeOptions.AddUserProfilePreference("download.default_directory", $@"{desktopPath}\123rf\{DateTime.Now.ToString("dd-MM-yyyy")}");
            driver = new ChromeDriver(chromeOptions);          
            xmlfile.readXmlFile();
            Regex reg = new Regex(@"\d");
            ReadOnlyCollection<IWebElement> headerbutton;          
                //////////////////////////////////////////////////////////////////////////////////////////////////////////
               
                try
            {
                driver.Manage().Window.Maximize();
                driver.Navigate().GoToUrl(urlName);
                //Thread.Sleep(3000);
            }
            catch (Exception)
            {
                MessageBox.Show("Nie udało się uruchomić sesji przeglądarki");
                throw;
            }

            do
            {
              headerbutton = driver.FindElements(By.Id("header-button-login"));
            } while (headerbutton.Count==0);
            ElementOnlyClick(By.Id("header-button-login"));                    //button "zaloguj"               
            ElementClickAndWrite(By.Id("userName"), xmlfile.login);             //login
            ElementClickAndWrite(By.Id("userPassword"), xmlfile.pass);         //login
            ElementOnlyClick(By.Id("login-button"));                           //button "zaloguj"  
            Thread.Sleep(10000);                       
            CheckExistID(By.ClassName("UserDetails__skip"));                    //zamyka pojawiające się okno                        
            CreateFileCSV();
            var ostatnieZapytanie = File.ReadAllText($@"{appData}\123rf\zapytane.csv");
              

                for (int i = 1, j = 1; i < rozdzielenieNowaLinia.Length; i += 2)
                {
                    if (reg.IsMatch(ostatnieZapytanie) && j == 1)
                    {
                        if (j == 1)
                        {
                            try
                            {
                              
                                  if (tekstZPliku.Contains(ostatnieZapytanie))
                                  {
                                      //MessageBox.Show("zawiera");
                                      while (rozdzielenieNowaLinia[i] != ostatnieZapytanie)
                                      {
                                          i += 2;
                                      }
                                      i += 2;
                                  }
                                  else
                                  {
                                      //MessageBox.Show("nie");
                                  }
                                  j++;
                             
                               
                            }
                            catch (Exception)
                            {

                                j++;
                            }

                        }
                        else
                        {
                            j++;
                        }
                    }
                   
                    ElementClickAndWrite(By.Id("Main-Searchbar-input"), rozdzielenieNowaLinia[i]);                   
                    ElementOnlyClick(By.XPath("//div[@class='input-group-append d-flex']/button[@id='Main-Searchbar-submit']"));
                    Thread.Sleep(1000);
                    ElementOnlyClick(By.ClassName("ImageThumbnail__overlay")); //click on the photo                                                                              //MoveToItem(By.Id("download-button"));
                    MoveToItem(By.Id("social-share"));
                    ElementOnlyClickUsingGreaterTimeSpan(By.Id("download-button"));
                    ElementOnlyClickUsingGreaterTimeSpan(By.XPath("//div[@class='ImageDetailsDownloadModal__pricingButtonWrapper flex-column']/button[@id='download-button']"));
                    //WaitForID(By.Id("referralLink-btn"));
                    WaitForID(By.Id("copyright-link-btn"));
                    File.WriteAllText($@"{appData}\123rf\zapytane.csv", $"{rozdzielenieNowaLinia[i]}");                  
                    Menu(By.Id("Main-Searchbar-input"), rozdzielenieNowaLinia[i]);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (DebugModeUserControl.DebugModeVar == 1)
                {
                    _logger.Info("Application stopped...");
                }           
                throw;//
            }
            finally
            {

                if (DebugModeUserControl.DebugModeVar == 1)
                {
                    _logger.Info("Application stopped...");
                }
            }
        }
        

        void ElementOnlyClick(By idElement)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));          
            IWebElement iWebElement = wait.Until(ExpectedConditions.ElementExists(idElement));
            iWebElement.Click();

            if (DebugModeUserControl.DebugModeVar == 1)
            {
                _logger.Debug($"Klikam w {idElement}");
            }
            
        }       
        void ElementOnlyClickUsingGreaterTimeSpan(By idElement)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            IWebElement LoginButton = wait.Until(ExpectedConditions.ElementExists(idElement));
            LoginButton.Click();
            if (DebugModeUserControl.DebugModeVar == 1)
            {
                _logger.Debug($"Klikam w {idElement}");
            }
            
        }
        void ElementClickAndWrite(By idElement, string text)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement iWebElement = wait.Until(ExpectedConditions.ElementExists(idElement));           
            iWebElement.SendKeys(text);
            var a = text.Replace("\r", "");
            if (DebugModeUserControl.DebugModeVar == 1)
            {
                _logger.Debug($"Wpisuję  {a}");
            }
            
        }
        void CheckExistID(By idElement)
        {
            try
            {
                
                var a = driver.FindElements(idElement);
                if (a.Count == 0)
                {

                }
                else if (a.Count > 0)
                {
                    ElementOnlyClick(idElement);
                }
            }
            catch (Exception e)
            {              
                var a = driver.FindElements(idElement);
                if (a.Count == 0)
                {

                }
                else if (a.Count > 0)
                {
                    ElementOnlyClick(idElement);
                }
            }
           
        }     
        void Menu(By idElement, string text)
        {         
                driver.Navigate().GoToUrl("https://www.123rf.com/");
            if (DebugModeUserControl.DebugModeVar == 1)
            {
                _logger.Debug("Wchodzę na stronę https://www.123rf.com/");
            }
            
        }

        void WaitForID(By idElement)
        {
            ReadOnlyCollection<IWebElement> element;
            do
            {
            element = driver.FindElements(idElement);
            } while (element.Count == 0);
        }

        void CreateFileCSV()
        {
            if (File.Exists($@"{appData}\123rf\zapytane.csv"))
            {
                
            }
            else
            {
                var createcsv = File.Create($@"{appData}\123rf\zapytane.csv");
                createcsv.Close();
            }
        }
        void MoveToItem(By idElement)
        {
            ReadOnlyCollection<IWebElement> element;
            IWebElement webElement;           
            do
            {
              element = driver.FindElements(idElement);
            } while (element.Count == 0);
            Actions action = new Actions(driver);
            webElement = driver.FindElement(idElement);
            action.MoveToElement(webElement).Perform();
            if (DebugModeUserControl.DebugModeVar == 1)
            {
                _logger.Debug($"przechodzę do elementu {idElement}");
            }
            
        }
    }
}

