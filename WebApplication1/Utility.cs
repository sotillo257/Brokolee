using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication1
{
    public static class Utility
    {
        public static DataTable ConvertCSVtoDataTable(string strPath)
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(strPath))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }

                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    if (rows.Length > 1)
                    {
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            dr[i] = rows[i].Trim();
                        }
                        dt.Rows.Add(dr);
                    }
                }

            }


            return dt;
        }
    }
}