
namespace JLClient.Core.PersistModels
{
    public class Statistic
    {
        public int id { get; set; }
        public int userId { get; set; }
        public string successExecution { get; set; }
        public string failedExecution { get; set; }
    }
}
