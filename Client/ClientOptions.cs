namespace Client
{
    public class ClientOptions
    {
        public string BaseUri { get; set; }
            = "http://localhost:5001/service/";
        public int Timeout { get; set;}
            = 100;
    }
}
