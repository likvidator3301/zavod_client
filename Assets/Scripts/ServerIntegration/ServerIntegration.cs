namespace ServerIntegration
{
    public class ServerIntegration
    {
        private string url = "http://localhost:5000";
        public readonly ZavodClient.ZavodClient client;

        public ServerIntegration()
        {
            client = new ZavodClient.ZavodClient(url);
        }
    }
}