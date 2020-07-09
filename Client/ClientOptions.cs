namespace Client
{
    public class ClientOptions
    {
        public string BaseUri { get; set; }
            = "http://localhost:5001/service/";
        public string ImageAPIBaseUri { get; set; }
        public string AccountsUri { get; set; }
        public JobmailOptions JobmailAPI { get; set; }
        public string WebUri { get; set; }
        public int Timeout { get; set;}
            = 100;
    }
}
