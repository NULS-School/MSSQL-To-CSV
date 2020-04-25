using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections;

namespace CSVIO
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

        public Object GetRowElement(Int32 index)
        {
            return(Elements[index]);
        }


        public bool Equals(ObjDataRow B, bool CaseSensitive)
        {
            if (Elements.Count() == B.Elements.Count())
                for (Int32 I = 0; I < Elements.Count(); I++)
                {
                    if (!CaseSensitive)
                    {
                        if (!Elements[I].ToString().ToUpper().Equals(B.Elements[I].ToString().ToUpper()))
                            return (false);
                    }
                    else
                    {
                        if (!Elements[I].Equals(B.Elements[I]))
                            return (false);
                    }
                }

        return (true);
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

        public bool ReplaceHeader(String sOld, String sNew)
        {

            KeyValuePair<String, Type> OldValue = new KeyValuePair<string, Type>(sOld, null);
            KeyValuePair<String, Type> NewValue = new KeyValuePair<string, Type>(sNew, null);

            Int32 Position = Headers.IndexOf(OldValue);

            if (Position >= 0)
            {
                Headers[Position] = NewValue;
                return (true);
            }

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
            if (Headers.Count() == 0)
                if (Rows.Count() > 0)
                    return (Rows[0].GetRowData().Count());

            return (Headers.Count());
        }

        public Int32 GetHeaderCount()
        {
            return (Headers.Count());
        }

        public String[] GetHeaders()
        {
            List<String> S = new List<string>();

            for (Int32 I = 0; I < Headers.Count(); I++)
                S.Add(Headers[I].Key);

            return (S.ToArray());
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


        public bool Contains(ObjDataRow R, bool CaseSensitive)
        {
        if(HasRow(R, CaseSensitive))
            return (true);

        return (false);
        }

        private bool HasRow(ObjDataRow R, bool CaseSensitive)
        {
            for (Int32 I = 0; I < Rows.Count(); I++)
            {
                if(Rows[I].Equals(R,CaseSensitive))
                    return (true);
            }
        return (false);
        }

    }


 
    class SQLDataElements
    {
    }
}
