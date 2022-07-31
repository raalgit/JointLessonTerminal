using JLClient.Core.Arguments.InnerModels;
using JLClient.MVVM.ViewModel.Components.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JLClient.Core.Arguments
{
    public class OnStartTestArgument : EventArgs
    {
        public TestCardViewModel cardVM { get; set; }
        public bool Show { get; set; }
    }
}
