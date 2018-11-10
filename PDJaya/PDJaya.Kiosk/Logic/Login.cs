using PDJaya.Kiosk.Helpers;
using PDJaya.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDJaya.Kiosk.Logic
{
    public class Login
    {
      public static string GetStoreNoByCardNo(string CardNo,out bool IsMoreThanOne)
        {
            var datas = from x in Program.DBcontext.GetAllData<TenantCard>()
                        where x.CardNo==CardNo
                        select x;
            if (datas.Count() > 1) IsMoreThanOne = true;
            else
                IsMoreThanOne = false;
            foreach(var item in datas)
            {
                return item.StoreNo;
            }
            return null;
        }
        public static TenantCard GetCardbyCardNo(string CardNo)
        {
            var datas = from x in Program.DBcontext.GetAllData<TenantCard>()
                        where x.CardNo == CardNo 
                        select x;
            foreach (var item in datas)
            {
                return item;
            }
            return null;
        }
        public static Tenant CheckStoreNoIsValid(string StoreNo)
        {
            var datas = from x in Program.DBcontext.GetAllData<Tenant>()
                        where x.StoreNo == StoreNo && x.MarketNo == GlobalVars.Config.MarketNo
                        select x;
            foreach (var item in datas)
            {
                return item;
            }
            return null;
        }
    }
}
