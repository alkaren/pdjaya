
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
    public partial class FormPilihKioskBerhasil : Form
    {
        Button BtnKembali;
        Button BtnLanjutkan;

        Label LblKioskNo;
        string KioskNo = "";

        public FormPilihKioskBerhasil()
        {
            ActiveFormInfo();
            InitializeComponent();
            this.KioskNo = GlobalVars.CurrentTenant.StoreNo + " - " + GlobalVars.CurrentTenant.Remark;
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

            BtnKembali = new Button();
            BtnLanjutkan = new Button();
            LblKioskNo = new Label();
            StyleHelper.SetLabelStyle(LblKioskNo);
            LblKioskNo.Text = KioskNo;

            StyleHelper.SetButtonStyle(BtnKembali);
            StyleHelper.SetButtonStyle(BtnLanjutkan);

            PictureBox picBox = new PictureBox();
            picBox.Dock = DockStyle.Fill;
            picBox.SizeMode = PictureBoxSizeMode.StretchImage;
            picBox.Image = Properties.Resources.layar_pilihkios_ok;
            picBox.BorderStyle = BorderStyle.None;

            picBox.Controls.Add(BtnKembali);
            picBox.Controls.Add(BtnLanjutkan);
            picBox.Controls.Add(LblKioskNo);


            Controls.Add(picBox);

            //this.TopMost = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            this.SizeChanged += Form_SizeChanged;
            BtnLanjutkan.Click += BtnLanjutkan_Click;
            BtnKembali.Click += BtnKembali_Click;
            // Start the timer
        }

        private void BtnLanjutkan_Click(object sender, EventArgs e)
        {
            var newFrm = new FormMenu();
            newFrm.Show();
            this.Close();
        }

        private void BtnKembali_Click(object sender, EventArgs e)
        {
            var newFrm = new FormPilihKios(GlobalVars.CurrentCard.CardNo);
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


            BtnKembali.Left = Convert.ToInt32(150 * ratioW);
            BtnKembali.Top = Convert.ToInt32(1650 * ratioH);

            BtnKembali.Width = Convert.ToInt32(583 * ratioW);
            BtnKembali.Height = Convert.ToInt32(168 * ratioH);

            BtnLanjutkan.Left = Convert.ToInt32(689 * ratioW);
            BtnLanjutkan.Top = Convert.ToInt32(1650* ratioH);
            
            BtnLanjutkan.Width = Convert.ToInt32(600 * ratioW);
            BtnLanjutkan.Height = Convert.ToInt32(169 * ratioH);

            LblKioskNo.Left = Convert.ToInt32(600 * ratioW);
            LblKioskNo.Top = Convert.ToInt32(1300 * ratioH);

            LblKioskNo.Width = Convert.ToInt32(500 * ratioW);
            LblKioskNo.Height = Convert.ToInt32(110 * ratioH);
        }
    }
}
