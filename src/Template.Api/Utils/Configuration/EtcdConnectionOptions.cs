namespace Template.Api.Utils.Configuration
{
    public class EtcdConnectionOptions
    {
        public string[] Urls { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string RootKey { get; set; }
    }
}