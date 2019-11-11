using Excel = Microsoft.Office.Interop.Excel;
using ClickerDeliveryDocuments.MicrosoftOfficeUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace ClickerDeliveryDocuments
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private string PathDeliveryFile { get; set; }


        #region FormFieldsEventHandler
        private void DpnColTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string txtIndexDpnCol = ((System.Windows.Controls.TextBox)sender).Text;
            int intIndexDpnCol;

            if (Int32.TryParse(txtIndexDpnCol, out intIndexDpnCol))
            {
                QtyColTextBox.Text = Convert.ToString(++intIndexDpnCol, DataUtil.GetCultureInfoProvider());
                DeliveryColTextBox.Text = Convert.ToString(++intIndexDpnCol, DataUtil.GetCultureInfoProvider());
            }
        }

        private void DpnStartRowTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            QtyStartRowTextBox.Text = DeliveryStartRowTextBox.Text = ((System.Windows.Controls.TextBox)sender).Text;
        }

        private void DpnEndRowTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            QtyEndRowTextBox.Text = DeliveryEndRowTextBox.Text = ((System.Windows.Controls.TextBox)sender).Text;
        }
        #endregion

        #region FormButtonClick
        private void BrowseDeliveryFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(WorksheetTextBox.Text) &&
               !String.IsNullOrEmpty(DpnColTextBox.Text) &&
               !String.IsNullOrEmpty(DpnStartRowTextBox.Text) &&
               !String.IsNullOrEmpty(DpnEndRowTextBox.Text) &&
               !String.IsNullOrEmpty(QtyColTextBox.Text) &&
               !String.IsNullOrEmpty(QtyStartRowTextBox.Text) &&
               !String.IsNullOrEmpty(QtyEndRowTextBox.Text) &&
               !String.IsNullOrEmpty(DeliveryColTextBox.Text) &&
               !String.IsNullOrEmpty(DeliveryStartRowTextBox.Text) &&
               !String.IsNullOrEmpty(DeliveryEndRowTextBox.Text))
            {
                PathDeliveryFile = ExcelUtil.OpenFileDialogExcel();
                if (!String.IsNullOrEmpty(PathDeliveryFile)) 
                {
                    DeliveryData.Items.Clear();
                    ReadExcelFile();
                }
            }
            else
            {
                ShowWorningInputParameters();
            }
        }

        private void ListViewItemChooseButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            var context = new Dictionary<string, string>();

            foreach (var prop in button.DataContext.GetType().GetProperties())
            {
                context.Add(prop.Name, prop.GetValue(button.DataContext, null).ToString());
            };

            var confirmationWindow = new ConfirmationWindow();
            confirmationWindow.CurrentDataContext = context;
            confirmationWindow.Owner = Application.Current.MainWindow;
            confirmationWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            confirmationWindow.Show();
        }
        #endregion

        private void ReadExcelFile()
        {
            int worksheet;
            int columnDpn, startRowDpn, endRowDpn;
            int columnQty, startRowQty, endRowQty;
            int columnDelivery, startRowDelivery, endRowDelivery;

            if (Int32.TryParse(WorksheetTextBox.Text, out worksheet) &&
                Int32.TryParse(DpnColTextBox.Text, out columnDpn) &&
                Int32.TryParse(DpnStartRowTextBox.Text, out startRowDpn) &&
                Int32.TryParse(DpnEndRowTextBox.Text, out endRowDpn) &&
                Int32.TryParse(QtyColTextBox.Text, out columnQty) &&
                Int32.TryParse(QtyStartRowTextBox.Text, out startRowQty) &&
                Int32.TryParse(QtyEndRowTextBox.Text, out endRowQty) &&
                Int32.TryParse(DeliveryColTextBox.Text, out columnDelivery) &&
                Int32.TryParse(DeliveryStartRowTextBox.Text, out startRowDelivery) &&
                Int32.TryParse(DeliveryEndRowTextBox.Text, out endRowDelivery))
            {
                Excel.Application excelApp = new Excel.Application();
                Excel.Workbook excelWorkbook = excelApp.Workbooks.Open(PathDeliveryFile, ReadOnly: true);
                Excel.Worksheet excelWorksheet = (Excel.Worksheet)excelWorkbook.Worksheets.get_Item(worksheet);

                Excel.Range excelRange = excelWorksheet.UsedRange;

                string txtDpn, txtQty, txtDelivery, tmpDelivery;
                txtDelivery = "";

                while ((startRowDpn <= endRowDpn) ||
                    (startRowQty <= endRowQty) ||
                    (startRowDelivery <= endRowDelivery))
                {
                    txtDpn = txtQty = tmpDelivery = "";

                    if (startRowDpn <= endRowDpn)
                    {
                        txtDpn = ExcelUtil.GetTextFromCell(excelRange, startRowDpn, columnDpn);
                        startRowDpn++;
                    }

                    if (startRowQty <= endRowQty)
                    {
                        txtQty = ExcelUtil.GetTextFromCell(excelRange, startRowQty, columnQty);
                        startRowQty++;
                    }

                    if (startRowDelivery <= endRowDelivery)
                    {
                        tmpDelivery = ExcelUtil.GetTextFromCell(excelRange, startRowDelivery, columnDelivery);
                        if (!String.IsNullOrEmpty(tmpDelivery))
                            txtDelivery = tmpDelivery;
                        startRowDelivery++;
                    }

                    AddListViewItems(txtDpn, txtQty, txtDelivery);
                }

                excelWorkbook.Close();
                excelApp.Quit();

                Marshal.ReleaseComObject(excelWorksheet);
                Marshal.ReleaseComObject(excelWorkbook);
                Marshal.ReleaseComObject(excelApp);
            }
        }

        private void AddListViewItems(string dpn, string qty, string delivery)
        {
            DeliveryData.Items.Add(new
            {
                Dpn = dpn,
                Qty = qty,
                Delivery = delivery
            });
        }

        private void ShowWorningInputParameters()
        {
            MessageBox.Show("Check the input parameters for Excel-document");
        }

    }
}
