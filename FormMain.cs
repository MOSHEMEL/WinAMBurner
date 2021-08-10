using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Text.Json;
using System.Windows.Forms;

namespace WinAMBurner
{
    public partial class FormMain : Form
    {
        //private RichTextBox richTextBox1;
        //private RichTextBox richTextBox2;
        private Label label1;
        private Button button1;
        //private Button button2;
        //private ComboBox comboBox1;
        //private ComboBox comboBox2;
        private ProgressBar progressBar1;
        //private System.Windows.Forms.BindingSource bindingSource1;
        private DataGridView dataGridView1;
        //private CheckBox checkBox1;
        //private CheckBox checkBox2;
        //private RadioButton radioButton1;
        //private RadioButton radioButton2;
        private bool AMConnected = false;
        private AM am = new AM();

        private Web web = new Web();

        private List<Farm> farms = null;
        private Farm farm = null;
        //private DataRow farmRow = null;
        //private DataTable farmTable = null;

        private List<Service> services = null;
        private Service service = null;
        //private DataRow serviceRow = null;
        //private DataTable serviceTable = null;

        private string tabletNo = null;

        //private List<string> partNumbers;
        private List<TreatmentPackage> treatmentPackages = null;
        //private TreatmentPackage treatmentPackage;

        private SettingsJson settings = null;

        //private LoginJson login = null;
        private Login login = null;
        private UserJson user = null;

        //private TreatmentPackage treatmentPackage;
        private Action action;

        //private string password = null;
        //private Field fUsername;
        //private Field fPassword;

        private string TabletNo
        {
            get
            {
                if (tabletNo == null)
                    tabletNo = GetBIOSSerNo();
                return tabletNo;
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

        //private int cmdCount;

        public FormMain()
        {
            InitializeComponent();
            this.Size = new Size(2400, 2400);// 1600);
            this.Scale(new SizeF(Field.ScaleFactor, Field.ScaleFactor));
            this.Font = new Font("Segoe UI", Field.DefaultFont, FontStyle.Regular, GraphicsUnit.Point);
            //pictureBoxTitle.Scale(new SizeF(Misc.ScaleFactor, Misc.ScaleFactor));

            (login = new Login(linkLabel1_LinkClicked, buttonLogin_Click) { tablet = TabletNo }).drawFields(this);

            //screenLoginShow();
        }

        private void logout()
        {
            clearAM();

            web = new Web();

            farms = null;
            farm = null;
            //farmRow = null;
            //farmTable = null;

            services = null;
            service = null;
            //serviceRow = null;
            //serviceTable = null;

            tabletNo = null;

            treatmentPackages = null;

            settings = null;

            //login = null;
            login = new Login(linkLabel1_LinkClicked, buttonLogin_Click) { tablet = TabletNo };
            user = null;

            //amData.SNum = 0;
            //amData.Maxi = 0;
            //amData.MaxiSet = 0;
        }

        private void clearAM()
        {
            AMConnected = false;
            am = new AM();
        }

        //private void screenLoginShow()
        //{
        //    new Field(ltype: typeof(PictureBox), lplacev: Place.One).draw(this, true);
        //    login.drawFields(this);
        //    //drawFields(login);
        //    new Field(ltype: typeof(LinkLabel), ltext: "Forgot password", linkEventHandler: linkLabel1_LinkClicked, lplacev: Place.Seven).draw(this, true);
        //    new Field(ltype: typeof(Button), ltext: "Login", buttonEventHandler: buttonLogin_Click, lplacev: Place.End).draw(this, true);
        //}

        private async void buttonLogin_Click(object sender, EventArgs e)
        {
            ErrCode errcode = ErrCode.ERROR;

            //allControlsDisable();
            login.disableControls();

            //updateParams(login);
            login.updateParams();

            login.email = "yael@gmail.com";
            login.password = "yael123";

            //if ((login.email != string.Empty) && (login.password != string.Empty))
            if ((errcode = login.checkParams()) == ErrCode.OK)
            {
                LoginResponseJson loginResponse = await web.login(login);
                if ((loginResponse != null) && (loginResponse.token != null))
                {
                    // if ok
                    user = loginResponse.user;
                    JsonDocument jsonDocument = await web.getConstants();
                    if (jsonDocument != null)
                        Const.parseConstants(jsonDocument);
                    farms = await web.entityGet<List<Farm>>("api/p/farms/");
                    if (farms != null)
                        farms = farms.Where(f => f.is_active).ToList();
                    services = await web.entityGet<List<Service>>("api/p/service_providers/");
                    treatmentPackages = await web.entityGet<List<TreatmentPackage>>("api/p/treatment_package/");
                    if (treatmentPackages != null)
                        treatmentPackages = treatmentPackages.Where(t => t.is_active).ToList();
                    settings = await web.entityGet<SettingsJson>("api/p/settings/");

                    if ((user != null) && (farms != null) && (services != null) && (treatmentPackages != null) && (settings != null) &&
                        (Const.DCOUNTRY != null) && (Const.COUNTRY != null) && (Const.DSTATE != null) && (Const.STATE != null) &&
                        (Const.FARM_TYPE != null) && (Const.BREED_TYPE != null) && (Const.MILKING_SETUP_TYPE != null) &&
                        (Const.LOCATION_OF_TREATMENT_TYPE != null) && (Const.CONTRACT_TYPE != null))
                    {
                        //Field.hide(this);
                        login.hide();
                        screenActionShow();
                        errcode = ErrCode.OK;
                    }
                    else
                        errcode = ErrCode.EPARAM;
                }
                else
                    errcode = ErrCode.EPARAM;
            }
            else
                errcode = ErrCode.EPARAM;

            if (errcode != ErrCode.OK)
            {
                screenError(new List<string>() { "Login failed Check your username and password,",
                    "make sure your tablet is connected to the internet"}, "Login Failed");
            }
        }

        private void screenError(List<string> text, string caption)
        {
            notify(text, NotifyButtons.OK, caption);
            allControlsEnable();
        }

        private void notify(List<string> text, NotifyButtons notifyButtons, string caption)
        {
            FormNotify formNotify = new FormNotify(text, notifyButtons, caption);
            formNotify.ShowDialog();
            formNotify.Dispose();
        }

        private void screenActionShow()
        {
            new Field(ltype: typeof(PictureBox), lplacev: Place.One).draw(this, true);
            new Field(ltype: typeof(Label), ltext: "Welcome distributor", font: Field.DefaultFontLarge, lplacev: Place.Two).draw(this, true);
            new Field(ltype: typeof(Label), ltext: "Choose Action: ", lplacev: Place.Four).draw(this, true);
            new Field(ltype: typeof(Button), ltext: "Update AM", buttonEventHandler: buttonUpdateAM_Click, lplacev: Place.Five).draw(this, true);
            new Field(ltype: typeof(Button), ltext: "Manage Farms", buttonEventHandler: buttonFarm_Click, lplacev: Place.Six).draw(this, true);
            new Field(ltype: typeof(Button), ltext: "Manage Service provider", buttonEventHandler: buttonService_Click, lplacev: Place.Seven).draw(this, true);
            new Field(ltype: typeof(LinkLabel), ltext: "Calculate your farm’s profits with APT",
                linkEventHandler: linkLabel2_LinkClicked, lplacev: Place.Nine).draw(this, true);
            new Field(ltype: typeof(Button), ltext: "Logout", buttonEventHandler: buttonLogout_Click, lplacev: Place.End).draw(this, true);
        }

        public void hide()
        {
            while (Controls.Count > 0)
                Controls[0].Dispose();
        }

        private void buttonLogout_Click(object sender, EventArgs e)
        {
            logout();
            //Field.hide(this);
            hide();
            login.drawFields(this);
            //screenLoginShow();
        }

        private void buttonUpdateAM_Click(object sender, EventArgs e)
        {
            allControlsDisable();
            //Field.hide(this);
            hide();
            screenConnectShow();
        }

        private async void buttonFarm_Click(object sender, EventArgs e)
        {
            allControlsDisable();
            //Field.hide(this);
            hide();
            //FarmTableGet();
            //farmTable = entityTableGet(farms.Cast<Entity>().ToList());
            screenFarmShow();
        }

        private async void buttonService_Click(object sender, EventArgs e)
        {
            allControlsDisable();
            //Field.hide(this);
            hide();
            //serviceTableGet();
            //serviceTable = entityTableGet(services.Cast<Entity>().ToList());
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

        //private DataTable entityTableGet<T>(List<T> entities)
        private DataTable entityTableGet(List<Entity> entities)
        {
            //farms = await web.farmsGet();
            //if (entities != null)
            //{
            DataTable entityTable = new DataTable();
            entityTable.Columns.AddRange(new List<DataColumn>(){new DataColumn("Name"),
                                    new DataColumn("Country / State"),
                                    new DataColumn("City"),
                                    new DataColumn("Address"),
                                    new DataColumn("Contract") }.ToArray());
            foreach (var entity in entities.Select(e => entityToRow(e)))
                entityTable.Rows.Add(entity.ToArray<string>());
            //}
            return entityTable;
        }

        private List<string> entityToRow(Entity e)
        {
            List<string> l = null;
            l = new List<string> { e.Name.val as string, e.Country.val.ToString() + ((e.State.val == string.Empty) ? string.Empty : " / ") + e.State.val.ToString(), e.City.val as string, e.Address.val as string, e.ContractType.val as string };

            //if (typeof(T) == typeof(Farm))
            //{
            //    Farm e = entity as Farm;
            //    l = new List<string> { e.Name.val, e.Country.val + ((e.State.val == null) ? string.Empty : " / ") + e.State.val, e.City.val, e.Address.val, e.ContractType.val };
            //}
            //if (typeof(T) == typeof(Service))
            //{
            //    Service e = entity as Service;
            //    l = new List<string> { e.Name.val, e.Country.val + ((e.State.val == null) ? string.Empty : " / ") + e.State.val, e.City.val, e.Address.val, e.ContractType.val };
            //}
            return l;
        }

        //private List<string> entityToRow(Farm farm)
        //{
        //    return new List<string> { farm.Name.val, farm.Country.val + ((farm.State.val == null) ? string.Empty : " / ") + farm.State.val, farm.City.val, farm.Address.val };
        //}

        //private List<string> entityToRow(Service service)
        //{
        //    return new List<string> { service.Name.val, service.Country.val + ((service.State.val == null) ? string.Empty : " / ") + service.State.val, service.City.val, service.Address.val };
        //}

        private void screenConnectShow()
        {
            new Field(ltype: typeof(PictureBox), lplacev: Place.One).draw(this, true);
            new Field(ltype: typeof(Label), ltext: "Welcome distributor", font: Field.DefaultFontLarge, lplacev: Place.Two).draw(this, true);
            new Field(ltype: typeof(Label), ltext: "Please make sure the AM is connected to your tablet before continue", lplacev: Place.Four).draw(this, true);
            label1 = new Field(ltype: typeof(Label), lplacev: Place.Six).draw(this, true) as Label;
            progressBar1 = new Field(ltype: typeof(ProgressBar), width: Field.DefaultWidthLarge, height: Field.DefaultHeightSmall, lplacev: Place.Ten).draw(this, true) as ProgressBar;
            button1 = new Field(ltype: typeof(Button), ltext: "Check AM present", buttonEventHandler: buttonCheckAM_Click, lplacev: Place.End).draw(this, true) as Button;
            if (AMConnected)
                AMConnectedShow();
            else
            {
                AMDisconnectedShow();
                label1.Visible = false;
                //button2.Enabled = false;
            }
            progressBar1.Visible = false;
        }

        private async void buttonCheckAM_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
            button1.Enabled = false;
            //button2.Enabled = false;
            progressBar1.Visible = true;
            am.serialPortProgressEvent += progressBar_Callback;
            progressBar1.Value = progressBar1.Minimum;
            ErrCode errcode = await am.AMDataCheckConnect();
            if (errcode >= ErrCode.OK)
                errcode = await am.AMDataRead();
            progressBar1.Value = progressBar1.Maximum;
            if (errcode == ErrCode.OK)
            {
                //if ok
                AMConnected = true;
                //AMConnectedShow();
                //Field.hide(this);
                hide();
                screenInfoShow();
            }
            else
            {
                // if fail
                //AMConnected = false;
                clearAM();
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
            progressBar1.Visible = false;
        }

        private void AMDisconnectedShow()
        {
            label1 = new Field(ltype: typeof(Label), ltext: "AM not found – make sure AM is connected using USB cable", color: Color.Red,
                lplaceh: Place.Center, lplacev: Place.Six).draw(this, true) as Label;
            //button2.Enabled = false;
        }

        private void AMConnectedShow()
        {
            label1 = new Field(ltype: typeof(Label), text: "AM found – connected to AM", color: Color.Green,
                placeh: Place.Center, placev: Place.Six).draw(this, true) as Label;
            //button2.Enabled = true;
        }

        private void buttonConnectForward_Click(object sender, EventArgs e)
        {
            //Field.hide(this);
            hide();
            screenInfoShow();
        }

        private void screenInfoShow()
        {
            new Field(ltype: typeof(PictureBox), lplacev: Place.One).draw(this, true);
            new Field(ltype: typeof(Label), ltext: "Welcome distributor", font: Field.DefaultFontLarge, lplacev: Place.Two).draw(this, true);
            new Field(ltype: typeof(Label), ltext: "AM identified with SN: " + am.SNum, lplacev: Place.Four).draw(this, true);
            new Field(ltype: typeof(Label), ltext: "Current available treatments: " + am.Maxi / settings.number_of_pulses_per_treatment, lplacev: Place.Six).draw(this, true);
            new Field(ltype: typeof(Button), ltext: "Back", buttonEventHandler: buttonInfoBack_Click, lplaceh: Place.Five, lplacev: Place.End).draw(this, true);
            new Field(ltype: typeof(Button), ltext: "Continue", buttonEventHandler: buttonInfoContinue_Click, lplaceh: Place.Two, lplacev: Place.End).draw(this, true);
        }

        private void buttonInfoBack_Click(object sender, EventArgs e)
        {
            //Field.hide(this);
            hide();
            screenConnectShow();
        }

        private void buttonInfoContinue_Click(object sender, EventArgs e)
        {
            //Field.hide(this);
            hide();
            //screenTreatShow();
            action = new Action(am, TabletNo, farms.ToArray(), services.ToArray(),
                comboBoxFarm_SelectedIndexChanged, radioButton_CheckedChanged,
                buttonTreatCansel_Click, buttonTreatApprove_Click);

            action.drawFields(this);

            RadioButton radioButton = action.RadioFarm.lcontrol as RadioButton;
            if (radioButton != null)
                radioButton.Checked = true;
            ProgressBar progressBar = action.Progress.lcontrol as ProgressBar;
            if (progressBar != null)
                progressBar.Visible = false;
        }

        //private void screenTreatShow()
        //{
        //    new Field(ltype: typeof(PictureBox), lplacev: Place.One).draw(this, true);
        //    new Field(ltype: typeof(Label), ltext: "Welcome distributor", font: Field.DefaultFontLarge, lplacev: Place.Two).draw(this, true);
        //    radioButton1 = new Field(ltype: typeof(RadioButton), ltext: "Farm", radioEventHandler: radioButton_CheckedChanged, lplaceh: Place.Two, lplacev: Place.Five).draw(this, true) as RadioButton;
        //    radioButton2 = new Field(ltype: typeof(RadioButton), ltext: "Service provider", radioEventHandler: radioButton_CheckedChanged, lplaceh: Place.Two, lplacev: Place.Six).draw(this, true) as RadioButton;
        //    action = new Action(am, TabletNo, comboBoxAction_SelectedIndexChanged);
        //    drawFields(action);
        //    progressBar1 = new Field(ltype: typeof(ProgressBar), width: Field.DefaultWidthLarge, height: Field.DefaultHeightSmall, lplacev: Place.Ten).draw(this, true) as ProgressBar;
        //    new Field(ltype: typeof(Button), ltext: "Cancel", buttonEventHandler: buttonTreatCansel_Click, lplaceh: Place.Five, lplacev: Place.End).draw(this, true);
        //    new Field(ltype: typeof(Button), ltext: "Approve", buttonEventHandler: buttonTreatApprove_Click, lplaceh: Place.Two, lplacev: Place.End).draw(this, true);
        //    radioButton1.Checked = true;
        //    progressBar1.Visible = false;
        //}

        private void comboBoxFarm_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBoxId = sender as ComboBox;
            ComboBox comboBoxPN = action.PartNumber.control as ComboBox;
            if ((comboBoxId != null) && (comboBoxId.SelectedItem != null) && (comboBoxPN != null) && (treatmentPackages != null))
            {
                //removeItems(comboBoxPN, action.PartNumber);
                action.PartNumber.removeItems();
                Farm farm = comboBoxId.SelectedItem as Farm;
                Service service = comboBoxId.SelectedItem as Service;
                Entity entity = null;
                if (farm != null)
                    entity = farm;
                if (service != null)
                    entity = service;
                if (entity != null)
                    //comboBoxPN.Items.AddRange(treatmentPackages.Where(t => t.contract_type == entity.contract_type).ToArray());
                    //addItems(comboBoxPN, action.PartNumber, treatmentPackages.Where(t => t.contract_type == entity.contract_type).ToArray());
                    action.PartNumber.addItems(treatmentPackages.Where(t => t.contract_type == entity.contract_type).ToArray());
            }
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (action != null)
            {
                RadioButton radioFarm = action.RadioFarm.lcontrol as RadioButton;
                RadioButton radioService = action.RadioService.lcontrol as RadioButton;
                //ComboBox comboBoxId = action.Farm.control as ComboBox;
                //ComboBox comboBoxPN = action.PartNumber.control as ComboBox;
                //if ((radioButton != null) && (radioFarm != null) && (radioService != null) && (comboBoxId != null) && (comboBoxPN != null))
                if ((radioButton != null) && (radioFarm != null) && (radioService != null))
                {
                    if (radioButton.Checked)
                    {
                        if (radioButton == radioFarm)
                        {
                            radioService.Checked = false;
                            //addItems(comboBoxId, action.Farm, farms.ToArray());
                            //action.Farm.removeItems();
                            //action.Farm.addItems(farms.ToArray());
                            //action.Farm.items = farms.ToArray();
                            action.Farm.control.Visible = true;
                            action.Farm.view = true;

                            action.Service.control.Visible = false;
                            (action.Service.control as ComboBox).SelectedItem = null;
                            action.Service.dflag = true;
                            action.Service.view = false;
                            //action.clearService();
                            //comboBoxId.Items.AddRange(farms.ToArray());
                        }
                        if (radioButton == radioService)
                        {
                            radioFarm.Checked = false;
                            //addItems(comboBoxId, action.Farm, services.ToArray());
                            //action.Farm.removeItems();
                            //action.Farm.addItems(services.ToArray());
                            action.Farm.control.Visible = false;
                            (action.Farm.control as ComboBox).SelectedItem = null;
                            action.Farm.dflag = true;
                            action.Farm.view = false;
                            //action.clearFarm();

                            action.Service.control.Visible = true;
                            action.Service.view = true;
                            //action.Farm.items = services.ToArray();
                        }
                    }
                    else
                        action.PartNumber.removeItems();
                    //{
                    //    //removeItems(comboBoxId, action.Farm);
                    //    action.Farm.removeItems();
                    //}
                    ////removeItems(comboBoxPN, action.PartNumber);
                    //action.PartNumber.removeItems();
                }
            }
        }

        //private void addItems(ComboBox comboBox, Field field, object[] items)
        //{
        //    //field.items = items.Select(s => s.ToString()).ToArray();
        //    field.dflag = true;
        //    field.items = items;
        //    //field.setDefault();
        //    comboBox.Items.AddRange(items);
        //    //comboBox.Text = field.dtext;
        //}

        //private void removeItems(ComboBox comboBox, Field field)
        //{
        //    if (comboBox != null)
        //    {
        //        //field.setDefault();
        //        field.dflag = true;
        //        field.items = null;
        //        while (comboBox.Items.Count > 0)
        //            comboBox.Items.RemoveAt(0);
        //        //comboBox.Text = field.dtext;
        //    }
        //}

        private void progressBar_Callback(object sender, EventArgs e)
        {
            ProgressBar progressBar;
            if (action != null)
                progressBar = action.Progress.lcontrol as ProgressBar;
            else
                progressBar = progressBar1;

            if (progressBar != null)
            {
                SerialPortEventArgs args = e as SerialPortEventArgs;
                if (args != null)
                {
                    if (args.progress == 0)
                        progressBar.Maximum = progressBar.Value + args.maximum * 2;

                    if ((progressBar.Value + args.progress) <= progressBar.Maximum)
                        progressBar.Value += args.progress;
                }
            }
        }

        private void buttonTreatCansel_Click(object sender, EventArgs e)
        {
            FormNotify formNotify = new FormNotify(new List<string>() {
                "Are you sure you want to cancel the operation?"},
                NotifyButtons.YesNo, caption: "Abort");
            formNotify.ShowDialog();
            if (formNotify.DialogResult == DialogResult.Yes)
            {
                action.hide();
                clearAM();
                screenActionShow();
            }
            formNotify.Dispose();
        }

        private async void buttonTreatApprove_Click(object sender, EventArgs e)
        {
            ErrCode errcode = ErrCode.ERROR;

            //allControlsDisable();
            action.disableControls();

            ProgressBar progressBar = action.Progress.lcontrol as ProgressBar;
            
            //ComboBox comboBoxPN = action.PartNumber.control as ComboBox;
            //TreatmentPackage treatmentPackage = null;
            //if (comboBoxPN != null)
            //    treatmentPackage = comboBoxPN.SelectedItem as TreatmentPackage;
            //updateParams(action);
            action.updateParams();
            //if ((errcode = checkParams(action)) == ErrCode.OK)
            if ((errcode = action.checkParams()) == ErrCode.OK)
            {
                Farm farm = farms.Find(f => f.id == action.farm);
                Service service = services.Find(s => s.id == action.service_provider);
                TreatmentPackage treatmentPackage = treatmentPackages.Find(t => t.part_number == action.part_number);
                if (((farm != null) || (service != null)) && (treatmentPackage != null))
                {
                    am.MaxiSet = (uint)(treatmentPackage.amount_of_treatments * settings.number_of_pulses_per_treatment);
                    uint maxi = am.Maxi;
                    if ((am.Maxi + am.MaxiSet) < settings.max_am_pulses)
                    {
                        FormNotify formNotify = new FormNotify(new List<string>() {
                                    string.Format("{0} treatments will beadded",am.MaxiSet / settings.number_of_pulses_per_treatment),
                                    string.Format("to the AM - SN {0}", am.SNum),
                                    (farm != null) ? string.Format("Farm {0}", farm.Name.val as string) :
                                    ((service != null) ? string.Format("Service Provider {0}", service.Name.val as string) : string.Empty),
                                    "Press the button to proceed"}, NotifyButtons.YesNo, caption: "Approve");
                        formNotify.ShowDialog();
                        if (formNotify.DialogResult == DialogResult.Yes)
                        {
                            formNotify.Dispose();

                            //ProgressBar progressBar = action.Progress.lcontrol as ProgressBar;
                            if (progressBar != null)
                                progressBar.Visible = true;
                            am.serialPortProgressEvent += new EventHandler(progressBar_Callback);
                            if (progressBar != null)
                                progressBar.Value = progressBar.Minimum;

                            if ((errcode = await am.AMDataWrite()) == ErrCode.OK)
                            {
                                if ((errcode = await am.AMDataRead()) == ErrCode.OK)
                                {
                                    JsonDocument jsonDocument = await web.entityAdd<ActionJson>(action, "api/p/actions/");
                                    //if ((jsonDocument != null) && ((errcode = responseParse<ActionJson>(jsonDocument)) == ErrCode.OK))
                                    //if ((jsonDocument != null) && ((errcode = responseParse(action, jsonDocument)) == ErrCode.OK))
                                    if (jsonDocument != null)
                                    {
                                        Action raction = null;
                                        try { raction = JsonSerializer.Deserialize<Action>(jsonDocument.RootElement.ToString()); } catch { }
                                        if (raction != null)
                                        {
                                            notify(new List<string>() {
                                                    string.Format("The original amount of treatments: {0}", maxi / settings.number_of_pulses_per_treatment),
                                                    string.Format("Added treatments: {0}",am.MaxiSet / settings.number_of_pulses_per_treatment),
                                                    string.Format("The treatments available on AM - SN {1}: {0}", am.Maxi / settings.number_of_pulses_per_treatment, am.SNum),
                                                    "please disconnect the AM"},
                                                NotifyButtons.OK, "Approve Success");
                                            //Field.hide(this);
                                            action.hide();
                                            clearAM();
                                            screenActionShow();
                                        }
                                        else
                                        {
                                            //
                                            // erase am needed
                                            //
                                            responseParse(action, jsonDocument);
                                            errcode = ErrCode.ERASE;
                                        }
                                    }
                                    else
                                    {
                                        //
                                        // erase am needed
                                        //
                                        errcode = ErrCode.ERASE;
                                    }
                                }
                            }
                        }
                        else
                            errcode = ErrCode.OK;
                    }
                    else
                        errcode = ErrCode.MAX;
                }
                else
                    errcode = ErrCode.EPARAM;
            }
            else
                errcode = ErrCode.EPARAM;

            if (errcode == ErrCode.EPARAM)
                screenTreatError(new List<string>() {
                    "Wrong parameters,",
                    "please choose the Farm / Service provider",
                    "and the number of treatments" });
            else if (errcode == ErrCode.MAX)
                screenTreatError(new List<string>() {
                    "Wrong part number,",
                    "the maximum number of treatments reached,",
                    "please choose a smaller number of treatments" });
            else if (errcode == ErrCode.ERASE)
            {
                notify(new List<string>() { "Restoring AM" }, NotifyButtons.OK, "Approve Fail");
                am.MaxiSet = 0;
                if (progressBar != null)
                    progressBar.Value = progressBar.Minimum;
                if ((errcode = await am.AMDataWrite()) == ErrCode.OK)
                    if ((errcode = await am.AMDataRead()) != ErrCode.OK)
                        errcode = ErrCode.EFAIL;

                screenTreatError(new List<string>() {
                    "Approve failed,",
                    "AM sucsessfully restored to original values" });

            }
            else if (errcode == ErrCode.EFAIL)
                screenTreatError(new List<string>() { "Failed to restore AM" });
            else if (errcode == ErrCode.ERROR)
                screenTreatError(new List<string>() { "The operation failed, the treatments were not added" });
            else if (errcode == ErrCode.OK)
                action.enableControls();
        }

        private void screenTreatError(List<string> text)
        {
            notify(text, NotifyButtons.OK, "Approve Fail");
            ProgressBar progressBar = action.Progress.lcontrol as ProgressBar;
            if (progressBar != null)
                progressBar.Visible = false;
            //allControlsEnable();
            action.enableControls();
        }

        //
        // Farm Service
        //

        private void screenFarmShow()
        {
            //farmTable = entityTableGet(farms.Cast<Entity>().ToList());
            //dataGridView1.DataSource = farmTable;

            screenDataGridShow("Manage Farms",
                new EventHandler(this.buttonFarmEdit_Click),
                new EventHandler(this.buttonFarmAdd_Click),
                new EventHandler(richTextBoxFarmSearch_TextChanged),
                new EventHandler(this.buttonBackToAction_Click));

            dataGridView1.DataSource = entityTableGet(farms.Cast<Entity>().ToList());
        }

        private void screenServiceShow()
        {
            //serviceTable = entityTableGet(services.Cast<Entity>().ToList());
            //dataGridView1.DataSource = serviceTable;

            screenDataGridShow("Manage Service providers",
                new EventHandler(buttonServiceEdit_Click),
                new EventHandler(buttonServiceAdd_Click),
                new EventHandler(richTextBoxServiceSearch_TextChanged),
                new EventHandler(buttonBackToAction_Click));

            dataGridView1.DataSource = entityTableGet(services.Cast<Entity>().ToList());
        }

        private void richTextBoxFarmSearch_TextChanged(object sender, EventArgs e)
        {
            DataTable table = null;
            if ((table = richTextBoxSearch(sender, farms.Cast<Entity>().ToList())) != null)
                //farmTable = table;
                dataGridView1.DataSource = table;
        }

        private void richTextBoxServiceSearch_TextChanged(object sender, EventArgs e)
        {
            DataTable table = null;
            if ((table = richTextBoxSearch(sender, services.Cast<Entity>().ToList())) != null)
                //serviceTable = table;
                dataGridView1.DataSource = table;
        }

        private DataTable richTextBoxSearch(object sender, List<Entity> entities)
        {
            RichTextBox richTextBox = sender as RichTextBox;
            DataTable table = null;
            if ((richTextBox != null) && (richTextBox.Text != null) && (dataGridView1 != null) && (entities != null))
            {
                //if ((richTextBox.Text != null) && (richTextBox.Name != Gui.DefaultText))
                //{
                //table = entityTableGet(entities.Where(e => e.Name.text.ToLower().Contains(richTextBox.Text.ToLower())).ToList());
                table = entityTableGet(entities.Where(e => e.Name.val.ToString().ToLower().Contains(richTextBox.Text.ToLower())).ToList());
                dataGridView1.DataSource = table;
                //}
            }
            return table;
        }

        private void screenDataGridShow(string dataName, EventHandler eventHandlerButton1, EventHandler eventHandlerButton2, EventHandler eventHandlerButton3, EventHandler eventHandlerButton4)
        {
            new Field(ltype: typeof(PictureBox), lplacev: Place.One).draw(this, true);
            new Field(ltype: typeof(Label), ltext: dataName, lplacev: Place.Two).draw(this, true);
            new Field(ltype: typeof(Button), ltext: "Back", buttonEventHandler: eventHandlerButton4, lplaceh: Place.Four, lplacev: Place.Three).draw(this, true);
            new Field(type: typeof(RichTextBox), text: "Search", textEventHandler: eventHandlerButton3, placeh: Place.Six, placev: Place.Three).draw(this, false);
            new Field(ltype: typeof(Button), ltext: "Edit", buttonEventHandler: eventHandlerButton1, lplaceh: Place.One, lplacev: Place.Three).draw(this, true);
            new Field(ltype: typeof(Button), ltext: "Add New", buttonEventHandler: eventHandlerButton2, lplaceh: Place.Three, lplacev: Place.Three).draw(this, true);
            Field.dataGridDraw(this, ref dataGridView1, placev: Place.Four);
        }

        private void buttonBackToAction_Click(object sender, EventArgs e)
        {
            //Field.hide(this);
            hide();
            screenActionShow();
        }

        private void buttonFarmAdd_Click(object sender, EventArgs e)
        {
            //Field.hide(this);
            hide();
            //screenFarmAddShow();
            (farm = new Farm(false, comboBoxCountry_SelectedIndexChanged, buttonFarmCancel_Click, buttonFarmAddSubmit_Click)).drawFields(this);

        }

        private void buttonServiceAdd_Click(object sender, EventArgs e)
        {
            //Field.hide(this);
            hide();
            //screenServiceAddShow();
            (service = new Service(false, comboBoxCountry_SelectedIndexChanged, buttonServiceCancel_Click, buttonServiceAddSubmit_Click)).drawFields(this);
        }

        //private void screenFarmAddShow()
        //{
        //    farm = new Farm();
        //    farm.Country.comboEventHandler = comboBoxCountry_SelectedIndexChanged;
        //    //farm.ContractType.view = true;
        //    screenUpdateShow(farm, buttonFarmCancel_Click);
        //    //Gui.draw(this, typeof(Label), text: "Add Farm", font: Gui.DefaultFontLarge, placev: Gui.Place.Two);
        //    new Field(ltype: typeof(Label), ltext: "Add Farm", font: Field.DefaultFontLarge, lplacev: Place.Two).draw(this, true);
        //    //Gui.draw(this, typeof(Button), text: "Submit", eventHandler: new EventHandler(buttonFarmAddSubmit_Click),
        //    //    placeh: Gui.Place.Three, placev: Gui.Place.Eleven);
        //    new Field(ltype: typeof(Button), ltext: "Submit", buttonEventHandler: buttonFarmAddSubmit_Click,
        //        lplaceh: Place.Three, lplacev: Place.Eleven).draw(this, true);
        //}

        //private void screenServiceAddShow()
        //{
        //    service = new Service();
        //    service.Country.comboEventHandler = comboBoxCountry_SelectedIndexChanged;
        //    service.ContractType.view = true;
        //    screenUpdateShow(service, buttonServiceCancel_Click);
        //    //Gui.draw(this, typeof(Label), text: "Add Service provider", font: Gui.DefaultFontLarge, placev: Gui.Place.Two);
        //    new Field(ltype: typeof(Label), ltext: "Add Service provider", font: Field.DefaultFontLarge, lplacev: Place.Two).draw(this, true);
        //    //Gui.draw(this, typeof(Button), text: "Submit", eventHandler: new EventHandler(buttonServiceAddSubmit_Click),
        //    //    placeh: Gui.Place.Three, placev: Gui.Place.Eleven);
        //    new Field(ltype: typeof(Button), ltext: "Submit", buttonEventHandler: new EventHandler(buttonServiceAddSubmit_Click),
        //        lplaceh: Place.Three, lplacev: Place.Eleven).draw(this, true);
        //}

        //private void screenUpdateShow<T>(T entity, EventHandler eventHandler)
        //{
        //    drawFields(entity);
        //    //Gui.draw(this, typeof(PictureBox), placev: Gui.Place.One);
        //    new Field(ltype: typeof(PictureBox), lplacev: Place.One).draw(this, true);
        //    //Gui.draw(this, typeof(Button), text: "Cancel", eventHandler: eventHandler,
        //    //    placeh: Gui.Place.Six, placev: Gui.Place.Eleven);
        //    new Field(ltype: typeof(Button), ltext: "Cancel", buttonEventHandler: eventHandler,
        //        lplaceh: Place.Six, lplacev: Place.Eleven).draw(this, true);
        //}

        //private void drawFields<T>(T entity)
        //{
        //    foreach (PropertyInfo prop in typeof(T).GetProperties())
        //    {
        //        //Console.WriteLine("{0} = {1}", prop.Name, prop.GetValue(user, null));
        //        //PropertyInfo prop = props.ElementAt(controls.IndexOf(control));
        //        Field field = prop.GetValue(entity) as Field;
        //        if (field != null)
        //        {
        //            //if (edit && field.ltext.Contains("Contract"))
        //            //    continue;
        //            if (field.view)
        //            {
        //                field.draw(this, false);
        //                field.draw(this, true);
        //            }
        //        }
        //    }
        //}

        //private Control drawField(Field field, bool edit)
        //{
        //    //string dflt = null;
        //    //if (edit)
        //    //    dflt = field.text;
        //    //else
        //    //    dflt = Gui.DefaultText;
        //    //field.control = Gui.draw(this, field.type, text: field.val, name: defaultText, items: field.items, eventHandler: field.comboEventHandler, placeh: field.placeh, placev: field.placev);
        //    //field.lcontrol = Gui.draw(this, typeof(Label), text: field.text, autoSize: false, placeh: field.lplaceh, placev: field.placev);
        //    Control control = field.draw(this);
        //    field.draw(this, false);
        //    return control;
        //}

        private void buttonFarmEdit_Click(object sender, EventArgs e)
        {
            //if ((farms != null) && (farmTable != null) && (dataGridView1 != null))
            if ((farms != null) && (dataGridView1 != null))
            {
                farm = getCurrentEntity(farms.Cast<Entity>().ToList()) as Farm;
                //farmRow = getCurrentRow(farmTable);
                //Field.hide(this);
                hide();
                //screenFarmEditShow(farm);
                farm.initFields(true, comboBoxCountry_SelectedIndexChanged, buttonFarmCancel_Click, buttonFarmEditSubmit_Click);
                //farm.ContractType.view = false;
                farm.drawFields(this);
            }
        }

        //private DataRow getCurrentRow(DataTable dataTable)
        //{
        //    return dataTable.Rows[dataGridView1.CurrentCell.RowIndex];
        //}

        private Entity getCurrentEntity(List<Entity> entities)
        {
            //return entities.ElementAt(dataGridView1.CurrentCell.RowIndex);
            return entities.Find(e => e.Name.val.ToString().Equals(dataGridView1.CurrentRow.Cells[0].Value));
        }

        private void buttonServiceEdit_Click(object sender, EventArgs e)
        {
            //if ((services != null) && (serviceTable != null) && (dataGridView1 != null))
            if ((services != null) && (dataGridView1 != null))
            {
                service = getCurrentEntity(services.Cast<Entity>().ToList()) as Service;
                //serviceRow = getCurrentRow(serviceTable);
                //Field.hide(this);
                hide();
                //screenServiceEditShow(service);
                service.initFields(true, comboBoxCountry_SelectedIndexChanged, buttonServiceCancel_Click, buttonServiceEditSubmit_Click);
                //service.ContractType.view = false;
                service.drawFields(this);
            }
        }

        //private void screenFarmEditShow(Farm farm)
        //{
        //    farm.Country.comboEventHandler = comboBoxCountry_SelectedIndexChanged;
        //    farm.ContractType.view = false;
        //    screenUpdateShow(farm, buttonFarmCancel_Click);
        //    //Gui.draw(this, typeof(Label), text: "Edit Farm", font: Gui.DefaultFontLarge, placev: Gui.Place.Two);
        //    new Field(ltype: typeof(Label), ltext: "Edit Farm", font: Field.DefaultFontLarge, lplacev: Place.Two).draw(this, true);
        //    //Gui.draw(this, typeof(Button), text: "Submit", eventHandler: new EventHandler(buttonFarmEditSubmit_Click),
        //    //    placeh: Gui.Place.Three, placev: Gui.Place.Eleven);
        //    new Field(ltype: typeof(Button), ltext: "Submit", buttonEventHandler: new EventHandler(buttonFarmEditSubmit_Click),
        //        lplaceh: Place.Three, lplacev: Place.Eleven).draw(this, true);
        //}

        //private void screenServiceEditShow(Service service)
        //{
        //    service.Country.comboEventHandler = comboBoxCountry_SelectedIndexChanged;
        //    service.ContractType.view = false;
        //    screenUpdateShow(service, buttonServiceCancel_Click);
        //    //Gui.draw(this, typeof(Label), text: "Edit Service provider", font: Gui.DefaultFontLarge, placev: Gui.Place.Two);
        //    new Field(ltype: typeof(Label), ltext: "Edit Service provider", font: Field.DefaultFontLarge, lplacev: Place.Two).draw(this, true);
        //    //Gui.draw(this, typeof(Button), text: "Submit", eventHandler: new EventHandler(buttonServiceEditSubmit_Click),
        //    //    placeh: Gui.Place.Three, placev: Gui.Place.Eleven);
        //    new Field(ltype: typeof(Button), ltext: "Submit", buttonEventHandler: buttonServiceEditSubmit_Click,
        //        lplaceh: Place.Three, lplacev: Place.Eleven).draw(this, true);
        //}

        private void comboBoxCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                Entity entity = null;
                if (farm != null)
                {
                    if (comboBox == farm.Country.control)
                        entity = farm;
                }
                if (service != null)
                {
                    if (comboBox == service.Country.control)
                        entity = service;
                }
                if ((entity != null) && (entity.State.control != null))
                {
                    entity.State.dflag = true;
                    entity.State.control.Text = entity.State.dtext;
                    if (comboBox.Text.Contains("United States of America"))
                        entity.State.control.Enabled = true;
                    else
                        entity.State.control.Enabled = false;
                }
            }
        }

        private void buttonFarmCancel_Click(object sender, EventArgs e)
        {
            //Field.hide(this);
            farm.hide();
            screenFarmShow();
        }

        private void buttonServiceCancel_Click(object sender, EventArgs e)
        {
            //Field.hide(this);
            service.hide();
            screenServiceShow();
        }

        private async void buttonFarmAddSubmit_Click(object sender, EventArgs e)
        {
            ErrCode errCode = ErrCode.ERROR;

            //allControlsDisable();
            farm.disableControls();

            //updateParams(farm);
            farm.updateParams();
            //if ((errCode = checkParams(farm)) == ErrCode.OK)
            if ((errCode = farm.checkParams()) == ErrCode.OK)
            {
                JsonDocument jsonDocument = await web.entityAdd<FarmJson>(farm, "api/p/farms/");
                if (jsonDocument != null)
                {
                    //if ((errCode = responseParse<FarmJson>(jsonDocument)) == ErrCode.OK)
                    Farm rfarm = null;
                    try { rfarm = JsonSerializer.Deserialize<Farm>(jsonDocument.RootElement.ToString()); } catch { }
                    if (rfarm != null)
                    {
                        farms.Add(rfarm);
                        farm.hide();
                        screenFarmShow();
                        errCode = ErrCode.OK;
                        //if ((errCode = responseParse(farm, jsonDocument)) == ErrCode.OK)
                        //{
                        //    farms.Add(farm);
                        //    farm.hide();
                        //    screenFarmShow();
                        //    errCode = ErrCode.OK;
                        //}
                    }
                    else
                    {
                        responseParse(farm, jsonDocument);
                        errCode = ErrCode.EPARAM;
                    }
                }
                else
                    errCode = ErrCode.EPARAM;
            }

            if (errCode != ErrCode.OK)
                screenError(new List<string>() { "Submit failed, can't add empty or negative fields,",
                    "make sure all the fields are filled with valid values"}, "Submit Failed");
        }

        private async void buttonServiceAddSubmit_Click(object sender, EventArgs e)
        {
            ErrCode errCode = ErrCode.ERROR;

            //allControlsDisable();
            service.disableControls();

            //updateParams(service);
            service.updateParams();
            //if ((errCode = checkParams(service)) == ErrCode.OK)
            if ((errCode = service.checkParams()) == ErrCode.OK)
            {
                JsonDocument jsonDocument = await web.entityAdd<ServiceJson>(service, "api/p/service_providers/");
                if (jsonDocument != null)
                {
                    Service rservice = null;
                    try { rservice = JsonSerializer.Deserialize<Service>(jsonDocument.RootElement.ToString()); } catch { }
                    if (rservice != null)
                    {
                        services.Add(rservice);
                        service.hide();
                        screenServiceShow();
                        errCode = ErrCode.OK;
                        //if ((errCode = responseParse<ServiceJson>(jsonDocument)) == ErrCode.OK)
                        //if ((errCode = responseParse(service, jsonDocument)) == ErrCode.OK)
                        //{
                        //    services.Add(service);
                        //    service.hide();
                        //    screenServiceShow();
                        //    errCode = ErrCode.OK;
                        //}
                    }
                    else
                    {
                        responseParse(service, jsonDocument);
                        errCode = ErrCode.EPARAM;
                    }
                }
                else
                    errCode = ErrCode.EPARAM;
            }

            if (errCode != ErrCode.OK)
                screenError(new List<string>() { "Submit failed, can't add empty or negative fields,",
                    "make sure all the fields are filled with valid values"}, "Submit Failed");
        }

        //private ErrCode responseParse<T>(T entity, JsonDocument jsonDocument)
        private void responseParse<T>(T entity, JsonDocument jsonDocument)
        {
            //ErrCode errcode = ErrCode.EPARAM;
            List<string> errors = new List<string>();
            Gui gui = entity as Gui;

            if (gui != null)
                //errcode = gui.responseParse(jsonDocument, errors);
                gui.responseParse(jsonDocument, errors);

            //if (errcode != ErrCode.OK)
            //{
            //    FormNotify formNotify = new FormNotify(new List<string>() { "Error occured while processing", "the entry by the server" }, NotifyButtons.OK);
            //    formNotify.ShowDialog();
            //    formNotify.Dispose();
            //}

            if (errors.Count > 0)
            {
                FormNotify formNotify = new FormNotify(errors, NotifyButtons.OK);
                formNotify.ShowDialog();
                formNotify.Dispose();
                //errcode = ErrCode.EPARAM;
            }

            //return errcode;
        }

        //private ErrCode responseParse<T>(JsonDocument jsonDocument)
        //{
        //    ErrCode errCode = ErrCode.EPARAM;
        //    List<string> errors = new List<string>();
        //
        //    if (jsonDocument != null)
        //    {
        //        foreach (PropertyInfo prop in typeof(T).GetProperties())
        //        {
        //            //Console.WriteLine("{0} = {1}", prop.Name, prop.GetValue(user, null));
        //            JsonElement jsonElement;
        //            if (jsonDocument.RootElement.TryGetProperty(prop.Name, out jsonElement))
        //                if (jsonElement.ValueKind == JsonValueKind.Array)
        //                    errors.AddRange(jsonElement.EnumerateArray().Select(e => e.ToString()));
        //        }
        //        errCode = ErrCode.OK;
        //    }
        //    else
        //    {
        //        FormNotify formNotify = new FormNotify(new List<string>() { "Error occured while processing", "the entry by the server" }, NotifyButtons.OK);
        //        formNotify.ShowDialog();
        //        formNotify.Dispose();
        //        errCode = ErrCode.ERROR;
        //    }
        //
        //    if (errors.Count > 0)
        //    {
        //        FormNotify formNotify = new FormNotify(errors, NotifyButtons.OK);
        //        formNotify.ShowDialog();
        //        formNotify.Dispose();
        //        errCode = ErrCode.ERROR;
        //    }
        //    return errCode;
        //}

        //private void updateParams<T>(T entity)
        //{
        //    //PropertyInfo [] props = typeof(T).GetProperties();
        //    foreach (PropertyInfo prop in typeof(T).GetProperties())
        //    {
        //        //Console.WriteLine("{0} = {1}", prop.Name, prop.GetValue(user, null));
        //        Field field = prop.GetValue(entity) as Field;
        //        if (field != null)
        //        {
        //            field.updateField();
        //            prop.SetValue(entity, field);
        //        }
        //        //prop.SetValue(entity, control.Text);
        //    }
        //}

        //private ErrCode checkParams<T>(T entity)
        //{
        //    ErrCode errCode = ErrCode.OK;
        //    //PropertyInfo [] props = typeof(T).GetProperties();
        //    foreach (PropertyInfo prop in typeof(T).GetProperties())
        //    {
        //        //Console.WriteLine("{0} = {1}", prop.Name, prop.GetValue(user, null));
        //        Field field = prop.GetValue(entity) as Field;
        //        if (field != null)
        //        {
        //            if (field.error == ErrCode.EPARAM)
        //            {
        //                if (field.control != null)
        //                {
        //                    field.control.ForeColor = Color.Red;
        //                    field.control.Text = field.dtext;
        //                    field.dflag = true;
        //                }
        //                errCode = ErrCode.EPARAM;
        //            }
        //        }
        //    }
        //    return errCode;
        //}

        private async void buttonFarmEditSubmit_Click(object sender, EventArgs e)
        {
            ErrCode errCode = ErrCode.ERROR;

            //allControlsDisable();
            farm.disableControls();

            Farm ufarm = farm.clone() as Farm;
            //updateParams(f);
            ufarm.updateParams();
            //if ((errCode = checkParams(f)) == ErrCode.OK)
            if ((errCode = ufarm.checkParams()) == ErrCode.OK)
            {
                JsonDocument jsonDocument = await web.entityEdit<FarmJson>(ufarm, "api/p/farms/" + farm.Id + "/");
                if (jsonDocument != null)
                {
                    Farm rfarm = null;
                    try { rfarm = JsonSerializer.Deserialize<Farm>(jsonDocument.RootElement.ToString()); } catch { }
                    if (rfarm != null)
                    {
                        farms.Insert(farms.IndexOf(farm), rfarm);
                        farms.Remove(farm);
                        farm.hide();
                        screenFarmShow();
                        errCode = ErrCode.OK;
                        //if ((errCode = responseParse<FarmJson>(jsonDocument)) == ErrCode.OK)
                        //if ((errCode = responseParse(farm, jsonDocument)) == ErrCode.OK)
                        //{
                        //    //updateParams(farm);
                        //    farm.updateParams();
                        //    //farmRow.ItemArray = entityToRow(farm).ToArray<string>();
                        //
                        //    //Field.hide(this);
                        //    farm.hide();
                        //    screenFarmShow();
                        //    errCode = ErrCode.OK;
                        //}
                    }
                    else
                    {
                        responseParse(ufarm, jsonDocument);
                        errCode = ErrCode.EPARAM;
                    }
                }
                else
                    errCode = ErrCode.EPARAM;
            }

            if (errCode != ErrCode.OK)
            {
                screenError(new List<string>() { "Submit failed, can't add empty or negative fields,",
                    "make sure all the fields are filled with valid values"}, "Submit Failed");
            }
        }

        private async void buttonServiceEditSubmit_Click(object sender, EventArgs e)
        {
            ErrCode errCode = ErrCode.ERROR;

            //allControlsDisable();
            service.disableControls();

            Service uservice = service.clone() as Service;
            //updateParams(s);
            uservice.updateParams();
            //if ((errCode = checkParams(s)) == ErrCode.OK)
            if ((errCode = uservice.checkParams()) == ErrCode.OK)
            {
                JsonDocument jsonDocument = await web.entityEdit<ServiceJson>(uservice, "api/p/service_providers/" + service.Id + "/");
                if (jsonDocument != null)
                {
                    Service rservice = null;
                    try { rservice = JsonSerializer.Deserialize<Service>(jsonDocument.RootElement.ToString()); } catch { }
                    if (rservice != null)
                    {
                        services.Insert(services.IndexOf(service), rservice);
                        services.Remove(service);
                        service.hide();
                        screenServiceShow();
                        errCode = ErrCode.OK;
                        //if ((errCode = responseParse<ServiceJson>(jsonDocument)) == ErrCode.OK)
                        //if ((errCode = responseParse(service, jsonDocument)) == ErrCode.OK)
                        //{
                        //    //updateParams(service);
                        //    service.updateParams();
                        //    service.hide();
                        //    screenServiceShow();
                        //    errCode = ErrCode.OK;
                        //}
                    }
                    else
                    {
                        responseParse(uservice, jsonDocument);
                        errCode = ErrCode.EPARAM;
                    }
                }
                else
                    errCode = ErrCode.EPARAM;
            }

            if (errCode != ErrCode.OK)
                screenError(new List<string>() { "Submit failed, can't add empty or negative fields,",
                    "make sure all the fields are filled with valid values"}, "Submit Failed");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel linkLabel = sender as LinkLabel;
            if (linkLabel != null)
            {
                linkLabel.LinkVisited = true;
                var uri = "https://google.com";
                var psi = new System.Diagnostics.ProcessStartInfo();
                psi.UseShellExecute = true;
                psi.FileName = uri;
                System.Diagnostics.Process.Start(psi);
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel linkLabel = sender as LinkLabel;
            if (linkLabel != null)
            {
                linkLabel.LinkVisited = true;
                var uri = "https://armentavet.com/apt-calc/";
                var psi = new System.Diagnostics.ProcessStartInfo();
                psi.UseShellExecute = true;
                psi.FileName = uri;
                System.Diagnostics.Process.Start(psi);
            }
        }
    }
}
