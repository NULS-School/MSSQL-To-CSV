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
            ObjDataSet S;
            DataIO.Read("MyCSV.csv", out S, true);

            //DataIO.Extract(S,new String[2]{"Field1","Field2"});

        }
    }
}
