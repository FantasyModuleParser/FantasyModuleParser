using FantasyModuleParser.Spells.Models;
using Newtonsoft.Json;
using System;
using System.IO;

namespace FantasyModuleParser.Spells.Services
{
    public class SpellService : ISpellService
    {
        public SpellModel Load(string defaultFolderPath)
        {
            //Placeholder for final SpellModel object
            SpellModel spellModel = null;

            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();

            openFileDialog.InitialDirectory = defaultFolderPath;
            openFileDialog.Filter = "Image files (*.spl)|*.spl|All files (*.*)|*.*";

            // Launch OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = openFileDialog.ShowDialog();
            // Get the selected file name and display in a TextBox.
            // Load content of file in a TextBlock
            if (result == true)
            {
                string jsonData = File.ReadAllText(@openFileDialog.FileName);
                spellModel = JsonConvert.DeserializeObject<SpellModel>(jsonData, new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
            }

            return spellModel;
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Save(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}
