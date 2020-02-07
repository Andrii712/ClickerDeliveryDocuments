using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ClickerDeliveryDocuments.Models
{
    public class ClickerModel
    {
        public ClickerModel()
        {
            DeliveryData = new ObservableCollection<DeliveryItemModel>();
            PrecessedFiles = new List<DocumentHyperlinkModel>();
        }

        /// <summary>
        /// The index of relevant worksheet in Excel-document.
        /// </summary>
        public int Worksheet { get; set; } = 2;
        /// <summary>
        /// The index of worksheet which contains the checking plan.
        /// </summary>
        public int CheckingPlanWorksheet { get; set; } = 2;

        #region Dpn area in Excel-document 
        public int ColumnDpn { get; set; } = 1;
        public int StartRowDpn { get; set; } = 2;
        public int EndRowDpn { get; set; } = 2;
        #endregion

        #region Quantity area in Excel-document
        public int ColumnQty { get; set; } = 2;
        public int StartRowQty { get; set; } = 2;
        public int EndRowQty { get; set; } = 2;
        #endregion

        #region Delivery area in Excel-document
        public int ColumnDelivery { get; set; } = 3;
        public int StartRowDelivery { get; set; } = 2;
        public int EndRowDelivery { get; set; } = 2;
        #endregion

        #region Delivery data
        // Path to delivery document.
        public string DeliveryFileName { get; set; } = String.Empty;
        
        public DateTime DispatchDate { get; set; } = DateTime.Now.Date;
        public DateTime ReceiptDate { get; set; } = DateTime.Now.Date;

        // Collection of delivery datas.
        public ObservableCollection<DeliveryItemModel> DeliveryData { get; set; }
        #endregion

        #region Data for checking
        public string Qty { get; set; } = String.Empty;
        public string CheckingPlanDir { get; set; } = String.Empty;
        #endregion

        // The collection of hyperlinks to processed the excel-files.
        public List<DocumentHyperlinkModel> PrecessedFiles { get; set; }
    }
}
