using JLClient.Core.Observer;
using System;

namespace JLClient.Core.Manual
{
    [Serializable]
    public class Block : ObservableObject, IBlock
    {
        public BlockAccess access { get { return _access; } set { _access = value; OnPropsChanged("access"); } }
        public string title { get { return _title; } set { _title = value; OnPropsChanged("title"); } }

        private BlockAccess _access;
        private string _title;
    }
}
