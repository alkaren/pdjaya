using PDJaya.Kiosk.Helpers;
using PDJaya.Models;
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
    public partial class FormRetribusiGagal : Form
    {
        Button BtnKembali;
        Button BtnUlang;

        Label LblAmount;
        string Amount = "";

        public FormRetribusiGagal(Payment info, string Message)
        {
            ActiveFormInfo();
            InitializeComponent();
            this.Amount = info.Amount.ToString("C2");
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
            BtnUlang = new Button();

            LblAmount = new Label();
            StyleHelper.SetLabelStyle(LblAmount);
            LblAmount.Text = Amount;

            StyleHelper.SetButtonStyle(BtnKembali);
            StyleHelper.SetButtonStyle(BtnUlang);

            PictureBox picBox = new PictureBox();
            picBox.Dock = DockStyle.Fill;
            picBox.SizeMode = PictureBoxSizeMode.StretchImage;
            picBox.Image = Properties.Resources.layar_retribusi_nomoney;
            picBox.BorderStyle = BorderStyle.None;

            picBox.Controls.Add(BtnKembali);
            picBox.Controls.Add(BtnUlang);
            picBox.Controls.Add(LblAmount);
            Controls.Add(picBox);

            //this.TopMost = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            this.SizeChanged += Form_SizeChanged;
            BtnUlang.Click += BtnUlang_Click;
            BtnKembali.Click += BtnKembali_Click;
            // Start the timer
        }

        private void BtnUlang_Click(object sender, EventArgs e)
        {
            var newFrm = new FormTagihanRetribusi();
            newFrm.Show();
            this.Close();
        }

        private void BtnKembali_Click(object sender, EventArgs e)
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

            BtnUlang.Left = Convert.ToInt32(730 * ratioW);
            BtnUlang.Top = Convert.ToInt32(1600 * ratioH);
            BtnUlang.Width = Convert.ToInt32(600 * ratioW);
            BtnUlang.Height = Convert.ToInt32(200 * ratioH);

            BtnKembali.Left = Convert.ToInt32(130 * ratioW);
            BtnKembali.Top = Convert.ToInt32(1600 * ratioH);
            BtnKembali.Width = Convert.ToInt32(600 * ratioW);
            BtnKembali.Height = Convert.ToInt32(200 * ratioH);

            LblAmount.Left = Convert.ToInt32(650 * ratioW);
            LblAmount.Top = Convert.ToInt32(1170 * ratioH);
            LblAmount.Width = Convert.ToInt32(643 * ratioW);
            LblAmount.Height = Convert.ToInt32(80 * ratioH);
        }
    }
}
