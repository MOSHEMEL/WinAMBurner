using System;
using System.Collections.Generic;
using System.Drawing;
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
        private Button buttonOK;
        private Button buttonYes;
        private Button buttonNo;

        public FormNotify()
        {
            InitializeComponent();
            Scale(new SizeF(Gui.ScaleFactor, Gui.ScaleFactor));
            Font = new Font("Segoe UI", Gui.DefaultFont, FontStyle.Regular, GraphicsUnit.Point);
        }

        //public FormNotify(string[] text, NotifyButtons notifyButtons, string caption = "Error")
        public FormNotify(List<string> text, NotifyButtons notifyButtons, string caption = "Error")
        {
            InitializeComponent();
            Scale(new SizeF(Gui.ScaleFactor, Gui.ScaleFactor));
            Font = new Font("Segoe UI", Gui.DefaultFont, FontStyle.Regular, GraphicsUnit.Point);
            Name = caption;

            //Gui.labelDraw(this, ref label1, text[0], placev: Gui.Place.Two);
            Gui.draw(this, typeof(Label), text: text[0], placev: Gui.Place.Two);
            if (text.Count > 1)
                //Gui.labelDraw(this, ref label2, text[1], placev: Gui.Place.Three);
                Gui.draw(this, typeof(Label), text: text[1], placev: Gui.Place.Three);
            if (text.Count > 2)
                //Gui.labelDraw(this, ref label3, text[2], placev: Gui.Place.Four);
                Gui.draw(this, typeof(Label), text: text[2], placev: Gui.Place.Four);
            if (text.Count > 3)
                //Gui.labelDraw(this, ref label4, text[3], placev: Gui.Place.Five);
                Gui.draw(this, typeof(Label), text: text[3], placev: Gui.Place.Five);
            if (text.Count > 4)
                //Gui.labelDraw(this, ref label5, text[4], placev: Gui.Place.Six);
                Gui.draw(this, typeof(Label), text: text[4], placev: Gui.Place.Six);
            //if (text.Count > 5)
            //    Gui.labelDraw(this, ref label6, text[5], placev: Gui.Place.Six);

            buttonOK = Gui.draw(this, typeof(Button), text: "OK", eventHandler: new EventHandler(this.buttonOK_Click), placev: Gui.Place.Seven) as Button;
            //Gui.buttonDraw(this, ref buttonYes, "Yes", new System.EventHandler(this.buttonOK_Click), placev: Gui.Place.Seven, placeh: Gui.Place.Right);
            buttonYes = Gui.draw(this, typeof(Button), text: "Yes", eventHandler: new EventHandler(this.buttonYes_Click), placeh: Gui.Place.One, placev: Gui.Place.Seven) as Button;
            //Gui.buttonDraw(this, ref buttonNo, "No", new System.EventHandler(this.buttonOK_Click), placev: Gui.Place.Seven, placeh: Gui.Place.Left);
            buttonNo = Gui.draw(this, typeof(Button), text: "No", eventHandler: new EventHandler(this.buttonNo_Click), placeh: Gui.Place.Six, placev: Gui.Place.Seven) as Button;

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
