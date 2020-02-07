using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ClickerDeliveryDocuments.Models
{
    public class DeliveryItemModel : INotifyPropertyChanged
    {
        // Fields which can change its value.
        private bool processed = false;
        private string qty = String.Empty;

        public bool Processed
        {
            get => processed;
            set
            {
                processed = value;
                OnPropertyChanged(nameof(Processed));
            }
        }
        public string Dpn { get; set; }
        [DisplayName("Quantity")]
        public string Qty
        {
            get => qty;
            set
            {
                qty = value;
                OnPropertyChanged("Qty");
            }
        }
        public string Delivery { get; set; }


        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        #endregion
    }
}
