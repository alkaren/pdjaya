using PDJaya.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDJaya.Kiosk.Helpers
{
   
    public static class GlobalVars
    {
        public const string SUCCESS_CODE = "000000";
        public static ExtendedConfig Config = new ExtendedConfig ();
        public static Tenant CurrentTenant { get; set; }
        public static TenantCard CurrentCard { get; set; }
        public static string CurrentForm { get; set; }

    }
    public class ExtendedConfig : Device
    {
        public TimeSpan AutoCloseTimeout { get; set; } = new TimeSpan(0,0,60);
        public TimeSpan SyncTime { get; set; } = new TimeSpan(23, 59, 00);

    }
    public enum ResultTypes
    {
        CardUnrecognized, NotEnoughBalance, CannotPrintTicket,
        Succeed, RePrint, Others, ReprintFailed, Reprintsuccess, AlreadyPrinted, ShelterTimeOut
    }
}
