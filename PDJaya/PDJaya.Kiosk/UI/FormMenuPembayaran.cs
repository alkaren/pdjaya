using PDJaya.Kiosk.Helpers;
using PDJaya.Kiosk.Logic;
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
    public partial class FormMenuPembayaran : Form
    {
        Button BtnRetribusi;
        Button BtnListrik;
        Button BtnBack;

        public FormMenuPembayaran()
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
            BtnRetribusi = new Button();
            BtnListrik = new Button();
            BtnBack = new Button();

            StyleHelper.SetButtonStyle(BtnRetribusi);
            StyleHelper.SetButtonStyle(BtnListrik);
            StyleHelper.SetButtonStyle(BtnBack);

            PictureBox picBox = new PictureBox();
            picBox.Dock = DockStyle.Fill;
            picBox.SizeMode = PictureBoxSizeMode.StretchImage;
            picBox.Image = Properties.Resources.layar_pembayaran;
            picBox.BorderStyle = BorderStyle.None;
            picBox.Controls.Add(BtnRetribusi);
            picBox.Controls.Add(BtnListrik);
            picBox.Controls.Add(BtnBack);


            Controls.Add(picBox);

            this.TopMost = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            this.SizeChanged += Form_SizeChanged;
            BtnRetribusi.Tag = 2;
            BtnRetribusi.Click += BtnRetribusi_Click;
            BtnListrik.Tag = 1;
            BtnListrik.Click += BtnListrik_Click;
            BtnBack.Click += BtnBack_Click;
            // Start the timer
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            var newFrm = new FormMenu();
            newFrm.Show();
            this.Close();
        }

        private void BtnListrik_Click(object sender, EventArgs e)
        {
            var DataTagihan =  ListrikManager.GetBills();
            if (DataTagihan != null)
            {
                var newFrm = new FormTagihanListrik();
                newFrm.Show();
                this.Close();
            }
            else
            {
                var res = MessageBox.Show("Tidak ada tagihan saat ini, kembali ke form pembayaran.");

            }
        }

        private  void BtnRetribusi_Click(object sender, EventArgs e)
        {
            var DataTagihan =  RetribusiManager.GetBills();
            if (DataTagihan != null)
            {
                var newFrm = new FormTagihanRetribusi();
                newFrm.Show();
                this.Close();
            }
            else
            {
                var res = MessageBox.Show("Tidak ada tagihan saat ini, kembali ke form pembayaran.");
               
            }
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

            BtnRetribusi.Left = Convert.ToInt32(125 * ratioW);
            BtnRetribusi.Top = Convert.ToInt32(750 * ratioH);

            BtnRetribusi.Width = Convert.ToInt32(1180 * ratioW);
            BtnRetribusi.Height = Convert.ToInt32(354 * ratioH);

            BtnListrik.Left = Convert.ToInt32(125 * ratioW);
            BtnListrik.Top = Convert.ToInt32(1130 * ratioH);

            BtnListrik.Width = Convert.ToInt32(1180 * ratioW);
            BtnListrik.Height = Convert.ToInt32(354 * ratioH);

            BtnBack.Left = Convert.ToInt32(130 * ratioW);
            BtnBack.Top = Convert.ToInt32(1650 * ratioH);

            BtnBack.Width = Convert.ToInt32(1179 * ratioW);
            BtnBack.Height = Convert.ToInt32(187 * ratioH);

        }
    }
}
