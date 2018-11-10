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
    public partial class FormPengaturanKartu : Form
    {
        Button BtnTambahKartu;
        Button BtnHapuKartu;
        Button BtnKembali;

        public FormPengaturanKartu()
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
            BtnTambahKartu = new Button();
            BtnHapuKartu = new Button();
            BtnKembali = new Button();

            StyleHelper.SetButtonStyle(BtnTambahKartu);
            StyleHelper.SetButtonStyle(BtnHapuKartu);
            StyleHelper.SetButtonStyle(BtnKembali);

            PictureBox picBox = new PictureBox();
            picBox.Dock = DockStyle.Fill;
            picBox.SizeMode = PictureBoxSizeMode.StretchImage;
            picBox.Image = Properties.Resources.layar_kartu;
            picBox.BorderStyle = BorderStyle.None;
            picBox.Controls.Add(BtnTambahKartu);
            picBox.Controls.Add(BtnHapuKartu);
            picBox.Controls.Add(BtnKembali);

            Controls.Add(picBox);

            //this.TopMost = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            this.SizeChanged += Form_SizeChanged;
            BtnTambahKartu.Tag = 2;
            BtnTambahKartu.Click += BtnTambah_Click;
            BtnHapuKartu.Tag = 1;
            BtnHapuKartu.Click += BtnHapus_Click;
            BtnKembali.Click += BtnKembali_Click;
            // Start the timer
        }

        private void BtnKembali_Click(object sender, EventArgs e)
        {
            var newFrm = new FormMenu();
            newFrm.Show();
            this.Close();
        }

        private void BtnHapus_Click(object sender, EventArgs e)
        {
            var newFrm = new FormHapusKartu();
            newFrm.Show();
            this.Close();
        }

        private void BtnTambah_Click(object sender, EventArgs e)
        {
            var newFrm = new FormTambahKartu();
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

            BtnTambahKartu.Left = Convert.ToInt32(90 * ratioW);
            BtnTambahKartu.Top = Convert.ToInt32(666 * ratioH);

            BtnTambahKartu.Width = Convert.ToInt32(1357 * ratioW);
            BtnTambahKartu.Height = Convert.ToInt32(358 * ratioH);

            BtnHapuKartu.Left = Convert.ToInt32(90 * ratioW);
            BtnHapuKartu.Top = Convert.ToInt32(1050 * ratioH);

            BtnHapuKartu.Width = Convert.ToInt32(1357 * ratioW);
            BtnHapuKartu.Height = Convert.ToInt32(358 * ratioH);

            BtnKembali.Left = Convert.ToInt32(90 * ratioW);
            BtnKembali.Top = Convert.ToInt32(1650 * ratioH);
            
            BtnKembali.Width = Convert.ToInt32(1179 * ratioW);
            BtnKembali.Height = Convert.ToInt32(174 * ratioH);
        }
    }
}
