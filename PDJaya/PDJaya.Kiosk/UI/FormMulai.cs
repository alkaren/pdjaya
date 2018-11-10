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
    public partial class FormMulai : Form
    {
        Button BtnMulai;
        public FormMulai()
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
            BtnMulai = new Button();
          
            StyleHelper.SetButtonStyle(BtnMulai);
        

            PictureBox picBox = new PictureBox();
            picBox.Dock = DockStyle.Fill;
            picBox.SizeMode = PictureBoxSizeMode.StretchImage;
            picBox.Image = Properties.Resources.layar_awal;
            picBox.BorderStyle = BorderStyle.None;
            picBox.Controls.Add(BtnMulai);
     

            Controls.Add(picBox);

            //this.TopMost = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            this.SizeChanged += PilihModa_SizeChanged; ;
            BtnMulai.Tag = 2;
            BtnMulai.Click += BtnMulai_Click;
        }

        private void BtnMulai_Click(object sender, EventArgs e)
        {
            var newFrm = new FormMasuk();
            newFrm.Show();
            this.Close();
        }        

        private void PilihModa_SizeChanged(object sender, EventArgs e)
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

          
            BtnMulai.Width = Convert.ToInt32(488 * ratioW);
            BtnMulai.Height = Convert.ToInt32(1235 * ratioH);
            BtnMulai.Left = Convert.ToInt32(513 * ratioW);
            BtnMulai.Top = Convert.ToInt32(189 * ratioH);
            //BtnAngkot.Foreground = new SolidColorBrush(Colors.Black);
            //BtnAngkot.Opacity = 0;

        }
    }
}
