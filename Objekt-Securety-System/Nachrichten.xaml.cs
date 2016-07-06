using System;
using System.Collections.Generic;
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
    public sealed partial class Nachrichten : Page
    {
        public Nachrichten()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Wird aufgerufen, wenn diese Seite in einem Frame angezeigt werden soll.
        /// </summary>
        /// <param name="e">Ereignisdaten, die beschreiben, wie diese Seite erreicht wurde.
        /// Dieser Parameter wird normalerweise zum Konfigurieren der Seite verwendet.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            String Antwort = "";
            string Ziel = "/Nachricht.php?";

            HttpContent content = new FormUrlEncodedContent(new[]             // POST inhalt vorbereiten
            {
                new KeyValuePair<string, string>("NachrichtHead", GlobalData.Pers_ID),
                new KeyValuePair<string, string>("Betreff",textBox1.Text),
                new KeyValuePair<string, string>("inhalt", textBox.Text),
                    });
            // Schritt 4 Abfrage abschicken und ergebnis entgegennehmen 
            try
            {

                Antwort = await Globafunctions.HttpAbfrage(Ziel, content);
            }
            catch
            {
                MessageDialog msgbox = new MessageDialog("Nachricht konnte nicht Versendet werden");
                await msgbox.ShowAsync();

            }
            
            MessageDialog msgbox1 = new MessageDialog(Antwort);
            await msgbox1.ShowAsync();
            Frame.Navigate(typeof(Intern));
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Intern));
        }
    }
}
