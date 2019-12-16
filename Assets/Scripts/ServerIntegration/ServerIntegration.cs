namespace ServerIntegration
{
    public class ServerIntegration
    {
        private string url = "http://127.0.0.1:1234";
        public readonly ZavodClient.ZavodClient client;

        public ServerIntegration()
        {
            client = new ZavodClient.ZavodClient(url);
        }
    }
}