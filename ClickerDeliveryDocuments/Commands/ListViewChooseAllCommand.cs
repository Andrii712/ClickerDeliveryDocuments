using ClickerDeliveryDocuments.Utilities;
using ClickerDeliveryDocuments.ViewModels;
using ClickerDeliveryDocuments.Views.Dialogs;
using System;
using System.Windows;
using System.Windows.Input;

namespace ClickerDeliveryDocuments.Commands
{
    class ListViewChooseAllCommand : ICommand
    {
        private MainViewModel model;

        public ListViewChooseAllCommand(MainViewModel model)
        {
            this.model = model;
        }


        #region ICommand Members
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (model.DeliveryDataItems.Count > 0)
            {
                model.Qty = "0";

                ConfirmationWindow confirmationWindow = new ConfirmationWindow()
                {
                    DataContext = model,
                    Owner = Application.Current.MainWindow
                };

                // Hide the Quantity text box.
                confirmationWindow.QtyTextBox.IsEnabled = false;

                try
                {
                    // Opens a new confirmtion window and returns only when the newly opened window is closed.
                    if (confirmationWindow.ShowDialog() == true)
                    {
                        ProcessingChoiceWindow processingWindow = new ProcessingChoiceWindow()
                        {
                            Owner = Application.Current.MainWindow
                        };
                        ((ProcessingChoiceViewModel)processingWindow.DataContext)
                            .SetUnprocessedItems(model.DeliveryDataItems);
                        processingWindow.Show();
                    }
                }
                catch (InvalidOperationException ex)
                {
                    DataUtil.ShowErrorMessage(ex.Message);
                }
                catch (Exception ex)
                {
                    DataUtil.ShowErrorMessage($"{ex.Message} " +
                        $"\nMake sure that all confirmation windows closed or close app and try again.");
                }
            }
        }
        #endregion
    }
}
