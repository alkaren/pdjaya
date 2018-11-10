using GemBox.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ExcelReader
/// </summary>
public class ExcelReader
{
    public ExcelReader()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static DataTable  ReadExcelFile(byte[] FileBytes)
    {
        try
        {
            var SN = ConfigurationManager.AppSettings["GemboxKey"];
            // If using Professional version, put your serial key below.
            SpreadsheetInfo.SetLicense(SN);
            MemoryStream ms = new MemoryStream(FileBytes);
            ExcelFile ef = ExcelFile.Load(ms, LoadOptions.XlsxDefault);
            DataTable dt = new DataTable("excel");
            int RowCount = 0;
            int ColumnCount = 0;
            // Iterate through all worksheets in an Excel workbook.
            foreach (ExcelWorksheet sheet in ef.Worksheets)
            {
                // Iterate through all rows in an Excel worksheet.
                foreach (ExcelRow row in sheet.Rows)
                {
                    RowCount++;
                    ColumnCount = 0;
                    var rowObject = new List<object>();
                    // Iterate through all allocated cells in an Excel row.
                    foreach (ExcelCell cell in row.AllocatedCells)
                    {
                        ColumnCount++;

                        if (RowCount == 1)
                        {
                            if (cell.Value != null)
                                dt.Columns.Add(cell.Value.ToString());
                            else
                                dt.Columns.Add($"Col-{ColumnCount}");

                        }
                        else
                        {
                            if (cell.Value != null)
                                rowObject.Add(cell.Value.ToString());
                            else
                                rowObject.Add("");

                        }
                    }
                    if (RowCount > 1 && rowObject.Count > 0)
                    {
                        dt.Rows.Add(rowObject.ToArray());
                    }
                }
                break;
            }
            return dt;
        }
        catch
        {
            return null;
        }

    }
}