
namespace JLClient.Core.PersistModels
{
    public class GroupAtCourse
    {
        public int id { get; set; }
        public int courseId { get; set; }
        public int groupId { get; set; }
        public bool isActive { get; set; }
    }
}
