using ClickerDeliveryDocuments.Commands;
using ClickerDeliveryDocuments.Models;
using ClickerDeliveryDocuments.Utilities;
using ClickerDeliveryDocuments.Views.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace ClickerDeliveryDocuments.ViewModels
{
    public class ProcessingChoiceViewModel : INotifyPropertyChanged
    {
        #region Fields
        // Represent the ClickerDeliveryDocuments object.
        private readonly ClickerModel clicker;

        private string currentFileName;
        private int currnetProgress;
        private static BackgroundWorker worker;

        public ObservableCollection<DeliveryItemModel> UnprocessedItems { get; private set; }
        public Window Window { get; set; }
        public int CurrentProgress
        {
            get => currnetProgress;
            set
            {
                if (currnetProgress != value)
                {
                    currnetProgress = value;
                    OnPropertyChanged(nameof(CurrentProgress));
                }
            }
        }
        public string CurrentFileName
        {
            get => currentFileName;
            set
            {
                currentFileName = value;
                OnPropertyChanged(nameof(CurrentFileName));
            }
        }
        public BackgroundWorker Worker => worker;
        #endregion

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        #endregion

        public ProcessingChoiceViewModel(ProcessingChoiceWindow window)
        {
            Window = window;

            // Get the instance of a Clicker class model.
            clicker = (Application.Current as ClickerDeliveryDocuments.App)?.clicker;
            clicker.PrecessedFiles.Clear();

            InitializeBackgroundWorker();
        }

        public void SetUnprocessedItems(in ObservableCollection<DeliveryItemModel> collection)
        {
            UnprocessedItems = collection;
        }

        /// <summary>
        /// Set up the BackgroundWorker object by attaching event handlers. 
        /// </summary>
        private void InitializeBackgroundWorker()
        {
            using (worker = new BackgroundWorker())
            {
                // Set up the BackgroundWorker object by attaching event handlers.
                worker.DoWork += Worker_DoWork;
                worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
                worker.ProgressChanged += Worker_ProgressChanged;
                worker.WorkerReportsProgress = true;
                worker.WorkerSupportsCancellation = true;
            }
        }

        #region Worker Members
        /// <summary>
        /// This event handler is where the time-consuming work is done.
        /// </summary>
        /// <param name="sender">The BackgroundWorker which raised this event.</param>
        /// <param name="e">Provides data for the DoWork event handler.</param>
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Get the BackgroundWorker that raised this event.
            BackgroundWorker worker = sender as BackgroundWorker;
            ObservableCollection<DeliveryItemModel> deliveryItems = (ObservableCollection<DeliveryItemModel>)e.Argument;

            worker.ReportProgress(0);

            foreach (DeliveryItemModel item in deliveryItems)
            {
                // Abort the operation if the user has canceled.
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    string[] files = ExcelUtil.GetFilesCheckingPlans(clicker.CheckingPlanDir, item.Dpn);

                    for (int j = 0; j < files.Length; j++)
                    {
                        CurrentFileName = files[j];

                        if (File.Exists(CurrentFileName))
                        {
                            if (ExcelUtil.DublicateAreaInDocument(CurrentFileName, item))
                            {
                                // mark item of the collection as processed.
                                item.Processed = true;
                                AddProcessedFile(new DocumentHyperlinkModel
                                {
                                    Url = new Uri(CurrentFileName),
                                    LinkTitle = CurrentFileName
                                });
                            }
                        }
                        else
                        {
                            throw new Exception($"File '{CurrentFileName}' not found!");
                        }
                    }

                    // Perform a time consuming operation.
                    System.Threading.Thread.Sleep(500);

                    // Report progress as a percentage of the total task
                    int percentComplete = ((deliveryItems.IndexOf(item) + 1) * 100) / deliveryItems.Count;
                    worker.ReportProgress(percentComplete);
                }
            }
        }

        /// <summary>
        /// This event handler updates the progress.
        /// </summary>
        /// <param name="sender">The BackgroundWorker which raised this event.</param>
        /// <param name="e">Provides data for the ProgressChanged event.</param>
        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            CurrentProgress = e.ProgressPercentage;
        }

        /// <summary>
        /// This event handler deals with the results of the background operation.
        /// </summary>
        /// <param name="sender">The BackgroundWorker which raised this event.</param>
        /// <param name="e">Provides data for the ProgressChanged event.</param>
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // First, handle the case where an exception was thrown.
            if (e.Error != null)
            {
                DataUtil.ShowErrorMessage(e.Error.Message);
            }
            else if (e.Cancelled)
            {
                // Next, handle the case where the user canceled operation.
                CurrentFileName = "Canceled";
                ProcessChoiseShowResult();
            }
            else
            {
                // Finally, handle the case where the operation succeeded.
                CurrentFileName = "All files were processed.";
                ProcessChoiseShowResult();
            }

            // Perform a time consuming operation.
            System.Threading.Thread.Sleep(500);

            // Close current processinng window.
            Window.Close();
        }
        #endregion

        /// <summary>
        /// Add the file hyperlink to the collection of processed files.
        /// </summary>
        /// <param name="documentHyperlink">The hyperlink to the excel-document.</param>
        private void AddProcessedFile(DocumentHyperlinkModel documentHyperlink)
        {
            clicker.PrecessedFiles.Add(documentHyperlink);
        }

        /// <summary>
        /// Show a dialog window with processed documents.
        /// </summary>
        private void ProcessChoiseShowResult()
        {
            if (clicker.PrecessedFiles.Count > 0)
            {
                CheckingPlanResultWindow window = new CheckingPlanResultWindow()
                {
                    Owner = Application.Current.MainWindow
                };
                window.Show();
            }
            else
            {
                DataUtil.ShowErrorMessage(String.Format(DataUtil.defaultCultureProvider, "Does not have processed files."));
            }
        }

        #region ButtonCommands
        public ICommand StartBackgroundWorker => new StartBackgroundWorkerCommand(this);
        public ICommand CancelBackgroundWorker => new CancelBackgroundWorkerCommand(this);
        #endregion
    }
}
