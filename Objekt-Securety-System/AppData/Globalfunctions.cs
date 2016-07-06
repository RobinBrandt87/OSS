
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace Objekt_Securety_System
{
    public class Globafunctions
    {

        public static async Task
        checkserver()
        {
           
            

            var httpClient = new HttpClient();      // Neue httpClient instanz
               
            try
            {
                var Anfrage = new HttpRequestMessage();
               
                Anfrage.RequestUri = new Uri(GlobalData.Uri2 + "/Hallo.php");
                HttpResponseMessage response = await httpClient.SendAsync(Anfrage); // schicke die abfrage an die Url mit cockie im gepaäck dann warte bis antwort komplett und speicher erst mal alles
                GlobalData.Antwort = await response.Content.ReadAsStringAsync();                             // wenn der vorherige schritt ferig ist ab in den String damit
                if (GlobalData.Antwort != "")                                    // auswerten und feritg 
                {
                    GlobalData.Check = true;
                    GlobalData.SessionID = GlobalData.Antwort;
                }
            }
            catch (Exception)
            {
                GlobalData.Check = false;
                throw;
            }
        }

        public static async Task<string> 
        HttpAbfrage(String Ziel, HttpContent content)
        {
           
            //################################################################################################

            var httpClient = new HttpClient();      // Neue httpClient instanz

            //##################################################################################################
            // mit Cockies aber nicht zu ende Programmiert weil wir keine Cockies nutzen 

            CookieContainer cookie = new CookieContainer();             // Cockie Container Construcktor
            HttpClientHandler handler = new HttpClientHandler()         // nutze beim zugriff cockies
            {
            };
            HttpClient client = new HttpClient(handler as HttpMessageHandler) // neuer http client
            {
                BaseAddress = new Uri(GlobalData.Uri2 + Ziel + GlobalData.SessionID)     // hier wird auch gleich die Session an das ziel angehangen                                        // url aus uri 2 nutzen test2.php
            };
            handler.UseCookies = false;                                        // beim zugriff cockies nicht zulassen
            handler.UseDefaultCredentials = false;

            //#################################################################################################
            // Jetzt mit POST
            // Schritt 4 Abfrage abschicken und ergebnis entgegennehmen 
            HttpResponseMessage response = await httpClient.PostAsync(client.BaseAddress, content); // schicke die abfrage an die Url , dann warte bis antwort komplett und speicher erst mal alles
            GlobalData.HttpResponse = await response.Content.ReadAsStringAsync();
           // MessageDialog msgboxRespons = new MessageDialog(GlobalData.HttpResponse);
           // await msgboxRespons.ShowAsync();        // Zeige mir an was angekommen ist 
            return GlobalData.HttpResponse;
        }

    }
}


