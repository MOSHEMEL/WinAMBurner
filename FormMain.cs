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
        private Label label1;
        private Button button1;
        private Button button2;
        private ProgressBar progressBar1;
        //private System.Windows.Forms.BindingSource bindingSource1;
        private DataGridView dataGridView1;
        private LinkLabel linkLabel1;
        private LinkLabel linkLabel2;

        private AM amData = new AM();

        private bool AMConnected = false;

        private readonly Web web = new Web();

        private List<Farm> farms;
        private Farm farm;
        private DataRow farmRow;
        private DataTable farmTable;

        private List<Service> services;
        private Service service;
        private DataRow serviceRow;
        private DataTable serviceTable;

        private string tabletNo = null;

        private List<string> partNumbers;
        private Settings settings;

        private Login login;
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

            screenLoginShow();
        }

        private void formClear()
        {
            AMConnected = false;
            amData.SNum = 0;
            amData.Maxi = 0;
            amData.MaxiSet = 0;
        }

        private void screenLoginShow()
        {
            //Gui.draw<PictureBox>(this, width: 500, height: 130, placev: Gui.Place.Zero);
            Gui.draw(this, typeof(PictureBox), placev: Gui.Place.One);
            //Gui.linkLabelDraw(this, ref linkLabel1, "Forgot password",
            Gui.draw(this, typeof(LinkLabel), text: "Forgot password",
                linkLabelLinkClickedEventHandler: new LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked), 
                placev: Gui.Place.Six);
            //Gui.buttonDraw(this, ref button1, "Login", new EventHandler(buttonLogin_Click), placev: Gui.Place.End);
            Gui.draw(this, typeof(Button), text: "Login", eventHandler: new EventHandler(buttonLogin_Click), placev: Gui.Place.End);
            //Gui.textBoxDraw(this, ref richTextBox1, "Username", placev: Gui.Place.One);
            Gui.draw(this, typeof(RichTextBox), text: "Username", width: Gui.DefaultWidthLarge, placev: Gui.Place.Two);
            //Gui.textBoxDraw(this, ref richTextBox2, "Password", placev: Gui.Place.Three);
            Gui.draw(this, typeof(RichTextBox), text: "Password", width: Gui.DefaultWidthLarge, placev: Gui.Place.Four);
        }

        private async void buttonLogin_Click(object sender, EventArgs e)
        {
            allControlsDisable();
            login = new Login()
            { 
                email = "yael@gmail.com",//richTextBox1.Text,
                password = "yael123",//richTextBox2.Text,
                tablet = TabletNo
            };
            
            LoginResponse loginResponse= await web.loginPost(login);
            if ((loginResponse != null) && (loginResponse.token != null))
            {
                // if ok
                //token = loginResponse.token;
                user = loginResponse.user;
                //screenLoginHide();
                Gui.hide(this);
                screenActionShow();
                await web.farmOptions();
                farms = await web.farmsGet();
                farms = farms.Where(f => f.is_active).ToList();
                services = await web.servicesGet();
                partNumbers = await web.treatmentPackagesGet();
                settings = await web.settingsGet();
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
                allControlsEnable();
            }
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
            //Gui.draw<PictureBox>(this, width: 500, height: 130, placev: Gui.Place.Zero);
            Gui.draw(this, typeof(PictureBox), placev: Gui.Place.One);
            //Gui.labelDraw(this, ref label1, "Welcome distributor", placev: Gui.Place.One,
            Gui.draw(this, typeof(Label), text: "Welcome distributor", font: Gui.DefaultFontLarge, placev: Gui.Place.Two);
            //Gui.labelDraw(this, ref label2, "Choose Action: ", placev: Gui.Place.Three);
            Gui.draw(this, typeof(Label), text: "Choose Action: ", placev: Gui.Place.Four);
            //Gui.buttonDraw(this, ref button1, "Update AM", new System.EventHandler(buttonUpdateAM_Click), placev: Gui.Place.Four);
            Gui.draw(this, typeof(Button), text: "Update AM", eventHandler: new EventHandler(buttonUpdateAM_Click), placev: Gui.Place.Five);
            //Gui.buttonDraw(this, ref button2, "Manage Farms", new System.EventHandler(buttonFarm_Click), placev: Gui.Place.Five);
            Gui.draw(this, typeof(Button), text: "Manage Farms", eventHandler: new EventHandler(buttonFarm_Click), placev: Gui.Place.Six);
            //Gui.buttonDraw(this, ref button3, "Manage Service provider", new System.EventHandler(buttonService_Click), placev: Gui.Place.Six);
            Gui.draw(this, typeof(Button), text: "Manage Service provider", eventHandler: new EventHandler(buttonService_Click), placev: Gui.Place.Seven);
            //Gui.linkLabelDraw(this, ref linkLabel2, "Calculate your farm’s profits with APT", 
            Gui.draw(this, typeof(LinkLabel), text: "Calculate your farm’s profits with APT", 
                linkLabelLinkClickedEventHandler: new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel2_LinkClicked), 
                placev: Gui.Place.Nine);
        }

        private void buttonUpdateAM_Click(object sender, EventArgs e)
        {
            allControlsDisable();
            //screenActionHide();
            Gui.hide(this);
            screenConnectShow();
        }

        private async void buttonFarm_Click(object sender, EventArgs e)
        {
            allControlsDisable();
            Gui.hide(this);
            await FarmTableGet();
            //screenActionHide();
            screenFarmShow();
        }

        private async void buttonService_Click(object sender, EventArgs e)
        {
            allControlsDisable();
            //screenActionHide();
            Gui.hide(this);
            await serviceTableGet();
            screenServiceShow();
        }

        private void allControlsDisable()
        {
            foreach (Control control in this.Controls)
                control.Enabled = false;
        }

        private void allControlsEnable()
        {
            foreach (Control control in this.Controls)
                control.Enabled = true;
        }

        private async Task FarmTableGet()
        {
            //farms = await web.farmsGet();
            if (farms != null)
            {
                farmTable = new DataTable();
                farmTable.Columns.AddRange(new List<DataColumn>(){new DataColumn("Farm Name"),
                                    new DataColumn("Country / State"),
                                    new DataColumn("City"),
                                    new DataColumn("Address")}.ToArray());
                //foreach (var f in farms.Select(f => new { f.name, f.country, f.city, f.address }).ToArray())
                foreach (var farm in farms.Select(f => entityToRow(f)))
                    farmTable.Rows.Add(farm.ToArray<string>());
            }
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
                foreach (var s in services.Select(s => entityToRow(s)))
                    serviceTable.Rows.Add(s.ToArray<string>());
            }
        }

        private List<string> entityToRow(Farm farm)
        {
            return new List<string> { farm.Name.val, farm.Country.val + ((farm.State.val == null) ? string.Empty : " / ") + farm.State.val, farm.City.val, farm.Address.val };
        }

        private List<string> entityToRow(Service service)
        {
            return new List<string> { service.Name.val, service.Country.val + ((service.State.val == null) ? string.Empty : " / ") + service.State.val, service.City.val, service.Address.val };
        }

        private void screenConnectShow()
        {
            Gui.draw(this, typeof(PictureBox), placev: Gui.Place.One);
            //Gui.labelDraw(this, ref label1, "Welcome distributor", placev: Gui.Place.One,
            Gui.draw(this, typeof(Label), text: "Welcome distributor", font: Gui.DefaultFontLarge, placev: Gui.Place.Two);
            //Gui.labelDraw(this, ref label2, "Please make sure the AM is connected to your tablet before continue", placev: Gui.Place.Three);
            Gui.draw(this, typeof(Label), text: "Please make sure the AM is connected to your tablet before continue", placev: Gui.Place.Four);
            //Gui.labelDraw(this, ref label3, "", placev: Gui.Place.Five);
            label1 = Gui.draw(this, typeof(Label), text: "", placev: Gui.Place.Six) as Label;
            //Gui.progressBarDraw(this, ref progressBar1, placev: Gui.Place.Five);
            progressBar1 = Gui.draw(this, typeof(ProgressBar), width: Gui.DefaultWidthLarge, placev: Gui.Place.Six) as ProgressBar;
            //Gui.buttonDraw(this, ref button1, "Check AM present", new EventHandler(buttonCheckAM_Click), placeh: Gui.Place.Left, placev: Gui.Place.End);
            button1 = Gui.draw(this, typeof(Button), text: "Check AM present", eventHandler: new EventHandler(buttonCheckAM_Click), placeh: Gui.Place.Left, placev: Gui.Place.End) as Button;
            //Gui.buttonDraw(this, ref button2, "Forward", new EventHandler(buttonConnectForward_Click), placeh: Gui.Place.Right, placev: Gui.Place.End);
            button2 = Gui.draw(this, typeof(Button), text: "Forward", eventHandler: new EventHandler(buttonConnectForward_Click), placeh: Gui.Place.Right, placev: Gui.Place.End) as Button;
            if (AMConnected)
                AMConnectedShow();
            else
            {
                AMDisconnectedShow();
                label1.Visible = false;
                button2.Enabled = false;
            }
        }

        private async void buttonCheckAM_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
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
            label1.Visible = true;
            button1.Enabled = true;
        }

        private void AMDisconnectedShow()
        {
            //Gui.labelDraw(this, label3, new System.Drawing.Point(360, 620), "AM not found – make sure AM is connected using USB cable", Color.Red,
            label1 = Gui.draw(this, typeof(Label), text: "AM not found – make sure AM is connected using USB cable", color: Color.Red,
                placeh: Gui.Place.Center, placev: Gui.Place.Six) as Label;
            button2.Enabled = false;
        }

        private void AMConnectedShow()
        {
            //Gui.labelDraw(this, label3, new System.Drawing.Point(360, 620), "AM found – connected to AM", Color.Green,
            label1 = Gui.draw(this, typeof(Label), text: "AM found – connected to AM", color: Color.Green,
                placeh: Gui.Place.Center, placev: Gui.Place.Six) as Label;
            button2.Enabled = true;
        }

        private void buttonConnectForward_Click(object sender, EventArgs e)
        {
            //screenConnectHide();
            Gui.hide(this);
            screenInfoShow();
        }

        private void screenInfoShow()
        {
            amData.SNum = 0x123;
            Gui.draw(this, typeof(PictureBox), placev: Gui.Place.One);
            //Gui.labelDraw(this, ref label1, "Welcome distributor", placev: Gui.Place.One,
            Gui.draw(this, typeof(Label), text: "Welcome distributor", font: Gui.DefaultFontLarge, placev: Gui.Place.Two);
            //Gui.labelDraw(this, ref label2, "AM information, pulses per treatment : " + 1500, placev: Gui.Place.Three);
            Gui.draw(this, typeof(Label), text: "AM information, pulses per treatment : " + 1500, placev: Gui.Place.Four);
            //Gui.labelDraw(this, ref label3, "AM identified with SN: " + amData.SNum, placev: Gui.Place.Five);
            Gui.draw(this, typeof(Label), text: "AM identified with SN: " + amData.SNum, placev: Gui.Place.Six);
            //Gui.labelDraw(this, ref label4, "Current available treatments: " + amData.Maxi, placev: Gui.Place.Seven);
            Gui.draw(this, typeof(Label), text: "Current available treatments: " + amData.Maxi, placev: Gui.Place.Eight);
            //Gui.buttonDraw(this, ref button1, "Back", new EventHandler(buttonInfoBack_Click), placeh: Gui.Place.Left, placev: Gui.Place.End);
            Gui.draw(this, typeof(Button), text: "Back", eventHandler: new EventHandler(buttonInfoBack_Click), placeh: Gui.Place.Left, placev: Gui.Place.End);
            //Gui.buttonDraw(this, ref button2, "Continue", new EventHandler(buttonInfoContinue_Click), placeh: Gui.Place.Right, placev: Gui.Place.End);
            Gui.draw(this, typeof(Button), text: "Continue", eventHandler: new EventHandler(buttonInfoContinue_Click), placeh: Gui.Place.Right, placev: Gui.Place.End);
        }

        private void buttonInfoBack_Click(object sender, EventArgs e)
        {
            //screenInfoHide();
            Gui.hide(this);
            screenConnectShow();
        }

        private void buttonInfoContinue_Click(object sender, EventArgs e)
        {
            //screenInfoHide();
            Gui.hide(this);
            screenTreatShow();
        }

        private void screenTreatShow()
        {
            Gui.draw(this, typeof(PictureBox), placev: Gui.Place.One);
            //Gui.labelDraw(this, ref label1, "Welcome distributor", placev: Gui.Place.One,
            Gui.draw(this, typeof(Label), text: "Welcome distributor", font: Gui.DefaultFontLarge, placev: Gui.Place.Two);
            //Gui.labelDraw(this, ref label2, "Select Farm / Service provider", placev: Gui.Place.Three);
            Gui.draw(this, typeof(Label), text: "Select Farm / Service provider", placev: Gui.Place.Four);
            //Gui.comboBoxDraw(this, ref comboBox1, new System.EventHandler(comboBox1_SelectedIndexChanged), placev: Gui.Place.Five);
            //comboBox1.Items.AddRange(new string[] { "farm1", "farm2", "farm3", "farm4", "farm5", "farm6" });
            Gui.draw(this, typeof(ComboBox), text: farms.First().name, items: farms.Select(f => f.name).ToList(), placev: Gui.Place.Five);
            Gui.draw(this, typeof(Label), text: "Add treatments to AM – SN" + amData.SNum, placev: Gui.Place.Six);
            //comboBox2.Items.AddRange(new string[] { "50", "100", "150", "200", "250", "300" });
            Gui.draw(this, typeof(ComboBox), text: partNumbers.First(), items: partNumbers, placev: Gui.Place.Seven);
            progressBar1 = Gui.draw(this, typeof(ProgressBar), width: Gui.DefaultWidthLarge, placev: Gui.Place.Eight) as ProgressBar;
            Gui.draw(this, typeof(Button), text: "Cancel", eventHandler: new EventHandler(buttonTreatCansel_Click), placeh: Gui.Place.Left, placev: Gui.Place.End);
            Gui.draw(this, typeof(Button), text: "Approve", eventHandler: new EventHandler(buttonTreatApprove_Click), placeh: Gui.Place.Right, placev: Gui.Place.End);
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
                //screenTreatHide();
                Gui.hide(this);
                screenLoginShow();
                formClear();
            }
            formNotify.Dispose();
        }

        private async void buttonTreatApprove_Click(object sender, EventArgs e)
        {
            allControlsDisable();
            //comboBox1.Enabled = false;
            //button1.Enabled = false;
            //button2.Enabled = false;
            progressBar1.Visible = true;
            amData.serialPortProgressEvent += new EventHandler(progressBar1_Callback);
            progressBar1.Maximum = 160;
            //int.TryParse(comboBox1.Text, out amData.MaxiSet);
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
            //comboBox1.Enabled = true;
            //button1.Enabled = true;
            //button2.Enabled = true;
            allControlsEnable();
            if (errcode != ErrCode.EPARAM)
            {
                formClear();
                //screenTreatHide();
                Gui.hide(this);
                screenLoginShow();
            }
        }

        //private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        amData.MaxiSet = int.Parse((sender as ComboBox).SelectedItem.ToString(), NumberStyles.Integer);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogFile.logWrite(ex.ToString());
        //    }
        //}



        //
        // Farm Service
        //



        private void screenFarmShow()
        {
            screenDataGridShow("Manage Farms", 
                new System.EventHandler(this.buttonFarmEdit_Click), 
                new System.EventHandler(this.buttonFarmAdd_Click), 
                new System.EventHandler(this.buttonBackToAction_Click));

            dataGridView1.DataSource = farmTable;
        }

        private void screenServiceShow()
        {
            screenDataGridShow("Manage Service providers",
                new System.EventHandler(this.buttonServiceEdit_Click),
                new System.EventHandler(this.buttonServiceAdd_Click),
                new System.EventHandler(this.buttonBackToAction_Click));
            dataGridView1.DataSource = serviceTable;
        }

        private void screenDataGridShow(string dataName, System.EventHandler eventHandlerButton1, System.EventHandler eventHandlerButton2, System.EventHandler eventHandler3)
        {
            Gui.draw(this, typeof(PictureBox), placev: Gui.Place.One);
            //labelDraw(ref label1, "Welcome distributor", placev: Place.First,
            //    font: new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point));
            Gui.draw(this, typeof(Label), text: dataName, placev: Gui.Place.Two);
            Gui.draw(this, typeof(Button), text: "Edit", eventHandler: eventHandlerButton1, placeh: Gui.Place.RightOne, placev: Gui.Place.Three);
            Gui.draw(this, typeof(Button), text: "Add New", eventHandler: eventHandlerButton2, placeh: Gui.Place.RightTwo, placev: Gui.Place.Three);
            Gui.draw(this, typeof(RichTextBox), text: "Search", placeh: Gui.Place.LeftOne, placev: Gui.Place.Three);
            Gui.draw(this, typeof(Button), text: "Back", eventHandler: eventHandler3, placeh: Gui.Place.LeftTwo, placev: Gui.Place.Three);
            Gui.dataGridDraw(this, ref dataGridView1, placev: Gui.Place.Four);
        }

        private void buttonBackToAction_Click(object sender, EventArgs e)
        {
            //screenDataGridHide();
            Gui.hide(this);
            screenActionShow();
        }

        //private void buttonServiceBack_Click(object sender, EventArgs e)
        //{
        //    //screenDataGridHide();
        //    Gui.hide(this);
        //    screenActionShow();
        //}

        private void buttonFarmAdd_Click(object sender, EventArgs e)
        {
            //screenDataGridHide();
            Gui.hide(this);
            screenFarmAddShow();
        }

        private void buttonServiceAdd_Click(object sender, EventArgs e)
        {
            //screenDataGridHide();
            Gui.hide(this);
            screenServiceAddShow();
        }

        private void screenFarmAddShow()
        {
            farm = new Farm();
            Gui.draw(this, typeof(Label), text: "Add Farm", font: Gui.DefaultFontLarge, placev: Gui.Place.Two);
            Gui.draw(this, typeof(Button), text: "Submit", eventHandler: new System.EventHandler(buttonFarmAddSubmit_Click),
                placeh: Gui.Place.RightTwo, placev: Gui.Place.Eleven);
            screenUpdateShow(farm, false, buttonFarmCancel_Click);
            //Gui.labelDraw(this, ref label1, "Add Farm", placev: Gui.Place.Two,
            //Gui.comboBoxSmallDraw(this, ref comboBox5, farm.contract_type, Farm.CONTRACT_TYPE, new System.EventHandler(comboBoxFarm5_SelectedIndexChanged), placeh: Gui.Place.RightTwo, placev: Gui.Place.Nine);
            //Gui.draw<ComboBox>(this, text: farm.contract_type, items: Farm.CONTRACT_TYPE, placeh: Gui.Place.RightTwo, placev: Gui.Place.Nine);
            //Gui.draw<Label>(this, text: "Contract:", autoSize: false, placeh: Gui.Place.RightOne, placev: Gui.Place.Nine);
            //locationStart = 0;
        }

        private void screenServiceAddShow()
        {
            service = new Service();
            Gui.draw(this, typeof(Label), text: "Add Service provider", font: Gui.DefaultFontLarge, placev: Gui.Place.Two);
            Gui.draw(this, typeof(Button), text: "Submit", eventHandler: new System.EventHandler(buttonServiceAddSubmit_Click),
                placeh: Gui.Place.RightTwo, placev: Gui.Place.Eleven);
            screenUpdateShow(service, false, buttonServiceCancel_Click);
            //screenServiceUpdateShow(new Service());
            //Gui.labelDraw(this, ref label11, "Add Service provider", placev: Gui.Place.Two,
            //    font: new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point));
            //Gui.comboBoxSmallDraw(this, ref comboBox1, (new Service()).contract_type, Service.CONTRACT_TYPE, placeh: Gui.Place.RightTwo, placev: Gui.Place.Seven);
            ////textBoxSmallDraw(ref richTextBox15, "", "Contract:", placeh: Misc.Place.RightTwo, placev: Misc.Place.Six);
            //Gui.labelDraw(this, ref label10, "Contract:", autoSize: false, placeh: Gui.Place.RightOne, placev: Gui.Place.Seven);
            //Gui.buttonDraw(this, ref button2, "Submit", new System.EventHandler(buttonServiceAddSubmit_Click), placeh: Gui.Place.RightTwo,
            //    placev: Gui.Place.Eleven);
        }

        private void screenUpdateShow<T>(T entity, bool edit, EventHandler eventHandler)
        {
            //CultureInfo[] ci = CultureInfo.GetCultures(CultureTypes.AllCultures & ~CultureTypes.NeutralCultures);
            //RegionInfo region = new RegionInfo(ci[0].Name);
            Gui.draw(this, typeof(PictureBox), placev: Gui.Place.One);
            //Gui.draw(this, typeof(Button), text: "Cancel", eventHandler: new EventHandler(buttonFarmCancel_Click),
            Gui.draw(this, typeof(Button), text: "Cancel", eventHandler: eventHandler,
                placeh: Gui.Place.LeftOne, placev: Gui.Place.Eleven);

            foreach (PropertyInfo prop in typeof(T).GetProperties())
            {
                //Console.WriteLine("{0} = {1}", prop.Name, prop.GetValue(user, null));
                //PropertyInfo prop = props.ElementAt(controls.IndexOf(control));
                Field field = prop.GetValue(entity) as Field;
                if (field != null)
                {
                    //Gui.draw<RichTextBox>(this, field.type, text: field.val, items: field.items, name: field.deflt, placeh: field.placeh, placev: field.placev);
                    //if (field.type == typeof(RichTextBox))
                    //{
                    string defaultText;
                    if (edit)
                    {
                        if (field.text.Contains("Contract"))
                            continue;
                        defaultText = field.val;
                    }
                    else
                        defaultText = Gui.DefaultText;

                    field.control = Gui.draw(this, field.type, text: field.val, name: defaultText, items: field.items, placeh: field.placeh, placev: field.placev);
                    field.lcontrol = Gui.draw(this, typeof(Label), text: field.text, autoSize: false, items: field.items, placeh: field.lplaceh, placev: field.placev);
                }
            }
        }

        //private void screenFarmUpdateShow(Farm farm, bool edit)
        //{
        //    //CultureInfo[] ci = CultureInfo.GetCultures(CultureTypes.AllCultures & ~CultureTypes.NeutralCultures);
        //    //RegionInfo region = new RegionInfo(ci[0].Name);
        //    Gui.draw(this, typeof(PictureBox), placev: Gui.Place.One);
        //    Gui.draw(this, typeof(Button), text: "Cancel", eventHandler: new System.EventHandler(buttonFarmCancel_Click),
        //        placeh: Gui.Place.LeftOne, placev: Gui.Place.Eleven);
        //    //Misc.buttonDraw(this, ref button2, "Submit", new System.EventHandler(buttonFarmSubmit_Click), placeh: Misc.Place.RightTwo,
        //    //    placev: Misc.Place.Ten);
        //
        //    foreach (PropertyInfo prop in typeof(Farm).GetProperties())
        //    {
        //        //Console.WriteLine("{0} = {1}", prop.Name, prop.GetValue(user, null));
        //        //PropertyInfo prop = props.ElementAt(controls.IndexOf(control));
        //        Farm.Field field = prop.GetValue(farm) as Farm.Field;
        //        if (field != null)
        //        {
        //            //Gui.draw<RichTextBox>(this, field.type, text: field.val, items: field.items, name: field.deflt, placeh: field.placeh, placev: field.placev);
        //            //if (field.type == typeof(RichTextBox))
        //            //{
        //            string defaultText;
        //            if (edit)
        //            {
        //                if (field.text.Contains("Contract"))
        //                    continue;
        //                defaultText = field.val;
        //            }
        //            else
        //                defaultText = Gui.DefaultText;
        //
        //            field.control = Gui.draw(this, field.type, text: field.val, name: defaultText, items: field.items, placeh: field.placeh, placev: field.placev);
        //            field.lcontrol = Gui.draw(this, typeof(Label), text: field.text, autoSize: false, items: field.items, placeh: field.lplaceh, placev: field.placev);
        //            //}
        //            //if (field.type == typeof(ComboBox))
        //            //{
        //            //    if (edit)
        //            //        if (field.text.Contains("Contract"))
        //            //            continue;
        //            //    field.control = Gui.draw<ComboBox>(this, text: field.val, items: field.items, placeh: field.placeh, placev: field.placev);
        //            //    field.lcontrol = Gui.draw<Label>(this, text: field.text, autoSize: false, items: field.items, placeh: field.lplaceh, placev: field.placev);
        //            //}
        //        }
        //    }
        //
        //    //Gui.draw<RichTextBox>(this, text: farm.Mobile, name: Farm.MOBILE, placeh: Gui.Place.LeftOne, placev: Gui.Place.Nine);
        //    //Gui.draw<RichTextBox>(this, text: farm.Address, name: Farm.ADDRESS, placeh: Gui.Place.LeftOne, placev: Gui.Place.Four);
        //    //Gui.draw<ComboBox>(this, text: farm.Country, items: Farm.COUNTRY, placeh: Gui.Place.LeftOne, placev: Gui.Place.Five);
        //    //Gui.draw<RichTextBox>(this, text: farm.City, name: Farm.CITY, placeh: Gui.Place.LeftOne, placev: Gui.Place.Seven);
        //    //Gui.draw<RichTextBox>(this, text: farm.State, name: Farm.STATE, placeh: Gui.Place.LeftOne, placev: Gui.Place.Six);
        //    //Gui.draw<RichTextBox>(this, text: farm.Name, name: Farm.NAME, placeh: Gui.Place.LeftOne, placev: Gui.Place.Three);
        //    //Gui.draw<ComboBox>(this, text: farm.FarmType, items: Farm.FARM_TYPE, placeh: Gui.Place.RightTwo, placev: Gui.Place.Three);
        //    //Gui.draw<ComboBox>(this, text: farm.BreedType, items: Farm.BREED_TYPE, placeh: Gui.Place.RightTwo, placev: Gui.Place.Four);
        //    //Gui.draw<ComboBox>(this, text: farm.MilkingSetupType, items: Farm.MILKING_SETUP_TYPE, placeh: Gui.Place.RightTwo, placev: Gui.Place.Seven);
        //    //Gui.draw<ComboBox>(this, text: farm.LocationOfTreatmentType, items: Farm.LOCATION_OF_TREATMENT_TYPE,placeh: Gui.Place.RightTwo, placev: Gui.Place.Eight);
        //    //Gui.draw<RichTextBox>(this, text: farm.NumberOfLactatingCows, name: Farm.NUMBER_OF_LACTATING_COWS, placeh: Gui.Place.RightTwo, placev: Gui.Place.Five);
        //    //Gui.draw<RichTextBox>(this, text: farm.DhiTest, name: Farm.DHI_TEST, placeh: Gui.Place.RightTwo, placev: Gui.Place.Six);
        //    //Gui.draw<RichTextBox>(this, text: farm.Contact, name: Farm.CONTACT, placeh: Gui.Place.LeftOne, placev: Gui.Place.Eight);
        //    //Gui.draw<RichTextBox>(this, text:  "", name: Farm.EMAIL, placeh: Gui.Place.LeftOne, placev: Gui.Place.Ten);
        //
        //    //locationIdx = locationStart;
        //    //Gui.draw<Label>(this, text: "Name of Dairy farm:", autoSize: false, placeh: Gui.Place.LeftTwo, placev: Gui.Place.Three);
        //    //Gui.draw<Label>(this, text: "Address:", autoSize: false, placeh: Gui.Place.LeftTwo, placev: Gui.Place.Four);
        //    //Gui.draw<Label>(this, text: "Country:", autoSize: false, placeh: Gui.Place.LeftTwo, placev: Gui.Place.Five);
        //    //Gui.draw<Label>(this, text: "State:", autoSize: false, placeh: Gui.Place.LeftTwo, placev: Gui.Place.Six);
        //    //Gui.draw<Label>(this, text: "City:", autoSize: false, placeh: Gui.Place.LeftTwo, placev: Gui.Place.Seven);
        //    //Gui.draw<Label>(this, text: "Contact Name:", autoSize: false, placeh: Gui.Place.LeftTwo, placev: Gui.Place.Eight);
        //    //Gui.draw<Label>(this, text: "Mobile:", autoSize: false, placeh: Gui.Place.LeftTwo, placev: Gui.Place.Nine);
        //    //Gui.draw<Label>(this, text: "Email Address:", autoSize: false, placeh: Gui.Place.LeftTwo, placev: Gui.Place.Ten);
        //    //Gui.draw<Label>(this, text: "Farm Type:", autoSize: false, placeh: Gui.Place.RightOne, placev: Gui.Place.Three);
        //    //Gui.draw<Label>(this, text: "Breed:", autoSize: false, placeh: Gui.Place.RightOne, placev: Gui.Place.Four);
        //    //Gui.draw<Label>(this, text: "# of lactating cows:", autoSize: false, placeh: Gui.Place.RightOne, placev: Gui.Place.Five);
        //    //Gui.draw<Label>(this, text: "Monthly DHI test:", autoSize: false, placeh: Gui.Place.RightOne, placev: Gui.Place.Six);
        //    //Gui.draw<Label>(this, text: "Milking Setup:", autoSize: false, placeh: Gui.Place.RightOne, placev: Gui.Place.Seven);
        //    //Gui.draw<Label>(this, text: "Location of Treatment:", autoSize: false, placeh: Gui.Place.RightOne, placev: Gui.Place.Eight);
        //
        //}

        private void buttonFarmEdit_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count > 0)
            {
                farm = farms.ElementAt(dataGridView1.Rows.IndexOf(dataGridView1.SelectedRows[0]));
                farmRow = farmTable.Rows[dataGridView1.Rows.IndexOf(dataGridView1.SelectedRows[0])];
                //screenDataGridHide();
                Gui.hide(this);
                screenFarmEditShow(farm);
            }
        }

        private void buttonServiceEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                service = services.ElementAt(dataGridView1.Rows.IndexOf(dataGridView1.SelectedRows[0]));
                serviceRow = serviceTable.Rows[dataGridView1.Rows.IndexOf(dataGridView1.SelectedRows[0])];
                //screenDataGridHide();
                Gui.hide(this);
                screenServiceEditShow(service);
            }
            //if (dataGridView1.SelectedRows.Count > 0)
            //{
            //    Service service = services[dataGridView1.Rows.IndexOf(dataGridView1.SelectedRows[0])];
            //    //screenDataGridHide();
            //    Gui.hide(this);
            //    screenServiceEditShow(service);
            //}
        }

        private void screenFarmEditShow(Farm farm)
        {
            screenUpdateShow(farm, true, buttonFarmCancel_Click);
            Gui.draw(this, typeof(Label), text: "Edit Farm", font: Gui.DefaultFontLarge, placev: Gui.Place.Two);
            Gui.draw(this, typeof(Button), text: "Submit", eventHandler: new System.EventHandler(buttonFarmEditSubmit_Click), 
                placeh: Gui.Place.RightTwo, placev: Gui.Place.Eleven);
        }

        private void screenServiceEditShow(Service service)
        {
            screenUpdateShow(service, true, buttonServiceCancel_Click);
            Gui.draw(this, typeof(Label), text: "Edit Service provider", font: Gui.DefaultFontLarge, placev: Gui.Place.Two);
            Gui.draw(this, typeof(Button), text: "Submit", eventHandler: new System.EventHandler(buttonFarmEditSubmit_Click),
                placeh: Gui.Place.RightTwo, placev: Gui.Place.Eleven);
            //screenServiceUpdateShow(service);
            //Gui.labelDraw(this, ref label11, "Edit Service provider", placev: Gui.Place.Two,
            //    font: new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point));
            //Gui.buttonDraw(this, ref button2, "Submit", new System.EventHandler(buttonServiceEditSubmit_Click), placeh: Gui.Place.RightTwo,
            //    placev: Gui.Place.Eleven);
        }

        private void buttonFarmCancel_Click(object sender, EventArgs e)
        {
            //farm = new Farm();
            //screenFarmUpdateHide();
            Gui.hide(this);
            //screenFarmUpdateHide(farmIOControls, farmNameControls);
            screenFarmShow();
        }

        private void buttonServiceCancel_Click(object sender, EventArgs e)
        {
            Gui.hide(this);
            //screenServiceUpdateHide();
            //screenUpdateHide(serviceIOControls, serviceNameControls);
            screenServiceShow();
        }

        private async void buttonFarmAddSubmit_Click(object sender, EventArgs e)
        {
            allControlsDisable();
            //farm = new Farm();
            updateParams(farm);
            //JsonDocument farmResponse = await web.farmAdd(farm);
            JsonDocument farmResponse = await web.entityAdd<FarmJson>(farm as FarmJson, "api/p/farms/");
            if(responseParse<FarmJson>(farmResponse) == ErrCode.OK)
            {
                //farms.Remove(farm);
                farms.Add(farm);
                //farmTable.Rows.Add((new List<string>() { farm.Name.val, farm.Country.val + " / " + farm.State.val, farm.City.val, farm.Address.val }).ToArray<string>());
                farmTable.Rows.Add(entityToRow(farm).ToArray<string>());

                //screenFarmUpdateHide();
                Gui.hide(this);
                //screenFarmUpdateHide(farmIOControls, farmNameControls);
                screenFarmShow();
            }
            else
                allControlsEnable();
        }

        private async void buttonServiceAddSubmit_Click(object sender, EventArgs e)
        {
            allControlsDisable();
            updateParams(service);
            JsonDocument serviceResponse = await web.entityAdd<ServiceJson>(service as ServiceJson, "api/p/service_providers/");
            if (responseParse<ServiceJson>(serviceResponse) == ErrCode.OK)
            {
                services.Add(service);
                serviceTable.Rows.Add(entityToRow(service).ToArray<string>());

                Gui.hide(this);
                screenServiceShow();
            }
            else
                allControlsEnable();
            ////serviceParams();
            //updateParams(service);
            //JsonDocument serviceResponse = await web.serviceAdd(service);
            ////serviceParse(serviceResponse);
            //if (responseParse<Service>(serviceResponse) == ErrCode.OK)
            //{
            //    services.Remove(service);
            //    services.Add(service);
            //    serviceTable.Rows.Add((new List<string>() { service.name, service.country, service.city, service.address }).ToArray<string>());
            //
            //    screenServiceUpdateHide();
            //    //screenUpdateHide(serviceIOControls, serviceNameControls);
            //    screenServiceShow();
            //}
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

        //private void updateParams<T>(T entity, System.Windows.Forms.Control.ControlCollection controls)
        private void updateParams<T>(T entity)
        {
            //PropertyInfo [] props = typeof(T).GetProperties();
            //int i = 1;
            //foreach (Control control in controls)
            foreach (PropertyInfo prop in typeof(T).GetProperties())
            {
                //Console.WriteLine("{0} = {1}", prop.Name, prop.GetValue(user, null));
                //PropertyInfo prop = props.ElementAt(controls.IndexOf(control));
                //PropertyInfo prop = props.ElementAt(i);
                //i++;
                //if (control.GetType() == typeof(RichTextBox))
                Field field = prop.GetValue(entity) as Field;
                if (field != null)
                {
                    if (field.control != null)
                    {
                        if (field.control.Name == Gui.DefaultText)
                            field.val = string.Empty;
                        else
                            field.val = field.control.Text;
                        prop.SetValue(entity, field);
                    }
                }
                //prop.SetValue(entity, control.Text);
            }
        }

        private async void buttonFarmEditSubmit_Click(object sender, EventArgs e)
        {
            allControlsDisable();
            updateParams(farm);
            //JsonDocument farmResponse = await web.farmEdit(farm);
            JsonDocument farmResponse = await web.entityEdit<FarmJson>(farm as FarmJson, "api/p/farms/" + farm.Id + "/");
            if (responseParse<Farm>(farmResponse) == ErrCode.OK)
            {
                //farms.Remove(farm);
                //farms.Add(farm);
                //farmTable.Rows.Add((new List<string>() { farm.name, farm.country, farm.city, farm.address }).ToArray<string>());
                farmTable.Rows.Remove(farmRow);
                farmTable.Rows.Add(entityToRow(farm).ToArray<string>());

                //screenFarmUpdateHide();
                Gui.hide(this);
                //screenFarmUpdateHide(farmIOControls, farmNameControls);
                screenFarmShow();
            }
            else
                allControlsEnable();
        }

        private async void buttonServiceEditSubmit_Click(object sender, EventArgs e)
        {
            allControlsDisable();
            updateParams(service);
            JsonDocument serviceResponse = await web.entityEdit<ServiceJson>(service as ServiceJson, "api/p/service_providers/" + service.Id + "/");
            if (responseParse<Service>(serviceResponse) == ErrCode.OK)
            {
                serviceTable.Rows.Remove(serviceRow);
                serviceTable.Rows.Add(entityToRow(service).ToArray<string>());

                Gui.hide(this);
                screenServiceShow();
            }
            else
                allControlsEnable();
            ////serviceParams();
            //updateParams(service);
            //JsonDocument serviceResponse = await web.serviceEdit(service);
            ////serviceParse(serviceResponse);
            //if (responseParse<Service>(serviceResponse) == ErrCode.OK)
            //{
            //    services.Remove(service);
            //    services.Add(service);
            //    serviceTable.Rows.Add((new List<string>() { service.name, service.country, service.city, service.address }).ToArray<string>());
            //
            //    screenServiceUpdateHide();
            //    //screenUpdateHide(serviceIOControls, serviceNameControls);
            //    screenServiceShow();
            //}
        }



        //
        // Service
        //



        //private void screenServiceUpdateShow(Service service)
        //{
        //    Gui.textBoxSmallDraw(this, ref richTextBox1, service.NumberOfDairyFarms, Service.NUMBER_OF_DAIRY_FARMS, placeh: Gui.Place.LeftOne,  placev: Gui.Place.Three);
        //    Gui.textBoxSmallDraw(this, ref richTextBox2, service.NumberOfDairyCows, Service.NUMBER_OF_DAIRY_COWS, placeh: Gui.Place.LeftOne,  placev: Gui.Place.Four);
        //    Gui.textBoxSmallDraw(this, ref richTextBox3, service.Address, Service.ADDRESS, placeh: Gui.Place.LeftOne,  placev: Gui.Place.Five);
        //    Gui.textBoxSmallDraw(this, ref richTextBox4, service.Country, Service.COUNTRY, placeh: Gui.Place.LeftOne,  placev: Gui.Place.Six);
        //    Gui.textBoxSmallDraw(this, ref richTextBox5, service.State, Service.STATE, placeh: Gui.Place.LeftOne,  placev: Gui.Place.Seven);
        //    Gui.textBoxSmallDraw(this, ref richTextBox6, service.City, Service.CITY, placeh: Gui.Place.RightTwo, placev: Gui.Place.Three);
        //    Gui.textBoxSmallDraw(this, ref richTextBox7, service.Name, Service.NAME, placeh: Gui.Place.RightTwo, placev: Gui.Place.Four);
        //    Gui.textBoxSmallDraw(this, ref richTextBox8, service.Mobile, Service.MOBILE, placeh: Gui.Place.RightTwo, placev: Gui.Place.Five);
        //    Gui.textBoxSmallDraw(this, ref richTextBox9, service.Email, Service.EMAIL, placeh: Gui.Place.RightTwo, placev: Gui.Place.Six);
        //
        //    Gui.labelDraw(this, ref label1, "# of dairy farms:", autoSize: false, placeh: Gui.Place.LeftTwo, placev: Gui.Place.Three);
        //    Gui.labelDraw(this, ref label2, "# of dairy cows:", autoSize: false, placeh: Gui.Place.LeftTwo, placev: Gui.Place.Four);
        //    Gui.labelDraw(this, ref label3, "Address:", autoSize: false, placeh: Gui.Place.LeftTwo, placev: Gui.Place.Five);
        //    Gui.labelDraw(this, ref label4, "Country:", autoSize: false, placeh: Gui.Place.LeftTwo, placev: Gui.Place.Six);
        //    Gui.labelDraw(this, ref label5, "State:", autoSize: false, placeh: Gui.Place.LeftTwo, placev: Gui.Place.Seven);
        //    Gui.labelDraw(this, ref label6, "City:", autoSize: false, placeh: Gui.Place.RightOne, placev: Gui.Place.Three);
        //    Gui.labelDraw(this, ref label7, "Name of contact :", autoSize: false, placeh: Gui.Place.RightOne, placev: Gui.Place.Four);
        //    Gui.labelDraw(this, ref label8, "Mobile:", autoSize: false, placeh: Gui.Place.RightOne, placev: Gui.Place.Five);
        //    Gui.labelDraw(this, ref label9, "Email Address:", autoSize: false, placeh: Gui.Place.RightOne, placev: Gui.Place.Six);
        //
        //    Gui.buttonDraw(this, ref button1, "Cancel", new System.EventHandler(buttonServiceCancel_Click), placeh: Gui.Place.LeftOne,
        //        placev: Gui.Place.Eleven);
        //    //Misc.buttonDraw(this, ref button2, "Submit", new System.EventHandler(buttonServiceSubmit_Click), placeh: Misc.Place.RightTwo,
        //    //    placev: Misc.Place.Ten);
        //}

        //private void screenServiceUpdateHide()
        //{
        //    //detailsHide();
        //    richTextBox1.Dispose();
        //    richTextBox2.Dispose();
        //    richTextBox3.Dispose();
        //    richTextBox4.Dispose();
        //    richTextBox5.Dispose();
        //    richTextBox6.Dispose();
        //    richTextBox7.Dispose();
        //    richTextBox8.Dispose();
        //    richTextBox9.Dispose();
        //    //if (richTextBox15 != null)
        //    //    richTextBox15.Dispose();
        //    if (comboBox1!= null)
        //        comboBox1.Dispose();
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
        //    if (label10 != null)
        //        label10.Dispose();
        //    label11.Dispose();
        //
        //    button1.Dispose();
        //    button2.Dispose();
        //}

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
