using JLClient.Core.Arguments;
using JLClient.Core.Observer;
using JLClient.Core.Settings;
using JLClient.MVVM.Model;
using JLClient.MVVM.Model.Components.Lesson;
using JLClient.MVVM.Model.Components.Profile.InnerModels;
using JLClient.MVVM.ViewModel.Components.Auth;
using JLClient.MVVM.ViewModel.Components.Course;
using JLClient.MVVM.ViewModel.Components.Editor;
using JLClient.MVVM.ViewModel.Components.Lesson;
using JLClient.MVVM.ViewModel.Components.Profile;

namespace JLClient.MVVM.ViewModel
{
    public class MainWindowViewModel : ObservableObject
    {
        // Текущий контекст окна
        private object _currentView;
        public object CurrentView { get { return _currentView; } set { _currentView = value; OnPropsChanged(); } }

        // Контексты вложенных окон
        public AuthWindowViewModel AuthVM { get; set; }
        public ProfileWindowViewModel ProfileVM { get; set; }
        public EditorWindowViewModel EditorVM { get; set; }
        public CourseWindowViewModel CourseVM { get; set; }
        public LessonWindowViewModel LessonVM { get; set; }

        // Главный обработчик
        private MainHandler handler;

        // Отображение пунктов меню
        public bool ShowExitFromProfile { get { return showExitFromProfile;  } set { showExitFromProfile = value; OnPropsChanged("ShowExitFromProfile"); } }
        private bool showExitFromProfile;
        public bool ShowEditorFull { get { return showEditorFull; } set { showEditorFull = value; OnPropsChanged("ShowEditorFull"); } }
        private bool showEditorFull;
        public bool ShowBackToProfileButton { get { return showBackToProfileButton; } set { showBackToProfileButton = value; OnPropsChanged("ShowBackToProfileButton"); } }
        private bool showBackToProfileButton;

        // Команды
        public RelayCommand ExitFromProfileCommand { get; set; }
        public RelayCommand OpenEditorFullAccessCommand { get; set; }
        public RelayCommand OpenEditorBaseAccessCommand { get; set; }
        public RelayCommand BackToMainPageCommand { get; set; }
        

        public MainWindowViewModel()
        {
            ShowExitFromProfile = false;
            ShowEditorFull = false;

            ExitFromProfileCommand = new RelayCommand(_ => ExitFromProfile());
            OpenEditorFullAccessCommand = new RelayCommand(_ => OpenEditorPage(true));
            OpenEditorBaseAccessCommand = new RelayCommand(_ => OpenEditorPage(false));
            BackToMainPageCommand = new RelayCommand(_ => BackToMainPage());

            handler = new MainHandler();
            handler.SetOSDataSetting();

            AuthVM = new AuthWindowViewModel();
            ProfileVM = new ProfileWindowViewModel();
            EditorVM = new EditorWindowViewModel();
            CourseVM = new CourseWindowViewModel();
            LessonVM = new LessonWindowViewModel();

            CurrentView = AuthVM;
            AuthVM.OnAuthCompleted += AuthVM_OnAuthCompleted;
            ProfileVM.CourseList_CourseSelected += ProfileVM_CourseList_CourseSelected;
        }

        private void ProfileVM_CourseList_CourseSelected(object sender, System.EventArgs e)
        {
            if (e is CourseSelectedArgument argument)
            {
                OpenCursePage(argument.Course);
            }
        }

        private void CourseVM_CourseDescription_OnLessonJoin(object sender, System.EventArgs e)
        {
            if (e is JointLessonArgument argument)
            {
                OpenLessonPage(argument.CourseId, argument.ManualId, argument.IsTeacher, argument.Type);
            }
        }

        private void AuthVM_OnAuthCompleted(object sender, System.EventArgs e)
        {
            ShowExitFromProfile = true;
            ShowEditorFull = handler.IsEditor;
            CurrentView = ProfileVM;
            ProfileVM.Init();
        }

        private void ExitFromProfile()
        {
            handler.ClearUserData();
            ShowExitFromProfile = false;
            ShowEditorFull = false;
            ShowBackToProfileButton = false;
            CurrentView = AuthVM;
        }

        private void OpenEditorPage(bool fullAccess)
        {
            if (fullAccess) ShowBackToProfileButton = true;
            CurrentView = EditorVM;
            EditorVM.Init(fullAccess);
        }

        private void OpenCursePage(CourseData course)
        {
            ShowBackToProfileButton = true;
            CurrentView = CourseVM;
            CourseVM.Init(course);
            CourseVM.CourseDescription_OnLessonJoin += CourseVM_CourseDescription_OnLessonJoin;
        }

        private void OpenLessonPage(int courseId, int manualId, bool isTeacher, LessonType type)
        {
            CurrentView = LessonVM;
            LessonVM.Init(courseId, manualId, isTeacher, type);
        }

        private void BackToMainPage()
        {
            ShowBackToProfileButton = false;
            CurrentView = ProfileVM;
        }
    }
}
