//using SerialPortLib;
using Comfile.ComfilePi;
//using MonoSerialPort;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace STI.CardReader
{
    public class CardReader
    {
        public bool IsMono = false;
        static GlobalFunction fungsi = new GlobalFunction();
        string STX = "02";
        string InitKey = "";
        string ReaderTimeout = "0002";    //2detik

        string CmdReaderInit = "EF0101";
        string CmdReaderCheckBalance = "EF0102";
        string CmdReaderDeduct = "EF0103";
        string CmdReaderCancelDeductCorrection = "EF0104";
        string CmdReaderGetLastTransaction = "EF0105";

        string CmdReadData = "EF010800";
        string CmdWriteData = "EF010801";
        string SlotData = "01";
        int LengthCardData = 16;

        string CmdGetUIDMifare = "EF010700";
        string CmdReadMifare = "EF010701";
        string CmdWriteMifare = "EF010702";
        string KeyType = "0A";

        string CmdSetDisplayBuzzer = "EF0109";

        string DataPrepare = "", LRC = "";
        static string TempData = "", DataComplete = "";
        static bool DataReceiveComplete = false;
        int Timeout = 5;

        private byte[] CommandToReader;
        public bool IsConnected { get; set; } = false;
        public bool IsInit { get; set; } = false;
        string PortName { get; set; }
        int discarded;
        public static LinuxSerialPort ComPort = null; //= new SerialPort();
        //const int blockLimit = 1024;

        public CardReader(bool IsMono = false)
        {
            this.IsMono = IsMono;
            if (ComPort == null)
            {
                //ComPort = new SerialPortInput();
                //if (!IsMono)
                //{
                //ComPort.MessageReceived += ComPort_MessageReceived;//ComPort_DataReceived;
                //ComPort.ConnectionStatusChanged += ComPort_ConnectionStatusChanged;
                //}

            }


            //btnConnect.Enabled = true;
            //btnDisconnect.Enabled = false;


            GetKeyReaderINIT();
            WriteLog("STI Reader Library v" + Assembly.GetEntryAssembly().GetName().Version + " (" + InitKey + ")");
            //this.Text = "Sample App Parking Reader Demo " + Application.ProductVersion + " (" + InitKey + ")";

        }
        
        /*
        private void ComPort_MessageReceived(object sender, MessageReceivedEventArgs args)
        {
            byte[] data = args.Data; //new byte[args.Data.Length];
            //ComPort.Read(data, 0, data.Length);

            InputData = fungsi.ToString(data);
            TempData += InputData;
            if (TempData.Length >= 6)
            {
                int lengthData = int.Parse(TempData.Substring(2, 4), System.Globalization.NumberStyles.HexNumber) * 2;
                if (TempData.Length == lengthData + 8)
                {
                    DataComplete = TempData;
                    DataReceiveComplete = true;
                }
            }
        }
        */
        void WriteLog(string Message)
        {
            Console.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " -> " + Message);
        }

        private void GetKeyReaderINIT()
        {
            InitKey = ConfigurationManager.AppSettings["KeyReaderInit"];
            /*
            string sourceFile = Application.StartupPath + "\\KEYREADERINIT.txt";
            if (File.Exists(sourceFile))
            {
                InitKey = File.ReadAllText(sourceFile, Encoding.UTF8);
            }*/
        }

        public bool Connect(string PortName = "COM3")
        {
            this.PortName = PortName;

            ComPort = new LinuxSerialPort();
            ComPort.ReadBufferSize = 1024;
            ComPort.WriteBufferSize = 1024;
            ComPort.DataReceived += ComPort_DataReceived;
            //+= ComPort_MessageReceived;
            //ComPort.SetPort(this.PortName, 38400, System.IO.Ports.StopBits.One, System.IO.Ports.Parity.None);
            //ComPort.SetPort(this.PortName, 38400, MonoSerialPort.Port.Handshake.None);
           

            
            ComPort.BaudRate = 38400;
            ComPort.DataBits = 8;
            ComPort.Parity = Parity.None;
            ComPort.StopBits = StopBits.One;
            
            /*
            ComPort.ReadBufferSize = 4096;
            //ComPort.ParityReplace = 63;
            ComPort.ReadTimeout = -1;
            //ComPort.ReceivedBytesThreshold = 1;
            ComPort.RtsEnable = false;
            ComPort.WriteBufferSize = 2048;
            ComPort.WriteTimeout = -1;
            ComPort.Handshake = Handshake.None;
            ComPort.DtrEnable = false;
            //ComPort.DiscardNull = false;
            
            //ComPort.PortName = PortName;
            //btnDisconnect.Enabled = true;
            */
            if (OpenCommunication())
            {
                /*
                if (IsMono)
                {
                    Thread readThread = new Thread(new ThreadStart(ReadThread));
                    readThread.Start();
                }*/
                /*
                if (IsMono)
                {
                    byte[] buffer = new byte[blockLimit];
                    Action kickoffRead = null;
                    kickoffRead = delegate {
                        ComPort.BaseStream.BeginRead(buffer, 0, buffer.Length, delegate (IAsyncResult ar) {
                            try
                            {
                                int actualLength = ComPort.BaseStream.EndRead(ar);
                                byte[] received = new byte[actualLength];
                                Buffer.BlockCopy(buffer, 0, received, 0, actualLength);
                                raiseAppSerialDataEvent(received);
                            }
                            catch (Exception exc)
                            {
                                Console.WriteLine("error read:" + exc.Message);
                                //handleAppSerialError(exc);
                            }
                            kickoffRead();
                        }, null);
                    };
                    kickoffRead();
                }*/
                WriteLog("Open Com Serial Success");
                IsConnected = true;
                //btnConnect.Enabled = false;
                return true;
            }
            else
            {
                //btnDisconnect.Enabled = false;
                WriteLog("Open Com Serial Failed");
                IsConnected = false;
                //btnConnect.Enabled = true;
                return false;
            }
        }
        /*
        private void ComPort_ConnectionStatusChanged(object sender, ConnectionStatusChangedEventArgs args)
        {
            Console.WriteLine($"Status : {args.Connected}");
        }
        */
        /*
private void ReadThread()
{
   SerialPort sp = new SerialPort(this.PortName, 38400, Parity.None, 8, StopBits.One);
   List<byte> datas = new List<byte>();
   sp.Open();

   if (sp.IsOpen)
       Console.WriteLine("ReadThread(), sp Opened.");
   else
       Console.WriteLine("ReadThread(), sp is not open.");

   while (true)
   {
       Thread.Sleep(200);
       byte tmpByte;

       tmpByte = (byte)sp.ReadByte();

       while (tmpByte != 255)
       {
           datas.Add(tmpByte);
           tmpByte = (byte)sp.ReadByte();
       }

       raiseAppSerialDataEvent(datas.ToArray());
       datas.Clear();

   }
}*/
        public void Disconnect()
        {
            ClosedCommunication();
            //btnDisconnect.Enabled = false;
            WriteLog("Closed Com Serial Success");
            IsConnected = false;
            IsInit = false;
            //btnConnect.Enabled = true;
        }

        public void InitReader()
        {
            //SetButton(false);
            string Data = CmdReaderInit + InitKey;
            DataPrepare = (Data.Length / 2).ToString("X").PadLeft(4, '0') + Data;
            LRC = fungsi.calculateLRC(DataPrepare);

            CommandToReader = fungsi.GetBytes(STX + DataPrepare + LRC, out discarded);
            SendCommandToSerial(CommandToReader);

            DateTime start = DateTime.Now;
            do
            {
                if (DataReceiveComplete)
                {
                    if (fungsi.cekLRC(DataComplete.Substring(2, DataComplete.Length - 4), DataComplete.Substring(DataComplete.Length - 2, 2)))
                    {
                        WriteLog(DataComplete);
                        WriteLog("Status Code : " + DataComplete.Substring(8, 6));
                        WriteLog("MID : " + DataComplete.Substring(14, 16));
                        WriteLog("TID : " + DataComplete.Substring(30, 8));
                    }
                    break;
                }
            } while (start.AddSeconds(Timeout) > DateTime.Now);
            //SetButton(true);
            IsInit = true;
        }

        public CheckBalanceInfo CheckBalance()
        {
            if (!IsConnected || !IsInit) return null;
            CheckBalanceInfo info = new CheckBalanceInfo();
            //SetButton(false);
            string Data = CmdReaderCheckBalance + DateTime.Now.ToString("ddMMyyyyHHmmss") + ReaderTimeout;
            DataPrepare = (Data.Length / 2).ToString("X").PadLeft(4, '0') + Data;
            LRC = fungsi.calculateLRC(DataPrepare);

            CommandToReader = fungsi.GetBytes(STX + DataPrepare + LRC, out discarded);
            SendCommandToSerial(CommandToReader);

            DateTime start = DateTime.Now;
            do
            {
                if (DataReceiveComplete)
                {
                    if (fungsi.cekLRC(DataComplete.Substring(2, DataComplete.Length - 4), DataComplete.Substring(DataComplete.Length - 2, 2)))
                    {
                        WriteLog(DataComplete);
                        WriteLog("Status Code : " + DataComplete.Substring(8, 6));
                        info.StatusCode = DataComplete.Substring(8, 6);
                        if (DataComplete.Substring(8, 6) == "000000")
                        {
                            info.CardType = DataComplete.Substring(14, 2);
                            info.CardNo = DataComplete.Substring(16, 16);
                            info.Balance = int.Parse(DataComplete.Substring(32, 8), System.Globalization.NumberStyles.HexNumber);
                            WriteLog("Card Type : " + info.CardType);
                            WriteLog("Card No : " + info.CardNo);
                            WriteLog("Balance : " + info.Balance);
                        }
                    }
                    break;
                }
            } while (start.AddSeconds(Timeout) > DateTime.Now);
            //SetButton(true);
            return info;
        }

        public TransactionInfo Deduct(int DeductAmount = 1)
        {
            if (!IsConnected || !IsInit) return null;
            TransactionInfo info = new TransactionInfo();
            /*
            SetButton(false);
            int DeductAmount = 1;
            if (rb1.Checked) { DeductAmount = 1; }
            else if (rb1000.Checked) { DeductAmount = 1000; }
            else if (rb5000.Checked) { DeductAmount = 5000; }
            else if (rb1000k.Checked) { DeductAmount = 1000000; }
            */
            string Data = CmdReaderDeduct + DateTime.Now.ToString("ddMMyyyyHHmmss") + DeductAmount.ToString("X").PadLeft(8, '0') + ReaderTimeout;
            DataPrepare = (Data.Length / 2).ToString("X").PadLeft(4, '0') + Data;
            LRC = fungsi.calculateLRC(DataPrepare);

            CommandToReader = fungsi.GetBytes(STX + DataPrepare + LRC, out discarded);
            SendCommandToSerial(CommandToReader);

            DateTime start = DateTime.Now;
            do
            {
                if (DataReceiveComplete)
                {
                    if (fungsi.cekLRC(DataComplete.Substring(2, DataComplete.Length - 4), DataComplete.Substring(DataComplete.Length - 2, 2)))
                    {
                        WriteLog(DataComplete);
                        info.StatusCode = DataComplete.Substring(8, 6);
                        WriteLog("Status Code : " + info.StatusCode);

                        if (DataComplete.Substring(8, 6) == "000000")
                        {
                            info.CardType = DataComplete.Substring(14, 2);
                            info.MID = DataComplete.Substring(16, 16);
                            info.TID = DataComplete.Substring(32, 8);
                            WriteLog("Card Type : " + info.CardType);
                            WriteLog("MID : " + info.MID);
                            WriteLog("TID : " + info.TID);
                            string Transdate = DataComplete.Substring(40, 2) + "-" + DataComplete.Substring(42, 2) + "-" + DataComplete.Substring(44, 4) + " " +
                                               DataComplete.Substring(48, 2) + ":" + DataComplete.Substring(50, 2) + ":" + DataComplete.Substring(52, 2);
                            info.TransDate = Transdate;
                            WriteLog("TransDate : " + Transdate);
                            info.CardNo = DataComplete.Substring(54, 16);
                            info.Amount = int.Parse(DataComplete.Substring(70, 8), System.Globalization.NumberStyles.HexNumber);
                            info.Balance = int.Parse(DataComplete.Substring(78, 8), System.Globalization.NumberStyles.HexNumber);
                            info.TransCounter = DataComplete.Substring(86, 8);
                            info.TransLog = DataComplete.Substring(94, DataComplete.Length - 96);

                            WriteLog("Card No : " + info.CardNo);
                            WriteLog("Amount : " + info.Amount);
                            WriteLog("Balance : " + info.Balance);
                            WriteLog("Trans Counter : " + info.TransCounter);
                            WriteLog("Trans Log : " + info.TransLog);
                        }
                    }
                    break;
                }
            } while (start.AddSeconds(Timeout) > DateTime.Now);
            //SetButton(true);
            return info;
        }

        public TransactionInfo GetLastTransaction()
        {
            if (!IsConnected || !IsInit) return null;
            TransactionInfo info = new TransactionInfo();
            //SetButton(false);
            string Data = CmdReaderGetLastTransaction;
            DataPrepare = (Data.Length / 2).ToString("X").PadLeft(4, '0') + Data;
            LRC = fungsi.calculateLRC(DataPrepare);

            CommandToReader = fungsi.GetBytes(STX + DataPrepare + LRC, out discarded);
            SendCommandToSerial(CommandToReader);

            DateTime start = DateTime.Now;
            do
            {
                if (DataReceiveComplete)
                {
                    if (fungsi.cekLRC(DataComplete.Substring(2, DataComplete.Length - 4), DataComplete.Substring(DataComplete.Length - 2, 2)))
                    {
                        WriteLog(DataComplete);
                        if (DataComplete.Substring(8, 6) == "000000")
                        {
                            info.CardType = DataComplete.Substring(14, 2);
                            info.MID = DataComplete.Substring(16, 16);
                            info.TID = DataComplete.Substring(32, 8);
                            WriteLog("Card Type : " + info.CardType);
                            WriteLog("MID : " + info.MID);
                            WriteLog("TID : " + info.TID);
                            string Transdate = DataComplete.Substring(40, 2) + "-" + DataComplete.Substring(42, 2) + "-" + DataComplete.Substring(44, 4) + " " +
                                               DataComplete.Substring(48, 2) + ":" + DataComplete.Substring(50, 2) + ":" + DataComplete.Substring(52, 2);
                            info.TransDate = Transdate;
                            info.CardNo = DataComplete.Substring(54, 16);
                            info.Amount = int.Parse(DataComplete.Substring(70, 8), System.Globalization.NumberStyles.HexNumber);
                            info.Balance = int.Parse(DataComplete.Substring(78, 8), System.Globalization.NumberStyles.HexNumber);
                            info.TransCounter = DataComplete.Substring(86, 8);
                            info.TransLog = DataComplete.Substring(94, DataComplete.Length - 96);
                            WriteLog("TransDate : " + Transdate);
                            WriteLog("Card No : " + info.CardNo);
                            WriteLog("Amount : " + info.Amount);
                            WriteLog("Balance : " + info.Balance);
                            WriteLog("Trans Counter : " + info.TransCounter);
                            WriteLog("Trans Log : " + info.TransLog);
                        }
                    }
                    break;
                }
            } while (start.AddSeconds(Timeout) > DateTime.Now);
            return info;
            //SetButton(true);
        }

        public DeductCorrectionInfo DeductCorrection()
        {
            if (!IsConnected || !IsInit) return null;
            DeductCorrectionInfo info = new DeductCorrectionInfo();
            //SetButton(false);
            string Data = CmdReaderCancelDeductCorrection;
            DataPrepare = (Data.Length / 2).ToString("X").PadLeft(4, '0') + Data;
            LRC = fungsi.calculateLRC(DataPrepare);

            CommandToReader = fungsi.GetBytes(STX + DataPrepare + LRC, out discarded);
            SendCommandToSerial(CommandToReader);

            DateTime start = DateTime.Now;
            do
            {
                if (DataReceiveComplete)
                {
                    if (fungsi.cekLRC(DataComplete.Substring(2, DataComplete.Length - 4), DataComplete.Substring(DataComplete.Length - 2, 2)))
                    {
                        WriteLog(DataComplete);
                        info.StatusCode = DataComplete.Substring(8, 6);
                        WriteLog("Status Code : " + info.StatusCode);
                        if (DataComplete.Substring(8, 6) == "000000" && DataComplete.Length > 16)
                        {
                            info.CardType = DataComplete.Substring(14, 2);
                            info.CardNo = DataComplete.Substring(16, 16);
                            WriteLog("Card Type : " + info.CardType);
                            WriteLog("Card No : " + info.CardNo);
                        }
                    }
                    break;
                }
            } while (start.AddSeconds(Timeout) > DateTime.Now);
            return info;
            //SetButton(true);
        }
        /*
        private void SetButton(bool StatusButton)
        {
            btnReaderINIT.Enabled = StatusButton;
            btnCheckBalance.Enabled = StatusButton;
            btnDeduct.Enabled = StatusButton;
            btnCancelDeductCorrection.Enabled = StatusButton;
            btnGetStatus.Enabled = StatusButton;
            btnReadData.Enabled = StatusButton;
            btnWriteData.Enabled = StatusButton;

            // MIFARE
            btnGetUID.Enabled = StatusButton;
            btnReadMifare.Enabled = StatusButton;
            btnWriteMifare.Enabled = StatusButton;

            //Display and Buzzer
            btnDisplaySet.Enabled = StatusButton;
            btnDisplayReset.Enabled = StatusButton;
            btnBuzzOK.Enabled = StatusButton;
            btnBuzzNOK.Enabled = StatusButton;
        }
        */
        public CardDataInfo ReadCardData()
        {
            if (!IsConnected || !IsInit) return null;
            CardDataInfo info = new CardDataInfo();
            //SetButton(false);
            string Data = CmdReadData + SlotData + LengthCardData.ToString("X").PadLeft(2, '0') + ReaderTimeout;
            DataPrepare = (Data.Length / 2).ToString("X").PadLeft(4, '0') + Data;
            LRC = fungsi.calculateLRC(DataPrepare);

            CommandToReader = fungsi.GetBytes(STX + DataPrepare + LRC, out discarded);
            SendCommandToSerial(CommandToReader);

            DateTime start = DateTime.Now;
            do
            {
                if (DataReceiveComplete)
                {
                    if (fungsi.cekLRC(DataComplete.Substring(2, DataComplete.Length - 4), DataComplete.Substring(DataComplete.Length - 2, 2)))
                    {
                        WriteLog(DataComplete);
                        info.StatusCode = DataComplete.Substring(8, 6);
                        WriteLog("Status Code : " + info.StatusCode);
                        if (DataComplete.Substring(8, 6) == "000000")
                        {
                            info.CardType = DataComplete.Substring(14, 2);
                            info.CardNo = DataComplete.Substring(16, 16);
                            info.Data = DataComplete.Substring(32, DataComplete.Length - 34);
                            WriteLog("Card Type : " + info.CardType);
                            WriteLog("Card No : " + info.CardNo);
                            WriteLog("Data : " + info.Data);
                        }
                    }
                    break;
                }
            } while (start.AddSeconds(Timeout) > DateTime.Now);
            return info;
            //SetButton(true);
        }

        public CardDataInfo WriteData(string DataToWrite)
        {
            if (!IsConnected || !IsInit) return null;
            CardDataInfo info = new CardDataInfo();
            //SetButton(false);

            string Data = CmdWriteData + SlotData + (DataToWrite.Length / 2).ToString("X").PadLeft(2, '0') + DataToWrite + ReaderTimeout;
            DataPrepare = (Data.Length / 2).ToString("X").PadLeft(4, '0') + Data;
            LRC = fungsi.calculateLRC(DataPrepare);

            CommandToReader = fungsi.GetBytes(STX + DataPrepare + LRC, out discarded);
            SendCommandToSerial(CommandToReader);

            DateTime start = DateTime.Now;
            do
            {
                if (DataReceiveComplete)
                {
                    if (fungsi.cekLRC(DataComplete.Substring(2, DataComplete.Length - 4), DataComplete.Substring(DataComplete.Length - 2, 2)))
                    {
                        WriteLog(DataComplete);
                        info.StatusCode = DataComplete.Substring(8, 6);
                        WriteLog("Status Code : " + info.StatusCode);
                        if (DataComplete.Substring(8, 6) == "000000")
                        {
                            info.CardType = DataComplete.Substring(14, 2);
                            info.CardNo = DataComplete.Substring(16, 16);
                            info.Data = DataComplete.Substring(32, DataComplete.Length - 34);
                            WriteLog("Card Type : " + info.CardType);
                            WriteLog("Card No : " + info.CardNo);
                            WriteLog("Data : " + info.Data);

                        }
                    }
                    break;
                }
            } while (start.AddSeconds(Timeout) > DateTime.Now);
            return info;
            //SetButton(true);
        }

        #region "           SERIAL              "

        delegate void SetTextCallback(string text);
        static string InputData = String.Empty;

        private bool OpenCommunication()
        {
            try
            {
                if (!ComPort.IsOpen)
                {
                   
                    //ComPort.Connect();
                    ComPort.PortName = this.PortName;//"COM" + txtCOMPort.Text;
                    ComPort.Open();
                }
                else
                {
                    //ComPort.Disconnect();
                    //ComPort.Connect();
                    ComPort.Close();
                    ComPort.Open();
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        private void ClosedCommunication()
        {
            try
            {
                //ComPort.Disconnect();
                ComPort.Close();
            }
            catch { }
        }
        private bool SendCommandToSerial(byte[] theCommand)
        {
            //txtResult.Text = "";
            TempData = "";
            DataReceiveComplete = false;
            DataComplete = "";

            try
            {
                OpenCommunication();
                System.Threading.Thread.Sleep(20);
                ComPort.Write(theCommand, 0, theCommand.GetLength(0));//SendMessage(theCommand);
                //
            }
            catch (Exception ex)
            {
                Console.WriteLine("error send message :" + ex.Message);
            }
            return false;
        }
        
        private static void ComPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] data = new byte[ComPort.BytesToRead];
            ComPort.Read(data, 0, data.Length);

            InputData = fungsi.ToString(data);
            TempData += InputData;
            if (TempData.Length >= 6)
            {
                int lengthData = int.Parse(TempData.Substring(2, 4), System.Globalization.NumberStyles.HexNumber) * 2;
                if (TempData.Length == lengthData + 8)
                {
                    DataComplete = TempData;
                    DataReceiveComplete = true;
                }
            }
        }
        private static void raiseAppSerialDataEvent(byte[] data)
        {
            //byte[] data = new byte[DataReceived.Length];
            //ComPort.Read(data, 0, data.Length);

            InputData = fungsi.ToString(data);
            TempData += InputData;
            if (TempData.Length >= 6)
            {
                int lengthData = int.Parse(TempData.Substring(2, 4), System.Globalization.NumberStyles.HexNumber) * 2;
                if (TempData.Length == lengthData + 8)
                {
                    DataComplete = TempData;
                    DataReceiveComplete = true;
                }
            }

        }
        #endregion

        /*

        #region "                   MIFARE                      "

        private void btnGetUID_Click(object sender, EventArgs e)
        {
            SetButton(false);
            string Data = CmdGetUIDMifare + ReaderTimeout;
            DataPrepare = (Data.Length / 2).ToString("X").PadLeft(4, '0') + Data;
            LRC = fungsi.calculateLRC(DataPrepare);

            CommandToReader = fungsi.GetBytes(STX + DataPrepare + LRC, out discarded);
            SendCommandToSerial(CommandToReader);

            DateTime start = DateTime.Now;
            do
            {
                if (DataReceiveComplete)
                {
                    if (fungsi.cekLRC(DataComplete.Substring(2, DataComplete.Length - 4), DataComplete.Substring(DataComplete.Length - 2, 2)))
                    {
                        txtResult.Text = DataComplete + Environment.NewLine;
                        txtResult.AppendText("Status Code : " + DataComplete.Substring(8, 6) + Environment.NewLine);
                        if (DataComplete.Substring(8, 6) == "000000")
                        {
                            txtResult.AppendText("Card Type : " + DataComplete.Substring(14, 2) + Environment.NewLine);
                            txtResult.AppendText("Card UID : " + DataComplete.Substring(16, 14) + Environment.NewLine);
                        }
                    }
                    break;
                }
            } while (start.AddSeconds(Timeout) > DateTime.Now);
            SetButton(true);
        }

        private void btnReadMifare_Click(object sender, EventArgs e)
        {
            SetButton(false);
            try
            {
                if (txtKeyMifare.Text.Length != 12) { throw new Exception("READ FAILED" + Environment.NewLine + "Wrong length key"); }

                string Data = CmdReadMifare + Convert.ToInt32(txtBlok.Text).ToString("X").PadLeft(2, '0') + KeyType + txtKeyMifare.Text + ReaderTimeout;
                DataPrepare = (Data.Length / 2).ToString("X").PadLeft(4, '0') + Data;
                LRC = fungsi.calculateLRC(DataPrepare);

                CommandToReader = fungsi.GetBytes(STX + DataPrepare + LRC, out discarded);
                SendCommandToSerial(CommandToReader);

                DateTime start = DateTime.Now;
                do
                {
                    if (DataReceiveComplete)
                    {
                        if (fungsi.cekLRC(DataComplete.Substring(2, DataComplete.Length - 4), DataComplete.Substring(DataComplete.Length - 2, 2)))
                        {
                            txtResult.Text = DataComplete + Environment.NewLine;
                            txtResult.AppendText("Status Code : " + DataComplete.Substring(8, 6) + Environment.NewLine);
                            if (DataComplete.Substring(8, 6) == "000000")
                            {
                                txtResult.AppendText("Data : " + DataComplete.Substring(14, 32) + Environment.NewLine);
                            }
                        }
                        break;
                    }
                } while (start.AddSeconds(Timeout) > DateTime.Now);
            }
            catch (Exception ex)
            {
                txtResult.Text = ex.Message;
            }
            SetButton(true);
        }
        private void btnWriteMifare_Click(object sender, EventArgs e)
        {
            SetButton(false);
            try
            {
                if (txtKeyMifare.Text.Length != 12) { throw new Exception("READ FAILED" + Environment.NewLine + "Wrong length key"); }

                string Data = CmdWriteMifare + Convert.ToInt32(txtBlok.Text).ToString("X").PadLeft(2, '0') + txtData.Text.PadLeft(32, '0') + ReaderTimeout;
                DataPrepare = (Data.Length / 2).ToString("X").PadLeft(4, '0') + Data;
                LRC = fungsi.calculateLRC(DataPrepare);

                CommandToReader = fungsi.GetBytes(STX + DataPrepare + LRC, out discarded);
                SendCommandToSerial(CommandToReader);

                DateTime start = DateTime.Now;
                do
                {
                    if (DataReceiveComplete)
                    {
                        if (fungsi.cekLRC(DataComplete.Substring(2, DataComplete.Length - 4), DataComplete.Substring(DataComplete.Length - 2, 2)))
                        {
                            txtResult.Text = DataComplete + Environment.NewLine;
                            txtResult.AppendText("Status Code : " + DataComplete.Substring(8, 6) + Environment.NewLine);
                            if (DataComplete.Substring(8, 6) == "000000")
                            {
                                txtResult.AppendText("Data : " + DataComplete.Substring(14, 32) + Environment.NewLine);
                            }
                        }
                        break;
                    }
                } while (start.AddSeconds(Timeout) > DateTime.Now);
            }
            catch (Exception ex)
            {
                txtResult.Text = ex.Message;
            }
            SetButton(true);
        }

        #endregion
        */
        #region "              DISPLAY AND BUZZER               "
        public bool SetDisplay(string Dsp1, string Dsp2, string Dsp3, string Dsp4, bool ClearDisplay)
        {
            bool scs = false;
            string ResultText = "";
            WriteLog("Set Display");
            ResultText += "Set Display" + Environment.NewLine;
            string Data = CmdSetDisplayBuzzer;
            if (ClearDisplay) Data += "00";
            else Data += "01" + (Dsp1.PadRight(16, ' ') + Dsp2.PadRight(16, ' ') + Dsp3.PadRight(16, ' ') + Dsp4.PadRight(16, ' ')).ToHex();
            bool isTimeout = true;
            DataPrepare = (Data.Length / 2).ToString("X").PadLeft(4, '0') + Data;
            LRC = fungsi.calculateLRC(DataPrepare);


            WriteLog(STX + DataPrepare + LRC);
            ResultText += STX + DataPrepare + LRC + Environment.NewLine;
            CommandToReader = fungsi.GetBytes(STX + DataPrepare + LRC, out discarded);
            SendCommandToSerial(CommandToReader);

            DateTime start = DateTime.Now;
            do
            {
                if (DataReceiveComplete)
                {
                    isTimeout = false;
                    if (fungsi.cekLRC(DataComplete.Substring(2, DataComplete.Length - 4), DataComplete.Substring(DataComplete.Length - 2, 2)))
                    {
                        WriteLog(DataComplete);
                        ResultText += DataComplete + Environment.NewLine;
                        WriteLog("Status Code : " + DataComplete.Substring(8, 6));
                        ResultText += "Status Code : " + DataComplete.Substring(8, 6) + Environment.NewLine;
                        if (DataComplete.Substring(8, 6) == "000000")
                        {
                            scs = true;
                        }
                    }
                    break;
                }
            } while (start.AddSeconds(Timeout) > DateTime.Now);

            if (isTimeout)
            {
                WriteLog("OPERATION TIMEOUT!");
                ResultText += "OPERATION TIMEOUT!";
            }
            return scs;
        }
        public bool SetDisplay(string Line1, string Line2, string Line3, string Line4)
        {
            return SetDisplay(Line1, Line2, Line3, Line4, false);
        }
        public bool ResetDisplay()
        {
            return SetDisplay("", "", "", "", true);
        }
        public bool SetBuzzer(bool BuzzSuccess)
        {
            bool scs = false;
            string ResultText = "";
            WriteLog("Set Buzzer");
            ResultText += "Set Buzzer" + Environment.NewLine;
            string Data = CmdSetDisplayBuzzer + "02";
            if (BuzzSuccess) Data += "00";
            else Data += "01";
            bool isTimeout = true;
            DataPrepare = (Data.Length / 2).ToString("X").PadLeft(4, '0') + Data;
            LRC = fungsi.calculateLRC(DataPrepare);


            WriteLog(STX + DataPrepare + LRC);
            ResultText += STX + DataPrepare + LRC + Environment.NewLine;
            CommandToReader = fungsi.GetBytes(STX + DataPrepare + LRC, out discarded);
            SendCommandToSerial(CommandToReader);

            DateTime start = DateTime.Now;
            do
            {
                if (DataReceiveComplete)
                {
                    isTimeout = false;
                    if (fungsi.cekLRC(DataComplete.Substring(2, DataComplete.Length - 4), DataComplete.Substring(DataComplete.Length - 2, 2)))
                    {
                        WriteLog(DataComplete);
                        ResultText += DataComplete + Environment.NewLine;
                        WriteLog("Status Code : " + DataComplete.Substring(8, 6));
                        ResultText += "Status Code : " + DataComplete.Substring(8, 6) + Environment.NewLine;
                        if (DataComplete.Substring(8, 6) == "000000")
                        {
                            scs = true;
                        }
                    }
                    break;
                }
            } while (start.AddSeconds(Timeout) > DateTime.Now);

            if (isTimeout)
            {
                WriteLog("OPERATION TIMEOUT!");
                ResultText += "OPERATION TIMEOUT!";
            }
            return scs;
        }

        #endregion
    }
    public static class StringExtensions
    {
        public static string ToHex(this string input)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in input)
                sb.AppendFormat("{0:X2}", (int)c);
            return sb.ToString().Trim();
        }
        public static string HexToString(this string hexString)
        {
            try
            {
                string ascii = string.Empty;

                for (int i = 0; i < hexString.Length; i += 2)
                {
                    String hs = string.Empty;

                    hs = hexString.Substring(i, 2);
                    uint decval = System.Convert.ToUInt32(hs, 16);
                    char character = System.Convert.ToChar(decval);
                    ascii += character;
                }

                return ascii;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            return string.Empty;
        }
    }
    public class CheckBalanceInfo
    {
        public string CardType { get; set; }
        public string CardNo { get; set; }
        public int Balance { get; set; }
        public string StatusCode { get; set; }

    }

    public class DeductCorrectionInfo
    {
        public string CardType { get; set; }
        public string CardNo { get; set; }

        public string StatusCode { get; set; }

    }
    public class CardDataInfo
    {
        public string CardType { get; set; }
        public string CardNo { get; set; }
        public string Data { get; set; }
        public string StatusCode { get; set; }

    }

    public class TransactionInfo
    {
        public string StatusCode { get; set; }
        public string CardType { get; set; }
        public string MID { get; set; }
        public string TID { get; set; }
        public string TransDate { get; set; }
        public string CardNo { get; set; }
        public int Amount { get; set; }
        public int Balance { get; set; }
        public string TransCounter { get; set; }
        public string TransLog { get; set; }

    }
}
