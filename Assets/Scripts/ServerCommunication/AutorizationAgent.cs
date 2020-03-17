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
        private GoogleAuthDto authDto;

        public async Task<GoogleAuthDto> GetAuthDto()
        {
            if (authDto is null)
                authDto = await ServerClient.Client.User.GetAuthCode();

            return authDto;
        }


    }
}
