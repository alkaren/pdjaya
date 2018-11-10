using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PDJaya.Kiosk.Helpers;

namespace PDJaya.Kiosk
{
    public partial class FormGagalMasukByID : Form
    {
        Button BtnKembali;
        public FormGagalMasukByID()
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
            BtnKembali = new Button();
            StyleHelper.SetButtonStyle(BtnKembali);
            PictureBox PicBox = new PictureBox();
            PicBox.Dock = DockStyle.Fill;
            PicBox.SizeMode = PictureBoxSizeMode.StretchImage;
            PicBox.Image = Properties.Resources.layar_login_nok;
            PicBox.Controls.Add(BtnKembali);

            Controls.Add(PicBox);

            this.TopMost = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            this.SizeChanged += Form_SizeChanged;
            BtnKembali.Tag = 2;
            BtnKembali.Click += BtnKembali_Click;
        }

        private void BtnKembali_Click(Object sender, EventArgs e)
        {
            var newfrm = new FormMasuk();
            newfrm.Show();
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

            BtnKembali.Left = Convert.ToInt32(90 * ratioW);
            BtnKembali.Top = Convert.ToInt32(1650 * ratioH);

            BtnKembali.Width = Convert.ToInt32(1240 * ratioW);
            BtnKembali.Height = Convert.ToInt32(220 * ratioH);
        }
    }
}
