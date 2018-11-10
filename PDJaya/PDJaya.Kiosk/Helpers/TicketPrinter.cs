using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;

namespace PDJaya.Kiosk.Helpers
{
    public class TicketPrinter
    {
        PrintDocument PrintDocument;
        string PrinterName;
        /*
        CardLog Ticket { set; get; }
        TicketPool TicketHeader { set; get; }

        public void Printv1(CardLog data, TicketPool header, string printerName = "ZJ-58")
        {
            Ticket = data;
            TicketHeader = header;
            PrinterName = printerName;
            var PrinterSettings = new PrinterSettings()
            {
                PrinterName = printerName
            };
            PrintDocument = new PrintDocument()
            {
                PrinterSettings = PrinterSettings
            };
            PrintDocument.PrintPage += Pd_PrintPage;
            PrintDocument.Print();
            Console.WriteLine("Print Completed");
        }

        private void Pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            Font font = new Font("Arial", 10);
            Console.WriteLine("Entered on event");
            var img = Properties.Resources.logo;
            var newwidth = 50 * img.Width / img.Height;
            ev.Graphics.DrawImage(img, 0, 0,newwidth,50);
            //tanggal dan waktu
            ev.Graphics.DrawString(DateTime.Now.ToString("dd-MM-yy"), font, new SolidBrush(Color.Black), 0, 70);
            ev.Graphics.DrawString(DateTime.Now.ToString("HH:mm"), font, new SolidBrush(Color.Black), 120, 70);
            //moda
            ev.Graphics.DrawString(Ticket.Moda, new Font("Arial", 10), new SolidBrush(Color.Black), 0, 90);
            //static 
            ev.Graphics.DrawString("Angkutan Kota", new Font("Arial", 10), new SolidBrush(Color.Black), 0, 120);
            //ticket no
            ev.Graphics.DrawString($"No Ticket :{Ticket.TicketNo}", new Font("Arial", 8), new SolidBrush(Color.Black), 0, 135);
            //lokasi
            ev.Graphics.DrawString($"{Ticket.Location}", new Font("Arial", 10), new SolidBrush(Color.Black), 0, 160);
            //tiket start
            ev.Graphics.DrawString($"Tiket Valid", new Font("Arial", 10), new SolidBrush(Color.Black), 0, 175);
            ev.Graphics.DrawString($"{TicketHeader.StartDate.ToString("HH:mm")}", new Font("Arial", 10), new SolidBrush(Color.Black), 0, 190);

            //tiket exp
            ev.Graphics.DrawString($"Tiket Expired", new Font("Arial", 10), new SolidBrush(Color.Black), 0, 210);
            ev.Graphics.DrawString($"{TicketHeader.EndDate.ToString("HH:mm")}", new Font("Arial", 10), new SolidBrush(Color.Black), 0, 225);

            //amount
            ev.Graphics.DrawString($"Rp.{String.Format("{0:n}", Ticket.Amount)}", new Font("Arial", 15), new SolidBrush(Color.Black), 0, 245);

            //ev.Graphics.DrawImage(_Ticket, 0, 0);
            Console.WriteLine("Leaving event");
        }
        */
        public static void Printv2(string cmd = "/home/pi/print/print.py")//CardLog data, TicketPool header,string cmd = "/home/pi/print/struk.py")
        {
            var args = "";// $"'{JsonConvert.SerializeObject(header,Formatting.None)}' '{JsonConvert.SerializeObject(data,Formatting.None)}'";
            Console.WriteLine(args);
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = "python";
            start.Arguments = string.Format("{0} {1}", cmd, args);
            start.UseShellExecute = false;
            start.CreateNoWindow = true;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    Console.Write(result);
                }
            }
        }
        
    }
}
