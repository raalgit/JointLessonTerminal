using JLClient.MVVM.Model.Components.Profile.InnerModels;
using JLClient.MVVM.ViewModel.Components.Course.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JLClient.MVVM.ViewModel.Components.Course
{
    public class CourseWindowViewModel
    {
        public CourseDescriptionViewModel CourseDescriptionVM { get; set; }

        public event EventHandler CourseDescription_OnLessonJoin;

        public CourseWindowViewModel()
        {
            CourseDescriptionVM = new CourseDescriptionViewModel();
        }

        public void Init(CourseData course)
        {
            CourseDescriptionVM.Init(course);
            CourseDescriptionVM.OnLessonJoin += CourseDescriptionVM_OnLessonJoin;
        }

        private void CourseDescriptionVM_OnLessonJoin(object sender, EventArgs e)
        {
            CourseDescription_OnLessonJoin?.Invoke(sender, e);
        }
    }
}
