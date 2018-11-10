using PDJaya.Kiosk.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDJaya.Kiosk.UI
{
    public partial class FormKonfigurasi : Form
    {
        ConfigHelper config;
        public FormKonfigurasi()
        {
            ActiveFormInfo();
            InitializeComponent();

            config = new ConfigHelper();
            BtnConfig.Click += BtnConfig_Click;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
        }

        //Show Current Active Form
        private void ActiveFormInfo()
        {
            GlobalVars.CurrentForm = this.GetType().ToString();
            Console.WriteLine(GlobalVars.CurrentForm);
        }

        private async void BtnConfig_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty( TxtDeviceNo.Text))
            {
                var DeviceNo = TxtDeviceNo.Text;
                Console.WriteLine("Try to call service from server..");
                var hasil = await config.GetConfigFromServer(DeviceNo);
                if (!hasil)
                {
                    System.Windows.Forms.MessageBox.Show("Data konfigurasi Kiosk tidak ditemukan di server, silakan kontak admin.", "Warning");
                    Application.Exit();
                }
                else
                {
                    Console.WriteLine("Configuration has been retrieved from server successfully, please start the app again.");
                    this.Close();
                    Application.Exit();
                    /*
                    if (Program.Configure().GetAwaiter().GetResult())
                    {
                        FormMulai frm = new FormMulai();
                        frm.Show();
                        this.Close();
                    }
                    else
                    {
                        Console.WriteLine("Gagal sync data ke server, cek koneksi dan hubungi admin.");
                        Application.Exit();
                    }*/

                }
            }
        }
    }
}
