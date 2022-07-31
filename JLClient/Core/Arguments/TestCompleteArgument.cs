using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JLClient.Core.Arguments
{
    public class TestCompleteArgument : EventArgs
    {
        public bool IsSuccess { get; set; }
        public string Question { get; set; }
        public List<string> Answeres { get; set; }
        public List<int> CorrectAnsweres { get; set; }
    }
}
