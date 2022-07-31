using System;

namespace JLClient.Core.PersistModels
{
    public class LessonTabel
    {
        public int id { get; set; }
        public int userId { get; set; }
        public int lessonId { get; set; }
        public bool handUp { get; set; }
        public DateTime enterDate { get; set; }
        public DateTime? leaveDate { get; set; }
    }
}
