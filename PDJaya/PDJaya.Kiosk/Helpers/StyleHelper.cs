using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDJaya.Kiosk.Helpers
{
    public class StyleHelper
    {
        public static void SetButtonStyle(Button b)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.BackColor = Color.Transparent;
            b.FlatAppearance.MouseDownBackColor = Color.Transparent;
            b.FlatAppearance.MouseOverBackColor = Color.Transparent;
            b.TabStop = false;
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255); //transparent
        }

        public static void SetLabelStyle(Label l)
        {
            l.FlatStyle = FlatStyle.Flat;
            l.BackColor = Color.Transparent;
            l.ForeColor = Color.Black;
            l.Font = new Font("Arial", 24);
        }
    }
}
