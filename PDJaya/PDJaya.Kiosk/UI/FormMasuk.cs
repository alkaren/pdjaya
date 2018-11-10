using PDJaya.Kiosk.Helpers;
using STI.CardReader;
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
    public partial class FormMasuk : Form
    {
        Timer cardTimer = new Timer();
        Button BtnSubmit;
        TextBox TxtNoKiosk;
        CardReader reader;
        public FormMasuk()
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
            BtnSubmit = new Button();
            TxtNoKiosk = new TextBox();
            TxtNoKiosk.Name = "TxtNoKiosk";
            StyleHelper.SetButtonStyle(BtnSubmit);
            PictureBox picBox = new PictureBox();
            picBox.Dock = DockStyle.Fill;
            picBox.SizeMode = PictureBoxSizeMode.StretchImage;
            picBox.Image = Properties.Resources.layar_login;
            picBox.BorderStyle = BorderStyle.None;
            picBox.Controls.Add(BtnSubmit);
            picBox.Controls.Add(TxtNoKiosk);
            
            Controls.Add(picBox);

            this.TopMost = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            this.SizeChanged += Form_SizeChanged; 
            BtnSubmit.Tag = 2;
            BtnSubmit.Click += BtnSubmit_Click;

            if (reader == null)
            {
                reader = ObjectContainer.Get<CardReader>();
            }

            cardTimer.Tick += new EventHandler(timer_Tick); // Everytime timer ticks, timer_Tick will be called
            cardTimer.Interval = 2000;              // Timer will tick evert second
            cardTimer.Enabled = true;                       // Enable the timer
            cardTimer.Start();                              // Start the timer
        }

        void timer_Tick(object sender, EventArgs e)
        {
            cardTimer.Stop();
            var info = reader.ReadCardData();
            if (info.StatusCode==GlobalVars.SUCCESS_CODE)
            {
                var cardno = info.CardNo;
                var StoreNo = Logic.Login.GetStoreNoByCardNo(cardno, out bool MoreThanOne);
                var hasil = Logic.Login.CheckStoreNoIsValid(StoreNo);
                var card = Logic.Login.GetCardbyCardNo(cardno);
                if (hasil != null && card != null)
                {
                    GlobalVars.CurrentTenant = hasil;
                    GlobalVars.CurrentCard = card;
                    GotoMenu(MoreThanOne);
                }
                else
                {
                    var newfrm = new FormGagalMasukByCard();
                    newfrm.Show();
                    this.Close();
                }
            }
            else
            {
                cardTimer.Start();
            }
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TxtNoKiosk.Text))
            {
                var StoreNo = TxtNoKiosk.Text.Trim();
                var hasil = Logic.Login.CheckStoreNoIsValid(StoreNo);
              
                if (hasil != null)
                {
                    GlobalVars.CurrentTenant = hasil;
                    GlobalVars.CurrentCard = null;
                    GotoMenu(false);
                }
                else
                {
                    var newfrm = new FormGagalMasukByID();
                    newfrm.Show();
                    this.Close();
                }
            }
           
        }

        void GotoMenu(bool IsMoreThanOneStore)
        {
            MethodInvoker methodInvokerDelegate = delegate ()
            {
                ContinueToNextForm(IsMoreThanOneStore);
            };

            //This will be true if Current thread is not UI thread.
            if (this.InvokeRequired)
                this.Invoke(methodInvokerDelegate);
            else
                ContinueToNextForm(IsMoreThanOneStore);
        }

        void ContinueToNextForm(bool IsMoreThanOneStore)
        {
            if (!IsMoreThanOneStore)
            {
                var newFrm = new FormBerhasilMasuk();
                newFrm.Show();
            }
            else
            {
                var newFrm = new FormPilihKios(GlobalVars.CurrentCard.CardNo);
                newFrm.Show();
            }
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

            BtnSubmit.Left = Convert.ToInt32(905 * ratioW);
            BtnSubmit.Top = Convert.ToInt32(1760 * ratioH);

            BtnSubmit.Width = Convert.ToInt32(476 * ratioW);
            BtnSubmit.Height = Convert.ToInt32(142 * ratioH);

            //BtnAngkot.Foreground = new SolidColorBrush(Colors.Black);
            //BtnAngkot.Opacity = 0;
            TxtNoKiosk.Left = Convert.ToInt32(74 * ratioW);
            TxtNoKiosk.Top = Convert.ToInt32(1416 * ratioH);

            TxtNoKiosk.Width = Convert.ToInt32(1305 * ratioW);
            TxtNoKiosk.Height = Convert.ToInt32(140 * ratioH);
        }
    }
}
