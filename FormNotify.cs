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
            Scale(new SizeF(Field.ScaleFactor, Field.ScaleFactor));
            Font = new Font("Segoe UI", Field.DefaultFont, FontStyle.Regular, GraphicsUnit.Point);
        }

        //public FormNotify(string[] text, NotifyButtons notifyButtons, string caption = "Error")
        public FormNotify(List<string> text, NotifyButtons notifyButtons, string caption = "Error")
        {
            InitializeComponent();
            Scale(new SizeF(Field.ScaleFactor, Field.ScaleFactor));
            Font = new Font("Segoe UI", Field.DefaultFont, FontStyle.Regular, GraphicsUnit.Point);
            Name = caption;

            //Gui.labelDraw(this, ref label1, text[0], placev: Gui.Place.Two);
            //Gui.draw(this, typeof(Label), text: text[0], placev: Gui.Place.Two);
            new Field(ltype: typeof(Label), ltext: text[0], lplacev: Place.Two).draw(this, true);
            if (text.Count > 1)
                //Gui.labelDraw(this, ref label2, text[1], placev: Gui.Place.Three);
                //Gui.draw(this, typeof(Label), text: text[1], placev: Gui.Place.Three);
                new Field(ltype: typeof(Label), ltext: text[1], lplacev: Place.Three).draw(this, true);
            if (text.Count > 2)
                //Gui.labelDraw(this, ref label3, text[2], placev: Gui.Place.Four);
                //Gui.draw(this, typeof(Label), text: text[2], placev: Gui.Place.Four);
                new Field(ltype: typeof(Label), ltext: text[2], lplacev: Place.Four).draw(this, true);
            if (text.Count > 3)
                //Gui.labelDraw(this, ref label4, text[3], placev: Gui.Place.Five);
                //Gui.draw(this, typeof(Label), text: text[3], placev: Gui.Place.Five);
                new Field(ltype: typeof(Label), ltext: text[3], lplacev: Place.Five).draw(this, true);
            if (text.Count > 4)
                //Gui.labelDraw(this, ref label5, text[4], placev: Gui.Place.Six);
                //Gui.draw(this, typeof(Label), text: text[4], placev: Gui.Place.Six);
                new Field(ltype: typeof(Label), ltext: text[4], lplacev: Place.Six).draw(this, true);
            //if (text.Count > 5)
            //    Gui.labelDraw(this, ref label6, text[5], placev: Gui.Place.Six);

            //buttonOK = Gui.draw(this, typeof(Button), text: "OK", eventHandler: new EventHandler(this.buttonOK_Click), placev: Gui.Place.Seven) as Button;
            buttonOK = new Field(ltype: typeof(Button), ltext: "OK", buttonEventHandler: new EventHandler(this.buttonOK_Click), lplacev: Place.Seven).draw(this, true) as Button;
            //Gui.buttonDraw(this, ref buttonYes, "Yes", new System.EventHandler(this.buttonOK_Click), placev: Gui.Place.Seven, placeh: Gui.Place.Right);
            //buttonYes = Gui.draw(this, typeof(Button), text: "Yes", eventHandler: new EventHandler(this.buttonYes_Click), placeh: Gui.Place.One, placev: Gui.Place.Seven) as Button;
            buttonYes = new Field(ltype: typeof(Button), ltext: "Yes", buttonEventHandler: new EventHandler(this.buttonYes_Click), lplaceh: Place.One, lplacev: Place.Seven).draw(this, true) as Button;
            //Gui.buttonDraw(this, ref buttonNo, "No", new System.EventHandler(this.buttonOK_Click), placev: Gui.Place.Seven, placeh: Gui.Place.Left);
            //buttonNo = Gui.draw(this, typeof(Button), text: "No", eventHandler: new EventHandler(this.buttonNo_Click), placeh: Gui.Place.Six, placev: Gui.Place.Seven) as Button;
            buttonNo = new Field(ltype: typeof(Button), ltext: "No", buttonEventHandler: new EventHandler(this.buttonNo_Click), lplaceh: Place.Six, lplacev: Place.Seven).draw(this, true) as Button;

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
