using FantasyModuleParser.Main.Models;
using FantasyModuleParser.NPC.ViewModel;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace FantasyModuleParser.Main.ViewModels
{
    public class SupportersViewModel : ViewModelBase
    {
        public List<PatronModel> LevelZeroPatronList { get; set; }
        public List<PatronModel> LevelOnePatronList { get; set; }
        public List<PatronModel> LevelTwoPatronList { get; set; }
        public string uri = "https://program.fantasymoduleparser.tech/PATRONS.json";
        public SupportersViewModel()
        {
            LevelZeroPatronList = new List<PatronModel>();
            LevelOnePatronList = new List<PatronModel>();
            LevelTwoPatronList = new List<PatronModel>();
            run();
        }
        public void run()
        {
            List<PatronModel> AllPatrons = JsonConvert.DeserializeObject<List<PatronModel>>(Get(uri));

            LevelZeroPatronList = AllPatrons.Where(x => x.level == 0).ToList();
            LevelOnePatronList = AllPatrons.Where(x => x.level == 1).ToList();
            LevelTwoPatronList = AllPatrons.Where(x => x.level == 2).ToList();
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
