using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using UnityEngine;

namespace ServerCommunication
{
    public class SessionUpdater
    {
        private Timer sessionUpdateTimer;

        public SessionUpdater()
        {
            sessionUpdateTimer = new Timer(831);
            sessionUpdateTimer.Elapsed += (e, o) => SessionUpdate();
            sessionUpdateTimer.Start();
        }

        private async void SessionUpdate()
        {
            ServerClient.Sessions.AllSessions = await ServerClient.Client.Session.GetAllSessions();

            if (ServerClient.Sessions.CurrentSessionGuid == Guid.Empty)
                return;

            ServerClient.Sessions.CurrentSessionInfo = await ServerClient.Client.Session.GetSession(ServerClient.Sessions.CurrentSessionGuid);
        }

        ~SessionUpdater()
        {
            sessionUpdateTimer.Stop();
        }
    }
}
