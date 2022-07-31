using JLClient.Core.Observer;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using JLClient.Core.Arguments;
using System.Text.Json.Serialization;

namespace JLClient.Core.Manual
{
    public class Page : ObservableObject
    {
        #region Экспорт данные
        public ObservableCollection<PageTest> Tests { 
            get { return _tests; } 
            set { 
                _tests = value; 
                foreach (var test in _tests)
                {
                    test.OnTestDisplay += NewTest_OnTestStart;
                }
                OnPropsChanged("tests"); 
            } 
        }
        public bool CanAddTest { get { return canAddTest; } set { canAddTest = value; OnPropsChanged("CanAddTest"); } }
        public bool CanShowTests { get { return canShowTests; } set { canShowTests = value; OnPropsChanged("CanShowTests"); } }
        public bool OptimizeMemory { get { return optimizeMemory; } set { optimizeMemory = value; OnPropsChanged("OptimizeMemory"); } }
        public string Id { get { return id; } set { id = value; OnPropsChanged("id"); } }
        public int FileDataId { get { return fileDataId; } set { fileDataId = value; OnPropsChanged("fileDataId"); } }
        public string DirPath { get { return dirPath; } set { dirPath = value; OnPropsChanged("dirPath"); } }
        public string FileName { get { return fileName; } set { fileName = value; OnPropsChanged("fileName"); } }
        public string PageTitle { get { return pageTitle; } set { pageTitle = value; OnPropsChanged("pageTitle"); } }
        public PageType Type
        {
            get { return _type; }
            set
            {
                _type = value;
                OnPropsChanged("type");
                CanAddTest = _type == PageType.WORDFILE_WITH_TEST;
                CanShowTests = _type == PageType.WORDFILE_WITH_TEST && Tests != null && Tests.Count > 0;
            }
        }
        #endregion

        #region Поля
        private ObservableCollection<PageTest> _tests;
        private bool canAddTest;
        private bool optimizeMemory;
        private bool canShowTests;
        private PageType _type = PageType.WORDFILE;
        private string id;
        private int fileDataId;
        private string dirPath;
        private string fileName;
        private string pageTitle;
        private ManualNode parent;
        #endregion

        [JsonIgnore]
        public IEnumerable<PageType> PageTypes
        {
            get
            {
                return Enum.GetValues(typeof(PageType)).Cast<PageType>();
            }
        }

        #region Команды
        [JsonIgnore] public RelayCommand SelectDocumentCommand { get; set; }
        [JsonIgnore] public RelayCommand RemovePageCommand { get; set; }
        [JsonIgnore] public RelayCommand MoveUpCommand { get; set; }
        [JsonIgnore] public RelayCommand MoveDownCommand { get; set; }
        [JsonIgnore] public RelayCommand AddTestCommand { get; set; }
        #endregion

        #region События
        public event EventHandler OnPageRemove;
        public event EventHandler OnTestDisplay;
        #endregion

        public Page()
        {
            SelectDocumentCommand = new RelayCommand(_ => SelectDocument());
            RemovePageCommand = new RelayCommand(_ => RemovePage());
            MoveUpCommand = new RelayCommand((arg) => MoveUp(arg));
            MoveDownCommand = new RelayCommand((arg) => MoveDown(arg));
            AddTestCommand = new RelayCommand(_ => addTest());
        }

        public void Init()
        {
            Tests = new ObservableCollection<PageTest>();
        }

        public void SetParent(ManualNode parentNode)
        {
            parent = parentNode;
        }

        private void MoveUp(object arg)
        {
            if (parent == null) return;
            parent.MoveChildrenPage(this, true, arg.GetType());
        }

        private void MoveDown(object arg)
        {
            if (parent == null) return;
            parent.MoveChildrenPage(this, false, arg.GetType());
        }

        private void SelectDocument()
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.DefaultExt = ".doc";
            fileDialog.Filter = "Word documents|*.doc;*.docx";
            var result = fileDialog.ShowDialog();

            if (result == true)
            {
                DirPath = Path.GetDirectoryName(fileDialog.FileName);
                FileName = Path.GetFileName(fileDialog.FileName);
                PageTitle = FileName;
                FileDataId = -1;
            }
        }

        private void RemovePage()
        {
            OnPageRemove?.Invoke(this, null);
        }

        private void addTest()
        {
            if (_type != PageType.WORDFILE_WITH_TEST) return;

            if (Tests == null) Tests = new ObservableCollection<PageTest>();
            var newTest = new PageTest();
            newTest.OnTestDisplay += NewTest_OnTestStart;
            Tests.Add(newTest);
            newTest.CreateNew();
        }

        private void NewTest_OnTestStart(object sender, EventArgs e)
        {
            OnTestDisplay?.Invoke(this, e);
        }
    }
}
