using ClickerDeliveryDocuments.Utilities;
using ClickerDeliveryDocuments.ViewModels;
using System;
using System.Windows.Input;

namespace ClickerDeliveryDocuments.Commands
{
    public class BrowseDeliveryFileCommand : ICommand
    {
        private MainViewModel model;

        public BrowseDeliveryFileCommand(MainViewModel model)
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
            if (model != null)
            {
                if ((!String.IsNullOrEmpty(model.Worksheet) && model.Worksheet != "0") &&
                   (!String.IsNullOrEmpty(model.ColumnDpn) && model.ColumnDpn != "0") &&
                   (!String.IsNullOrEmpty(model.StartRowDpn) && model.StartRowDpn != "0") &&
                   (!String.IsNullOrEmpty(model.EndRowDpn) && model.EndRowDpn != "0") &&
                   (!String.IsNullOrEmpty(model.ColumnQty) && model.ColumnQty != "0") &&
                   (!String.IsNullOrEmpty(model.StartRowQty) && model.StartRowQty != "0") &&
                   (!String.IsNullOrEmpty(model.EndRowQty) && model.EndRowQty != "0") &&
                   (!String.IsNullOrEmpty(model.ColumnDelivery) && model.ColumnDelivery != "0") &&
                   (!String.IsNullOrEmpty(model.StartRowDelivery) && model.StartRowDelivery != "0") &&
                   (!String.IsNullOrEmpty(model.EndRowDelivery) && model.EndRowDelivery != "0"))
                {
                    model.DeliveryFileName = ExcelUtil.OpenFileDialogExcel();
                    if (!String.IsNullOrEmpty(model.DeliveryFileName))
                    {
                        // Clears the items collection.
                        model.ClearDeliveryData();

                        // Bind ArrayList with the ListView.
                        model.SetDeliveryData(ExcelUtil.ReadDeliveryExcelFile());
                    }
                    else
                    {
                        DataUtil.ShowErrorMessageInputParameters(model.DeliveryFileName);
                    }
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
