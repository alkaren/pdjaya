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

namespace PDJaya.Kiosk
{
    public partial class FormMenu : Form
    {
        Button BtnPembayaran;
        Button BtnPengaturanKartu;
        Button BtnKeluar;

        public FormMenu()
        {
            ActiveFormInfo();
            InitializeComponent();
            Configure();
        }

        //Show Current Active Form
        private void ActiveFormInfo()
        {
            GlobalVars.CurrentForm = this.GetType().ToString();
            Console.WriteLine(GlobalVars.CurrentForm);
        }


        void Configure()
        {
            //layout
            BtnPembayaran = new Button();
            BtnPengaturanKartu = new Button();
            BtnKeluar = new Button();

            StyleHelper.SetButtonStyle(BtnPembayaran);
            StyleHelper.SetButtonStyle(BtnPengaturanKartu);
            StyleHelper.SetButtonStyle(BtnKeluar);

            PictureBox picBox = new PictureBox();
            picBox.Dock = DockStyle.Fill;
            picBox.SizeMode = PictureBoxSizeMode.StretchImage;
            picBox.Image = Properties.Resources.layar_menu;
            picBox.BorderStyle = BorderStyle.None;
            picBox.Controls.Add(BtnPembayaran);
            picBox.Controls.Add(BtnPengaturanKartu);
            picBox.Controls.Add(BtnKeluar);


            Controls.Add(picBox);

            //this.TopMost = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            this.SizeChanged += Form_SizeChanged;
            BtnPembayaran.Tag = 2;
            BtnPembayaran.Click += BtnPembayaran_Click;
            BtnPengaturanKartu.Tag = 1;
            BtnPengaturanKartu.Click += BtnPengaturanKartu_Click;
            BtnKeluar.Click += BtnKeluar_Click;
            // Start the timer
        }

        private void BtnKeluar_Click(object sender, EventArgs e)
        {
            var newFrm = new FormMulai();
            newFrm.Show();
            GlobalVars.CurrentCard = null;
            GlobalVars.CurrentTenant = null;
            this.Close();
        }

        private void BtnPengaturanKartu_Click(object sender, EventArgs e)
        {
            var newFrm = new FormPengaturanKartu();
            newFrm.Show();
            this.Close();
        }

        private void BtnPembayaran_Click(object sender, EventArgs e)
        {
            var newFrm = new FormMenuPembayaran();
            newFrm.Show();
            this.Close();
        }
        
        
        private void Form_SizeChanged(object sender, EventArgs e)
        {
            var frm = sender as Form;
            ResizeComponent(frm.Width, frm.Height);
        }

        void ResizeComponent(double newWidth, double newHeight)
        {
            //Canvas1.Width = 963;
            //Canvas1.Height = 1278;
            double ratioW = newWidth / GlobalVars.Config.DesignWidth;
            //newHeight = newWidth * 1278 / 963;
            double ratioH = newHeight / GlobalVars.Config.DesignHeight;

            BtnPembayaran.Left = Convert.ToInt32(125 * ratioW);
            BtnPembayaran.Top = Convert.ToInt32(750 * ratioH);

            BtnPembayaran.Width = Convert.ToInt32(1179 * ratioW);
            BtnPembayaran.Height = Convert.ToInt32(357 * ratioH);

            BtnPengaturanKartu.Left = Convert.ToInt32(125 * ratioW);
            BtnPengaturanKartu.Top = Convert.ToInt32(1130 * ratioH);

            BtnPengaturanKartu.Width = Convert.ToInt32(1179 * ratioW);
            BtnPengaturanKartu.Height = Convert.ToInt32(357 * ratioH);

            BtnKeluar.Left = Convert.ToInt32(130 * ratioW);
            BtnKeluar.Top = Convert.ToInt32(1650 * ratioH);

            BtnKeluar.Width = Convert.ToInt32(1179 * ratioW);
            BtnKeluar.Height = Convert.ToInt32(187 * ratioH);
        }
    }
}
