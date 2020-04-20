using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMSExport;

using CSVIO;

/*
 * Parameters
 * -S:<Settings File>
 * -T:<Script file>
 * -V verbose option
 * 
 */


namespace SIMSExport
{
    class Program
    {
        static void Main(string[] args)
        {
            String sSettingsFile = "Settings.txt";
            String sScriptFile = "";

            foreach (String Param in args)
            {
                String[] Options = Param.ToLower().Split(':');
                if (Options.Count() == 2)
                {
                    if (Options[0] == "-s")
                        sSettingsFile = Options[1];
                    if (Options[0] == "-t")
                        sScriptFile = Options[1];
                    if (Options[0] == "-v")
                        Logging.SetEcho(true);
                }
            }

            Logging.SetEcho(true);
            Logging.Write("Loading Settings ....");

            Settings ProgramSettings = new Settings();
            if(!ProgramSettings.Load(sSettingsFile))
                {
                Logging.Write("Failed to open settings file");
                return;
                }
            
            Settings ScriptSettings = new Settings();
            if (!ScriptSettings.Load(sScriptFile))
            {
                Logging.Write("Failed to open script file");
                return;
            }


            SQLQuery S = new SQLQuery();
            if (S.Connect(ProgramSettings.GetValue("ConnectionString")))
            {
                S.ExecuteQuery(ScriptSettings.GetValue("Query"),out ObjDataSet Results);
                DataIO.Write(Results,ScriptSettings.GetValue("Output"));
                Console.WriteLine("Exported " + Results.GetRowCount() + " records");            
            }

            


        }
    }
}
