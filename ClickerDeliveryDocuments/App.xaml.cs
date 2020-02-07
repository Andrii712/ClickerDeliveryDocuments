using ClickerDeliveryDocuments.Models;
using ClickerDeliveryDocuments.Utilities;
using ClickerDeliveryDocuments.Views;
using System.Windows;

namespace ClickerDeliveryDocuments
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // Represent the ClickerDeliveryDocuments object.
        internal ClickerModel clicker;

        public App()
        {
            // Specify the way to exit from the application.
            this.ShutdownMode = ShutdownMode.OnMainWindowClose;

            // Create an instance of a Clicker class model.
            clicker = new ClickerModel();
        }

        #region ApplicationEvents
        protected override void OnStartup(StartupEventArgs e)
        {
            Window window = new MainWindow();
            window.Show();

            base.OnStartup(e);

            // Initialize Excel COM-Object.
            ExcelUtil.InitializeObjectExcelApplication();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            // Close Excel COM-Object.
            ExcelUtil.CloseObjectExcelApplication();
        }
        #endregion
    }
}
