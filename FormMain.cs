using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinAMBurner
{
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

        private AM amData = new AM();

        private bool AMConnected = false;

        //private readonly HttpClient client = new HttpClient();
        private readonly Web web = new Web();

        private List<Control> farmIOControls;
        //private List<Misc.Place> farmIOHPlaces = new List<Misc.Place>()
        //{
        //    Misc.Place.LeftOne, Misc.Place.LeftOne, Misc.Place.LeftOne, Misc.Place.LeftOne, Misc.Place.LeftOne, Misc.Place.LeftOne, Misc.Place.LeftOne, Misc.Place.LeftOne,
        //    Misc.Place.RightTwo, Misc.Place.RightTwo, Misc.Place.RightTwo, Misc.Place.RightTwo, Misc.Place.RightTwo, Misc.Place.RightTwo
        //};
        //private List<Misc.Place> farmIOVPlaces = new List<Misc.Place>()
        //{
        //    Misc.Place.Two, Misc.Place.Three, Misc.Place.Four, Misc.Place.Five, Misc.Place.Six, Misc.Place.Seven, Misc.Place.Eight, Misc.Place.Nine,
        //    Misc.Place.Two, Misc.Place.Three, Misc.Place.Four, Misc.Place.Five, Misc.Place.Six, Misc.Place.Seven
        //};
        //private List<Control> farmNameControls;
        //private List<Misc.Place> farmNameHPlaces = new List<Misc.Place>()
        //{
        //    Misc.Place.LeftTwo, Misc.Place.LeftTwo, Misc.Place.LeftTwo, Misc.Place.LeftTwo, Misc.Place.LeftTwo, Misc.Place.LeftTwo, Misc.Place.LeftTwo, Misc.Place.LeftTwo,
        //    Misc.Place.RightOne, Misc.Place.RightOne, Misc.Place.RightOne, Misc.Place.RightOne, Misc.Place.RightOne, Misc.Place.RightOne,
        //    Misc.Place.LeftOne
        //};
        //private List<Misc.Place> farmNameVPlaces = new List<Misc.Place>()
        //{
        //    Misc.Place.Two, Misc.Place.Three, Misc.Place.Four, Misc.Place.Five, Misc.Place.Six, Misc.Place.Seven, Misc.Place.Eight, Misc.Place.Nine,
        //    Misc.Place.Two, Misc.Place.Three, Misc.Place.Four, Misc.Place.Five, Misc.Place.Six, Misc.Place.Seven,
        //    Misc.Place.Ten
        //};

        private List<Farm> farms;
        private Farm farm;
        private DataTable farmTable;

        private List<Control> serviceIOControls;
        //private List<Misc.Place> serviceIOHPlaces = new List<Misc.Place>()
        //{
        //    Misc.Place.LeftOne, Misc.Place.LeftOne, Misc.Place.LeftOne, Misc.Place.LeftOne, Misc.Place.LeftOne, Misc.Place.LeftOne, Misc.Place.LeftOne, Misc.Place.LeftOne,
        //    Misc.Place.RightTwo, Misc.Place.RightTwo, Misc.Place.RightTwo, Misc.Place.RightTwo, Misc.Place.RightTwo, Misc.Place.RightTwo
        //};
        //private List<Misc.Place> serviceIOVPlaces = new List<Misc.Place>()
        //{
        //    Misc.Place.Two, Misc.Place.Three, Misc.Place.Four, Misc.Place.Five, Misc.Place.Six, Misc.Place.Seven, Misc.Place.Eight, Misc.Place.Nine,
        //    Misc.Place.Two, Misc.Place.Three, Misc.Place.Four, Misc.Place.Five, Misc.Place.Six, Misc.Place.Seven
        //};
        //private List<Control> serviceNameControls;
        //private List<Misc.Place> serviceNameHPlaces = new List<Misc.Place>()
        //{
        //    Misc.Place.LeftTwo, Misc.Place.LeftTwo, Misc.Place.LeftTwo, Misc.Place.LeftTwo, Misc.Place.LeftTwo, Misc.Place.LeftTwo, Misc.Place.LeftTwo, Misc.Place.LeftTwo,
        //    Misc.Place.RightOne, Misc.Place.RightOne, Misc.Place.RightOne, Misc.Place.RightOne, Misc.Place.RightOne, Misc.Place.RightOne,
        //    Misc.Place.LeftOne
        //};
        //private List<Misc.Place> serviceNameVPlaces = new List<Misc.Place>()
        //{
        //    Misc.Place.Two, Misc.Place.Three, Misc.Place.Four, Misc.Place.Five, Misc.Place.Six, Misc.Place.Seven, Misc.Place.Eight, Misc.Place.Nine,
        //    Misc.Place.Two, Misc.Place.Three, Misc.Place.Four, Misc.Place.Five, Misc.Place.Six, Misc.Place.Seven,
        //    Misc.Place.Ten
        //};
        private List<Service> services;
        private Service service;
        private DataTable serviceTable;

        private string tabletNo = null;

        private Login login;
        //private string token;
        private User user;

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
            this.Scale(new SizeF(Gui.ScaleFactor, Gui.ScaleFactor));
            this.Font = new System.Drawing.Font("Segoe UI", Gui.DefaultFont, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            //pictureBoxTitle.Scale(new SizeF(Misc.ScaleFactor, Misc.ScaleFactor));

            farmIOControls = new List<Control>()
            {
                richTextBox1, richTextBox2, richTextBox3, richTextBox4, richTextBox5, richTextBox6, richTextBox7, richTextBox8,
                comboBox1, comboBox2,
                richTextBox9, richTextBox10,
                comboBox3, comboBox4, comboBox5
            };

            //farmNameControls = new List<Control>()
            //{
            //    label1, label2, label3, label4, label5, label6, label7, label8, label9, label10, label11, label12, label13, label14, label15, label16,
            //    button1, button2
            //};

            serviceIOControls = new List<Control>()
            {
                richTextBox1, richTextBox2, richTextBox3, richTextBox4, richTextBox5, richTextBox6, richTextBox7, richTextBox8, richTextBox9,
                comboBox1
            };

            //serviceNameControls = new List<Control>()
            //{
            //    label1, label2, label3, label4, label5, label6, label7, label8, label9, label10, label11, 
            //    button1, button2
            //};

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
            //richTextBox1.Dispose();
            //richTextBox2.Dispose();
            //linkLabel1.Dispose();
            //button1.Dispose();
            //foreach (Control control in this.Controls)

            while(this.Controls.Count > 0)
                this.Controls[0].Dispose();
        }

        private void screenLoginShow()
        {
            Gui.draw<PictureBox>(this, width: 500, height: 130, placev: Gui.Place.Zero);
            //Gui.linkLabelDraw(this, ref linkLabel1, "Forgot password",
            Gui.draw<LinkLabel>(this, text: "Forgot password",
                linkLabelLinkClickedEventHandler: new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked), 
                placev: Gui.Place.Five);
            //Gui.buttonDraw(this, ref button1, "Login", new EventHandler(buttonLogin_Click), placev: Gui.Place.End);
            Gui.draw<Button>(this, text: "Login", eventHandler: new EventHandler(buttonLogin_Click), placev: Gui.Place.End);
            //Gui.textBoxDraw(this, ref richTextBox1, "Username", placev: Gui.Place.One);
            Gui.draw<RichTextBox>(this, text: "Username", width: Gui.DefaultWidthLarge, placev: Gui.Place.One);
            //Gui.textBoxDraw(this, ref richTextBox2, "Password", placev: Gui.Place.Three);
            Gui.draw<RichTextBox>(this, text: "Password", width: Gui.DefaultWidthLarge, placev: Gui.Place.Three);
        }

        //private void textBoxEnter_Click(object sender, EventArgs e)
        //{
        //    Control control = sender as Control;
        //    string text = control.Name;
        //    if (control.Text == text)
        //    {
        //        control.Text = "";
        //        control.ForeColor = Color.Black;
        //    }
        //}
        //
        //private void textBoxLeave_Click(object sender, EventArgs e)
        //{
        //    Control control = sender as Control;
        //    string text = control.Name;
        //    if (control.Text == string.Empty)
        //    {
        //        control.Text = text;
        //        control.ForeColor = Color.Silver;
        //    }
        //}

        //private void linkLabelDraw(ref LinkLabel linkLabel, string text, LinkLabelLinkClickedEventHandler eventHandler,
        //    Gui.Place placeh = Gui.Place.Center, Gui.Place placev = Gui.Place.Center)
        //{
        //    linkLabel = new System.Windows.Forms.LinkLabel();
        //    Controls.Add(linkLabel);
        //    linkLabel.AutoSize = true;
        //    linkLabel.Anchor = ((System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Left));//((System.Windows.Forms.AnchorStyles.Right) | (System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Bottom));
        //    linkLabel.Size = new System.Drawing.Size(Gui.DefaultWidth, Gui.DefaultHeight);
        //    linkLabel.Scale(new SizeF(Gui.ScaleFactor, Gui.ScaleFactor));
        //    linkLabel.TabIndex = 1;
        //    linkLabel.TabStop = true;
        //    linkLabel.Text = text;
        //    linkLabel.Font = new System.Drawing.Font("Segoe UI", Gui.DefaultFont, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        //    linkLabel.Location = Gui.placeCalc(this, linkLabel, placeh: placeh, placev: placev);
        //    linkLabel.LinkClicked += eventHandler;
        //}
        //
        //private void textBoxDraw(ref RichTextBox textBox, string text, Gui.Place placeh = Gui.Place.Center, Gui.Place placev = Gui.Place.Center)
        //{
        //    textBox = new System.Windows.Forms.RichTextBox();
        //    Controls.Add(textBox);
        //    //textBox.AutoSize = true;
        //    textBox.Anchor = ((System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Left));//((System.Windows.Forms.AnchorStyles.Right) | (System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Bottom));
        //    textBox.Margin = new System.Windows.Forms.Padding(4);
        //    textBox.Size = new System.Drawing.Size(Gui.DefaultWidthLarge, Gui.DefaultHeight);
        //    textBox.Scale(new SizeF(Gui.ScaleFactor, Gui.ScaleFactor));
        //    textBox.TabIndex = 1;
        //    textBox.Text = text;
        //    textBox.Name = text;
        //    textBox.Font = new System.Drawing.Font("Segoe UI", Gui.DefaultFont, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        //    textBox.ForeColor = Color.Silver;
        //    textBox.Location = Gui.placeCalc(this, textBox, placeh: placeh, placev: placev);
        //    textBox.Enter += new System.EventHandler(textBoxEnter_Click);
        //    textBox.Leave += new System.EventHandler(textBoxLeave_Click);
        //}

        private async void buttonLogin_Click(object sender, EventArgs e)
        {
            login = new Login()
            { 
                email = "yaelv@armentavet.com",//richTextBox1.Text,
                password = "yaelv123",//richTextBox2.Text,
                tablet = 1//TabletNo
            };
            LoginResponse loginResponse= await web.loginPost(login);
            if(loginResponse != null)
            {
                // if ok
                //token = loginResponse.token;
                user = loginResponse.user;
                screenLoginHide();
                screenActionShow();
                JsonDocument farmResponse = await web.farmOptions();
            }
            else
            {
                // if failed
                FormNotify formNotify = new FormNotify(new List<string>() {
                    "Login failed Check your username and password,",
                    "make sure your tablet is connected to the internet"},
                    NotifyButtons.OK, caption: "Login Failed");
                formNotify.ShowDialog();
                formNotify.Dispose();
            }
        }

        //private async Task<LoginResponse> loginPost(Login login)
        //{
        //    var response = await client.PostAsync("https://90fd534a-bb34-45c5-8622-5a1bba51db6e.mock.pstmn.io/" + "api/p/login/",
        //        new StringContent(JsonSerializer.Serialize(login), Encoding.UTF8, "application/json"));
        //    var loginResponse = await JsonSerializer.DeserializeAsync<LoginResponse>(await response.Content.ReadAsStreamAsync());
        //    return loginResponse;
        //
        //    //var request = new HttpRequestMessage(new HttpMethod("POST"), "https://90fd534a-bb34-45c5-8622-5a1bba51db6e.mock.pstmn.io/" + "api/p/login/");
        //    //
        //    //Dictionary<string, string> postParams = new Dictionary<string, string>() 
        //    //{ { "email", richTextBox1.Text }, { "password", richTextBox2.Text }, { "tablet", TabletNo } };
        //    //
        //    //foreach (var param in postParams)
        //    //{
        //    //
        //    //    request.Headers.Add(param.Key, param.Value);
        //    //
        //    //}
        //    //response = await client.SendAsync(request);
        //    //
        //    //var responseString = await response.Content.ReadAsStringAsync();
        //    //
        //    //return response;
        //}

        //private async Task<List<Farm>> farmsGet()
        //{
        //    var streamTask = client.GetStreamAsync("https://90fd534a-bb34-45c5-8622-5a1bba51db6e.mock.pstmn.io/" + "api/p/farms/0");
        //    var farms = await JsonSerializer.DeserializeAsync<List<Farm>>(await streamTask);
        //    return farms;
        //}

        //private async Task<List<Service>> servicesGet()
        //{
        //    var streamTask = client.GetStreamAsync("https://90fd534a-bb34-45c5-8622-5a1bba51db6e.mock.pstmn.io/" + "api/p/service_providers/0");
        //    var services = await JsonSerializer.DeserializeAsync<List<Service>>(await streamTask);
        //    return services;
        //}
        //
        //private async Task<FarmResponse> farmAdd(Farm farm)
        //{
        //    var response = await client.PostAsync("https://90fd534a-bb34-45c5-8622-5a1bba51db6e.mock.pstmn.io/" + "api/p/farms/",
        //        new StringContent(JsonSerializer.Serialize(farm), Encoding.UTF8, "application/json"));
        //    var farmResponse = await JsonSerializer.DeserializeAsync<FarmResponse>(await response.Content.ReadAsStreamAsync());
        //    return farmResponse;
        //}
        //
        //private async Task<FarmResponse> farmEdit(Farm farm)
        //{
        //    var response = await client.PatchAsync("https://90fd534a-bb34-45c5-8622-5a1bba51db6e.mock.pstmn.io/" + "api/p/farms/1/",
        //        new StringContent(JsonSerializer.Serialize(farm), Encoding.UTF8, "application/json"));
        //    var farmResponse = await JsonSerializer.DeserializeAsync<FarmResponse>(await response.Content.ReadAsStreamAsync());
        //    return farmResponse;
        //}
        //
        //private async Task<ServiceResponse> serviceAdd(Service service)
        //{
        //    var response = await client.PostAsync("https://90fd534a-bb34-45c5-8622-5a1bba51db6e.mock.pstmn.io/" + "api/p/service_providers/",
        //        new StringContent(JsonSerializer.Serialize(service), Encoding.UTF8, "application/json"));
        //    var serviceResponse = await JsonSerializer.DeserializeAsync<ServiceResponse>(await response.Content.ReadAsStreamAsync());
        //    return serviceResponse;
        //}
        //
        //private async Task<ServiceResponse> serviceEdit(Service service)
        //{
        //    var response = await client.PatchAsync("https://90fd534a-bb34-45c5-8622-5a1bba51db6e.mock.pstmn.io/" + "api/p/service_providers/1/",
        //        new StringContent(JsonSerializer.Serialize(service), Encoding.UTF8, "application/json"));
        //    var serviceResponse = await JsonSerializer.DeserializeAsync<ServiceResponse>(await response.Content.ReadAsStreamAsync());
        //    return serviceResponse;
        //}

        //private async Task<HttpResponseMessage> farmAddEdit(Farm farm, HttpRequestMessage request)
        //{
        //    Dictionary<string, string> postParams = new Dictionary<string, string>()
        //    {
        //          {"name", farm.name},
        //          {"farm_type", farm.farm_type },
        //          {"breed_type", farm.breed_type },
        //          {"address", farm.address},
        //          {"country", farm.country },
        //          {"city", farm.city },
        //          {"state", farm.state },
        //          {"mobile", farm.mobile },
        //          {"location_of_treatment_type", farm.location_of_treatment_type.ToString() },
        //          {"number_of_lactating_cows", farm.number_of_lactating_cows.ToString()},
        //          {"dhi_test", farm.dhi_test.ToString()},
        //          {"contract_type", farm.contract_type },
        //          {"milking_setup_type", farm.milking_setup_type}
        //    };
        //
        //
        //    foreach (var param in postParams)
        //    {
        //
        //        request.Headers.Add(param.Key, param.Value);
        //
        //    }
        //    var response = await client.SendAsync(request);
        //
        //    var responseString = await response.Content.ReadAsStringAsync();
        //
        //    return response;
        //}

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
            Gui.labelDraw(this, ref label1, "Welcome distributor", placev: Gui.Place.One,
                font: new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point));
            Gui.labelDraw(this, ref label2, "Choose Action: ", placev: Gui.Place.Three);
            Gui.buttonDraw(this, ref button1, "Update AM", new System.EventHandler(buttonUpdateAM_Click), placev: Gui.Place.Four);
            Gui.buttonDraw(this, ref button2, "Manage Farms", new System.EventHandler(buttonFarm_Click), placev: Gui.Place.Five);
            Gui.buttonDraw(this, ref button3, "Manage Service provider", new System.EventHandler(buttonService_Click), placev: Gui.Place.Six);
            Gui.linkLabelDraw(this, ref linkLabel2, "Calculate your farm’s profits with APT", 
                new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked), placev: Gui.Place.Eight);
        }

        private void buttonUpdateAM_Click(object sender, EventArgs e)
        {
            screenActionHide();
            screenConnectShow();
        }

        private async void buttonFarm_Click(object sender, EventArgs e)
        {
            await FarmTableGet();
            screenActionHide();
            screenFarmShow();
        }

        private async Task FarmTableGet()
        {
            farms = await web.farmsGet();
            if(farms != null)
            {
                farmTable = new DataTable();
                farmTable.Columns.AddRange(new List<DataColumn>(){new DataColumn("Farm Name"),
                                    new DataColumn("Country / State"),
                                    new DataColumn("City"),
                                    new DataColumn("Address")}.ToArray());
                //foreach (var f in farms.Select(f => new { f.name, f.country, f.city, f.address }).ToArray())
                foreach (var f in farms.Select(f => new List<string> { f.name, f.country, f.city, f.address }))
                    farmTable.Rows.Add(f.ToArray<string>());
            }
        }

        private async void buttonService_Click(object sender, EventArgs e)
        {
            screenActionHide();
            await serviceTableGet();
            screenServiceShow();
        }

        private async Task serviceTableGet()
        {
            services = await web.servicesGet();
            if (services != null)
            {
                serviceTable = new DataTable();
                serviceTable.Columns.AddRange(new List<DataColumn>(){new DataColumn("Name"),
                                    new DataColumn("Country / State"),
                                    new DataColumn("City"),
                                    new DataColumn("Address")}.ToArray());
                //foreach (var f in farms.Select(f => new { f.name, f.country, f.city, f.address }).ToArray())
                foreach (var s in services.Select(s => new List<string> { s.name, s.country, s.city, s.address }))
                    serviceTable.Rows.Add(s.ToArray<string>());
            }
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
            Gui.labelDraw(this, ref label1, "Welcome distributor", placev: Gui.Place.One,
                font: new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point));
            Gui.labelDraw(this, ref label2, "Please make sure the AM is connected to your tablet before continue", placev: Gui.Place.Three);
            Gui.labelDraw(this, ref label3, "", placev: Gui.Place.Five);
            Gui.progressBarDraw(this, ref progressBar1, placev: Gui.Place.Five);
            Gui.buttonDraw(this, ref button1, "Check AM present", new EventHandler(buttonCheckAM_Click), placeh: Gui.Place.Left, placev: Gui.Place.End);
            Gui.buttonDraw(this, ref button2, "Forward", new EventHandler(buttonConnectForward_Click), placeh: Gui.Place.Right, placev: Gui.Place.End);
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
                FormNotify formNotify = new FormNotify(new List<string>()
                    { "AM not found make sure the AM is connected",
                        "to the tablet by using a USB cable" },
                    NotifyButtons.OK, caption: "Am not connected");
                formNotify.ShowDialog();
                formNotify.Dispose();
            }
            label3.Visible = true;
            button1.Enabled = true;
        }

        private void AMDisconnectedShow()
        {
            Gui.labelDraw(this, label3, new System.Drawing.Point(360, 620), "AM not found – make sure AM is connected using USB cable", Color.Red,
                Gui.Place.Center, Gui.Place.Five);
            button2.Enabled = false;
        }

        private void AMConnectedShow()
        {
            Gui.labelDraw(this, label3, new System.Drawing.Point(360, 620), "AM found – connected to AM", Color.Green,
                Gui.Place.Center, Gui.Place.Five);
            button2.Enabled = true;
        }

        //private void labelDraw(Label label, Point location, string text, Color color, Gui.Place placeh, Gui.Place placev)
        //{
        //    label.ForeColor = color;
        //    label.Text = text;
        //    label.Font = new System.Drawing.Font("Segoe UI", Gui.DefaultFont, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        //    label.Location = Gui.placeCalc(this, label, location: location, placeh: placeh, placev: placev);
        //}

        private void buttonConnectForward_Click(object sender, EventArgs e)
        {
            screenConnectHide();
            screenInfoShow();
        }

        //private void progressBarDraw(ref ProgressBar progressBar, Gui.Place placeh = Gui.Place.Center, Gui.Place placev = Gui.Place.Center)
        //{
        //    progressBar = new System.Windows.Forms.ProgressBar();
        //    Controls.Add(progressBar);
        //    //progressBar.AutoSize = true;
        //    progressBar.Anchor = ((System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Left));//((System.Windows.Forms.AnchorStyles.Right) | (System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Bottom));
        //    progressBar.Margin = new System.Windows.Forms.Padding(4);
        //    progressBar.Size = new System.Drawing.Size(Gui.DefaultWidthLarge, 24);
        //    progressBar.Scale(new SizeF(Gui.ScaleFactor, Gui.ScaleFactor));
        //    progressBar.Location = Gui.placeCalc(this, progressBar, placev: placev);
        //    progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
        //    progressBar.TabIndex = 18;
        //    progressBar.Visible = false;
        //}

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
            Gui.labelDraw(this, ref label1, "Welcome distributor", placev: Gui.Place.One,
                font: new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point));
            Gui.labelDraw(this, ref label2, "AM information, pulses per treatment : " + 1500, placev: Gui.Place.Three);
            Gui.labelDraw(this, ref label3, "AM identified with SN: " + amData.SNum, placev: Gui.Place.Five);
            Gui.labelDraw(this, ref label4, "Current available treatments: " + amData.Maxi, placev: Gui.Place.Seven);
            Gui.buttonDraw(this, ref button1, "Back", new EventHandler(buttonInfoBack_Click), placeh: Gui.Place.Left, placev: Gui.Place.End);
            Gui.buttonDraw(this, ref button2, "Continue", new EventHandler(buttonInfoContinue_Click), placeh: Gui.Place.Right, placev: Gui.Place.End);
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
            Gui.labelDraw(this, ref label1, "Welcome distributor", placev: Gui.Place.One,
                font: new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point));
            Gui.labelDraw(this, ref label2, "Select Farm / Service provider", placev: Gui.Place.Three);
            Gui.comboBoxDraw(this, ref comboBox1, new System.EventHandler(comboBox1_SelectedIndexChanged), placev: Gui.Place.Four);
            comboBox1.Items.AddRange(new string[] { "farm1", "farm2", "farm3", "farm4", "farm5", "farm6" });
            Gui.labelDraw(this, ref label3, "Add treatments to AM – SN" + amData.SNum, placev: Gui.Place.Five);
            Gui.comboBoxDraw(this, ref comboBox2, new System.EventHandler(comboBox2_SelectedIndexChanged), placev: Gui.Place.Six);
            comboBox2.Items.AddRange(new string[] { "50", "100", "150", "200", "250", "300" });
            Gui.progressBarDraw(this, ref progressBar1, placev: Gui.Place.Seven);
            Gui.buttonDraw(this, ref button1, "Cancel", new EventHandler(buttonTreatCansel_Click), placeh: Gui.Place.Left, placev: Gui.Place.End);
            Gui.buttonDraw(this, ref button2, "Approve", new EventHandler(buttonTreatApprove_Click), placeh: Gui.Place.Right, placev: Gui.Place.End);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
        
        //private void controlDraw(ref Control control, string text, List<string> items, EventHandler eventHandler, Misc.Place placeh = Misc.Place.Center, Misc.Place placev = Misc.Place.Center)
        //{
        //    control = control.GetType().GetConstructor(
        //        BindingFlags.Instance | BindingFlags.Public, null,
        //        CallingConventions.HasThis, null, null).Invoke(null) as Control;
        //    Controls.Add(control);
        //    foreach (PropertyInfo prop in control.GetType().GetProperties())
        //    {
        //        //Console.WriteLine("{0} = {1}", prop.Name, prop.GetValue(user, null));
        //        if (prop.Name == "Anchor")
        //            prop.SetValue(control, ((System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Left)));
        //        if (prop.Name == "FormattingEnabled")
        //            prop.SetValue(control, true);
        //    }
        //}
       
        //private void comboBoxSmallDraw(ref ComboBox comboBox, string text, List<string> items, EventHandler eventHandler, Gui.Place placeh = Gui.Place.Center, Gui.Place placev = Gui.Place.Center)
        //{
        //    comboBox = new System.Windows.Forms.ComboBox();
        //    Controls.Add(comboBox);
        //    //comboBox.AutoSize = true;
        //    comboBox.Anchor = ((System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Left));//((System.Windows.Forms.AnchorStyles.Right) | (System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Bottom));
        //    comboBox.FormattingEnabled = true;
        //    comboBox.Margin = new System.Windows.Forms.Padding(4);
        //    comboBox.Size = new System.Drawing.Size(Gui.DefaultWidth, Gui.DefaultHeight);
        //    comboBox.Scale(new SizeF(Gui.ScaleFactor, Gui.ScaleFactor));
        //    comboBox.Font = new System.Drawing.Font("Segoe UI", Gui.DefaultFont, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        //    comboBox.Location = Gui.placeCalc(this, comboBox, placeh: placeh, placev: placev);
        //    comboBox.TabIndex = 17;
        //    comboBox.SelectedIndexChanged += eventHandler;
        //    comboBox.Items.AddRange(items.ToArray());
        //    comboBox.Text = text;
        //}
        //
        //private void comboBoxDraw(ref ComboBox comboBox, EventHandler eventHandler, Gui.Place placeh = Gui.Place.Center, Gui.Place placev = Gui.Place.Center)
        //{
        //    comboBox = new System.Windows.Forms.ComboBox();
        //    Controls.Add(comboBox);
        //    //comboBox.AutoSize = true;
        //    comboBox.Anchor = ((System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Left));//((System.Windows.Forms.AnchorStyles.Right) | (System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Bottom));
        //    comboBox.FormattingEnabled = true;
        //    comboBox.Margin = new System.Windows.Forms.Padding(4);
        //    comboBox.Size = new System.Drawing.Size(Gui.DefaultWidthLarge, Gui.DefaultHeightLarge);
        //    comboBox.Scale(new SizeF(Gui.ScaleFactor, Gui.ScaleFactor));
        //    comboBox.Font = new System.Drawing.Font("Segoe UI", Gui.DefaultFont, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        //    comboBox.Location = Gui.placeCalc(this, comboBox, placeh: placeh, placev: placev);
        //    comboBox.TabIndex = 17;
        //    comboBox.SelectedIndexChanged += eventHandler;
        //}
        
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
            FormNotify formNotify = new FormNotify(new List<string>() {
                "Are you sure you want to cancel the operation?"},
                NotifyButtons.YesNo, caption: "Abort");
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
                FormNotify formNotify = new FormNotify(new List<string>() {
                    string.Format("{0} treatments updated,",amData.MaxiSet),
                    string.Format("{0} treatments available on AM - SN {1},", amData.Maxi, amData.SNum),
                    "please disconnect the AM"},
                    NotifyButtons.OK, caption: "Success");
                formNotify.ShowDialog();
                formNotify.Dispose();
            }
            else if (errcode == ErrCode.EPARAM)
            {
                // wrong parameters
                FormNotify formNotify = new FormNotify(new List<string>() {
                    "Wrong parameters,",
                    "please choose the number of treatments"},
                    NotifyButtons.OK, caption: "Fail");
                formNotify.ShowDialog();
                formNotify.Dispose();
            }
            else
            {
                // if fail
                AMConnected = false;
                FormNotify formNotify = new FormNotify(new List<string>() {
                    "The operation failed, the treatments were not added"},
                    NotifyButtons.OK, caption: "Fail");
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
            Gui.labelDraw(this, ref label2, dataName, placev: Gui.Place.One);
            Gui.buttonDraw(this, ref button1, "Edit", eventHandlerButton1, placeh: Gui.Place.RightOne, placev: Gui.Place.Two);
            Gui.buttonDraw(this, ref button2, "Add New", eventHandlerButton2, placeh: Gui.Place.RightTwo, placev: Gui.Place.Two);
            Gui.textBoxSmallDraw(this, ref richTextBox1, "Search", "Search", placeh: Gui.Place.LeftOne, placev: Gui.Place.Two);
            Gui.buttonDraw(this, ref button3, "Back", eventHandler3, placeh: Gui.Place.LeftTwo, placev: Gui.Place.Two);
            Gui.dataGridDraw(this, ref dataGridView1, placev: Gui.Place.Three);
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

        //private void dataGridDraw(ref DataGridView dataGridView, Gui.Place placeh = Gui.Place.Center, Gui.Place placev = Gui.Place.Center)
        //{
        //  this.components = new System.ComponentModel.Container();
        //    //this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
        //    dataGridView = new System.Windows.Forms.DataGridView();
        //    //((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
        //    ((System.ComponentModel.ISupportInitialize)(dataGridView)).BeginInit();
        //    //dataGridView.AutoSize = true;
        //    dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Left));//((System.Windows.Forms.AnchorStyles.Right) | (System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Bottom));
        //    dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        //    dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
        //    dataGridView.RowHeadersWidth = 62;
        //    dataGridView.RowTemplate.Height = 33;
        //    dataGridView.Size = new System.Drawing.Size(1853, 800);
        //    dataGridView.Scale(new SizeF(Gui.ScaleFactor, Gui.ScaleFactor));
        //    dataGridView.Font = new System.Drawing.Font("Segoe UI", Gui.DefaultFont, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        //    dataGridView.Location = Gui.placeCalc(this, dataGridView, placeh: placeh, placev: placev);
        //    dataGridView.TabIndex = 6;
        //    Controls.Add(dataGridView);
        //    //((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
        //    ((System.ComponentModel.ISupportInitialize)(dataGridView)).EndInit();
        //    //this.bindingSource1.DataSource = new DataSet1.DataTable1DataTable();
        //    //dataGridView.DataSource = this.bindingSource1;
        //
        //}

        //private void textBoxSmallDraw(ref RichTextBox textBox, string text, string name, Gui.Place placeh = Gui.Place.Center, Gui.Place placev = Gui.Place.Center)
        //{
        //    textBox = new System.Windows.Forms.RichTextBox();
        //    Controls.Add(textBox);
        //    //textBox.AutoSize = true;
        //    textBox.Anchor = ((System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Left));//((System.Windows.Forms.AnchorStyles.Right) | (System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Bottom));
        //    textBox.Margin = new System.Windows.Forms.Padding(4);
        //    textBox.Size = new System.Drawing.Size(Gui.DefaultWidth, Gui.DefaultHeight);
        //    textBox.Scale(new SizeF(Gui.ScaleFactor, Gui.ScaleFactor));
        //    textBox.TabIndex = 1;
        //    textBox.Text = text;
        //    textBox.Name = name;
        //    textBox.Font = new System.Drawing.Font("Segoe UI", Gui.DefaultFont, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        //    if (name == text)
        //        textBox.ForeColor = Color.Silver;
        //    textBox.Location = Gui.placeCalc(this, textBox, placeh: placeh, placev: placev);
        //    textBox.Enter += new System.EventHandler(textBoxEnter_Click);
        //    textBox.Leave += new System.EventHandler(textBoxLeave_Click);
        //}

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
            farm = new Farm();
            screenFarmUpdateShow(farm);
            Gui.labelDraw(this, ref label1, "Add Farm", placev: Gui.Place.One,
                font: new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point));
            Gui.comboBoxSmallDraw(this, ref comboBox5, farm.contract_type, Farm.CONTRACT_TYPE, new System.EventHandler(comboBoxFarm5_SelectedIndexChanged), placeh: Gui.Place.RightTwo, placev: Gui.Place.Eight);
            Gui.labelDraw(this, ref label16, "Contract:", autoSize: false, placeh: Gui.Place.RightOne, placev: Gui.Place.Eight);
            //locationStart = 0;
            Gui.buttonDraw(this, ref button2, "Submit", new System.EventHandler(buttonFarmAddSubmit_Click), placeh: Gui.Place.RightTwo,
                placev: Gui.Place.Ten);
        }

        private void comboBoxFarm5_SelectedIndexChanged(object sender, EventArgs e)
        {
            //farm.ContractType = (sender as ComboBox).SelectedItem as string;
        }

        private void screenFarmEditShow(Farm farm)
        {
            screenFarmUpdateShow(farm);
            Gui.labelDraw(this, ref label1, "Edit Farm", placev: Gui.Place.One,
                font: new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point));
            Gui.buttonDraw(this, ref button2, "Submit", new System.EventHandler(buttonFarmEditSubmit_Click), placeh: Gui.Place.RightTwo,
                placev: Gui.Place.Ten);
        }

        //private void screenFarmUpdateShow<T>(Farm farm, List<Control> ioControls, List<Control> nameControls, List<Misc.Place> placesh, List<Misc.Place> placesv)
        private void screenFarmUpdateShow(Farm farm)
        {
            //for(int i = 0; i < ioControls.Count; i ++)
            //{
            //    if(ioControls.GetType() == typeof(RichTextBox))
            //    {
            //        RichTextBox control = ioControls[i] as RichTextBox;
            //        textBoxSmallDraw(ref control, farm.Name, Farm.NAME, placeh: placesh[i], placev: placesv[i]);
            //    }
            //    if (ioControls.GetType() == typeof(ComboBox))
            //    {
            //        ComboBox control = ioControls[i] as ComboBox;
            //        comboBoxSmallDraw(ref control, farm.FarmType, Farm.FARM_TYPE, new System.EventHandler(comboBoxFarm1_SelectedIndexChanged), placeh: Misc.Place.RightTwo, placev: Misc.Place.Two);
            //    }
            //}

            //CultureInfo[] ci = CultureInfo.GetCultures(CultureTypes.AllCultures & ~CultureTypes.NeutralCultures);
            //RegionInfo region = new RegionInfo(ci[0].Name);

            //locationIdx = locationStart;

            Gui.textBoxSmallDraw(this, ref richTextBox1, farm.Name, Farm.NAME, placeh: Gui.Place.LeftOne, placev: Gui.Place.Two);
            Gui.textBoxSmallDraw(this, ref richTextBox2, farm.Address, Farm.ADDRESS, placeh: Gui.Place.LeftOne, placev: Gui.Place.Three);
            Gui.comboBoxSmallDraw1(this, ref comboBox1, farm.Country, Farm.COUNTRY.Values.ToList(), new System.EventHandler(comboBoxFarm4_SelectedIndexChanged), placeh: Gui.Place.LeftOne, placev: Gui.Place.Four);
            Gui.textBoxSmallDraw(this, ref richTextBox4, farm.State, Farm.STATE, placeh: Gui.Place.LeftOne, placev: Gui.Place.Five);
            Gui.textBoxSmallDraw(this, ref richTextBox5, farm.City, Farm.CITY, placeh: Gui.Place.LeftOne, placev: Gui.Place.Six);
            Gui.textBoxSmallDraw(this, ref richTextBox6, farm.Contact, Farm.CONTACT, placeh: Gui.Place.LeftOne, placev: Gui.Place.Seven);
            Gui.textBoxSmallDraw(this, ref richTextBox7, farm.Mobile, Farm.MOBILE, placeh: Gui.Place.LeftOne, placev: Gui.Place.Eight);
            Gui.textBoxSmallDraw(this, ref richTextBox8, "", Farm.EMAIL, placeh: Gui.Place.LeftOne, placev: Gui.Place.Nine);
            Gui.comboBoxSmallDraw(this, ref comboBox1, farm.FarmType, Farm.FARM_TYPE, new System.EventHandler(comboBoxFarm1_SelectedIndexChanged), placeh: Gui.Place.RightTwo, placev: Gui.Place.Two);
            Gui.comboBoxSmallDraw(this, ref comboBox2, farm.BreedType, Farm.BREED_TYPE, new System.EventHandler(comboBoxFarm2_SelectedIndexChanged), placeh: Gui.Place.RightTwo, placev: Gui.Place.Three);
            Gui.textBoxSmallDraw(this, ref richTextBox9, farm.NumberOfLactatingCows, Farm.NUMBER_OF_LACTATING_COWS, placeh: Gui.Place.RightTwo, placev: Gui.Place.Four);
            Gui.textBoxSmallDraw(this, ref richTextBox10, farm.DhiTest, Farm.DHI_TEST, placeh: Gui.Place.RightTwo, placev: Gui.Place.Five);
            Gui.comboBoxSmallDraw(this, ref comboBox3, farm.MilkingSetupType, Farm.MILKING_SETUP_TYPE, new System.EventHandler(comboBoxFarm3_SelectedIndexChanged), placeh: Gui.Place.RightTwo, placev: Gui.Place.Six);
            Gui.comboBoxSmallDraw(this, ref comboBox4, farm.LocationOfTreatmentType, Farm.LOCATION_OF_TREATMENT_TYPE, new System.EventHandler(comboBoxFarm4_SelectedIndexChanged), placeh: Gui.Place.RightTwo, placev: Gui.Place.Seven);

            //locationIdx = locationStart;
            Gui.labelDraw(this, ref label2, "Name of Dairy farm:", autoSize: false, placeh: Gui.Place.LeftTwo, placev: Gui.Place.Two);
            Gui.labelDraw(this, ref label3, "Address:", autoSize: false, placeh: Gui.Place.LeftTwo, placev: Gui.Place.Three);
            Gui.labelDraw(this, ref label4, "Country:", autoSize: false, placeh: Gui.Place.LeftTwo, placev: Gui.Place.Four);
            Gui.labelDraw(this, ref label5, "State:", autoSize: false, placeh: Gui.Place.LeftTwo, placev: Gui.Place.Five);
            Gui.labelDraw(this, ref label6, "City:", autoSize: false, placeh: Gui.Place.LeftTwo, placev: Gui.Place.Six);
            Gui.labelDraw(this, ref label7, "Contact Name:", autoSize: false, placeh: Gui.Place.LeftTwo, placev: Gui.Place.Seven);
            Gui.labelDraw(this, ref label8, "Mobile:", autoSize: false, placeh: Gui.Place.LeftTwo, placev: Gui.Place.Eight);
            Gui.labelDraw(this, ref label9, "Email Address:", autoSize: false, placeh: Gui.Place.LeftTwo, placev: Gui.Place.Nine);
            Gui.labelDraw(this, ref label10, "Farm Type:", autoSize: false, placeh: Gui.Place.RightOne, placev: Gui.Place.Two);
            Gui.labelDraw(this, ref label11, "Breed:", autoSize: false, placeh: Gui.Place.RightOne, placev: Gui.Place.Three);
            Gui.labelDraw(this, ref label12, "# of lactating cows:", autoSize: false, placeh: Gui.Place.RightOne, placev: Gui.Place.Four);
            Gui.labelDraw(this, ref label13, "Monthly DHI test:", autoSize: false, placeh: Gui.Place.RightOne, placev: Gui.Place.Five);
            Gui.labelDraw(this, ref label14, "Milking Setup:", autoSize: false, placeh: Gui.Place.RightOne, placev: Gui.Place.Six);
            Gui.labelDraw(this, ref label15, "Location of Treatment:", autoSize: false, placeh: Gui.Place.RightOne, placev: Gui.Place.Seven);

            //locationIdx = 0;
            Gui.buttonDraw(this, ref button1, "Cancel", new System.EventHandler(buttonFarmCancel_Click), placeh: Gui.Place.LeftOne,
                placev: Gui.Place.Ten);
            //Misc.buttonDraw(this, ref button2, "Submit", new System.EventHandler(buttonFarmSubmit_Click), placeh: Misc.Place.RightTwo,
            //    placev: Misc.Place.Ten);
        }

        private void comboBoxFarm1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //farm.FarmType = (sender as ComboBox).SelectedItem as string;
        }

        private void comboBoxFarm2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //farm.BreedType = (sender as ComboBox).SelectedItem as string;
        }

        private void comboBoxFarm4_SelectedIndexChanged(object sender, EventArgs e)
        {
            //farm.MilkingSetupType = (sender as ComboBox).SelectedItem as string;
        }

        private void comboBoxFarm3_SelectedIndexChanged(object sender, EventArgs e)
        {
            //farm.LocationOfTreatmentType = (sender as ComboBox).SelectedItem as string;
        }

        private void buttonFarmCancel_Click(object sender, EventArgs e)
        {
            //farm = new Farm();
            screenFarmUpdateHide();
            //screenFarmUpdateHide(farmIOControls, farmNameControls);
            screenFarmShow();
        }

        //private void screenFarmUpdateHide(List<Control> ioControls, List<Control> nameControls)
        private void screenFarmUpdateHide()
        {
            //foreach(Control control in ioControls)
            //    if (control != null)
            //        control.Dispose();

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

            //foreach (Control control in nameControls)
            //    if (control != null)
            //        control.Dispose();

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

        private async void buttonFarmAddSubmit_Click(object sender, EventArgs e)
        {
            //farm = new Farm();
            updateParams(farm, farmIOControls);
            JsonDocument farmResponse = await web.farmAdd(farm);
            if(responseParse<FarmJson>(farmResponse) == ErrCode.OK)
            {
                farms.Remove(farm);
                farms.Add(farm);
                farmTable.Rows.Add((new List<string>() { farm.name, farm.country, farm.city, farm.address }).ToArray<string>());

                screenFarmUpdateHide();
                //screenFarmUpdateHide(farmIOControls, farmNameControls);
                screenFarmShow();
            }
        }

        //private void farmParse(FarmResponse farmResponse)
        private ErrCode responseParse<T>(JsonDocument jsonDocument)
        {
            ErrCode errCode = ErrCode.ERROR;
            List<string> texts = new List<string>();

            if (jsonDocument == null)
            { 
                 texts.Add("Error");
            }
            else
            {
                foreach (PropertyInfo prop in typeof(T).GetProperties())
                {
                    //Console.WriteLine("{0} = {1}", prop.Name, prop.GetValue(user, null));
                    string text = "";
                    try { text = jsonDocument.RootElement.GetProperty(prop.Name).EnumerateArray().ElementAt(0).ToString(); }
                    catch { }
                    if (text != "")
                        texts.Add(text);
                }
            }
            if(texts.Count > 0)
            {
                FormNotify formNotify = new FormNotify(texts, NotifyButtons.OK);
                formNotify.ShowDialog();
                formNotify.Dispose();
                return errCode;
            }

            errCode = ErrCode.OK;
            return errCode;
        }

            //if (farmResponse.mobile != "OK")
            //{
            //    FormNotify formNotify = new FormNotify(new string[] { farmResponse.mobile }, NotifyButtons.OK);
            //    formNotify.ShowDialog();
            //    formNotify.Dispose();
            //}
            //else if (farmResponse.country != "OK")
            //{
            //    FormNotify formNotify = new FormNotify(new string[] { farmResponse.country }, NotifyButtons.OK);
            //    formNotify.ShowDialog();
            //    formNotify.Dispose();
            //}
            //else if (farmResponse.state != "OK")
            //{
            //    FormNotify formNotify = new FormNotify(new string[] { farmResponse.state }, NotifyButtons.OK);
            //    formNotify.ShowDialog();
            //    formNotify.Dispose();
            //}
            //else if (farmResponse.name != "OK")
            //{
            //    FormNotify formNotify = new FormNotify(new string[] { farmResponse.name }, NotifyButtons.OK);
            //    formNotify.ShowDialog();
            //    formNotify.Dispose();
            //}
            //else if (farmResponse.breed_type != "OK")
            //{
            //    FormNotify formNotify = new FormNotify(new string[] { farmResponse.breed_type }, NotifyButtons.OK);
            //    formNotify.ShowDialog();
            //    formNotify.Dispose();
            //}
            //else if (farmResponse.location_of_treatment_type != "OK")
            //{
            //    FormNotify formNotify = new FormNotify(new string[] { farmResponse.location_of_treatment_type }, NotifyButtons.OK);
            //    formNotify.ShowDialog();
            //    formNotify.Dispose();
            //}
            //else if (farmResponse.number_of_lactating_cows != "OK")
            //{
            //    FormNotify formNotify = new FormNotify(new string[] { farmResponse.number_of_lactating_cows }, NotifyButtons.OK);
            //    formNotify.ShowDialog();
            //    formNotify.Dispose();
            //}
            //else
            //{
            //    farms.Remove(farm);
            //    farms.Add(farm);
            //    farmTable.Rows.Add((new List<string>() { farm.name, farm.country, farm.city, farm.address }).ToArray<string>());
            //
            //    screenFarmUpdateHide();
            //    screenFarmShow();
            //}
        //}

        private void updateParams<T>(T entity, List<Control> controls)
        {
            PropertyInfo [] props = typeof(T).GetProperties();

            foreach (Control control in controls)
            //foreach (PropertyInfo prop in typeof(T).GetProperties())
            {
                //Console.WriteLine("{0} = {1}", prop.Name, prop.GetValue(user, null));
                if(control != null)
                {
                    PropertyInfo prop = props.ElementAt(controls.IndexOf(control));
                    prop.SetValue(entity, control.Text);
                }
            }
            //farm.Name = richTextBox1.Text;
            //farm.Address = richTextBox2.Text;
            //farm.Country = richTextBox3.Text;
            //farm.State = richTextBox4.Text;
            //farm.City = richTextBox5.Text;
            //farm.Contact = richTextBox6.Text;
            //farm.Mobile = richTextBox7.Text;
            ////"" = richTextBox8
            //farm.FarmType = comboBox1.Text;
            //farm.BreedType = comboBox2.Text;
            //farm.NumberOfLactatingCows = richTextBox9.Text;
            //farm.DhiTest = richTextBox10.Text;
            //farm.MilkingSetupType = comboBox3.Text;
            //farm.LocationOfTreatmentType = comboBox4.Text;
            //if (!comboBox5.IsDisposed)
            //    farm.ContractType = comboBox5.Text;
        }

        private async void buttonFarmEditSubmit_Click(object sender, EventArgs e)
        {
            updateParams(farm, farmIOControls);
            JsonDocument farmResponse = await web.farmEdit(farm);
            if (responseParse<Farm>(farmResponse) == ErrCode.OK)
            {
                farms.Remove(farm);
                farms.Add(farm);
                farmTable.Rows.Add((new List<string>() { farm.name, farm.country, farm.city, farm.address }).ToArray<string>());

                screenFarmUpdateHide();
                //screenFarmUpdateHide(farmIOControls, farmNameControls);
                screenFarmShow();
            }
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
            Gui.labelDraw(this, ref label11, "Add Service provider", placev: Gui.Place.One,
                font: new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point));
            Gui.comboBoxSmallDraw(this, ref comboBox1, (new Service()).contract_type, Service.CONTRACT_TYPE, new System.EventHandler(comboBox1_SelectedIndexChanged), placeh: Gui.Place.RightTwo, placev: Gui.Place.Six);
            //textBoxSmallDraw(ref richTextBox15, "", "Contract:", placeh: Misc.Place.RightTwo, placev: Misc.Place.Six);
            Gui.labelDraw(this, ref label10, "Contract:", autoSize: false, placeh: Gui.Place.RightOne, placev: Gui.Place.Six);
            Gui.buttonDraw(this, ref button2, "Submit", new System.EventHandler(buttonServiceAddSubmit_Click), placeh: Gui.Place.RightTwo,
                placev: Gui.Place.Ten);
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
            Gui.labelDraw(this, ref label11, "Edit Service provider", placev: Gui.Place.One,
                font: new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point));
            Gui.buttonDraw(this, ref button2, "Submit", new System.EventHandler(buttonServiceEditSubmit_Click), placeh: Gui.Place.RightTwo,
                placev: Gui.Place.Ten);
        }

        private void buttonServiceBack_Click(object sender, EventArgs e)
        {
            screenDataGridHide();
            screenActionShow();
        }

        private void screenServiceUpdateShow(Service service)
        {
            Gui.textBoxSmallDraw(this, ref richTextBox1, service.NumberOfDairyFarms, Service.NUMBER_OF_DAIRY_FARMS, placeh: Gui.Place.LeftOne,  placev: Gui.Place.Two);
            Gui.textBoxSmallDraw(this, ref richTextBox2, service.NumberOfDairyCows, Service.NUMBER_OF_DAIRY_COWS, placeh: Gui.Place.LeftOne,  placev: Gui.Place.Three);
            Gui.textBoxSmallDraw(this, ref richTextBox3, service.Address, Service.ADDRESS, placeh: Gui.Place.LeftOne,  placev: Gui.Place.Four);
            Gui.textBoxSmallDraw(this, ref richTextBox4, service.Country, Service.COUNTRY, placeh: Gui.Place.LeftOne,  placev: Gui.Place.Five);
            Gui.textBoxSmallDraw(this, ref richTextBox5, service.State, Service.STATE, placeh: Gui.Place.LeftOne,  placev: Gui.Place.Six);
            Gui.textBoxSmallDraw(this, ref richTextBox6, service.City, Service.CITY, placeh: Gui.Place.RightTwo, placev: Gui.Place.Two);
            Gui.textBoxSmallDraw(this, ref richTextBox7, service.Name, Service.NAME, placeh: Gui.Place.RightTwo, placev: Gui.Place.Three);
            Gui.textBoxSmallDraw(this, ref richTextBox8, service.Mobile, Service.MOBILE, placeh: Gui.Place.RightTwo, placev: Gui.Place.Four);
            Gui.textBoxSmallDraw(this, ref richTextBox9, service.Email, Service.EMAIL, placeh: Gui.Place.RightTwo, placev: Gui.Place.Five);
        
            Gui.labelDraw(this, ref label1, "# of dairy farms:", autoSize: false, placeh: Gui.Place.LeftTwo, placev: Gui.Place.Two);
            Gui.labelDraw(this, ref label2, "# of dairy cows:", autoSize: false, placeh: Gui.Place.LeftTwo, placev: Gui.Place.Three);
            Gui.labelDraw(this, ref label3, "Address:", autoSize: false, placeh: Gui.Place.LeftTwo, placev: Gui.Place.Four);
            Gui.labelDraw(this, ref label4, "Country:", autoSize: false, placeh: Gui.Place.LeftTwo, placev: Gui.Place.Five);
            Gui.labelDraw(this, ref label5, "State:", autoSize: false, placeh: Gui.Place.LeftTwo, placev: Gui.Place.Six);
            Gui.labelDraw(this, ref label6, "City:", autoSize: false, placeh: Gui.Place.RightOne, placev: Gui.Place.Two);
            Gui.labelDraw(this, ref label7, "Name of contact :", autoSize: false, placeh: Gui.Place.RightOne, placev: Gui.Place.Three);
            Gui.labelDraw(this, ref label8, "Mobile:", autoSize: false, placeh: Gui.Place.RightOne, placev: Gui.Place.Four);
            Gui.labelDraw(this, ref label9, "Email Address:", autoSize: false, placeh: Gui.Place.RightOne, placev: Gui.Place.Five);

            Gui.buttonDraw(this, ref button1, "Cancel", new System.EventHandler(buttonServiceCancel_Click), placeh: Gui.Place.LeftOne,
                placev: Gui.Place.Ten);
            //Misc.buttonDraw(this, ref button2, "Submit", new System.EventHandler(buttonServiceSubmit_Click), placeh: Misc.Place.RightTwo,
            //    placev: Misc.Place.Ten);
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
            //screenUpdateHide(serviceIOControls, serviceNameControls);
            screenServiceShow();
        }

        private async void buttonServiceAddSubmit_Click(object sender, EventArgs e)
        {
            //serviceParams();
            updateParams(service, serviceIOControls);
            JsonDocument serviceResponse = await web.serviceAdd(service);
            //serviceParse(serviceResponse);
            if (responseParse<Service>(serviceResponse) == ErrCode.OK)
            {
                services.Remove(service);
                services.Add(service);
                serviceTable.Rows.Add((new List<string>() { service.name, service.country, service.city, service.address }).ToArray<string>());

                screenServiceUpdateHide();
                //screenUpdateHide(serviceIOControls, serviceNameControls);
                screenServiceShow();
            }
        }

        //private void serviceParse(ServiceResponse serviceResponse)
        //{
        //    if (serviceResponse.mobile != "OK")
        //    {
        //        FormNotify formNotify = new FormNotify(new string[] { serviceResponse.mobile }, NotifyButtons.OK);
        //        formNotify.ShowDialog();
        //        formNotify.Dispose();
        //    }
        //    else if (serviceResponse.country != "OK")
        //    {
        //        FormNotify formNotify = new FormNotify(new string[] { serviceResponse.country }, NotifyButtons.OK);
        //        formNotify.ShowDialog();
        //        formNotify.Dispose();
        //    }
        //    else if (serviceResponse.state != "OK")
        //    {
        //        FormNotify formNotify = new FormNotify(new string[] { serviceResponse.state }, NotifyButtons.OK);
        //        formNotify.ShowDialog();
        //        formNotify.Dispose();
        //    }
        //    else if (serviceResponse.email != "OK")
        //    {
        //        FormNotify formNotify = new FormNotify(new string[] { serviceResponse.email }, NotifyButtons.OK);
        //        formNotify.ShowDialog();
        //        formNotify.Dispose();
        //    }
        //    else if (serviceResponse.name != "OK")
        //    {
        //        FormNotify formNotify = new FormNotify(new string[] { serviceResponse.name }, NotifyButtons.OK);
        //        formNotify.ShowDialog();
        //        formNotify.Dispose();
        //    }
        //    else
        //    {
        //        services.Remove(service);
        //        services.Add(service);
        //        serviceTable.Rows.Add((new List<string>() { service.name, service.country, service.city, service.address }).ToArray<string>());
        //
        //        screenServiceUpdateHide();
        //        screenServiceShow();
        //    }
        //}

        //private void serviceParams()
        //{
        //    service.NumberOfDairyFarms = richTextBox1.Text;
        //    service.NumberOfDairyCows = richTextBox2.Text;
        //    service.Address = richTextBox3.Text;
        //    service.Country = richTextBox4.Text;
        //    service.State = richTextBox5.Text;
        //    service.City = richTextBox6.Text;
        //    service.Name = richTextBox7.Text;
        //    service.Mobile = richTextBox8.Text;
        //    service.Email = richTextBox9.Text;
        //    service.ContractType = comboBox1.Text;
        //}

        private async void buttonServiceEditSubmit_Click(object sender, EventArgs e)
        {
            //serviceParams();
            updateParams(service, serviceIOControls);
            JsonDocument serviceResponse = await web.serviceEdit(service);
            //serviceParse(serviceResponse);
            if (responseParse<Service>(serviceResponse) == ErrCode.OK)
            {
                services.Remove(service);
                services.Add(service);
                serviceTable.Rows.Add((new List<string>() { service.name, service.country, service.city, service.address }).ToArray<string>());

                screenServiceUpdateHide();
                //screenUpdateHide(serviceIOControls, serviceNameControls);
                screenServiceShow();
            }
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
}
