using FantasyModuleParser.Main.Models;
using FantasyModuleParser.NPC.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace FantasyModuleParser.Main.ViewModels
{
    public class ChangelogViewModel: ViewModelBase
    {
        public List<ChangelogModel> ChangelogList { get; set; }
        public string uri = "https://program.fantasymoduleparser.tech/CHANGELOG.json";
        public ChangelogViewModel()
        {
            ChangelogList = new List<ChangelogModel>();
            run();
        }
        public void run()
        {
            List<ChangelogModel> ChangeLogs = JsonConvert.DeserializeObject<List<ChangelogModel>>(Get(uri));
            ChangelogList = ChangeLogs.Skip(Math.Max(0, ChangeLogs.Count() - 5)).ToList();
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
