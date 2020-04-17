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
            if (ServerClient.Communication == null)
                return;

            ServerClient.Communication.Sessions.AllSessions = await ServerClient.Communication.Client.Session.GetAllSessions();

            if (ServerClient.Communication.Sessions.CurrentSessionGuid == Guid.Empty)
                return;

            ServerClient
                .Communication
                .Sessions
                .CurrentSessionInfo = await ServerClient
                                            .Communication
                                            .Client
                                            .Session
                                            .GetSession(ServerClient.Communication.Sessions.CurrentSessionGuid);
        }

        ~SessionUpdater()
        {
            sessionUpdateTimer.Stop();
        }
    }
}
