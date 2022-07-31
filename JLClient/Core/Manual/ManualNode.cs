using JLClient.Core.Observer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace JLClient.Core.Manual
{
    [Serializable]
    public class ManualNode : Block, IBlock
    {
        #region Экспорт данные
        public bool CanAddBlock { get { return _canAddBlock; } set { _canAddBlock = value; OnPropsChanged("CanAddBlock"); } }
        public bool CanAddPage { get { return _canAddPage; } set { _canAddPage = value; OnPropsChanged("CanAddPage"); } }
        public ObservableCollection<ManualNode> Children { 
            get { return _children; } 
            set 
            { 
                _children = value; 
                foreach (var child in _children)
                {
                    child.SetParent(this);
                    child.OnNodeRemove += NewNode_OnNodeRemove;
                    child.OnTestDisplay += Component_OnTestStart;
                }
                OnPropsChanged("Children");
                OnPropsChanged("Items");
            } 
        }
        public BlockType BlockType { get { return _blockType; } set { _blockType = value; OnPropsChanged("BlockType"); } }
        public ObservableCollection<Page> Pages { 
            get { return _pages; } 
            set { 
                _pages = value; 
                foreach (var page in _pages)
                {
                    page.SetParent(this);
                    page.OnPageRemove += NewPage_OnPageRemove;
                    page.OnTestDisplay += Component_OnTestStart;
                }
                OnPropsChanged("Pages");
                OnPropsChanged("Items");
            } 
        }
        #endregion

        #region Поля
        private bool _canAddBlock;
        private bool _canAddPage;
        private BlockType _blockType;
        private ObservableCollection<ManualNode> _children;
        private ObservableCollection<Page> _pages;
        private ManualNode parent;
        #endregion

        #region Команды
        [JsonIgnore] public RelayCommand AddNewPageCommand { get; set; }
        [JsonIgnore] public RelayCommand AddNewNodeCommand { get; set; }
        [JsonIgnore] public RelayCommand MoveUpCommand { get; set; }
        [JsonIgnore] public RelayCommand MoveDownCommand { get; set; }
        [JsonIgnore] public RelayCommand RemoveNodeCommand { get; set; }
        #endregion

        #region События
        public event EventHandler OnNodeRemove;
        public event EventHandler OnTestDisplay;
        #endregion

        [JsonIgnore]
        public List<object> Items
        {
            get
            {
                var resp = new List<object>();
                resp.AddRange(Children);
                resp.AddRange(Pages);
                return resp;
            }
        }

        public ManualNode()
        {
            RemoveNodeCommand = new RelayCommand(_ => RemoveNode());
            AddNewPageCommand = new RelayCommand(_ => AddPage());
            AddNewNodeCommand = new RelayCommand(_ => AddNode());
            MoveUpCommand = new RelayCommand((arg) => MoveUp(arg));
            MoveDownCommand = new RelayCommand((arg) => MoveDown(arg));
        }

        public void Init()
        {
            Children = new ObservableCollection<ManualNode>();
            Pages = new ObservableCollection<Page>();
            CanAddBlock = true;
            CanAddPage = true;
        }

        public void SetParent(ManualNode parentNode)
        {
            parent = parentNode;
        }

        private void RemoveNode()
        {
            OnNodeRemove?.Invoke(this, null);
        }

        private void MoveUp(object arg)
        {
            if (parent == null) return;
            parent.MoveChildrenNode(this, true, arg.GetType());
        }

        private void MoveDown(object arg)
        {
            if (parent == null) return;
            parent.MoveChildrenNode(this, false, arg.GetType());
        }

        public void MoveChildrenNode(ManualNode child, bool top, Type childType)
        {
            var itemIndex = Children.IndexOf(child);
            if (itemIndex == -1 || itemIndex == 0 && top || itemIndex == Children.Count - 1 && !top)
                return;

            Children.Move(itemIndex, top ? --itemIndex : ++itemIndex);
            OnPropsChanged("Items");
        }

        public void MoveChildrenPage(Page child, bool top, Type childType)
        {
            var itemIndex = Pages.IndexOf(child);
            if (itemIndex == -1 || itemIndex == 0 && top || itemIndex == Pages.Count - 1 && !top)
                return;

            Pages.Move(itemIndex, top ? --itemIndex : ++itemIndex);
            OnPropsChanged("Items");
        }

        public ManualNode AddNode(BlockAccess blockAccess = BlockAccess.READ_ALL, string blockTitle = "Новый блок")
        {
            if (Pages != null && Pages.Count > 0)
                throw new InvalidOperationException("Нельзя добавить узел в узел со страницами");

            if (string.IsNullOrEmpty(blockTitle))
                blockTitle = this.title + "-" + Children.Count + 1;

            var newNode = new ManualNode()
            {
                access = blockAccess,
                title = blockTitle,
                Children = new ObservableCollection<ManualNode>(),
                Pages = new ObservableCollection<Page>()
            };

            newNode.Init();
            newNode.SetParent(this);

            Children.Add(newNode);
            OnPropsChanged("Items");
            BlockType = BlockType.BLOCK_WITH_NODES;
            CanAddPage = false;
            newNode.OnNodeRemove += NewNode_OnNodeRemove;
            newNode.OnTestDisplay += Component_OnTestStart;
            return newNode;
        }

        private void NewNode_OnNodeRemove(object sender, EventArgs e)
        {
            if (sender is ManualNode node)
            {
                Children.Remove(node);
                OnPropsChanged("Items");
                if (Children.Count == 0) CanAddPage = true;
            }
        }

        public void AddPage(string pageFileDirPath = null, string pageFileName = null, PageType type = PageType.WORDFILE)
        {
            if (Children != null && Children.Count > 0)
                throw new InvalidOperationException("Нельзя добавить страницу в узловой узел");

            var newPage = new Page()
            {
                DirPath = pageFileDirPath,
                FileName = pageFileName,
                PageTitle = pageFileName ?? "Новая страница",
                FileDataId = -1,
                Id = null,
                Type = type
            };

            newPage.Init();
            newPage.SetParent(this);
            Pages.Add(newPage);
            OnPropsChanged("Items");
            CanAddBlock = false;
            newPage.OnPageRemove += NewPage_OnPageRemove;
            newPage.OnTestDisplay += Component_OnTestStart;
        }

        private void Component_OnTestStart(object sender, EventArgs e)
        {
            OnTestDisplay?.Invoke(sender, e);
        }

        private void NewPage_OnPageRemove(object sender, EventArgs e)
        {
            if (sender is Page page)
            {
                Pages.Remove(page);
                OnPropsChanged("Items");
                if (Pages.Count == 0) CanAddBlock = true;
            }
        }
    }
}
