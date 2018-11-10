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
    public partial class FormHapusKartu : Form
    {

        Button BtnHapusKartu;
        Button BtnKembali;
        Button BtnNext;
        Button BtnPrev;
        Label LblPrimaryCard;
        List<TenantCard> Cards = new List<TenantCard>();
        Button[] BtnPilih = new Button[2];
        public int CurrentIndex { get; set; }
        public static TenantCard DelCard { get; set; }
        public static string PrimaryCard { get; set; }

        public FormHapusKartu()
        {
            ActiveFormInfo();
            InitializeComponent();
            Configure();
            LoadCards();
            LoadPrimaryCard();
            LblPrimaryCard.Text = PrimaryCard;
        }


        //Show Current Active Form
        private void ActiveFormInfo()
        {
            GlobalVars.CurrentForm = this.GetType().ToString();
            Console.WriteLine(GlobalVars.CurrentForm);
        }

        void LoadPrimaryCard()
        {
            //Read By CardType
            //for (int i = 0; i < Cards.Count; i++)
            //{
            //    if (Cards[i].CardType == "1")
            //    {
            //        PrimaryCard = Cards[i].CardID;
            //    }
            //}
            PrimaryCard = Cards[0].CardID;
        }

        void LoadCards()
        {
            Cards = CardManager.GetUserCards(GlobalVars.CurrentCard.StoreNo);
            CurrentIndex = 0;
            if (Cards != null)
            {
                DisplayCards();
            }
        }

        void DisplayCards()
        {
            //Clear Button Text
            for (int i = 0; i < 2; i++)
            {
                BtnPilih[i].Text = "";
            }

            var Startidx = (CurrentIndex * 2);

            for (int i = 0; i < 2; i++)
            {
                if ((Startidx + i) == Cards.Count-1) break;
                BtnPilih[i].Text = Cards[Startidx + i+1].CardID + " - " + Cards[Startidx + i+1].StoreNo;
            }
        }

        void Configure()
        {
            //layout

            LblPrimaryCard = new Label();
            StyleHelper.SetLabelStyle(LblPrimaryCard);

            for (int i = 0; i < 2; i++)
            {
                BtnPilih[i] = new Button();
                StyleHelper.SetButtonStyle(BtnPilih[i]);
                BtnPilih[i].ForeColor = Color.Black;
                BtnPilih[i].Font = new Font("Arial", 18);
            }

            BtnHapusKartu = new Button();
            BtnKembali = new Button();
            BtnPrev = new Button();
            BtnNext = new Button();

            StyleHelper.SetButtonStyle(BtnHapusKartu);
            StyleHelper.SetButtonStyle(BtnKembali);

            BtnNext.Text = "Next";
            BtnPrev.Text = "Preview";
            //StyleHelper.SetButtonStyle(BtnNext);
            //StyleHelper.SetButtonStyle(BtnPrev);

            PictureBox picBox = new PictureBox();
            picBox.Dock = DockStyle.Fill;
            picBox.SizeMode = PictureBoxSizeMode.StretchImage;
            picBox.Image = Properties.Resources.layar_kartu_hapus;
            picBox.BorderStyle = BorderStyle.None;
            picBox.Controls.Add(BtnHapusKartu);
            picBox.Controls.Add(BtnKembali);
            picBox.Controls.Add(BtnNext);
            picBox.Controls.Add(BtnPrev);
            picBox.Controls.Add(LblPrimaryCard);

            for (int i = 0; i < 2; i++)
            {
                picBox.Controls.Add(BtnPilih[i]);
            }

            Controls.Add(picBox);

            //this.TopMost = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            this.SizeChanged += Form_SizeChanged;
          
       
            BtnHapusKartu.Click += BtnHapus_Click;
            BtnKembali.Click += BtnKembali_Click;
            BtnPrev.Click += BtnPrev_Click;
            BtnNext.Click += BtnNext_Click;

            for (int i = 0; i < 2; i++)
            {
                BtnPilih[i].Click += BtnPilih_Click;
                BtnPilih[i].Tag = i;
            }
            // Start the timer
        }

        void BtnPilih_Click(object sender, EventArgs e)
        {
            var btnSel = sender as Button;
            var idx = (CurrentIndex * 2) + Convert.ToInt32(btnSel.Tag);
            if (idx < 0 || idx >= Cards.Count) return;
            DelCard = Cards[idx+1];

            //styling button
            for (int i = 0; i < 2; i++)
            {
                BtnPilih[i].ForeColor = Color.Black;
            }
            ((Control)sender).ForeColor = Color.Red;
        }


        private void BtnKembali_Click(object sender, EventArgs e)
        {
            var newFrm = new FormPengaturanKartu();
            newFrm.Show();
            this.Close();
        }

        private void BtnPrev_Click(object sender, EventArgs e)
        {
            var max = (Cards.Count / 2) + (Cards.Count % 2 > 0 ? 1 : 0);
            CurrentIndex--;
            if (CurrentIndex < 0) CurrentIndex = max - 1;
            DisplayCards();
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            var max = (Cards.Count / 2) + (Cards.Count % 2 > 0 ? 1 : 0);
            CurrentIndex++;
            if (CurrentIndex >= max) CurrentIndex = 0;
            DisplayCards();
        }

        private async void BtnHapus_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Anda akan Menghapus kartu", "Alert", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                var response = await CardManager.DeleteCard(DelCard);
                if (response)
                {
                    var newFrm = new FormHapusKartuBerhasil(DelCard.CardID);
                    newFrm.Show();
                    this.Close();
                }
                else
                {
                    var newFrm = new FormHapusKartuGagal(DelCard.CardID);
                    newFrm.Show();
                    this.Close();
                }
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

            LblPrimaryCard.Left = Convert.ToInt32(300 * ratioW);
            LblPrimaryCard.Top = Convert.ToInt32(740 * ratioH);
            LblPrimaryCard.Width = Convert.ToInt32(500 * ratioW);
            LblPrimaryCard.Height = Convert.ToInt32(100 * ratioH);

            BtnHapusKartu.Left = Convert.ToInt32(750 * ratioW);
            BtnHapusKartu.Top = Convert.ToInt32(1650 * ratioH);
            BtnHapusKartu.Width = Convert.ToInt32(585 * ratioW);
            BtnHapusKartu.Height = Convert.ToInt32(173 * ratioH);

            BtnKembali.Left = Convert.ToInt32(150 * ratioW);
            BtnKembali.Top = Convert.ToInt32(1650 * ratioH);
            BtnKembali.Width = Convert.ToInt32(585 * ratioW);
            BtnKembali.Height = Convert.ToInt32(173 * ratioH);

            BtnNext.Left = Convert.ToInt32(730 * ratioW);
            BtnNext.Top = Convert.ToInt32(1450 * ratioH);
            BtnNext.Width = Convert.ToInt32(583 * ratioW);
            BtnNext.Height = Convert.ToInt32(100 * ratioH);

            BtnPrev.Left = Convert.ToInt32(130 * ratioW);
            BtnPrev.Top = Convert.ToInt32(1450 * ratioH);
            BtnPrev.Width = Convert.ToInt32(584 * ratioW);
            BtnPrev.Height = Convert.ToInt32(100 * ratioH);


            for (int i = 0; i < 2; i++)
            {
                BtnPilih[i].Left = Convert.ToInt32(100 * ratioW);
                BtnPilih[i].Top = Convert.ToInt32((950 + (200 * i)) * ratioH);

                BtnPilih[i].Width = Convert.ToInt32(1169 * ratioW);
                BtnPilih[i].Height = Convert.ToInt32(177 * ratioH);

            }
        }
    }
}
