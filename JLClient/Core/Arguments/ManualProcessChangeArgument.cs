using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JLClient.Core.Arguments
{
    public class ManualProcessChangeArgument : EventArgs
    {
        public int Step { get; set; }
    }
}
