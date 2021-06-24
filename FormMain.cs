using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinAMBurner
{
    //enum Place
    //{
    //    Center,
    //    Right,
    //    RightOne,
    //    RightTwo,
    //    Left,
    //    LeftOne,
    //    LeftTwo,
    //    Start,
    //    End,
    //    One,
    //    Two,
    //    Three,
    //    Four,
    //    Five,
    //    Six,
    //    Seven,
    //    Eight,
    //    Nine,
    //    Ten
    //}

    public partial class FormMain : Form
    {
        private RichTextBox richTextBox1;
        private RichTextBox richTextBox2;
        private RichTextBox richTextBox3;
        private RichTextBox richTextBox4;
        private RichTextBox richTextBox5;
        private RichTextBox richTextBox6;
        private RichTextBox richTextBox7;
        private RichTextBox richTextBox8;
        private RichTextBox richTextBox9;
        private RichTextBox richTextBox10;
        private RichTextBox richTextBox11;
        private RichTextBox richTextBox12;
        private RichTextBox richTextBox13;
        private RichTextBox richTextBox14;
        private RichTextBox richTextBox15;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label14;
        private Label label15;
        private Label label16;
        private Button button1;
        private Button button2;
        private Button button3;
        private ProgressBar progressBar1;
        private ComboBox comboBox1;
        private ComboBox comboBox2;
        //private System.Windows.Forms.BindingSource bindingSource1;
        private DataGridView dataGridView1;
        private LinkLabel linkLabel1;
        private LinkLabel linkLabel2;
        private ComboBox comboBox3;
        private ComboBox comboBox4;
        private ComboBox comboBox5;

        private AMData amData = new AMData();

        private bool AMConnected = false;

        private readonly HttpClient client = new HttpClient();

        private List<Farm> farms;
        private DataTable farmTable;
        private List<Service> services;
        private DataTable serviceTable;

        private string tabletNo = null;
        private string TabletNo
        {
            get
            {
                if (tabletNo == null)
                    tabletNo = GetBIOSSerNo();
                return tabletNo;
            }
        }

        public FormMain()
        {
            InitializeComponent();
            this.Size = new Size(2400, 1600);
            this.Scale(new SizeF(Misc.ScaleFactor, Misc.ScaleFactor));
            this.Font = new System.Drawing.Font("Segoe UI", Misc.DefaultFont, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            //pictureBoxTitle.Scale(new SizeF(Misc.ScaleFactor, Misc.ScaleFactor));
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
            linkLabel1.Dispose();
            button1.Dispose();
        }
        private void screenLoginShow()
        {
            linkLabelDraw(ref linkLabel1, "Forgot password",
                new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked), placev: Misc.Place.Five);
            Misc.buttonDraw(this, ref button1, "Login", new EventHandler(buttonLogin_Click), placev: Misc.Place.End);
            textBoxDraw(ref richTextBox1, "Username", placev: Misc.Place.One);
            textBoxDraw(ref richTextBox2, "Password", placev: Misc.Place.Three);
        }

        private void textBoxEnter_Click(object sender, EventArgs e)
        {
            Control control = sender as Control;
            string text = control.Name;
            if (control.Text == text)
            {
                control.Text = "";
                control.ForeColor = Color.Black;
            }
        }

        private void textBoxLeave_Click(object sender, EventArgs e)
        {
            Control control = sender as Control;
            string text = control.Name;
            if (control.Text == string.Empty)
            {
                control.Text = text;
                control.ForeColor = Color.Silver;
            }
        }

        private void linkLabelDraw(ref LinkLabel linkLabel, string text, LinkLabelLinkClickedEventHandler eventHandler,
            Misc.Place placeh = Misc.Place.Center, Misc.Place placev = Misc.Place.Center)
        {
            linkLabel = new System.Windows.Forms.LinkLabel();
            Controls.Add(linkLabel);
            linkLabel.AutoSize = true;
            linkLabel.Anchor = ((System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Left));//((System.Windows.Forms.AnchorStyles.Right) | (System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Bottom));
            linkLabel.Size = new System.Drawing.Size(Misc.DefaultWidth, Misc.DefaultHeight);
            linkLabel.Scale(new SizeF(Misc.ScaleFactor, Misc.ScaleFactor));
            linkLabel.TabIndex = 1;
            linkLabel.TabStop = true;
            linkLabel.Text = text;
            linkLabel.Font = new System.Drawing.Font("Segoe UI", Misc.DefaultFont, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            linkLabel.Location = Misc.placeCalc(this, linkLabel, placeh: placeh, placev: placev);
            linkLabel.LinkClicked += eventHandler;
        }

        private void textBoxDraw(ref RichTextBox textBox, string text, Misc.Place placeh = Misc.Place.Center, Misc.Place placev = Misc.Place.Center)
        {
            textBox = new System.Windows.Forms.RichTextBox();
            Controls.Add(textBox);
            //textBox.AutoSize = true;
            textBox.Anchor = ((System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Left));//((System.Windows.Forms.AnchorStyles.Right) | (System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Bottom));
            textBox.Margin = new System.Windows.Forms.Padding(4);
            textBox.Size = new System.Drawing.Size(Misc.DefaultWidthLarge, Misc.DefaultHeight);
            textBox.Scale(new SizeF(Misc.ScaleFactor, Misc.ScaleFactor));
            textBox.TabIndex = 1;
            textBox.Text = text;
            textBox.Name = text;
            textBox.Font = new System.Drawing.Font("Segoe UI", Misc.DefaultFont, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            textBox.ForeColor = Color.Silver;
            textBox.Location = Misc.placeCalc(this, textBox, placeh: placeh, placev: placev);
            textBox.Enter += new System.EventHandler(textBoxEnter_Click);
            textBox.Leave += new System.EventHandler(textBoxLeave_Click);
        }

        private async void buttonLogin_Click(object sender, EventArgs e)
        {
            HttpResponseMessage response = await loginPost();
            if(response.StatusCode != HttpStatusCode.OK)
            {
                // if failed
                FormNotify formNotify = new FormNotify("Login Failed",
                    new string[] {
                "Login failed Check your username and password,",
                "make sure your tablet is connected to the internet"},
                    NotifyButtons.OK);
                formNotify.ShowDialog();
                formNotify.Dispose();
            }
            {
                // if ok

                screenLoginHide();
                screenActionShow();
            }
        }

        private async Task<HttpResponseMessage> loginPost()
        {          
            var request = new HttpRequestMessage(new HttpMethod("POST"), "https://90fd534a-bb34-45c5-8622-5a1bba51db6e.mock.pstmn.io/" + "api/p/login/");
            
            Dictionary<string, string> postParams = new Dictionary<string, string>() 
            { { "email", richTextBox1.Text }, { "password", richTextBox2.Text }, { "tablet", TabletNo } };

            foreach (var param in postParams)
            {

                request.Headers.Add(param.Key, param.Value);

            }
            var response = await client.SendAsync(request);

            var responseString = await response.Content.ReadAsStringAsync();

            return response;
        }

        private async Task<List<Farm>> farmsGet()
        {
            var streamTask = client.GetStreamAsync("https://90fd534a-bb34-45c5-8622-5a1bba51db6e.mock.pstmn.io/" + "api/p/farms/0");
            var farms = await JsonSerializer.DeserializeAsync<List<Farm>>(await streamTask);
            return farms;
        }

        private async Task<List<Service>> servicesGet()
        {
            var streamTask = client.GetStreamAsync("https://90fd534a-bb34-45c5-8622-5a1bba51db6e.mock.pstmn.io/" + "api/p/service_providers/0");
            var services = await JsonSerializer.DeserializeAsync<List<Service>>(await streamTask);
            return services;
        }

        private async Task<HttpResponseMessage> farmAdd(Farm farm)
        {
            var request = new HttpRequestMessage(new HttpMethod("POST"), "https://90fd534a-bb34-45c5-8622-5a1bba51db6e.mock.pstmn.io/" + "api/p/farms/");

            return await farmAddEdit(farm, request);
        }

        private async Task<HttpResponseMessage> farmEdit(Farm farm)
        {
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), "https://90fd534a-bb34-45c5-8622-5a1bba51db6e.mock.pstmn.io/" + "api/p/farms/" + farm.id);

            return await farmAddEdit(farm, request);
        }

        private async Task<HttpResponseMessage> farmAddEdit(Farm farm, HttpRequestMessage request)
        {
            Dictionary<string, string> postParams = new Dictionary<string, string>()
            {
                  {"name", farm.name},
                  {"farm_type", farm.farm_type },
                  {"breed_type", farm.breed_type },
                  {"address", farm.address},
                  {"country", farm.country },
                  {"city", farm.city },
                  {"state", farm.state },
                  {"mobile", farm.mobile },
                  {"location_of_treatment_type", farm.location_of_treatment_type.ToString() },
                  {"number_of_lactating_cows", farm.number_of_lactating_cows.ToString()},
                  {"dhi_test", farm.dhi_test.ToString()},
                  {"contract_type", farm.contract_type },
                  {"milking_setup_type", farm.milking_setup_type}
            };


            foreach (var param in postParams)
            {

                request.Headers.Add(param.Key, param.Value);

            }
            var response = await client.SendAsync(request);

            var responseString = await response.Content.ReadAsStringAsync();

            return response;
        }

        private string GetBIOSSerNo()
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
            Misc.labelDraw(this, ref label1, "Welcome distributor", placev: Misc.Place.One,
                font: new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point));
            Misc.labelDraw(this, ref label2, "Choose Action: ", placev: Misc.Place.Three);
            Misc.buttonDraw(this, ref button1, "Update AM", new System.EventHandler(buttonUpdateAM_Click), placev: Misc.Place.Four);
            Misc.buttonDraw(this, ref button2, "Manage Farms", new System.EventHandler(buttonFarm_Click), placev: Misc.Place.Five);
            Misc.buttonDraw(this, ref button3, "Manage Service provider", new System.EventHandler(buttonService_Click), placev: Misc.Place.Six);
            linkLabelDraw(ref linkLabel2, "Calculate your farm’s profits with APT", 
                new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked), placev: Misc.Place.Eight);
        }

        private void buttonUpdateAM_Click(object sender, EventArgs e)
        {
            screenActionHide();
            screenConnectShow();
        }

        private async void buttonFarm_Click(object sender, EventArgs e)
        {
            screenActionHide();
            await farmTableGet();
            screenFarmShow();
        }

        private async Task farmTableGet()
        {
            farms = await farmsGet();
            farmTable = new DataTable();
            farmTable.Columns.AddRange(new List<DataColumn>(){new DataColumn("Farm Name"),
                                    new DataColumn("Country / State"),
                                    new DataColumn("City"),
                                    new DataColumn("Address")}.ToArray());
            //foreach (var f in farms.Select(f => new { f.name, f.country, f.city, f.address }).ToArray())
            foreach (var f in farms.Select(f => new List<string> { f.name, f.country, f.city, f.address }))
                farmTable.Rows.Add(f.ToArray<string>());
        }

        private async void buttonService_Click(object sender, EventArgs e)
        {
            screenActionHide();
            await serviceTableGet();
            screenServiceShow();
        }

        private async Task serviceTableGet()
        {
            services = await servicesGet();
            serviceTable = new DataTable();
            serviceTable.Columns.AddRange(new List<DataColumn>(){new DataColumn("Name"),
                                    new DataColumn("Country / State"),
                                    new DataColumn("City"),
                                    new DataColumn("Address")}.ToArray());
            //foreach (var f in farms.Select(f => new { f.name, f.country, f.city, f.address }).ToArray())
            foreach (var s in services.Select(s => new List<string> { s.name, s.country, s.city, s.address }))
                serviceTable.Rows.Add(s.ToArray<string>());
        }

        private void screenActionHide()
        {
            label1.Dispose();
            label2.Dispose();
            button1.Dispose();
            button2.Dispose();
            button3.Dispose();
            linkLabel2.Dispose();
        }

        private void screenConnectShow()
        {
            Misc.labelDraw(this, ref label1, "Welcome distributor", placev: Misc.Place.One,
                font: new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point));
            Misc.labelDraw(this, ref label2, "Please make sure the AM is connected to your tablet before continue", placev: Misc.Place.Three);
            Misc.labelDraw(this, ref label3, "", placev: Misc.Place.Five);
            progressBarDraw(ref progressBar1, placev: Misc.Place.Five);
            Misc.buttonDraw(this, ref button1, "Check AM present", new EventHandler(buttonCheckAM_Click), placeh: Misc.Place.Left, placev: Misc.Place.End);
            Misc.buttonDraw(this, ref button2, "Forward", new EventHandler(buttonConnectForward_Click), placeh: Misc.Place.Right, placev: Misc.Place.End);
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
            labelDraw(label3, new System.Drawing.Point(360, 620), "AM not found – make sure AM is connected using USB cable", Color.Red,
                Misc.Place.Center, Misc.Place.Five);
            button2.Enabled = false;
        }

        private void AMConnectedShow()
        {
            labelDraw(label3, new System.Drawing.Point(360, 620), "AM found – connected to AM", Color.Green,
                Misc.Place.Center, Misc.Place.Five);
            button2.Enabled = true;
        }

        private void labelDraw(Label label, Point location, string text, Color color, Misc.Place placeh, Misc.Place placev)
        {
            label.ForeColor = color;
            label.Text = text;
            label.Font = new System.Drawing.Font("Segoe UI", Misc.DefaultFont, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label.Location = Misc.placeCalc(this, label, location: location, placeh: placeh, placev: placev);
        }

        private void buttonConnectForward_Click(object sender, EventArgs e)
        {
            screenConnectHide();
            screenInfoShow();
        }

        private void progressBarDraw(ref ProgressBar progressBar, Misc.Place placeh = Misc.Place.Center, Misc.Place placev = Misc.Place.Center)
        {
            progressBar = new System.Windows.Forms.ProgressBar();
            Controls.Add(progressBar);
            //progressBar.AutoSize = true;
            progressBar.Anchor = ((System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Left));//((System.Windows.Forms.AnchorStyles.Right) | (System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Bottom));
            progressBar.Margin = new System.Windows.Forms.Padding(4);
            progressBar.Size = new System.Drawing.Size(Misc.DefaultWidthLarge, 24);
            progressBar.Scale(new SizeF(Misc.ScaleFactor, Misc.ScaleFactor));
            progressBar.Location = Misc.placeCalc(this, progressBar, placev: placev);
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
            Misc.labelDraw(this, ref label1, "Welcome distributor", placev: Misc.Place.One,
                font: new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point));
            Misc.labelDraw(this, ref label2, "AM information, pulses per treatment : " + 1500, placev: Misc.Place.Three);
            Misc.labelDraw(this, ref label3, "AM identified with SN: " + amData.SNum, placev: Misc.Place.Five);
            Misc.labelDraw(this, ref label4, "Current available treatments: " + amData.Maxi, placev: Misc.Place.Seven);
            Misc.buttonDraw(this, ref button1, "Back", new EventHandler(buttonInfoBack_Click), placeh: Misc.Place.Left, placev: Misc.Place.End);
            Misc.buttonDraw(this, ref button2, "Continue", new EventHandler(buttonInfoContinue_Click), placeh: Misc.Place.Right, placev: Misc.Place.End);
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
            Misc.labelDraw(this, ref label1, "Welcome distributor", placev: Misc.Place.One,
                font: new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point));
            Misc.labelDraw(this, ref label2, "Select Farm / Service provider", placev: Misc.Place.Three);
            comboBoxDraw(ref comboBox1, new System.EventHandler(comboBox1_SelectedIndexChanged), placev: Misc.Place.Four);
            comboBox1.Items.AddRange(new string[] { "farm1", "farm2", "farm3", "farm4", "farm5", "farm6" });
            Misc.labelDraw(this, ref label3, "Add treatments to AM – SN" + amData.SNum, placev: Misc.Place.Five);
            comboBoxDraw(ref comboBox2, new System.EventHandler(comboBox2_SelectedIndexChanged), placev: Misc.Place.Six);
            comboBox2.Items.AddRange(new string[] { "50", "100", "150", "200", "250", "300" });
            progressBarDraw(ref progressBar1, placev: Misc.Place.Seven);
            Misc.buttonDraw(this, ref button1, "Cancel", new EventHandler(buttonTreatCansel_Click), placeh: Misc.Place.Left, placev: Misc.Place.End);
            Misc.buttonDraw(this, ref button2, "Approve", new EventHandler(buttonTreatApprove_Click), placeh: Misc.Place.Right, placev: Misc.Place.End);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBoxSmallDraw(ref ComboBox comboBox, EventHandler eventHandler, List<string> items, string text, Misc.Place placeh = Misc.Place.Center, Misc.Place placev = Misc.Place.Center)
        {
            comboBox = new System.Windows.Forms.ComboBox();
            Controls.Add(comboBox);
            //comboBox.AutoSize = true;
            comboBox.Anchor = ((System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Left));//((System.Windows.Forms.AnchorStyles.Right) | (System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Bottom));
            comboBox.FormattingEnabled = true;
            comboBox.Margin = new System.Windows.Forms.Padding(4);
            comboBox.Size = new System.Drawing.Size(Misc.DefaultWidth, Misc.DefaultHeight);
            comboBox.Scale(new SizeF(Misc.ScaleFactor, Misc.ScaleFactor));
            comboBox.Font = new System.Drawing.Font("Segoe UI", Misc.DefaultFont, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            comboBox.Location = Misc.placeCalc(this, comboBox, placeh: placeh, placev: placev);
            comboBox.TabIndex = 17;
            comboBox.SelectedIndexChanged += eventHandler;
            comboBox.Items.AddRange(items.ToArray());
            comboBox.Text = text;
        }

        private void comboBoxDraw(ref ComboBox comboBox, EventHandler eventHandler, Misc.Place placeh = Misc.Place.Center, Misc.Place placev = Misc.Place.Center)
        {
            comboBox = new System.Windows.Forms.ComboBox();
            Controls.Add(comboBox);
            //comboBox.AutoSize = true;
            comboBox.Anchor = ((System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Left));//((System.Windows.Forms.AnchorStyles.Right) | (System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Bottom));
            comboBox.FormattingEnabled = true;
            comboBox.Margin = new System.Windows.Forms.Padding(4);
            comboBox.Size = new System.Drawing.Size(Misc.DefaultWidthLarge, Misc.DefaultHeightLarge);
            comboBox.Scale(new SizeF(Misc.ScaleFactor, Misc.ScaleFactor));
            comboBox.Font = new System.Drawing.Font("Segoe UI", Misc.DefaultFont, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            comboBox.Location = Misc.placeCalc(this, comboBox, placeh: placeh, placev: placev);
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

            dataGridView1.DataSource = farmTable;
        }

        private void screenDataGridShow(string dataName, System.EventHandler eventHandlerButton1, System.EventHandler eventHandlerButton2, System.EventHandler eventHandler3)
        {
            //labelDraw(ref label1, "Welcome distributor", placev: Place.First,
            //    font: new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point));
            Misc.labelDraw(this, ref label2, dataName, placev: Misc.Place.One);
            Misc.buttonDraw(this, ref button1, "Edit", eventHandlerButton1, placeh: Misc.Place.RightOne, placev: Misc.Place.Two);
            Misc.buttonDraw(this, ref button2, "Add New", eventHandlerButton2, placeh: Misc.Place.RightTwo, placev: Misc.Place.Two);
            textBoxSmallDraw(ref richTextBox1, "Search", "Search", placeh: Misc.Place.LeftOne, placev: Misc.Place.Two);
            Misc.buttonDraw(this, ref button3, "Back", eventHandler3, placeh: Misc.Place.LeftTwo, placev: Misc.Place.Two);
            dataGridDraw(ref dataGridView1, placev: Misc.Place.Three);
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
            //bindingSource1.Dispose();
            dataGridView1.Dispose();
        }

        private void dataGridDraw(ref DataGridView dataGridView, Misc.Place placeh = Misc.Place.Center, Misc.Place placev = Misc.Place.Center)
        {
            this.components = new System.ComponentModel.Container();
            //this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            dataGridView = new System.Windows.Forms.DataGridView();
            //((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(dataGridView)).BeginInit();
            //dataGridView.AutoSize = true;
            dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Left));//((System.Windows.Forms.AnchorStyles.Right) | (System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Bottom));
            dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.RowHeadersWidth = 62;
            dataGridView.RowTemplate.Height = 33;
            dataGridView.Size = new System.Drawing.Size(1853, 800);
            dataGridView.Scale(new SizeF(Misc.ScaleFactor, Misc.ScaleFactor));
            dataGridView.Font = new System.Drawing.Font("Segoe UI", Misc.DefaultFont, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridView.Location = Misc.placeCalc(this, dataGridView, placeh: placeh, placev: placev);
            dataGridView.TabIndex = 6;
            Controls.Add(dataGridView);
            //((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(dataGridView)).EndInit();
            //this.bindingSource1.DataSource = new DataSet1.DataTable1DataTable();
            //dataGridView.DataSource = this.bindingSource1;

        }
        private void textBoxSmallDraw(ref RichTextBox textBox, string text, string name, Misc.Place placeh = Misc.Place.Center, Misc.Place placev = Misc.Place.Center)
        {
            textBox = new System.Windows.Forms.RichTextBox();
            Controls.Add(textBox);
            //textBox.AutoSize = true;
            textBox.Anchor = ((System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Left));//((System.Windows.Forms.AnchorStyles.Right) | (System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Bottom));
            textBox.Margin = new System.Windows.Forms.Padding(4);
            textBox.Size = new System.Drawing.Size(Misc.DefaultWidth, Misc.DefaultHeight);
            textBox.Scale(new SizeF(Misc.ScaleFactor, Misc.ScaleFactor));
            textBox.TabIndex = 1;
            textBox.Text = text;
            textBox.Name = name;
            textBox.Font = new System.Drawing.Font("Segoe UI", Misc.DefaultFont, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            if (name == text)
                textBox.ForeColor = Color.Silver;
            textBox.Location = Misc.placeCalc(this, textBox, placeh: placeh, placev: placev);
            textBox.Enter += new System.EventHandler(textBoxEnter_Click);
            textBox.Leave += new System.EventHandler(textBoxLeave_Click);
        }
        private void buttonFarmAdd_Click(object sender, EventArgs e)
        {
            screenDataGridHide();
            screenFarmAddShow();
        }
        private void buttonFarmEdit_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count > 0)
            {
                Farm farm = farms[dataGridView1.Rows.IndexOf(dataGridView1.SelectedRows[0])];
                screenDataGridHide();
                screenFarmEditShow(farm);
            }
        }

        private void screenFarmAddShow()
        {
            screenFarmUpdateShow(new Farm());
            Misc.labelDraw(this, ref label16, "Add Farm", placev: Misc.Place.One,
                font: new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point));
            comboBoxSmallDraw(ref comboBox5, new System.EventHandler(comboBox5_SelectedIndexChanged), Farm.CONTRACT_TYPE, (new Farm()).contract_type, placeh: Misc.Place.RightTwo, placev: Misc.Place.Eight);
            Misc.labelDraw(this, ref label15, "Contract:", autoSize: false, placeh: Misc.Place.RightOne, placev: Misc.Place.Eight);
            //locationStart = 0;
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void screenFarmEditShow(Farm farm)
        {
            screenFarmUpdateShow(farm);
            Misc.labelDraw(this, ref label16, "Edit Farm", placev: Misc.Place.One,
                font: new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point));
        }

        private void screenFarmUpdateShow(Farm farm)
        {
            CultureInfo[] ci = CultureInfo.GetCultures(CultureTypes.AllCultures & ~CultureTypes.NeutralCultures);
            RegionInfo region = new RegionInfo(ci[0].Name);

            //locationIdx = locationStart;
            textBoxSmallDraw(ref richTextBox1, farm.Name, Farm.NAME, placeh: Misc.Place.LeftOne, placev: Misc.Place.Two);
            textBoxSmallDraw(ref richTextBox2, farm.Address, Farm.ADDRESS, placeh: Misc.Place.LeftOne, placev: Misc.Place.Three);
            textBoxSmallDraw(ref richTextBox3, farm.Country, Farm.COUNTRY, placeh: Misc.Place.LeftOne, placev: Misc.Place.Four);
            textBoxSmallDraw(ref richTextBox4, farm.State, Farm.STATE, placeh: Misc.Place.LeftOne, placev: Misc.Place.Five);
            textBoxSmallDraw(ref richTextBox5, farm.City, Farm.CITY, placeh: Misc.Place.LeftOne, placev: Misc.Place.Six);
            textBoxSmallDraw(ref richTextBox6, farm.Contact, Farm.CONTACT, placeh: Misc.Place.LeftOne, placev: Misc.Place.Seven);
            textBoxSmallDraw(ref richTextBox7, farm.Mobile, Farm.MOBILE, placeh: Misc.Place.LeftOne, placev: Misc.Place.Eight);
            textBoxSmallDraw(ref richTextBox8, "", Farm.EMAIL, placeh: Misc.Place.LeftOne, placev: Misc.Place.Nine);
            comboBoxSmallDraw(ref comboBox1, new System.EventHandler(comboBox1_SelectedIndexChanged), Farm.FARM_TYPE, farm.FarmType, placeh: Misc.Place.RightTwo, placev: Misc.Place.Two);
            comboBoxSmallDraw(ref comboBox2, new System.EventHandler(comboBox2_SelectedIndexChanged), Farm.BREED_TYPE, farm.BreedType, placeh: Misc.Place.RightTwo, placev: Misc.Place.Three);
            textBoxSmallDraw(ref richTextBox9, farm.NumberOfLactatingCows, Farm.NUMBER_OF_LACTATING_COWS, placeh: Misc.Place.RightTwo, placev: Misc.Place.Four);
            textBoxSmallDraw(ref richTextBox10, farm.DhiTest, Farm.DHI_TEST, placeh: Misc.Place.RightTwo, placev: Misc.Place.Five);
            comboBoxSmallDraw(ref comboBox3, new System.EventHandler(comboBox3_SelectedIndexChanged), Farm.MILKING_SETUP_TYPE, farm.MilkingSetupType, placeh: Misc.Place.RightTwo, placev: Misc.Place.Six);
            comboBoxSmallDraw(ref comboBox4, new System.EventHandler(comboBox4_SelectedIndexChanged), Farm.LOCATION_OF_TREATMENT_TYPE, farm.LocationOfTreatmentType, placeh: Misc.Place.RightTwo, placev: Misc.Place.Seven);

            //locationIdx = locationStart;
            Misc.labelDraw(this, ref label1, "Name of Dairy farm:", autoSize: false, placeh: Misc.Place.LeftTwo, placev: Misc.Place.Two);
            Misc.labelDraw(this, ref label2, "Address:", autoSize: false, placeh: Misc.Place.LeftTwo, placev: Misc.Place.Three);
            Misc.labelDraw(this, ref label3, "Country:", autoSize: false, placeh: Misc.Place.LeftTwo, placev: Misc.Place.Four);
            Misc.labelDraw(this, ref label4, "State:", autoSize: false, placeh: Misc.Place.LeftTwo, placev: Misc.Place.Five);
            Misc.labelDraw(this, ref label5, "City:", autoSize: false, placeh: Misc.Place.LeftTwo, placev: Misc.Place.Six);
            Misc.labelDraw(this, ref label6, "Contact Name:", autoSize: false, placeh: Misc.Place.LeftTwo, placev: Misc.Place.Seven);
            Misc.labelDraw(this, ref label7, "Mobile:", autoSize: false, placeh: Misc.Place.LeftTwo, placev: Misc.Place.Eight);
            Misc.labelDraw(this, ref label8, "Email Address:", autoSize: false, placeh: Misc.Place.LeftTwo, placev: Misc.Place.Nine);
            Misc.labelDraw(this, ref label9, "Farm Type:", autoSize: false, placeh: Misc.Place.RightOne, placev: Misc.Place.Two);
            Misc.labelDraw(this, ref label10, "Breed:", autoSize: false, placeh: Misc.Place.RightOne, placev: Misc.Place.Three);
            Misc.labelDraw(this, ref label11, "# of lactating cows:", autoSize: false, placeh: Misc.Place.RightOne, placev: Misc.Place.Four);
            Misc.labelDraw(this, ref label12, "Monthly DHI test:", autoSize: false, placeh: Misc.Place.RightOne, placev: Misc.Place.Five);
            Misc.labelDraw(this, ref label13, "Milking Setup:", autoSize: false, placeh: Misc.Place.RightOne, placev: Misc.Place.Six);
            Misc.labelDraw(this, ref label14, "Location of Treatment:", autoSize: false, placeh: Misc.Place.RightOne, placev: Misc.Place.Seven);

            //locationIdx = 0;
            Misc.buttonDraw(this, ref button1, "Cancel", new System.EventHandler(buttonFarmCancel_Click), placeh: Misc.Place.LeftOne,
                placev: Misc.Place.Ten);
            Misc.buttonDraw(this, ref button2, "Submit", new System.EventHandler(buttonFarmSubmit_Click), placeh: Misc.Place.RightTwo,
                placev: Misc.Place.Ten);
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void buttonFarmCancel_Click(object sender, EventArgs e)
        {
            screenFarmUpdateHide();
            screenFarmShow();
        }

        private void screenFarmUpdateHide()
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
            richTextBox10.Dispose();
            comboBox1.Dispose();
            comboBox2.Dispose();
            comboBox3.Dispose();
            comboBox4.Dispose();
            if (comboBox5!= null)
                comboBox5.Dispose();

            label1.Dispose();
            label2.Dispose();
            label3.Dispose();
            label4.Dispose();
            label5.Dispose();
            label6.Dispose();
            label7.Dispose();
            label8.Dispose();
            label9.Dispose();
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

        //private void detailsHide()
        //{
        //    richTextBox1.Dispose();
        //    richTextBox2.Dispose();
        //    richTextBox3.Dispose();
        //    richTextBox4.Dispose();
        //    richTextBox5.Dispose();
        //    richTextBox6.Dispose();
        //    richTextBox7.Dispose();
        //    richTextBox8.Dispose();
        //    comboBox3.Dispose();
        //
        //    label1.Dispose();
        //    label2.Dispose();
        //    label3.Dispose();
        //    label4.Dispose();
        //    label5.Dispose();
        //    label6.Dispose();
        //    label7.Dispose();
        //    label8.Dispose();
        //    label9.Dispose();
        //}

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
            dataGridView1.DataSource = serviceTable;
        }

        private void buttonServiceAdd_Click(object sender, EventArgs e)
        {
            screenDataGridHide();
            screenServiceAddShow();
        }

        private void screenServiceAddShow()
        {
            screenServiceUpdateShow(new Service());
            Misc.labelDraw(this, ref label11, "Add Service provider", placev: Misc.Place.One,
                font: new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point));
            comboBoxSmallDraw(ref comboBox1, new System.EventHandler(comboBox1_SelectedIndexChanged), Service.CONTRACT_TYPE, (new Service()).contract_type, placeh: Misc.Place.RightTwo, placev: Misc.Place.Six);
            //textBoxSmallDraw(ref richTextBox15, "", "Contract:", placeh: Misc.Place.RightTwo, placev: Misc.Place.Six);
            Misc.labelDraw(this, ref label10, "Contract:", autoSize: false, placeh: Misc.Place.RightOne, placev: Misc.Place.Six);
        }

        private void buttonServiceEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                Service service = services[dataGridView1.Rows.IndexOf(dataGridView1.SelectedRows[0])];
                screenDataGridHide();
                screenServiceEditShow(service);
            }
        }

        private void screenServiceEditShow(Service service)
        {
            screenServiceUpdateShow(service);
            Misc.labelDraw(this, ref label11, "Edit Service provider", placev: Misc.Place.One,
                font: new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point));
        }

        private void buttonServiceBack_Click(object sender, EventArgs e)
        {
            screenDataGridHide();
            screenActionShow();
        }

        private void screenServiceUpdateShow(Service service)
        {
            textBoxSmallDraw(ref richTextBox1, service.NumberOfDairyFarms, Service.NUMBER_OF_DAIRY_FARMS, placeh: Misc.Place.LeftOne,  placev: Misc.Place.Two);
            textBoxSmallDraw(ref richTextBox2, service.NumberOfDairyCows, Service.NUMBER_OF_DAIRY_COWS, placeh: Misc.Place.LeftOne,  placev: Misc.Place.Three);
            textBoxSmallDraw(ref richTextBox3, service.Address, Service.ADDRESS, placeh: Misc.Place.LeftOne,  placev: Misc.Place.Four);
            textBoxSmallDraw(ref richTextBox4, service.Country, Service.COUNTRY, placeh: Misc.Place.LeftOne,  placev: Misc.Place.Five);
            textBoxSmallDraw(ref richTextBox5, service.State, Service.STATE, placeh: Misc.Place.LeftOne,  placev: Misc.Place.Six);
            textBoxSmallDraw(ref richTextBox6, service.City, Service.CITY, placeh: Misc.Place.RightTwo, placev: Misc.Place.Two);
            textBoxSmallDraw(ref richTextBox7, service.Name, Service.NAME, placeh: Misc.Place.RightTwo, placev: Misc.Place.Three);
            textBoxSmallDraw(ref richTextBox8, service.Mobile, Service.MOBILE, placeh: Misc.Place.RightTwo, placev: Misc.Place.Four);
            textBoxSmallDraw(ref richTextBox9, service.Email, Service.EMAIL, placeh: Misc.Place.RightTwo, placev: Misc.Place.Five);
        
            Misc.labelDraw(this, ref label1, "# of dairy farms:", autoSize: false, placeh: Misc.Place.LeftTwo, placev: Misc.Place.Two);
            Misc.labelDraw(this, ref label2, "# of dairy cows:", autoSize: false, placeh: Misc.Place.LeftTwo, placev: Misc.Place.Three);
            Misc.labelDraw(this, ref label3, "Address:", autoSize: false, placeh: Misc.Place.LeftTwo, placev: Misc.Place.Four);
            Misc.labelDraw(this, ref label4, "Country:", autoSize: false, placeh: Misc.Place.LeftTwo, placev: Misc.Place.Five);
            Misc.labelDraw(this, ref label5, "State:", autoSize: false, placeh: Misc.Place.LeftTwo, placev: Misc.Place.Six);
            Misc.labelDraw(this, ref label6, "City:", autoSize: false, placeh: Misc.Place.RightOne, placev: Misc.Place.Two);
            Misc.labelDraw(this, ref label7, "Name of contact :", autoSize: false, placeh: Misc.Place.RightOne, placev: Misc.Place.Three);
            Misc.labelDraw(this, ref label8, "Mobile:", autoSize: false, placeh: Misc.Place.RightOne, placev: Misc.Place.Four);
            Misc.labelDraw(this, ref label9, "Email Address:", autoSize: false, placeh: Misc.Place.RightOne, placev: Misc.Place.Five);

            Misc.buttonDraw(this, ref button1, "Cancel", new System.EventHandler(buttonServiceCancel_Click), placeh: Misc.Place.LeftOne,
                placev: Misc.Place.Ten);
            Misc.buttonDraw(this, ref button2, "Submit", new System.EventHandler(buttonServiceSubmit_Click), placeh: Misc.Place.RightTwo,
                placev: Misc.Place.Ten);
        }

        private void screenServiceUpdateHide()
        {
            //detailsHide();
            richTextBox1.Dispose();
            richTextBox2.Dispose();
            richTextBox3.Dispose();
            richTextBox4.Dispose();
            richTextBox5.Dispose();
            richTextBox6.Dispose();
            richTextBox7.Dispose();
            richTextBox8.Dispose();
            richTextBox9.Dispose();
            //if (richTextBox15 != null)
            //    richTextBox15.Dispose();
            if (comboBox1!= null)
                comboBox1.Dispose();

            label1.Dispose();
            label2.Dispose();
            label3.Dispose();
            label4.Dispose();
            label5.Dispose();
            label6.Dispose();
            label7.Dispose();
            label8.Dispose();
            label9.Dispose();
            if (label10 != null)
                label10.Dispose();
            label11.Dispose();

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

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel1.LinkVisited = true;
            var uri = "https://google.com";
            var psi = new System.Diagnostics.ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.FileName = uri;
            System.Diagnostics.Process.Start(psi);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel2.LinkVisited = true;
            var uri = "https://armentavet.com/apt-calc/";
            var psi = new System.Diagnostics.ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.FileName = uri;
            System.Diagnostics.Process.Start(psi); 
        }
    }

    //static class Misc
    //{
    //    public const int DefaultWidth = 390;
    //    public const int DefaultWidthLarge = 1200;
    //    public const int DefaultHeight = 90;
    //    public const float ScaleFactor = 0.7F;
    //    public const float PlaceOne = 200 * ScaleFactor;
    //    public const float DeltaV = 100 * ScaleFactor;
    //    public const float DefaultFont = 18F * ScaleFactor;
    //
    //    public static Point placeCalc(Form thisForm,
    //            Control control, Point location = new System.Drawing.Point(), Place placeh = Place.Center, Place placev = Place.Center)
    //    {
    //        if (placeh == Place.Center)
    //            location.X = thisForm.Width / 2 - control.Width / 2;
    //        else if (placeh == Place.Right)
    //            location.X = thisForm.Width / 2 + control.Width / 2 + control.Width / 4;
    //        else if (placeh == Place.RightOne)
    //            location.X = thisForm.Width / 2 + control.Width / 4 / 2;
    //        else if (placeh == Place.RightTwo)
    //            location.X = thisForm.Width / 2 + control.Width + control.Width / 4 / 2 + control.Width / 4;
    //        else if (placeh == Place.Left)
    //            location.X = thisForm.Width / 2 - control.Width / 2 - control.Width - control.Width / 4;
    //        else if (placeh == Place.LeftOne)
    //            location.X = thisForm.Width / 2 - control.Width - control.Width / 4 / 2;
    //        else if (placeh == Place.LeftTwo)
    //            location.X = thisForm.Width / 2 - control.Width * 2 - control.Width / 4 / 2 - control.Width / 4;
    //        else if (placeh == Place.Start)
    //            location.X = thisForm.Width / 2 - control.Width / 2 - control.Width * 2 - control.Width * 2 / 4;
    //        else if (placeh == Place.End)
    //            location.X = thisForm.Width / 2 + control.Width / 2 + control.Width + control.Width * 2 / 4;
    //
    //        if (placev == Place.One)
    //            location.Y = (int)(PlaceOne + 0 * DeltaV);//200;
    //        else if (placev == Place.Two)
    //            location.Y = (int)(PlaceOne + 1 * DeltaV);//300;
    //        else if (placev == Place.Three)
    //            location.Y = (int)(PlaceOne + 2 * DeltaV);//400;
    //        else if (placev == Place.Four)
    //            location.Y = (int)(PlaceOne + 3 * DeltaV);//500;
    //        else if (placev == Place.Five)
    //            location.Y = (int)(PlaceOne + 4 * DeltaV);//600;
    //        else if (placev == Place.Six)
    //            location.Y = (int)(PlaceOne + 5 * DeltaV);//700;
    //        else if (placev == Place.Seven)
    //            location.Y = (int)(PlaceOne + 6 * DeltaV);//800;
    //        else if (placev == Place.Eight)
    //            location.Y = (int)(PlaceOne + 7 * DeltaV);//900;
    //        else if (placev == Place.Nine)
    //            location.Y = (int)(PlaceOne + 8 * DeltaV);//1000;
    //        else if (placev == Place.Ten)
    //            location.Y = (int)(PlaceOne + 9 * DeltaV);//1100;
    //        else if (placev == Place.End)
    //            location.Y = (int)(PlaceOne + 10 * DeltaV);//1200;
    //
    //        return location;
    //    }
    //
    //    public static void labelDraw(Form thisForm, ref Label label, string text, bool autoSize = true, Font font = null,
    //        Color color = new System.Drawing.Color(), Place placeh = Place.Center, Place placev = Place.Center)
    //    {
    //        label = new System.Windows.Forms.Label();
    //        thisForm.Controls.Add(label);
    //        label.AutoSize = autoSize;
    //        label.Anchor = ((System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Left));//((System.Windows.Forms.AnchorStyles.Right) | (System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Bottom));
    //        if (font != null)
    //            label.Font = font;
    //        if (color != Color.Empty)
    //            label.ForeColor = color;
    //        label.Size = new System.Drawing.Size(DefaultWidth, DefaultHeight);
    //        label.Scale(new SizeF(Misc.ScaleFactor, Misc.ScaleFactor));
    //        label.TabIndex = 20;
    //        label.Text = text;
    //        label.Font = new System.Drawing.Font("Segoe UI", Misc.DefaultFont, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
    //        label.Location = Misc.placeCalc(thisForm, label, placeh: placeh, placev: placev);
    //    }
    //
    //    public static void buttonDraw(Form thisForm, ref Button button, string text, EventHandler eventHandler, Place placeh = Place.Center, Place placev = Place.Center)
    //    {
    //        button = new System.Windows.Forms.Button();
    //        thisForm.Controls.Add(button);
    //        //button.AutoSize = true;
    //        button.Anchor = ((System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Left));//((System.Windows.Forms.AnchorStyles.Right) | (System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Bottom));
    //        button.Margin = new System.Windows.Forms.Padding(4);
    //        button.Size = new System.Drawing.Size(Misc.DefaultWidth, Misc.DefaultHeight);
    //        button.Scale(new SizeF(Misc.ScaleFactor, Misc.ScaleFactor));
    //        button.TabIndex = 3;
    //        button.Text = text;
    //        button.Font = new System.Drawing.Font("Segoe UI", Misc.DefaultFont, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
    //        button.Location = Misc.placeCalc(thisForm, button, placeh: placeh, placev: placev);
    //        button.UseVisualStyleBackColor = true;
    //        button.Click += eventHandler;
    //    }
    //
    //}

}
