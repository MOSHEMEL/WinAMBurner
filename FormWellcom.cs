using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace WinAMBurner
{
    public partial class FormWellcom : Form
    {
        public FormWellcom()
        {
            InitializeComponent();
            System.Timers.Timer timer = new System.Timers.Timer(1000);
            timer.Elapsed += OnElapsedEvent;
            timer.Enabled = true;
        }

        private void formWellcomClose(object sender, ElapsedEventArgs e)
        {
            Close();
        }

        private void OnElapsedEvent(object sender, ElapsedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { formWellcomClose(sender, e); });
            }
        }
    }
}
