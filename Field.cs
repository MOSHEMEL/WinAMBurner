using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WinAMBurner
{
    public enum Place
    {
        None,
        //Center, RightTwo, RightOne, RightThree, LeftFive, LeftSix, LeftFour, Start, End,
        Center, Start, End,
        One, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Eleven
    }

    class Field
    {
        public const int DefaultWidth = 390;
        public const int DefaultWidthLarge = 1200;
        public const int DefaultHeight = 60;
        public const int DefaultHeightSmall = 12;
        public const int DefaultHeightLarge = 90;
        public const float ScaleFactor = 0.5F;
        public const float PlaceOne = 200 * ScaleFactor;
        public const float DeltaV = 100 * ScaleFactor;
        public const float DefaultFont = 24F * ScaleFactor;
        public const float DefaultFontLarge = 30F * ScaleFactor;
        public const string DefaultText = "DefaultText";

        public ErrCode error;
        //public string text;
        public object val;
        public string ltext;
        public string dtext;
        public bool dflt;
        //public string[] items;
        public object[] items;
        public Control control;
        public Control lcontrol;
        public Type type;
        public Type ltype;
        private float font;
        private Color color;
        private int width;
        private int height;
        private bool autosize;
        private Place placeh;
        private Place lplaceh;
        private Place placev;
        private Place lplacev;
        //public Field depend;
        public EventHandler comboEventHandler;
        private EventHandler textEventHandler;
        public LinkLabelLinkClickedEventHandler linkEventHandler;
        public EventHandler radioEventHandler;
        public EventHandler buttonEventHandler;
        public delegate bool fCheck(Field field);
        public fCheck fcheck;
        //public delegate bool pCheck(string param);
        public delegate bool pCheck(object param);
        public pCheck pcheck;
        public bool view;
        //public bool clear;
        public bool enable;

        private string combotext;

        public Field(Type type = null, Type ltype = null, string dtext = null, object val = null, string ltext = null, object[] items = null,
            LinkLabelLinkClickedEventHandler linkEventHandler = null,
            EventHandler comboEventHandler = null,
            EventHandler buttonEventHandler = null,
            EventHandler textEventHandler = null,
            EventHandler radioEventHandler = null,
            fCheck fcheck = null, pCheck pcheck = null,
            float font = DefaultFont, Color color = new Color(),
            int width = DefaultWidth, int height = DefaultHeight, bool autosize = true,
            Place placeh = Place.Center, Place lplaceh = Place.Center, Place placev = Place.None, Place lplacev = Place.None)
        {
            this.dtext = dtext;
            this.val = this.dtext;
            this.dflt = true;
            //this.text = dtext;
            this.ltext = ltext;
            this.items = items;
            this.type = type;
            this.ltype = ltype;
            this.font = font;
            this.color = color;
            this.width = width;
            this.height = height;
            this.autosize = autosize;
            this.placeh = placeh;
            this.lplaceh = lplaceh;
            this.placev = placev;
            if (lplacev == Place.None)
                this.lplacev = placev;
            else
                this.lplacev = lplacev;

            this.linkEventHandler = linkEventHandler;
            this.buttonEventHandler = buttonEventHandler;
            this.textEventHandler = textEventHandler;
            //this.comboEventHandler = comboBox_SelectedIndexChanged;
            this.comboEventHandler = comboEventHandler;
            this.radioEventHandler = radioEventHandler;
            if (fcheck != null)
                this.fcheck = fcheck;
            else
                this.fcheck += checkValid;
            if (pcheck != null)
                this.pcheck = pcheck;
            else
                this.pcheck += checkValid;
            this.view = true;
            //this.clear = false;
            this.enable = true;
        }

        //public static bool setField(ref Field field, Field value)
        //{
        //    if (value.fcheck(value))
        //    {
        //        field = value;
        //        //field.error = ErrCode.OK;
        //        return true;
        //    }
        //    else
        //    {
        //        field = value;
        //        //field.error = ErrCode.EPARAM;
        //        return false;
        //    }
        //}

        //public static Field getField(Field field, string param)
        //{
        //    if (field.pcheck(param))
        //    {
        //        field.text = param;
        //        field.dflag = false;
        //    }
        //    //field.error = ErrCode.OK;
        //    return field;
        //}

        //public static T setField<T>(ref Field field, Field value, T param)
        //{
        //    if (value.fcheck(value))
        //    {
        //        field = value;
        //        T p = (T)field.val;
        //        if (checkType<T>(value, p))
        //            param = p;
        //    }
        //    return param;
        //}

        //public static Field setField(Field field, Field value)
        //{
        //    if (value.fcheck(value))
        //        field = value;
        //    return field;
        //}

        //public static T setParam<T>(Field field, T param)
        //{
        //    if (field.error == ErrCode.OK)
        //    {
        //        param = (T)field.val;
        //        checkType(field, param);
        //    }
        //    return param;
        //}

        //public static T setField<T>(ref Field field, Field value, T param)
        //{
        //    if (value != null)
        //    {
        //        field = value;
        //        if (value.fcheck(value))
        //        {
        //            T p = (T)field.val;
        //            if (checkType<T>(field, p))
        //                param = p;
        //        }
        //    }
        //    return param;
        //}

        public static object setField(ref Field field, Field value, object param)
        {
            field = value;
            if (field != null)
            {
                if (field.val != field.dtext)
                    param = field.val;

                if (field.fcheck(value) && field.pcheck(param))
                    field.error = ErrCode.OK;
                else
                    field.error = ErrCode.EPARAM;
            }
            else
                param = null;
            return param;

        }

        public static Field getField(Field field, object param)
        {
            if (field != null)
            {
                field.val = param;
                if (field.pcheck(param))
                    field.dflt = false;
                else
                {
                    field.dflt = true;
                    field.val = field.dtext;
                }
            }
            return field;
        }

        public static object setObject(Field field, ref object entity, object value, object param)
        {
            if (field != null)
            {
                entity = value;
                //if (field.pcheck(value))
                //{
                Farm farm;
                Service service;
                TreatmentPackage treatmentPackage;

                farm = field.items.FirstOrDefault() as Farm;
                service = field.items.FirstOrDefault() as Service;
                treatmentPackage = field.items.FirstOrDefault() as TreatmentPackage;
                if (farm != null)
                {
                    farm = field.items.Cast<Farm>().ToList().Find(i => i.Name.val as string == value as string);
                    if (farm != null)
                        param = farm.Id;
                }
                else if (service != null)
                {
                    service = field.items.Cast<Service>().ToList().Find(i => i.Name.val as string == value as string);
                    if (service != null)
                        param = service.Id;
                }
                else if (treatmentPackage != null)
                {
                    treatmentPackage = field.items.Cast<TreatmentPackage>().ToList().Find(i => i.description == value as string);
                    param = treatmentPackage.part_number;
                }
                else
                    param = null;
                //if (field.clear)
                //{
                //    entity = value;
                //    param = null;
                //}
                //}
            }
            return param;
        }

        public static object getObject(Field field, object entity, object param)
        {
            if (field != null)
            {
                //if (field.pcheck(param))
                //{
                Farm farm = entity as Farm;
                Service service = entity as Service;
                TreatmentPackage treatmentPackage = entity as TreatmentPackage;
                if (farm != null)
                    farm.Id = param as string;
                if (service != null)
                    service.Id = param as string;
                if (treatmentPackage != null)
                    treatmentPackage.part_number = param as string;
                //}
            }
            return entity;
        }

        public static bool checkValid(Field field)
        {
            //if ((field.text != null) && (field.text != field.dtext) && (field.text != string.Empty))
            //if ((field != null) && (field.text != null) && (field.text != string.Empty) && (!field.dflag))
            if ((field != null) && (field.val != null) && (field.val as string != string.Empty) && (field.val != field.dtext))
            {
                //field.error = ErrCode.OK;
                return true;
            }
            //field.error = ErrCode.EPARAM;
            return false;
        }

        public static bool checkValid(object param)
        {
            if (param != null)
            {
                if (param != string.Empty)
                    return true;
            }
            return false;
        }

        public static bool icheckValid(object param)
        {
            if (param != null)
            {
                int iparam = stringToInt(param as string);
                if ((param != string.Empty) && (iparam > 0))
                    return true;
            }
            return false;
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
            if (sVal.Equals("Yes"))
                return true;
            else
                return false;
        }

        public static int stringToInt(string sVal)
        {
            int iVal = 0;
            int.TryParse(sVal, out iVal);
            return iVal;
        }

        //public static int stringToInt(string sVal)
        //{
        //    return (int)stringToIntOrNull(sVal);
        //}

        public static int? stringToIntOrNull(string sVal)
        {
            if (sVal != null)
            {
                int iVal = 0;
                int.TryParse(sVal, out iVal);
                return iVal;
            }
            return null;
        }

        public static string intToString(int? iVal)
        {
            if (iVal != null)
                return iVal.ToString();
            return null;
        }

        public void updateField(object val)
        {
                this.val = val;
        }

        public void updateField()
        {
            if (control != null)
            {
                control.Text = control.Text.Trim();
                val = control.Text;

                //ComboBox comboBox = control as ComboBox;
                //if ((comboBox != null) && (comboBox.SelectedItem != null))
                //{
                //    val = comboBox.SelectedItem;
                //}
            }
        }

        public ErrCode checkField()
        {
            if (control != null)
            {
                if (error == ErrCode.EPARAM)
                {
                    control.ForeColor = Color.Red;
                    control.Text = dtext;
                    dflt = true;
                    return ErrCode.EPARAM;
                }
            }
            return ErrCode.OK;
        }

    //public string getText()
    //{
    //    if (control != null)
    //    {
    //        updateField();
    //        return text;
    //    }
    //    return null;
    //}

    //public void setDefault()
    //{
    //    dflag = true;
    //}

    //private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    ComboBox comboBox = sender as ComboBox;
    //    if(comboBox != null)
    //    { 
    //        checkState(depend); 
    //    }
    //}

    //public bool checkState(Field depend)
    //{
    //    if(depend != null)
    //    {
    //        if (control.Text.Contains("United States of America"))
    //            depend.control.Enabled = true;
    //        else
    //            depend.control.Enabled = false;
    //        return depend.control.Enabled;
    //    }
    //    return false;
    //}

    public Control draw(Form thisForm, bool readnly)
        {
            if(readnly)
            {
                lcontrol = draw(thisForm, ltype, val: ltext, placeh: lplaceh, placev: lplacev);
                return lcontrol;
            }
            else
            {
                control = draw(thisForm, type, val: val, placeh: placeh, placev: placev);
                return control;
            }
        }

        //public Control draw(Form thisForm)
        //{
        //    control = draw(thisForm, type, text: text, placeh: placeh, placev: placev);
        //    return control;
        //}

        //public Control draw(Form thisForm, bool autosize)
        //{
        //    lcontrol = draw(thisForm, ltype, text: ltext, autosize: autosize, placeh: lplaceh, placev: lplacev);
        //    return lcontrol;
        //}

        //public Control draw(Form thisForm, Type type, string text = null, string name = DefaultText,
        public Control draw(Form thisForm, Type type, object val = null,
            //float font = DefaultFont, Color color = new Color(), bool autoSize = true,
            //bool autosize = true,
            //int width = DefaultWidth, int height = DefaultHeight, bool autoSize = true,
            Place placeh = Place.Center, Place placev = Place.Center)
        {
            Control control = null;
            if (type != null)
            {
                control = type.GetConstructor(new Type[] { }).Invoke(null) as Control;
                thisForm.Controls.Add(control);
                control.Anchor = (AnchorStyles.Top);// | (AnchorStyles.Left);
                control.Margin = new Padding(4);
                control.Size = new Size(width, height);
                control.Scale(new SizeF(ScaleFactor, ScaleFactor));
                control.TabIndex = 1;
                control.Text = val as string;
                control.Font = new Font("Segoe UI", font, FontStyle.Regular, GraphicsUnit.Point);

                //if ((type == typeof(PictureBox)) || (type == typeof(LinkLabel)))
                //    control.AutoSize = true;
                if (type == typeof(Label))
                {
                    if (color != Color.Empty)
                        control.ForeColor = color;
                    control.AutoSize = autosize;
                }
                if (type == typeof(PictureBox))
                {
                    PictureBox pictureBox = (control as PictureBox);
                    if (pictureBox != null)
                    {
                        control.AutoSize = true;
                        pictureBox.Image = Properties.Resources.ARmentaSmall;
                    }
                }
                if (type == typeof(RichTextBox))
                {
                    RichTextBox richTextBox = control as RichTextBox;
                    if (richTextBox != null)
                    {
                        richTextBox.Multiline = false;
                        if (textEventHandler != null)
                            richTextBox.TextChanged += textEventHandler;
                        defaultText(control);
                    }
                }
                if (type == typeof(TextBox))
                {
                    TextBox textBox = control as TextBox;
                    if (textBox != null)
                    {
                        textBox.Multiline = false;
                        defaultText(control);
                    }
                }
                if (type == typeof(Button))
                    if (buttonEventHandler != null)
                        control.Click += buttonEventHandler;
                if (type == typeof(LinkLabel))
                {
                    LinkLabel linkLabel = control as LinkLabel;
                    if (linkLabel != null)
                    {
                        control.AutoSize = true;
                        if (linkEventHandler != null)
                            linkLabel.LinkClicked += linkEventHandler;
                        if (buttonEventHandler != null)
                            linkLabel.Click += buttonEventHandler;
                    }
                }
                if (type == typeof(ComboBox))
                {
                    ComboBox comboBox = (control as ComboBox);
                    if (comboBox != null)
                    {
                        comboBox.DropDownHeight = DefaultWidth;
                        if (items != null)
                            comboBox.Items.AddRange(items);
                        if (comboEventHandler != null)
                            comboBox.SelectedIndexChanged += comboEventHandler;
                        comboBox.TextUpdate += comboBox_TextUpdate;
                        //comboBox.KeyPress += comboBox_KeyPress;
                        //comboBox.KeyPress += comboBox_KeyPress;
                        //comboBox.EnabledChanged += comboBox_EnabledChanged;
                        //comboBox.DisplayMember = type.Name;
                        //comboBox.TextUpdate += comboBox_TextChanged;
                        defaultText(control);
                    }
                }
                if (type == typeof(RadioButton))
                {
                    RadioButton radioButton = control as RadioButton;
                    if (radioButton != null)
                        if (radioEventHandler != null)
                            radioButton.CheckedChanged += radioEventHandler;
                }

                control.Location = placeCalc(thisForm, control, placeh: placeh, placev: placev);
                control.Enabled = enable;
            }
            return control;
        }

        //private void defaultText(string name, Control control)
        private void defaultText(Control control)
        {
            //control.Name = name;
            //if (control.Name == DefaultText)
            //{
            //if (control.Text == dflt)
            //if (text == dtext)
            if (dflt)
                control.ForeColor = Color.Silver;
            else
                control.ForeColor = Color.Black;
            control.Enter += new EventHandler(controlEnter_Click);
            control.Leave += new EventHandler(controlLeave_Click);
            //}
        }

        public static Point placeCalc(Form thisForm,
                Control control, Point location = new System.Drawing.Point(), Place placeh = Place.Center, Place placev = Place.Center)
        {
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

        private void controlEnter_Click(object sender, EventArgs e)
        {
            Control control = sender as Control;
            TextBox textBox = sender as TextBox;
            if (control != null)
            {
                //if (control.Name == DefaultText)
                //if (control.Text == dtext)
                if (dflt)
                {
                    //    string dflt = control.Text;
                    control.Text = string.Empty;
                    //control.Name = string.Empty;
                    control.ForeColor = Color.Black;
                    if (textBox != null)
                        textBox.PasswordChar = '*';
                    dflt = false;
                }
            }
        }

        private void controlLeave_Click(object sender, EventArgs e)
        {
            Control control = sender as Control;
            TextBox textBox = sender as TextBox;
            if (control != null)
            {
                if (control.Text == string.Empty)
                {
                    //    string dflt = control.Name;
                    //control.Name = dflt;
                    control.Text = dtext;
                    control.ForeColor = Color.Silver;
                    if (textBox != null)
                        textBox.PasswordChar = '\0';
                    dflt = true;
                }
            }
        }

        private void comboBox_TextUpdate(object sender, EventArgs e)
        //private void comboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            //if ((comboBox != null) && (comboBox.Text != null) && (comboBox.Items != null) && (items != null))
            if ((comboBox != null) && (items != null))
            {
                try
                {
                    //combotext = comboBox.Text + e.KeyChar;
                    combotext = comboBox.Text;
                    removeItems();
                    //addItems(items.Where(s => s.ToString().ToLower().StartsWith(comboBox.Text.ToLower())).ToArray());
                    addItems(items.Where(s => s.ToString().ToLower().StartsWith(combotext.ToLower())).ToArray());
                    //comboBox.DropDownHeight = DefaultWidthLarge;
                    comboBox.DroppedDown = true;
                    //comboBox.Text = text;
                }
                catch(Exception ex)
                {
                    LogFile.logWrite(ex.ToString());
                }
            }
        }

        //private void comboBox_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    ComboBox comboBox = sender as ComboBox;
        //    if (comboBox != null)
        //        comboBox.DroppedDown = true;
        //}

        //private void comboBox_EnabledChanged(object sender, EventArgs e)
        //{
        //    ComboBox comboBox = sender as ComboBox;
        //    if ((comboBox != null) && (items != null))
        //    {
        //        //if (comboBox.Enabled)
        //        //    //comboBox.Items.AddRange(items);
        //        //    addItems(items);
        //        //else
        //        //    removeItems();
        //    }
        //}

        //private void addComboItems(object[] items)
        //{
        //    ComboBox comboBox = control as ComboBox;
        //    if (comboBox != null)
        //    {
        //        //field.items = items.Select(s => s.ToString()).ToArray();
        //        dflag = true;
        //        //items = items;
        //        //field.setDefault();
        //        comboBox.Items.AddRange(items);
        //        //comboBox.Text = dtext;
        //    }
        //}

        //private void removeComboItems()
        //{
        //    ComboBox comboBox = control as ComboBox;
        //    if (comboBox != null)
        //    {
        //        //while (comboBox.Items.Count > 0)
        //        //    comboBox.Items.RemoveAt(0);
        //        dflag = true;
        //        //items = null;
        //        while (comboBox.Items.Count > 0)
        //            comboBox.Items.RemoveAt(0);
        //        //comboBox.Text = dtext;
        //    }
        //}

        public void addItems()
        {
            ComboBox comboBox = control as ComboBox;
            if (comboBox != null)
            {
                //field.items = items.Select(s => s.ToString()).ToArray();
                dflt = true;
                //items = items;
                //field.setDefault();
                comboBox.Items.AddRange(items);
                //comboBox.Text = field.dtext;
            }
        }

        public void addItems(object[] items)
        {
            ComboBox comboBox = control as ComboBox;
            if (comboBox != null)
            {
                //field.items = items.Select(s => s.ToString()).ToArray();
                dflt = true;
                //items = items;
                //field.setDefault();
                comboBox.Items.AddRange(items);
                //comboBox.Text = field.dtext;
            }
        }

        public void removeItems()
        {
            ComboBox comboBox = control as ComboBox;
            if (comboBox != null)
            {
                //field.setDefault();
                dflt = true;
                //items = null;
                while (comboBox.Items.Count > 0)
                    comboBox.Items.RemoveAt(0);
                //comboBox.Text = field.dtext;
            }
        }

        //public static void hide(Form thisForm)
        //{
        //    while (thisForm.Controls.Count > 0)
        //        thisForm.Controls[0].Dispose();
        //}

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

        //internal void setEventHandler(EventHandler comboEventHandler)
        //{
        //    if (type == typeof(ComboBox))
        //        this.comboEventHandler = comboEventHandler;
        //}
    }
}
