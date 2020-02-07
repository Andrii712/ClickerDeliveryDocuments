using ClickerDeliveryDocuments.Utilities;
using ClickerDeliveryDocuments.ViewModels;
using System;
using System.Windows.Input;

namespace ClickerDeliveryDocuments.Commands
{
    class ShowExcelDocumentCommand : ICommand
    {
        private CheckingPlanResultViewModel model;

        public ShowExcelDocumentCommand(CheckingPlanResultViewModel model)
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
            if (parametr is Uri uri)
            {
                ExcelUtil.ShowExcelDocument(uri.ToString(), model.Worksheet);
            }
        }
        #endregion
    }
}
