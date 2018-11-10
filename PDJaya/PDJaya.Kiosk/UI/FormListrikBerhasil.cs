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
    public partial class FormListrikBerhasil : Form
    {
        Button BtnKembali;
        Label LblAmount;
        string Amount = "";

        public FormListrikBerhasil(Payment info)
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
            LblAmount = new Label();
            StyleHelper.SetLabelStyle(LblAmount);
            LblAmount.Text = Amount;

            StyleHelper.SetButtonStyle(BtnKembali);

            PictureBox picBox = new PictureBox();
            picBox.Dock = DockStyle.Fill;
            picBox.SizeMode = PictureBoxSizeMode.StretchImage;
            picBox.Image = Properties.Resources.layar_listrik_ok;
            picBox.BorderStyle = BorderStyle.None;

            picBox.Controls.Add(BtnKembali);
            picBox.Controls.Add(LblAmount);
            Controls.Add(picBox);

            //this.TopMost = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            this.SizeChanged += Form_SizeChanged;

            BtnKembali.Click += BtnKembali_Click;
            // Start the timer
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

           

            BtnKembali.Left = Convert.ToInt32(130 * ratioW);
            BtnKembali.Top = Convert.ToInt32(1650 * ratioH);

            BtnKembali.Width = Convert.ToInt32(1179 * ratioW);
            BtnKembali.Height = Convert.ToInt32(187 * ratioH);

            LblAmount.Left = Convert.ToInt32(650 * ratioW);
            LblAmount.Top = Convert.ToInt32(1185 * ratioH);

            LblAmount.Width = Convert.ToInt32(643 * ratioW);
            LblAmount.Height = Convert.ToInt32(80 * ratioH);
        }
    }
}
