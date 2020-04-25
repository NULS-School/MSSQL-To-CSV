using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVIO
{


    //Deal with object Dataset and read/write from CSV files
    //Params:
    //Filename      - Input filename
    //FileStruct    - Output File Struct object
    //Header        - Does the file contain a header

    public class DataIO
    {

        private const Int32 SUCCESS = 0;
        private const Int32 FAILURE = -1;


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
                        FileStruct.AddHeader(H, null);
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
                    if (HeaderIndex < FileStruct.GetColumnCount() - 1)
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

            catch (Exception E)
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
                    Output.AddHeader(A.GetHeaderName(I), null);
            }


            for (Int32 I = 0; I < A.GetRowCount(); I++)
            {
                if (!B.Contains(A.GetRow(I), CaseSensitive))
                {
                    Output.Add(A.GetRow(I));
                }
            }

            return (Output);
        }



        //Output the CSV data to the console
        //
        public static bool Dump(ObjDataSet A)
        {

            for (Int32 I = 0; I < A.GetHeaderCount(); I++)
            {
                Console.Write(A.GetHeaderName(I));
                if (I < A.GetColumnCount())
                    Console.Write(",");

            }
            Console.WriteLine("");

            for (Int32 I = 0; I < A.GetRowCount(); I++)
            {
                ObjDataRow R = A.GetRow(I);

                Int32 Elementcnt=1;
                foreach (String S in A.GetRow(I).GetRowData())
                {
                    Console.Write(S);
                    if (Elementcnt < A.GetColumnCount())
                        Console.Write(",");

                    Elementcnt++;
                }
            Console.WriteLine("");
                
            }
        return (true);
        }


        //Extract selected Fields from a given CSV file
        //
        public static ObjDataSet Extract(ObjDataSet A, String[] Fields)
        {
            ObjDataSet Result = new ObjDataSet();
            List<int> Indexes = new List<int>();

            //Check which headers we need and the Index's of columns
            for (Int32 I = 0; I < A.GetHeaderCount(); I++)
            {
                foreach (String H in Fields)
                {
                    if (H.ToUpper() == A.GetHeaderName(I).ToUpper())
                    {
                        Result.AddHeader(A.GetHeaderName(I), null);
                        Indexes.Add(I);
                    }
                }
            }

            //Get the row data
            for (Int32 I = 0; I < A.GetRowCount(); I++)
            {

                ObjDataRow R = new ObjDataRow();
                List<Object> L = new List<object>();
                Object []RData = A.GetRow(I).GetRowData();

                for (Int32 ColData = 0; ColData < RData.Count(); ColData++)
                {
                    if (Indexes.Contains(ColData))
                        L.Add(RData[ColData]);
                }

            R.Add(L.ToArray());
            Result.Add(R);
            }

            
        return (Result);
        }



        public static Int32 GetHeaderIndex(ObjDataSet A, String HeaderName)
        {
            for (Int32 I = 0; I < A.GetHeaderCount(); I++)
            {
                if (A.GetHeaderName(I).ToUpper() == HeaderName.ToUpper())
                    return (I);
            }

        return (FAILURE);
        }


        /* Merge two tables
         * Take all columns from A and B
         * Use A as the master index
         * Merge fields based on lookups between field A1 and B1
         */


        private static ObjDataRow FindRow(ObjDataSet A, Int32 cIndex, string Value)
        {
            ObjDataRow R = null;

            for (Int32 I = 0; I < A.GetRowCount(); I++)
            {
                R = A.GetRow(I);
                Object O = R.GetRowElement(cIndex);
                if (O.ToString().ToUpper() == Value.ToUpper())
                    return (R);           
            }

        return (null);
        }


        public static ObjDataSet Merge(ObjDataSet A, ObjDataSet B, String aFieldName,String bFieldname)
        {
            ObjDataSet Result = new ObjDataSet();

            Int32 aIndex = GetHeaderIndex(A, aFieldName);
            Int32 bIndex = GetHeaderIndex(A, aFieldName);

            //Need to merge headers - excluding the index from B
            //Add the Headers from A
            for (Int32 I = 0; I < A.GetHeaderCount(); I++)
                Result.AddHeader(A.GetHeaderName(I), null);
        

            //Add the headers from B
            for (Int32 I = 0; I < B.GetHeaderCount(); I++)
            {
                if(I != bIndex)
                    Result.AddHeader(B.GetHeaderName(I),null);
            }


            //Merge the data

            for (Int32 I = 0; I < A.GetRowCount(); I++)
            {
                ObjDataRow AR = A.GetRow(I);
                String AData = AR.GetRowElement(aIndex).ToString();

                ObjDataRow BR = FindRow(B, bIndex, AData);
                if (BR != null)
                {
                    List<Object> NewRowData = new List<Object>();
                    Object[] ExistingRowData = BR.GetRowData();

                    for (Int32 J = 0; J < ExistingRowData.Count(); J++)
                    {
                        if(J != bIndex)
                            NewRowData.Add(ExistingRowData[J]);
                    }
                    AR.Add(NewRowData.ToArray());
                    Result.Add(AR);
                }

            }


            
            return (Result);
        }



    }
}
