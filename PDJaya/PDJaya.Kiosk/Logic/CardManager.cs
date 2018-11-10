using Newtonsoft.Json;
using PDJaya.Kiosk.Helpers;
using PDJaya.Models;
using PDJaya.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PDJaya.Kiosk.Logic
{
    public class CardManager
    {
        public static async Task<bool> AddNewCard(TenantCard card)
        {
            try
            {
                var hasil = false;
                if (string.IsNullOrEmpty(GlobalVars.Config.AccessToken))
                {
                    hasil = await ServiceManagement.GetAccessToken();
                }
                else
                {
                    //always already have token
                    hasil = true;
                }
                if (!hasil) return false;
                var client = new HttpClient();
                client.SetBearerToken(GlobalVars.Config.AccessToken);
                var stringContent = new StringContent(JsonConvert.SerializeObject(card), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(GlobalVars.Config.ServiceHost + "api/TenantCards", stringContent);
                if (!response.IsSuccessStatusCode == true)
                {
                    Logs.WriteLog("add tenant card failed: " + response.StatusCode);
                }
                else
                {
                    var res = Program.DBcontext.InsertData<TenantCard>(card);
                    Logs.WriteLog("add tenant card : " + res);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logs.WriteLog("Add tenant card failed :" + ex.Message);
            }
            return false;
        }

        public static List<TenantCard> GetUserCards(string StoreNo)
        {
            try
            {
                var datas = from x in Program.DBcontext.GetAllData<TenantCard>()
                            where x.StoreNo == StoreNo
                            select x;
                return datas != null ? datas.ToList() : null;
            }
            catch(Exception ex)
            {
                Logs.WriteLog("Error get user cards " + ex.Message);
                return null;
            }

        }

        public static async Task<bool> DeleteCard(TenantCard card)
        {
            try
            {
                var hasil = false;
                if (string.IsNullOrEmpty(GlobalVars.Config.AccessToken))
                {
                    hasil = await ServiceManagement.GetAccessToken();
                }
                else
                {
                    //always already have token
                    hasil = true;
                }
                if (!hasil) return false;
                var client = new HttpClient();
                client.SetBearerToken(GlobalVars.Config.AccessToken);
                var stringContent = new StringContent(JsonConvert.SerializeObject((card)), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(GlobalVars.Config.ServiceHost + "api/TenantCards/DeleteCard", stringContent);
                if (!response.IsSuccessStatusCode == true)
                {
                    Logs.WriteLog("delete tenant card failed: " + response.StatusCode);
                }
                else
                {
                    var res = Program.DBcontext.DeleteData<TenantCard>("TenantCard",card.Id);
                    Logs.WriteLog("delete tenant card : " + res);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Logs.WriteLog("Delete tenant card failed :" + ex.Message);
            }
            return false;
        }
    }
}
