using JLClient.Core.Manual;
using JLClient.Core.Observer;
using JLClient.MVVM.Model.Components.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace JLClient.MVVM.ViewModel.Components.Base
{
    public class PdfViewerViewModel : ObservableObject
    {
        public FixedDocumentSequence ActiveDocument { get { return activeDocument; } set { activeDocument = value; OnPropsChanged("ActiveDocument"); } }
        private FixedDocumentSequence activeDocument;

        public bool IsLoading { get { return isLoading; } set { isLoading = value; OnPropsChanged("IsLoading"); } }
        private bool isLoading;

        public bool IsPrevPageVisible { get { return isPrevPageVisible; } set { isPrevPageVisible = value; OnPropsChanged("IsPrevPageVisible"); } }
        private bool isPrevPageVisible;

        public bool IsNextPageVisible { get { return isNextPageVisible; } set { isNextPageVisible = value; OnPropsChanged("IsNextPageVisible"); } }
        private bool isNextPageVisible;

        public bool IsSyncPageVisible { get { return isSyncPageVisible; } set { isSyncPageVisible = value; OnPropsChanged("IsSyncPageVisible"); } }
        private bool isSyncPageVisible;

        #region Команды
        public RelayCommand CloseCommand { get; set; }
        public RelayCommand NextPageCommand { get; set; }
        public RelayCommand PrevPageCommand { get; set; }
        public RelayCommand SyncPageCommand { get; set; }
        #endregion

        #region События
        public event EventHandler OnClose;
        #endregion

        private readonly PdfViewerHandler handler;
        private readonly ViewerMode currentMod;
        private readonly int currentLessonId;

        private ManualData manualData;
        private List<Page> activeDocumentPages;
        private Page currentPage;

        public PdfViewerViewModel(ViewerMode mod, int? lessonId = null)
        {
            ActiveDocument = new FixedDocumentSequence();
            handler = new PdfViewerHandler();
            currentMod = mod;
            CloseCommand = new RelayCommand(_ => Close());
            NextPageCommand = new RelayCommand(_ => NextPage());
            PrevPageCommand = new RelayCommand(_ => PrevPage());
            SyncPageCommand = new RelayCommand(_ => SyncPage());

            switch (currentMod)
            {
                case ViewerMode.PREVIEW:
                    IsPrevPageVisible = true;
                    IsSyncPageVisible = false;
                    isNextPageVisible = true;
                    break;
                case ViewerMode.SYNCHRONIZATION_FULL:
                    IsPrevPageVisible = true;
                    IsSyncPageVisible = false;
                    isNextPageVisible = true;
                    if(lessonId.HasValue) currentLessonId = lessonId.Value;
                    break;
                case ViewerMode.SYNCHRONIZATION_MANUAL:
                    IsPrevPageVisible = true;
                    IsSyncPageVisible = true;
                    isNextPageVisible = true;
                    if (lessonId.HasValue) currentLessonId = lessonId.Value;
                    break;
            }
        }

        public void Init(ManualData manual)
        {
            IsLoading = true;

            manualData = manual;
            activeDocumentPages = handler.GetPages(manualData);
            SetFirstPage();

            IsLoading = false;
        }

        private void SetFirstPage()
        {
            if (activeDocumentPages == null || activeDocumentPages.Count == 0)
                return;

            SetPageContent(activeDocumentPages.First());
        }

        private void SetFixedDoc(string path, Page page)
        {
            currentPage = page;
            if (handler.TryGetFixedDoc(path, out FixedDocumentSequence doc))
            { 
                if (doc != null) ActiveDocument = doc; 
            }
        }

        private void SetPageContent(Page page)
        {
            // Если на локальном устройстве есть файл страницы
            if (handler.TryGetPageFilePath(page, out string path))
            {
                SetFixedDoc(path, page);
            }
            // Если в кэш директории есть файл страницы
            else if (handler.TryGetPageFilePathFromCache(page, out string cachedPath))
            {
                SetFixedDoc(cachedPath, page);
            }
            else
            {
                // Скачивание файла с сервера
                Task.Factory.StartNew(async () =>
                {
                    var cachedFilePath = await handler.DownloadFileWithCachingAndGetPathAsync(page);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        SetFixedDoc(cachedFilePath, page);
                    });
                });
            }
        }

        private void Close()
        {
            OnClose?.Invoke(this, EventArgs.Empty);
        }

        private void NextPage()
        {
            if (currentPage == null) return;
            
            IsLoading = true;
            if (handler.TryGetNextPage(activeDocumentPages, currentPage, out Page newPage))
            {
                SetPageContent(newPage);
                IsLoading = false;
            }
            else
            {
                IsLoading = false;
            }
        }

        private void PrevPage()
        {
            if (currentPage == null) return;

            IsLoading = true;
            if (handler.TryGetPrevPage(activeDocumentPages, currentPage, out Page newPage))
            {
                SetPageContent(newPage);
                IsLoading = false;
            }
            else
            {
                IsLoading = false;
            }
        }

        private void SyncPage()
        {
            if (currentPage == null) return;

            IsLoading = true;

            IsLoading = false;
        }
    }
}
