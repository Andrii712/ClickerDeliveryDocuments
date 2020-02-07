using ClickerDeliveryDocuments.ViewModels;
using System;
using System.Windows.Input;

namespace ClickerDeliveryDocuments.Commands
{
    class CancelBackgroundWorkerCommand : ICommand
    {
        private ProcessingChoiceViewModel model;

        public CancelBackgroundWorkerCommand(ProcessingChoiceViewModel model)
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
            // Cancel the asynchronous operation.
            model.Worker.CancelAsync();
        }
        #endregion
    }
}
