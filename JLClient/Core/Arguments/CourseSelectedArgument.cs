using JLClient.MVVM.Model.Components.Profile.InnerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JLClient.Core.Arguments
{
    public class CourseSelectedArgument : EventArgs
    {
        public CourseData Course { get; set; }
    }
}
