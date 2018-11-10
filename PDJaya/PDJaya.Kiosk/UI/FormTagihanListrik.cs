using PDJaya.Kiosk.Helpers;
using PDJaya.Kiosk.Logic;
using PDJaya.Kiosk.UI;
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
    public partial class FormTagihanListrik : Form
    {
        //declare item layout
        Timer cardTimer = new Timer();
        CardReader reader;
        Label IDKiosLbl = new Label();
        Label NamaLbl = new Label();
        Label PeriodeLbl = new Label();
        Label TagihanLbl = new Label();

        Button BtnKembali = new Button();
        Button BtnNext = new Button();
        Button BtnPrev = new Button();

        PictureBox picBox = new PictureBox();

        public List<Bill> DataTagihan { get; set; }
        public int CurrentIndex { get; set; }

        bool IsReadyToPay = false;
        public FormTagihanListrik()
        {
            ActiveFormInfo();
            InitializeComponent();
            Configure();
            LoadBills();
        }

        //Show Current Active Form
        private void ActiveFormInfo()
        {
            GlobalVars.CurrentForm = this.GetType().ToString();
            Console.WriteLine(GlobalVars.CurrentForm);
        }

        async void LoadBills()
        {
            DataTagihan =  await ListrikManager.GetBills();
            if (DataTagihan == null)
            {
                MessageBox.Show("Tidak ada tagihan saat ini, kembali ke form pembayaran.");
                GoBack();
            }
            else
            {
                CurrentIndex = 0;
                ShowBillInfo();
                IsReadyToPay = true;
            }
        }



        void ShowBillInfo()
        {
            var item = DataTagihan[CurrentIndex];
            IDKiosLbl.Text = item.StoreNo;
            NamaLbl.Text = GlobalVars.CurrentTenant.Owner;
            PeriodeLbl.Text = $"{ CalendarTool.GetMonthName(item.Month)} {item.Year}";
            TagihanLbl.Text = $"{item.Amount.ToString("C2")}";
        }

        void Configure()
        {
            //layout
            
            StyleHelper.SetLabelStyle(IDKiosLbl);
            StyleHelper.SetLabelStyle(NamaLbl);
            StyleHelper.SetLabelStyle(PeriodeLbl);
            StyleHelper.SetLabelStyle(TagihanLbl);
            
            StyleHelper.SetButtonStyle(BtnKembali);
            StyleHelper.SetButtonStyle(BtnNext);
            StyleHelper.SetButtonStyle(BtnPrev);

            picBox.Dock = DockStyle.Fill;
            picBox.SizeMode = PictureBoxSizeMode.StretchImage;
            picBox.Image = Properties.Resources.layar_listrik;
            picBox.BorderStyle = BorderStyle.None;
            picBox.Controls.Add(BtnKembali);
            picBox.Controls.Add(BtnNext);
            picBox.Controls.Add(BtnPrev);

            picBox.Controls.Add(IDKiosLbl);
            picBox.Controls.Add(NamaLbl);
            picBox.Controls.Add(PeriodeLbl);
            picBox.Controls.Add(TagihanLbl);

            Controls.Add(picBox);

            //this.TopMost = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            this.SizeChanged += Form_SizeChanged;

            BtnKembali.Click += BtnKembali_Click;
            BtnNext.Click += BtnNext_Click;
            BtnPrev.Click += BtnPrev_Click;
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

        private void BtnPrev_Click(object sender, EventArgs e)
        {
            CurrentIndex--;
            if (CurrentIndex < 0) CurrentIndex = DataTagihan.Count - 1;
            ShowBillInfo();
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            CurrentIndex++;
            if (CurrentIndex >= DataTagihan.Count) CurrentIndex = 0;
            ShowBillInfo();
        }

        async void timer_Tick(object sender, EventArgs e)
        {
            cardTimer.Stop();
            var info = reader.ReadCardData();
            if (info.StatusCode == GlobalVars.SUCCESS_CODE)
            {
                var cardno = info.CardNo;
                if (!string.IsNullOrEmpty(cardno) && IsReadyToPay)
                {
                    var Tagihan = DataTagihan[CurrentIndex];

                    var hasil = reader.Deduct((int)Tagihan.Amount);

                    var payment = new Payment() { Amount = Tagihan.Amount, CardNo = cardno, MarketNo = GlobalVars.CurrentTenant.MarketNo, PaymentDate = DateTime.Now, Status = PaymentStatus.Success, StoreNo = GlobalVars.CurrentTenant.StoreNo, TransactionCode = "2", TransactionID = Tagihan.BillID, TransactionName = $"Listrik - {Tagihan.Month} - {Tagihan.Year}" };
                    Console.WriteLine("transaksi :"+hasil.StatusCode +", balance :"+hasil.Balance);
                    if (hasil.StatusCode==GlobalVars.SUCCESS_CODE) //jika berhasil deduct
                    {
                        var res =  await ListrikManager.PushPayment(new List<Payment> { payment });
                        if (res)
                        {
                            MethodInvoker methodInvokerDelegate = delegate ()
                            {
                                payment.Status = PaymentStatus.Success;
                                GotoBerhasilForm(payment);
                            };

                            //This will be true if Current thread is not UI thread.
                            if (this.InvokeRequired)
                                this.Invoke(methodInvokerDelegate);
                            else
                            {
                                payment.Status = PaymentStatus.Success;
                                GotoBerhasilForm(payment);
                            }
                               
                        }
                        else
                        {
                            MethodInvoker methodInvokerDelegate = delegate ()
                            {
                                payment.Status = PaymentStatus.Failed;
                                GotoFailPage(payment, "Gagal push data ke server");
                            };

                            //This will be true if Current thread is not UI thread.
                            if (this.InvokeRequired)
                                this.Invoke(methodInvokerDelegate);
                            else {
                                payment.Status = PaymentStatus.Failed;
                                GotoFailPage(payment, "Gagal push data ke server");
                            }

                        }

                    }
                    else
                    {
                        MethodInvoker methodInvokerDelegate = delegate ()
                        {
                            payment.Status = PaymentStatus.Failed;
                            GotoFailPage(payment, "saldo tidak mencukupi");
                        };

                        //This will be true if Current thread is not UI thread.
                        if (this.InvokeRequired)
                            this.Invoke(methodInvokerDelegate);
                        else
                        {
                            payment.Status = PaymentStatus.Failed;
                            GotoFailPage(payment, "saldo tidak mencukupi");
                        }
                    }
                }
            }
            else
            {
                cardTimer.Start();
            }
        }

    
        void GotoBerhasilForm(Payment payment)
        {
            var newFrm = new FormListrikBerhasil(payment);
            newFrm.Show();
            this.Close();
        }
        void GotoFailPage(Payment info, string Message)
        {
            var newFrm = new FormListrikGagal(info, Message);
            newFrm.Show();
            this.Close();
        }

        void GoBack()
        {
            cardTimer.Stop();
            var newFrm = new FormMenuPembayaran();
            newFrm.Show();
            this.Close();
        }

        private void BtnKembali_Click(object sender, EventArgs e)
        {
            GoBack();
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

            //Button Coordinat
            BtnKembali.Left = Convert.ToInt32(130 * ratioW);
            BtnKembali.Top = Convert.ToInt32(1650 * ratioH);
            BtnKembali.Width = Convert.ToInt32(1179 * ratioW);
            BtnKembali.Height = Convert.ToInt32(187 * ratioH);

            BtnNext.Left = Convert.ToInt32(730 * ratioW);
            BtnNext.Top = Convert.ToInt32(1350 * ratioH);
            BtnNext.Width = Convert.ToInt32(600 * ratioW);
            BtnNext.Height = Convert.ToInt32(200 * ratioH);

            BtnPrev.Left = Convert.ToInt32(130 * ratioW);
            BtnPrev.Top = Convert.ToInt32(1350 * ratioH);
            BtnPrev.Width = Convert.ToInt32(600 * ratioW);
            BtnPrev.Height = Convert.ToInt32(200 * ratioH);

            //Label Coordinat
            IDKiosLbl.Left = Convert.ToInt32(200 * ratioW);
            IDKiosLbl.Top = Convert.ToInt32(870 * ratioH);
            IDKiosLbl.Width = Convert.ToInt32(400 * ratioW);
            IDKiosLbl.Height = Convert.ToInt32(90 * ratioH);

            TagihanLbl.Left = Convert.ToInt32(200 * ratioW);
            TagihanLbl.Top = Convert.ToInt32(1070 * ratioH);
            TagihanLbl.Width = Convert.ToInt32(400 * ratioW);
            TagihanLbl.Height = Convert.ToInt32(90 * ratioH);

            PeriodeLbl.Left = Convert.ToInt32(730 * ratioW);
            PeriodeLbl.Top = Convert.ToInt32(1070 * ratioH);
            PeriodeLbl.Width = Convert.ToInt32(400 * ratioW);
            PeriodeLbl.Height = Convert.ToInt32(90 * ratioH);

            NamaLbl.Left = Convert.ToInt32(730 * ratioW);
            NamaLbl.Top = Convert.ToInt32(870 * ratioH);
            NamaLbl.Width = Convert.ToInt32(400 * ratioW);
            NamaLbl.Height = Convert.ToInt32(90 * ratioH);
        }
    }
}
