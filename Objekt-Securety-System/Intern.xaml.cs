using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using System.Net.Http;


// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkID=390556 dokumentiert.

namespace Objekt_Securety_System
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet werden kann oder auf die innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class Intern : Page
    {

        class NFC_Tag
        {
            private string PlanID;
            private string RoutenID;
            private string Startzeit;
            private string RoutinenZeit;
            private string id;
            private string bild;
            private string etage;
            private string standort;
            private string zeit;
            private string standort_genau;

            public string Bild
            {
                get { return bild; }
                set { bild = value; }
            }
            public string Zeit
            {
                get { return zeit; }
                set { zeit = value; }
            }
            public string ID
            {
                get { return id; }
                set { id = value; }
            }
            public string Etage
            {
                get { return etage; }
                set { etage = value; }
            }
            public string Standort
            {
                get { return standort; }
                set { standort = value; }
            }
            public string Standort_Genau
            {
                get { return standort_genau; }
                set { standort_genau = value; }
            }
        }
        private static string Data;
        private static string[] String;
        string Pers_ID = GlobalData.Pers_ID;
        string TAG = GlobalData.Tag_ID;
        

        private NFC_Tag[] NFCStack { get; set; }


        public Intern()
        {

            
            this.InitializeComponent();
            GlobalData.Tag_ID = "";    

            textBlock.Text = GlobalData.Login;
            textBlock1.Text = GlobalData.Pers_ID;

            RouteStarten.IsEnabled = false;
            NFCStack = new NFC_Tag[63];
            for (int f = 0; f < 60; f++)
            {
                NFCStack[f] = new NFC_Tag();

            }

            


        }

        public async void timer_Tick(object sender, object e)
        {
            MessageDialog Frage = new MessageDialog("Alles Ok bei Ihnen ?");
            MessageDialog Frage2 = new MessageDialog("Eine Meldung ging an Ihren Kollegen");
            MessageDialog Frage3 = new MessageDialog("Eine Meldung an den Wachschutz wurde Ausgelöst");
            var jetzt = GlobalData.Routinenzeit - DateTime.Now;
            Routen_Zeit.Text = jetzt.Minutes.ToString() + ":" + jetzt.Seconds.ToString();
            if(GlobalData.I == 0)
            {
                Routen_Zeit.Text = "00:00";
            }

            if (Routen_Zeit.Text == "0:0" && GlobalData.Timer_Enable == true)
            {
                
                await Frage.ShowAsync();

            }
            if (Routen_Zeit.Text == "-2:0" && GlobalData.Timer_Enable == true)
            {
                
            }

            if (Routen_Zeit.Text == "-3:0" && GlobalData.Timer_Enable == true)
            {
               
                await Frage2.ShowAsync();
                // Code zum bearbeiten des Alarms
            }
            if (Routen_Zeit.Text == "-5:00" && GlobalData.Timer_Enable == true)
            {

                await Frage3.ShowAsync();
                NFCStack[GlobalData.I].Zeit = "0";
                // Code zum bearbeiten des Alarms 


            }

            if (GlobalData.Tag_ID != "")
            {

                if (GlobalData.Tag_ID == NFCStack[GlobalData.I].ID && Routen_Zeit.Visibility == Visibility.Visible)
                {
                    int Id;
                    Int32.TryParse(GlobalData.Tag_ID, out Id);
                    Id = Id + 1;
                    listBox.Items.RemoveAt(0);
                    var BaseAddress = new Uri(GlobalData.Uri2 + "/Bild.php?ID=" + listBox.Items.ElementAt(0));
                    Ansicht.Navigate(BaseAddress);
                    Scan();
                    GlobalData.I = GlobalData.I + 1;
                    GlobalData.Tag_ID_Alt = GlobalData.Tag_ID;
                    GlobalData.Tag_ID = "";
                    double Routinentime;
                    double.TryParse(NFCStack[GlobalData.I].Zeit, out Routinentime);
                    var Zeit = TimeSpan.FromMinutes(Routinentime);
                    GlobalData.Routinenzeit = DateTime.Now + Zeit;
  
                    if (NFCStack[GlobalData.I].ID == null)     // wenn im nachsten tag nichts drin steht 
                    {
                        GlobalData.Timer_Enable = false;
                        RouteStarten.IsEnabled = true;
                        Routen_Zeit.Visibility = Visibility.Collapsed;
                        
                    }


                }
                else if ((Routen_Zeit.Visibility == Visibility.Visible) && (GlobalData.Tag_ID_Alt != GlobalData.Tag_ID))
                {
                    GlobalData.Tag_ID_Alt = GlobalData.Tag_ID;
                    Scan();
                }

            }
        }

        /// <summary>
        /// Wird aufgerufen, wenn diese Seite in einem Frame angezeigt werden soll.
        /// </summary>
        /// <param name="e">Ereignisdaten, die beschreiben, wie diese Seite erreicht wurde.
        /// Dieser Parameter wird normalerweise zum Konfigurieren der Seite verwendet.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void textBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {
            
        }


        private async void Scan () 
        {
            HttpContent content = new FormUrlEncodedContent(new[]             // POST inhalt vorbereiten
{
                new KeyValuePair<string, string>("NfcID", GlobalData.Tag_ID),
                new KeyValuePair<string, string>("uid", GlobalData.Pers_ID),                // Rute bla abrufen noch nicht implementiert // in arbeit 20.05.2016 
                new KeyValuePair<string, string>("PlanID", GlobalData.Plan_ID),
                new KeyValuePair<string, string>("RoutenID", GlobalData.Routen_ID),
            });
            string Ziel = "/NfcScan.php?";
           string Antwort = await Globafunctions.HttpAbfrage(Ziel, content);
           
        }

        private async void button1_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog msgboxx = new MessageDialog("Parameter" + GlobalData.Tag_ID);
            await msgboxx.ShowAsync();
 
            

        }


        private void textBlock_SelectionChanged_1(object sender, RoutedEventArgs e)
        {
            
        }

        private void textBlock1_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void Service_Click(object sender, RoutedEventArgs e)
        {
           // Frame.Navigate(typeof(Service));
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void RoutenAbfragen_Click(object sender, RoutedEventArgs e)
        {
            string Ziel = "/Routen.php?";
            HttpContent content = new FormUrlEncodedContent(new[]             // POST inhalt vorbereiten
            {
                new KeyValuePair<string, string>("MitarbeiterID", GlobalData.Pers_ID ),                // Rute bla abrufen noch nicht implementiert // in arbeit 20.05.2016 
                           
                new KeyValuePair<string, string>("Startdatum",  DateTime.Now.Year.ToString()+","+DateTime.Now.Month.ToString()+","+DateTime.Now.Day.ToString())             // übermittelt das aktuelle datum im format JJJJ-MM-TT   
                
            });
            Data = await Globafunctions.HttpAbfrage(Ziel, content);                            // wenn der vorherige schritt ferig ist ab in den String damit
            MessageDialog msgboxRoute = new MessageDialog(Data);
            await msgboxRoute.ShowAsync();

            if (!string.IsNullOrWhiteSpace(Data)) // nur Wenn du eine Antwort erhälst die nicht leer ist oder nur aus leerzeichen enthält bekommst.
            {
                listBox.Items.Clear();
                String = Data.Split(' ');

                GlobalData.Plan_ID = String[0];
                GlobalData.Routen_ID = String[1];
                
                GlobalData.Startzeit = String[2];
                string Ziel2 = "/RoutenDetails.php?";
                
                HttpContent content2 = new FormUrlEncodedContent(new[]             // POST inhalt vorbereiten
                 {
                        new KeyValuePair<string, string>("RoutenID",  GlobalData.Routen_ID),            // übermittelt die empfangene RoutenID
                        new KeyValuePair<string, string>("Startdatum",  DateTime.Now.Year.ToString()+"-"+DateTime.Now.Month.ToString()+"-"+DateTime.Now.Day.ToString())             // übermittelt das aktuelle datum im format JJJJ-MM-TT   
                });
                String Data2 = await Globafunctions.HttpAbfrage(Ziel2, content2);
                String[] String2 = Data2.Split('X');                              // den String in seine einzelnen Segmente zerlgen
                int x = String2.Length - 2;
                int i = 0;
                foreach (string part in String2)
                {

                    String[] NFCDetails = part.Split(' ');                         // trenne den String an Whitespaces 
                    if (NFCDetails[0] != "")                                     //für jedes segment des string einmal bitte die relevanten daten speichern
                    {
                        NFCStack[i].ID = NFCDetails[0];
                        NFCStack[i].Bild = NFCDetails[1];
                        NFCStack[i].Zeit = NFCDetails[2];
                        listBox.Items.Add(NFCDetails[0]);

                        if (i < x)
                        {
                            i++;
                        }
                    }
                    GlobalData.I = 0;
                    RoutenAbfragen.IsEnabled = false;
                    LogOut.IsEnabled = false;
                    RouteStarten.IsEnabled = true;
                }

                var BaseAddress = new Uri(GlobalData.Uri2 + "/Bild.php?ID=" + listBox.Items.ElementAt(0));

                Ansicht.Navigate(BaseAddress);
            }
        }

        private async void LogOut_Click(object sender, RoutedEventArgs e)
        {
            string Ziel = "/logout.php?";
            GlobalData.Login_Enable = false;
            HttpContent content = new FormUrlEncodedContent(new[]             // POST inhalt vorbereiten
            {
                new KeyValuePair<string, string>("uid", GlobalData.Pers_ID )                // logout un login in datenbank wider verfügbar machen // in arbeit 20.05.2016 
            });
            Data = await Globafunctions.HttpAbfrage(Ziel,content);             // übergabe an ausführenden Task
            MessageDialog msgboxRoute = new MessageDialog(Data);
            await msgboxRoute.ShowAsync();

            GlobalData.StringPasswd = "";                                       // Speicher leer machen
            GlobalData.HttpResponse = "";
            GlobalData.Login = "";
            GlobalData.Passwdsha1 = "";
            GlobalData.Pers_ID = "";
            GlobalData.Antwort = "";
            GlobalData.MyDebug = false;
            GlobalData.Rute = "";
            GlobalData.SessionID = "";
            GlobalData.Tag_ID = "";
            GlobalData.Tag_ID2 = "";
            //GlobalData.Uri2 = "";

            


            Frame.Navigate(typeof(MainPage));
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Nachrichten));
        }

        public async void Starten(object sender, RoutedEventArgs e)
        {
            
            DispatcherTimer timer = new DispatcherTimer();      // wenn die route gestartet ist starte den timer wenn eine startzeit größer gleich 1 minuten hast
            timer.Interval = TimeSpan.FromSeconds(1);           // daraus ergibt sich das die nidestzeit zwischen 2 Tags 1 Minute beträgt 
            timer.Start();

            
            if (RouteStarten.Content.ToString() == "Fertig")
            {
                
                RoutenAbfragen.IsEnabled = true;
                RouteStarten.IsEnabled = false;                     // route wird durch den letzte Tag wider verfügbar gemacht
                RouteStarten.Content = "Route Starten";
                String Ziel = "/RouteFertig.php?";
                timer.Stop();
                HttpContent content = new FormUrlEncodedContent(new[]             // POST inhalt vorbereiten
{
                new KeyValuePair<string, string>("PlanID", GlobalData.Plan_ID  )                // logout un login in datenbank wider verfügbar machen // in arbeit 20.05.2016 
            });
              String x =  await Globafunctions.HttpAbfrage(Ziel, content);
                LogOut.IsEnabled = true;
            }
            if (RouteStarten.Content.ToString() == "Route Starten" && RouteStarten.IsEnabled == true)
            {
                GlobalData.Timer_Enable = true;
                timer.Tick += timer_Tick;                           // fügt eine Event handler zu der abei jeden tick abgearbeitet wird 
                
                RouteStarten.IsEnabled = false;                     // route wird durch den letzte Tag wider verfügbar gemacht
                RouteStarten.Content = "Fertig";
                Routen_Zeit.Visibility = Visibility.Visible;
            }
        }


    }
}
