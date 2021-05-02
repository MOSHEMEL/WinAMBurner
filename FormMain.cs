using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Management;
using System.Text;
using System.Windows.Forms;

using System.Data;
using System.Data.SqlClient;

using System.Globalization;
using System.Xml.Linq;
using System.Linq;

namespace WinAMBurner
{
    enum Place
    {
        Center,
        Right,
        Left,
        Start,
        End,
        None
    }

    enum Property
    {
        None
    }

    public partial class FormMain : Form
    {
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.RichTextBox richTextBox3;
        private System.Windows.Forms.RichTextBox richTextBox4;
        private System.Windows.Forms.RichTextBox richTextBox5;
        private System.Windows.Forms.RichTextBox richTextBox6;
        private System.Windows.Forms.RichTextBox richTextBox7;
        private System.Windows.Forms.RichTextBox richTextBox8;
        private System.Windows.Forms.RichTextBox richTextBox9;
        private System.Windows.Forms.RichTextBox richTextBox10;
        private System.Windows.Forms.RichTextBox richTextBox11;
        private System.Windows.Forms.RichTextBox richTextBox12;
        private System.Windows.Forms.RichTextBox richTextBox13;
        private System.Windows.Forms.RichTextBox richTextBox14;
        private System.Windows.Forms.RichTextBox richTextBox15;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.DataGridView dataGridView1;

        private AMData amData = new AMData();

        private bool AMConnected = false;

        public FormMain()
        {
            InitializeComponent();
            screenLoginShow();
        }

        private void formClear()
        {
            AMConnected = false;
            amData.SNum = 0;
            amData.Maxi = 0;
            amData.MaxiSet = 0;
        }

        private void screenLoginHide()
        {
            richTextBox1.Dispose();
            richTextBox2.Dispose();
            label1.Dispose();
            button1.Dispose();
        }
        private void screenLoginShow()
        {
            textBoxDraw(ref richTextBox1, new System.Drawing.Point(360, 260), "Username");
            textBoxDraw(ref richTextBox2, new System.Drawing.Point(360, 440), "Password");
            labelDraw(ref label1, new System.Drawing.Point(360, 620), "Forgot password", font: new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point),
                        color: System.Drawing.Color.Blue);
            buttonDraw(ref button1, new System.Drawing.Point(790, 980), "Login", new EventHandler(buttonLogin_Click), placev: Place.End);
        }

        private void buttonDraw(ref Button button, Point location, string text, EventHandler eventHandler, Place placeh = 0, Place placev = 0)
        {
            button = new System.Windows.Forms.Button();
            Controls.Add(button);
            button.Anchor = System.Windows.Forms.AnchorStyles.Top;
            button.Margin = new System.Windows.Forms.Padding(4);
            button.Size = new System.Drawing.Size(390, 90);
            button.TabIndex = 3;
            button.Text = text;
            button.Location = placeCalc(button, location: location, placeh: placeh, placev: placev);
            button.UseVisualStyleBackColor = true;
            button.Click += eventHandler;
        }

        private Point placeCalc(
            Control control, Point location = new System.Drawing.Point(), Place placeh = Place.Center, Place placev = Place.Center)
        {
            if (placeh == Place.Center)
                location.X = this.Width / 2 - control.Width / 2;
            else if (placeh == Place.Left)
                location.X = this.Width / 2 - control.Width / 2 - control.Width - control.Width / 3;
            else if (placeh == Place.Right)
                location.X = this.Width / 2 - control.Width / 2 + control.Width + control.Width / 3;
            else if (placeh == Place.Start)
                location.X = this.Width / 2 - control.Width / 2 - control.Width - control.Width / 3 - control.Width - control.Width / 3;

            if (placev == Place.End)
                location.Y = this.Height - control.Height * 2;

            return location;
        }

        private void labelDraw(
            ref Label label, Point location, string text, bool autoSize = true,
            Font font = null, Color color = new System.Drawing.Color(), Place placeh = Place.Center)
        {
            label = new System.Windows.Forms.Label();
            Controls.Add(label);
            label.Anchor = System.Windows.Forms.AnchorStyles.Top;
            label.AutoSize = autoSize;
            if (font != null)
                label.Font = font;
            if (color != Color.Empty)
                label.ForeColor = color;
            label.Size = new System.Drawing.Size(390, 90);
            label.TabIndex = 20;
            label.Text = text;
            label.Location = placeCalc(label, location: location, placeh: placeh);
        }

        private void textBoxDraw(ref RichTextBox textBox, Point location, string text)
        {
            textBox = new System.Windows.Forms.RichTextBox();
            Controls.Add(textBox);
            textBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            textBox.Margin = new System.Windows.Forms.Padding(4);
            textBox.Size = new System.Drawing.Size(1200, 90);
            textBox.TabIndex = 1;
            textBox.Text = text;
            textBox.Location = placeCalc(textBox, location: location);
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            // if failed
            FormNotify formNotify = new FormNotify("Login Failed",
                new string[] {
                "Login failed Check your username and password,",
                "make sure your tablet is connected to the internet"},
                NotifyButtons.OK);
            formNotify.ShowDialog();
            formNotify.Dispose();
            // if ok
            screenLoginHide();
            screenActionShow();
        }

        public static string GetBIOSserNo()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");
            foreach (ManagementObject wmi in searcher.Get())
            {
                try
                {
                    return wmi.GetPropertyValue("SerialNumber").ToString();
                }
                catch { }
            }
            return "BIOS Serial Number: Unknown";
        }

        private void screenActionShow()
        {
            labelDraw(ref label1, new System.Drawing.Point(1090, 260), "Welcome distributor",
                        font: new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point));
            labelDraw(ref label2, new System.Drawing.Point(1090, 440), "Choose Action: " + GetBIOSserNo());
            buttonDraw(ref button1, new System.Drawing.Point(1090, 620), "Update AM", new System.EventHandler(buttonUpdateAM_Click));
            buttonDraw(ref button2, new System.Drawing.Point(1090, 800), "Manage Farms", new System.EventHandler(buttonFarm_Click));
            buttonDraw(ref button3, new System.Drawing.Point(1090, 980), "Manage Service provider", new System.EventHandler(buttonService_Click));
        }

        private void buttonUpdateAM_Click(object sender, EventArgs e)
        {
            screenActionHide();
            screenConnectShow();
        }

        private void buttonFarm_Click(object sender, EventArgs e)
        {
            //FormFarm formFarm = new FormFarm();
            //formFarm.ShowDialog();
            //formFarm.Dispose();
            screenActionHide();
            screenFarmShow();
        }

        private void buttonService_Click(object sender, EventArgs e)
        {
            //FormService formService = new FormService();
            //formService.ShowDialog();
            //formService.Dispose();
            screenActionHide();
            screenServiceShow();
        }

        private void screenActionHide()
        {
            label1.Dispose();
            label2.Dispose();
            button1.Dispose();
            button2.Dispose();
            button3.Dispose();
        }

        private void screenConnectShow()
        {
            labelDraw(ref label1, new System.Drawing.Point(360, 260), "Welcome distributor",
                        font: new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point));
            labelDraw(ref label2, new System.Drawing.Point(360, 440), "Please make sure the AM is connected to your tablet before continue");
            labelDraw(ref label3, new System.Drawing.Point(360, 620), "");
            progressBarDraw(ref progressBar1, new System.Drawing.Point(360, 620));
            buttonDraw(ref button1, new System.Drawing.Point(360, 980), "Check AM present", new EventHandler(buttonCheckAM_Click), placeh: Place.Left, placev: Place.End);
            buttonDraw(ref button2, new System.Drawing.Point(1217, 980), "Forward", new EventHandler(buttonConnectForward_Click), placeh: Place.Right, placev: Place.End);
            if (AMConnected)
                AMConnectedShow();
            else
            {
                AMDisconnectedShow();
                label3.Visible = false;
                button2.Enabled = false;
            }
        }

        private async void buttonCheckAM_Click(object sender, EventArgs e)
        {
            label3.Visible = false;
            button1.Enabled = false;
            button2.Enabled = false;
            progressBar1.Visible = true;
            amData.serialPortProgressEvent += new EventHandler(progressBar1_Callback);
            progressBar1.Maximum = 60;
            ErrCode errcode = await amData.AMDataCheckConnect();
            if (errcode >= ErrCode.OK)
                errcode = await amData.AMDataRead();
            progressBar1.Value = progressBar1.Minimum;
            progressBar1.Visible = false;
            if (errcode == ErrCode.OK)
            {
                //if ok
                AMConnected = true;
                AMConnectedShow();
            }
            else
            {
                // if fail
                AMConnected = false;
                AMDisconnectedShow();
                FormNotify formNotify = new FormNotify("Am not connected",
                    new string[]
                    { "AM not found make sure the AM is connected",
                        "to the tablet by using a USB cable" },
                    NotifyButtons.OK);
                formNotify.ShowDialog();
                formNotify.Dispose();
            }
            label3.Visible = true;
            button1.Enabled = true;
        }

        private void AMDisconnectedShow()
        {
            labelDraw(label3, new System.Drawing.Point(360, 620), "AM not found – make sure AM is connected using USB cable", Color.Red, Place.Center);
            button2.Enabled = false;
        }

        private void AMConnectedShow()
        {
            labelDraw(label3, new System.Drawing.Point(360, 620), "AM found – connected to AM", Color.Green, Place.Center);
            button2.Enabled = true;
        }

        private void labelDraw(Label label, Point location, string text, Color color, Place placeh)
        {
            label.ForeColor = color;
            label.Text = text;
            label.Location = placeCalc(label, location: location, placeh: placeh);
        }

        private void buttonConnectForward_Click(object sender, EventArgs e)
        {
            screenConnectHide();
            screenInfoShow();
        }

        private void progressBarDraw(ref ProgressBar progressBar, Point location)
        {
            progressBar = new System.Windows.Forms.ProgressBar();
            Controls.Add(progressBar);
            progressBar.Anchor = System.Windows.Forms.AnchorStyles.Top;
            progressBar.Margin = new System.Windows.Forms.Padding(4);
            progressBar.Size = new System.Drawing.Size(1200, 24);
            progressBar.Location = placeCalc(progressBar, location: location);
            progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            progressBar.TabIndex = 18;
            progressBar.Visible = false;
        }

        private void screenConnectHide()
        {
            label1.Dispose();
            label2.Dispose();
            label3.Dispose();
            button1.Dispose();
            button2.Dispose();
        }

        private void screenInfoShow()
        {
            amData.SNum = 0x123;
            labelDraw(ref label1, new System.Drawing.Point(360, 260), "Welcome distributor",
                        font: new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point));
            labelDraw(ref label2, new System.Drawing.Point(360, 440), "AM information, pulses per treatment : " + 1500);
            labelDraw(ref label3, new System.Drawing.Point(360, 620), "AM identified with SN: " + amData.SNum);
            labelDraw(ref label4, new System.Drawing.Point(360, 800), "Current available treatments: " + amData.Maxi);
            buttonDraw(ref button1, new System.Drawing.Point(360, 980), "Back", new EventHandler(buttonInfoBack_Click), placeh: Place.Left, placev: Place.End);
            buttonDraw(ref button2, new System.Drawing.Point(1217, 980), "Continue", new EventHandler(buttonInfoContinue_Click), placeh: Place.Right, placev: Place.End);
        }

        private void buttonInfoBack_Click(object sender, EventArgs e)
        {
            screenInfoHide();
            screenConnectShow();
        }

        private void buttonInfoContinue_Click(object sender, EventArgs e)
        {
            screenInfoHide();
            screenTreatShow();
        }

        private void screenInfoHide()
        {
            label1.Dispose();
            label2.Dispose();
            label3.Dispose();
            label4.Dispose();
            button1.Dispose();
            button2.Dispose();
        }
        private void screenTreatShow()
        {
            labelDraw(ref label1, new System.Drawing.Point(360, 200), "Welcome distributor",
                        font: new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point));
            labelDraw(ref label2, new System.Drawing.Point(360, 400), "Select Farm / Service provider");
            comboBoxDraw(ref comboBox1, new System.Drawing.Point(360, 500), new System.EventHandler(comboBox1_SelectedIndexChanged));
            comboBox1.Items.AddRange(new string[] { "farm1", "farm2", "farm3", "farm4", "farm5", "farm6" });
            labelDraw(ref label3, new System.Drawing.Point(360, 700), "Add treatments to AM – SN" + amData.SNum);
            comboBoxDraw(ref comboBox2, new System.Drawing.Point(360, 800), new System.EventHandler(comboBox2_SelectedIndexChanged));
            comboBox2.Items.AddRange(new string[] { "50", "100", "150", "200", "250", "300" });
            progressBarDraw(ref progressBar1, new System.Drawing.Point(360, 1000));
            buttonDraw(ref button1, new System.Drawing.Point(360, 1160), "Cancel", new EventHandler(buttonTreatCansel_Click), placeh: Place.Left, placev: Place.End);
            buttonDraw(ref button2, new System.Drawing.Point(1217, 1160), "Approve", new EventHandler(buttonTreatApprove_Click), placeh: Place.Right, placev: Place.End);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBoxDraw(ref ComboBox comboBox, Point location, EventHandler eventHandler, Place placeh = Place.Center)
        {
            comboBox = new System.Windows.Forms.ComboBox();
            Controls.Add(comboBox);
            comboBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            comboBox.FormattingEnabled = true;
            comboBox.Margin = new System.Windows.Forms.Padding(4);
            comboBox.Size = new System.Drawing.Size(1200, 56);
            comboBox.Location = placeCalc(comboBox, location: location, placeh: placeh);
            comboBox.TabIndex = 17;
            comboBox.SelectedIndexChanged += eventHandler;
        }

        private void screenTreatHide()
        {
            label1.Dispose();
            label2.Dispose();
            label3.Dispose();
            comboBox1.Dispose();
            comboBox2.Dispose();
            button1.Dispose();
            button2.Dispose();
        }

        private void progressBar1_Callback(object sender, EventArgs e)
        {
            SerialPortEventArgs args = e as SerialPortEventArgs;
            if ((progressBar1.Value + args.progress) <= progressBar1.Maximum)
                progressBar1.Value += args.progress;
            if ((progressBar1.Value > (progressBar1.Maximum * 2 / 3)) && (args.progress == args.maximum))
                progressBar1.Value = progressBar1.Maximum;
        }

        private void buttonTreatCansel_Click(object sender, EventArgs e)
        {
            FormNotify formNotify = new FormNotify("Abort",
                new string[] {
                "Are you sure you want to cancel the operation?"},
                NotifyButtons.YesNo);
            formNotify.ShowDialog();
            if (formNotify.DialogResult == DialogResult.Yes)
            {
                screenTreatHide();
                screenLoginShow();
                formClear();
            }
            formNotify.Dispose();
        }

        private async void buttonTreatApprove_Click(object sender, EventArgs e)
        {
            comboBox1.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = false;
            progressBar1.Visible = true;
            amData.serialPortProgressEvent += new EventHandler(progressBar1_Callback);
            progressBar1.Maximum = 160;
            ErrCode errcode = await amData.AMDataWrite();
            if (errcode >= ErrCode.OK)
            {
                errcode = await amData.AMDataRead();
            }
            progressBar1.Value = progressBar1.Minimum;
            progressBar1.Visible = false;
            if (errcode == ErrCode.OK)
            {
                FormNotify formNotify = new FormNotify("Success",
                    new string[] {
                    string.Format("{0} treatments updated,",amData.MaxiSet),
                    string.Format("{0} treatments available on AM - SN {1},", amData.Maxi, amData.SNum),
                    "please disconnect the AM"},
                    NotifyButtons.OK);
                formNotify.ShowDialog();
                formNotify.Dispose();
            }
            else if (errcode == ErrCode.EPARAM)
            {
                // wrong parameters
                FormNotify formNotify = new FormNotify("Fail",
                    new string[] {
                    "Wrong parameters,",
                    "please choose the number of treatments"},
                    NotifyButtons.OK);
                formNotify.ShowDialog();
                formNotify.Dispose();
            }
            else
            {
                // if fail
                AMConnected = false;
                FormNotify formNotify = new FormNotify("Fail",
                    new string[] {
                    "The operation failed, the treatments were not added"},
                    NotifyButtons.OK);
                formNotify.ShowDialog();
                formNotify.Dispose();
            }
            comboBox1.Enabled = true;
            button1.Enabled = true;
            button2.Enabled = true;
            if (errcode != ErrCode.EPARAM)
            {
                formClear();
                screenTreatHide();
                screenLoginShow();
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                amData.MaxiSet = int.Parse(comboBox2.SelectedItem.ToString(), NumberStyles.Integer);
            }
            catch (Exception ex)
            {
                LogFile.logWrite(ex.ToString());
            }
        }



        //
        // Farm
        //



        private void screenFarmShow()
        {
            screenDataGridShow("Manage Farms", 
                new System.EventHandler(this.buttonFarmEdit_Click), 
                new System.EventHandler(this.buttonFarmAdd_Click), 
                new System.EventHandler(this.buttonFarmBack_Click));

        }

        private void screenDataGridShow(string dataName, System.EventHandler eventHandler1, System.EventHandler eventHandler2, System.EventHandler eventHandler3)
        {
            labelDraw(ref label1, new System.Drawing.Point(760, 200), "Welcome distributor",
                font: new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point));
            labelDraw(ref label2, new System.Drawing.Point(863, 300), dataName);
            buttonDraw(ref button1, new System.Drawing.Point(790, 400), "Edit", eventHandler1);
            buttonDraw(ref button2, new System.Drawing.Point(1220, 400), "Add New", eventHandler2, placeh: Place.Right);
            textBoxSmallDraw(ref richTextBox1, new System.Drawing.Point(360, 400), "search", placeh: Place.Left);
            buttonDraw(
                ref button3, new System.Drawing.Point(180, 400), "Back", eventHandler3,
                placeh: Place.Start);
            dataGridDraw(ref dataGridView1, new System.Drawing.Point(360, 525));
        }

        private void buttonFarmBack_Click(object sender, EventArgs e)
        {
            screenDataGridHide();
            screenActionShow();
        }

        private void screenDataGridHide()
        {
            label1.Dispose();
            label2.Dispose();
            button1.Dispose();
            button2.Dispose();
            richTextBox1.Dispose();
            button3.Dispose();
            bindingSource1.Dispose();
            dataGridView1.Dispose();
        }

        private void dataGridDraw(ref DataGridView dataGridView, Point location, Place placeh = Place.Center)
        {
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            dataGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(dataGridView)).BeginInit();
            dataGridView.Anchor = System.Windows.Forms.AnchorStyles.Top;
            dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.RowHeadersWidth = 62;
            dataGridView.RowTemplate.Height = 33;
            dataGridView.Size = new System.Drawing.Size(2460, 800);
            dataGridView.Location = placeCalc(dataGridView, location: location, placeh: placeh);
            dataGridView.TabIndex = 6;
            Controls.Add(dataGridView);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(dataGridView)).EndInit();
            this.bindingSource1.DataSource = new DataSet1.DataTable1DataTable();
            dataGridView.DataSource = this.bindingSource1;

        }
        private void textBoxSmallDraw(ref RichTextBox textBox, Point location, string text, Place placeh = Place.Center)
        {
            textBox = new System.Windows.Forms.RichTextBox();
            Controls.Add(textBox);
            textBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            textBox.Margin = new System.Windows.Forms.Padding(4);
            textBox.Size = new System.Drawing.Size(390, 90);
            textBox.TabIndex = 1;
            textBox.Text = text;
            textBox.Location = placeCalc(textBox, location: location, placeh: placeh);
        }
        private void buttonFarmAdd_Click(object sender, EventArgs e)
        {
            screenDataGridHide();
            screenFarmAddShow();
        }
        private void buttonFarmEdit_Click(object sender, EventArgs e)
        {
            screenDataGridHide();
            screenFarmEditShow();
        }

        private void screenFarmAddShow()
        {
            screenFarmUpdateShow();
            labelDraw(ref label16, new System.Drawing.Point(360, 200), "Add Farm",
                font: new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point));
            textBoxSmallDraw(ref richTextBox15, new System.Drawing.Point(360, 900), "", placeh: Place.Right);
            labelDraw(ref label15, new System.Drawing.Point(360, 900), "Contract:", autoSize: false);
        }

        private void screenFarmEditShow()
        {
            screenFarmUpdateShow();
            labelDraw(ref label16, new System.Drawing.Point(360, 200), "Edit Farm",
                font: new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point));
        }

        private void screenFarmUpdateShow()
        {
            CultureInfo[] ci = CultureInfo.GetCultures(CultureTypes.AllCultures & ~CultureTypes.NeutralCultures);
            RegionInfo region = new RegionInfo(ci[0].Name);

            textBoxSmallDraw(ref richTextBox1, new System.Drawing.Point(360, 300), "", placeh: Place.Left);
            textBoxSmallDraw(ref richTextBox2, new System.Drawing.Point(360, 400), "", placeh: Place.Left);
            textBoxSmallDraw(ref richTextBox3, new System.Drawing.Point(360, 500), "", placeh: Place.Left);
            textBoxSmallDraw(ref richTextBox4, new System.Drawing.Point(360, 600), "", placeh: Place.Left);
            textBoxSmallDraw(ref richTextBox5, new System.Drawing.Point(360, 700), "", placeh: Place.Left);
            textBoxSmallDraw(ref richTextBox6, new System.Drawing.Point(360, 800), "", placeh: Place.Left);
            textBoxSmallDraw(ref richTextBox7, new System.Drawing.Point(360, 900), "", placeh: Place.Left);
            textBoxSmallDraw(ref richTextBox8, new System.Drawing.Point(360, 1000), "", placeh: Place.Left);
            textBoxSmallDraw(ref richTextBox9, new System.Drawing.Point(360, 300), "", placeh: Place.Right);
            textBoxSmallDraw(ref richTextBox10, new System.Drawing.Point(360, 400), "", placeh: Place.Right);
            textBoxSmallDraw(ref richTextBox11, new System.Drawing.Point(360, 500), "", placeh: Place.Right);
            textBoxSmallDraw(ref richTextBox12, new System.Drawing.Point(360, 600), "", placeh: Place.Right);
            textBoxSmallDraw(ref richTextBox13, new System.Drawing.Point(360, 700), "", placeh: Place.Right);
            textBoxSmallDraw(ref richTextBox14, new System.Drawing.Point(360, 800), "", placeh: Place.Right);

            labelDraw(ref label1, new System.Drawing.Point(360, 300), "Name of Dairy farm:", autoSize: false, placeh: Place.Start);
            labelDraw(ref label2, new System.Drawing.Point(360, 400), "Address:", autoSize: false, placeh: Place.Start);
            labelDraw(ref label3, new System.Drawing.Point(360, 500), "Country:", autoSize: false, placeh: Place.Start);
            labelDraw(ref label4, new System.Drawing.Point(360, 600), "State:", autoSize: false, placeh: Place.Start);
            labelDraw(ref label5, new System.Drawing.Point(360, 700), "City:", autoSize: false, placeh: Place.Start);
            labelDraw(ref label6, new System.Drawing.Point(360, 800), "Contact Name:", autoSize: false, placeh: Place.Start);
            labelDraw(ref label7, new System.Drawing.Point(360, 900), "Mobile:", autoSize: false, placeh: Place.Start);
            labelDraw(ref label8, new System.Drawing.Point(360, 1000), "Email Address:", autoSize: false, placeh: Place.Start);
            labelDraw(ref label9, new System.Drawing.Point(360, 300), "Farm Type:", autoSize: false);
            labelDraw(ref label10, new System.Drawing.Point(360, 400), "Breed:", autoSize: false);
            labelDraw(ref label11, new System.Drawing.Point(360, 500), "# of lactating cows:", autoSize: false);
            labelDraw(ref label12, new System.Drawing.Point(360, 600), "Monthly DHI test:", autoSize: false);
            labelDraw(ref label13, new System.Drawing.Point(360, 700), "Milking Setup:", autoSize: false);
            labelDraw(ref label14, new System.Drawing.Point(360, 800), "Location of Treatment:", autoSize: false);
            
            buttonDraw(ref button1, new System.Drawing.Point(360, 1150), "Cancel", new System.EventHandler(buttonFarmCancel_Click),
                placeh: Place.Left, placev: Place.End);
            buttonDraw(ref button2, new System.Drawing.Point(360, 1150), "Submit", new System.EventHandler(buttonFarmSubmit_Click),
                placeh: Place.Right, placev: Place.End);
        }

        private void buttonFarmCancel_Click(object sender, EventArgs e)
        {
            screenFarmUpdateHide();
            screenFarmShow();
        }

        private void screenFarmUpdateHide()
        {
            detailsHide();

            richTextBox10.Dispose();
            richTextBox11.Dispose();
            richTextBox12.Dispose();
            richTextBox13.Dispose();
            richTextBox14.Dispose();
            if (richTextBox15 != null)
                richTextBox15.Dispose();

            label10.Dispose();
            label11.Dispose();
            label12.Dispose();
            label13.Dispose();
            label14.Dispose();
            if (label15 != null)
                label15.Dispose();
            label16.Dispose();

            button1.Dispose();
            button2.Dispose();
        }

        private void detailsHide()
        {
            richTextBox1.Dispose();
            richTextBox2.Dispose();
            richTextBox3.Dispose();
            richTextBox4.Dispose();
            richTextBox5.Dispose();
            richTextBox6.Dispose();
            richTextBox7.Dispose();
            richTextBox8.Dispose();
            richTextBox9.Dispose();

            label1.Dispose();
            label2.Dispose();
            label3.Dispose();
            label4.Dispose();
            label5.Dispose();
            label6.Dispose();
            label7.Dispose();
            label8.Dispose();
            label9.Dispose();
        }

        private void buttonFarmSubmit_Click(object sender, EventArgs e)
        {
            screenFarmUpdateHide();
            screenFarmShow();
        }



        //
        // Service
        //



        private void screenServiceShow()
        {
            screenDataGridShow("Manage Service providers", 
                new System.EventHandler(this.buttonServiceEdit_Click),
                new System.EventHandler(this.buttonServiceAdd_Click),
                new System.EventHandler(this.buttonServiceBack_Click));
        }

        private void buttonServiceAdd_Click(object sender, EventArgs e)
        {
            screenDataGridHide();
            screenServiceAddShow();
        }

        private void screenServiceAddShow()
        {
            screenServiceUpdateShow();
            labelDraw(ref label16, new System.Drawing.Point(360, 200), "Add Service provider",
                font: new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point));
            textBoxSmallDraw(ref richTextBox15, new System.Drawing.Point(360, 1000), "", placeh: Place.Right);
            labelDraw(ref label15, new System.Drawing.Point(360, 1000), "Contract:", autoSize: false);
        }

        private void buttonServiceEdit_Click(object sender, EventArgs e)
        {
            screenDataGridHide();
            screenServiceEditShow();
        }

        private void screenServiceEditShow()
        {
            screenServiceUpdateShow();
            labelDraw(ref label16, new System.Drawing.Point(360, 200), "Edit Service provider",
                font: new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point));
        }

        private void buttonServiceBack_Click(object sender, EventArgs e)
        {
            screenDataGridHide();
            screenActionShow();
        }

        private void screenServiceUpdateShow()
        {
            textBoxSmallDraw(ref richTextBox1, new System.Drawing.Point(360, 600), "", placeh: Place.Left);
            textBoxSmallDraw(ref richTextBox2, new System.Drawing.Point(360, 700), "", placeh: Place.Left);
            textBoxSmallDraw(ref richTextBox3, new System.Drawing.Point(360, 800), "", placeh: Place.Left);
            textBoxSmallDraw(ref richTextBox4, new System.Drawing.Point(360, 900), "", placeh: Place.Left);
            textBoxSmallDraw(ref richTextBox5, new System.Drawing.Point(360, 1000), "", placeh: Place.Left);
            textBoxSmallDraw(ref richTextBox6, new System.Drawing.Point(360, 600), "", placeh: Place.Right);
            textBoxSmallDraw(ref richTextBox7, new System.Drawing.Point(360, 700), "", placeh: Place.Right);
            textBoxSmallDraw(ref richTextBox8, new System.Drawing.Point(360, 800), "", placeh: Place.Right);
            textBoxSmallDraw(ref richTextBox9, new System.Drawing.Point(360, 900), "", placeh: Place.Right);

            labelDraw(ref label1, new System.Drawing.Point(360, 600), "# of dairy farms:", autoSize: false, placeh: Place.Start);
            labelDraw(ref label2, new System.Drawing.Point(360, 700), "# of dairy cows", autoSize: false, placeh: Place.Start);
            labelDraw(ref label3, new System.Drawing.Point(360, 800), "Address:", autoSize: false, placeh: Place.Start);
            labelDraw(ref label4, new System.Drawing.Point(360, 900), "Country:", autoSize: false, placeh: Place.Start);
            labelDraw(ref label5, new System.Drawing.Point(360, 1000), "State:", autoSize: false, placeh: Place.Start);
            labelDraw(ref label6, new System.Drawing.Point(360, 600), "City:", autoSize: false);
            labelDraw(ref label7, new System.Drawing.Point(360, 700), "Name of contact :", autoSize: false);
            labelDraw(ref label8, new System.Drawing.Point(360, 800), "Mobile:", autoSize: false);
            labelDraw(ref label9, new System.Drawing.Point(360, 900), "Email Address:", autoSize: false);

            buttonDraw(ref button1, new System.Drawing.Point(360, 1150), "Cancel", new System.EventHandler(buttonServiceCancel_Click),
                placeh: Place.Left, placev: Place.End);
            buttonDraw(ref button2, new System.Drawing.Point(360, 1150), "Submit", new System.EventHandler(buttonServiceSubmit_Click),
                placeh: Place.Right, placev: Place.End);
        }

        private void screenServiceUpdateHide()
        {
            detailsHide();

            if (richTextBox15 != null)
                richTextBox15.Dispose();

            if (label15 != null)
                label15.Dispose();
            label16.Dispose();

            button1.Dispose();
            button2.Dispose();
        }

        private void buttonServiceCancel_Click(object sender, EventArgs e)
        {
            screenServiceUpdateHide();
            screenServiceShow();
        }

        private void buttonServiceSubmit_Click(object sender, EventArgs e)
        {
            screenServiceUpdateHide();
            screenServiceShow();
        }

    }

}
