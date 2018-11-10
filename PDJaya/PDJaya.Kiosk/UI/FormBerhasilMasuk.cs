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
    public partial class FormBerhasilMasuk : Form
    {
        Button BtnLanjut;
        Label LbNoKiosk;
        public FormBerhasilMasuk()
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
            BtnLanjut = new Button();
            BtnLanjut.Dock = DockStyle.Fill;
            LbNoKiosk = new Label();
            LbNoKiosk.Name = "LbNoKiosK";
            LbNoKiosk.Text = GlobalVars.CurrentTenant.Id.ToString();
            StyleHelper.SetLabelStyle(LbNoKiosk);


            StyleHelper.SetButtonStyle(BtnLanjut);
            PictureBox PicBox = new PictureBox();
            PicBox.Dock = DockStyle.Fill;
            PicBox.SizeMode = PictureBoxSizeMode.StretchImage;
            PicBox.Image = Properties.Resources.layar_login_ok;
            //PicBox.Controls.Add(BtnLanjut);
            PicBox.Controls.Add(LbNoKiosk);
            PicBox.Controls.Add(BtnLanjut);

            Controls.Add(PicBox);

            this.TopMost = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            this.SizeChanged += Form_SizeChanged;
            BtnLanjut.Tag = 2;
            BtnLanjut.Click += BtnLanjut_Click;
        }

        private void BtnLanjut_Click(object sender, EventArgs e)
        {
            var newfrm = new FormMenu();
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

            //BtnAngkot.Foreground = new SolidColorBrush(Colors.Black);
            //BtnAngkot.Opacity = 0;
            LbNoKiosk.Left = Convert.ToInt32(670 * ratioW);
            LbNoKiosk.Top = Convert.ToInt32(1160 * ratioH);

            LbNoKiosk.Width = Convert.ToInt32(350 * ratioW);
            LbNoKiosk.Height = Convert.ToInt32(140 * ratioH);
        }
    }
}
