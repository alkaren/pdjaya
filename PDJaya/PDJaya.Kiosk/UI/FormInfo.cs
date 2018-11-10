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

namespace PDJaya.Kiosk.UI
{
    public partial class FormInfo : Form
    {
        Button BtnBack;
        Timer timer = new Timer();
        public FormInfo(TransactionResult Result)
        {
            ActiveFormInfo();
            InitializeComponent();
            Configure(Result);

            timer.Interval = (int)GlobalVars.Config.AutoCloseTimeout.TotalMilliseconds; // 5 secs
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        //Show Current Active Form
        private void ActiveFormInfo()
        {
            GlobalVars.CurrentForm = this.GetType().ToString();
            Console.WriteLine(GlobalVars.CurrentForm);
        }
        public void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            MethodInvoker methodInvokerDelegate = delegate ()
            {
                CloseForm();
            };

            //This will be true if Current thread is not UI thread.
            if (this.InvokeRequired)
                this.Invoke(methodInvokerDelegate);
            else
            {
                CloseForm();
            }
        }

        void CloseForm()
        {
            var newFrm = new FormMulai();
            newFrm.Show();
            this.Close();
        }
        void Configure(TransactionResult res)
        {
            BtnBack = new Button();
            StyleHelper.SetButtonStyle(BtnBack);

            PictureBox picBox = new PictureBox();
            picBox.Dock = DockStyle.Fill;
            picBox.SizeMode = PictureBoxSizeMode.StretchImage;

            Console.WriteLine("Load halaman akhir");

            Bitmap selForm;
            switch (res.Output)
            {
                case Helpers.ResultTypes.CannotPrintTicket:
                    //selForm = Properties.Resources.layar_9;
                    //break;
                case Helpers.ResultTypes.CardUnrecognized:
                    //selForm = Properties.Resources.layar_7;
                    //break;
                case Helpers.ResultTypes.NotEnoughBalance:
                    //selForm = Properties.Resources.layar_8;
                    //break;
                case Helpers.ResultTypes.RePrint:
                    //selForm = Properties.Resources.layar_5;
                    //break;
                case Helpers.ResultTypes.Succeed:
                    //selForm = Properties.Resources.layar_6;
                    //break;
                case Helpers.ResultTypes.ReprintFailed:
                    //selForm = Properties.Resources.layar_11;
                    //break;
                case Helpers.ResultTypes.Reprintsuccess:
                    //selForm = Properties.Resources.layar_12;
                    //break;
                case Helpers.ResultTypes.AlreadyPrinted:
                case Helpers.ResultTypes.ShelterTimeOut:
                    //selForm = Properties.Resources.layar_13;
                    //break;
                default:
                    selForm = Properties.Resources.layar_awal;
                    break;

            }
            picBox.Image = selForm;

            //BtnBack.Text = "Click me!!";
            picBox.BorderStyle = BorderStyle.None;
            picBox.Controls.Add(BtnBack);

            Controls.Add(picBox);

            //this.TopMost = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            if (!string.IsNullOrEmpty(res.Message))
            {
                Console.WriteLine(res.Message);
                //var dialog = new MessageDialog(res.Message);
                //await dialog.ShowAsync();
            }
            Console.WriteLine("Selesai load halaman akhir");

            this.SizeChanged += PilihModa_SizeChanged;
            BtnBack.Click += BtnBack_Click;
            //this.Focus();
            //this.Refresh();
        }
        private void BtnBack_Click(object sender, EventArgs e)
        {
            var newFrm = new FormMulai();
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
            double ratioW = newWidth / GlobalVars.Config.DesignWidth;
            double ratioH = newHeight / GlobalVars.Config.DesignHeight;

            BtnBack.Width = Convert.ToInt32(1187 * ratioW);
            BtnBack.Height = Convert.ToInt32(187 * ratioH);
            BtnBack.Left = Convert.ToInt32(1270 * ratioW);
            BtnBack.Top = Convert.ToInt32(1752 * ratioH);
            //BtnBack.Foreground = new SolidColorBrush(Colors.Black);
            //BtnBack.Opacity = 0;


        }
    }
}
