using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDJaya.Kiosk.Helpers
{
    public class ServiceManagement
    {
        public static async Task<bool> GetAccessToken()
        {
            // discover endpoints from metadata
            var Disclient = new DiscoveryClient(GlobalVars.Config.ServiceAuth);
            Disclient.Policy.RequireHttps = false;
            var disco = await Disclient.GetAsync();
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return false;
            }

            // request token
            var tokenClient = new TokenClient(disco.TokenEndpoint, GlobalVars.Config.ApiScope, GlobalVars.Config.ApiKey);
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("transactionapi");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return false;
            }
            GlobalVars.Config.AccessToken = tokenResponse.AccessToken;
            Console.WriteLine(tokenResponse.Json);
            return true;
        }
    }
}
