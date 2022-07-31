using JLClient.Core.Manual;
using JLClient.Core.Observer;
using JLClient.Core.PersistModels;
using JLClient.Core.Settings;
using JLClient.MVVM.Model.Components.Lesson;
using JLClient.MVVM.ViewModel.Components.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JLClient.MVVM.ViewModel.Components.Lesson
{
    public class LessonWindowViewModel : ObservableObject
    {
        public ManualData ManualData { get { return manualData; } set { manualData = value; OnPropsChanged("ManualData"); } }
        private ManualData manualData;

        public Manual Manual { get { return manual; } set { manual = value; OnPropsChanged("Manual"); } }
        private Manual manual;

        public bool IsLoading { get { return isLoading; } set { isLoading = value; OnPropsChanged("IsLoading"); } }
        public bool isLoading;

        public PdfViewerViewModel MainPdfViewerVM { get { return mainPdfViewerVM; } set { mainPdfViewerVM = value; OnPropsChanged("MainPdfViewerVM"); } }
        private PdfViewerViewModel mainPdfViewerVM;

        private readonly LessonHandler handler;

        

        public LessonWindowViewModel()
        {
            handler = new LessonHandler();
        }

        public void Init(int courseId, int manualId, bool isTeacher, LessonType type)
        {
            LoadManual(manualId, isTeacher);   
        }

        private void LoadManual(int manualId, bool isTeacher)
        {
            IsLoading = true;
            Task.Factory.StartNew(async () => {
                try
                {
                    var response = await handler.LoadManualData(manualId);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Manual = response.Manual;
                        ManualData = response.ManualData;
                        InitPdfViewer(isTeacher);
                    });
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

        private void InitPdfViewer(bool isTeacher)
        {
            var mainPdfViewerType = isTeacher ? Model.Components.Base.ViewerMode.SYNCHRONIZATION_FULL : Model.Components.Base.ViewerMode.PREVIEW;
            MainPdfViewerVM = new PdfViewerViewModel(mainPdfViewerType);
            MainPdfViewerVM.Init(manualData);
        }
    }
}
