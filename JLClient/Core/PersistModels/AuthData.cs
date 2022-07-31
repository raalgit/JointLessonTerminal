
namespace JLClient.Core.PersistModels
{
    public class AuthData
    {
        public int id { get; set; }
        public int userId { get; set; }
        public string login { get; set; }
        public string passwordHash { get; set; }
    }
}
