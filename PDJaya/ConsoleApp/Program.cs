using IdentityModel.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PDJaya.Kiosk.Helpers;
using PDJaya.Models;
using STI.CardReader;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            var Reader = new CardReader();
            Reader.Connect(ConfigurationManager.AppSettings["ComPort"]);
            Reader.InitReader();
            //Reader.SetBuzzer(true);
            var item = Reader.CheckBalance();
            Console.WriteLine("Card No:" + item.CardNo);
            */
            TestToken().GetAwaiter().GetResult();
            
            Console.ReadLine();
        }

        async static Task TestToken()
        {
            var DBcontext = new PDJayaDBSqlite();
            // discover endpoints from metadata
            var Disclient = new DiscoveryClient("http://pdjayaauthapi.azurewebsites.net/");
            Disclient.Policy.RequireHttps = false;
            var disco = await Disclient.GetAsync();
            //var disco = await DiscoveryClient.GetAsync("http://pdjayaauthapi.azurewebsites.net/");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            // request token
            var tokenClient = new TokenClient(disco.TokenEndpoint, "serviceapp", "4515647407");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("transactionapi");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);


            /*
            // request token
            var tokenClient = new TokenClient(disco.TokenEndpoint, "webapp1", "web123");
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("admin", "admin", "masterdataapi");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");
            */
            // call api
            /*
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            var response = await client.GetAsync("http://pdjayaservice.azurewebsites.net/api/Bills");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine((content));
                //
            }*/
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);
            var response = await client.GetAsync($"http://pdjayaservice.azurewebsites.net/api/Bills/GetBillByTransactionCode?CurrentDate={DateTime.Now.ToString("yyyy-MM-dd")}&TransactionCode={"1"}&StoreNo={"S001"}");

            //var response = await client.GetAsync("http://pdjayaservice.azurewebsites.net/api/UserProfiles");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("sync user profile is failed : " + response.StatusCode);

            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                var output = JsonConvert.DeserializeObject<OutputData>(content);
                if (output != null && output.IsSucceed)
                {
                    Console.WriteLine(output.Data);
                    /*
                    var result = ((JArray)output.Data).ToObject<List<UserProfile>>();
                    if (result != null)
                    {
                        DBcontext.DeleteAllData<UserProfile>("UserProfile");
                        DBcontext.InsertBulkData<UserProfile>(result);
                        Console.WriteLine("sync user profile is succeed ");
                    }*/
                }

            }
        }
    }
}
