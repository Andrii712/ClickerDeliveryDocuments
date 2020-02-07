using ClickerDeliveryDocuments.ViewModels;
using System;
using System.Windows.Input;

namespace ClickerDeliveryDocuments.Commands
{
    class StartBackgroundWorkerCommand : ICommand
    {
        private ProcessingChoiceViewModel model;

        public StartBackgroundWorkerCommand(ProcessingChoiceViewModel model)
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
            if ((model.UnprocessedItems != null) && (model.UnprocessedItems.Count > 0))
            {
                // Reset the text in the result label.
                model.CurrentFileName = String.Empty;

                // Start the asynchronous operation.
                model.Worker.RunWorkerAsync(model.UnprocessedItems);
            }
        }
        #endregion
    }
}
