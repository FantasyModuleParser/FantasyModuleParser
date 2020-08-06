using FantasyModuleParser.Main.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Input;


namespace FantasyModuleParser.Main
{
    /// <summary>
    /// Interaction logic for Supporters.xaml
    /// </summary>
    public partial class Supporters : Window
    {
        public Supporters()
        {
            InitializeComponent();

            // Enable it so the popup window can close on the Escape key
            PreviewKeyDown += (sender, eventArgs) => { if (eventArgs.Key == Key.Escape) Close(); };
        }
        private void ESExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public string uri = "https://program.fantasymoduleparser.tech/PATRONS.json";
        public void run()
        {
            List<PatronModel> patrons = JsonConvert.DeserializeObject<List<PatronModel>>(Get(uri));
            Console.WriteLine("Length of Patrons: " + patrons.Count);
        }
        public string Get(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
