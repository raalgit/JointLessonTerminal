using JLClient.Core.Arguments;
using JLClient.Core.Observer;
using JLClient.MVVM.Model.Components.Profile;
using JLClient.MVVM.Model.Components.Profile.InnerModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JLClient.MVVM.ViewModel.Components.Profile.Components
{
    public class MyCoursesCardViewModel : ObservableObject
    {
        public bool IsLoading { get { return isLoading; } set { isLoading = value; OnPropsChanged("IsLoading"); } }
        private bool isLoading;

        public List<CourseData> Courses { get { return courses; } set { courses = value; OnPropsChanged("Courses"); } }
        private List<CourseData> courses;

        public CourseData SelectedCourse { get { return selectedCourse; } set { SelectCourse(value); OnPropsChanged("SelectedCourse"); } }
        private CourseData selectedCourse;

        public event EventHandler OnCourseSelected;

        private MyCourseCardHandler handler;

        public MyCoursesCardViewModel()
        {
            
        }

        public void Init()
        {
            handler = new MyCourseCardHandler();
            InitCourses();
        }

        private void InitCourses()
        {
            IsLoading = true;
            Task.Factory.StartNew(async () => {
                try
                {
                    Courses = await handler.GetMyCourses();
                }
                catch (Exception ex)
                {
                    // MESSAGE1
                }
                finally
                {
                    IsLoading = false;
                }
            });
        }

        private void SelectCourse(CourseData course)
        {
            OnCourseSelected?.Invoke(this, new CourseSelectedArgument()
            {
                Course = course
            });
        }
    }
}
