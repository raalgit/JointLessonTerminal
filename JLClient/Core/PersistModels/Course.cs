
namespace JLClient.Core.PersistModels
{
    public class Course
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int? avatarId { get; set; }
        public int manualId { get; set; }
        public int disciplineId { get; set; }
    }
}
