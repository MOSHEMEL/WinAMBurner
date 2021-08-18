﻿using System;
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
        private Label label1;
        private Button button1;
        private ProgressBar progressBar1;
        private DataGridView dataGridView1;
        private bool AMConnected = false;
        private AM am = new AM();

        private Web web = new Web();

        private List<Farm> farms = null;
        private Farm farm = null;

        private List<Service> services = null;
        private Service service = null;

        private string tabletNo = null;

        private List<TreatmentPackage> treatmentPackages = null;

        private SettingsJson settings = null;

        private Login login = null;
        private UserJson user = null;

        private Action action;
        private Password password;
        private Reset reset;
        private Field search;

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

        public FormMain()
        {
            InitializeComponent();
            this.Size = new Size(2400, 2400);// 1600);
            this.Scale(new SizeF(Field.ScaleFactor, Field.ScaleFactor));
            this.Font = new Font("Segoe UI", Field.DefaultFont, FontStyle.Regular, GraphicsUnit.Point);
            //pictureBoxTitle.Scale(new SizeF(Misc.ScaleFactor, Misc.ScaleFactor));

            login = new Login(forgot_Click, buttonLogin_Click) { tablet = TabletNo };
            if (login != null)
                login.drawFields(this);
        }

        private void forgot_Click(object sender, EventArgs e)
        {
            if (login != null)
                login.hide();
            reset = new Reset(buttonResetPassword_Click);
            if (reset != null)
                reset.drawFields(this);
        }

        private async void buttonResetPassword_Click(object sender, EventArgs e)
        {
            if (reset != null)
            {
                ErrCode errcode = ErrCode.ERROR;
                List<string> errors = new List<string>();
                List<string> messages = new List<string>();

                reset.disableControls();
                reset.updateParams();

                if ((errcode = reset.checkParams()) == ErrCode.OK)
                {
                    JsonDocument jsonDocument = await web.entityAdd<ResetJson>(reset, "api/p/password/reset/");
                    if (jsonDocument != null)
                    {
                        errcode = responseParse(reset, jsonDocument, errors, messages);

                        Reset rreset = null;
                        try { rreset = JsonSerializer.Deserialize<Reset>(jsonDocument.RootElement.ToString()); }
                        catch (Exception ex) { LogFile.logWrite(ex.ToString()); }

                        if (rreset != null)
                            errcode = ErrCode.OK;
                    }
                }

                if (errcode == ErrCode.EPARAM)
                {
                    screenError(errors, "Reset Password Failed");
                }
                else if (errcode == ErrCode.ERROR)
                {
                    screenError(new List<string>() { "Reset password failed,",
                    "please enter a valid values"}, "Reset Password Failed");
                }
                else if (errcode == ErrCode.OK)
                {
                    screenError(messages, "Success");
                    reset.hide();
                    login = new Login(forgot_Click, buttonLogin_Click) { tablet = TabletNo };
                    if (login != null)
                        login.drawFields(this);
                }
            }
        }

        private void logout()
        {
            clearAM();

            web = new Web();

            farms = null;
            farm = null;

            services = null;
            service = null;

            tabletNo = null;

            treatmentPackages = null;

            settings = null;

            login = new Login(forgot_Click, buttonLogin_Click) { tablet = TabletNo };
            user = null;
        }

        private void clearAM()
        {
            AMConnected = false;
            am = new AM();
        }

        private async void buttonLogin_Click(object sender, EventArgs e)
        {
            if (login != null)
            {
                ErrCode errcode = ErrCode.ERROR;

                login.disableControls();

                login.updateParams();

                //login.email = "yael@gmail.com";
                //login.password = "yael123";
                //login.email = "yaelv@armentavet.com";
                //login.password = "Yyyaeeel123";
                //login.tablet = "kjh1g234123";

                if ((errcode = login.checkParams()) == ErrCode.OK)
                {
                    LoginResponseJson loginResponse = await web.login(login);
                    if ((loginResponse != null) && (loginResponse.token != null))
                    {
                        // if ok
                        user = loginResponse.user;
                        //user.is_password_changed = false;
                        if (!user.is_password_changed)
                        {
                            login.hide();
                            password = new Password(buttonChangePassword_Click);
                            password.drawFields(this);
                            return;
                        }
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
                            login.hide();
                            screenActionShow();
                            //else
                            //{
                            //    password = new Password(buttonChangePassword_Click);
                            //    password.drawFields(this);
                            //}
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
        }

        private void screenError(List<string> text, string caption)
        {
            notify(text, NotifyButtons.OK, caption);
            allControlsEnable();
        }

        private DialogResult notify(List<string> text, NotifyButtons notifyButtons, string caption)
        {
            DialogResult dialogResult = default;
            if (text.Count > 0)
            {
                FormNotify formNotify = new FormNotify(text, notifyButtons, caption);
                formNotify.ShowDialog();
                dialogResult = formNotify.DialogResult;
                formNotify.Dispose();
            }
            return dialogResult;
        }

        private async void buttonChangePassword_Click(object sender, EventArgs e)
        {
            if (password != null)
            {
                ErrCode errcode = ErrCode.ERROR;
                JsonDocument jsonDocument = null;
                List<string> errors = new List<string>();
                List<string> messages = new List<string>();

                password.disableControls();
                password.updateParams();

                //password.new_password1 = "Yyyaeeel123";
                //password.new_password2 = "Yyyaeeel123";

                if ((errcode = password.checkParams()) == ErrCode.OK)
                {
                    jsonDocument = await web.entityAdd<PasswordJson>(password, "api/p/password/change/");
                    if (jsonDocument != null)
                    {

                        Password rpassword = null;
                        try { rpassword = JsonSerializer.Deserialize<Password>(jsonDocument.RootElement.ToString()); }
                        catch (Exception ex) { LogFile.logWrite(ex.ToString()); }

                        if (rpassword != null)
                            errcode = ErrCode.OK;
                    }
                }

                responseParse(password, jsonDocument, errors, messages);

                if (errcode == ErrCode.EPARAM)
                {
                    screenError(errors, "Change Password Failed");
                }
                else if (errcode == ErrCode.ERROR)
                {
                    screenError(new List<string>() { "Change password failed,",
                    "please enter a valid values"}, "Change Password Failed");
                }
                else if (errcode == ErrCode.OK)
                {
                    screenError(messages, "Success");
                    password.hide();
                    if (login != null)
                    {
                        login.password = null;
                        login.drawFields(this);
                    }
                }
            }
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
            hide();
            if (login != null)
                login.drawFields(this);
        }

        private void buttonUpdateAM_Click(object sender, EventArgs e)
        {
            allControlsDisable();
            hide();
            screenConnectShow();
        }

        private async void buttonFarm_Click(object sender, EventArgs e)
        {
            allControlsDisable();
            hide();
            screenFarmShow();
        }

        private async void buttonService_Click(object sender, EventArgs e)
        {
            allControlsDisable();
            hide();
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
                l = new List<string> { entity.Name.val as string, entity.Country.val as string + ((entity.State.val as string == string.Empty) ? string.Empty : " / ") + entity.State.val as string, entity.City.val as string, entity.Address.val as string, entity.ContractType.val as string };
            
            return l;
        }

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
                    hide();
                    screenInfoShow();
                }
                else
                {
                    // if fail
                    clearAM();
                    AMDisconnectedShow();
                    //FormNotify formNotify = new FormNotify(new List<string>()
                    //{ "AM not found make sure the AM is connected",
                    //    "to the tablet by using a USB cable" },
                    //    NotifyButtons.OK, caption: "Am not connected");
                    //formNotify.ShowDialog();
                    //formNotify.Dispose();

                    notify(new List<string>() { "AM not found make sure the AM is connected", "to the tablet by using a USB cable" },
                        NotifyButtons.OK, caption: "Am not connected");
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
            new Field(ltype: typeof(Label), ltext: "AM identified with SN: " + am.SNum, lplacev: Place.Four).draw(this, true);
            new Field(ltype: typeof(Label), ltext: "Current available treatments: " + am.Maxi / settings.number_of_pulses_per_treatment, lplacev: Place.Six).draw(this, true);
            new Field(ltype: typeof(Button), ltext: "Back", buttonEventHandler: buttonInfoBack_Click, lplaceh: Place.Five, lplacev: Place.End).draw(this, true);
            new Field(ltype: typeof(Button), ltext: "Continue", buttonEventHandler: buttonInfoContinue_Click, lplaceh: Place.Two, lplacev: Place.End).draw(this, true);
        }

        private void buttonInfoBack_Click(object sender, EventArgs e)
        {
            hide();
            screenConnectShow();
        }

        private void buttonInfoContinue_Click(object sender, EventArgs e)
        {
            hide();
            action = new Action(am, TabletNo, farms.ToArray(), services.ToArray(),
                comboBoxFarm_SelectedIndexChanged, radioButton_CheckedChanged,
                buttonTreatCansel_Click, buttonTreatApprove_Click);
            if ((action != null) && (action.RadioFarm != null) && (action.Progress != null))
            {
                action.drawFields(this);

                RadioButton radioButton = action.RadioFarm.lcontrol as RadioButton;
                if (radioButton != null)
                    radioButton.Checked = true;
                ProgressBar progressBar = action.Progress.lcontrol as ProgressBar;
                if (progressBar != null)
                    progressBar.Visible = false;
            }
        }

        private void comboBoxFarm_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBoxId = sender as ComboBox;
            if ((action != null) && (action.PartNumber != null))
            {
                ComboBox comboBoxPN = action.PartNumber.control as ComboBox;
                if ((comboBoxId != null) && (comboBoxId.SelectedItem != null) && (comboBoxPN != null) && (treatmentPackages != null))
                {
                    action.PartNumber.removeItems();
                    Farm farm = comboBoxId.SelectedItem as Farm;
                    Service service = comboBoxId.SelectedItem as Service;
                    Entity entity = null;
                    if (farm != null)
                        entity = farm;
                    if (service != null)
                        entity = service;
                    if (entity != null)
                    {
                        action.PartNumber.items = treatmentPackages.Where(t => t.contract_type == entity.contract_type).ToArray();
                        //action.PartNumber.addItems(treatmentPackages.Where(t => t.contract_type == entity.contract_type).ToArray());
                        action.PartNumber.addItems(action.PartNumber.items);
                    }
                    comboBoxPN.Text = action.PartNumber.dflt;
                }
            }
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if ((action != null) && (action.RadioFarm != null) && (action.RadioService != null))
            {
                RadioButton radioFarm = action.RadioFarm.lcontrol as RadioButton;
                RadioButton radioService = action.RadioService.lcontrol as RadioButton;
                if ((radioButton != null) && (radioFarm != null) && (radioService != null)
                     && (action.Farm != null) && (action.Farm.control != null)
                     && (action.Service != null) && (action.Service.control != null)
                     && (action.PartNumber != null) && (action.PartNumber.control != null))
                {
                    if (radioButton.Checked)
                    {
                        if (radioButton == radioFarm)
                        {
                            radioService.Checked = false;
                            action.Farm.control.Visible = true;
                            action.Farm.view = true;

                            action.Service.control.Visible = false;
                            action.Service.control.Text = null;
                            action.Service.view = false;
                        }
                        if (radioButton == radioService)
                        {
                            radioFarm.Checked = false;
                            action.Farm.control.Visible = false;
                            action.Farm.control.Text = null;
                            action.Farm.view = false;

                            action.Service.control.Visible = true;
                            action.Service.view = true;
                        }
                    }
                    else
                    {
                        action.PartNumber.removeItems();
                        action.PartNumber.control.Text = action.PartNumber.dflt;
                    }
                }
            }
        }

        private void progressBar_Callback(object sender, EventArgs e)
        {
            ProgressBar progressBar;
            if ((action != null) && (action.Progress != null))
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
            //FormNotify formNotify = new FormNotify(new List<string>() {
            //    "Are you sure you want to cancel the operation?"},
            //    NotifyButtons.YesNo, caption: "Abort");
            //formNotify.ShowDialog();
            //if (formNotify.DialogResult == DialogResult.Yes)
            
            DialogResult dialogResult = notify(new List<string>() { "Are you sure you want to cancel the operation?"}, NotifyButtons.YesNo, caption: "Abort");
            if (dialogResult == DialogResult.Yes)
            {
                if(action != null)
                    action.hide();
                clearAM();
                screenActionShow();
            }
            //formNotify.Dispose();
        }

        private async void buttonTreatApprove_Click(object sender, EventArgs e)
        {
            if ((action != null) && (am != null) && (settings != null))
            {
                ErrCode errcode = ErrCode.ERROR;
                JsonDocument jsonDocument = null;
                List<string> errors = new List<string>();
                List<string> messages = new List<string>();
                uint maxi = am.Maxi;

                action.disableControls();

                ProgressBar progressBar = action.Progress.lcontrol as ProgressBar;

                action.updateParams();

                if ((errcode = action.checkParams()) == ErrCode.OK)
                {
                    Farm farm = farms.Find(f => f.id == action.farm);
                    Service service = services.Find(s => s.id == action.service_provider);
                    TreatmentPackage treatmentPackage = treatmentPackages.Find(t => t.part_number == action.part_number);
                    if (((farm != null) || (service != null)) && (treatmentPackage != null))
                    {
                        am.MaxiSet = (uint)(treatmentPackage.amount_of_treatments * settings.number_of_pulses_per_treatment);
                        //uint maxi = am.Maxi;
                        if ((am.Maxi + am.MaxiSet) < settings.max_am_pulses)
                        {
                            //FormNotify formNotify = new FormNotify(new List<string>() {
                            //        string.Format("{0} treatments will be added",am.MaxiSet / settings.number_of_pulses_per_treatment),
                            //        string.Format("to the AM - SN {0}", am.SNum),
                            //        (farm != null) ? string.Format("Farm {0}", farm.Name.val as string) :
                            //        ((service != null) ? string.Format("Service Provider {0}", service.Name.val as string) : string.Empty),
                            //        "Press the button to proceed"}, NotifyButtons.YesNo, caption: "Approve");
                            //formNotify.ShowDialog();

                            DialogResult dialogResult = notify(new List<string>() {
                                    string.Format("{0} treatments will be added",am.MaxiSet / settings.number_of_pulses_per_treatment),
                                    string.Format("to the AM - SN {0}", am.SNum),
                                    (farm != null) ? string.Format("Farm {0}", farm.Name.val as string) :
                                    ((service != null) ? string.Format("Service Provider {0}", service.Name.val as string) : string.Empty),
                                    "Press the button to proceed"}, NotifyButtons.YesNo, caption: "Approve");
                            if (dialogResult == DialogResult.Yes)
                            {
                                //formNotify.Dispose();

                                if (progressBar != null)
                                    progressBar.Visible = true;
                                am.serialPortProgressEvent += new EventHandler(progressBar_Callback);
                                if (progressBar != null)
                                    progressBar.Value = progressBar.Minimum;

                                if ((errcode = await am.AMDataWrite()) == ErrCode.OK)
                                {
                                    if ((errcode = await am.AMDataRead()) == ErrCode.OK)
                                    {
                                        jsonDocument = await web.entityAdd<ActionJson>(action, "api/p/actions/");
                                        if (jsonDocument != null)
                                        {
                                            Action raction = null;
                                            try { raction = JsonSerializer.Deserialize<Action>(jsonDocument.RootElement.ToString()); }
                                            catch (Exception ex) { LogFile.logWrite(ex.ToString()); }
                                            if (raction != null)
                                            {
                                                //notify(new List<string>() {
                                                //    string.Format("The original amount of treatments: {0}", maxi / settings.number_of_pulses_per_treatment),
                                                //    string.Format("Added treatments: {0}",am.MaxiSet / settings.number_of_pulses_per_treatment),
                                                //    string.Format("The treatments available on AM - SN {1}: {0}", am.Maxi / settings.number_of_pulses_per_treatment, am.SNum),
                                                //    "please disconnect the AM"},
                                                //    NotifyButtons.OK, "Approve Success");
                                                //action.hide();
                                                //clearAM();
                                                //screenActionShow();
                                                errcode = ErrCode.OK;
                                            }
                                            else
                                            {
                                                //
                                                // erase am needed
                                                //
                                                //responseParse(action, jsonDocument, errors, messages);
                                                //notify(errors, NotifyButtons.OK, "Approve Fail");
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
                                errcode = ErrCode.CANSEL;
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
                    "and the number of treatments" }, "Approve Fail");
                else if (errcode == ErrCode.MAX)
                    screenTreatError(new List<string>() {
                    "Wrong part number,",
                    "the maximum number of treatments reached,",
                    "please choose a smaller number of treatments" }, "Approve Fail");
                else if (errcode == ErrCode.ERASE)
                {
                    if (jsonDocument != null)
                    {
                        responseParse(action, jsonDocument, errors, messages);
                        notify(errors, NotifyButtons.OK, "Approve Fail");
                    }
                    notify(new List<string>() { "Restoring AM" }, NotifyButtons.OK, "Approve Fail");

                    am.MaxiSet = 0;
                    if (progressBar != null)
                        progressBar.Value = progressBar.Minimum;

                    if (await am.AMDataWrite() == ErrCode.OK)
                    {
                        if (await am.AMDataRead() == ErrCode.OK)
                            screenTreatError(new List<string>() { "Approve failed,", "AM sucsessfully restored to original values" }, "Approve Fail");
                        else
                            screenTreatError(new List<string>() { "Approve failed,", "Faild to restore to original values" }, "Approve Fail");
                    }
                    else
                        screenTreatError(new List<string>() { "Approve failed,", "Faild to restore to original values" }, "Approve Fail");
                }
                //else if (errcode == ErrCode.EFAIL)
                //    screenTreatError(new List<string>() { "Failed to restore AM" }, "Approve Fail");
                else if (errcode == ErrCode.ERROR)
                    screenTreatError(new List<string>() { "The operation failed, the treatments were not added" }, "Approve Fail");
                else if (errcode == ErrCode.CANSEL)
                    action.enableControls();
                else if (errcode == ErrCode.OK)
                {
                    notify(new List<string>() {
                        string.Format("The original amount of treatments: {0}", maxi / settings.number_of_pulses_per_treatment),
                        string.Format("Added treatments: {0}",am.MaxiSet / settings.number_of_pulses_per_treatment),
                        string.Format("The treatments available on AM - SN {1}: {0}", am.Maxi / settings.number_of_pulses_per_treatment, am.SNum),
                        "please disconnect the AM"},
                        NotifyButtons.OK, "Approve Success");
                    action.hide();
                    clearAM();
                    screenActionShow();
                }
            }
        }

        private void screenTreatError(List<string> text, string caption)
        {
            if ((action != null) && (action.Progress != null))
            {
                ProgressBar progressBar = action.Progress.lcontrol as ProgressBar;
                if (progressBar != null)
                    progressBar.Visible = false;
                screenError(text, caption);
            }
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
            if (dataGridView1 != null)
                dataGridView1.DataSource = entityTableGet(farms.Cast<Entity>().ToList());
        }

        private void screenServiceShow()
        {
            screenDataGridShow("Manage Service providers",
                new EventHandler(buttonServiceEdit_Click),
                new EventHandler(buttonServiceAdd_Click),
                new EventHandler(richTextBoxServiceSearch_TextChanged),
                new EventHandler(buttonBackToAction_Click));
            if (dataGridView1 != null)
                dataGridView1.DataSource = entityTableGet(services.Cast<Entity>().ToList());
        }

        private void richTextBoxFarmSearch_TextChanged(object sender, EventArgs e)
        {
            if ((dataGridView1 != null) && (farms != null) && (sender != null))
            {
                DataTable table = null;
                if ((table = richTextBoxSearch(sender, farms.Cast<Entity>())) != null)
                    dataGridView1.DataSource = table;
            }
        }

        private void richTextBoxServiceSearch_TextChanged(object sender, EventArgs e)
        {
            if ((dataGridView1 != null) && (services != null) && (sender != null))
            {
                DataTable table = null;
                if ((table = richTextBoxSearch(sender, services.Cast<Entity>())) != null)
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
                    table = entityTableGet(entities.Where(e => (e.Name.val as string != null) && ((e.Name.val as string).ToLower().Contains(richTextBox.Text.ToLower()))).ToList());
                    dataGridView1.DataSource = table;
                }
            }
            return table;
        }

        private void screenDataGridShow(string dataName, EventHandler eventHandlerButton1, EventHandler eventHandlerButton2, EventHandler eventHandlerButton3, EventHandler eventHandlerButton4)
        {
            new Field(ltype: typeof(PictureBox), lplacev: Place.One).draw(this, true);
            new Field(ltype: typeof(Label), ltext: dataName, lplacev: Place.Two).draw(this, true);
            new Field(ltype: typeof(Button), ltext: "Back", buttonEventHandler: eventHandlerButton4, lplaceh: Place.Four, lplacev: Place.Three).draw(this, true);
            search = new Field(type: typeof(RichTextBox), dflt: "Search", textEventHandler: eventHandlerButton3, placeh: Place.Six, placev: Place.Three);
            search.draw(this, false);
            new Field(ltype: typeof(Button), ltext: "Edit", buttonEventHandler: eventHandlerButton1, lplaceh: Place.One, lplacev: Place.Three).draw(this, true);
            new Field(ltype: typeof(Button), ltext: "Add New", buttonEventHandler: eventHandlerButton2, lplaceh: Place.Three, lplacev: Place.Three).draw(this, true);
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
            farm = new Farm(false, comboBoxCountry_SelectedIndexChanged, buttonFarmCancel_Click, buttonFarmAddSubmit_Click);
            if (farm != null)
                farm.drawFields(this);
        }

        private void buttonServiceAdd_Click(object sender, EventArgs e)
        {
            hide();
            service = new Service(false, comboBoxCountry_SelectedIndexChanged, buttonServiceCancel_Click, buttonServiceAddSubmit_Click);
            if (service != null)
                service.drawFields(this);
        }

        private void buttonFarmEdit_Click(object sender, EventArgs e)
        {
            if (farms != null)
            {
                farm = getCurrentEntity(farms.Cast<Entity>()) as Farm;
                if (farm != null)
                {
                    hide();
                    farm.initFields(true, comboBoxCountry_SelectedIndexChanged, buttonFarmCancel_Click, buttonFarmEditSubmit_Click);
                    farm.drawFields(this);
                }
            }
        }

        private Entity getCurrentEntity(IEnumerable<Entity> entities)
        {
            if ((entities != null) && (dataGridView1 != null) && (dataGridView1.CurrentRow != null) && (dataGridView1.CurrentRow.Cells != null)
                && (dataGridView1.CurrentRow.Cells.Count > 0))
                return entities.ToList().Find(e => ((e.Name.val as string != null) && (e.Name.val as string == dataGridView1.CurrentRow.Cells[0].Value as string)));
            return null;
        }

        private void buttonServiceEdit_Click(object sender, EventArgs e)
        {
            if (services != null)
            {
                service = getCurrentEntity(services.Cast<Entity>()) as Service;
                if (service != null)
                {
                    hide();
                    service.initFields(true, comboBoxCountry_SelectedIndexChanged, buttonServiceCancel_Click, buttonServiceEditSubmit_Click);
                    service.drawFields(this);
                }
            }
        }

        private void comboBoxCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                Entity entity = null;
                if ((farm != null) && (farm.Country != null))
                {
                    if (comboBox == farm.Country.control)
                        entity = farm;
                }
                if ((service != null) && (service.Country != null))
                {
                    if (comboBox == service.Country.control)
                        entity = service;
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
            if (farm != null)
            {
                farm.hide();
                screenFarmShow();
            }
        }

        private void buttonServiceCancel_Click(object sender, EventArgs e)
        {
            if (service != null)
            {
                service.hide();
                screenServiceShow();
            }
        }

        private async void buttonFarmAddSubmit_Click(object sender, EventArgs e)
        {
            if (farm != null)
            {
                ErrCode errcode = ErrCode.ERROR;
                JsonDocument jsonDocument = null;
                List<string> errors = new List<string>();
                List<string> messages = new List<string>();

                farm.disableControls();

                farm.updateParams();

                if ((errcode = farm.checkParams()) == ErrCode.OK)
                {
                    jsonDocument = await web.entityAdd<FarmJson>(farm, "api/p/farms/");
                    if (jsonDocument != null)
                    {
                        Farm rfarm = null;
                        try { rfarm = JsonSerializer.Deserialize<Farm>(jsonDocument.RootElement.ToString()); }
                        catch (Exception ex) { LogFile.logWrite(ex.ToString()); }
                        if (rfarm != null)
                        {
                            farms.Add(rfarm);
                            errcode = ErrCode.OK;
                        }
                    }
                }

                if (jsonDocument != null)
                    responseParse(farm, jsonDocument, errors, messages);
                responseShow(errcode, errors);
                if (errcode == ErrCode.OK)
                {
                    farm.hide();
                    screenFarmShow();
                }
                else
                    farm.enableControls();
            }
        }

        private void responseShow(ErrCode errcode, List<string> errors)
        {
            if (errcode == ErrCode.EPARAM)
            {
                if (errors.Count > 0)
                    notify(errors, NotifyButtons.OK, "Submit Failed");
                else
                    notify(new List<string>() { "Submit failed, can't add empty or negative fields,",
                    "make sure all the fields are filled with valid values"}, NotifyButtons.OK, "Submit Failed");
            }
        }

        private async void buttonServiceAddSubmit_Click(object sender, EventArgs e)
        {
            if (service != null)
            {
                ErrCode errcode = ErrCode.ERROR;
                JsonDocument jsonDocument = null;
                List<string> errors = new List<string>();
                List<string> messages = new List<string>();

                service.disableControls();

                service.updateParams();
                if ((errcode = service.checkParams()) == ErrCode.OK)
                {
                    jsonDocument = await web.entityAdd<ServiceJson>(service, "api/p/service_providers/");
                    if (jsonDocument != null)
                    {
                        Service rservice = null;
                        try { rservice = JsonSerializer.Deserialize<Service>(jsonDocument.RootElement.ToString()); }
                        catch (Exception ex) { LogFile.logWrite(ex.ToString()); }
                        if (rservice != null)
                        {
                            services.Add(rservice);
                            errcode = ErrCode.OK;
                        }
                    }
                }

                if (jsonDocument != null)
                    responseParse(service, jsonDocument, errors, messages);
                responseShow(errcode, errors);
                if (errcode == ErrCode.OK)
                {
                    service.hide();
                    screenServiceShow();
                }
                else
                    service.enableControls();
            }
        }

        private ErrCode responseParse<T>(T entity, JsonDocument jsonDocument, List<string> errors, List<string> messages)
        {
            ErrCode errcode = ErrCode.ERROR;
            Gui gui = entity as Gui;

            if (gui != null)
                errcode = gui.responseParse(jsonDocument, errors, messages);

            return errcode;
        }

        private async void buttonFarmEditSubmit_Click(object sender, EventArgs e)
        {
            if (farm != null)
            {
                ErrCode errcode = ErrCode.ERROR;
                JsonDocument jsonDocument = null;
                List<string> errors = new List<string>();
                List<string> messages = new List<string>();

                farm.disableControls();

                Farm ufarm = farm.clone() as Farm;
                ufarm.updateParams();
                if ((errcode = ufarm.checkParams()) == ErrCode.OK)
                {
                    jsonDocument = await web.entityEdit<FarmJson>(ufarm, "api/p/farms/" + farm.Id + "/");
                    if (jsonDocument != null)
                    {
                        Farm rfarm = null;
                        try { rfarm = JsonSerializer.Deserialize<Farm>(jsonDocument.RootElement.ToString()); }
                        catch (Exception ex) { LogFile.logWrite(ex.ToString()); }
                        if (rfarm != null)
                        {
                            farms.Insert(farms.IndexOf(farm), rfarm);
                            farms.Remove(farm);
                            errcode = ErrCode.OK;
                        }
                    }
                }

                if (jsonDocument != null)
                    responseParse(farm, jsonDocument, errors, messages);
                responseShow(errcode, errors);
                if (errcode == ErrCode.OK)
                {
                    farm.hide();
                    screenFarmShow();
                }
                else
                    farm.enableControls();
            }
        }

        private async void buttonServiceEditSubmit_Click(object sender, EventArgs e)
        {
            if (service != null)
            {
                ErrCode errcode = ErrCode.ERROR;
                JsonDocument jsonDocument = null;
                List<string> errors = new List<string>();
                List<string> messages = new List<string>();

                service.disableControls();

                Service uservice = service.clone() as Service;
                uservice.updateParams();
                if ((errcode = uservice.checkParams()) == ErrCode.OK)
                {
                    jsonDocument = await web.entityEdit<ServiceJson>(uservice, "api/p/service_providers/" + service.Id + "/");
                    if (jsonDocument != null)
                    {
                        Service rservice = null;
                        try { rservice = JsonSerializer.Deserialize<Service>(jsonDocument.RootElement.ToString()); }
                        catch (Exception ex) { LogFile.logWrite(ex.ToString()); }
                        if (rservice != null)
                        {
                            services.Insert(services.IndexOf(service), rservice);
                            services.Remove(service);
                            errcode = ErrCode.OK;
                        }
                    }
                }

                if (jsonDocument != null)
                    responseParse(service, jsonDocument, errors, messages);
                responseShow(errcode, errors);
                if (errcode == ErrCode.OK)
                {
                    service.hide();
                    screenServiceShow();
                }
                else
                    service.enableControls();
            }
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
