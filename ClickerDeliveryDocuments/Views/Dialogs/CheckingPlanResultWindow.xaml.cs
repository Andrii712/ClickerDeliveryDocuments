using ClickerDeliveryDocuments.Utilities;
using ClickerDeliveryDocuments.ViewModels;
using System.Windows;
using System.Windows.Documents;

namespace ClickerDeliveryDocuments.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for CheckingPlanResult.xaml
    /// </summary>
    public partial class CheckingPlanResultWindow : Window
    {
        public CheckingPlanResultWindow()
        {
            InitializeComponent();
        }

        //private void LinkClick(object sender, RoutedEventArgs e)
        //{
        //    Hyperlink hl = (Hyperlink)sender;
        //    string navigateUri = hl.NavigateUri.ToString();
        //    ExcelUtil.ShowExcelDocument(navigateUri, 2);
        //}
    }
}
