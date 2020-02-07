using ClickerDeliveryDocuments.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace ClickerDeliveryDocuments.Utilities
{
    internal static class ExcelUtil
    {
        #region Fields
        /// <summary>
        /// Conteines the instance of Microsoft.Office.Interop.Excel.Application object.
        /// </summary>
        internal static Excel.Application ExcelApp { get; private set; } = null;

        /// <summary>
        /// The List of Excel file extensions.
        /// </summary>
        private static List<string> filesExtensions = new List<string> { ".xls", ".xlsx" };
        #endregion


        #region ExcelFilesIO
        internal static string[] GetFilesCheckingPlans(string path, string pattern)
        {
            string searchPattern = $"{pattern}.*";
            return Directory.GetFiles(path, searchPattern, SearchOption.AllDirectories)
                .Where(f => filesExtensions.IndexOf(System.IO.Path.GetExtension(f)) >= 0)
                .ToArray();
        }

        internal static string OpenFileDialogExcel()
        {
            // Create OpenFileDialog.
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel files (*.xls;*.xlsx)|*.xls;*xlsx|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;

            // Launch OpenFileDialog by calling ShowDialog method.
            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == true)
                return openFileDialog.FileName;
            else
                return String.Empty;
        }
        #endregion


        #region CommonActionsWithExcelApplication
        /// <summary>
        /// Instantiate new Excel object.
        /// </summary>
        internal static void InitializeObjectExcelApplication()
        {
            ExcelApp = GetInstanceExcelApplication();
        }

        /// <summary>
        /// Closes COM-Object of the Excel application and  releases all referances 
        /// at the current Excel application to a RCW by setting its reference count to 0.
        /// </summary>
        internal static void CloseObjectExcelApplication()
        {
            if (ExcelApp != null)
            {
                // Quit and release.
                ExcelApp.Quit();
                Marshal.FinalReleaseComObject(ExcelApp);
                ExcelApp = null;
            }
        }

        /// <summary>
        /// Creates new COM-Object of the Excel application.
        /// </summary>
        /// <param name="visible">The flag indicates to show the Excel application or not.</param>
        /// <returns>Returns new instance of Microsoft.Office.Interop.ExcelAplication.</returns>
        internal static Excel.Application GetInstanceExcelApplication(bool visible = false)
        {
            try
            {
                // Create COM Object.
                return new Excel.Application
                {
                    // Disable displays certain alerts and messages while a macro is running.
                    DisplayAlerts = false,
                    // Determines whether the object is visible.
                    Visible = visible
                };
            }
            catch (Exception ex)
            {
                DataUtil.ShowErrorMessage($"Cannot create an Excel COM Object." +
                    $"\n {ex.Message} " +
                    $"\nThe current action will be canceled.");
                return null;
            }
        }

        /// <summary>
        /// Reads a text of the specific cell.
        /// </summary>
        /// <param name="excelRange">The current cells range in Excel-document.</param>
        /// <param name="rowIndex">The current row index.</param>
        /// <param name="colIndex">The current col index.</param>
        /// <returns>System.String representation of text in the cell.</returns>
        internal static string GetTextFromCell(Excel.Range excelRange, int rowIndex, int colIndex)
        {
            dynamic textValue = (excelRange.Cells[rowIndex, colIndex] as Excel.Range).Text;
            return Convert.ToString(textValue);
        }
        #endregion


        #region ActionsWithExcelDocument
        /// <summary>
        /// Duplicates the last area in the document.
        /// </summary>
        /// <param name="fileName">The full file name (The checking plan file  name).</param>
        /// <param name="substitutionData">Data for substitution in the area of the Excel-document.</param>
        /// <returns></returns>
        internal static Boolean DublicateAreaInDocument(string fileName, DeliveryItemModel substitutionData)
        {
            // The flag which indicates that the aria was duplicated.
            bool dublicated = true;

            // Dafault values for the document.
            int startRow = 6;
            int endRow = 20;

            // The last used row and column in the document.
            int rowsUsed = -1;
            int colsUsed = -1;

            if (System.Windows.Application.Current is ClickerDeliveryDocuments.App clickerApp)
            {
                // Exit from method when the application object equal null.
                if (ExcelApp == null)
                {
                    dublicated = false;
                }
                else
                {
                    Excel.Workbook excelWorkbook = ExcelApp.Workbooks
                        .Open(fileName, ReadOnly: false);
                    Excel.Worksheet excelWorksheet = (Excel.Worksheet)excelWorkbook.Worksheets
                        .get_Item(clickerApp.clicker.CheckingPlanWorksheet);

                    //Excel.Range workRange = excelWorksheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
                    //rowsUsed = workRange.Row;
                    //colsUsed = workRange.Column;

                    rowsUsed = ((Excel.Range)excelWorksheet.Cells[excelWorksheet.Rows.Count, 1])
                        .End[Excel.XlDirection.xlUp].Row;
                    colsUsed = ((Excel.Range)excelWorksheet.Cells[startRow, excelWorksheet.Columns.Count])
                        .End[Excel.XlDirection.xlToLeft].Column;

                    // Checking the correctness of filling in the document. 
                    if (endRow == rowsUsed)
                    {
                        // Get the number of the previous delivery.
                        string preDelivery = GetTextFromCell(excelWorksheet.UsedRange, startRow + 4, colsUsed);
                        if (String.IsNullOrEmpty(preDelivery))
                        {
                            dublicated = false;
                        }
                        else
                        {
                            Excel.Range copyOrigin = excelWorksheet.Range[
                                (Excel.Range)excelWorksheet.Cells[startRow, colsUsed],
                                (Excel.Range)excelWorksheet.Cells[endRow, colsUsed]];

                            int shiftColNum = colsUsed + 1;
                            Excel.Range destinationRange = excelWorksheet.Range[
                                (Excel.Range)excelWorksheet.Cells[startRow, shiftColNum],
                                (Excel.Range)excelWorksheet.Cells[endRow, shiftColNum]];

                            copyOrigin.Copy(destinationRange);
                            destinationRange.ColumnWidth = "11";

                            // Get the text representation of the current position number.
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

                            excelWorksheet.Cells[startRow + 2, shiftColNum] = clickerApp.clicker.DispatchDate;
                            excelWorksheet.Cells[startRow + 3, shiftColNum] = substitutionData.Qty;
                            excelWorksheet.Cells[startRow + 4, shiftColNum] = substitutionData.Delivery;
                            excelWorksheet.Cells[startRow + 14, shiftColNum] = clickerApp.clicker.ReceiptDate;

                            // Release com objects to fully kill excel process from running in the background.
                            Marshal.FinalReleaseComObject(copyOrigin);
                            Marshal.FinalReleaseComObject(destinationRange);
                        }
                    }
                    else
                    {
                        dublicated = false;
                    }

                    // Cleanup.
                    DataUtil.CallGarbageCollector();

                    // Release com objects to fully kill excel process from running in the background.
                    //Marshal.FinalReleaseComObject(workRange);
                    Marshal.FinalReleaseComObject(excelWorksheet);

                    // Close and release.
                    excelWorkbook.Save();
                    excelWorkbook.Close(true);
                    Marshal.FinalReleaseComObject(excelWorkbook);
                }
            }
            return dublicated;
        }

        /// <summary>
        /// Reads the selected excel document.
        /// </summary>
        /// <returns>Collection of delivery data</returns>
        internal static ObservableCollection<DeliveryItemModel> ReadDeliveryExcelFile()
        {
            ObservableCollection<DeliveryItemModel> items = new ObservableCollection<DeliveryItemModel>();

            if (System.Windows.Application.Current is ClickerDeliveryDocuments.App clickerApp)
            {
                // Exit from method when application object equal null.
                if (ExcelApp != null)
                {
                    Excel.Workbook excelWorkbook = ExcelApp.Workbooks
                        .Open(clickerApp.clicker.DeliveryFileName, ReadOnly: true);
                    Excel.Worksheet excelWorksheet = (Excel.Worksheet)excelWorkbook.Worksheets
                        .get_Item(clickerApp.clicker.Worksheet);

                    Excel.Range excelRange = excelWorksheet.UsedRange;

                    // Specify the current row positions for variables.
                    int currentRowDpn = clickerApp.clicker.StartRowDpn;
                    int currentRowQty = clickerApp.clicker.StartRowQty;
                    int currentRowDelivery = clickerApp.clicker.StartRowDelivery;

                    string txtDpn, txtQty, txtDelivery, tmpDelivery;
                    txtDelivery = String.Empty;

                    while ((currentRowDpn <= clickerApp.clicker.EndRowDpn) ||
                        (currentRowQty <= clickerApp.clicker.EndRowQty) ||
                        (currentRowDelivery <= clickerApp.clicker.EndRowDelivery))
                    {
                        // Clearing the variables by setting the blank value.
                        txtDpn = txtQty = tmpDelivery = String.Empty;

                        if (currentRowDpn <= clickerApp.clicker.EndRowDpn)
                        {
                            txtDpn = ExcelUtil.GetTextFromCell(excelRange, currentRowDpn, clickerApp.clicker.ColumnDpn);
                            currentRowDpn++;
                        }

                        if (currentRowQty <= clickerApp.clicker.EndRowQty)
                        {
                            txtQty = ExcelUtil.GetTextFromCell(excelRange, currentRowQty, clickerApp.clicker.ColumnQty);
                            currentRowQty++;
                        }

                        if (currentRowDelivery <= clickerApp.clicker.EndRowDelivery)
                        {
                            tmpDelivery = ExcelUtil.GetTextFromCell(excelRange, currentRowDelivery, clickerApp.clicker.ColumnDelivery);
                            if (!String.IsNullOrEmpty(tmpDelivery))
                                txtDelivery = tmpDelivery;
                            currentRowDelivery++;
                        }

                        // Skips the row which has the empty Dpn or Quantity.
                        if (String.IsNullOrEmpty(txtDpn) || String.IsNullOrEmpty(txtQty))
                            continue;

                        items.Add(new DeliveryItemModel
                        {
                            Dpn = txtDpn,
                            Qty = txtQty,
                            Delivery = txtDelivery
                        });
                    }

                    // Cleanup.
                    DataUtil.CallGarbageCollector();

                    // Release com objects to fully kill excel process from running in the background.
                    Marshal.FinalReleaseComObject(excelRange);
                    Marshal.FinalReleaseComObject(excelWorksheet);

                    // Close and release.
                    excelWorkbook.Close(false);
                    Marshal.ReleaseComObject(excelWorkbook);
                }
            }
            return items;
        }

        /// <summary>
        /// Display the excel document.
        /// </summary>
        /// <param name="fileName">The full filename which will be open.</param>
        /// <param name="worksheetIndex">The index of worksheet in document which will show.</param>
        internal static void ShowExcelDocument(string fileName, int worksheetIndex)
        {
            Excel.Application visibleApp = GetInstanceExcelApplication();

            if (visibleApp != null)
            {
                Excel.Workbook excelWorkbook = visibleApp.Workbooks.Open(fileName, ReadOnly: false);
                Excel.Worksheet excelWorksheet = (Excel.Worksheet)excelWorkbook.Worksheets.get_Item(worksheetIndex);
                excelWorksheet.Activate();
                visibleApp.Visible = true;
            }
        }
        #endregion


        

    }
}
