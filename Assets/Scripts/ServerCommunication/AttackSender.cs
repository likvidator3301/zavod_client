using System;
using System.Collections.Generic;
using System.Linq;
using ServerCommunication;
using Components;
using System.Timers;

namespace ServerCommunication
{
    public class AttackSender
    {
        public List<AttackInfo> attacks = new List<AttackInfo>();

        private Timer attacksTimer;

        public AttackSender()
        {
            attacksTimer = new Timer(100);
            attacksTimer.Elapsed += (e, v) => SendAttacks();
            attacksTimer.Start();
        }

        private async void SendAttacks()
        {
            var copyAttacks = attacks;
            attacks = new List<AttackInfo>();

            foreach(var attack in copyAttacks)
            {
                await ServerClient.Communication.Client.Unit.AttackUnit(attack.AttackUnit, attack.DamagedUnit, attack.Hp);
            }
        }
    }
}
