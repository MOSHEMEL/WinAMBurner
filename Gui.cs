using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinAMBurner
{
    static class Gui
    {
        public enum Place
        {
            Center,
            Right,
            RightOne,
            RightTwo,
            Left,
            LeftOne,
            LeftTwo,
            Start,
            End,
            One,
            Two,
            Three,
            Four,
            Five,
            Six,
            Seven,
            Eight,
            Nine,
            Ten,
            Eleven
        }

        public const int DefaultWidth = 390;
        public const int DefaultWidthLarge = 1200;
        public const int DefaultHeight = 60;
        public const int DefaultHeightLarge = 90;
        public const float ScaleFactor = 0.7F;
        public const float PlaceOne = 200 * ScaleFactor;
        public const float DeltaV = 100 * ScaleFactor;
        public const float DefaultFont = 18F * ScaleFactor;
        public const float DefaultFontLarge = 24F * ScaleFactor;
        public const string DefaultText = "DefaultText";

        public static Control draw(Form thisForm, Type type, string text = null, string name = DefaultText, 
        //public static Control draw<T>(Form thisForm, string text = null, string name = DefaultText, 
            float font = DefaultFont, Color color = new Color(), 
            int width = DefaultWidth, int height = DefaultHeight, bool autoSize = true,
            //List<JsonElement> items = null, List<string> sitems = null,
            List<string> items = null,
            EventHandler eventHandler = null, LinkLabelLinkClickedEventHandler linkLabelLinkClickedEventHandler = null,
            Place placeh = Place.Center, Place placev = Place.Center)
        {
            //control = new System.Windows.Forms.RichTextBox();
            Control control = type.GetConstructor(new Type[] { }).Invoke(null) as Control;
            thisForm.Controls.Add(control);
            //textBox.AutoSize = true;
            control.Anchor = ((System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Left));//((System.Windows.Forms.AnchorStyles.Right) | (System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Bottom));
            control.Margin = new System.Windows.Forms.Padding(4);
            control.Size = new System.Drawing.Size(width, height);
            control.Scale(new SizeF(ScaleFactor, ScaleFactor));
            control.TabIndex = 1;
            control.Text = text;
            //control.Name = name;
            //if (name == null)
            //    control.Name = text;
            control.Font = new System.Drawing.Font("Segoe UI", font, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);

            if ((type == typeof(PictureBox)) || (type == typeof(LinkLabel)))
                control.AutoSize = true;
            if (type == typeof(Label))
            {
                if (color != Color.Empty)
                    control.ForeColor = color;
                control.AutoSize = autoSize;
            }
            if (type == typeof(PictureBox))
                (control as PictureBox).Image = global::WinAMBurner.Properties.Resources.ARmentaSmall;
            if (type == typeof(RichTextBox))
            {
                control.Name = name;
                if (control.Name == DefaultText)
                    control.ForeColor = Color.Silver;
                control.Enter += new System.EventHandler(richTextBoxEnter_Click);
                control.Leave += new System.EventHandler(richTextBoxLeave_Click);
                (control as RichTextBox).AutoWordSelection = true;
            }
            if (type == typeof(Button))
                if (eventHandler != null)
                    control.Click += eventHandler;
            if (type == typeof(LinkLabel))
                if (linkLabelLinkClickedEventHandler != null)
                    (control as LinkLabel).LinkClicked += linkLabelLinkClickedEventHandler;
            if (type == typeof(ComboBox))
            {
                if (items != null)
                    (control as ComboBox).Items.AddRange(items.ToArray());
                //(control as ComboBox).SelectedItem = items.Where(i => i == text);
                (control as ComboBox).SelectedIndexChanged += eventHandler;
            }

            control.Location = placeCalc(thisForm, control, placeh: placeh, placev: placev);

            return control;
        }

        public static void hide(Form thisForm)
        {
            while(thisForm.Controls.Count > 0)
                thisForm.Controls[0].Dispose();
        }

        public static Point placeCalc(Form thisForm,
                Control control, Point location = new System.Drawing.Point(), Place placeh = Place.Center, Place placev = Place.Center)
        {
            if (placeh == Place.Center)
                location.X = thisForm.Width / 2 - control.Width / 2;
            else if (placeh == Place.Right)
                location.X = thisForm.Width / 2 + control.Width / 2 + control.Width / 4;
            else if (placeh == Place.RightOne)
                location.X = thisForm.Width / 2 + control.Width / 4 / 2;
            else if (placeh == Place.RightTwo)
                location.X = thisForm.Width / 2 + control.Width + control.Width / 4 / 2 + control.Width / 4;
            else if (placeh == Place.Left)
                location.X = thisForm.Width / 2 - control.Width / 2 - control.Width - control.Width / 4;
            else if (placeh == Place.LeftOne)
                location.X = thisForm.Width / 2 - control.Width - control.Width / 4 / 2;
            else if (placeh == Place.LeftTwo)
                location.X = thisForm.Width / 2 - control.Width * 2 - control.Width / 4 / 2 - control.Width / 4;
            else if (placeh == Place.Start)
                location.X = thisForm.Width / 2 - control.Width / 2 - control.Width * 2 - control.Width * 2 / 4;
            else if (placeh == Place.End)
                location.X = thisForm.Width / 2 + control.Width / 2 + control.Width + control.Width * 2 / 4;

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

        public static void labelDraw(Form thisForm, ref Label label, string text, bool autoSize = true, Font font = null,
            Color color = new System.Drawing.Color(), Place placeh = Place.Center, Place placev = Place.Center)
        {
            label = new System.Windows.Forms.Label();
            thisForm.Controls.Add(label);
            label.AutoSize = autoSize;
            label.Anchor = ((System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Left));//((System.Windows.Forms.AnchorStyles.Right) | (System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Bottom));
            if (font != null)
                label.Font = font;
            if (color != Color.Empty)
                label.ForeColor = color;
            label.Size = new System.Drawing.Size(DefaultWidth, DefaultHeight);
            label.Scale(new SizeF(ScaleFactor, ScaleFactor));
            label.TabIndex = 20;
            label.Text = text;
            label.Font = new System.Drawing.Font("Segoe UI", DefaultFont, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label.Location = placeCalc(thisForm, label, placeh: placeh, placev: placev);
        }

        public static void buttonDraw(Form thisForm, ref Button button, string text, EventHandler eventHandler, Place placeh = Place.Center, Place placev = Place.Center)
        {
            button = new System.Windows.Forms.Button();
            thisForm.Controls.Add(button);
            //button.AutoSize = true;
            button.Anchor = ((System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Left));//((System.Windows.Forms.AnchorStyles.Right) | (System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Bottom));
            button.Margin = new System.Windows.Forms.Padding(4);
            button.Size = new System.Drawing.Size(DefaultWidth, DefaultHeight);
            button.Scale(new SizeF(ScaleFactor, ScaleFactor));
            button.TabIndex = 3;
            button.Text = text;
            button.Font = new System.Drawing.Font("Segoe UI", DefaultFont, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            button.Location = placeCalc(thisForm, button, placeh: placeh, placev: placev);
            button.UseVisualStyleBackColor = true;
            button.Click += eventHandler;
        }

        public static void linkLabelDraw(Form thisForm, ref LinkLabel linkLabel, string text, LinkLabelLinkClickedEventHandler eventHandler,
            Place placeh = Place.Center, Place placev = Place.Center)
        {
            linkLabel = new System.Windows.Forms.LinkLabel();
            thisForm.Controls.Add(linkLabel);
            linkLabel.AutoSize = true;
            linkLabel.Anchor = ((System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Left));//((System.Windows.Forms.AnchorStyles.Right) | (System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Bottom));
            linkLabel.Size = new System.Drawing.Size(DefaultWidth, DefaultHeight);
            linkLabel.Scale(new SizeF(ScaleFactor, ScaleFactor));
            linkLabel.TabIndex = 1;
            linkLabel.TabStop = true;
            linkLabel.Text = text;
            linkLabel.Font = new System.Drawing.Font("Segoe UI", DefaultFont, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            linkLabel.Location = placeCalc(thisForm, linkLabel, placeh: placeh, placev: placev);
            linkLabel.LinkClicked += eventHandler;
        }

        public static void textBoxDraw(Form thisForm, ref RichTextBox textBox, string text, Place placeh = Place.Center, Place placev = Place.Center)
        {
            textBox = new System.Windows.Forms.RichTextBox();
            thisForm.Controls.Add(textBox);
            //textBox.AutoSize = true;
            textBox.Anchor = ((System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Left));//((System.Windows.Forms.AnchorStyles.Right) | (System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Bottom));
            textBox.Margin = new System.Windows.Forms.Padding(4);
            textBox.Size = new System.Drawing.Size(DefaultWidthLarge, DefaultHeight);
            textBox.Scale(new SizeF(ScaleFactor, ScaleFactor));
            textBox.TabIndex = 1;
            textBox.Text = text;
            textBox.Name = text;
            textBox.Font = new System.Drawing.Font("Segoe UI", DefaultFont, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            textBox.ForeColor = Color.Silver;
            textBox.Location = placeCalc(thisForm, textBox, placeh: placeh, placev: placev);
            textBox.Enter += new System.EventHandler(richTextBoxEnter_Click);
            textBox.Leave += new System.EventHandler(richTextBoxLeave_Click);
        }

        public static void textBoxSmallDraw(Form thisForm, ref RichTextBox textBox, string text, string name, Place placeh = Place.Center, Place placev = Place.Center)
        {
            textBox = new System.Windows.Forms.RichTextBox();
            thisForm.Controls.Add(textBox);
            //textBox.AutoSize = true;
            textBox.Anchor = ((System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Left));//((System.Windows.Forms.AnchorStyles.Right) | (System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Bottom));
            textBox.Margin = new System.Windows.Forms.Padding(4);
            textBox.Size = new System.Drawing.Size(DefaultWidth, DefaultHeight);
            textBox.Scale(new SizeF(ScaleFactor, ScaleFactor));
            textBox.TabIndex = 1;
            textBox.Text = text;
            textBox.Name = name;
            textBox.Font = new System.Drawing.Font("Segoe UI", DefaultFont, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            if (name == text)
                textBox.ForeColor = Color.Silver;
            textBox.Location = placeCalc(thisForm, textBox, placeh: placeh, placev: placev);
            textBox.Enter += new System.EventHandler(richTextBoxEnter_Click);
            textBox.Leave += new System.EventHandler(richTextBoxLeave_Click);
        }

        public static void comboBoxSmallDraw(Form thisForm, ref ComboBox comboBox, string text, List<string> items, EventHandler eventHandler = null, Place placeh = Place.Center, Place placev = Place.Center)
        {
            comboBox = new System.Windows.Forms.ComboBox();
            thisForm.Controls.Add(comboBox);
            //comboBox.AutoSize = true;
            comboBox.Anchor = ((System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Left));//((System.Windows.Forms.AnchorStyles.Right) | (System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Bottom));
            comboBox.FormattingEnabled = true;
            comboBox.Margin = new System.Windows.Forms.Padding(4);
            comboBox.Size = new System.Drawing.Size(DefaultWidth, DefaultHeight);
            comboBox.Scale(new SizeF(ScaleFactor, ScaleFactor));
            comboBox.Font = new System.Drawing.Font("Segoe UI", DefaultFont, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            comboBox.Location = placeCalc(thisForm, comboBox, placeh: placeh, placev: placev);
            comboBox.TabIndex = 17;
            comboBox.SelectedIndexChanged += eventHandler;
            comboBox.Items.AddRange(items.ToArray());
            comboBox.Text = text;
        }

        public static void comboBoxSmallDraw1(Form thisForm, ref ComboBox comboBox, string text, List<JsonElement> items, EventHandler eventHandler, Place placeh = Place.Center, Place placev = Place.Center)
        {
            comboBox = new System.Windows.Forms.ComboBox();
            thisForm.Controls.Add(comboBox);
            //comboBox.AutoSize = true;
            comboBox.Anchor = ((System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Left));//((System.Windows.Forms.AnchorStyles.Right) | (System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Bottom));
            comboBox.FormattingEnabled = true;
            comboBox.Margin = new System.Windows.Forms.Padding(4);
            comboBox.Size = new System.Drawing.Size(DefaultWidth, DefaultHeight);
            comboBox.Scale(new SizeF(ScaleFactor, ScaleFactor));
            comboBox.Font = new System.Drawing.Font("Segoe UI", DefaultFont, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            comboBox.Location = placeCalc(thisForm, comboBox, placeh: placeh, placev: placev);
            comboBox.TabIndex = 17;
            comboBox.SelectedIndexChanged += eventHandler;
            comboBox.Items.AddRange(items.Select(i => i.ToString()).ToArray());
            comboBox.Text = text;
        }

        public static void comboBoxDraw(Form thisForm, ref ComboBox comboBox, EventHandler eventHandler, Place placeh = Place.Center, Place placev = Place.Center)
        {
            comboBox = new System.Windows.Forms.ComboBox();
            thisForm.Controls.Add(comboBox);
            //comboBox.AutoSize = true;
            comboBox.Anchor = ((System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Left));//((System.Windows.Forms.AnchorStyles.Right) | (System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Bottom));
            comboBox.FormattingEnabled = true;
            comboBox.Margin = new System.Windows.Forms.Padding(4);
            comboBox.Size = new System.Drawing.Size(DefaultWidthLarge, DefaultHeightLarge);
            comboBox.Scale(new SizeF(ScaleFactor, ScaleFactor));
            comboBox.Font = new System.Drawing.Font("Segoe UI", DefaultFont, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            comboBox.Location = placeCalc(thisForm, comboBox, placeh: placeh, placev: placev);
            comboBox.TabIndex = 17;
            comboBox.SelectedIndexChanged += eventHandler;
        }

        public static void dataGridDraw(Form thisForm, ref DataGridView dataGridView, Place placeh = Place.Center, Place placev = Place.Center)
        {
            //this.components = new System.ComponentModel.Container();
            //this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            dataGridView = new DataGridView();
            //((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(dataGridView)).BeginInit();
            //dataGridView.AutoSize = true;
            dataGridView.Anchor = (AnchorStyles.Top) | (AnchorStyles.Left);//((System.Windows.Forms.AnchorStyles.Right) | (System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Bottom));
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

        public static void labelDraw(Form thisForm, Label label, Point location, string text, Color color, Place placeh, Place placev)
        {
            label.ForeColor = color;
            label.Text = text;
            label.Font = new System.Drawing.Font("Segoe UI", DefaultFont, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label.Location = placeCalc(thisForm, label, location: location, placeh: placeh, placev: placev);
        }

        public static void progressBarDraw(Form thisForm, ref ProgressBar progressBar, Place placeh = Place.Center, Place placev = Place.Center)
        {
            progressBar = new System.Windows.Forms.ProgressBar();
            thisForm.Controls.Add(progressBar);
            //progressBar.AutoSize = true;
            progressBar.Anchor = ((System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Left));//((System.Windows.Forms.AnchorStyles.Right) | (System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Bottom));
            progressBar.Margin = new System.Windows.Forms.Padding(4);
            progressBar.Size = new System.Drawing.Size(DefaultWidthLarge, 24);
            progressBar.Scale(new SizeF(ScaleFactor, ScaleFactor));
            progressBar.Location = placeCalc(thisForm, progressBar, placev: placev);
            progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            progressBar.TabIndex = 18;
            progressBar.Visible = false;
        }

        private static void richTextBoxEnter_Click(object sender, EventArgs e)
        {
            Control control = sender as Control;
            if (control.Name == DefaultText)
            {
                control.Name = control.Text;
                control.Text = "";
                control.ForeColor = Color.Black;
            }
        }

        private static void richTextBoxLeave_Click(object sender, EventArgs e)
        {
            Control control = sender as Control;
            if (control.Text == string.Empty)
            {
                control.Text = control.Name;
                control.Name = DefaultText;
                control.ForeColor = Color.Silver;
            }
        }

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

