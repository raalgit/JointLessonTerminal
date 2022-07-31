

namespace JLClient.Core.PersistModels
{
    public class CourseTeacher
    {
        public int id { get; set; }
        public int courseId { get; set; }
        public int userId { get; set; }
        public bool onLesson { get; set; }
    }
}
