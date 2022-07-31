using JLClient.Core.Arguments;
using JLClient.Core.Observer;
using JLClient.MVVM.Model.Components.Course;
using JLClient.MVVM.Model.Components.Profile.InnerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JLClient.MVVM.ViewModel.Components.Course.Components
{
    public class CourseDescriptionViewModel : ObservableObject
    {
        public string Title { get { return title; } set { title = value; OnPropsChanged("Title"); } }
        private string title;
        public string Description { get { return description; } set { description = value; OnPropsChanged("Description"); } }
        private string description;
        public List<Core.PersistModels.Group> Groups { get { return groups; } set { groups = value; OnPropsChanged("Groups"); } }
        private List<Core.PersistModels.Group> groups;
        public Core.PersistModels.Group SelectedGroup { get { return selectedGroup; } set { selectedGroup = value; OnPropsChanged("SelectedGroup"); } }
        public Core.PersistModels.Group selectedGroup { get; set; }

        public bool IsLoading { get { return isLoading; } set { isLoading = value; OnPropsChanged("IsLoading"); } }
        private bool isLoading;
        public bool ShowJoinLessonButton { get { return showJoinLessonButton; } set { showJoinLessonButton = value; OnPropsChanged("ShowJoinLessonButton"); } }
        private bool showJoinLessonButton;
        public bool ShowStartLessonButton { get { return showStartLessonButton; } set { showStartLessonButton = value; OnPropsChanged("ShowStartLessonButton"); } }
        private bool showStartLessonButton;
        public bool ShowStopLessonButton { get { return showStopLessonButton; } set { showStopLessonButton = value; OnPropsChanged("ShowStopLessonButton"); } }
        private bool showStopLessonButton;

        public RelayCommand JoinLessonCommand { get; set; }
        public RelayCommand JoinSrsLessonCommand { get; set; }
        public RelayCommand StartLessonCommand { get; set; }
        public RelayCommand StopLessonCommand { get; set; }

        public event EventHandler OnLessonJoin;
        private CourseDescriptionhandler handler;
        private int courseId;
        private int manualId;
        private bool isTeacher;

        public CourseDescriptionViewModel()
        {
            JoinLessonCommand = new RelayCommand(_ => JoinLesson());
            JoinSrsLessonCommand = new RelayCommand(_ => JoinSrsLesson());
            StartLessonCommand = new RelayCommand(_ => StartLesson());
            StopLessonCommand = new RelayCommand(_ => StopLesson());

            handler = new CourseDescriptionhandler();
        }

        public void Init(CourseData course)
        {
            if (course == null) return;

            InitCourseDescription(course);
            LoadCourseData(course.Id);
        }

        private void InitCourseDescription(CourseData course)
        {
            ShowJoinLessonButton = false;
            ShowStartLessonButton = false;
            ShowStopLessonButton = false;
            courseId = course.Id;
            manualId = course.ManualId;
            Title = course.Name;
            Description = course.Description;
        }

        private void LoadCourseData(int courseId)
        {
            IsLoading = true;
            Task.Factory.StartNew(async () => {
                try
                {
                    var resp = await handler.LoadCourseData(courseId);
                    Groups = resp.courseGroups;
                    ShowJoinLessonButton = resp.lessonIsActive;
                    ShowStartLessonButton = resp.isTeacher && !resp.lessonIsActive;
                    ShowStopLessonButton = resp.isTeacher && resp.lessonIsActive;
                    isTeacher = resp.isTeacher;

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

        private void JoinLesson()
        {
            var arg = new JointLessonArgument();
            arg.CourseId = courseId;
            arg.ManualId = manualId;
            arg.IsTeacher = isTeacher;
            arg.Type = Model.Components.Lesson.LessonType.ONLINE;
            OnLessonJoin?.Invoke(this, arg);
        }

        private void JoinSrsLesson()
        {
            var arg = new JointLessonArgument();
            arg.CourseId = courseId;
            arg.ManualId = manualId;
            arg.IsTeacher = isTeacher;
            arg.Type = Model.Components.Lesson.LessonType.SRS;
            OnLessonJoin?.Invoke(this, arg);
        }

        private void StartLesson()
        {
            if (courseId == 0 || selectedGroup == null) return;
            IsLoading = true;
            Task.Factory.StartNew(async () => {
                try
                {
                    var resp = await handler.StartLesson(courseId, selectedGroup.id);
                    ShowJoinLessonButton = resp;
                    ShowStartLessonButton = !resp;
                    ShowStopLessonButton = resp;
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

        private void StopLesson()
        {
            if (courseId == 0) return;
            IsLoading = true;
            Task.Factory.StartNew(async () => {
                try
                {
                    var resp = await handler.StopLesson(courseId);
                    ShowJoinLessonButton = !resp;
                    ShowStartLessonButton = resp;
                    ShowStopLessonButton = !resp;
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
    }
}
