using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CSVIO;

namespace CSVExtract
{

    class Program
    {
        static void Main(string[] args)
        {

            String sOutputFile = "";
            String sInputFile = "";
            String[] Fields= { };
            String O;
            bool bEcho = false;


            foreach (String Param in args)
            {
                if (Param == "-d")
                    bEcho = true;

                String[] Options = Param.ToLower().Split(':');
                O = Options[0].ToUpper();
                if (O == "-O")
                    sOutputFile = Options[1];
                if (O == "-I")
                    sInputFile = Options[1];
                if (O == "-FIELDS")
                    {
                    Fields = Options[1].Split(',');
                    }
            }


            if((sInputFile=="") || (sOutputFile=="") || Fields.Count()==0)
            {
                Usage();
                return;
            }

            ObjDataSet S;
            DataIO.Read(sInputFile, out S, true);
            ObjDataSet Extracted = DataIO.Extract(S,Fields);
            DataIO.Write(Extracted, sOutputFile);
            if (bEcho)
                DataIO.Dump(Extracted);

        }


        public static void Usage()
        {
            Console.WriteLine("Extract one or more fields from a CSV");
            Console.WriteLine("Usage:");
            Console.WriteLine("<options>");
            Console.WriteLine("options:");
            Console.WriteLine("-i:<output> \tread input from file");
            Console.WriteLine("-o:<output> \twrite the output to file");
            Console.WriteLine("-fields: \tcomma delimited list of fields");
            Console.WriteLine("-d \tdump the data to console also");
        }

    }
}
