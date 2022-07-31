using JLClient.MVVM.Model.Components.Lesson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JLClient.Core.Arguments
{
    public class JointLessonArgument : EventArgs
    {
        public LessonType Type { get; set; }
        public int CourseId { get; set; }
        public int ManualId { get; set; }
        public bool IsTeacher { get; set; }
    }
}
