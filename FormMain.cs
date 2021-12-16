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
        private string tabletNo = null;
        private Field search;

        private Data data;

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
        {
            DialogResult dialogResult = default;
            if ((title != null) && (messages != null) && (cancel != null))
            {
                FormNotify formNotify = new FormNotify(new List<string>() { messages}, NotifyButtons.OK, title);
                formNotify.ShowDialog();
                dialogResult = formNotify.DialogResult;
                formNotify.Dispose();
            }
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
            data = new Data(progressBar_Callback);
            if (data != null)
            {
                data.web = new Web();
                data.am = new Am(progressBar_Callback);
                data.login = new Login(forgot_Click, buttonLogin_Click, checkBox_CheckedChanged, logout, hide, enabled, notify, draw, dshow: screenActionShow) { tablet = TabletNo };
                data.password = new Password(buttonChangePassword_Click, logout, hide, enabled, notify, draw);
                if (data.login != null)
                    data.login.ddraw(data.login);
            }
        }

        private void forgot_Click(object sender, EventArgs e)
        {
            if (data.login != null)
                hide();
            data.reset = new Reset(buttonResetPassword_Click, logout, hide, enabled, notify, draw);
            if (data.reset != null)
                data.reset.ddraw(data.reset);
        }

        private async void buttonResetPassword_Click(object sender, EventArgs e)
        {
            if ((data != null) && (data.reset != null))
            {
                data.login = new Login(forgot_Click, buttonLogin_Click, checkBox_CheckedChanged, logout, hide, enabled, notify, draw, dshow: screenActionShow) { tablet = TabletNo };
                await data.reset.send(data);
            }
        }

        private void clear()
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

            data.login = new Login(forgot_Click, buttonLogin_Click, checkBox_CheckedChanged, logout, hide, enabled, notify, draw, dshow: screenActionShow) { tablet = TabletNo };
            data.user = null;
        }

        private void clearAM()
        {
            AMConnected = false;
            data.am = new Am(progressBar_Callback);
        }

        private async void buttonLogin_Click(object sender, EventArgs e)
        {
            if ((data != null) && (data.login != null))
            {
                //data.login.Email.control.Text = "avner.hilu@outlook.com";
                //data.login.Password.control.Text = "Plk90!@34";
                //data.login.Email.control.Text = "yael@gmail.com";
                //data.login.Password.control.Text = "yael1234";
                //data.login.tablet = "PF1C9VKU";
                //data.login.Email.control.Text = "mikiy@armentavet.com";
                //data.login.Password.control.Text = "miki1973";
                //data.login.tablet = "VAKD9Z0199D2";
                //        //login.email = "yaelv@armentavet.com";
                //        //login.password = "Yyyaeeel123";
                //        //login.tablet = "kjh1g234123";
                await data.login.send(data);
            }
        }

        private async void buttonChangePassword_Click(object sender, EventArgs e)
        {
            if ((data != null) && (data.password != null))
            {
                await data.password.send(data);
            }
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

        private void buttonLogout_Click(object sender, EventArgs e)
        {
            logout();
        }

        private void logout()
        {
            clear();
            hide();
            if ((data != null) && (data.login != null))
                data.login.ddraw(data.login);
        }

        private void buttonUpdateAM_Click(object sender, EventArgs e)
        {
            enabled(false);
            hide();
            screenConnectShow();
        }

        private async void buttonFarm_Click(object sender, EventArgs e)
        {
            enabled(false);
            hide();
            screenFarmShow();
        }

        private async void buttonService_Click(object sender, EventArgs e)
        {
            enabled(false);
            hide();
            screenServiceShow();
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
                progressBar1.Visible = true;
                progressBar1.Minimum = 0;
                progressBar1.Value = progressBar1.Minimum;
                progressBar1.Maximum = 27;
                data.am.progress = progressBar1;

                ErrCode errcode = ErrCode.ERROR;

                if ((errcode = await data.am.AMCheckConnect()) == ErrCode.OK)
                    errcode = await data.am.AMCmd(Cmd.READ);

                progressBar1.Value = progressBar1.Maximum;

                if (errcode == ErrCode.EEMPTY)
                {
                    if (await notify("AM content corrupted", "AM content corrupted, \ndo you want to restore AM \nto the last saved values?", "Yes", "No"))
                    {
                        progressBar1.Minimum = 0;
                        progressBar1.Value = progressBar1.Minimum;
                        progressBar1.Maximum = 16;
                        errcode = await data.am.AMCmd(Cmd.RESTORE);
                        progressBar1.Value = progressBar1.Maximum;
                    }
                }
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

                    await notify("AM not connected", "AM not found make sure the AM is connected\nto the tablet by using a USB cable", "OK");
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
        }

        private void AMConnectedShow()
        {
            if (label1 != null)
                label1 = new Field(ltype: typeof(Label), dflt: "AM found – connected to AM", color: Color.Green,
                    placeh: Place.Center, placev: Place.Six).draw(this, true) as Label;
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
            new Field(ltype: typeof(Label), ltext: "Current available treatments: " + data.Current, lplacev: Place.Six).draw(this, true);
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
            data.action = new Action(data.am, TabletNo, data.farms.ToArray(), data.services.ToArray(),
                comboBoxPartNumber_SelectedIndexChanged, comboBoxFarm_SelectedIndexChanged, radioButton_CheckedChanged,
                buttonTreatCansel_Click, buttonTreatApprove_Click, logout, hide, enabled, notify, draw, dshow: screenActionShow, dnotifyAnswer: notify);
            if ((data != null) && (data.action != null) && (data.action.RadioFarm != null) && (data.action.Progress != null))
            {
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
                if ((treatmentPackage != null) && (data != null) && (data.settings != null))
                {
                    data.action.PartNumber.val = treatmentPackage.PartNumber;
                    data.am.MaxiSet = (uint)(treatmentPackage.amount_of_treatments * data.settings.number_of_pulses_per_treatment);
                }
            }
        }

        private async void comboBoxFarm_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
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
                    }
                    if (service != null)
                    {
                        entity = service;
                        data.action.Service.val = service.Id;
                    }
                    if (entity != null)
                    {
                        if ((data.treatmentPackages != null) && (data.settings != null) && (data.am != null))
                        {
                            if ((data.action.PartNumber.items = data.treatmentPackages.Where(t => (t.contract_type == entity.contract_type) &&
                                 ((t.amount_of_treatments * data.settings.number_of_pulses_per_treatment + data.am.Maxi) < data.settings.max_am_pulses)).ToArray()) != null)
                            {
                                data.action.PartNumber.addItems(data.action.PartNumber.items);
                                data.action.PartNumber.control.Text = data.action.PartNumber.dflt;
                                data.action.PartNumber.control.ForeColor = Color.Silver;
                                data.action.PartNumber.val = data.action.PartNumber.dflt;
                                if (data.action.PartNumber.items.Length == 0)
                                { 
                                    await notify("Part Number Error", "The attached AM reached the max allowed treatments.\n" +
                                       "There are no available part numbers.\n" +
                                       "Please replace the AM or contact support.", "OK");
                                }
                            }
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

        private void progressBar_Callback(Object progress, bool reset)
        {
            ProgressBar progressBar = progress as ProgressBar;
            //if ((data != null) && (data.action != null) && (data.action.Progress != null))
            //    progressBar = data.action.Progress.lcontrol as ProgressBar;
            //else
            //    progressBar = progressBar1;

            if (progressBar != null)
            {
                if (reset)
                    progressBar.Value = progressBar.Minimum;
                else if ((progressBar.Value + 1) <= progressBar.Maximum)
                    progressBar.Value++;
            }
        }

        private async void buttonTreatCansel_Click(object sender, EventArgs e)
        {
            if ((data != null) && (data.action != null))
            {
                bool answer = await notify("Abort", "Are you sure you want to cancel the operation?", "Yes", "No");
                if (answer)
                {
                    data.action.dhide();
                    screenActionShow();
                }
            }
        }

        private async void buttonTreatApprove_Click(object sender, EventArgs e)
        {
            if ((data != null) && (data.action != null))
            {
                ErrCode errcode = ErrCode.ERROR;
                ProgressBar progressBar = data.action.Progress.lcontrol as ProgressBar;

                if (progressBar != null)
                {
                    progressBar.Visible = true;
                    progressBar.Minimum = 0;
                    progressBar.Value = progressBar.Minimum;
                    progressBar.Maximum = 86;
                    data.am.progress = progressBar;

                    errcode = await data.action.send(data);

                    if (errcode == ErrCode.OK)
                        progressBar.Value = progressBar.Maximum;
                    else
                        progressBar.Visible = false;
                }
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
            if (data != null)
            {
                data.farm = new Farm(false, comboBoxCountry_SelectedIndexChanged, buttonFarmCancel_Click, buttonFarmAddSubmit_Click,
                    logout, hide, enabled, notify, draw, dshow: screenFarmShow);
                if (data.farm != null)
                    data.farm.ddraw(data.farm);
            }
        }

        private void buttonServiceAdd_Click(object sender, EventArgs e)
        {
            hide();
            if (data != null)
            {
                data.service = new Service(false, comboBoxCountry_SelectedIndexChanged, buttonServiceCancel_Click, buttonServiceAddSubmit_Click,
                logout, hide, enabled, notify, draw, dshow: screenServiceShow);
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
                    data.farm.initFields(true, comboBoxCountry_SelectedIndexChanged, buttonFarmCancel_Click, buttonFarmEditSubmit_Click,
                        logout, hide, enabled, notify, draw, dshow: screenFarmShow);
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
                    data.service.initFields(true, comboBoxCountry_SelectedIndexChanged, buttonServiceCancel_Click, buttonServiceEditSubmit_Click,
                        logout, hide, enabled, notify, draw, dshow: screenServiceShow);
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
        }

        private async void buttonServiceAddSubmit_Click(object sender, EventArgs e)
        {
            if ((data != null) && (data.service != null) && (data.web != null))
            {
                await data.service.send(data, false);
            }
        }

        private async void buttonFarmEditSubmit_Click(object sender, EventArgs e)
        {
            if ((data != null) && (data.farm != null) && (data.web != null))
            {
                await data.farm.send(data, true);
            }
        }
        
        private async void buttonServiceEditSubmit_Click(object sender, EventArgs e)
        {
            if ((data != null) && (data.service != null) && (data.web != null))
            {
                await data.service.send(data, true);
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

        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                if ((data != null) && (data.login != null) && (data.login.Password != null) && (data.login.Password.control != null))
                {
                    TextBox textBox = data.login.Password.control as TextBox;
                    if (textBox != null)
                    {
                        if (checkBox.Checked)
                            data.login.Password.pswdchar = '\0';
                        else
                            data.login.Password.pswdchar = '*';
                        if (textBox.Text != data.login.Password.dflt)
                            textBox.PasswordChar = data.login.Password.pswdchar;
                    }
                }
            }
        }
    }
}
