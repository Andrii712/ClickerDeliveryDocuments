using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ClickerDeliveryDocuments.MicrosoftOfficeUtil
{
    internal static class ExcelUtil
    {
        internal static string OpenFileDialogExcel()
        {
            string fullPathToFile = default;

            // Create OpenFileDialog.
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel files (*.xls;*.xlsx)|*.xls;*xlsx|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;

            // Launch OpenFileDialog by calling ShowDialog method.
            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == true)
                fullPathToFile = openFileDialog.FileName;

            return fullPathToFile;
        }

        internal static string GetTextFromCell(Excel.Range excelRange, int rowIndex, int colIndex)
        {
            dynamic textValue = (excelRange.Cells[rowIndex, colIndex] as Excel.Range).Text;
            return Convert.ToString(textValue);
        }

        internal static Boolean DublicateAreaInDocument(string fileName, Dictionary<string, string> substitutionData)
        {
            bool dublicated = true;

            int defaultWorksheet = 2;
            int startRow = 6;
            int endRow = 20;

            int rowsUsed = -1;
            int colsUsed = -1;

            // Create COM Object.
            Excel.Application excelApp = new Excel.Application
            {
                // Disable displays certain alerts and messages while a macro is running..
                DisplayAlerts = false,
                // Determines whether the object is visible.
                Visible = false
            };

            Excel.Workbook excelWorkbook = excelApp.Workbooks.Open(fileName, ReadOnly: false);
            Excel.Worksheet excelWorksheet = (Excel.Worksheet)excelWorkbook.Worksheets.get_Item(defaultWorksheet);

            Excel.Range workRange = excelWorksheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell);

            rowsUsed = workRange.Row;
            colsUsed = workRange.Column;

            if (endRow == rowsUsed)
            {
                string preDelivery = GetTextFromCell(excelWorksheet.UsedRange, startRow + 4, colsUsed);
                //if (!String.IsNullOrEmpty(preDelivery))
                //{
                //    dublicated = false;
                //}
                //else
                //{
                    Excel.Range copyOrigin = excelWorksheet.Range[
                        (Excel.Range)excelWorksheet.Cells[1, colsUsed], 
                        (Excel.Range)excelWorksheet.Cells[endRow, colsUsed]];

                    int shiftColNum = colsUsed + 1;
                    Excel.Range destinationRange  = excelWorksheet.Range[
                        (Excel.Range)excelWorksheet.Cells[1, shiftColNum], 
                        (Excel.Range)excelWorksheet.Cells[endRow, shiftColNum]];

                    copyOrigin.Copy(destinationRange); //copy to clipboard.
                    destinationRange.ColumnWidth = "11";

                    //destinationRange.PasteSpecial(Microsoft.Office.Interop.Excel.XlPasteType.xlPasteAll,
                    //                            Microsoft.Office.Interop.Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone,
                    //                            false,
                    //                            false);

                    string reprPosition = GetTextFromCell(excelWorksheet.UsedRange, startRow, colsUsed);
                    int position;
                    if (Int32.TryParse(reprPosition, out position))
                    {
                        position++;
                        excelWorksheet.Cells[startRow, shiftColNum] = position;
                    }
                    else
                    {
                        excelWorksheet.Cells[startRow, shiftColNum] = colsUsed;
                    }
                
                    excelWorksheet.Cells[startRow + 2, shiftColNum] = substitutionData["DispatchDate"];
                    excelWorksheet.Cells[startRow + 3, shiftColNum] = substitutionData["Qty"];
                    excelWorksheet.Cells[startRow + 4, shiftColNum] = substitutionData["Delivery"];
                    excelWorksheet.Cells[startRow + 14, shiftColNum] = substitutionData["ReceiptDate"];

                    // Release com objects to fully kill excel process from running in the background.
                    Marshal.FinalReleaseComObject(copyOrigin);
                    Marshal.FinalReleaseComObject(destinationRange);
                //}
            }
            else
            {
                dublicated = false;
            }

            // Cleanup.
            CallGarbageCollector();

            // Release com objects to fully kill excel process from running in the background.
            Marshal.FinalReleaseComObject(workRange);
            Marshal.FinalReleaseComObject(excelWorksheet);

            // Close and release.
            excelWorkbook.Save();
            excelWorkbook.Close(true);
            Marshal.FinalReleaseComObject(excelWorkbook);

            // Quit and release.
            excelApp.Quit();
            Marshal.FinalReleaseComObject(excelApp);

            return dublicated;
        }

        private static void CallGarbageCollector()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
