using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringSuite
{
    class FolderCreation
    {
        public static string BaseDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EngineerSuite2");
        public static void CreateSomeSubfolder()
        {
            var path = Path.Combine(BaseDirectory, "NPC Engineer");
            // Create Folder for NPC Engineer
        }
        public static void CreateSomeSubfolder1()
        {
            var path = Path.Combine(BaseDirectory, "Table Engineer");
            // Create Folder for Table Engineer
        }
        public static void CreateSomeSubfolder2()
        {
            var path = Path.Combine(BaseDirectory, "Parcel Engineer");
            // Create Folder for Parcel Engineer
        }
        public static void CreateSomeSubfolder3()
        {
            var path = Path.Combine(BaseDirectory, "Artifact Engineer");
            // Create Folder for Artifact Engineer
        }
        public static void CreateSomeSubfolder4()
        {
            var path = Path.Combine(BaseDirectory, "Equipment Engineer");
            // Create Folder for Equipment Engineer
        }
        public static void CreateSomeSubfolder5()
        {
            var path = Path.Combine(BaseDirectory, "Spell Engineer");
            // Create Folder for Spell Engineer
        }
        public static void CreateSomeSubfolder6()
        {
            var path = Path.Combine(BaseDirectory, "Class Engineer");
            // Create Folder for Class Engineer
        }
        public static void CreateSomeSubfolder7()
        {
            var path = Path.Combine(BaseDirectory, "Background Engineer");
            // Create Folder for Background Engineer
        }
        public static void CreateSomeSubfolder8()
        {
            var path = Path.Combine(BaseDirectory, "RefMan Engineer");
            // Create Folder for Reference Manual Engineer
        }
        public static void CreateSomeSubfolder9()
        {
            var path = Path.Combine(BaseDirectory, "Feats Engineer");
            // Create Folder for Feats Engineer
        }
    }
}
