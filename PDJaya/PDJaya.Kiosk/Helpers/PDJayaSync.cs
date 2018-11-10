using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using IdentityModel.Client;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using PDJaya.Models;
using PDJaya.Tools;

namespace PDJaya.Kiosk.Helpers
{
    public enum SyncMode { Push, Pull};
    public class PDJayaSync
    {
        public PDJayaDBSqlite DBcontext { set; get; }
        public PDJayaSync()
        {
            if (DBcontext == null)
            {
                DBcontext = new PDJayaDBSqlite();
            }
            
        }

       

        public async Task<bool> RunSync(SyncMode Mode)
        {
            try
            {
                var hasil = false;
                if (string.IsNullOrEmpty(GlobalVars.Config.AccessToken))
                {
                    hasil = await ServiceManagement.GetAccessToken();
                }
                if (!hasil) return false;
                // call api
                var client = new HttpClient();
                client.SetBearerToken(GlobalVars.Config.AccessToken);

                //sync transaction
                if (Mode == SyncMode.Pull)
                {
                    //sync master data
                    //userprofile
                    var response = await client.GetAsync(GlobalVars.Config.ServiceHost + "api/UserProfiles");
                    if (!response.IsSuccessStatusCode)
                    {
                        Logs.WriteLog("sync user profile is failed : " + response.StatusCode);

                    }
                    else
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var output = JsonConvert.DeserializeObject<OutputData>(content);
                        if (output != null && output.IsSucceed)
                        {
                            var result = ((JArray)output.Data).ToObject<List<UserProfile>>();
                            if (result != null)
                            {
                                DBcontext.DeleteAllData<UserProfile>("UserProfile");
                                DBcontext.InsertBulkData<UserProfile>(result);
                                Logs.WriteLog("sync user profile is succeed ");
                            }
                        }
                        
                    }
                    //market
                    response = await client.GetAsync(GlobalVars.Config.ServiceHost + "api/Markets");
                    if (!response.IsSuccessStatusCode)
                    {
                        Logs.WriteLog("sync market is failed : " + response.StatusCode);

                    }
                    else
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var output = JsonConvert.DeserializeObject<OutputData>(content);
                        if (output != null && output.IsSucceed)
                        {
                           
                            var result = ((JArray)output.Data).ToObject<List<Market>>();
                            if (result != null)
                            {
                                DBcontext.DeleteAllData<Market>("Market");
                                DBcontext.InsertBulkData<Market>(result);
                                Logs.WriteLog("sync market is succeed ");
                            }
                        }
                        
                    }
                    //tenant
                    response = await client.GetAsync(GlobalVars.Config.ServiceHost + "api/Tenants");
                    if (!response.IsSuccessStatusCode)
                    {
                        Logs.WriteLog("sync tenant is failed : " + response.StatusCode);

                    }
                    else
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var output = JsonConvert.DeserializeObject<OutputData>(content);
                        if (output != null && output.IsSucceed)
                        {
                            var result = ((JArray)output.Data).ToObject<List<Tenant>>();
                            if (result != null)
                            {
                                DBcontext.DeleteAllData<Tenant>("Tenant");
                                DBcontext.InsertBulkData<Tenant>(result);
                                Logs.WriteLog("sync tenant is succeed ");
                            }
                        }
                        
                    }
                    //tenant card
                    response = await client.GetAsync(GlobalVars.Config.ServiceHost + "api/TenantCards");
                    if (!response.IsSuccessStatusCode)
                    {
                        Logs.WriteLog("sync tenant card is failed : " + response.StatusCode);

                    }
                    else
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var output = JsonConvert.DeserializeObject<OutputData>(content);
                        if (output != null && output.IsSucceed)
                        {
                         
                            var result = ((JArray)output.Data).ToObject<List<TenantCard>>();
                            if (result != null)
                            {
                                DBcontext.DeleteAllData<TenantCard>("TenantCard");
                                DBcontext.InsertBulkData<TenantCard>(result);
                                Logs.WriteLog("sync tenant card is succeed ");
                            }
                        }
                        
                    }
                    //
                }
                else
                {
                    {
                        //app log
                        var datas = from x in DBcontext.GetAllData<AppLog>()
                                    select x;
                        if (datas.Count() > 0)
                        {
                            var stringContent = new StringContent(JsonConvert.SerializeObject(datas.ToList()), Encoding.UTF8, "application/json");
                            var response = await client.PostAsync(GlobalVars.Config.ServiceHost + "api/AppLogs/PushAppLogs", stringContent);
                            if (!response.IsSuccessStatusCode)
                            {
                                Logs.WriteLog("sync applogs is failed : " + response.StatusCode);

                            }
                            else
                            {
                                var res = DBcontext.DeleteAllData<AppLog>("AppLog");
                                Logs.WriteLog("sync app logs is succeed ");
                            }

                        }
                    }
                    {
                        //sync log

                        var datas = from x in DBcontext.GetAllData<SyncLog>()
                                    select x;
                        if (datas.Count() > 0)
                        {
                            var stringContent = new StringContent(JsonConvert.SerializeObject(datas.ToList()), Encoding.UTF8, "application/json");
                            var response = await client.PostAsync(GlobalVars.Config.ServiceHost + "api/SyncLogs/PushSyncLogs", stringContent);
                            if (!response.IsSuccessStatusCode)
                            {
                                Logs.WriteLog("sync sync logs is failed : " + response.StatusCode);

                            }
                            else
                            {
                                var res = DBcontext.DeleteAllData<AppLog>("SyncLog");
                                Logs.WriteLog("sync sync logs is succeed ");
                            }

                        }
                    }
                }
                
                return true;
            }
            catch(Exception ex)
            {
                Logs.WriteLog("Sync Failed :" + ex.Message);
                return false;
            }
           
        }

        public async Task<Device> GetConfigFromServer(string DeviceNo)
        {

            try
            {
                var hasil = false;
                if (string.IsNullOrEmpty(GlobalVars.Config.AccessToken))
                {
                    hasil = await ServiceManagement.GetAccessToken();
                }
                if (!hasil) return null;

                // call api
                var client = new HttpClient();
                client.SetBearerToken(GlobalVars.Config.AccessToken);

                var response = await client.GetAsync(GlobalVars.Config.ServiceHost + "api/Devices/GetDeviceByNo?DeviceNo="+DeviceNo);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine(response.StatusCode);
                    return null;
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<OutputData>(content);
                    if (data.Data != null)
                    {
                        var res = ((JArray)data.Data).ToObject<List<Device>>();
                        return res != null && res.Count > 0 ? res[0] : null;
                    }
                }

            }
            catch (Exception x)
            {
                Console.WriteLine(x.StackTrace);
            }
            return null;

        }

    }
}
