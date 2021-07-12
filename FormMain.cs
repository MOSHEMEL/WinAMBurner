using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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
        private ComboBox comboBox1;
        private ComboBox comboBox2;
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

        //private List<string> partNumbers;
        private List<TreatmentPackage> treatmentPackages;
        //private TreatmentPackage treatmentPackage;
        
        private SettingsJson settings;

        private LoginJson login;
        private UserJson user;

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
            this.Font = new Font("Segoe UI", Gui.DefaultFont, FontStyle.Regular, GraphicsUnit.Point);
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
            Gui.draw(this, typeof(PictureBox), placev: Gui.Place.One);
            Gui.draw(this, typeof(LinkLabel), text: "Forgot password",
                linkLabelLinkClickedEventHandler: new LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked), 
                placev: Gui.Place.Six);
            Gui.draw(this, typeof(Button), text: "Login", eventHandler: new EventHandler(buttonLogin_Click), placev: Gui.Place.End);
            Gui.draw(this, typeof(RichTextBox), text: "Username", width: Gui.DefaultWidthLarge, placev: Gui.Place.Two);
            Gui.draw(this, typeof(RichTextBox), text: "Password", width: Gui.DefaultWidthLarge, placev: Gui.Place.Four);
        }

        private async void buttonLogin_Click(object sender, EventArgs e)
        {
            allControlsDisable();
            login = new LoginJson()
            { 
                email = "yael@gmail.com",//richTextBox1.Text,
                password = "yael123",//richTextBox2.Text,
                tablet = TabletNo
            };
            
            LoginResponseJson loginResponse= await web.loginPost(login);
            if ((loginResponse != null) && (loginResponse.token != null))
            {
                // if ok
                user = loginResponse.user;
                Gui.hide(this);
                screenActionShow();
                await web.farmOptions();
                farms = await web.entityGet<List<Farm>>("api/p/farms/");
                farms = farms.Where(f => f.is_active).ToList();
                services = await web.entityGet<List<Service>>("api/p/service_providers/");
                treatmentPackages = await web.entityGet<List<TreatmentPackage>>("api/p/treatment_package/");
                settings = await web.entityGet<SettingsJson>("api/p/settings/");
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
            Gui.draw(this, typeof(PictureBox), placev: Gui.Place.One);
            Gui.draw(this, typeof(Label), text: "Welcome distributor", font: Gui.DefaultFontLarge, placev: Gui.Place.Two);
            Gui.draw(this, typeof(Label), text: "Choose Action: ", placev: Gui.Place.Four);
            Gui.draw(this, typeof(Button), text: "Update AM", eventHandler: new EventHandler(buttonUpdateAM_Click), placev: Gui.Place.Five);
            Gui.draw(this, typeof(Button), text: "Manage Farms", eventHandler: new EventHandler(buttonFarm_Click), placev: Gui.Place.Six);
            Gui.draw(this, typeof(Button), text: "Manage Service provider", eventHandler: new EventHandler(buttonService_Click), placev: Gui.Place.Seven);
            Gui.draw(this, typeof(LinkLabel), text: "Calculate your farm’s profits with APT", 
                linkLabelLinkClickedEventHandler: new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel2_LinkClicked), 
                placev: Gui.Place.Nine);
        }

        private void buttonUpdateAM_Click(object sender, EventArgs e)
        {
            allControlsDisable();
            Gui.hide(this);
            screenConnectShow();
        }

        private async void buttonFarm_Click(object sender, EventArgs e)
        {
            allControlsDisable();
            Gui.hide(this);
            await FarmTableGet();
            screenFarmShow();
        }

        private async void buttonService_Click(object sender, EventArgs e)
        {
            allControlsDisable();
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
            //services = await web.servicesGet();
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
            Gui.draw(this, typeof(Label), text: "Welcome distributor", font: Gui.DefaultFontLarge, placev: Gui.Place.Two);
            Gui.draw(this, typeof(Label), text: "Please make sure the AM is connected to your tablet before continue", placev: Gui.Place.Four);
            label1 = Gui.draw(this, typeof(Label), text: "", placev: Gui.Place.Six) as Label;
            progressBar1 = Gui.draw(this, typeof(ProgressBar), width: Gui.DefaultWidthLarge, placev: Gui.Place.Six) as ProgressBar;
            button1 = Gui.draw(this, typeof(Button), text: "Check AM present", eventHandler: new EventHandler(buttonCheckAM_Click), placeh: Gui.Place.Left, placev: Gui.Place.End) as Button;
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
            label1 = Gui.draw(this, typeof(Label), text: "AM not found – make sure AM is connected using USB cable", color: Color.Red,
                placeh: Gui.Place.Center, placev: Gui.Place.Six) as Label;
            button2.Enabled = false;
        }

        private void AMConnectedShow()
        {
            label1 = Gui.draw(this, typeof(Label), text: "AM found – connected to AM", color: Color.Green,
                placeh: Gui.Place.Center, placev: Gui.Place.Six) as Label;
            button2.Enabled = true;
        }

        private void buttonConnectForward_Click(object sender, EventArgs e)
        {
            Gui.hide(this);
            screenInfoShow();
        }

        private void screenInfoShow()
        {
            //amData.SNum = 0x123;
            Gui.draw(this, typeof(PictureBox), placev: Gui.Place.One);
            Gui.draw(this, typeof(Label), text: "Welcome distributor", font: Gui.DefaultFontLarge, placev: Gui.Place.Two);
            Gui.draw(this, typeof(Label), text: "AM information, pulses per treatment : " + 1500, placev: Gui.Place.Four);
            Gui.draw(this, typeof(Label), text: "AM identified with SN: " + amData.SNum, placev: Gui.Place.Six);
            Gui.draw(this, typeof(Label), text: "Current available treatments: " + amData.Maxi, placev: Gui.Place.Eight);
            Gui.draw(this, typeof(Button), text: "Back", eventHandler: new EventHandler(buttonInfoBack_Click), placeh: Gui.Place.Left, placev: Gui.Place.End);
            Gui.draw(this, typeof(Button), text: "Continue", eventHandler: new EventHandler(buttonInfoContinue_Click), placeh: Gui.Place.Right, placev: Gui.Place.End);
        }

        private void buttonInfoBack_Click(object sender, EventArgs e)
        {
            Gui.hide(this);
            screenConnectShow();
        }

        private void buttonInfoContinue_Click(object sender, EventArgs e)
        {
            Gui.hide(this);
            screenTreatShow();
        }

        private void screenTreatShow()
        {
            Gui.draw(this, typeof(PictureBox), placev: Gui.Place.One);
            Gui.draw(this, typeof(Label), text: "Welcome distributor", font: Gui.DefaultFontLarge, placev: Gui.Place.Two);
            Gui.draw(this, typeof(Label), text: "Select Farm / Service provider", placev: Gui.Place.Four);
            comboBox1 = Gui.draw(this, typeof(ComboBox), items: farms.Select(f => f.name).ToList(), placev: Gui.Place.Five) as ComboBox;
            Gui.draw(this, typeof(Label), text: "Add treatments to AM – SN" + amData.SNum, placev: Gui.Place.Six);
            comboBox2 = Gui.draw(this, typeof(ComboBox), items: treatmentPackages.Select(t => t.PartNumber).ToList(), placev: Gui.Place.Seven) as ComboBox;
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

            Farm farm = farms.FirstOrDefault(f => f.Name.val == comboBox1.Text);
            //int.TryParse(comboBox1.Text, out amData.MaxiSet);
            TreatmentPackage treatmentPackage = treatmentPackages.FirstOrDefault(t => t.PartNumber == comboBox2.Text);
            amData.MaxiSet = treatmentPackage.amount_of_treatments * settings.number_of_pulses_per_treatment;

            ErrCode errcode = await amData.AMDataWrite();
            if (errcode >= ErrCode.OK)
            {
                errcode = await amData.AMDataRead();
            }
            
            progressBar1.Value = progressBar1.Minimum;
            progressBar1.Visible = false;
            
            if (errcode == ErrCode.OK)
            {
                //treatmentPackage.amount_of_treatments = amData.Maxi / settings.number_of_pulses_per_treatment;
                ActionJson action = new ActionJson()
                {
                    aptx_id = "111",
                    am_id = amData.SNum.ToString(),
                    part_number = treatmentPackage.part_number,
                    tablet = TabletNo,
                    farm = farm.id,
                    service_provider = null
                };
    //"tablet": 2,
    //"service_provider": null,
    //"farm": 2,
    //"aptx_id": "some id",
    //"am_id": "some new id",
    //"part_number": 40
//public int id { get; set; }
//public string date { get; set; }
//public string aptx_id { get; set; }
//public string am_id { get; set; }
//public int part_number { get; set; }
//public int contact { get; set; }
//public string tablet { get; set; }
//public int farm { get; set; }
//public int service_provider { get; set; }
//public int distributor { get; set; }

                JsonDocument jsonDocument = await web.entityAdd<ActionJson>(action, "api/p/actions/");
                
                FormNotify formNotify = new FormNotify(new List<string>() {
                    string.Format("{0} treatments updated,",amData.MaxiSet / settings.number_of_pulses_per_treatment),
                    string.Format("{0} treatments available on AM - SN {1},", amData.Maxi / settings.number_of_pulses_per_treatment, amData.SNum),
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
                Gui.hide(this);
                screenLoginShow();
            }
        }

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
            Gui.draw(this, typeof(Label), text: dataName, placev: Gui.Place.Two);
            Gui.draw(this, typeof(Button), text: "Edit", eventHandler: eventHandlerButton1, placeh: Gui.Place.RightOne, placev: Gui.Place.Three);
            Gui.draw(this, typeof(Button), text: "Add New", eventHandler: eventHandlerButton2, placeh: Gui.Place.RightTwo, placev: Gui.Place.Three);
            Gui.draw(this, typeof(RichTextBox), text: "Search", placeh: Gui.Place.LeftOne, placev: Gui.Place.Three);
            Gui.draw(this, typeof(Button), text: "Back", eventHandler: eventHandler3, placeh: Gui.Place.LeftTwo, placev: Gui.Place.Three);
            Gui.dataGridDraw(this, ref dataGridView1, placev: Gui.Place.Four);
        }

        private void buttonBackToAction_Click(object sender, EventArgs e)
        {
            Gui.hide(this);
            screenActionShow();
        }

        private void buttonFarmAdd_Click(object sender, EventArgs e)
        {
            Gui.hide(this);
            screenFarmAddShow();
        }

        private void buttonServiceAdd_Click(object sender, EventArgs e)
        {
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
        }

        private void screenServiceAddShow()
        {
            service = new Service();
            Gui.draw(this, typeof(Label), text: "Add Service provider", font: Gui.DefaultFontLarge, placev: Gui.Place.Two);
            Gui.draw(this, typeof(Button), text: "Submit", eventHandler: new System.EventHandler(buttonServiceAddSubmit_Click),
                placeh: Gui.Place.RightTwo, placev: Gui.Place.Eleven);
            screenUpdateShow(service, false, buttonServiceCancel_Click);
        }

        private void screenUpdateShow<T>(T entity, bool edit, EventHandler eventHandler)
        {
            Gui.draw(this, typeof(PictureBox), placev: Gui.Place.One);
            Gui.draw(this, typeof(Button), text: "Cancel", eventHandler: eventHandler,
                placeh: Gui.Place.LeftOne, placev: Gui.Place.Eleven);

            drawFields(entity, edit);
        }

        private void drawFields<T>(T entity, bool edit)
        {
            foreach (PropertyInfo prop in typeof(T).GetProperties())
            {
                //Console.WriteLine("{0} = {1}", prop.Name, prop.GetValue(user, null));
                //PropertyInfo prop = props.ElementAt(controls.IndexOf(control));
                Field field = prop.GetValue(entity) as Field;
                if (field != null)
                {
                    ////Gui.draw<RichTextBox>(this, field.type, text: field.val, items: field.items, name: field.deflt, placeh: field.placeh, placev: field.placev);
                    ////if (field.type == typeof(RichTextBox))
                    ////{
                    //string defaultText;
                    //if (edit)
                    //{
                    //    if (field.text.Contains("Contract"))
                    //        continue;
                    //    defaultText = field.val;
                    //}
                    //else
                    //    defaultText = Gui.DefaultText;
                    //
                    //field.control = Gui.draw(this, field.type, text: field.val, name: defaultText, items: field.items, placeh: field.placeh, placev: field.placev);
                    //field.lcontrol = Gui.draw(this, typeof(Label), text: field.text, autoSize: false, items: field.items, placeh: field.lplaceh, placev: field.placev);

                    if (edit && field.text.Contains("Contract"))
                        continue;
                    drawField(field, edit);
                }
            }
        }

        private void drawField(Field field, bool edit)
        {
            //Gui.draw<RichTextBox>(this, field.type, text: field.val, items: field.items, name: field.deflt, placeh: field.placeh, placev: field.placev);
            //if (field.type == typeof(RichTextBox))
            //{
            string defaultText;
            if (edit)
                defaultText = field.val;
            else
                defaultText = Gui.DefaultText;

            field.control = Gui.draw(this, field.type, text: field.val, name: defaultText, items: field.items, placeh: field.placeh, placev: field.placev);
            field.lcontrol = Gui.draw(this, typeof(Label), text: field.text, autoSize: false, items: field.items, placeh: field.lplaceh, placev: field.placev);
        }

        private void buttonFarmEdit_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count > 0)
            {
                farm = farms.ElementAt(dataGridView1.Rows.IndexOf(dataGridView1.SelectedRows[0]));
                farmRow = farmTable.Rows[dataGridView1.Rows.IndexOf(dataGridView1.SelectedRows[0])];
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
                Gui.hide(this);
                screenServiceEditShow(service);
            }
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
        }

        private void buttonFarmCancel_Click(object sender, EventArgs e)
        {
            Gui.hide(this);
            screenFarmShow();
        }

        private void buttonServiceCancel_Click(object sender, EventArgs e)
        {
            Gui.hide(this);
            screenServiceShow();
        }

        private async void buttonFarmAddSubmit_Click(object sender, EventArgs e)
        {
            allControlsDisable();
            updateParams(farm);
            JsonDocument farmResponse = await web.entityAdd<FarmJson>(farm as FarmJson, "api/p/farms/");
            if(responseParse<FarmJson>(farmResponse) == ErrCode.OK)
            {
                //farms.Remove(farm);
                farms.Add(farm);
                //farmTable.Rows.Add((new List<string>() { farm.Name.val, farm.Country.val + " / " + farm.State.val, farm.City.val, farm.Address.val }).ToArray<string>());
                farmTable.Rows.Add(entityToRow(farm).ToArray<string>());

                Gui.hide(this);
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
        }

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

        private void updateParams<T>(T entity)
        {
            //PropertyInfo [] props = typeof(T).GetProperties();
            foreach (PropertyInfo prop in typeof(T).GetProperties())
            {
                //Console.WriteLine("{0} = {1}", prop.Name, prop.GetValue(user, null));
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
            JsonDocument jsonDocument = await web.entityEdit<FarmJson>(farm as FarmJson, "api/p/farms/" + farm.Id + "/");
            if (responseParse<Farm>(jsonDocument) == ErrCode.OK)
            {
                //farms.Remove(farm);
                //farms.Add(farm);
                //farmTable.Rows.Add((new List<string>() { farm.name, farm.country, farm.city, farm.address }).ToArray<string>());
                farmTable.Rows.Remove(farmRow);
                farmTable.Rows.Add(entityToRow(farm).ToArray<string>());

                Gui.hide(this);
                screenFarmShow();
            }
            else
                allControlsEnable();
        }

        private async void buttonServiceEditSubmit_Click(object sender, EventArgs e)
        {
            allControlsDisable();
            updateParams(service);
            JsonDocument jsonDocument = await web.entityEdit<ServiceJson>(service as ServiceJson, "api/p/service_providers/" + service.Id + "/");
            if (responseParse<Service>(jsonDocument) == ErrCode.OK)
            {
                serviceTable.Rows.Remove(serviceRow);
                serviceTable.Rows.Add(entityToRow(service).ToArray<string>());

                Gui.hide(this);
                screenServiceShow();
            }
            else
                allControlsEnable();
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
