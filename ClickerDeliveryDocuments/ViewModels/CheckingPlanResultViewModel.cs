using ClickerDeliveryDocuments.Commands;
using ClickerDeliveryDocuments.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace ClickerDeliveryDocuments.ViewModels
{
    public class CheckingPlanResultViewModel : INotifyPropertyChanged
    {
        #region Fields
        // Represent the ClickerDeliveryDocuments object.
        private ClickerModel clicker;

        /// <summary>
        /// The index of worksheet which contains the checking plan.
        /// </summary>
        public int Worksheet => clicker.CheckingPlanWorksheet;
        /// <summary>
        /// The List of hyperlinks at processed excel-files.
        /// </summary>
        public List<DocumentHyperlinkModel> ProcessedFiles => clicker.PrecessedFiles;
        #endregion


        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        #endregion

        public CheckingPlanResultViewModel()
        {
            // Get the instance of a Clicker class model.
            clicker = (Application.Current as ClickerDeliveryDocuments.App)?.clicker;
        }

        public void SetProcessedFile(List<DocumentHyperlinkModel> items)
        {
            clicker.PrecessedFiles = items;
            OnPropertyChanged(nameof(ProcessedFiles));
        }
        public void ClearProcessedFiles()
        {
            clicker.PrecessedFiles.Clear();
            OnPropertyChanged(nameof(ProcessedFiles));
        }

        // ButtonCommands
        public ICommand OnClickHyperlink => new ShowExcelDocumentCommand(this);
    }
}
