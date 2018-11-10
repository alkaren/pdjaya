using PDJaya.Kiosk;
using PDJaya.Kiosk.Helpers;
using PDJaya.Kiosk.Logic;
using PDJaya.Models;
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
    public partial class FormTambahKartu : Form
    {
        Timer cardTimer = new Timer();
        CardReader reader;
        Button BtnKembali;

        public FormTambahKartu()
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
            picBox.Image = Properties.Resources.layar_kartu_add;
            picBox.BorderStyle = BorderStyle.None;
            picBox.Controls.Add(BtnKembali);

            Controls.Add(picBox);

            //this.TopMost = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            this.SizeChanged += Form_SizeChanged;
         
            BtnKembali.Click += BtnKembali_Click;
            // Start the timer
            if (reader == null)
            {
                reader = ObjectContainer.Get<CardReader>();
            }

            cardTimer.Tick += new EventHandler(timer_Tick); // Everytime timer ticks, timer_Tick will be called
            cardTimer.Interval = 2000;              // Timer will tick evert second
            cardTimer.Enabled = true;                       // Enable the timer
            cardTimer.Start();                              // Start the timer
        }

        async void timer_Tick(object sender, EventArgs e)
        {
            cardTimer.Stop();
            var info = reader.ReadCardData();
            if (info.StatusCode == GlobalVars.SUCCESS_CODE)
            {
                var cardno = info.CardNo;
                var newCard = new TenantCard() { CardID="C"+shortid.ShortId.Generate(9), CardNo=cardno, CardType="1", Created=DateTime.Now, CreatedBy= GlobalVars.CurrentTenant.Owner, Status=CardStatus.Active, StoreNo= GlobalVars.CurrentTenant.StoreNo, Updated=DateTime.Now, UpdatedBy= GlobalVars.CurrentTenant.Owner };
                if (!string.IsNullOrEmpty(cardno))
                {
                    var hasil = await CardManager.AddNewCard(newCard);
                    if (hasil)
                    {
                        MethodInvoker methodInvokerDelegate = delegate ()
                        {
                            var newFrm = new FormBerhasilTambahKartu(newCard.CardID);
                            newFrm.Show();
                            this.Close();
                        };

                        //This will be true if Current thread is not UI thread.
                        if (this.InvokeRequired)
                            this.Invoke(methodInvokerDelegate);
                        else
                        {
                            var newFrm = new FormBerhasilTambahKartu(newCard.CardID);
                            newFrm.Show();
                            this.Close();

                        }

                    }
                    else
                    {
                        MethodInvoker methodInvokerDelegate = delegate ()
                        {
                            var newFrm = new FormGagalTambahKartu();
                            newFrm.Show();
                            this.Close();
                        };

                        //This will be true if Current thread is not UI thread.
                        if (this.InvokeRequired)
                            this.Invoke(methodInvokerDelegate);
                        else
                        {
                            var newFrm = new FormGagalTambahKartu();
                            newFrm.Show();
                            this.Close();

                        }
                    }
                }
            }
            else
            {
                cardTimer.Start();
            }
        }


        private void BtnKembali_Click(object sender, EventArgs e)
        {
            cardTimer.Stop();
            var newFrm = new FormPengaturanKartu();
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
            BtnKembali.Height = Convert.ToInt32(172 * ratioH);
        }
    }
}
