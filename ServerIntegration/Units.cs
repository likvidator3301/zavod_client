using Models;
using ZavodClient;

namespace ServerIntegration
{
    public class Units
    {
        private ZavodClient.ZavodClient client;
        
        public Units(ZavodClient.ZavodClient client)
        {
            this.client = client;
        }
        
        public void Move()
        {
            client.Unit.SendMoveUnits();
        }

        public void Attack()
        {
            client.Unit.SendAttackUnits();
        }

        public void Create()
        {
            client.Unit.CreateUnit();
        }

        public void Delete()
        {
            client.Unit.DeleteUnit();
        }
    }
}