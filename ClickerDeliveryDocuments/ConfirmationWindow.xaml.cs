using ClickerDeliveryDocuments.MicrosoftOfficeUtil;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;
using System.Linq;

namespace ClickerDeliveryDocuments
{
    /// <summary>
    /// Interaction logic for ConfirmationWindow.xaml
    /// </summary>
    public partial class ConfirmationWindow : Window
    {
        public ConfirmationWindow()
        {
            InitializeComponent();
        }

        protected internal Dictionary<string, string> CurrentDataContext;
        private string PathCheckingPlan { get; set; }


        private void DispatchDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e) 
        {
            DateTime DispatchDate = (DateTime)dpDispatchDate.SelectedDate;
            if (CurrentDataContext.ContainsKey("DispatchDate"))
                CurrentDataContext["DispatchDate"] = DispatchDate
                    .ToString("d", DataUtil.GetCultureInfoProvider("uk-UA"));
            else
                CurrentDataContext.Add("DispatchDate", DispatchDate
                    .ToString("d", DataUtil.GetCultureInfoProvider("uk-UA")));
        }

        private void ReceiptDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e) 
        {
            DateTime ReceiptDate = (DateTime)dpReceiptDate.SelectedDate;
            if (CurrentDataContext.ContainsKey("ReceiptDate"))
                CurrentDataContext["ReceiptDate"] = ReceiptDate
                    .ToString("d", DataUtil.GetCultureInfoProvider("uk-UA"));
            else
                CurrentDataContext.Add("ReceiptDate", ReceiptDate
                    .ToString("d", DataUtil.GetCultureInfoProvider("uk-UA")));
        }

        private void ChoosePathCheckingPlanButton_Click(object sender, RoutedEventArgs e)
        {
            using (CommonOpenFileDialog dialog = new CommonOpenFileDialog())
            {
                if (String.IsNullOrEmpty(txtBoxCheckingPlan.Text))
                    dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                else
                    dialog.InitialDirectory = txtBoxCheckingPlan.Text;
                dialog.IsFolderPicker = true;
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                    PathCheckingPlan = dialog.FileName;
            };

            txtBoxCheckingPlan.Text = PathCheckingPlan;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string searchPattern =$"{CurrentDataContext["Dpn"]}.*";
            var extensions = new List<string> { ".xls", ".xlsx" }; 

            string[] files = Directory.GetFiles(PathCheckingPlan, searchPattern, SearchOption.AllDirectories)
                .Where(f => extensions.IndexOf(System.IO.Path.GetExtension(f)) >= 0).ToArray();

            StringBuilder @string = new StringBuilder();
            
            foreach (string item in files)
            {
                if (File.Exists(item))
                {
                    if (ExcelUtil.DublicateAreaInDocument(item, CurrentDataContext))
                        @string.Append(item);
                }
                else
                {
                    throw new Exception($"'{item}' not found");
                }
            }

            string msgCaption = String.Format(DataUtil.GetCultureInfoProvider(), 
                "Documents have been edited");
            if (@string.Length > 0)
                MessageBox.Show(@string.ToString(), msgCaption);
            else
                MessageBox.Show("Nothing has been edited!", msgCaption);

            this.Close();
            this.Owner.Activate();

        }
    }
}
