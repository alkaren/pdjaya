using PDJaya.Kiosk.Helpers;
using PDJaya.Kiosk.UI;
using STI.CardReader;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using PDJaya.Tools;

namespace PDJaya.Kiosk
{
    static class Program
    {
        static System.Timers.Timer aTimer;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new FormTagihanRetribusi());

            //check internet connection
            /*
            if (!Helpers.InternetTool.IsConnectedToInternet())
            {
                Console.WriteLine("Periksa koneksi internet Anda");
                return;
            }
            //sync time with ntp
            InternetTool.SyncTime();
            */
            if (!Helpers.ConfigHelper.CheckConfigIsExists())
            {
                var main_form = new FormKonfigurasi();
                main_form.Show();
            }
            else
            {
                var config = new ConfigHelper();
                config.ReadConfigFromFile();
                if (Configure().GetAwaiter().GetResult())
                {
                    var main_form = new FormMulai();
                    main_form.Show();
                 
                }
                else
                {
                    Console.WriteLine("Gagal sync data ke server, cek koneksi dan hubungi admin.");
                    Application.Exit();
                }
            }
            Application.Run();

           
        }

        public static PDJayaDBSqlite DBcontext { get; set; }
        public static async Task<bool> Configure()
        {
            try
            {
                //set interval in minutes format
                SetInterval(5);

                //TicketPrinter.Printv2();
                if (DBcontext == null)
                {
                    DBcontext = new PDJayaDBSqlite();

                }
                PDJayaSync sync = new PDJayaSync();
                var res = await sync.RunSync(SyncMode.Pull);
                
                CardReader Reader = new CardReader();
                ObjectContainer.Register<CardReader>(Reader);

              

                Reader.Connect(ConfigurationManager.AppSettings["ComPort"]);
                Reader.InitReader();
                return res;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Configure error : " + ex.Message);
                Logs.WriteLog(ex.Message+"_"+ex.StackTrace);
                return false;
            }
        }

        //set interval
        private static void SetInterval(double minutes)
        {
            double countdown = TimeSpan.FromMinutes(minutes).TotalMilliseconds;
            aTimer = new System.Timers.Timer(countdown);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.Enabled = true;
        }
        //sync to database every countdown
        private static async void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            PDJayaSync sync = new PDJayaSync();
            var res = await sync.RunSync(SyncMode.Pull);
            if (res)
            {
                Console.WriteLine("Auto Sync Success");
            }
            else
            {
                Console.WriteLine("Auto Sync Failed");
            }
        }
    }
}
