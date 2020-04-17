using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMSExport
{
    public static class Logging
    {
        public static string LogFilename = "Log.txt";

        private static bool echo = false;

        public static bool Write(String sText)
        {
            if (echo)
            {
                Console.WriteLine(sText);
            }

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(LogFilename, true))
            {
                string LogLine = GetDateTime() + " - " + sText + "\n";
                file.WriteLine(LogLine);
            }
                
            return (true);
        }


        public static bool LogException(Exception E)
        {
            return (Write(E.Message));
        }



        public static string GetDateTime()
        {
            return(DateTime.Now.ToLocalTime().ToString());
        }

        public static void SetEcho(bool R)
        {
            echo = R;
        }

    }


}
