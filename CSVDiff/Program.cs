using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using CSVIO;

namespace CSVDiff
{
    class Program
    {
        static void Main(string[] args)
        {

            //Compare 2 CSV files
            //Output the differences (either removed or added)
            ObjDataSet A,B;
            bool TwoWay = false;
            String sFileOne="";
            String sFileTwo="";
            String[] OutputFile = new string[2];
            bool Echo = false;
            String O;

            foreach (String Param in args)
            {
                String[] Options = Param.ToLower().Split(':');
                if (Options.Count() == 2)
                {
                    O = Options[0].ToUpper();
                    if (O == "-OUTPUT")
                        TwoWay = true;
                    else if (O == "-A")
                        sFileOne = Options[1];
                    else if (O == "-B")
                        sFileTwo = Options[1];
                    else if (O == "-OA")
                        OutputFile[0] = Options[1];
                    else if (O == "-OB")
                        OutputFile[1] = Options[1];
                }
                if (Options.Count() == 1)
                {
                    O = Options[0].ToUpper();
                    if (O == "-ONEWAY")
                        TwoWay = false;
                    else if (O == "-TWOWAY")
                        TwoWay = true;
                    else if (O == "-D")
                        Echo = true;
                }
            }

            DataIO.Read(sFileOne,out A,true);
            DataIO.Read(sFileTwo, out B,true);


            //One way only

            ObjDataSet OutputA = DataIO.Diff(A, B, false);
            Console.WriteLine(sFileOne + " has " + OutputA.GetRowCount() + " additionl records ");
            DataIO.Write(OutputA, OutputFile[0]);
            if (Echo)
                DataIO.Dump(OutputA);
             
            if(TwoWay)
                {
                ObjDataSet OutputB = DataIO.Diff(B, A, false);
                Console.WriteLine("\n" + sFileTwo + " has " + OutputB.GetRowCount() + " additionl records ");
                DataIO.Write(OutputB, OutputFile[0]);
                if (Echo)
                    DataIO.Dump(OutputB);
                }

        }

        public static bool Usage()
        {
            Console.WriteLine("Usage: -A:<FileA> -B:<FileB> <Options>");
            Console.WriteLine("Options:");
            Console.WriteLine("-oneway (default)");
            Console.WriteLine("-twoway");
            Console.WriteLine("-oA:<filename> - output differences in A compared with B to file");
            Console.WriteLine("-oB:<filename> - output differences in B compared with A to file");
            Console.WriteLine("-d \tOutput difference to the console");
            return (true);
        }
    }
}
