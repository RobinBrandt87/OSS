using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using System.Net.Http;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Windows.Security.Cryptography;






// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=391641 dokumentiert.
namespace Objekt_Securety_System
{

    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet werden kann oder auf die innerhalb eines Rahmens navigiert werden kann.
    /// </summary>

    public sealed partial class MainPage : Page
    {
       
        public MainPage()
        {
            GlobalData.Login_Enable = false;
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;     

            textBlock.Text = Windows.ApplicationModel.Store.CurrentApp.AppId.ToString();
            
        }

        public RoutedEventHandler textBlock_SelectionChanged { get; private set; }

        /// <summary>
        /// Wird aufgerufen, wenn diese Seite in einem Rahmen angezeigt werden soll.
        /// </summary>
        /// <param name="e">Ereignisdaten, die beschreiben, wie diese Seite erreicht wurde.
        /// Dieser Parameter wird normalerweise zum Konfigurieren der Seite verwendet.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {

            // TODO: Seite vorbereiten, um sie hier anzuzeigen.

            // TODO: Wenn Ihre Anwendung mehrere Seiten enthält, stellen Sie sicher, dass
            // die Hardware-Zurück-Taste behandelt wird, indem Sie das
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed-Ereignis registrieren.
            // Wenn Sie den NavigationHelper verwenden, der bei einigen Vorlagen zur Verfügung steht,
            // wird dieses Ereignis für Sie behandelt.
         
           

        }
        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalData.Login_Enable == true)
            {
                bool EingabeL = true;   // Überprüfungsvariable Login   true = Null or Whitespaces 
                bool EingabeP = true;   // Überprüfungsvariable Passwort  
                bool check = false;     // Hilfsvariable 
                bool check2 = false;
                Int32 ID;


                GlobalData.Login = EingabeLoginName.Text;
                GlobalData.Passwd = passwordBox.Password;


                //GlobalData.Login = EingabeLoginName.Text;

                EingabeL = String.IsNullOrWhiteSpace(GlobalData.Login);
                EingabeP = String.IsNullOrWhiteSpace(GlobalData.Passwd);


                if (!EingabeL & !EingabeP) // Ist ein LoginName/Passwort Eingegeben worden  ?
                {
                    // Datenbankzugriff

                    // Es findet kein dirtekter Datenbankserverzugriff statt !!

                    await Globafunctions.checkserver();      // warte darauf das der Task checkserver() erledigt ist bevor du weiter machst  06.04.2016

                    if (GlobalData.Check == true) //Verbingsaufbau zum Server prüfen aktuell noch true wird noch geändert// eingefügt 06.04.2016
                    {
                        // Schritt 3 SQL Abfrage zusammenbasteln - entfällt
                        // SQL Abfrage mitels https (GET übergabe) requests an den server 

                        //string Login = "Robin";                // Login Name Robin
                        //string Passwort = "93499236";           // Passwort 93499236
                        string Login = GlobalData.Login;                // Login Name Robin
                        string Passwort = GlobalData.Passwd;            // Passwort 93499236


                        //################################################################################################
                        // jetzt mit Verschlüsseleung (neu 27.01.2016)
                        // Generierung des Passworthashes mit SHA 512 

                        HashAlgorithmProvider hashProvider = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha512);
                        IBuffer hash = hashProvider.HashData(CryptographicBuffer.ConvertStringToBinary(GlobalData.Passwd, BinaryStringEncoding.Utf8));
                        GlobalData.Passwdsha1 = CryptographicBuffer.EncodeToHexString(hash);

                        // Ausbaustufe Entweder ein Prepaird statment (PHP)- Erledigt oder eine SHA funktion auf den Login - Auch Erledigt

                        string Ziel = "/login.php?";

                        HttpContent content = new FormUrlEncodedContent(new[]             // POST inhalt vorbereiten
                        {
                            new KeyValuePair<string, string>("login", GlobalData.Login.ToString()),
                            new KeyValuePair<string, string>("passwd", GlobalData.Passwdsha1.ToString()),
                            new KeyValuePair<string, string>("AppID", Windows.ApplicationModel.Store.CurrentApp.AppId.ToString())
                                });
                        // Schritt 4 Abfrage abschicken und ergebnis entgegennehmen 
                        try
                        {
                            GlobalData.Pers_ID = await Globafunctions.HttpAbfrage(Ziel, content);

                        }
                        catch (Exception)
                        {
                            GlobalData.Pers_ID = "False";
                            throw;
                        }
                        check = string.IsNullOrEmpty(GlobalData.Pers_ID);
                        check2 = Int32.TryParse(GlobalData.Pers_ID, out ID);

                        if (check || !check2)   // Prüft die Pers_ID auf empty bei true kommt ein False in die Pers ID 
                        {
                            GlobalData.Pers_ID = "False";   // verhindert Login ohne server bzw bei fehlermeldung vom Server

                        }
                        else
                        {
                            GlobalData.Pers_ID = ID.ToString();
                        }


                        //###############################################################################################################

                        if (true/*GlobalData.MyDebug*/)
                        {
                            GlobalData.StringPasswd = GlobalData.Passwdsha1.ToString();
                            MessageDialog msgbox = new MessageDialog("Ihr Login Name " + GlobalData.Login);
                            MessageDialog msgbox1 = new MessageDialog("Ihr Passwort " + GlobalData.StringPasswd);
                            //MessageDialog msgbox2 = new MessageDialog(GlobalData.HttpResponse);
                            MessageDialog msgbox3 = new MessageDialog(GlobalData.Pers_ID);
                            System.Threading.Tasks.Task<MessageDialog> ShowAsync = null;
                            await msgbox.ShowAsync();
                            await msgbox1.ShowAsync();
                            //await msgbox2.ShowAsync();
                            await msgbox3.ShowAsync();

                        }
                        // weiterleitung auf die nächste Seite ?

                        if (GlobalData.Pers_ID == "False")
                        {
                            MessageDialog msgbox3 = new MessageDialog("Falsche Anmeldedaten");
                            await msgbox3.ShowAsync();
                        }
                        else
                        {
                            GlobalData.Check = false;
                            if (GlobalData.Pers_ID != "0")
                            {
                                Frame.Navigate(typeof(Intern));
                                passwordBox.Password = "";
                                EingabeLoginName.Text = "";
                            }
                            else
                            {
                                MessageDialog msgbox4 = new MessageDialog("Ihr Acount wurde Gesperrt ");
                                await msgbox4.ShowAsync();
                            }

                        }

                    }
                    else
                    {
                        MessageDialog msgbox0 = new MessageDialog("Keine Verbindung zum Server \nBitte Verbindung Prüfen!");
                        System.Threading.Tasks.Task<MessageDialog> ShowAsync = null;
                        await msgbox0.ShowAsync();
                    }
                }
                else
                {
                    MessageDialog msgbox1 = new MessageDialog("Geben Sie Bitte Ihr Login&Passwort an!");
                    System.Threading.Tasks.Task<MessageDialog> ShowAsync = null;
                    await msgbox1.ShowAsync();

                }

            }
        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void Home_Checked(object sender, RoutedEventArgs e)
        {
            GlobalData.Login_Enable = true;
            IPBOX.IsChecked = false;
            Gemeinde.IsChecked = false;
            LocalHost.IsChecked = false;
            InternetCheckBox.IsChecked = false;
            GlobalData.Uri2 = "http://192.168.2.105/OSS/APP-COM";      // mit Phone Zuhause
        }

        private void Gemeinde_Checked(object sender, RoutedEventArgs e)
        {
            GlobalData.Login_Enable = true;
            IPBOX.IsChecked = false;
            Home.IsChecked = false;
            LocalHost.IsChecked = false;
            InternetCheckBox.IsChecked = false;
            GlobalData.Uri2 = "http://192.168.100.75/OSS/APP-COM";      // mit Phone Gemeinde  merken//pmg/v3/test3.php
        }

        private void LocalHost_Checked(object sender, RoutedEventArgs e)
        {
            GlobalData.Login_Enable = true;
            IPBOX.IsChecked = false;
            Home.IsChecked = false;
            Gemeinde.IsChecked = false;
            InternetCheckBox.IsChecked = false;
            GlobalData.Uri2 = "http://localhost/OSS/APP-COM";          // Lokaler Debug
        }

        private void IPBOX_Checked(object sender, RoutedEventArgs e)
        {
            
            Home.IsChecked = false;
            Gemeinde.IsChecked = false;
            LocalHost.IsChecked = false;
            InternetCheckBox.IsChecked = false;
            if(IP.Text.ToString() != "")
            {
                GlobalData.Login_Enable = true;
                GlobalData.Uri2 = "http://" + IP.Text + "/OSS/APP-COM";          // überall wo ipconfig eine ip für den Server leifert und das Netzwerk kommunikation unterinader erlaubt
                                                                                 // geht nur wenn man die IP eingibt bevor man das häckchen setzt 
            }

        }

        private void checkBox_Checked_1(object sender, RoutedEventArgs e)
        {
            GlobalData.Login_Enable = true;
            GlobalData.Pers_ID = "Debug";       // Aktiviert den DebugLogin achtung nur bei Virtualiesiertem WP da sonst keine verbindung zur DB besteht 
            //LocalHost.IsChecked = true;         // dieser DebugUser funktioniert auch nur wenn auf dem selben Gerät der Apache und MYSQL server ausgeführt wird
            GlobalData.Uri2 = "http://localhost/OSS/APP-COM";          // Lokaler Debug
            Frame.Navigate(typeof(Intern));
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog msgboxx = new MessageDialog("Parameter" + GlobalData.Tag_ID);
              await msgboxx.ShowAsync(); // zeigt den aktuellen scan an 
        }
        private void httpsBox_Checked(object sender, RoutedEventArgs e)
        {
        }
 
        private void InternetCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            GlobalData.Login_Enable = true;
            string https = "";
            IPBOX.IsChecked = false;
            Home.IsChecked = false;
            Gemeinde.IsChecked = false;
            LocalHost.IsChecked = false;
            if (httpsBox.IsChecked == true)     // setzt bei bedarf das s in die URL
            {
                https = "s";
            }
            else
            {
                https = "";
            }
            GlobalData.Uri2 = "http"+https+"://www.h2472138.stratoserver.net/oss/APP-COM";
        }


    }
}
