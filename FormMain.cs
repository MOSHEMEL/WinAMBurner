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
        private Button button2;
        //private ComboBox comboBox1;
        //private ComboBox comboBox2;
        private ProgressBar progressBar1;
        //private System.Windows.Forms.BindingSource bindingSource1;
        private DataGridView dataGridView1;
        //private CheckBox checkBox1;
        //private CheckBox checkBox2;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private bool AMConnected = false;
        private AM am = new AM();

        private Web web = new Web();

        private List<Farm> farms = null;
        private Farm farm = null;
        private DataRow farmRow = null;
        private DataTable farmTable = null;

        private List<Service> services = null;
        private Service service = null;
        private DataRow serviceRow = null;
        private DataTable serviceTable = null;

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
            this.Scale(new SizeF(Gui.ScaleFactor, Gui.ScaleFactor));
            this.Font = new Font("Segoe UI", Gui.DefaultFont, FontStyle.Regular, GraphicsUnit.Point);
            //pictureBoxTitle.Scale(new SizeF(Misc.ScaleFactor, Misc.ScaleFactor));
            
            login = new Login() { tablet = TabletNo };

            screenLoginShow();
        }

        private void logout()
        {
            clearAM();

            web = new Web();

            farms = null;
            farm = null;
            farmRow = null;
            farmTable = null;

            services = null;
            service = null;
            serviceRow = null;
            serviceTable = null;

            tabletNo = null;

            treatmentPackages = null;

            settings = null;

            //login = null;
            login = new Login() { tablet = TabletNo };
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

        private void screenLoginShow()
        {
            //Gui.draw(this, typeof(PictureBox), placev: Gui.Place.One);
            new Field(ltype: typeof(PictureBox), lplacev: Gui.Place.One).draw(this, true);
            //Gui.draw(this, typeof(LinkLabel), text: "Forgot password",
            //    linkLabelLinkClickedEventHandler: new LinkLabelLinkClickedEventHandler(linkLabel1_LinkClicked),
            //    placev: Gui.Place.Seven);
            new Field(ltype: typeof(LinkLabel), ltext: "Forgot password", linkEventHandler: new LinkLabelLinkClickedEventHandler(linkLabel1_LinkClicked), lplacev: Gui.Place.Seven).draw(this, true);
            //Gui.draw(this, typeof(Button), text: "Login", eventHandler: new EventHandler(buttonLogin_Click), placev: Gui.Place.End);
            new Field(ltype: typeof(Button), ltext: "Login", buttonEventHandler: new EventHandler(buttonLogin_Click), lplacev: Gui.Place.End).draw(this, true);
            //richTextBox1 = Gui.draw(this, typeof(RichTextBox), text: "Username", width: Gui.DefaultWidthLarge, placev: Gui.Place.Three) as RichTextBox;
            //fUsername = new Field(type: typeof(RichTextBox), text: "Username", width: Gui.DefaultWidthLarge, placev: Gui.Place.Three);
            //fUsername.draw(this, Gui.DefaultText);
            //richTextBox2 = Gui.draw(this, typeof(RichTextBox), text: "Password", eventHandler: richTextBoxPassword_TextChanged, width: Gui.DefaultWidthLarge, placev: Gui.Place.Five) as RichTextBox;
            //new Field(type: typeof(RichTextBox), text: "Password", textEventHandler: richTextBoxPassword_TextChanged, width: Gui.DefaultWidthLarge, placev: Gui.Place.Five).draw(this, Gui.DefaultText);
            //fPassword = new Field(type: typeof(TextBox), text: "Password", width: Gui.DefaultWidthLarge, placev: Gui.Place.Five);
            //fPassword.draw(this, Gui.DefaultText);
            drawFields(login, false);
        }

        //private void richTextBoxPassword_SelectionChanged(object sender, EventArgs e)
        //{
        //    RichTextBox richTextBox = sender as RichTextBox;
        //    if ((richTextBox != null) && (password != null))
        //    {
        //        if (richTextBox.SelectedText != string.Empty)
        //        {
        //            password.TakeWhile(c => c != password.ElementAt(richTextBox.SelectionStart)).Skip(1);
        //        }
        //    }
        //}

        //private void richTextBoxPassword_TextChanged(object sender, EventArgs e)
        //{
        //    RichTextBox richTextBox = sender as RichTextBox;
        //    if (richTextBox != null)
        //    {
        //        if ((richTextBox.Text.Length == 0) || (richTextBox.Text.Length == 1))
        //        {
        //            password = string.Empty;
        //        }
        //        if (richTextBox.Text.Length > 0)
        //        {
        //            char c = richTextBox.Text.Last();
        //            if (c == '*')
        //            {
        //                if (password.Length > 0)
        //                    password = password.Remove(password.IndexOf(password.Last()));
        //            }
        //            else
        //                password += c;
        //            richTextBox.Text = new string(richTextBox.Text.Select(c => c = '*').ToArray());
        //            richTextBox.SelectionStart = richTextBox.TextLength;
        //        }
        //    }
        //}

        private async void buttonLogin_Click(object sender, EventArgs e)
        {
            ErrCode errcode = ErrCode.ERROR;

            allControlsDisable();
            //login = new LoginJson()
            //{
            //    email = fUsername.getText(),
            //    //fUsername.control.Text.Trim(),
            //    //"yael@gmail.com",
            //    password = fPassword.getText(),
            //    //fPassword.control.Text.Trim(),
            //    //"yael123",
            //    //password,
            //    tablet = TabletNo
            //};

            updateParams(login);

            login.email = "yael@gmail.com";
            login.password = "yael123";

            if ((login.email != string.Empty) && (login.password != string.Empty))
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
                        Gui.hide(this);
                        screenActionShow();
                        errcode = ErrCode.OK;
                    }
                    else
                        errcode = ErrCode.EPARAM;
                    //{
                    //    // if failed
                    //    FormNotify formNotify = new FormNotify(new List<string>() {
                    //    "Login failed Check your username and password,",
                    //    "make sure your tablet is connected to the internet"},
                    //        NotifyButtons.OK, caption: "Login Failed");
                    //    formNotify.ShowDialog();
                    //    formNotify.Dispose();
                    //    allControlsEnable();
                    //}
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

        //private string GetBIOSSerNo()
        //{
        //    ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");
        //    foreach (ManagementObject wmi in searcher.Get())
        //    {
        //        try
        //        {
        //            return wmi.GetPropertyValue("SerialNumber").ToString();
        //        }
        //        catch { }
        //    }
        //    return "BIOS Serial Number: Unknown";
        //}

        private void screenActionShow()
        {
            //Gui.draw(this, typeof(PictureBox), placev: Gui.Place.One);
            new Field(ltype: typeof(PictureBox), lplacev: Gui.Place.One).draw(this, true);
            //Gui.draw(this, typeof(Label), text: "Welcome distributor", font: Gui.DefaultFontLarge, placev: Gui.Place.Two);
            new Field(ltype: typeof(Label), ltext: "Welcome distributor", font: Gui.DefaultFontLarge, lplacev: Gui.Place.Two).draw(this, true);
            //Gui.draw(this, typeof(Label), text: "Choose Action: ", placev: Gui.Place.Four);
            new Field(ltype: typeof(Label), ltext: "Choose Action: ", lplacev: Gui.Place.Four).draw(this, true);
            //Gui.draw(this, typeof(Button), text: "Update AM", eventHandler: new EventHandler(buttonUpdateAM_Click), placev: Gui.Place.Five);
            new Field(ltype: typeof(Button), ltext: "Update AM", buttonEventHandler: new EventHandler(buttonUpdateAM_Click), lplacev: Gui.Place.Five).draw(this, true);
            //Gui.draw(this, typeof(Button), text: "Manage Farms", eventHandler: new EventHandler(buttonFarm_Click), placev: Gui.Place.Six);
            new Field(ltype: typeof(Button), ltext: "Manage Farms", buttonEventHandler: new EventHandler(buttonFarm_Click), lplacev: Gui.Place.Six).draw(this, true);
            //Gui.draw(this, typeof(Button), text: "Manage Service provider", eventHandler: new EventHandler(buttonService_Click), placev: Gui.Place.Seven);
            new Field(ltype: typeof(Button), ltext: "Manage Service provider", buttonEventHandler: new EventHandler(buttonService_Click), lplacev: Gui.Place.Seven).draw(this, true);
            //Gui.draw(this, typeof(Button), text: "Logout", eventHandler: new EventHandler(buttonLogout_Click), placev: Gui.Place.End);
            new Field(ltype: typeof(Button), ltext: "Logout", buttonEventHandler: new EventHandler(buttonLogout_Click), lplacev: Gui.Place.End).draw(this, true);
            //Gui.draw(this, typeof(LinkLabel), text: "Calculate your farm’s profits with APT",
            //    linkLabelLinkClickedEventHandler: new LinkLabelLinkClickedEventHandler(linkLabel2_LinkClicked),
            //    placev: Gui.Place.Nine);
            new Field(ltype: typeof(LinkLabel), ltext: "Calculate your farm’s profits with APT",
                linkEventHandler: new LinkLabelLinkClickedEventHandler(linkLabel2_LinkClicked), lplacev: Gui.Place.Nine).draw(this, true);
        }

        private void buttonLogout_Click(object sender, EventArgs e)
        {
            logout();
            Gui.hide(this);
            screenLoginShow();
        }

        private void buttonUpdateAM_Click(object sender, EventArgs e)
        {
            allControlsDisable();
            Gui.hide(this);
            screenConnectShow(); 
            screenTreatShow();
        }

        private async void buttonFarm_Click(object sender, EventArgs e)
        {
            allControlsDisable();
            Gui.hide(this);
            //FarmTableGet();
            farmTable = entityTableGet(farms.Cast<Entity>().ToList());
            screenFarmShow();
        }

        private async void buttonService_Click(object sender, EventArgs e)
        {
            allControlsDisable();
            Gui.hide(this);
            //serviceTableGet();
            serviceTable = entityTableGet(services.Cast<Entity>().ToList());
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

        //private void FarmTableGet()
        //{
        //    //farms = await web.farmsGet();
        //    if (farms != null)
        //    {
        //        farmTable = new DataTable();
        //        farmTable.Columns.AddRange(new List<DataColumn>(){new DataColumn("Farm Name"),
        //                            new DataColumn("Country / State"),
        //                            new DataColumn("City"),
        //                            new DataColumn("Address")}.ToArray());
        //        //foreach (var f in farms.Select(f => new { f.name, f.country, f.city, f.address }).ToArray())
        //        foreach (var farm in farms.Select(f => entityToRow(f)))
        //            farmTable.Rows.Add(farm.ToArray<string>());
        //    }
        //}

        //private void serviceTableGet()
        //{
        //    //services = await web.servicesGet();
        //    if (services != null)
        //    {
        //        serviceTable = new DataTable();
        //        serviceTable.Columns.AddRange(new List<DataColumn>(){new DataColumn("Name"),
        //                            new DataColumn("Country / State"),
        //                            new DataColumn("City"),
        //                            new DataColumn("Address")}.ToArray());
        //        foreach (var service in services.Select(s => entityToRow(s)))
        //            serviceTable.Rows.Add(service.ToArray<string>());
        //    }
        //}

        private List<string> entityToRow(Entity e)
        {
            List<string> l = null;
            l = new List<string> { e.Name.text, e.Country.text + ((e.State.text == string.Empty) ? string.Empty : " / ") + e.State.text, e.City.text, e.Address.text, e.ContractType.text };

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
            //Gui.draw(this, typeof(PictureBox), placev: Gui.Place.One);
            new Field(ltype: typeof(PictureBox), lplacev: Gui.Place.One).draw(this, true);
            //Gui.draw(this, typeof(Label), text: "Welcome distributor", font: Gui.DefaultFontLarge, placev: Gui.Place.Two);
            new Field(ltype: typeof(Label), ltext: "Welcome distributor", font: Gui.DefaultFontLarge, lplacev: Gui.Place.Two).draw(this, true);
            //Gui.draw(this, typeof(Label), text: "Please make sure the AM is connected to your tablet before continue", placev: Gui.Place.Four);
            new Field(ltype: typeof(Label), ltext: "Please make sure the AM is connected to your tablet before continue", lplacev: Gui.Place.Four).draw(this, true);
            //label1 = Gui.draw(this, typeof(Label), text: "", placev: Gui.Place.Six) as Label;
            label1 = new Field(ltype: typeof(Label), ltext: "", lplacev: Gui.Place.Six).draw(this, true) as Label;
            //progressBar1 = Gui.draw(this, typeof(ProgressBar), width: Gui.DefaultWidthLarge, height: Gui.DefaultHeightSmall, placev: Gui.Place.Ten) as ProgressBar;
            progressBar1 = new Field(ltype: typeof(ProgressBar), width: Gui.DefaultWidthLarge, height: Gui.DefaultHeightSmall, lplacev: Gui.Place.Ten).draw(this, true) as ProgressBar;
            //button1 = Gui.draw(this, typeof(Button), text: "Check AM present", eventHandler: new EventHandler(buttonCheckAM_Click), placeh: Gui.Place.Five, placev: Gui.Place.End) as Button;
            button1 = new Field(ltype: typeof(Button), ltext: "Check AM present", buttonEventHandler: new EventHandler(buttonCheckAM_Click), lplaceh: Gui.Place.Five, lplacev: Gui.Place.End).draw(this, true) as Button;
            //button2 = Gui.draw(this, typeof(Button), text: "Forward", eventHandler: new EventHandler(buttonConnectForward_Click), placeh: Gui.Place.Two, placev: Gui.Place.End) as Button;
            button2 = new Field(ltype: typeof(Button), ltext: "Forward", buttonEventHandler: new EventHandler(buttonConnectForward_Click), lplaceh: Gui.Place.Two, lplacev: Gui.Place.End).draw(this, true) as Button;
            if (AMConnected)
                AMConnectedShow();
            else
            {
                AMDisconnectedShow();
                label1.Visible = false;
                button2.Enabled = false;
            }
            progressBar1.Visible = false;
        }

        private async void buttonCheckAM_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
            button1.Enabled = false;
            button2.Enabled = false;
            progressBar1.Visible = true;
            am.serialPortProgressEvent += new EventHandler(progressBar_Callback);
            //progressBar1.Maximum = 60;
            progressBar1.Value = progressBar1.Minimum;
            ErrCode errcode = await am.AMDataCheckConnect();
            if (errcode >= ErrCode.OK)
                errcode = await am.AMDataRead();
            progressBar1.Value = progressBar1.Maximum;
            //progressBar1.Value = progressBar1.Minimum;
            if (errcode == ErrCode.OK)
            {
                //if ok
                AMConnected = true;
                //AMConnectedShow();
                Gui.hide(this);
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
            //label1 = Gui.draw(this, typeof(Label), text: "AM not found – make sure AM is connected using USB cable", color: Color.Red,
            //    placeh: Gui.Place.Center, placev: Gui.Place.Six) as Label;
            label1 = new Field(ltype: typeof(Label), ltext: "AM not found – make sure AM is connected using USB cable", color: Color.Red,
                lplaceh: Gui.Place.Center, lplacev: Gui.Place.Six).draw(this, true) as Label;
            button2.Enabled = false;
        }

        private void AMConnectedShow()
        {
            //label1 = Gui.draw(this, typeof(Label), text: "AM found – connected to AM", color: Color.Green,
            //    placeh: Gui.Place.Center, placev: Gui.Place.Six) as Label;
            label1 = new Field(ltype: typeof(Label), text: "AM found – connected to AM", color: Color.Green,
                placeh: Gui.Place.Center, placev: Gui.Place.Six).draw(this, true) as Label;
            button2.Enabled = true;
        }

        private void buttonConnectForward_Click(object sender, EventArgs e)
        {
            Gui.hide(this);
            screenInfoShow();
        }

        private void screenInfoShow()
        {
            //Gui.draw(this, typeof(PictureBox), placev: Gui.Place.One);
            new Field(ltype: typeof(PictureBox), lplacev: Gui.Place.One).draw(this, true);
            //Gui.draw(this, typeof(Label), text: "Welcome distributor", font: Gui.DefaultFontLarge, placev: Gui.Place.Two);
            new Field(ltype: typeof(Label), ltext: "Welcome distributor", font: Gui.DefaultFontLarge, lplacev: Gui.Place.Two).draw(this, true);
            //Gui.draw(this, typeof(Label), text: "AM identified with SN: " + am.SNum, placev: Gui.Place.Four);
            new Field(ltype: typeof(Label), ltext: "AM identified with SN: " + am.SNum, lplacev: Gui.Place.Four).draw(this, true);
            //Gui.draw(this, typeof(Label), text: "Current available treatments: " + am.Maxi / settings.number_of_pulses_per_treatment, placev: Gui.Place.Six);
            new Field(ltype: typeof(Label), ltext: "Current available treatments: " + am.Maxi / settings.number_of_pulses_per_treatment, lplacev: Gui.Place.Six).draw(this, true);
            //Gui.draw(this, typeof(Button), text: "Back", eventHandler: new EventHandler(buttonInfoBack_Click), placeh: Gui.Place.Five, placev: Gui.Place.End);
            new Field(ltype: typeof(Button), ltext: "Back", buttonEventHandler: new EventHandler(buttonInfoBack_Click), lplaceh: Gui.Place.Five, lplacev: Gui.Place.End).draw(this, true);
            //Gui.draw(this, typeof(Button), text: "Continue", eventHandler: new EventHandler(buttonInfoContinue_Click), placeh: Gui.Place.Two, placev: Gui.Place.End);
            new Field(ltype: typeof(Button), ltext: "Continue", buttonEventHandler: new EventHandler(buttonInfoContinue_Click), lplaceh: Gui.Place.Two, lplacev: Gui.Place.End).draw(this, true);
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
            //Gui.draw(this, typeof(PictureBox), placev: Gui.Place.One);
            new Field(ltype: typeof(PictureBox), lplacev: Gui.Place.One).draw(this, true);
            //Gui.draw(this, typeof(Label), text: "Welcome distributor", font: Gui.DefaultFontLarge, placev: Gui.Place.Two);
            new Field(ltype: typeof(Label), ltext: "Welcome distributor", font: Gui.DefaultFontLarge, lplacev: Gui.Place.Two).draw(this, true);
            //radioButton1 = Gui.draw(this, typeof(RadioButton), text: "Farm", eventHandler: radioButton_CheckedChanged, placeh: Gui.Place.Two, placev: Gui.Place.Five) as RadioButton;
            radioButton1 = new Field(ltype: typeof(RadioButton), ltext: "Farm", radioEventHandler: radioButton_CheckedChanged, lplaceh: Gui.Place.Two, lplacev: Gui.Place.Five).draw(this, true) as RadioButton;
            //radioButton2 = Gui.draw(this, typeof(RadioButton), text: "Service provider", eventHandler: radioButton_CheckedChanged, placeh: Gui.Place.Two, placev: Gui.Place.Six) as RadioButton;
            radioButton2 = new Field(ltype: typeof(RadioButton), ltext: "Service provider", radioEventHandler: radioButton_CheckedChanged, lplaceh: Gui.Place.Two, lplacev: Gui.Place.Six).draw(this, true) as RadioButton;
            //Gui.draw(this, typeof(Label), text: "Select Farm / Service provider", placev: Gui.Place.Four);
            //new Field(ltype: typeof(Label), ltext: "Select Farm / Service provider", lplacev: Gui.Place.Four).draw(this, true);
            //comboBox1 = Gui.draw(this, typeof(ComboBox), eventHandler: comboBox_SelectedIndexChanged, placev: Gui.Place.Five) as ComboBox;
            //comboBox1 = new Field(type: typeof(ComboBox), comboEventHandler: comboBox_SelectedIndexChanged, placev: Gui.Place.Five).draw(this) as ComboBox;
            //Gui.draw(this, typeof(Label), text: "Add treatments to AM – SN" + am.SNum, placev: Gui.Place.Seven);
            //new Field(ltype: typeof(Label), ltext: "Add treatments to AM – SN" + am.SNum, lplacev: Gui.Place.Seven).draw(this, true);
            //comboBox2 = Gui.draw(this, typeof(ComboBox), placev: Gui.Place.Eight) as ComboBox;
            //comboBox2 = new Field(type: typeof(ComboBox), placev: Gui.Place.Eight).draw(this) as ComboBox;
            //treatmentPackage = new TreatmentPackage();
            //comboBox2 = drawField(treatmentPackage.PartNumber, false) as ComboBox;
            //treatmentPackage.PartNumber.lcontrol.Text += am.SNum;
            action = new Action(am, TabletNo, comboBox_SelectedIndexChanged);
            drawFields(action,false);
            //progressBar1 = Gui.draw(this, typeof(ProgressBar), width: Gui.DefaultWidthLarge, height: Gui.DefaultHeightSmall, placev: Gui.Place.Ten) as ProgressBar;
            progressBar1 = new Field(ltype: typeof(ProgressBar), width: Gui.DefaultWidthLarge, height: Gui.DefaultHeightSmall, lplacev: Gui.Place.Ten).draw(this, true) as ProgressBar;
            //Gui.draw(this, typeof(Button), text: "Cancel", eventHandler: new EventHandler(buttonTreatCansel_Click), placeh: Gui.Place.Five, placev: Gui.Place.End);
            new Field(ltype: typeof(Button), ltext: "Cancel", buttonEventHandler: new EventHandler(buttonTreatCansel_Click), lplaceh: Gui.Place.Five, lplacev: Gui.Place.End).draw(this, true);
            //Gui.draw(this, typeof(Button), text: "Approve", eventHandler: new EventHandler(buttonTreatApprove_Click), placeh: Gui.Place.Two, placev: Gui.Place.End);
            new Field(ltype: typeof(Button), ltext: "Approve", buttonEventHandler: new EventHandler(buttonTreatApprove_Click), lplaceh: Gui.Place.Two, lplacev: Gui.Place.End).draw(this, true);
            radioButton1.Checked = true;
            progressBar1.Visible = false;
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBoxId = sender as ComboBox;
            ComboBox comboBoxPN = action.PartNumber.control as ComboBox;
            if (comboBoxId != null)
            {
                if(comboBoxId.SelectedItem != null)
                {
                    if ((comboBoxPN != null) && (treatmentPackages != null))
                    {
                        clearComboBox(comboBoxPN, action.PartNumber.dText);
                        Farm farm = comboBoxId.SelectedItem as Farm;
                        Service service = comboBoxId.SelectedItem as Service;
                        Entity entity = null;
                        if (farm != null)
                            entity = farm;
                        if (service != null)
                            entity = service;
                        if (entity != null)
                            comboBoxPN.Items.AddRange(treatmentPackages.Where(t => t.contract_type == entity.contract_type).ToArray());
                    }
                }
            }
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            ComboBox comboBoxId = action.Farm.control as ComboBox;
            ComboBox comboBoxPN = action.PartNumber.control as ComboBox;
            if ((radioButton != null) && (radioButton1 != null) && (radioButton2 != null) && (comboBoxId != null) && (comboBoxPN != null))
            {
                if (radioButton.Checked)
                {
                    if (radioButton == radioButton1)
                    {
                        radioButton2.Checked = false;
                        comboBoxId.Items.AddRange(farms.ToArray());
                    }
                    if (radioButton == radioButton2)
                    {
                        radioButton1.Checked = false;
                        comboBoxId.Items.AddRange(services.ToArray());
                    }
                }
                else
                {
                    clearComboBox(comboBoxId, action.Farm.dText);
                }
                clearComboBox(comboBoxPN, action.PartNumber.dText);
            }
        }

        private void clearComboBox(ComboBox comboBox, string dflt)
        {
            if (comboBox != null)
            {
                comboBox.Text = dflt;
                while (comboBox.Items.Count > 0)
                    comboBox.Items.RemoveAt(0);
            }
        }

        private void progressBar_Callback(object sender, EventArgs e)
        {
            if (progressBar1 != null)
            {
                SerialPortEventArgs args = e as SerialPortEventArgs;
                if (args != null)
                {
                    if (args.progress == 0)
                        progressBar1.Maximum = progressBar1.Value + args.maximum * 2;
                    
                    if ((progressBar1.Value + args.progress) <= progressBar1.Maximum)
                        progressBar1.Value += args.progress;
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
                //logout();
                //Gui.hide(this);
                //screenLoginShow();
                Gui.hide(this);
                clearAM();
                screenActionShow();
            }
            formNotify.Dispose();
        }

        private async void buttonTreatApprove_Click(object sender, EventArgs e)
        {
            ErrCode errcode = ErrCode.ERROR;

            allControlsDisable();

            //Farm farm = null;
            //int? farmId = null;
            //Service service = null;
            //int? serviceId = null;
            //ComboBox comboBoxId = action.Farm.control as ComboBox;
            ComboBox comboBoxPN = action.PartNumber.control as ComboBox;
            //if ((comboBoxId != null) && (comboBoxPN != null))
            TreatmentPackage treatmentPackage = null;
            if (comboBoxPN != null)
                //{
                //    if (radioButton1.Checked)
                //    {
                //        farm = comboBoxId.SelectedItem as Farm;
                //        if (farm != null)
                //            farmId = farm.Id;
                //    }
                //    else if (radioButton2.Checked)
                //    {
                //        service = comboBoxId.SelectedItem as Service;
                //        if (service != null)
                //            serviceId = service.Id;
                //    }
                treatmentPackage = comboBoxPN.SelectedItem as TreatmentPackage;
            //    
            //    if ((farm != null) || (service != null) && (treatmentPackage != null))
            //    {
            updateParams(action);
            if ((errcode = checkParams(action)) == ErrCode.OK)
            {
                am.MaxiSet = (uint)(treatmentPackage.amount_of_treatments * settings.number_of_pulses_per_treatment);
                if ((am.Maxi + am.MaxiSet) < settings.max_am_pulses)
                {
                    progressBar1.Visible = true;
                    am.serialPortProgressEvent += new EventHandler(progressBar_Callback);
                    progressBar1.Value = progressBar1.Minimum;

                    if ((errcode = await am.AMDataWrite()) == ErrCode.OK)
                    {
                        if ((errcode = await am.AMDataRead()) == ErrCode.OK)
                        {
                            //ActionJson act = new ActionJson()
                            //{
                            //    aptx_id = string.Format("{0:x} {1:x} {2:x}", am.AptxId[0], am.AptxId[1], am.AptxId[2]),
                            //    am_id = am.SNum.ToString(),
                            //    part_number = treatmentPackage.part_number,
                            //    tablet = TabletNo,
                            //    farm = farmId,
                            //    service_provider = serviceId
                            //};

                            JsonDocument jsonDocument = await web.entityAdd<ActionJson>(action, "api/p/actions/");
                            if ((jsonDocument != null) && ((errcode = responseParse<ActionJson>(jsonDocument)) == ErrCode.OK))
                            {
                                notify(new List<string>() {
                                    string.Format("{0} treatments updated,",am.MaxiSet / settings.number_of_pulses_per_treatment),
                                    string.Format("{0} treatments available on AM - SN {1},", am.Maxi / settings.number_of_pulses_per_treatment, am.SNum),
                                    "please disconnect the AM"},
                                    NotifyButtons.OK, "Success");
                                Gui.hide(this);
                                clearAM();
                                screenActionShow();
                            }
                            else
                            {
                                //
                                // erase am needed
                                //
                                errcode = ErrCode.ERASE;
                                notify(new List<string>() { "Restoring AM" }, NotifyButtons.OK, "Approve Fail");
                                am.MaxiSet = 0;
                                progressBar1.Value = progressBar1.Minimum;
                                if ((errcode = await am.AMDataWrite()) == ErrCode.OK)
                                    if ((errcode = await am.AMDataRead()) == ErrCode.OK)
                                        errcode = ErrCode.EPARAM;
                            }
                        }
                    }
                }
                else
                    errcode = ErrCode.MAX;
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
                screenTreatError(new List<string>() { "Error, AM did not burned" });
            else if (errcode == ErrCode.ERROR)
                screenTreatError(new List<string>() { "The operation failed, the treatments were not added" });
        }

        //private void notify(List<string> text, NotifyButtons notifyButtons, string caption = "Error")
        //{
        //    FormNotify formNotify = new FormNotify(text, notifyButtons, caption);
        //    formNotify.ShowDialog();
        //    formNotify.Dispose();
        //}

        private void screenTreatError(List<string> text)
        {
            notify(text, NotifyButtons.OK, "Approve Fail");
            progressBar1.Visible = false;
            allControlsEnable();
        }

        //
        // Farm Service
        //

        private void screenFarmShow()
        {
            screenDataGridShow("Manage Farms",
                new EventHandler(this.buttonFarmEdit_Click),
                new EventHandler(this.buttonFarmAdd_Click),
                new EventHandler(richTextBoxFarmSearch_TextChanged),
                new EventHandler(this.buttonBackToAction_Click));

            dataGridView1.DataSource = farmTable;
        }

        private void screenServiceShow()
        {
            screenDataGridShow("Manage Service providers",
                new EventHandler(buttonServiceEdit_Click),
                new EventHandler(buttonServiceAdd_Click),
                new EventHandler(richTextBoxServiceSearch_TextChanged),
                new EventHandler(buttonBackToAction_Click));
            dataGridView1.DataSource = serviceTable;
        }

        private void richTextBoxFarmSearch_TextChanged(object sender, EventArgs e)
        {
            DataTable table = null;
            if ((table = richTextBoxSearch(sender, farms.Cast<Entity>().ToList())) != null)
                farmTable = table;
        }

        private void richTextBoxServiceSearch_TextChanged(object sender, EventArgs e)
        {
            DataTable table = null;
            if ((table = richTextBoxSearch(sender, services.Cast<Entity>().ToList())) != null)
                serviceTable = table;
        }

        private DataTable richTextBoxSearch(object sender, List<Entity> entities)
        {
            RichTextBox richTextBox = sender as RichTextBox;
            DataTable table = null;
            if ((richTextBox != null) && (dataGridView1 != null) && (entities != null))
            {
                if ((richTextBox.Text != null) && (richTextBox.Name != Gui.DefaultText))
                {
                    table = entityTableGet(entities.Where(e => e.Name.text.ToLower().Contains(richTextBox.Text.ToLower())).ToList());
                    dataGridView1.DataSource = table;
                }
            }
            return table;
        }

        private void screenDataGridShow(string dataName, EventHandler eventHandlerButton1, EventHandler eventHandlerButton2, EventHandler eventHandlerButton3, EventHandler eventHandlerButton4)
        {
            //Gui.draw(this, typeof(PictureBox), placev: Gui.Place.One);
            new Field(ltype: typeof(PictureBox), lplacev: Gui.Place.One).draw(this, true);
            //Gui.draw(this, typeof(Label), text: dataName, placev: Gui.Place.Two);
            new Field(ltype: typeof(Label), ltext: dataName, lplacev: Gui.Place.Two).draw(this, true);
            //Gui.draw(this, typeof(Button), text: "Back", eventHandler: eventHandlerButton4, placeh: Gui.Place.Four, placev: Gui.Place.Three);
            new Field(ltype: typeof(Button), ltext: "Back", buttonEventHandler: eventHandlerButton4, lplaceh: Gui.Place.Four, lplacev: Gui.Place.Three).draw(this, true);
            //Gui.draw(this, typeof(RichTextBox), text: "Search", eventHandler: eventHandlerButton3, placeh: Gui.Place.Six, placev: Gui.Place.Three);
            new Field(type: typeof(RichTextBox), text: "Search", textEventHandler: eventHandlerButton3, placeh: Gui.Place.Six, placev: Gui.Place.Three).draw(this);
            //Gui.draw(this, typeof(Button), text: "Edit", eventHandler: eventHandlerButton1, placeh: Gui.Place.One, placev: Gui.Place.Three);
            new Field(ltype: typeof(Button), ltext: "Edit", buttonEventHandler: eventHandlerButton1, lplaceh: Gui.Place.One, lplacev: Gui.Place.Three).draw(this, true);
            //Gui.draw(this, typeof(Button), text: "Add New", eventHandler: eventHandlerButton2, placeh: Gui.Place.Three, placev: Gui.Place.Three);
            new Field(ltype: typeof(Button), ltext: "Add New", buttonEventHandler: eventHandlerButton2, lplaceh: Gui.Place.Three, lplacev: Gui.Place.Three).draw(this, true);
            //Gui.dataGridDraw(this, ref dataGridView1, placev: Gui.Place.Four);
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
            screenUpdateShow(farm, false, buttonFarmCancel_Click);
            //Gui.draw(this, typeof(Label), text: "Add Farm", font: Gui.DefaultFontLarge, placev: Gui.Place.Two);
            new Field(ltype: typeof(Label), ltext: "Add Farm", font: Gui.DefaultFontLarge, lplacev: Gui.Place.Two).draw(this, true);
            //Gui.draw(this, typeof(Button), text: "Submit", eventHandler: new EventHandler(buttonFarmAddSubmit_Click),
            //    placeh: Gui.Place.Three, placev: Gui.Place.Eleven);
            new Field(ltype: typeof(Button), ltext: "Submit", buttonEventHandler: new EventHandler(buttonFarmAddSubmit_Click),
                lplaceh: Gui.Place.Three, lplacev: Gui.Place.Eleven).draw(this, true);
        }

        private void screenServiceAddShow()
        {
            service = new Service();
            screenUpdateShow(service, false, buttonServiceCancel_Click);
            //Gui.draw(this, typeof(Label), text: "Add Service provider", font: Gui.DefaultFontLarge, placev: Gui.Place.Two);
            new Field(ltype: typeof(Label), ltext: "Add Service provider", font: Gui.DefaultFontLarge, lplacev: Gui.Place.Two).draw(this, true);
            //Gui.draw(this, typeof(Button), text: "Submit", eventHandler: new EventHandler(buttonServiceAddSubmit_Click),
            //    placeh: Gui.Place.Three, placev: Gui.Place.Eleven);
            new Field(ltype: typeof(Button), ltext: "Submit", buttonEventHandler: new EventHandler(buttonServiceAddSubmit_Click),
                lplaceh: Gui.Place.Three, lplacev: Gui.Place.Eleven).draw(this, true);
        }

        private void screenUpdateShow<T>(T entity, bool edit, EventHandler eventHandler)
        {
            drawFields(entity, edit);
            //Gui.draw(this, typeof(PictureBox), placev: Gui.Place.One);
            new Field(ltype: typeof(PictureBox), lplacev: Gui.Place.One).draw(this, true);
            //Gui.draw(this, typeof(Button), text: "Cancel", eventHandler: eventHandler,
            //    placeh: Gui.Place.Six, placev: Gui.Place.Eleven);
            new Field(ltype: typeof(Button), ltext: "Cancel", buttonEventHandler: eventHandler,
                lplaceh: Gui.Place.Six, lplacev: Gui.Place.Eleven).draw(this, true);
        }

        private void drawFields<T>(T entity, bool edit)
        {
            Field country = null;
            Field state = null;

            foreach (PropertyInfo prop in typeof(T).GetProperties())
            {
                //Console.WriteLine("{0} = {1}", prop.Name, prop.GetValue(user, null));
                //PropertyInfo prop = props.ElementAt(controls.IndexOf(control));
                Field field = prop.GetValue(entity) as Field;
                if (field != null)
                {
                    if (edit && field.ltext.Contains("Contract"))
                        continue;

                    drawField(field, edit);

                    if (prop.Name == "Country")
                        country = field;
                    if (prop.Name == "State")
                        state = field;
                }
            }
            if (country != null)
                country.depend = state;
        }

        private Control drawField(Field field, bool edit)
        {
            //string dflt = null;
            //if (edit)
            //    dflt = field.text;
            //else
            //    dflt = Gui.DefaultText;
            //field.control = Gui.draw(this, field.type, text: field.val, name: defaultText, items: field.items, eventHandler: field.comboEventHandler, placeh: field.placeh, placev: field.placev);
            //field.lcontrol = Gui.draw(this, typeof(Label), text: field.text, autoSize: false, placeh: field.lplaceh, placev: field.placev);
            Control control = field.draw(this);
            field.draw(this, false);
            return control;
        }

        private void buttonFarmEdit_Click(object sender, EventArgs e)
        {
            if ((farms != null) && (farmTable != null) && (dataGridView1 != null))
            {
                farm = getCurrentEntity(farms.Cast<Entity>().ToList()) as Farm;
                farmRow = getCurrentRow(farmTable);
                Gui.hide(this);
                screenFarmEditShow(farm);
            }
        }

        private DataRow getCurrentRow(DataTable dataTable)
        {
            return dataTable.Rows[dataGridView1.CurrentCell.RowIndex];
        }

        private Entity getCurrentEntity(List<Entity> entities)
        {
            return entities.ElementAt(dataGridView1.CurrentCell.RowIndex);
        }

        private void buttonServiceEdit_Click(object sender, EventArgs e)
        {
            if ((services != null) && (serviceTable != null) && (dataGridView1 != null))
            {
                service = getCurrentEntity(services.Cast<Entity>().ToList()) as Service;
                serviceRow = getCurrentRow(serviceTable);
                Gui.hide(this);
                screenServiceEditShow(service);
            }
        }

        private void screenFarmEditShow(Farm farm)
        {
            screenUpdateShow(farm, true, buttonFarmCancel_Click);
            //Gui.draw(this, typeof(Label), text: "Edit Farm", font: Gui.DefaultFontLarge, placev: Gui.Place.Two);
            new Field(ltype: typeof(Label), ltext: "Edit Farm", font: Gui.DefaultFontLarge, lplacev: Gui.Place.Two).draw(this, true);
            //Gui.draw(this, typeof(Button), text: "Submit", eventHandler: new EventHandler(buttonFarmEditSubmit_Click),
            //    placeh: Gui.Place.Three, placev: Gui.Place.Eleven);
            new Field(ltype: typeof(Button), ltext: "Submit", buttonEventHandler: new EventHandler(buttonFarmEditSubmit_Click),
                lplaceh: Gui.Place.Three, lplacev: Gui.Place.Eleven).draw(this, true);
        }

        private void screenServiceEditShow(Service service)
        {
            screenUpdateShow(service, true, buttonServiceCancel_Click);
            //Gui.draw(this, typeof(Label), text: "Edit Service provider", font: Gui.DefaultFontLarge, placev: Gui.Place.Two);
            new Field(ltype: typeof(Label), ltext: "Edit Service provider", font: Gui.DefaultFontLarge, lplacev: Gui.Place.Two).draw(this, true);
            //Gui.draw(this, typeof(Button), text: "Submit", eventHandler: new EventHandler(buttonServiceEditSubmit_Click),
            //    placeh: Gui.Place.Three, placev: Gui.Place.Eleven);
            new Field(ltype: typeof(Button), ltext: "Submit", buttonEventHandler: new EventHandler(buttonServiceEditSubmit_Click),
                lplaceh: Gui.Place.Three, lplacev: Gui.Place.Eleven).draw(this, true);
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
            ErrCode errCode = ErrCode.ERROR;

            allControlsDisable();
            
            updateParams(farm);
            if ((errCode = checkParams(farm)) == ErrCode.OK)
            {
                JsonDocument jsonDocument = await web.entityAdd<FarmJson>(farm, "api/p/farms/");
                if (jsonDocument != null)
                {
                    if ((errCode = responseParse<ServiceJson>(jsonDocument)) == ErrCode.OK)
                    {
                        farms.Add(farm);
                        farmTable.Rows.Add(entityToRow(farm).ToArray<string>());

                        Gui.hide(this);
                        screenFarmShow();
                        errCode = ErrCode.OK;
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

            allControlsDisable();

            updateParams(service);
            if ((errCode = checkParams(service)) == ErrCode.OK)
            {
                JsonDocument jsonDocument = await web.entityAdd<ServiceJson>(service, "api/p/service_providers/");
                if (jsonDocument != null)
                {
                    if ((errCode = responseParse<ServiceJson>(jsonDocument)) == ErrCode.OK)
                    {
                        services.Add(service);
                        serviceTable.Rows.Add(entityToRow(service).ToArray<string>());

                        Gui.hide(this);
                        screenServiceShow();
                        errCode = ErrCode.OK;
                    }
                }
                else
                    errCode = ErrCode.EPARAM;
            }

            if (errCode != ErrCode.OK)
                screenError(new List<string>() { "Submit failed, can't add empty or negative fields,",
                    "make sure all the fields are filled with valid values"}, "Submit Failed");
        }

        private ErrCode responseParse<T>(JsonDocument jsonDocument)
        {
            ErrCode errCode = ErrCode.EPARAM;
            List<string> errors = new List<string>();

            if (jsonDocument != null)
            {
                foreach (PropertyInfo prop in typeof(T).GetProperties())
                {
                    //Console.WriteLine("{0} = {1}", prop.Name, prop.GetValue(user, null));
                    //string text = jsonDocument.RootElement.GetProperty(prop.Name).ToString();
                    //string text = "";
                    //try { text = jsonDocument.RootElement.GetProperty(prop.Name).EnumerateArray().ElementAt(0).ToString(); }
                    //catch { }
                    //if (text != "")
                    //    texts.Add(text);
                    JsonElement jsonElement;
                    if (jsonDocument.RootElement.TryGetProperty(prop.Name, out jsonElement))
                        if(jsonElement.ValueKind == JsonValueKind.Array )
                            //errors.Add(jsonElement.EnumerateArray().ElementAt(0).ToString());
                            errors.AddRange(jsonElement.EnumerateArray().Select(e => e.ToString()));
                            //errors.Add(jsonElement.ToString());
                }
                errCode = ErrCode.OK;
            }
            else
            {
                FormNotify formNotify = new FormNotify(new List<string>() { "Error occured while processing", "the entry by the server"}, NotifyButtons.OK);
                formNotify.ShowDialog();
                formNotify.Dispose();
                errCode = ErrCode.ERROR;
            }

            if (errors.Count > 0)
            {
                FormNotify formNotify = new FormNotify(errors, NotifyButtons.OK);
                formNotify.ShowDialog();
                formNotify.Dispose();
                errCode = ErrCode.ERROR;
            }
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
                    field.updateField();
                    prop.SetValue(entity, field);
                }
                //prop.SetValue(entity, control.Text);
            }
        }

        private ErrCode checkParams<T>(T entity)
        {
            ErrCode errCode = ErrCode.OK;
            //PropertyInfo [] props = typeof(T).GetProperties();
            foreach (PropertyInfo prop in typeof(T).GetProperties())
            {
                //Console.WriteLine("{0} = {1}", prop.Name, prop.GetValue(user, null));
                Field field = prop.GetValue(entity) as Field;
                if (field != null)
                {
                    //if ((field.text == string.Empty) || (field.text == ErrCode.EPARAM.ToString()))
                    if (field.error == ErrCode.EPARAM)
                    {
                        //if ((field.text == string.Empty) && (prop.Name == "State"))
                        //    continue;
                        if (field.control != null)
                        {
                            field.control.ForeColor = Color.Red;
                            field.control.Text = field.dText;
                        }
                        errCode = ErrCode.EPARAM;
                    }
                }
            }
            return errCode;
        }

        //public void updateField(Field field)
        //{
        //    if (field.control != null)
        //    {
        //        if (field.control.Name == Gui.DefaultText)
        //            field.text = string.Empty;
        //        else
        //            field.text = field.control.Text;
        //    }
        //}

        private async void buttonFarmEditSubmit_Click(object sender, EventArgs e)
        {
            ErrCode errCode = ErrCode.ERROR;

            allControlsDisable();
            Farm f = farm.clone() as Farm;
            updateParams(f);
            if ((errCode = checkParams(f)) == ErrCode.OK)
            {
                JsonDocument jsonDocument = await web.entityEdit<FarmJson>(f, "api/p/farms/" + farm.Id + "/");
                if (jsonDocument != null)
                {
                    if ((errCode = responseParse<ServiceJson>(jsonDocument)) == ErrCode.OK)
                    {
                        updateParams(farm);
                        farmRow.ItemArray = entityToRow(farm).ToArray<string>();

                        Gui.hide(this);
                        screenFarmShow();
                        errCode = ErrCode.OK;
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

            allControlsDisable();

            Service s = service.clone() as Service;
            updateParams(s);
            if ((errCode = checkParams(s)) == ErrCode.OK)
            {
                JsonDocument jsonDocument = await web.entityEdit<ServiceJson>(s, "api/p/service_providers/" + service.Id + "/");
                if (jsonDocument != null)
                {
                    if ((errCode = responseParse<ServiceJson>(jsonDocument)) == ErrCode.OK)
                    {
                        //serviceTable.Rows.Remove(serviceRow);
                        //serviceTable.Rows.Add(entityToRow(service).ToArray<string>());
                        updateParams(service);
                        serviceRow.ItemArray = entityToRow(service).ToArray<string>();

                        Gui.hide(this);
                        screenServiceShow();
                        errCode = ErrCode.OK;
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
