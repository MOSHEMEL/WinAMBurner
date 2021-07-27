using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WinAMBurner
{
    class GuiBase
    {
        public enum Place
        {
            None,
            //Center, RightTwo, RightOne, RightThree, LeftFive, LeftSix, LeftFour, Start, End,
            Center, Start, End,
            One, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Eleven
        }

        public const int DefaultWidth = 390;
        public const int DefaultWidthLarge = 1200;
        public const int DefaultHeight = 60;
        public const int DefaultHeightSmall = 12;
        public const int DefaultHeightLarge = 90;
        public const float ScaleFactor = 0.5F;
        public const float PlaceOne = 200 * ScaleFactor;
        public const float DeltaV = 100 * ScaleFactor;
        //public const float DefaultFont = 18F * ScaleFactor;
        public const float DefaultFont = 24F * ScaleFactor;
        //public const float DefaultFontLarge = 24F * ScaleFactor;
        public const float DefaultFontLarge = 30F * ScaleFactor;
        public const string DefaultText = "DefaultText";
    }

    class Gui : GuiBase
    {
        public static Control draw(Form thisForm, Type type, string text = null, string name = DefaultText,
            float font = DefaultFont, Color color = new Color(),
            int width = DefaultWidth, int height = DefaultHeight, bool autoSize = true,
            //List<string> items = null,
            object[] items = null,
            EventHandler eventHandler = null,
            LinkLabelLinkClickedEventHandler linkLabelLinkClickedEventHandler = null,
            Place placeh = Place.Center, Place placev = Place.Center)
        {
            Control control = type.GetConstructor(new Type[] { }).Invoke(null) as Control;
            thisForm.Controls.Add(control);
            control.Anchor = (AnchorStyles.Top);// | (AnchorStyles.Left);
            control.Margin = new Padding(4);
            control.Size = new Size(width, height);
            control.Scale(new SizeF(ScaleFactor, ScaleFactor));
            control.TabIndex = 1;
            control.Text = text;
            control.Font = new Font("Segoe UI", font, FontStyle.Regular, GraphicsUnit.Point);

            if ((type == typeof(PictureBox)) || (type == typeof(LinkLabel)))
                control.AutoSize = true;
            if (type == typeof(Label))
            {
                if (color != Color.Empty)
                    control.ForeColor = color;
                control.AutoSize = autoSize;
            }
            if (type == typeof(PictureBox))
            {
                PictureBox pictureBox = (control as PictureBox);
                if (pictureBox != null)
                    pictureBox.Image = Properties.Resources.ARmentaSmall;
            }
            if (type == typeof(RichTextBox))
            {
                RichTextBox richTextBox = control as RichTextBox;
                if (richTextBox != null)
                {
                    if (eventHandler != null)
                        richTextBox.TextChanged += eventHandler;
                    //if (eventHandler1 != null)
                    //    richTextBox.SelectionChanged += eventHandler1;
                    richTextBox.Multiline = false;
                }
                defaultText(name, control);
            }
            if (type == typeof(Button))
                if (eventHandler != null)
                    control.Click += eventHandler;
            if (type == typeof(LinkLabel))
            {
                LinkLabel linkLabel = control as LinkLabel;
                if (linkLabel != null)
                {
                    if (linkLabelLinkClickedEventHandler != null)
                        linkLabel.LinkClicked += linkLabelLinkClickedEventHandler;
                }
            }
            if (type == typeof(ComboBox))
            {
                ComboBox comboBox = (control as ComboBox);
                if (comboBox != null)
                {
                    if (items != null)
                        comboBox.Items.AddRange(items);
                    if (eventHandler != null)
                        comboBox.SelectedIndexChanged += eventHandler;
                    comboBox.TextChanged += comboBox_TextChanged;
                    //comboBox.TextUpdate += comboBox_TextChanged;
                }
                defaultText(name, control);
            }
            if (type == typeof(RadioButton))
            {
                RadioButton radioButton = control as RadioButton;
                if (radioButton != null)
                    radioButton.CheckedChanged += eventHandler;
            }

            control.Location = placeCalc(thisForm, control, placeh: placeh, placev: placev);

            return control;
        }

        private static void defaultText(string name, Control control)
        {
            control.Name = name;
            if (control.Name == DefaultText)
            {
                control.ForeColor = Color.Silver;
                control.Enter += new EventHandler(controlEnter_Click);
                control.Leave += new EventHandler(controlLeave_Click);
            }
        }

        public static void hide(Form thisForm)
        {
            while (thisForm.Controls.Count > 0)
                thisForm.Controls[0].Dispose();
        }

        public static Point placeCalc(Form thisForm,
                Control control, Point location = new System.Drawing.Point(), Place placeh = Place.Center, Place placev = Place.Center)
        {
            //if (placeh == Place.Center)
            //    location.X = thisForm.Width / 2 - control.Width / 2;
            //else if (placeh == Place.Right)
            //    location.X = thisForm.Width / 2 + control.Width / 2 + control.Width / 4;
            //else if (placeh == Place.RightOne)
            //    location.X = thisForm.Width / 2 + control.Width / 4 / 2;
            //else if (placeh == Place.RightTwo)
            //    location.X = thisForm.Width / 2 + control.Width + control.Width / 4 / 2 + control.Width / 4;
            //else if (placeh == Place.Left)
            //    location.X = thisForm.Width / 2 - control.Width / 2 - control.Width - control.Width / 4;
            //else if (placeh == Place.LeftOne)
            //    location.X = thisForm.Width / 2 - control.Width - control.Width / 4 / 2;
            //else if (placeh == Place.LeftTwo)
            //    location.X = thisForm.Width / 2 - control.Width * 2 - control.Width / 4 / 2 - control.Width / 4;
            //else if (placeh == Place.Start)
            //    location.X = thisForm.Width / 2 - control.Width / 2 - control.Width * 2 - control.Width * 2 / 4;
            //else if (placeh == Place.End)
            //    location.X = thisForm.Width / 2 + control.Width / 2 + control.Width + control.Width * 2 / 4;

            if (placeh == Place.Center)
                location.X = thisForm.Width / 2 - control.Width / 2;
            else if (placeh == Place.Start)
                location.X = thisForm.Width / 2 - control.Width / 2 - control.Width * 2 - control.Width * 2 / 4;
            else if (placeh == Place.End)
                location.X = thisForm.Width / 2 + control.Width / 2 + control.Width + control.Width * 2 / 4;
            else if (placeh == Place.One)
                location.X = thisForm.Width / 2 + control.Width / 4 / 2;
            else if (placeh == Place.Two)
                location.X = thisForm.Width / 2 + control.Width / 2 + control.Width / 4;
            else if (placeh == Place.Three)
                location.X = thisForm.Width / 2 + control.Width + control.Width / 4 / 2 + control.Width / 4;
            else if (placeh == Place.Four)
                location.X = thisForm.Width / 2 - control.Width * 2 - control.Width / 4 / 2 - control.Width / 4;
            else if (placeh == Place.Five)
                location.X = thisForm.Width / 2 - control.Width / 2 - control.Width - control.Width / 4;
            else if (placeh == Place.Six)
                location.X = thisForm.Width / 2 - control.Width - control.Width / 4 / 2;

            if (placev == Place.One)
                location.Y = 30;
            else if (placev == Place.Two)
                location.Y = (int)(PlaceOne + 0 * DeltaV);//200;
            else if (placev == Place.Three)
                location.Y = (int)(PlaceOne + 1 * DeltaV);//300;
            else if (placev == Place.Four)
                location.Y = (int)(PlaceOne + 2 * DeltaV);//400;
            else if (placev == Place.Five)
                location.Y = (int)(PlaceOne + 3 * DeltaV);//500;
            else if (placev == Place.Six)
                location.Y = (int)(PlaceOne + 4 * DeltaV);//600;
            else if (placev == Place.Seven)
                location.Y = (int)(PlaceOne + 5 * DeltaV);//700;
            else if (placev == Place.Eight)
                location.Y = (int)(PlaceOne + 6 * DeltaV);//800;
            else if (placev == Place.Nine)
                location.Y = (int)(PlaceOne + 7 * DeltaV);//900;
            else if (placev == Place.Ten)
                location.Y = (int)(PlaceOne + 8 * DeltaV);//1000;
            else if (placev == Place.Eleven)
                location.Y = (int)(PlaceOne + 9 * DeltaV);//1100;
            else if (placev == Place.End)
                location.Y = (int)(PlaceOne + 10 * DeltaV);//1200;

            return location;
        }

        public static void dataGridDraw(Form thisForm, ref DataGridView dataGridView, Place placeh = Place.Center, Place placev = Place.Center)
        {
            //this.components = new System.ComponentModel.Container();
            //this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            dataGridView = new DataGridView();
            //((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(dataGridView)).BeginInit();
            //dataGridView.AutoSize = true;
            dataGridView.Anchor = (AnchorStyles.Top);// | (AnchorStyles.Left);
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.RowHeadersWidth = 62;
            dataGridView.RowTemplate.Height = 33;
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView.ReadOnly = true;
            dataGridView.Size = new Size(1853, 800);
            dataGridView.Scale(new SizeF(ScaleFactor, ScaleFactor));
            dataGridView.Font = new Font("Segoe UI", DefaultFont, FontStyle.Regular, GraphicsUnit.Point);
            dataGridView.Location = placeCalc(thisForm, dataGridView, placeh: placeh, placev: placev);
            dataGridView.TabIndex = 6;
            thisForm.Controls.Add(dataGridView);
            //((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(dataGridView)).EndInit();
            //this.bindingSource1.DataSource = new DataSet1.DataTable1DataTable();
            //dataGridView.DataSource = this.bindingSource1;
        }

        private static void controlEnter_Click(object sender, EventArgs e)
        {
            Control control = sender as Control;
            if (control.Name == DefaultText)
            {
                string dflt = control.Text;
                control.Text = "";
                control.Name = dflt;
                control.ForeColor = Color.Black;
            }
        }

        private static void controlLeave_Click(object sender, EventArgs e)
        {
            Control control = sender as Control;
            if (control.Text == string.Empty)
            {
                string dflt = control.Name;
                control.Name = DefaultText;
                control.Text = dflt;
                control.ForeColor = Color.Silver;
            }
        }

        private static void comboBox_TextChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                if (comboBox.Text != string.Empty)
                {
                    if (comboBox.Items != null)
                    {
                        if (comboBox.Items.Count > 0)
                            comboBox.SelectedItem = comboBox.Items.Cast<string>().Where(s => s.ToLower().StartsWith(comboBox.Text.ToLower())).FirstOrDefault();
                        if (comboBox.SelectedItem != null)
                            comboBox.Text = comboBox.SelectedItem.ToString();
                        else
                            comboBox.Text = string.Empty;
                    }
                }
            }
        }

        //private static void comboBox_TextChanged(object sender, EventArgs e)
        //{
        //    ComboBox comboBox = sender as ComboBox;
        //    if (comboBox != null)
        //    {
        //        string str = comboBox.Text;
        //    }
        //}

        public static string boolToString(bool bVal)
        {
            if (bVal)
                return "Yes";
            else
                return "No";
        }

        public static bool stringToBool(string sVal)
        {
            if (sVal == "Yes")
                return true;
            else
                return false;
        }

        //public static string uintToString(uint iVal)
        //{
        //    return iVal.ToString();
        //}

        //public static uint stringToUint(string sVal)
        //{
        //    uint iVal = 0;
        //    uint.TryParse(sVal, out iVal);
        //    return iVal;
        //}

        public static string intToString(int iVal)
        {
            return iVal.ToString();
        }

        public static int stringToInt(string sVal)
        {
            int iVal = 0;
            int.TryParse(sVal, out iVal);
            return iVal;
        }
    }
}

