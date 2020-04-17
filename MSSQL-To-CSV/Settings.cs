using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMSExport
{
    public class Settings
    {
        String Delimeter = "::";
        Dictionary<string, string> Settings_Values = new Dictionary<string, string>();


        public Settings()
        {

        }

        public Settings(String fName)
        {
            Load(fName);
        }

        public bool Load(string sFilename)
        {
            string[] lines;

            try
            {
                lines = System.IO.File.ReadAllLines(sFilename);
            }
            catch (Exception E)
            {
                Logging.Write("Failed to open settings file, please check it exists (" + E.Message + ")");
                return (false);
            }

            foreach (string line in lines)
            {
                String[] sArray = line.Trim().Split(new string[] { Delimeter }, StringSplitOptions.None);
                if(sArray.Count()==2)
                {
                    SetValue(sArray[0].Trim(), sArray[1].Trim());
                    //Logging.Write("Setting Option '" + sArray[0] + "' to '" + sArray[1]+ "'");
                }
            }



        return (true);
        }


        public string GetValue(string sSetting)
        {
            String Value = "";
            if (Settings_Values.TryGetValue(sSetting, out Value))
                return (Value);

        return ("");
        }


        public bool SetValue(string sSetting, string sValue)
        {
            if (Settings_Values.ContainsKey(sSetting))
                Settings_Values.Remove(sSetting);

            Settings_Values.Add(sSetting, sValue);
            return (true);
        }

    }
}
