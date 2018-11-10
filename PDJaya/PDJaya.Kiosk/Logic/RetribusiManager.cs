using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PDJaya.Kiosk.Helpers;
using PDJaya.Models;
using PDJaya.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDJaya.Kiosk.Logic
{
    public class RetribusiManager
    {
        public static async Task<List<Bill>> GetBills(string TransCode="1")
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
                if (!hasil) return null;
                var client = new HttpClient();
                client.SetBearerToken(GlobalVars.Config.AccessToken);
                var response = await client.GetAsync(GlobalVars.Config.ServiceHost + $"api/Bills/GetBillByTransactionCode?CurrentDate={DateTime.Now.ToString("yyyy-MM-dd")}&TransactionCode={TransCode}&StoreNo={GlobalVars.CurrentTenant.StoreNo}");
                if (!response.IsSuccessStatusCode)
                {
                    Logs.WriteLog("get bill retribusi failed: " + response.StatusCode);
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var output = JsonConvert.DeserializeObject<OutputData>(content);
                    if (output != null && output.IsSucceed)
                    {
                        var result = ((JArray)output.Data).ToObject<List<Bill>>();
                        //count list of result because data can never be truly null value
                        if (!result.Count().Equals(0))
                        {
                            Logs.WriteLog("get bill retribusi : success");
                            return result;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logs.WriteLog("get bill retribusi failed :" + ex.Message);
            }
            return null;
        }

        public static async Task<bool> PushPayment(List<Payment> Payments,string TransCode = "1")
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
                var stringContent = new StringContent(JsonConvert.SerializeObject(Payments), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(GlobalVars.Config.ServiceHost + "api/Payments/PushPaymentData", stringContent);

                if (!response.IsSuccessStatusCode)
                {
                    Logs.WriteLog("push payment retribusi failed: " + response.StatusCode);
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var output = JsonConvert.DeserializeObject<OutputData>(content);
                    if (output != null && output.IsSucceed)
                    {
                        Logs.WriteLog("push payment retribusi : success");
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logs.WriteLog("push payment retribusi failed :" + ex.Message);
            }
            return false;
        }
    }
}
