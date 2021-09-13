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
        private ProgressBar progressBar1;
        private DataGridView dataGridView1;
        private bool AMConnected = false;
        //private Am am = new Am();

        //private Web web = new Web();

        //private List<Farm> farms = null;
        //private Farm farm = null;

        //private List<Service> services = null;
        //private Service service = null;

        private string tabletNo = null;

        //private List<TreatmentPackage> treatmentPackages = null;

        //private SettingsJson settings = null;

        //private Login login = null;
        //private UserJson user = null;

        //private Action action;
        //private Password password;
        //private Reset reset;
        private Field search;

        private Data data = new Data();

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
                try { return wmi.GetPropertyValue("SerialNumber").ToString(); }
                catch (Exception ex) { LogFile.logWrite(ex.ToString()); }
            }
            return "BIOS Serial Number: Unknown";
        }

        //private void hide()
        //{
        //    while (layout.Children.Count > 0)
        //        layout.Children.RemoveAt(0);
        //}
        public void hide()
        {
            while (Controls.Count > 0)
                Controls[0].Dispose();
        }

        private void enabled(bool enabled)
        {
            foreach (Control control in this.Controls)
                control.Enabled = enabled;
        }

        private async Task notify(string title, string messages, string cancel)
        //private DialogResult notify(List<string> text, NotifyButtons notifyButtons, string caption)
        {
            DialogResult dialogResult = default;
            if ((title != null) && (messages != null) && (cancel != null))
            {
                FormNotify formNotify = new FormNotify(new List<string>() { messages}, NotifyButtons.OK, title);
                formNotify.ShowDialog();
                dialogResult = formNotify.DialogResult;
                formNotify.Dispose();
            }
            //return dialogResult;
        }

        private async Task<bool> notify(string title, string messages, string accept, string cancel)
        {
            DialogResult dialogResult = default;
            if ((title != null) && (messages != null) && (cancel != null))
            {
                FormNotify formNotify = new FormNotify(new List<string>() { messages }, NotifyButtons.YesNo, title);
                formNotify.ShowDialog();
                dialogResult = formNotify.DialogResult;
                formNotify.Dispose();
            }
            return dialogResult == DialogResult.Yes ? true : false;
        }

        //private void draw<T>(StackLayout layout, T entity)
        private void draw<T>(T entity)
        {
            foreach (PropertyInfo prop in entity.GetType().GetProperties())
            {
                //Console.WriteLine("{0} = {1}", prop.Name, prop.GetValue(user, null));
                //PropertyInfo prop = props.ElementAt(controls.IndexOf(control));
                Field field = prop.GetValue(entity) as Field;
                if (field != null)
                {
                    if (field.view)
                    {
                        field.draw(this, false);
                        field.draw(this, true);
                    }
                }
            }
        }

        public FormMain()
        {
            InitializeComponent();
            this.Size = new Size(2400, 2400);// 1600);
            this.Scale(new SizeF(Field.ScaleFactor, Field.ScaleFactor));
            this.Font = new Font("Segoe UI", Field.DefaultFont, FontStyle.Regular, GraphicsUnit.Point);
            //pictureBoxTitle.Scale(new SizeF(Misc.ScaleFactor, Misc.ScaleFactor));
            this.Text += " Version " + Const.Version;
            //login = new Login(forgot_Click, buttonLogin_Click) { tablet = TabletNo };
            //if (login != null)
            //    login.drawFields(this);
            if (data != null)
            {
                data.web = new Web();
                data.am = new Am();
                data.login = new Login(forgot_Click, buttonLogin_Click, hide, enabled, notify, draw, dshow: screenActionShow) { tablet = TabletNo };
                data.password = new Password(buttonChangePassword_Click, hide, enabled, notify, draw);
                if (data.login != null)
                    data.login.ddraw(data.login);
                //draw(data.login);
            }
        }

        private void forgot_Click(object sender, EventArgs e)
        {
            //if (login != null)
            //    login.hide();
            //reset = new Reset(buttonResetPassword_Click);
            //if (reset != null)
            //    reset.drawFields(this);
            if (data.login != null)
                //login.hide();
                hide();
            data.reset = new Reset(buttonResetPassword_Click, hide, enabled, notify, draw);
            if (data.reset != null)
                data.reset.ddraw(data.reset);
        }

        private async void buttonResetPassword_Click(object sender, EventArgs e)
        {
            if ((data != null) && (data.reset != null))
            {
                data.login = new Login(forgot_Click, buttonLogin_Click, hide, enabled, notify, draw, dshow: screenActionShow) { tablet = TabletNo };
                await data.reset.send(data);
            }
            //if ((reset != null) && (web != null) && (login != null))
            //{
            //    reset.send<ResetJson, Reset>(reset, "api/p/password_reset/", web.entityAdd,
            //        "Reset Password Success", "Reset Password Failed", true,
            //        new Gui.dResponseOk<Reset>(async (rreset) =>
            //        {
            //            login = new Login(forgot_Click, buttonLogin_Click) { tablet = TabletNo };
            //            if (login != null)
            //                login.drawFields(this);
            //            return ErrCode.OK;
            //        }),
            //        messagesOk: new List<string>() { "An email with your logon details was sent.", "Please use those details to logon." },
            //        messagesErr: new List<string>() { "Reset password failed,", "please enter a valid values" });
            //}
        }

        private void logout()
        {
            clearAM();

            data.web = new Web();

            data.farms = null;
            data.farm = null;

            data.services = null;
            data.service = null;

            tabletNo = null;

            data.treatmentPackages = null;

            data.settings = null;

            //login = new Login(forgot_Click, buttonLogin_Click) { tablet = TabletNo };
            data.login = new Login(forgot_Click, buttonLogin_Click, hide, enabled, notify, draw, dshow: screenActionShow) { tablet = TabletNo };
            data.user = null;
        }

        private void clearAM()
        {
            AMConnected = false;
            data.am = new Am();
        }

        private async void buttonLogin_Click(object sender, EventArgs e)
        {
            if ((data != null) && (data.login != null))
            {
                data.login.Email.control.Text = "yael@gmail.com";
                data.login.Password.control.Text = "yael1234";
                //data.login.tablet = "PF1C9VKU";
                //        //login.email = "yaelv@armentavet.com";
                //        //login.password = "Yyyaeeel123";
                //        //login.tablet = "kjh1g234123";
                await data.login.send(data);
            }
            //if ((login != null) && (web != null))
            //{
            //    login.Email.control.Text = "yael@gmail.com";
            //    login.Password.control.Text = "yael1234";
            //    //        //login.email = "yaelv@armentavet.com";
            //    //        //login.password = "Yyyaeeel123";
            //    //        //login.tablet = "kjh1g234123";
            //    login.send<LoginJson, Login>(login, "api/p/login/", web.login,
            //        "Login Success", "Login Failed", false,
            //        new Gui.dResponseOk<Login>(async (rlogin) =>
            //        {
            //            if (rlogin.token != null)
            //            {
            //                // if ok
            //                user = rlogin.user;
            //                if (!user.is_password_changed)
            //                {
            //                    password = new Password(buttonChangePassword_Click);
            //                    if (password != null)
            //                    {
            //                        password.drawFields(this);
            //                        return ErrCode.OK;
            //                    }
            //                    return ErrCode.ERROR;
            //                }
            //                JsonDocument jsonDocument = await web.getConstants();
            //                if (jsonDocument != null)
            //                    Const.parseConstants(jsonDocument);
            //                farms = await web.entityGet<List<Farm>>("api/p/farms/");
            //                if (farms != null)
            //                    farms = farms.Where(f => f.is_active).ToList();
            //                services = await web.entityGet<List<Service>>("api/p/service_providers/");
            //                treatmentPackages = await web.entityGet<List<TreatmentPackage>>("api/p/treatment_package/");
            //                if (treatmentPackages != null)
            //                    treatmentPackages = treatmentPackages.Where(t => t.is_active).ToList();
            //                settings = await web.entityGet<SettingsJson>("api/p/settings/");
            //
            //                if ((user != null) && (farms != null) && (services != null) && (treatmentPackages != null) && (settings != null) &&
            //                    (Const.DCOUNTRY != null) && (Const.COUNTRY != null) && (Const.DSTATE != null) && (Const.STATE != null) &&
            //                    (Const.FARM_TYPE != null) && (Const.BREED_TYPE != null) && (Const.MILKING_SETUP_TYPE != null) &&
            //                    (Const.LOCATION_OF_TREATMENT_TYPE != null) && (Const.CONTRACT_TYPE != null))
            //                {
            //                    screenActionShow();
            //                }
            //                return ErrCode.OK;
            //            }
            //            return ErrCode.ERROR;
            //        }),
            //        messagesErr: new List<string>() { "Login failed, check your username and password,",
            //            "make sure your tablet is connected to the internet" });
            //}
        }

        //private DialogResult notify(List<string> text, NotifyButtons notifyButtons, string caption)
        //{
        //    DialogResult dialogResult = default;
        //    if (text.Count > 0)
        //    {
        //        FormNotify formNotify = new FormNotify(text, notifyButtons, caption);
        //        formNotify.ShowDialog();
        //        dialogResult = formNotify.DialogResult;
        //        formNotify.Dispose();
        //    }
        //    return dialogResult;
        //}

        private async void buttonChangePassword_Click(object sender, EventArgs e)
        {
            if ((data != null) && (data.password != null))
            {
                await data.password.send(data);
            }
            //if ((password != null) && (web != null) && (login != null))
            //{
            //    password.send<PasswordJson, Password>(password, "api/p/password/change/", web.entityAdd,
            //        "Change Password Success", "Change Password Failed", true,
            //        new Gui.dResponseOk<Password>(async (rpassword) =>
            //        {
            //            if (login != null)
            //            {
            //                login.password = null;
            //                login.drawFields(this);
            //            }
            //            return ErrCode.OK;
            //        }),
            //        dcheck: new Gui.dCheck(() =>
            //        {
            //            ErrCode errcode = ErrCode.OK;
            //            if (password.new_password1 != password.new_password2)
            //                errcode = ErrCode.EMATCH;
            //            else if (password.new_password1 == null)
            //                errcode = ErrCode.ELENGTH;
            //            else if (password.new_password1.Length < 8)
            //                errcode = ErrCode.ELENGTH;
            //            
            //            if (errcode != ErrCode.OK)
            //            {
            //                password.Password1.error = ErrCode.EPARAM;
            //                password.Password2.error = ErrCode.EPARAM;
            //            }
            //            return errcode;
            //        }),
            //        dresponseErr: new Gui.dResponseErr(async (errcode) =>
            //        {
            //            if (errcode == ErrCode.EMATCH)
            //                return new List<string>() { "Can't confirm the password,", "the two values don't match" };
            //            else if (errcode == ErrCode.ELENGTH)
            //                return new List<string>() { "The password specified is less than 8 characters long" };
            //            return new List<string>() { "Change password failed,", "please enter a valid values" };
            //        }));
            //}
        }

        private void screenActionShow()
        {
            clearAM();
            
            new Field(ltype: typeof(PictureBox), lplacev: Place.One).draw(this, true);
            new Field(ltype: typeof(Label), ltext: "Welcome distributor", font: Field.DefaultFontLarge, lplacev: Place.Two).draw(this, true);
            new Field(ltype: typeof(Label), ltext: "Choose Action: ", lplacev: Place.Four).draw(this, true);
            new Field(ltype: typeof(Button), ltext: "Update AM", eventHandler: buttonUpdateAM_Click, width: Field.DefaultWidthMedium, lplacev: Place.Five).draw(this, true);
            new Field(ltype: typeof(Button), ltext: "Manage Farms", eventHandler: buttonFarm_Click, width: Field.DefaultWidthMedium, lplacev: Place.Six).draw(this, true);
            new Field(ltype: typeof(Button), ltext: "Manage Service provider", eventHandler: buttonService_Click, width: Field.DefaultWidthMedium, lplacev: Place.Seven).draw(this, true);
            new Field(ltype: typeof(LinkLabel), ltext: "Calculate your farm’s profits with APT",
                linkEventHandler: linkLabel2_LinkClicked, lplacev: Place.Nine).draw(this, true);
            new Field(ltype: typeof(Button), ltext: "Logout", eventHandler: buttonLogout_Click, lplacev: Place.End).draw(this, true);
        }

        //public void hide()
        //{
        //    while (Controls.Count > 0)
        //        Controls[0].Dispose();
        //}

        private void buttonLogout_Click(object sender, EventArgs e)
        {
            logout();
            hide();
            //if (login != null)
            //    login.drawFields(this);
            if ((data != null) && (data.login != null))
                data.login.ddraw(data.login);
        }

        private void buttonUpdateAM_Click(object sender, EventArgs e)
        {
            //allControlsDisable();
            enabled(false);
            hide();
            screenConnectShow();
        }

        private async void buttonFarm_Click(object sender, EventArgs e)
        {
            //allControlsDisable();
            enabled(false);
            hide();
            screenFarmShow();
        }

        private async void buttonService_Click(object sender, EventArgs e)
        {
            //allControlsDisable();
            enabled(false);
            hide();
            screenServiceShow();
        }

        //private void allControlsDisable()
        //{
        //    foreach (Control control in this.Controls)
        //        control.Enabled = false;
        //}

        //private void allControlsEnable()
        //{
        //    foreach (Control control in this.Controls)
        //        control.Enabled = true;
        //}

        private DataTable entityTableGet(List<Entity> entities)
        {
            DataTable entityTable = new DataTable();
            entityTable.Columns.AddRange(new List<DataColumn>(){new DataColumn("Name"),
                                    new DataColumn("Country / State"),
                                    new DataColumn("City"),
                                    new DataColumn("Address"),
                                    new DataColumn("Contract") }.ToArray());
            foreach (var entity in entities.Select(e => entityToRow(e)))
            {
                if (entity != null)
                    entityTable.Rows.Add(entity.ToArray<string>());
            }
            return entityTable;
        }

        private List<string> entityToRow(Entity entity)
        {
            List<string> l = null;
            
            if (entity != null)
                l = new List<string> { entity.Name.val, entity.Country.val + ((entity.State.val == string.Empty) ? string.Empty : " / ") + 
                    entity.State.val, entity.City.val, entity.Address.val, entity.ContractType.val};
            
            return l;
        }

        private void screenConnectShow()
        {
            new Field(ltype: typeof(PictureBox), lplacev: Place.One).draw(this, true);
            new Field(ltype: typeof(Label), ltext: "Welcome distributor", font: Field.DefaultFontLarge, lplacev: Place.Two).draw(this, true);
            new Field(ltype: typeof(Label), ltext: "Please make sure the AM is connected to your tablet before continue", lplacev: Place.Four).draw(this, true);
            label1 = new Field(ltype: typeof(Label), lplacev: Place.Six).draw(this, true) as Label;
            progressBar1 = new Field(ltype: typeof(ProgressBar), width: Field.DefaultWidthLarge, height: Field.DefaultHeightSmall, lplacev: Place.Ten).draw(this, true) as ProgressBar;
            button1 = new Field(ltype: typeof(Button), ltext: "Check AM present", eventHandler: buttonCheckAM_Click, lplacev: Place.End).draw(this, true) as Button;
            if (AMConnected)
                AMConnectedShow();
            else
            {
                AMDisconnectedShow();
                if (label1 != null)
                    label1.Visible = false;
                //button2.Enabled = false;
            }
            if (progressBar1 != null)
                progressBar1.Visible = false;
        }

        private async void buttonCheckAM_Click(object sender, EventArgs e)
        {
            if ((label1 != null) && (button1 != null) && (progressBar1 != null))
            {
                label1.Visible = false;
                button1.Enabled = false;
                //button2.Enabled = false;
                progressBar1.Visible = true;
                data.am.serialPortProgressEvent += progressBar_Callback;
                progressBar1.Minimum = 0;
                progressBar1.Value = progressBar1.Minimum;
                progressBar1.Maximum = 20;

                ErrCode errcode = ErrCode.ERROR;

                if ((errcode = await data.am.AMDataCheckConnect()) == ErrCode.OK)
                    errcode = await data.am.AMDataRead();

                progressBar1.Value = progressBar1.Maximum;
                
                if (errcode == ErrCode.OK)
                {
                    //if ok
                    AMConnected = true;
                    hide();
                    screenInfoShow();
                }
                else
                {
                    // if fail
                    clearAM();
                    AMDisconnectedShow();

                    //notify(new List<string>() { "AM not found make sure the AM is connected", "to the tablet by using a USB cable" },
                    //    NotifyButtons.OK, caption: "Am not connected");
                    await notify("Am not connected", "AM not found make sure the AM is connected\nto the tablet by using a USB cable", "OK");
                }
                label1.Visible = true;
                button1.Enabled = true;
                progressBar1.Visible = false;
            }
        }

        private void AMDisconnectedShow()
        {
            if (label1 != null)
                label1 = new Field(ltype: typeof(Label), ltext: "AM not found – make sure AM is connected using USB cable", color: Color.Red,
                    lplaceh: Place.Center, lplacev: Place.Six).draw(this, true) as Label;
            //button2.Enabled = false;
        }

        private void AMConnectedShow()
        {
            if (label1 != null)
                label1 = new Field(ltype: typeof(Label), dflt: "AM found – connected to AM", color: Color.Green,
                    placeh: Place.Center, placev: Place.Six).draw(this, true) as Label;
            //button2.Enabled = true;
        }

        private void buttonConnectForward_Click(object sender, EventArgs e)
        {
            hide();
            screenInfoShow();
        }

        private void screenInfoShow()
        {
            new Field(ltype: typeof(PictureBox), lplacev: Place.One).draw(this, true);
            new Field(ltype: typeof(Label), ltext: "Welcome distributor", font: Field.DefaultFontLarge, lplacev: Place.Two).draw(this, true);
            new Field(ltype: typeof(Label), ltext: "AM identified with SN: " + data.am.SNum, lplacev: Place.Four).draw(this, true);
            new Field(ltype: typeof(Label), ltext: "Current available treatments: " + data.am.Maxi / data.settings.number_of_pulses_per_treatment, lplacev: Place.Six).draw(this, true);
            new Field(ltype: typeof(Button), ltext: "Back", eventHandler: buttonInfoBack_Click, lplaceh: Place.Five, lplacev: Place.End).draw(this, true);
            new Field(ltype: typeof(Button), ltext: "Continue", eventHandler: buttonInfoContinue_Click, lplaceh: Place.Two, lplacev: Place.End).draw(this, true);
        }

        private void buttonInfoBack_Click(object sender, EventArgs e)
        {
            hide();
            screenConnectShow();
        }

        private void buttonInfoContinue_Click(object sender, EventArgs e)
        {
            hide();
            //action = new Action(am, TabletNo, farms.ToArray(), services.ToArray(),
            //    comboBoxPartNumber_SelectedIndexChanged, comboBoxFarm_SelectedIndexChanged, radioButton_CheckedChanged,
            //    buttonTreatCansel_Click, buttonTreatApprove_Click);
            data.action = new Action(data.am, TabletNo, data.farms.ToArray(), data.services.ToArray(),
                comboBoxPartNumber_SelectedIndexChanged, comboBoxFarm_SelectedIndexChanged, radioButton_CheckedChanged,
                buttonTreatCansel_Click, buttonTreatApprove_Click, hide, enabled, notify, draw, dshow: screenActionShow, dnotifyAnswer: notify);
            //if ((action != null) && (action.RadioFarm != null) && (action.Progress != null))
            if ((data != null) && (data.action != null) && (data.action.RadioFarm != null) && (data.action.Progress != null))
            {
                //action.drawFields(this);
                data.action.ddraw(data.action);

                RadioButton radioButton = data.action.RadioFarm.lcontrol as RadioButton;
                if (radioButton != null)
                    radioButton.Checked = true;
                ProgressBar progressBar = data.action.Progress.lcontrol as ProgressBar;
                if (progressBar != null)
                    progressBar.Visible = false;
            }
        }

        private void comboBoxPartNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                TreatmentPackage treatmentPackage = comboBox.SelectedItem as TreatmentPackage;
                //if ((treatmentPackage != null) &&(settings != null))
                if ((treatmentPackage != null) && (data != null) && (data.settings != null))
                {
                    //action.PartNumber.val = treatmentPackage.PartNumber;
                    //am.MaxiSet = (uint)(treatmentPackage.amount_of_treatments * settings.number_of_pulses_per_treatment);
                    data.action.PartNumber.val = treatmentPackage.PartNumber;
                    data.am.MaxiSet = (uint)(treatmentPackage.amount_of_treatments * data.settings.number_of_pulses_per_treatment);
                }
            }
        }

        private void comboBoxFarm_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            //if ((action != null) && (action.PartNumber != null))
            if ((data != null) && (data.action != null) && (data.action.PartNumber != null))
            {
                ComboBox comboBoxPN = data.action.PartNumber.control as ComboBox;
                if ((comboBox != null) && (comboBox.SelectedItem != null) && (comboBoxPN != null) && (data.treatmentPackages != null))
                {
                    data.action.PartNumber.removeItems();
                    Farm farm = comboBox.SelectedItem as Farm;
                    Service service = comboBox.SelectedItem as Service;
                    Entity entity = null;
                    if (farm != null)
                    {
                        entity = farm;
                        data.action.Farm.val = farm.Id;
                        //action.Service.val = null;
                    }
                    if (service != null)
                    {
                        entity = service;
                        //action.Farm.val = null;
                        data.action.Service.val = service.Id;
                    }
                    if (entity != null)
                    {
                        if ((data.treatmentPackages != null) && (data.settings != null) && (data.am != null))
                        {
                            //action.PartNumber.items = treatmentPackages.Where(t => (t.contract_type == entity.contract_type)).ToArray();
                            if ((data.action.PartNumber.items = data.treatmentPackages.Where(t => (t.contract_type == entity.contract_type) &&
                                 ((t.amount_of_treatments * data.settings.number_of_pulses_per_treatment + data.am.Maxi) < data.settings.max_am_pulses)).ToArray()) != null)
                            {
                                data.action.PartNumber.addItems(data.action.PartNumber.items);
                                data.action.PartNumber.control.Text = data.action.PartNumber.dflt;
                                data.action.PartNumber.control.ForeColor = Color.Silver;
                                data.action.PartNumber.val = data.action.PartNumber.dflt;
                                if (data.action.PartNumber.items.Length == 0)
                                    notify("Part Number Error", "The attached AM reached the max allowed treatments.\n" +
                                       "There are no available part numbers.\n" +
                                       "Please replace the AM or contact support.", "OK");
                            }
                            //action.PartNumber.addItems(treatmentPackages.Where(t => (t.contract_type == entity.contract_type)).ToArray());
                        }
                    }
                }
            }
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if ((data != null) && (data.action != null) && (data.action.RadioFarm != null) && (data.action.RadioService != null))
            {
                RadioButton radioFarm = data.action.RadioFarm.lcontrol as RadioButton;
                RadioButton radioService = data.action.RadioService.lcontrol as RadioButton;
                if ((radioButton != null) && (radioFarm != null) && (radioService != null)
                     && (data.action.Farm != null) && (data.action.Farm.control != null)
                     && (data.action.Service != null) && (data.action.Service.control != null)
                     && (data.action.PartNumber != null) && (data.action.PartNumber.control != null))
                {
                    if (radioButton.Checked)
                    {
                        if (radioButton == radioFarm)
                        {
                            radioService.Checked = false;
                            data.action.Farm.control.Visible = true;
                            data.action.Farm.view = true;

                            data.action.Service.control.Visible = false;
                            data.action.Service.view = false;
                        }
                        if (radioButton == radioService)
                        {
                            radioFarm.Checked = false;
                            data.action.Farm.control.Visible = false;
                            data.action.Farm.view = false;

                            data.action.Service.control.Visible = true;
                            data.action.Service.view = true;
                        }
                        data.action.Farm.control.Text = data.action.Farm.dflt;
                        data.action.Farm.control.ForeColor = Color.Silver;
                        data.action.Farm.val = data.action.Farm.dflt;
                        data.action.Service.control.Text = data.action.Service.dflt;
                        data.action.Service.control.ForeColor = Color.Silver;
                        data.action.Service.val = data.action.Service.dflt;
                    }
                    else
                    {
                        data.action.PartNumber.removeItems();
                        data.action.PartNumber.control.Text = data.action.PartNumber.dflt;
                        data.action.PartNumber.control.ForeColor = Color.Silver;
                        data.action.PartNumber.val = data.action.PartNumber.dflt;
                    }
                }
            }
        }

        private void progressBar_Callback(object sender, EventArgs e)
        {
            ProgressBar progressBar;
            if ((data != null) && (data.action != null) && (data.action.Progress != null))
                progressBar = data.action.Progress.lcontrol as ProgressBar;
            else
                progressBar = progressBar1;

            if (progressBar != null)
            {
                SerialPortEventArgs args = e as SerialPortEventArgs;
                if (args != null)
                {
                    //if (args.progress == 0)
                    //    progressBar.Maximum = progressBar.Value + args.maximum * 2;

                    if ((progressBar.Value + args.progress) <= progressBar.Maximum)
                        progressBar.Value += args.progress;
                }
            }
        }

        private async void buttonTreatCansel_Click(object sender, EventArgs e)
        {
            if ((data != null) && (data.action != null))
            {
                //DialogResult dialogResult = action.notify(new List<string>() { "Are you sure you want to cancel the operation?" }, NotifyButtons.YesNo, caption: "Abort");
                bool answer = await notify("Abort", "Are you sure you want to cancel the operation?", "Yes", "No");
                //if (dialogResult == DialogResult.Yes)
                if (answer)
                {
                    //action.hide();
                    data.action.dhide();
                    //clearAM();
                    screenActionShow();
                }
            }
        }

        private async void buttonTreatApprove_Click(object sender, EventArgs e)
        {
            //if ((action != null) && (am != null) && (settings != null))
            if ((data != null) && (data.action != null))
            {
                ErrCode errcode = ErrCode.ERROR;
                //uint maxi = am.Maxi;
                ProgressBar progressBar = data.action.Progress.lcontrol as ProgressBar;

                if (progressBar != null)
                {
                    progressBar.Visible = true;
                    data.am.serialPortProgressEvent += progressBar_Callback;
                    progressBar.Minimum = 0;
                    progressBar.Value = progressBar.Minimum;
                    progressBar.Maximum = 90;

                    errcode = await data.action.send(data);

                    if (errcode == ErrCode.OK)
                        progressBar.Value = progressBar.Maximum;
                    else
                        progressBar.Visible = false;
                }
            }
            //ProgressBar progressBar = action.Progress.lcontrol as ProgressBar;

            //await action.send<ActionJson, Action>(action, "api/p/actions/", web.entityAdd,
            //    "Approve Success", "Approve Failed", false,
            //    new Gui.dResponseOk<Action>(async (raction) =>
            //    {
            //        action.notify(new List<string>() {
            //                string.Format("The original amount of treatments: {0}", maxi / settings.number_of_pulses_per_treatment),
            //                string.Format("Added treatments: {0}",am.MaxiSet / settings.number_of_pulses_per_treatment),
            //                string.Format("The treatments available on AM - SN {1}: {0}", am.Maxi / settings.number_of_pulses_per_treatment, am.SNum),
            //                "please disconnect the AM"}, NotifyButtons.OK, "Approve Failed");
            //        clearAM();
            //        screenActionShow();
            //        return ErrCode.OK;
            //    }),
            //    dapprove: new Gui.dApprove(async () =>
            //    {
            //        if (((action.farm != null) || (action.service_provider != null)))
            //        {
            //            if ((am.Maxi + am.MaxiSet) < settings.max_am_pulses)
            //            {
            //                DialogResult dialogResult = action.notify(new List<string>() {
            //                            string.Format("{0} current treatments available",am.Maxi / settings.number_of_pulses_per_treatment),
            //                            string.Format("{0} treatments will be added",am.MaxiSet / settings.number_of_pulses_per_treatment),
            //                            string.Format("AM - SN: {0}", am.SNum),
            //                            (action.farm != null) ? string.Format("Farm: {0}", farms.Find(f => f.id == action.farm).Name.val) :
            //                            ((action.service_provider != null) ? string.Format("Service Provider: {0}", services.Find(s => s.id == action.service_provider).Name.val) : string.Empty),
            //                            "Press the button to proceed"}, NotifyButtons.YesNo, caption: "Approve");
            //                if (dialogResult == DialogResult.Yes)
            //                {
            //                    if (progressBar != null)
            //                        progressBar.Visible = true;
            //                    am.serialPortProgressEvent += new EventHandler(progressBar_Callback);
            //                    if (progressBar != null)
            //                        progressBar.Value = progressBar.Minimum;
            //
            //                    if ((errcode = await am.AMDataWrite()) == ErrCode.OK)
            //                    {
            //                        if ((errcode = await am.AMDataRead()) == ErrCode.OK)
            //                        {
            //                            return ErrCode.OK;
            //                        }
            //                    }
            //                }
            //                else
            //                    errcode = ErrCode.CANSEL;
            //            }
            //            else
            //                errcode = ErrCode.EMAX;
            //        }
            //        else
            //            errcode = ErrCode.EPARAM;
            //        return errcode;
            //    }),
            //    dresponseErr: new Gui.dResponseErr(async (errcode) =>
            //    {
            //        List<string> errors;
            //        if (errcode == ErrCode.EPARAM)
            //            errors = new List<string>() { "Wrong parameters,", "please choose the Farm / Service provider",
            //                "and the number of treatments" };
            //        else if (errcode == ErrCode.EMAX)
            //            errors = new List<string>() { "Wrong part number,", "the maximum number of treatments reached,",
            //                "please choose a smaller number of treatments" };
            //        else if (errcode == ErrCode.SERROR)
            //        {
            //            am.Maxi -= am.MaxiSet;
            //            am.MaxiSet = 0;
            //            action.notify(new List<string>() { string.Format("Approve failed, restoring AM to {0} treatments", am.Maxi / settings.number_of_pulses_per_treatment) }, NotifyButtons.OK, "Approve Failed");
            //            if (progressBar != null)
            //                progressBar.Value = progressBar.Minimum;
            //
            //            if (await am.AMDataWrite() == ErrCode.OK)
            //            {
            //                if (await am.AMDataRead() == ErrCode.OK)
            //                    errors = new List<string>() { "Approve failed,", string.Format("AM sucsessfully restored to {0} treatments", am.Maxi / settings.number_of_pulses_per_treatment) };
            //                else
            //                    errors = new List<string>() { "Approve failed,", string.Format("Failed to restore to {0} treatments", am.Maxi / settings.number_of_pulses_per_treatment) };
            //            }
            //            else
            //                errors = new List<string>() { "Approve failed,", "Faild to restore to original values" };
            //        }
            //        else if (errcode == ErrCode.CANSEL)
            //            errors = null;
            //        else
            //            errors = new List<string>() { "The operation failed, the treatments were not added" };
            //
            //        if (progressBar != null)
            //            progressBar.Visible = false;
            //
            //        return errors;
            //    }));
            //}
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
            if ((dataGridView1 != null) && (data != null) && (data.farms != null))
                dataGridView1.DataSource = entityTableGet(data.farms.Cast<Entity>().ToList());
        }

        private void screenServiceShow()
        {
            screenDataGridShow("Manage Service providers",
                new EventHandler(buttonServiceEdit_Click),
                new EventHandler(buttonServiceAdd_Click),
                new EventHandler(richTextBoxServiceSearch_TextChanged),
                new EventHandler(buttonBackToAction_Click));
            if ((dataGridView1 != null) && (data != null) && (data.services != null))
                dataGridView1.DataSource = entityTableGet(data.services.Cast<Entity>().ToList());
        }

        private void richTextBoxFarmSearch_TextChanged(object sender, EventArgs e)
        {
            if ((dataGridView1 != null) && (data != null) && (data.farms != null) && (sender != null))
            {
                DataTable table = null;
                if ((table = richTextBoxSearch(sender, data.farms.Cast<Entity>())) != null)
                    dataGridView1.DataSource = table;
            }
        }

        private void richTextBoxServiceSearch_TextChanged(object sender, EventArgs e)
        {
            if ((dataGridView1 != null) && (data != null) && (data.services != null) && (sender != null))
            {
                DataTable table = null;
                if ((table = richTextBoxSearch(sender, data.services.Cast<Entity>())) != null)
                    dataGridView1.DataSource = table;
            }
        }

        private DataTable richTextBoxSearch(object sender, IEnumerable<Entity> entities)
        {
            RichTextBox richTextBox = sender as RichTextBox;
            DataTable table = null;
            if ((richTextBox != null) && (richTextBox.Text != null) && (dataGridView1 != null) && (entities != null) && (search != null))
            {
                if (richTextBox.Text != search.dflt)
                {
                    table = entityTableGet(entities.Where(e => (e.Name.val != null) && (e.Name.val.ToLower().Contains(richTextBox.Text.ToLower()))).ToList());
                    dataGridView1.DataSource = table;
                }
            }
            return table;
        }

        private void screenDataGridShow(string dataName, EventHandler eventHandlerButton1, EventHandler eventHandlerButton2, EventHandler eventHandlerButton3, EventHandler eventHandlerButton4)
        {
            new Field(ltype: typeof(PictureBox), lplacev: Place.One).draw(this, true);
            new Field(ltype: typeof(Label), ltext: dataName, lplacev: Place.Two).draw(this, true);
            new Field(ltype: typeof(Button), ltext: "Back", eventHandler: eventHandlerButton4, lplaceh: Place.Four, lplacev: Place.Three).draw(this, true);
            search = new Field(type: typeof(RichTextBox), dflt: "Search", eventHandler: eventHandlerButton3, placeh: Place.Six, placev: Place.Three);
            search.draw(this, false);
            new Field(ltype: typeof(Button), ltext: "Edit", eventHandler: eventHandlerButton1, lplaceh: Place.One, lplacev: Place.Three).draw(this, true);
            new Field(ltype: typeof(Button), ltext: "Add New", eventHandler: eventHandlerButton2, lplaceh: Place.Three, lplacev: Place.Three).draw(this, true);
            Field.dataGridDraw(this, ref dataGridView1, placev: Place.Four);
        }

        private void buttonBackToAction_Click(object sender, EventArgs e)
        {
            hide();
            screenActionShow();
        }

        private void buttonFarmAdd_Click(object sender, EventArgs e)
        {
            hide();
            //farm = new Farm(false, comboBoxCountry_SelectedIndexChanged, buttonFarmCancel_Click, buttonFarmAddSubmit_Click);
            //if (farm != null)
            //    farm.drawFields(this);
            if (data != null)
            {
                data.farm = new Farm(false, comboBoxCountry_SelectedIndexChanged, buttonFarmCancel_Click, buttonFarmAddSubmit_Click,
                    hide, enabled, notify, draw, dshow: screenFarmShow);
                if (data.farm != null)
                    data.farm.ddraw(data.farm);
            }
        }

        private void buttonServiceAdd_Click(object sender, EventArgs e)
        {
            hide();
            //service = new Service(false, comboBoxCountry_SelectedIndexChanged, buttonServiceCancel_Click, buttonServiceAddSubmit_Click);
            //if (service != null)
            //    service.drawFields(this);
            if (data != null)
            {
                data.service = new Service(false, comboBoxCountry_SelectedIndexChanged, buttonServiceCancel_Click, buttonServiceAddSubmit_Click,
                hide, enabled, notify, draw, dshow: screenServiceShow);
                if (data.service != null)
                    data.service.ddraw(data.service);
            }
        }

        private void buttonFarmEdit_Click(object sender, EventArgs e)
        {
            if ((data != null) && (data.farms != null))
            {
                data.farm = getCurrentEntity(data.farms.Cast<Entity>()) as Farm;
                if (data.farm != null)
                {
                    hide();
                    //farm.initFields(true, comboBoxCountry_SelectedIndexChanged, buttonFarmCancel_Click, buttonFarmEditSubmit_Click);
                    //farm.drawFields(this);
                    data.farm.initFields(true, comboBoxCountry_SelectedIndexChanged, buttonFarmCancel_Click, buttonFarmEditSubmit_Click,
                        hide, enabled, notify, draw, dshow: screenFarmShow);
                    data.farm.ddraw(data.farm);
                }
            }
        }

        private Entity getCurrentEntity(IEnumerable<Entity> entities)
        {
            if ((entities != null) && (dataGridView1 != null) && (dataGridView1.CurrentRow != null) && (dataGridView1.CurrentRow.Cells != null)
                && (dataGridView1.CurrentRow.Cells.Count > 0))
                return entities.ToList().Find(e => ((e.Name.val != null) && 
                    (e.Name.val == (dataGridView1.CurrentRow.Cells[0].Value as string))));
            return null;
        }

        private void buttonServiceEdit_Click(object sender, EventArgs e)
        {
            if ((data != null) && (data.services != null))
            {
                data.service = getCurrentEntity(data.services.Cast<Entity>()) as Service;
                if (data.service != null)
                {
                    hide();
                    //service.initFields(true, comboBoxCountry_SelectedIndexChanged, buttonServiceCancel_Click, buttonServiceEditSubmit_Click);
                    //service.drawFields(this);
                    data.service.initFields(true, comboBoxCountry_SelectedIndexChanged, buttonServiceCancel_Click, buttonServiceEditSubmit_Click,
                        hide, enabled, notify, draw, dshow: screenServiceShow);
                    data.service.ddraw(data.service);
                }
            }
        }

        private void comboBoxCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                Entity entity = null;
                if ((data != null) && (data.farm != null) && (data.farm.Country != null))
                {
                    if (comboBox == data.farm.Country.control)
                        entity = data.farm;
                }
                if ((data.service != null) && (data.service.Country != null))
                {
                    if (comboBox == data.service.Country.control)
                        entity = data.service;
                }
                if ((entity != null) && (entity.State != null) && (entity.State.control != null))
                {
                    entity.State.control.Text = entity.State.dflt;
                    if (comboBox.Text == "United States of America")
                    {
                        entity.State.enable = true;
                        entity.State.control.Enabled = true;
                    }
                    else
                    {
                        entity.State.enable = false;
                        entity.State.control.Enabled = false;
                    }
                }
            }
        }

        private void buttonFarmCancel_Click(object sender, EventArgs e)
        {
            hide();
            screenFarmShow();
        }

        private void buttonServiceCancel_Click(object sender, EventArgs e)
        {
            hide();
            screenServiceShow();
        }

        private async void buttonFarmAddSubmit_Click(object sender, EventArgs e)
        {
            if ((data != null) && (data.farm != null) && (data.web != null))
            {
                await data.farm.send(data, false);
            }
            //if ((farm != null) && (web != null))
            //{
            //    farm.send<FarmJson, Farm>(farm, "api/p/farms/", web.entityAdd,
            //        "Submit Success", "Submit Failed", false,
            //        new Gui.dResponseOk<Farm>(async (rfarm) =>
            //        {
            //            farms.Add(rfarm);
            //            screenFarmShow();
            //            return ErrCode.OK;
            //        }),
            //        messagesErr: new List<string>() { "Submit failed, can't add empty or negative fields,",
            //            "make sure all the fields are filled with valid values" });
            //}
        }

        private async void buttonServiceAddSubmit_Click(object sender, EventArgs e)
        {
            if ((data != null) && (data.service != null) && (data.web != null))
            {
                await data.service.send(data, false);
            }
            //if ((service != null) && (web != null))
            //{
            //    service.send<ServiceJson, Service>(service, "api/p/service_providers/", web.entityAdd,
            //        "Submit Success", "Submit Failed",
            //        false,
            //        new Gui.dResponseOk<Service>(async (rservice) =>
            //        {
            //            services.Add(rservice);
            //            screenServiceShow();
            //            return ErrCode.OK;
            //        }),
            //        messagesErr: new List<string>() { "Submit failed, can't add empty or negative fields,",
            //            "make sure all the fields are filled with valid values" });
            //}
        }

        private async void buttonFarmEditSubmit_Click(object sender, EventArgs e)
        {
            if ((data != null) && (data.farm != null) && (data.web != null))
            {
                await data.farm.send(data, true);
            }
            //if ((farm != null) && (web != null))
            //{
            //    farm.send<FarmJson, Farm>(farm, "api/p/farms/" + farm.Id + "/", web.entityEdit,
            //        "Submit Success", "Submit Failed", false,
            //        new Gui.dResponseOk<Farm>(async (rfarm) =>
            //        {
            //            farms.Insert(farms.IndexOf(farm), rfarm);
            //            farms.Remove(farm);
            //            screenFarmShow();
            //            return ErrCode.OK;
            //        }),
            //        messagesErr: new List<string>() { "Submit failed, can't add empty or negative fields,",
            //            "make sure all the fields are filled with valid values" });
            //}
        }
        
        private async void buttonServiceEditSubmit_Click(object sender, EventArgs e)
        {
            if ((data != null) && (data.service != null) && (data.web != null))
            {
                await data.service.send(data, true);
            }
            //if ((service != null) && (web != null))
            //{
            //    service.send<ServiceJson, Service>(service, "api/p/service_providers/" + service.Id + "/", web.entityEdit,
            //        "Submit Success", "Submit Failed", false,
            //        new Gui.dResponseOk<Service>(async (rservice) =>
            //        {
            //            services.Insert(services.IndexOf(service), rservice);
            //            services.Remove(service);
            //            screenServiceShow();
            //            return ErrCode.OK;
            //        }),
            //        messagesErr: new List<string>() { "Submit failed, can't add empty or negative fields,",
            //            "make sure all the fields are filled with valid values" });
            //}
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
