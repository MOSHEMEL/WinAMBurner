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

        public FormNotify(List<string> text, NotifyButtons notifyButtons, string caption = "Error")
        {
            InitializeComponent();
            Scale(new SizeF(Field.ScaleFactor, Field.ScaleFactor));
            Font = new Font("Segoe UI", Field.DefaultFont, FontStyle.Regular, GraphicsUnit.Point);
            Name = caption;

            new Field(ltype: typeof(Label), ltext: text[0], lplacev: Place.Two).draw(this, true);
            if (text.Count > 1)
                new Field(ltype: typeof(Label), ltext: text[1], lplacev: Place.Three).draw(this, true);
            if (text.Count > 2)
                new Field(ltype: typeof(Label), ltext: text[2], lplacev: Place.Four).draw(this, true);
            if (text.Count > 3)
                new Field(ltype: typeof(Label), ltext: text[3], lplacev: Place.Five).draw(this, true);
            if (text.Count > 4)
                new Field(ltype: typeof(Label), ltext: text[4], lplacev: Place.Six).draw(this, true);

            buttonOK = new Field(ltype: typeof(Button), ltext: "OK", buttonEventHandler: new EventHandler(this.buttonOK_Click), lplacev: Place.Seven).draw(this, true) as Button;
            buttonYes = new Field(ltype: typeof(Button), ltext: "Yes", buttonEventHandler: new EventHandler(this.buttonYes_Click), lplaceh: Place.One, lplacev: Place.Seven).draw(this, true) as Button;
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
