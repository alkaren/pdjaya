using PDJaya.Models;
using PDJaya.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDJaya.Kiosk.Logic
{
    public class TenantManager
    {
        public static List<Tenant> GetStoreByCardNo(string CardNo)
        {
            try
            {
                HashSet<string> Stores = new HashSet<string>();
                var StoreNos = (from x in Program.DBcontext.GetAllData<TenantCard>()
                                where x.CardNo == CardNo
                                select x.StoreNo).ToList();
                if (StoreNos != null)
                {
                    StoreNos.ForEach(x => Stores.Add(x));
                }
                var TenantList = from x in Program.DBcontext.GetAllData<Tenant>()
                                 where Stores.Contains(x.StoreNo)
                                 select x;
                return TenantList.ToList();
            }
            catch(Exception ex)
            {
                Logs.WriteLog("get store from card no error :"+ ex.Message);
                return null;
            }

        }
    }
}
