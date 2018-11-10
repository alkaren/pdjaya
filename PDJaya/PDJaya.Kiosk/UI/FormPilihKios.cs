using PDJaya.Kiosk.Helpers;
using PDJaya.Kiosk.Logic;
using PDJaya.Kiosk.UI;
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

namespace PDJaya.Kiosk
{
    public partial class FormPilihKios : Form
    {
        public string CardNo { get; set; }
        List<Tenant> Stores = new List<Tenant>();
        public int CurrentIndex { get; set; }
        Button[] BtnPilih = new Button[4];
        Button BtnKeluar;
        Button BtnNext;
        Button BtnPrev;

        public FormPilihKios(string CardNo)
        {
            ActiveFormInfo();
            InitializeComponent();
            Configure();
            this.CardNo = CardNo;
            LoadKiosk();
        }

        //Show Current Active Form
        private void ActiveFormInfo()
        {
            GlobalVars.CurrentForm = this.GetType().ToString();
            Console.WriteLine(GlobalVars.CurrentForm);
        }

        void LoadKiosk()
        {
            Stores = TenantManager.GetStoreByCardNo(this.CardNo);
            CurrentIndex = 0;
            if (Stores != null)
            {
                DisplayKiosk();
            }
        }

        void DisplayKiosk()
        {
            //Clear Button Text
            for (int i = 0; i < 4; i++)
            {
                BtnPilih[i].Text = "";
            }

            var StartIdx = (CurrentIndex * 4);

            for (int i = 0; i < 4; i++)
            {
                if ((StartIdx + i) == Stores.Count) break;
                BtnPilih[i].Text = Stores[StartIdx + i].StoreNo + " - " + Stores[StartIdx + i].Remark;
            }
        }

        void Configure()
        {
            //layout
            for (int i = 0; i < 4; i++)
            {
                BtnPilih[i] = new Button();
                StyleHelper.SetButtonStyle(BtnPilih[i]);
                BtnPilih[i].ForeColor = Color.Black;
                BtnPilih[i].Font = new Font("Arial", 18);
            }
            BtnPrev = new Button();
            BtnNext = new Button();
            BtnKeluar = new Button();

            StyleHelper.SetButtonStyle(BtnNext);
            StyleHelper.SetButtonStyle(BtnPrev);
            StyleHelper.SetButtonStyle(BtnKeluar);

            PictureBox picBox = new PictureBox();
            picBox.Dock = DockStyle.Fill;
            picBox.SizeMode = PictureBoxSizeMode.StretchImage;
            picBox.Image = Properties.Resources.layar_pilihkios;
            picBox.BorderStyle = BorderStyle.None;
            picBox.Controls.Add(BtnNext);
            picBox.Controls.Add(BtnPrev);
            picBox.Controls.Add(BtnKeluar);
            for (int i = 0; i < 4; i++)
            {
                picBox.Controls.Add(BtnPilih[i]);
            }


            Controls.Add(picBox);

            //this.TopMost = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            this.SizeChanged += Form_SizeChanged;
            BtnPrev.Click += BtnPrev_Click;
            BtnNext.Click += BtnNext_Click;
            BtnKeluar.Click += BtnKeluar_Click;
            for (int i = 0; i < 4; i++)
            {
                BtnPilih[i].Click += BtnPilih_Click;
                BtnPilih[i].Tag = i;
            }
            // Start the timer
        }

        private void BtnPilih_Click(object sender, EventArgs e)
        {
            var btnSel = sender as Button;
            var idx = (CurrentIndex*4)+ Convert.ToInt32(btnSel.Tag);
            if (idx < 0 || idx >= Stores.Count) return;
            GlobalVars.CurrentTenant = Stores[idx];
            var newFrm = new FormPilihKioskBerhasil();
            newFrm.Show();
          
            this.Close();

        }

        private void BtnPrev_Click(object sender, EventArgs e)
        {
            var max = (Stores.Count / 4) + (Stores.Count%4 > 0 ? 1 : 0);
            CurrentIndex--;
            if (CurrentIndex < 0) CurrentIndex = max-1;
            DisplayKiosk();
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            var max = (Stores.Count / 4) + (Stores.Count % 4 > 0 ? 1 : 0);
            CurrentIndex++;
            if (CurrentIndex >= max) CurrentIndex = 0;
            DisplayKiosk();
        }
        private void BtnKeluar_Click(object sender, EventArgs e)
        {
            var newFrm = new FormMulai();
            newFrm.Show();
            GlobalVars.CurrentCard = null;
            GlobalVars.CurrentTenant = null;
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

            BtnNext.Left = Convert.ToInt32(730 * ratioW);
            BtnNext.Top = Convert.ToInt32(1384 * ratioH);

            BtnNext.Width = Convert.ToInt32(583 * ratioW);
            BtnNext.Height = Convert.ToInt32(178 * ratioH);

            BtnPrev.Left = Convert.ToInt32(130 * ratioW);
            BtnPrev.Top = Convert.ToInt32(1384 * ratioH);

            BtnPrev.Width = Convert.ToInt32(584 * ratioW);
            BtnPrev.Height = Convert.ToInt32(180 * ratioH);

            BtnKeluar.Left = Convert.ToInt32(140 * ratioW);
            BtnKeluar.Top = Convert.ToInt32(1667 * ratioH);

            BtnKeluar.Width = Convert.ToInt32(1168 * ratioW);
            BtnKeluar.Height = Convert.ToInt32(173 * ratioH);

            for (int i = 0; i < 4; i++)
            {
                BtnPilih[i].Left = Convert.ToInt32(100 * ratioW);
                BtnPilih[i].Top = Convert.ToInt32((590+(177 * i)) * ratioH);

                BtnPilih[i].Width = Convert.ToInt32(1169 * ratioW);
                BtnPilih[i].Height = Convert.ToInt32(177 * ratioH);

            }
        }
    }
}
