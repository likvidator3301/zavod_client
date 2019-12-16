using ZavodClient;

namespace ServerIntegration
{
    public class ServerIntegration
    {
        private string url = "http://127.0.0.1:1234";
        private readonly ZavodClient.ZavodClient client;
        public readonly Units Units;
        public readonly Buildings Building;

        public ServerIntegration()
        {
            client = new ZavodClient.ZavodClient(url);
            Units = new Units(client);
            Building = new Buildings(client);
        }
    }
}