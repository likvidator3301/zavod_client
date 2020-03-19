using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCommunication
{
    public class AutorizationAgent
    {
        public bool isAuth = ServerClient.Client.User.IsRegistered;

        private GoogleAuthDto authDto;

        public AutorizationAgent()
        {
            ServerClient.Client.User.OnRegisterSuccessful += (a) => isAuth = true;
        }

        public async Task<GoogleAuthDto> GetAuthDto()
        {
            if (authDto is null)
                authDto = await ServerClient.Client.User.GetAuthCode();
            return authDto;
        }
    }
}
