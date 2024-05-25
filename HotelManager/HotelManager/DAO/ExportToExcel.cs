using System.Windows.Forms;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System;
using Microsoft.Office.Interop.Excel;

namespace HotelManager.DAO
{
    public interface IExportStrategy
    {
        void Export(Excel.Workbook workbook, string path);
    }

    public class ExportToExcel
    {
        private static ExportToExcel instance;

        public static ExportToExcel Instance
        {
            get
            {
                if (instance == null)
                    instance = new ExportToExcel();
                return instance;
            }
        }

        private ExportToExcel() { }

        private IExportStrategy exportStrategy;

        public void SetExportStrategy(IExportStrategy strategy)
        {
            exportStrategy = strategy;
        }

        public void Export(DataGridView dataGridView, string path)
        {
            Excel.Application appExcel = null;
            Excel.Workbook bookExcel = null;
            Excel.Worksheet sheetExcel = null;
            object misValue = System.Reflection.Missing.Value;

            try
            {
                appExcel = new Excel.Application();
                appExcel.DisplayAlerts = false;
                appExcel.Visible = false;

                bookExcel = appExcel.Workbooks.Add();
                sheetExcel = (Excel.Worksheet)bookExcel.Worksheets[1];

                int currentColumn = 0;
                int currentRow = 0;

                for (int i = 0; i < dataGridView.RowCount; i++)
                {
                    if (dataGridView.Rows[i].IsNewRow) continue;
                    else
                    {
                        currentColumn = 0;
                        ++currentRow;
                        for (int j = 0; j < dataGridView.ColumnCount; j++)
                        {
                            if (dataGridView.Columns[j].Visible == false) continue;
                            else
                                sheetExcel.Cells[currentRow, ++currentColumn] = dataGridView[j, i].Value.ToString();
                        }
                    }
                }

                sheetExcel.Columns.AutoFit();
                sheetExcel.Columns.HorizontalAlignment = XlHAlign.xlHAlignLeft;
                sheetExcel.Rows[1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                sheetExcel.Rows[1].Font.Bold = true;
                sheetExcel.Rows[1].Font.Size = 12;

                // Use the selected export strategy
                exportStrategy.Export(bookExcel, path);

                appExcel.Quit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error exporting data to Excel: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Clean up Excel objects
                if (sheetExcel != null)
                    Marshal.ReleaseComObject(sheetExcel);
                if (bookExcel != null)
                    Marshal.ReleaseComObject(bookExcel);
                if (appExcel != null)
                    Marshal.ReleaseComObject(appExcel);
            }
        }
    }

    public class ExportToXLS : IExportStrategy
    {
        public void Export(Workbook workbook, string path)
        {
            workbook.SaveAs(path, XlFileFormat.xlWorkbookNormal);
            workbook.Close();
        }
    }

    public class ExportToXLSX : IExportStrategy
    {
        public void Export(Workbook workbook, string path)
        {
            workbook.SaveAs(path, XlFileFormat.xlOpenXMLWorkbook);
            workbook.Close();
        }
    }

    public class ExportToPDF : IExportStrategy
    {
        public void Export(Workbook workbook, string path)
        {
            workbook.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, path, XlFixedFormatQuality.xlQualityStandard);
            workbook.Close();
        }
    }
}
