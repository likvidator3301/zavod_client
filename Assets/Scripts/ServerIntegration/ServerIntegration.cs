using System;
using System.Threading.Tasks;
using UnityEngine;

namespace ServerIntegration
{
    public class ServerIntegration
    {
        private string url = "http://localhost:5000";
        public readonly ZavodClient.ZavodClient client;

        private string myCookie =
            $"CfDJ8IGASaB2OJhNiYicgRiEXj_A3KY4DlZFnxis1eabigNmfdSB5GQgR93I8peVi6kCvvd2m8TwXcRhPWlBqRLQWKtKBuxtBWkwF97i8x5G-Dja49Jvp0ScyGTT5NLXlGrZJYLCtRTApOk72GqMeOZtt51DaMeZjNHNef2A_vC6EQMXQsX7grLlNPOXT3Uz66jV0GCs0qE9cM0tldH_9FVVQDGBs4kaESoS5MsMJmJqSz1QHeqnWBgepS9LFBeZplgyu_x5FgPbayy7Gzp9B4JCuSlvIkq27vZZaKfT1tkAiyi_RLlNzj5U30acfgqdzpqEZy6HIT5grkoRoQqEpnWWUtR6AGLNpb2Z2llC4c1ZVlmz2ehIECM67jaklTXZYhEBNuZyhwIq-cj9OLfeFwfGaEyQ-1zc7egbYb9RqTqNWIZpWQWUfvVGePWYNxvet0Wlf65HhIPH3b1FDaJCl73wfRE8V3ukxB87nJTVyHZVC6GzWGWKs2sJ4u1UD5TOGOsRlqtBjpU_pp5SsjHbyI67v4q0lI0qRGOCRV_CSXJydZN5fYR1uiwozMSitLGi3JPAmO2xer8RTvlSpEU6FCh9946ActEMQJMEZT_H2cvF1aT9mrTtbeaUPOZ6ySLaK5CXUDQQTta1SQr4dSHUtJRGZTX8GvTWD8iWz3gzSdaDaA47l9_gYRYTz5rASLPSk4Sopz0jJf_Vi9HUEl3dDAv8L0IoY50l_rDKUQvdue6fkEwUg55ZWcf89mlYCEGkbWTMMTWf4MJn2juL9yPNfyWhL70bQe7HErFWjTkftjyTF1Yd3ytkWWT7xXh5TYEorPrzL8OY9o-FXL02bnQog0ILncL7hQOTe4XJpBJaMUTiklka2UjmUPR30wm3bUWQihqm9Q";
        public ServerIntegration()
        {
            client = new ZavodClient.ZavodClient(url);
            ZavodClient.ZavodClient.Client.DefaultRequestHeaders.Add("Cookie", ".AspNetCore.Cookies="+myCookie);
        }
        
        public async Task<T> GetInfoFromServer<T>(Task<T> task, int timeout = 1000)
        {
            var info = default(T);
            if (await Task.WhenAny(task, Task.Delay(timeout)) == task)
            {
                info = task.Result;
            }
            return info;
        }
    }
}