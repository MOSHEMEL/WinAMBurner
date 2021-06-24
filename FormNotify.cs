using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WinAMBurner
{
    public enum NotifyButtons
    {
        OK,
        YesNo
    }
    public partial class FormNotify : Form
    {
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonYes;
        private System.Windows.Forms.Button buttonNo;

        public FormNotify()
        {
            InitializeComponent();
            this.Scale(new SizeF(Misc.ScaleFactor, Misc.ScaleFactor));
            this.Font = new System.Drawing.Font("Segoe UI", Misc.DefaultFont, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        }

        public FormNotify(string caption, string [] text, NotifyButtons notifyButtons)
        {
            InitializeComponent();
            this.Scale(new SizeF(Misc.ScaleFactor, Misc.ScaleFactor));
            this.Font = new System.Drawing.Font("Segoe UI", Misc.DefaultFont, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            Name = caption;

            Misc.labelDraw(this, ref label1, text[0], placev: Misc.Place.One);
            if (text.Length > 1)
                Misc.labelDraw(this, ref label2, text[1], placev: Misc.Place.Two);
            if (text.Length > 2)
                Misc.labelDraw(this, ref label3, text[2], placev: Misc.Place.Three);
            if (text.Length > 3)
                Misc.labelDraw(this, ref label4, text[3], placev: Misc.Place.Four);
            if (text.Length > 4)
                Misc.labelDraw(this, ref label5, text[4], placev: Misc.Place.Five);
            if (text.Length > 5)
                Misc.labelDraw(this, ref label6, text[5], placev: Misc.Place.Six);

            Misc.buttonDraw(this, ref buttonOK, "OK", new System.EventHandler(this.buttonOK_Click), placev: Misc.Place.Five);
            Misc.buttonDraw(this, ref buttonYes, "Yes", new System.EventHandler(this.buttonOK_Click), placev: Misc.Place.Five, placeh: Misc.Place.Right);
            Misc.buttonDraw(this, ref buttonNo, "No", new System.EventHandler(this.buttonOK_Click), placev: Misc.Place.Five, placeh: Misc.Place.Left);

            switch (notifyButtons)
            {
                case NotifyButtons.OK:
                    buttonOK.Visible = true;
                    buttonNo.Visible = false;
                    buttonYes.Visible = false;
                    break;
                case NotifyButtons.YesNo:
                    buttonOK.Visible = false;
                    buttonNo.Visible = true;
                    buttonYes.Visible = true;
                    break;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonNo_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            Close();
        }

        private void buttonYes_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            Close();
        }
    }
}
