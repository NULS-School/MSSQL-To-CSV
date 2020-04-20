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
            DataIO.Read("ParentsA.csv",out A,true);
            DataIO.Read("ParentsB.csv", out B,true);

            ObjDataSet Output = DataIO.Diff(A, B, false);

            DataIO.Write(Output, "Diff.csv");
           

        }
    }
}
