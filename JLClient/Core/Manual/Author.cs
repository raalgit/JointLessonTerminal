using JLClient.Core.Observer;
using System;

namespace JLClient.Core.Manual
{
    [Serializable]
    public class Author : ObservableObject
    {
        public string name { get { return _name; } set { _name = value; OnPropsChanged("name"); } }
        public string mail { get { return _mail; } set { _mail = value; OnPropsChanged("mail"); } }

        private string _name;
        private string _mail;
    }
}
