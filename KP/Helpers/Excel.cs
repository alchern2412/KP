using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Office.Interop.Excel;
using _Excel = Microsoft.Office.Interop.Excel;

namespace KP
{
    public class Excel
    {
        string path = "";
        Application excel = new _Excel.Application();
        Workbook wb;
        Worksheet ws;
        public Excel() { }
        public Excel(string path, int Sheet)
        {
            this.path = path;
            wb = excel.Workbooks.Open(path);
            ws = wb.Worksheets[Sheet];
        }

        public void CreateNewFile()
        {
            this.wb = excel.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            this.ws = wb.Worksheets[1];
        }

        public void CreateNewSheet()
        {
            Worksheet tempsheet = wb.Worksheets.Add(After: ws);
        }

        public string ReadCell(int i, int j)
        {
            i++;
            j++;
            if(ws.Cells[i,j].Value2 != null )
            {
                return ws.Cells[i, j];
            }
            else
            {
                return "";
            }
        }

        public void WriteToCell(int i, int j, string s)
        {
            ws.Cells[i, j].Value2 = s;
        }

        public void Save()
        {
            wb.Save();
        }
        public void SaveAs(string path)
        {
            wb.SaveAs(path);
        }

        public void Close()
        {
            wb.Close();
        }
    }
}
