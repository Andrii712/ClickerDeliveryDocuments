using ClickerDeliveryDocuments.ViewModels;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Windows.Input;

namespace ClickerDeliveryDocuments.Commands
{
    class ChoosePathCheckingPlanCommand : ICommand
    {
        private MainViewModel model;

        public ChoosePathCheckingPlanCommand(MainViewModel model)
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
            using (CommonOpenFileDialog dialog = new CommonOpenFileDialog())
            {
                if (String.IsNullOrEmpty(model.CheckingPlanDir))
                    dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                else
                    dialog.InitialDirectory = model.CheckingPlanDir;

                dialog.IsFolderPicker = true;

                // Show the dialog to set the path to the directory of the checking plan.
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    model.CheckingPlanDir = dialog.FileName;
                }
            };
        }
        #endregion
    }
}
