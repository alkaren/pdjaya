using System;
using System.Net.Http;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;
//using Grpc.Core;
//using Databucket;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.IO;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;
using PDJaya.Models;

namespace TestingApp
{
    /*
    
        public class ArtosSync
        {
            public ArtosSync()
            {
            }
            
            public void SyncDataFromServer()
            {
                //sync data from server
                Channel channel = new Channel($"192.168.100.3:50051", ChannelCredentials.Insecure);

                var client = new Databucket.DataSync.DataSyncClient(channel);
                Console.WriteLine("data sync start..");
                var reply = client.GetDailyData(new SyncRequest { Terminalid = 1, Currentdate = DateTime.Now.ToString("yyyy-MMM-dd") });
                //Console.WriteLine("Greeting: " + reply.);

                if (!string.IsNullOrEmpty(reply.Ticketpool))
                {
                    var ticketdata = JsonConvert.DeserializeObject<List<TicketPool>>(reply.Ticketpool);
                }
                if (!string.IsNullOrEmpty(reply.Cardlog))
                {
                    var cardlog = JsonConvert.DeserializeObject<List<CardLog>>(reply.Cardlog);
                }
                Console.WriteLine("data sync completed..");
                channel.ShutdownAsync().Wait();


            }
        }
    */

    class Program
    {
        public static IConfigurationRoot Configuration;
        static void Main(string[] args)
        {

            //TestToken();
            //ArtosSync sync = new ArtosSync();
            //sync.SyncDataFromServer();
            //Console.WriteLine("Hello World!");
            AddInitialData();
            Console.ReadLine();
        }
        
        static void AddInitialData()
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (String.IsNullOrWhiteSpace(environment))
                throw new ArgumentNullException("Environment not found in ASPNETCORE_ENVIRONMENT");

            Console.WriteLine("Environment: {0}", environment);


            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(AppContext.BaseDirectory))
                .AddJsonFile("appsettings.json", optional: true);
            if (environment == "Development")
            {

                builder
                    .AddJsonFile(
                        Path.Combine(AppContext.BaseDirectory, string.Format("..{0}..{0}..{0}", Path.DirectorySeparatorChar), $"appsettings.{environment}.json"),
                        optional: true
                    );
            }
            else
            {
                builder
                    .AddJsonFile($"appsettings.{environment}.json", optional: true);
            }
            //get config
            Configuration = builder.Build();
            var sqlConnectionString = Configuration.GetConnectionString("MySqlCon");
            PDJayaDB.sqlConnectionString = sqlConnectionString;
            var DbContext = new PDJayaDB();
            DbContext.Database.EnsureCreated();
            /*
            var MaxTerminal = 400;
            for (int i = 1; i <= MaxTerminal; i++)
            {
                var newConfig = new TerminalConfig();
                newConfig.Duration = new TimeSpan(3, 0, 0);
                newConfig.Moda = "AK";
                newConfig.TerminalId = i;
                newConfig.DesignWidth = 963;
                newConfig.DesignHeight = 1278;
                var numStr = i.ToString().PadLeft(4, '0');
                newConfig.Location = "T"+numStr;
                newConfig.DeviceClient = $"TerminalTicket{i}";
                newConfig.MQTTHost = "52.230.8.37";
                newConfig.UserMQTT = "mifmasterz";
                newConfig.PassMQTT = "123qweasd";
                newConfig.SendTopicMQTT = "mifmasterz/artos/control";
                newConfig.ReceiveTopicMQTT = "mifmasterz/artos/data";
                newConfig.GrpcHost = "52.230.8.37";
                newConfig.GrpcPort = 50051;
                newConfig.PrinterName = "ZJ-58";
                newConfig.ShelterId = i;
                newConfig.AutoCloseFormInterval = 2;
                newConfig.ShelterTimeOut = 10;
                newConfig.GMTTimeGap = 7;
                DbContext.TerminalConfigs.Add(newConfig);
                DbContext.SaveChanges();
            }
            */
            //create user
            for (int i = 1; i <= 10; i++)
            {
                UserProfile usr = new UserProfile();
                usr.Id = i;
                usr.UserId = i.ToString();
                usr.Firstname = "user";
                usr.Lastname = i.ToString();
                usr.Email = $"user{i}@yahoo.com";
                usr.FullName = $"user{i}";
                usr.IsActive = true;
                usr.Password = $"123";
                usr.Phone = $"08174810345";
                usr.Role = $"Admin";
                usr.UserName = usr.Email;
                DbContext.UserProfiles.Add(usr);
                DbContext.SaveChanges();
            }
            //create SP
            /*
            string SPStrZ = File.ReadAllText("SP.txt");
            var SPStr = Regex.Split(SPStrZ, "--break--");

            MySqlConnection con = new MySqlConnection(sqlConnectionString);
            con.Open();
            foreach (var str in SPStr)
            {
                MySqlCommand cmd = new MySqlCommand(str, con);
                cmd.ExecuteNonQuery();
            }
            con.Close();
            */
            Console.WriteLine("data initial has been inserted.");

        }
        async static void TestToken(){
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
                Console.WriteLine(JArray.Parse(content));
            }
        }
    }
}
