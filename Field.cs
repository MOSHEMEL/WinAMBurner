using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WinAMBurner
{
    public enum Place
    {
        None,
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
        //public object text;
        public string val;
        public string ltext;
        public string dflt;
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
        public EventHandler eventHandler;
        //private EventHandler textEventHandler;
        public LinkLabelLinkClickedEventHandler linkEventHandler;
        //public EventHandler radioEventHandler;
        //public EventHandler buttonEventHandler;
        //public delegate bool fCheck();
        //public fCheck fcheck;
        //public delegate bool pCheck(string param);
        //public pCheck pcheck;
        public bool view;
        public bool enable;

        public Field(Type type = null, Type ltype = null, string dflt = null, object val = null, string ltext = null, object[] items = null,
            LinkLabelLinkClickedEventHandler linkEventHandler = null,
            EventHandler eventHandler = null,
            //EventHandler buttonEventHandler = null,
            //EventHandler textEventHandler = null,
            //EventHandler radioEventHandler = null,
            //fCheck fcheck = null, pCheck pcheck = null,
            bool enable = true,
            float font = DefaultFont, Color color = new Color(),
            int width = DefaultWidth, int height = DefaultHeight, bool autosize = true,
            Place placeh = Place.Center, Place lplaceh = Place.Center, Place placev = Place.None, Place lplacev = Place.None)
        {
            this.dflt = dflt;
            //this.text = this.dflt;
            this.val = this.dflt;
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
            //this.buttonEventHandler = buttonEventHandler;
            //this.textEventHandler = textEventHandler;
            this.eventHandler = eventHandler;
            //this.radioEventHandler = radioEventHandler;
            //if (fcheck != null)
            //    this.fcheck = fcheck;
            //else
            //    this.fcheck = checkValid;
            //if (pcheck != null)
            //    this.pcheck = pcheck;
            //else
            //    this.pcheck = checkValid;
            this.view = true;
            this.enable = enable;
        }

        public void setValue(string value)
        {
            //if (pcheck(value))
            if (checkValid(value))
                val = value;
            else
                val = dflt;
        }

        public string getValue()
        {
            //if (fcheck())
            if (checkValid())
                error = ErrCode.OK;
            else
            {
                error = ErrCode.EPARAM;
                return null;
            }
            return val;
        }

        //public static object setField(ref Field field, Field value, object param)
        //{
        //    field = value;
        //    if (field != null)
        //    {
        //        if ((field.val as string) != field.dflt)
        //            param = field.val;
        //        else
        //            param = null;
        //
        //        if (field.fcheck(value) && field.pcheck(param))
        //            field.error = ErrCode.OK;
        //        else
        //            field.error = ErrCode.EPARAM;
        //    }
        //    else
        //        param = null;
        //    return param;
        //}

        //public static Field getField(Field field, object param)
        //{
        //    if (field != null)
        //    {
        //        if (field.pcheck(param))
        //        {
        //            field.val = param;
        //        }
        //        else
        //        {
        //            field.val = field.dflt;
        //        }
        //    }
        //    return field;
        //}

        //public static object setObject(Field field, ref object entity, object value, object param)
        //{
        //    if ((field != null) && (field.items != null))
        //    {
        //        entity = value;
        //        Farm farm;
        //        Service service;
        //        TreatmentPackage treatmentPackage;
        //
        //        farm = field.items.FirstOrDefault() as Farm;
        //        service = field.items.FirstOrDefault() as Service;
        //        treatmentPackage = field.items.FirstOrDefault() as TreatmentPackage;
        //        if (farm != null)
        //        {
        //            farm = field.items.Cast<Farm>().ToList().Find(i => (i.Name.val as string) == (value as string));
        //            if (farm != null)
        //                param = farm.Id;
        //            else
        //                param = null;
        //        }
        //        else if (service != null)
        //        {
        //            service = field.items.Cast<Service>().ToList().Find(i => (i.Name.val as string) == (value as string));
        //            if (service != null)
        //                param = service.Id;
        //            else
        //                param = null;
        //        }
        //        else if (treatmentPackage != null)
        //        {
        //            treatmentPackage = field.items.Cast<TreatmentPackage>().ToList().Find(i => i.description == (value as string));
        //            if (treatmentPackage != null)
        //                param = treatmentPackage.part_number;
        //            else
        //                param = null;
        //        }
        //    }
        //    return param;
        //}

        //public static object getObject(Field field, ref object entity, object param)
        //{
        //    if (field != null)
        //    {
        //        Farm farm = entity as Farm;
        //        Service service = entity as Service;
        //        TreatmentPackage treatmentPackage = entity as TreatmentPackage;
        //        if (farm != null)
        //            farm.Id = param as string;
        //        if (service != null)
        //            service.Id = param as string;
        //        if (treatmentPackage != null)
        //            treatmentPackage.part_number = param as string;
        //    }
        //    return entity;
        //}

        public bool checkValid()
        {
                int ivalue= stringToInt(val);
            if ((val != null) && (val != string.Empty) && (val != dflt) && (ivalue >= 0))
            {
                return true;
            }
            return false;
        }

        public bool checkValid(string param)
        {
            if (param != null)
            {
                if (param != string.Empty)
                    return true;
            }
            return false;
        }
        
        //public bool icheckValid(string param)
        //{
        //    if (param != null)
        //    {
        //        int iparam = stringToInt(param);
        //        if ((param != string.Empty) && (iparam > 0))
        //            return true;
        //    }
        //    return false;
        //}

        //public static bool checkValid(object param)
        //{
        //    if (param != null)
        //    {
        //        if (param != string.Empty)
        //            return true;
        //    }
        //    return false;
        //}
        //
        //public static bool icheckValid(object param)
        //{
        //    if (param != null)
        //    {
        //        int iparam = stringToInt(param as string);
        //        if ((param != string.Empty) && (iparam > 0))
        //            return true;
        //    }
        //    return false;
        //}

        //public int string2Int(string sVal)
        //{
        //    int iVal = 0;
        //    int.TryParse(sVal, out iVal);
        //    return iVal;
        //}

        //public string int2String(int? iVal)
        //{
        //    if (iVal != null)
        //        return iVal.ToString();
        //    return null;
        //}

        //public string bool2String(bool bVal)
        //{
        //    if (bVal)
        //        return "Yes";
        //    else
        //        return "No";
        //}

        //public bool string2Bool(string sVal)
        //{
        //    if (sVal == null)
        //        return false;
        //    if (sVal.Equals("Yes"))
        //        return true;
        //    else
        //        return false;
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
            if (sVal == null)
                return false;
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

        public static int? stringToIntOrNull(string sVal)
        {
            if (sVal != null)
            {
                int iVal = 0;
                if (int.TryParse(sVal, out iVal))
                    return iVal;
                else
                    return null;
            }
            return null;
        }

        public static string intToString(int? iVal)
        {
            if (iVal != null)
                return iVal.ToString();
            return null;
        }

        //public void updateField(object val)
        //{
        //        this.val = val;
        //}

        //public void updateField()
        //{
        //    if (control != null)
        //    {
        //        control.Text = control.Text.Trim();
        //        //val = text;
        //        val = control.Text;
        //    }
        //}

        public ErrCode checkField()
        {
            if (control != null)
            {
                if (error == ErrCode.EPARAM)
                {
                    control.ForeColor = Color.Red;
                    control.Text = dflt;
                    TextBox textBox = control as TextBox;
                    if (textBox != null)
                        textBox.PasswordChar = '\0';
                    return ErrCode.EPARAM;
                }
            }
            return ErrCode.OK;
        }

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

        public Control draw(Form thisForm, Type type, object val = null,
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
                        //if (eventHandler != null)
                        //    richTextBox.TextChanged += eventHandler;
                        //else
                        richTextBox.TextChanged += textBox_TextChanged;
                        defaultText(control);
                    }
                }
                if (type == typeof(TextBox))
                {
                    TextBox textBox = control as TextBox;
                    if (textBox != null)
                    {
                        textBox.Multiline = false;
                        textBox.TextChanged += textBox_TextChanged;
                        defaultText(control);
                    }
                }
                if (type == typeof(Button))
                    if (eventHandler != null)
                        control.Click += eventHandler;
                if (type == typeof(LinkLabel))
                {
                    LinkLabel linkLabel = control as LinkLabel;
                    if (linkLabel != null)
                    {
                        control.AutoSize = true;
                        if (linkEventHandler != null)
                            linkLabel.LinkClicked += linkEventHandler;
                        if (eventHandler != null)
                            linkLabel.Click += eventHandler;
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
                        //if (eventHandler != null)
                        //    comboBox.SelectedIndexChanged += eventHandler;
                        //else
                        comboBox.SelectedIndexChanged += comboBox_SelectedIndexChanged;
                        comboBox.TextUpdate += comboBox_TextUpdate;
                        defaultText(control);
                    }
                }
                if (type == typeof(RadioButton))
                {
                    RadioButton radioButton = control as RadioButton;
                    if (radioButton != null)
                        if (eventHandler != null)
                            radioButton.CheckedChanged += eventHandler;
                }

                control.Location = placeCalc(thisForm, control, placeh: placeh, placev: placev);
                control.Enabled = enable;
            }
            return control;
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            Control control = sender as Control;
            if (control != null)
            {
                val = control.Text;
                if (eventHandler != null)
                    eventHandler(sender, e);
            }
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if(comboBox != null)
            {
                val = comboBox.SelectedItem as string;
                if (eventHandler != null)
                    eventHandler(sender, e);
            }
        }

        private void defaultText(Control control)
        {
            if (control.Text == dflt)
                control.ForeColor = Color.Silver;
            else
                control.ForeColor = Color.Black;
            control.Enter += new EventHandler(controlEnter_Click);
            control.Leave += new EventHandler(controlLeave_Click);
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
                if (control.Text == dflt)
                {
                    control.Text = string.Empty;
                    control.ForeColor = Color.Black;
                    if (textBox != null)
                        textBox.PasswordChar = '*';
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
                    control.Text = dflt;
                    control.ForeColor = Color.Silver;
                    if (textBox != null)
                        textBox.PasswordChar = '\0';
                }
            }
        }

        private void comboBox_TextUpdate(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if ((comboBox != null) && (items != null))
            {
                try
                {
                    string combotext = comboBox.Text;
                    removeItems();
                    addItems(items.Where(s => s.ToString().ToLower().StartsWith(combotext.ToLower())).ToArray());
                    comboBox.DroppedDown = true;
                    comboBox.Text = combotext;
                    comboBox.SelectionStart = comboBox.Text.Count();
                }
                catch(Exception ex)
                {
                    LogFile.logWrite(ex.ToString());
                }
            }
        }

        public void addItems(object[] items)
        {
            ComboBox comboBox = control as ComboBox;
            if (comboBox != null)
            {
                comboBox.Items.AddRange(items);
                comboBox.Text = string.Empty;
                //comboBox.ForeColor = Color.Silver;
                //text = dflt;
                //val = dflt;
            }
        }

        public void removeItems()
        {
            ComboBox comboBox = control as ComboBox;
            if (comboBox != null)
            {
                while (comboBox.Items.Count > 0)
                    comboBox.Items.RemoveAt(0);
                comboBox.Text = string.Empty;
                //comboBox.ForeColor = Color.Silver;
                //text = dflt;
                //val = dflt;
            }
        }

        public static void dataGridDraw(Form thisForm, ref DataGridView dataGridView, Place placeh = Place.Center, Place placev = Place.Center)
        {
            dataGridView = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)(dataGridView)).BeginInit();
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
            ((System.ComponentModel.ISupportInitialize)(dataGridView)).EndInit();
        }
    }
}
