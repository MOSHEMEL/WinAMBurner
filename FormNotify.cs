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
            this.Scale(new SizeF(Gui.ScaleFactor, Gui.ScaleFactor));
            this.Font = new System.Drawing.Font("Segoe UI", Gui.DefaultFont, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        }

        //public FormNotify(string[] text, NotifyButtons notifyButtons, string caption = "Error")
        public FormNotify(List<string> text, NotifyButtons notifyButtons, string caption = "Error")
        {
            InitializeComponent();
            this.Scale(new SizeF(Gui.ScaleFactor, Gui.ScaleFactor));
            this.Font = new System.Drawing.Font("Segoe UI", Gui.DefaultFont, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            Name = caption;

            Gui.labelDraw(this, ref label1, text[0], placev: Gui.Place.One);
            if (text.Count > 1)
                Gui.labelDraw(this, ref label2, text[1], placev: Gui.Place.Two);
            if (text.Count > 2)
                Gui.labelDraw(this, ref label3, text[2], placev: Gui.Place.Three);
            if (text.Count > 3)
                Gui.labelDraw(this, ref label4, text[3], placev: Gui.Place.Four);
            if (text.Count > 4)
                Gui.labelDraw(this, ref label5, text[4], placev: Gui.Place.Five);
            //if (text.Count > 5)
            //    Gui.labelDraw(this, ref label6, text[5], placev: Gui.Place.Six);

            Gui.buttonDraw(this, ref buttonOK, "OK", new System.EventHandler(this.buttonOK_Click), placev: Gui.Place.Six);
            Gui.buttonDraw(this, ref buttonYes, "Yes", new System.EventHandler(this.buttonOK_Click), placev: Gui.Place.Six, placeh: Gui.Place.Right);
            Gui.buttonDraw(this, ref buttonNo, "No", new System.EventHandler(this.buttonOK_Click), placev: Gui.Place.Six, placeh: Gui.Place.Left);

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
