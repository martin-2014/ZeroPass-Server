namespace ZeroPass.Storage
{
    public class ConnectionOption
    {
        public string MasterMysqlConnectionString { get; set; } = string.Empty;
        public string ReadonlyMysqlConnectionString { get; set; } = string.Empty;
    }
}
