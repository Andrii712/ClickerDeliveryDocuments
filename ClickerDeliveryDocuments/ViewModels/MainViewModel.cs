using ClickerDeliveryDocuments.Commands;
using ClickerDeliveryDocuments.Models;
using ClickerDeliveryDocuments.Utilities;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace ClickerDeliveryDocuments.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Fields
        // Represent the ClickerDeliveryDocuments object.
        private ClickerModel clicker;

        // Relevant sheet in Excel-document.
        public string Worksheet
        {
            get => clicker.Worksheet.ToString(DataUtil.defaultCultureProvider);
            set
            {
                if (DataUtil.IsNumericValue(value))
                {
                    clicker.Worksheet = DataUtil.StringToInt32(value);
                }
                else
                {
                    DataUtil.ShowErrorMessageInputParameters(value);
                }
                OnPropertyChanged("Worksheet");
            }
        }
        public int WorksheetAsInt { get; set; }


        #region Dpn area in Excel-document
        public string ColumnDpn
        {
            get => clicker.ColumnDpn.ToString(DataUtil.defaultCultureProvider);
            set
            {
                if (DataUtil.IsNumericValue(value))
                {
                    clicker.ColumnDpn = DataUtil.StringToInt32(value);
                    ColumnQty = (clicker.ColumnDpn + 1).ToString(DataUtil.defaultCultureProvider);
                    ColumnDelivery = (clicker.ColumnDpn + 2).ToString(DataUtil.defaultCultureProvider);
                }
                else
                {
                    DataUtil.ShowErrorMessageInputParameters(value);
                }
                OnPropertyChanged("ColumnDpn");
            }
        }

        public string StartRowDpn
        {
            get => clicker.StartRowDpn.ToString(DataUtil.defaultCultureProvider);
            set
            {
                if (DataUtil.IsNumericValue(value))
                {
                    clicker.StartRowDpn = DataUtil.StringToInt32(value);
                    StartRowQty = StartRowDelivery = value;
                }
                else
                {
                    DataUtil.ShowErrorMessageInputParameters(value);
                }
                OnPropertyChanged("StartRowDpn");
            }
        }

        public string EndRowDpn
        {
            get => clicker.EndRowDpn.ToString(DataUtil.defaultCultureProvider);
            set
            {
                if (DataUtil.IsNumericValue(value))
                {
                    clicker.EndRowDpn = DataUtil.StringToInt32(value);
                    EndRowQty = EndRowDelivery = value;
                }
                else
                {
                    DataUtil.ShowErrorMessageInputParameters(value);
                }
                OnPropertyChanged("EndRowDpn");
            }
        }
        #endregion


        #region Quantity area in Excel-document
        public string ColumnQty
        {
            get => clicker.ColumnQty.ToString(DataUtil.defaultCultureProvider);
            set
            {
                if (DataUtil.IsNumericValue(value))
                {
                    clicker.ColumnQty = DataUtil.StringToInt32(value);
                }
                else
                {
                    DataUtil.ShowErrorMessageInputParameters(value);
                }
                OnPropertyChanged("ColumnQty");
            }
        }

        public string StartRowQty
        {
            get => clicker.StartRowQty.ToString(DataUtil.defaultCultureProvider);
            set
            {
                if (DataUtil.IsNumericValue(value))
                {
                    clicker.StartRowQty = DataUtil.StringToInt32(value);
                }
                else
                {
                    DataUtil.ShowErrorMessageInputParameters(value);
                }
                OnPropertyChanged("StartRowQty");
            }
        }

        public string EndRowQty
        {
            get => clicker.EndRowQty.ToString(DataUtil.defaultCultureProvider);
            set
            {
                if (DataUtil.IsNumericValue(value))
                {
                    clicker.EndRowQty = DataUtil.StringToInt32(value);
                }
                else
                {
                    DataUtil.ShowErrorMessageInputParameters(value);
                }
                OnPropertyChanged("EndRowQty");
            }
        }
        #endregion


        #region Delivery area in Excel-document
        public string ColumnDelivery
        {
            get => clicker.ColumnDelivery.ToString(DataUtil.defaultCultureProvider);
            set
            {
                if (DataUtil.IsNumericValue(value))
                {
                    clicker.ColumnDelivery = DataUtil.StringToInt32(value);
                }
                else
                {
                    DataUtil.ShowErrorMessageInputParameters(value);
                }
                OnPropertyChanged("ColumnDelivery");
            }

        }

        public string StartRowDelivery
        {
            get => clicker.StartRowDelivery.ToString(DataUtil.defaultCultureProvider);
            set
            {
                if (DataUtil.IsNumericValue(value))
                {
                    clicker.StartRowDelivery = DataUtil.StringToInt32(value);
                }
                else
                {
                    DataUtil.ShowErrorMessageInputParameters(value);
                }
                OnPropertyChanged("StartRowDelivery");
            }

        }

        public string EndRowDelivery
        {
            get => clicker.EndRowDelivery.ToString(DataUtil.defaultCultureProvider);
            set
            {
                if (DataUtil.IsNumericValue(value))
                {
                    clicker.EndRowDelivery = DataUtil.StringToInt32(value);
                }
                else
                {
                    DataUtil.ShowErrorMessageInputParameters(value);
                }
                OnPropertyChanged("EndRowDelivery");
            }
        }
        #endregion


        // Path to delivery document.
        public string DeliveryFileName
        {
            get => clicker.DeliveryFileName;
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    DataUtil.ShowErrorMessageInputParameters(DeliveryFileName);
                }
                else
                {
                    clicker.DeliveryFileName = value;
                    OnPropertyChanged("DeliveryFileName");
                }
            }
        }

        public DateTime DispatchDate
        {
            get => clicker.DispatchDate;
            set
            {
                if (value != DateTime.MinValue)
                {
                    clicker.DispatchDate = value;
                    OnPropertyChanged("DispatchDate");
                }
                else
                {
                    DataUtil.ShowErrorMessageInputParameters(DispatchDate);
                }
            }
        }

        public DateTime ReceiptDate
        {
            get => clicker.ReceiptDate;
            set
            {
                if (value != DateTime.MinValue)
                {
                    clicker.ReceiptDate = value;
                    OnPropertyChanged("ReceiptDate");
                }
                else
                {
                    DataUtil.ShowErrorMessageInputParameters("ReceiptDate");
                }
            }
        }

        // Quantity for confirm.
        public string Qty
        {
            get => clicker.Qty;
            set
            {
                if (DataUtil.IsNumericValue(value))
                {
                    clicker.Qty = value;
                }
                else
                {
                    DataUtil.ShowErrorMessageInputParameters(value);
                }
                OnPropertyChanged("Qty");
            }
        }

        public string CheckingPlanDir
        {
            get => clicker.CheckingPlanDir;
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    DataUtil.ShowErrorMessageInputParameters(CheckingPlanDir);
                }
                else
                {
                    clicker.CheckingPlanDir = value;
                    OnPropertyChanged("CheckingPlanDir");
                }
            }
        }
        #endregion


        #region Delivery datas by documnt
        public ObservableCollection<DeliveryItemModel> DeliveryDataItems => clicker.DeliveryData;

        public void SetDeliveryData(ObservableCollection<DeliveryItemModel> collection)
        {
            clicker.DeliveryData = collection;
            OnPropertyChanged(nameof(DeliveryDataItems));
        }

        public void ClearDeliveryData()
        {
            clicker.DeliveryData.Clear();
            OnPropertyChanged(nameof(DeliveryDataItems));
        }

        // Contains the selected item in "DeliveryDataListView".
        private DeliveryItemModel selectedItem;

        public DeliveryItemModel SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }
        #endregion

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        #endregion


        public MainViewModel()
        {
            // Get the instance of a Clicker class model.
            clicker = (Application.Current as ClickerDeliveryDocuments.App)?.clicker;
        }


        #region ButtonCommands
        //MainWindow command.
        public ICommand BrowseDeliveryFile => new BrowseDeliveryFileCommand(this);
        public ICommand ListViewChooseItem => new ListViewChooseItemCommand(this);
        public ICommand ListViewChooseAll => new ListViewChooseAllCommand(this);

        //ConfirmationWindow command.
        public ICommand ChoosePathCheckingPlan => new ChoosePathCheckingPlanCommand(this);
        public ICommand ConfirmData => new ConfirmDataCommand(this);
        #endregion
    }
}
