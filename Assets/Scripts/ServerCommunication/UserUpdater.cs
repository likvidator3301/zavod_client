using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ServerCommunication
{
    public class UserUpdater
    {
        private Timer updTimer;

        public UserUpdater()
        {
            updTimer = new Timer(1671);
            updTimer.Elapsed += (e, o) => UserUpdate();
            updTimer.Start();
        }

        private async void UserUpdate()
        {
            if (ServerClient.Communication.Client.User.IsRegistered)
                ServerClient.Communication.userInfo = await ServerClient.Communication.Client.User.GetUser();
        }

        ~UserUpdater()
        {
            updTimer.Stop();
        }
    }
}
