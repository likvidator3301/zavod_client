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
        public bool isAuth;

        private GoogleAuthDto authDto;

        public AutorizationAgent()
        {
            
        }

        public AutorizationAgent(ZavodClient.ZavodClient client)
        {
            isAuth = client.User.IsRegistered;
            client.User.OnRegisterSuccessful += (a) => isAuth = true;
        }

        public async Task<GoogleAuthDto> GetAuthDto()
        {
            if (authDto is null)
                authDto = await ServerClient.Communication.Client.User.GetAuthCode();
            return authDto;
        }
    }
}
