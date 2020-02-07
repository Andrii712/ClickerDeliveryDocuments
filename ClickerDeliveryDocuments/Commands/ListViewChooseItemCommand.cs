using ClickerDeliveryDocuments.Models;
using ClickerDeliveryDocuments.Utilities;
using ClickerDeliveryDocuments.ViewModels;
using ClickerDeliveryDocuments.Views.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace ClickerDeliveryDocuments.Commands
{
    class ListViewChooseItemCommand : ICommand
    {
        private MainViewModel model;

        public ListViewChooseItemCommand(MainViewModel model)
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
            if (parameter is ClickerDeliveryDocuments.Models.DeliveryItemModel deliveryItem)
            {
                // Fix the current value of Qty to check.
                model.Qty = deliveryItem.Qty;

                ConfirmationWindow confirmationWindow = new ConfirmationWindow()
                {
                    DataContext = model,
                    Owner = Application.Current.MainWindow
                };

                try
                {
                    // Opens a new confirmtion window and returns only when the newly opened window is closed.
                    if (confirmationWindow.ShowDialog() == true)
                    {
                        deliveryItem.Qty = model.Qty;
                        model.SelectedItem = deliveryItem;

                        ProcessingChoiceWindow processingWindow = new ProcessingChoiceWindow()
                        {
                            Owner = Application.Current.MainWindow
                        };
                        ((ProcessingChoiceViewModel)processingWindow.DataContext)
                            .SetUnprocessedItems(new ObservableCollection<DeliveryItemModel>() { deliveryItem });
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
