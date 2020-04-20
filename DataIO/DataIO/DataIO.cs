using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVIO
{

    //Deal with object Dataset and read/write from CSV files
    public class DataIO
    {
        public static bool Read(String Filename, out ObjDataSet FileStruct, bool header)
        {
            FileStruct = null;
            FileStruct = new ObjDataSet();
            try
            {
                System.IO.StreamReader file = new System.IO.StreamReader(Filename);
                //Read our header (if we have one)
                if (header)
                {
                    String hText = file.ReadLine();
                    String[] Headers = hText.Split(',');
                    foreach (String H in Headers)                
                        FileStruct.AddHeader(H,null);
                }

                String lText;
                String[] Cols;

                while (!file.EndOfStream)
                {
                    lText = file.ReadLine();
                    Cols = lText.Split(',');
                    ObjDataRow R = new ObjDataRow();
                    R.Add(Cols);
                    FileStruct.Add(R);
                }

                file.Close();
                return (true);
            
            }
            catch (Exception E)
            {
                Console.WriteLine("Error reading file: " + E.Message);
                return (false);
            }

            return (false);
        }

        public static bool Write(ObjDataSet FileStruct, String Filename)
        {

            try
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter(Filename);

                //Lets write the header out to a file
                for (Int32 HeaderIndex = 0; HeaderIndex < FileStruct.GetHeaderCount(); HeaderIndex++)
                {
                    file.Write(FileStruct.GetHeaderName(HeaderIndex));
                    if (HeaderIndex < FileStruct.GetColumnCount()-1)
                        file.Write(",");
                    else
                        file.WriteLine("");
                }


                //Lets write the data to a file
                for (Int32 Row = 0; Row < FileStruct.GetRowCount(); Row++)
                {
                    Object[] ObjData = FileStruct.GetRow(Row).GetRowData();
                    
                    for (Int32 O = 0; O < FileStruct.GetColumnCount(); O++)
                    {
                        file.Write(ObjData[O].ToString());
                        if (O < FileStruct.GetColumnCount() - 1)
                            file.Write(",");
                        else
                            file.WriteLine("");
                    }
                    
                }

            file.Close();
            return (true);
            }

            catch(Exception E)
            {
                Console.WriteLine("Failed to Write: " + E.Message);
                return (false);
            }

        }



        /*
         * Compare two CSV structures
         * Output to the differences to a new structure
         */


        public static ObjDataSet Diff(ObjDataSet A, ObjDataSet B, bool CaseSensitive)
        {
            ObjDataSet Output = new ObjDataSet();

            //Check the headers and create structure if they are the same
            if (A.GetHeaderCount() != B.GetHeaderCount())
                return (null);


            for (Int32 I = 0; I < A.GetHeaderCount(); I++)
            {
                String HA = A.GetHeaderName(I);
                String HB = B.GetHeaderName(I);

                if (!CaseSensitive)
                {
                    HA = HA.ToUpper();
                    HB = HB.ToUpper();
                }

                if (HA != HB)
                    return (null);

                else
                    Output.AddHeader(A.GetHeaderName(I),null);
            }


            for (Int32 I = 0; I < A.GetRowCount(); I++)
            {
                if (!B.Contains(A.GetRow(I),CaseSensitive))
                    {
                    Output.Add(A.GetRow(I));
                    }
            }

            return (Output);
        }



    }
}
