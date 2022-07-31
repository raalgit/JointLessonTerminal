using System;

namespace JLClient.Core.PersistModels
{
    public class UserRemoteAccess
    {
        public int id { get; set; }
        public int courseId { get; set; }
        public int userId { get; set; }
        public string connectionData { get; set; }
        public DateTime startDate { get; set; }
    }
}
