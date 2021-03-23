using System;
using System.IO;
using System.Linq;
using System.Data;

namespace fileSort
{
    public class dal
    {
        public static DataSet settings = new DataSet();
        public static string settingsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.xml");
        public static string tableName = "filex";
        private static void saveSettings()
        {
            settings.WriteXml(settingsFile); // Save the settings to settings file
        }

        public static void loadSettings()
        {
            if (File.Exists(settingsFile)) // If settings file exists
            {
                settings.ReadXml(settingsFile); // Load the settings file
                Console.WriteLine("Settings loaded successfully");
            }
            else
            {
                Console.WriteLine("Setings file not found, creating blank one");
                settings.Tables.Add(tableName); // Add table to the dataset
                settings.Tables[tableName].Columns.Add("ex", typeof(String)); // Add column for file extensions to table
                settings.Tables[tableName].Columns.Add("folder", typeof(String)); // Add column for folder names to table
                settings.WriteXml(settingsFile); // Save the new blank settings into the settings file
            }
        }
        public static void addFilex()
        {
            string ex; // For storing file extension
            string folder; // For storing folder name
            Console.Clear();

            Console.WriteLine("Adding file extension to sorting database. Use 'q' for quit");
            Console.Write("File extension: ");
            ex = Console.ReadLine(); // Get file extension from user input in console
            ex = String.Concat(ex.Where(c => !Char.IsWhiteSpace(c))); // Make sure, there is no 'whitespace' in the extension
            if (ex == "" || ex == null) // If there's nothing, call addFilex() again, so user can input proper extension
            {
                Console.Clear();
                Console.WriteLine("Invalid string. Try again.");
                addFilex();
            }
            else if (ex == "q") return; // If input is q, return to quit the function
            else if (!ex.StartsWith(".")) ex = "." + ex; // If use didn't put . in the begining of the file extension, add it

            Console.Clear();
            Console.Write($"Adding folder name for \'{ex}\' extension: ");
            folder = Console.ReadLine();

            if (folder == "q") return;

            DataRow row = settings.Tables[tableName].NewRow(); // Create new row
            row[0] = ex; // Add the file extension to the first column
            row[1] = folder; // Add folder name to second column
            settings.Tables[tableName].Rows.Add(row); // Add the new row to table

            Console.Clear();
            Console.WriteLine("Succefully added to database");
            System.Threading.Thread.Sleep(2000);

            saveSettings(); // Save the new settings

            return;
        }
        public static string folderName(string ex)
        {

            string extension = $"ex = '{ex}'"; // Load the file extension into a format that can be searched in settings table
            string folder = ""; // Initialize folder variable for storing foldername where the file will be moved

            DataRow[] foundRow = settings.Tables[tableName].Select(extension); // Search for the extension in settings table

            foreach (DataRow dr in foundRow) // Foreach row, where the file extension 'ex' exists (should be always one)
            {
                folder = dr["folder"].ToString(); // Get the foldername and store it in 'folder'
            }
            
            return folder;
        }
    }
}
