using JLClient.Core.Arguments;
using JLClient.Core.Manual;
using JLClient.Core.Observer;
using JLClient.Core.PersistModels;
using JLClient.MVVM.Model.Components.Base;
using JLClient.MVVM.Model.Components.Editor;
using JLClient.MVVM.ViewModel.Components.Base;
using JLClient.MVVM.ViewModel.Components.Editor.Components;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JLClient.MVVM.ViewModel.Components.Editor
{
    public class EditorWindowViewModel : ObservableObject
    {
        #region Представления
        public ManualTreeViewModel ManualTreeVM { get; set; }
        public StepProgressViewModel StepProgressVM { get; set; }
        public PdfViewerViewModel PdfViewerVM { get { return pdfViewerVM; } set { pdfViewerVM = value; OnPropsChanged("PdfViewerVM"); } }
        public PdfViewerViewModel pdfViewerVM;
        #endregion

        public List<Manual> MyManuals { get { return myManuals; } set { myManuals = value; OnPropsChanged("MyManuals"); } }
        private List<Manual> myManuals;
        public Manual CurrentManual { get { return currentManual; } set { currentManual = value; LoadManualData(currentManual); OnPropsChanged("CurrentManual"); } }
        private Manual currentManual;
        public bool IsManualDisplaying { get { return isManualDisplaying; } set { isManualDisplaying = value; OnPropsChanged("IsManualDisplaying"); } }
        private bool isManualDisplaying;
        public bool IsLoading { get { return isLoading; } set { isLoading = value; OnPropsChanged("IsLoading"); } }
        private bool isLoading;
        public bool FullAccess { get { return fullAccess; } set { fullAccess = value; OnPropsChanged("FullAccess"); } }
        private bool fullAccess;
        public bool CanUpdateManual { get { return canUpdateManual; } set { canUpdateManual = value; OnPropsChanged("CanUpdateManual"); } }
        private bool canUpdateManual;
        public WordSplitType SplitType { get { return splitType; } set { splitType = value; OnPropsChanged("SplitType"); } }
        private WordSplitType splitType;
        public IEnumerable<WordSplitType> SplitTypes { get => Enum.GetValues(typeof(WordSplitType)).Cast<WordSplitType>(); }

        private EditorHandler handler;

        #region Команды
        public RelayCommand ExportManualCommand { get; set; }
        public RelayCommand ImportManualCommand { get; set; }
        public RelayCommand UploadManualCommand { get; set; }
        public RelayCommand UpdateManualCommand { get; set; }
        public RelayCommand AutoGenerateManualCommand { get; set; }
        public RelayCommand ShowManualCommand { get; set; }
        #endregion

        public EditorWindowViewModel()
        {
            ManualTreeVM = new ManualTreeViewModel();
            StepProgressVM = new StepProgressViewModel();
            ExportManualCommand = new RelayCommand(_ => ExportManual());
            ImportManualCommand = new RelayCommand(_ => ImportManual());
            AutoGenerateManualCommand = new RelayCommand(_ => AutoGenerate());
            ShowManualCommand = new RelayCommand(_ => ShowManual());
            UploadManualCommand = new RelayCommand(_ => UploadManual());
            UpdateManualCommand = new RelayCommand(_ => UpdateManual());

            ManualTreeVM.OnManualDataChange += ManualTreeVM_OnManualDataChange;
            var manualSteps = new List<string>()
            {
                "Материал в разработке",
                "Материал сформирован",
                "Материал загружен на сервер"
            };

            StepProgressVM.Init(manualSteps, 0);
        }

        private void ManualTreeVM_OnManualDataChange(object sender, EventArgs e)
        {
            ManualTreeVM.CurrentManualData.OnProcessChange += CurrentManualData_OnProcessChange;
            SetManualProcess(ManualTreeVM.CurrentManualData.GetStep());
        }

        public void Init(bool fullAccess)
        {
            handler = new EditorHandler();
            ManualTreeVM.Init();
            FullAccess = fullAccess;
            
            if (!fullAccess) return;
            GetMyManuals();
        }

        private void CurrentManualData_OnProcessChange(object sender, EventArgs e)
        {
            if (e is ManualProcessChangeArgument argument && argument != null)
            {
                SetManualProcess(argument.Step);
            }
        }

        private void GetMyManuals()
        {
            IsLoading = true;
            Task.Factory.StartNew(async () => {
                try
                {
                    MyManuals = await handler.LoadMyManualsAsync();
                }
                catch (Exception ex)
                {
                    // MESSAGE1
                }
                finally
                {
                    IsLoading = false;
                }
            });
        }

        private void ShowManual()
        {
            PdfViewerVM = new PdfViewerViewModel(ViewerMode.PREVIEW);
            IsManualDisplaying = true;
            var manual = ManualTreeVM.CurrentManualData;
            PdfViewerVM.Init(manual);
            PdfViewerVM.OnClose += CloseManual;
        }

        private void CloseManual(object sender, EventArgs args)
        {
            PdfViewerVM = null;
            IsManualDisplaying = false;
        }

        private void ExportManual()
        {
            if (ManualTreeVM == null || ManualTreeVM.CurrentManualData == null) return;
            IsLoading = true;

            string manualPath = null;

            Thread thread = new Thread(() => {
                manualPath = handler.ExportManual(ManualTreeVM.CurrentManualData);
                IsLoading = false;
            });

            thread.IsBackground = true;
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();   
        }

        private void UploadManual()
        {
            if (ManualTreeVM == null) return;
            IsLoading = true;
            Task.Factory.StartNew(async () => {
                try 
                {
                    await handler.UploadManualAsync(ManualTreeVM.CurrentManualData);
                    GetMyManuals();
                }
                catch (Exception ex)
                {
                    // MESSAGE1
                }
                finally
                {
                    IsLoading = false;
                }                
            });
        }

        private void UpdateManual()
        {
            if (ManualTreeVM == null) return;
            IsLoading = true;
            Task.Factory.StartNew(async () => {
                try
                {
                    await handler.UpdateManualAsync(ManualTreeVM.CurrentManualData, CurrentManual.fileDataId);
                }
                catch (Exception ex)
                {
                    // MESSAGE1
                }
                finally
                {
                    IsLoading = false;
                }
            });
        }

        private void LoadManualData(Manual manual)
        {
            if (ManualTreeVM == null) return;
            IsLoading = true;
            Task.Factory.StartNew(async () => {
                try
                {
                    ManualTreeVM.CurrentManualData = await handler.GetManualDataAsync(manual);
                    CanUpdateManual = true;
                }
                catch (Exception ex)
                {
                    // MESSAGE1
                }
                finally
                {
                    IsLoading = false;
                }
            });
        }

        private void ImportManual()
        {
            if (ManualTreeVM == null) return;
            IsLoading = true;
            var manual = handler.ImportManual();
            if (manual != null) 
            { 
                ManualTreeVM.SetManual(manual);
                CanUpdateManual = false;
            }

            IsLoading = false;
        }

        private void AutoGenerate()
        {
            if (ManualTreeVM == null) return;
            IsLoading = true;
            Thread thread = new Thread(() => {
                var manual = handler.AutoGenerateManual(SplitType);
                if (manual != null) 
                { 
                    ManualTreeVM.SetManual(manual);
                    CanUpdateManual = false;
                }

                IsLoading = false;
            });

            thread.IsBackground = true;
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void SetManualProcess(int step)
        {
            if (ManualTreeVM == null || ManualTreeVM.CurrentManualData == null)
                return;

            StepProgressVM.SetStep(step);
        }
    }
}
