

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
    public partial class FormGagalTambahKartu : Form
    {
        Button BtnKembali;
      

        public FormGagalTambahKartu()
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

            BtnKembali = new Button();
           

            StyleHelper.SetButtonStyle(BtnKembali);

            PictureBox picBox = new PictureBox();
            picBox.Dock = DockStyle.Fill;
            picBox.SizeMode = PictureBoxSizeMode.StretchImage;
            picBox.Image = Properties.Resources.layar_kartu_add_error;
            picBox.BorderStyle = BorderStyle.None;

            picBox.Controls.Add(BtnKembali);
       
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


            BtnKembali.Left = Convert.ToInt32(106 * ratioW);
            BtnKembali.Top = Convert.ToInt32(1574 * ratioH);

            BtnKembali.Width = Convert.ToInt32(1177 * ratioW);
            BtnKembali.Height = Convert.ToInt32(174 * ratioH);

        }
    }
}