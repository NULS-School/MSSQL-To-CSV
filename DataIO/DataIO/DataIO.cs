using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMSExport
{

    //Deal with object Dataset and read/write from CSV files
    public class DataIO
    {
        public bool Read(String Filename, out ObjDataSet FileStruct)
        {
            FileStruct = null;
            return (false);
        }

        public static bool Write(ObjDataSet FileStruct, String Filename)
        {

            try
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter(Filename);

                //Lets write the header out to a file
                for (Int32 HeaderIndex = 0; HeaderIndex < FileStruct.GetColumnCount(); HeaderIndex++)
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
    }


}
