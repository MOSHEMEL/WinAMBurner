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
        }

        public FormNotify(string caption, string [] text, NotifyButtons notifyButtons)
        {
            InitializeComponent();
            Name = caption;
            
            labelDraw(ref label1, new Point(35, 240), text[0], Place.Center);
            if (text.Length > 1)
                labelDraw(ref label2, new Point(35, 300), text[1], Place.Center);
            if (text.Length > 2)
                labelDraw(ref label3, new Point(35, 360), text[2], Place.Center);
            if (text.Length > 3)
                labelDraw(ref label4, new Point(35, 420), text[3], Place.Center);
            if (text.Length > 4)
                labelDraw(ref label5, new Point(35, 480), text[4], Place.Center);
            if (text.Length > 5)
                labelDraw(ref label6, new Point(35, 540), text[5], Place.Center);
            
            buttonDraw(ref buttonOK, new System.Drawing.Point(438, 660), "OK", new System.EventHandler(this.buttonOK_Click), Place.Center);
            buttonDraw(ref buttonYes, new System.Drawing.Point(820, 660), "Yes", new System.EventHandler(this.buttonYes_Click), Place.Right);
            buttonDraw(ref buttonNo, new System.Drawing.Point(60, 660), "No", new System.EventHandler(this.buttonNo_Click), Place.Left);

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

        private void buttonDraw(ref Button button, Point location, string text, EventHandler eventHandler, Place place)
        {
            button = new System.Windows.Forms.Button();
            Controls.Add(button);
            button.Anchor = System.Windows.Forms.AnchorStyles.Top;
            button.Margin = new System.Windows.Forms.Padding(4);
            //button.Name = "button";
            button.Size = new System.Drawing.Size(300, 90);
            button.TabIndex = 3;
            button.Text = text;
            button.Location = placeCalc(button, location, place);
            button.UseVisualStyleBackColor = true;
            button.Click += eventHandler;
        }

        private Point placeCalc(Control control, Point location, Place place)
        {
            if (place == Place.Center)
                location.X = this.Width / 2 - control.Width / 2;
            else if (place == Place.Left)
                location.X = this.Width / 4 - control.Width / 2;
            else if (place == Place.Right)
                location.X = this.Width / 4 - control.Width / 2 + this.Width / 2;
            return location;
        }

        private void labelDraw(ref Label label, Point location, string text, Place place)
        {
            label = new System.Windows.Forms.Label();
            Controls.Add(label);
            label.Anchor = System.Windows.Forms.AnchorStyles.Top;
            label.AutoSize = true;
            //label.Name = "label";
            label.Size = new System.Drawing.Size(300, 48);
            label.TabIndex = 20;
            label.Text = text;
            //if (place == Place.Center)
            //    location.X = this.Width / 2 - label.Width / 2;
            label.Location = placeCalc(label, location, place);
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
