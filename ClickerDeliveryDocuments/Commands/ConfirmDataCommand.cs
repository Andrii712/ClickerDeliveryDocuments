using ClickerDeliveryDocuments.Utilities;
using ClickerDeliveryDocuments.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;

namespace ClickerDeliveryDocuments.Commands
{
    class ConfirmDataCommand : ICommand
    {
        private MainViewModel model;

        public ConfirmDataCommand(MainViewModel model)
        {
            this.model = model;
        }


        #region ICommand Members
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parametr)
        {
            return true;
        }

        public void Execute(object parametr)
        {
            if (parametr is Window window)
            {
                if ((model.DispatchDate != DateTime.MinValue) &&
                    (model.ReceiptDate != DateTime.MinValue) &&
                    (!String.IsNullOrEmpty(model.CheckingPlanDir)))
                {
                    // set DialogResult to tru as a confitmation of the selected data.
                    window.DialogResult = true;
                    window.Close();
                    window.Owner.Activate();
                }
                else
                {
                    DataUtil.ShowErrorMessageInputParameters();
                }
            }
        }
        #endregion
    }
}
