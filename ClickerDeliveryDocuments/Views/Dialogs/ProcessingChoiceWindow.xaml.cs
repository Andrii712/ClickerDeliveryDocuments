using ClickerDeliveryDocuments.Models;
using ClickerDeliveryDocuments.ViewModels;
using System.Windows;

namespace ClickerDeliveryDocuments.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for ProcessingChoiceWindow.xaml
    /// </summary>
    public partial class ProcessingChoiceWindow : Window
    {
        public ProcessingChoiceWindow()
        {
            DataContext = new ProcessingChoiceViewModel(this);
            InitializeComponent();
        }
    }
}
