using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections;

namespace SIMSExport
{

    public class ObjDataRow
    {
        List<Object> Elements;

        public ObjDataRow()
        {
            Elements = new List<object>();
        }

        ~ObjDataRow()
        {
        }

        public bool Add(Object[] Values)
        {
            Elements.AddRange(Values);
            return (true);
        }

        public Object[] GetRowData()
        {
            Object[] O = Elements.ToArray();
            return (O);
        }
    }

    public class ObjDataSet
    {
        List<KeyValuePair<string, Type>> Headers;
        List<ObjDataRow> Rows;


        public ObjDataSet()
        {
            Headers = new List<KeyValuePair<string, Type>>();
            Rows = new List<ObjDataRow>();
        }

        ~ObjDataSet()
        {
        }

        public bool AddHeader(string Name, Type DataType)
        {
            KeyValuePair<String, Type> K = new KeyValuePair<String, Type>(Name, DataType);
            Headers.Add(K);
            return (false);
        }

        public string GetHeaderName(Int32 index)
        {
            return (Headers[index].Key);
        }



        public Type GetFieldType(Int32 index)
        {
            return (Headers[index].Value);
        }

        public Int32 GetColumnCount()
        {
            return (Headers.Count());
        }

        public Int32 GetRowCount()
        {
            return (Rows.Count());
        }

        public ObjDataRow GetRow(Int32 index)
        {
            return (Rows[index]);
        }

        public bool Add(ObjDataRow oRow)
        {
            Rows.Add(oRow);
            return (true);
        }

    }


 
    class SQLDataElements
    {
    }
}
