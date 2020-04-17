using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMSExport;


namespace SIMSExport
{
    class SQLQuery
    {
        SqlConnection conn;

        public bool Connect(String connectionString)
        {
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                return (true);
            }
            catch(Exception E)
            {
                Logging.LogException(E);
            }

        return (false);
        }



        //Export a data set from our SQL server query
        public bool ExecuteQuery(string SQLStatement, out ObjDataSet ResultDataSet)
        {
            if (conn.State == System.Data.ConnectionState.Open)
            {
                SqlCommand cmd = new SqlCommand(SQLStatement, conn);
                SqlDataReader dataReader;

                dataReader = cmd.ExecuteReader();

                ObjDataRow Row;
                ObjDataSet Results = new ObjDataSet();

                //Get the headers
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    Results.AddHeader(dataReader.GetName(i),dataReader.GetFieldType(i));
                }
                


                //Store the data
                while (dataReader.Read())
                {
                    Object[] values = new Object[dataReader.FieldCount];
                    dataReader.GetValues(values);
                    Row = new ObjDataRow();
                    Row.Add(values);
                    Results.Add(Row);

                    values = null;
                }
                dataReader.Close();
                cmd.Dispose();
                conn.Close();
                ResultDataSet = Results;
            }
            else
            {
                Logging.Write("Tried to execute statement whilst not correct state, try later");
                ResultDataSet = null;
            }

            return (false);
        }
    }
}
