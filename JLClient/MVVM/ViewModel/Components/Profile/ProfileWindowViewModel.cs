using JLClient.Core.Observer;
using JLClient.MVVM.ViewModel.Components.Profile.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JLClient.MVVM.ViewModel.Components.Profile
{
    public class ProfileWindowViewModel : ObservableObject
    {
        // Контексты вложенных окон
        public AchievementsCardViewModel AchievementsCardVM { get; set; }
        public MyCoursesCardViewModel MyCoursesCardVM { get; set; }
        public StatisticCardViewModel StatisticCardVM { get; set; }
        public UserCardViewModel UserCardVM { get; set; }

        public event EventHandler CourseList_CourseSelected;

        public ProfileWindowViewModel()
        {
            AchievementsCardVM = new AchievementsCardViewModel();
            MyCoursesCardVM = new MyCoursesCardViewModel();
            StatisticCardVM = new StatisticCardViewModel();
            UserCardVM = new UserCardViewModel();  
            

        }

        public void Init()
        {
            UserCardVM.Init();
            MyCoursesCardVM.Init();

            MyCoursesCardVM.OnCourseSelected += MyCoursesCardVM_OnCourseSelected;
        }

        private void MyCoursesCardVM_OnCourseSelected(object sender, EventArgs e)
        {
            CourseList_CourseSelected?.Invoke(sender, e);
        }
    }
}
