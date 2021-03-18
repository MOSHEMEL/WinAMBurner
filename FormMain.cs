using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace WinAMBurner
{
    public partial class FormMain : Form
    {
        // login screen
        private const string loginForgotStr="Forgot password";
        private const string notifyLoginCapStr = "Login Failed";
        private const string notifyLoginTextStr = "Login failed Check your username and password, \nmake sure your tablet is connected to the internet";
        // connect screen
        private const string AMCheckWellcomDistStr= "Welcome distributor";
        private const string AMCheckNotifyStr = "Please make sure the AM is connected to your tablet before continue";
        private const string AMCheckFaultStr = "AM not found – make sure AM is connected using USB cable";
        private const string AMCheckOKStr = "AM found – connected to AM";
        private const string AMCheckStr = "Check AM present";
        private const string AMCheckContinueStr = "Continue";
        private const string notifyAMCheckCapStr = "Am not connected";
        private const string notifyAMCheckTextStr = "AM not found make sure the AM is connected \nto the tablet by using a USB cable";
        // info screen
        private const string AMInfoPulsNumStr = "AM information, one treatment in pulses : ";
        private const string AMInfoSNumStr = "AM identified with SN : ";
        private const string AMInfoTreatNumStr = "Current available treatments : ";
        private const string AMInfoBackStr = "Back";
        private const string AMInfoContinueStr = "Continue";
        // add screen
        private const string treatAddStr = "Add treatments to AM – SN ";
        private const string treatAddCancelStr = "Cancel";
        private const string treatAddApproveStr = "Approve";
        private const string notifyTreatAddCancelCapStr = "Abort";
        private const string notifyTreatAddCancelTextStr = "Are you sure you want to cancel the operation?";
        private const string notifyTreatAddOKCapStr = "Success";
        private const string notifyTreatAddOKTextStr = "{0} treatments updated, \n{1} treatments available on AM - SN {2}, \nplease disconnect the AM";
        private const string notifyTreatAddFaultCapStr = "Fail";
        private const string notifyTreatAddFaultTextStr = "The operation failed, the treatments were not added";
        private const string notifyTreatAddWrongParameterTextStr = "Wrong parameters, \nplease choose the number of treatments";

        private EventHandler buttonAMCheck_EventHandler;
        private EventHandler buttonAMCheckContinue_EventHandler;
        private EventHandler buttonAMInfoBack_EventHandler;
        private EventHandler buttonAMInfoContinue_EventHandler;
        private EventHandler buttonTreatAddCancel_EventHandler;
        private EventHandler buttonTreatAddApprove_EventHandler;

        private AMData amData = new AMData();

        //private string AMCheckResultStr = "";
        //private Color AMCheckResultCol = Color.Black;
        private bool AMConnected = false;
        private bool AMCheckShowFirst = true;

        public FormMain()
        {
            InitializeComponent();
            buttonAMCheck_EventHandler = new EventHandler(buttonAMCheck_Click);
            buttonAMCheckContinue_EventHandler = new EventHandler(buttonAMCheckContinue_Click);
            buttonAMInfoBack_EventHandler = new EventHandler(buttonAMInfoBack_Click);
            buttonAMInfoContinue_EventHandler = new EventHandler(buttonAMInfoContinue_Click);
            buttonTreatAddCancel_EventHandler = new EventHandler(buttonTreatAddCancel_Click);
            buttonTreatAddApprove_EventHandler = new EventHandler(buttonTreatAddApprove_Click);
        }

        private void loginHide()
        {
            richTextBoxUsername.Visible = false;
            richTextBoxPassword.Visible = false;
            //label3.Text = "";
            //label3.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            //label3.ForeColor = Color.Black;
            labelLoginForgot.Visible = false;
            buttonLogin.Visible = false;
        }
        private void loginShow()
        {
            richTextBoxUsername.Visible = true;
            richTextBoxPassword.Visible = true;
            //label3.Text = loginForgotStr;
            //label3.Font = new Font("Segoe UI", 9F, FontStyle.Underline, GraphicsUnit.Point);
            //label3.ForeColor = Color.Blue;
            labelLoginForgot.Visible = true;
            buttonLogin.Visible = true;
        }
        private void AMCheckShow()
        {
            labelAMCheckWellcom.Text = AMCheckWellcomDistStr;
            labelAMCheckWellcom.Visible = true;
            label1.Text = AMCheckNotifyStr;
            label1.Visible = true;
            //label2.Text = AMCheckResultStr;
            //label2.ForeColor= AMCheckResultCol;
            //label2.Visible = true;
            buttonBack.Text = AMCheckStr;
            buttonBack.Click += buttonAMCheck_EventHandler;
            buttonBack.Visible = true;
            buttonForward.Text = AMCheckContinueStr;
            buttonForward.Click += buttonAMCheckContinue_EventHandler;
            buttonForward.Visible = true;
            if (AMConnected)
                AMConnectedShow();
            else
                AMDisconnectedShow();
            if (AMCheckShowFirst)
            {
                labelAMCheckSts.Visible = false;
                AMCheckShowFirst = false;
            }
        }

        private void AMCheckHide()
        {
            labelAMCheckWellcom.Visible = false;
            label1.Visible = false;
            labelAMCheckSts.Visible = false;
            buttonBack.Visible = false;
            buttonBack.Click -= buttonAMCheck_EventHandler;
            buttonForward.Visible = false;
            buttonForward.Click -= buttonAMCheckContinue_EventHandler;
        }
        private void AMInfoShow()
        {
            label1.Text = AMInfoPulsNumStr + 1;
            label1.Visible = true;
            labelAMInfoSNum.Text = AMInfoSNumStr + amData.SNum;
            //label2.ForeColor = Color.Black;
            labelAMInfoSNum.Visible = true;
            labelAMInfoTreatNum.Text = AMInfoTreatNumStr + amData.Maxi;
            labelAMInfoTreatNum.Visible = true;
            buttonBack.Text = AMInfoBackStr;
            buttonBack.Click += buttonAMInfoBack_EventHandler;
            buttonBack.Visible = true;
            buttonForward.Text = AMInfoContinueStr;
            buttonForward.Click += buttonAMInfoContinue_EventHandler;
            buttonForward.Visible = true;
        }
        private void AMInfoHide()
        {
            label1.Visible = false;
            labelAMInfoSNum.Visible = false;
            labelAMInfoTreatNum.Visible = false;
            buttonBack.Click -= buttonAMInfoBack_EventHandler;
            buttonBack.Visible = false;
            buttonForward.Click -= buttonAMInfoContinue_EventHandler;
            buttonForward.Visible = false;
        }
        private void treatAddShow()
        {
            label1.Text = treatAddStr + amData.SNum;
            label1.Visible = true;
            comboBoxTreat.Visible = true;
            buttonBack.Text = treatAddCancelStr;
            buttonBack.Click += buttonTreatAddCancel_EventHandler;
            buttonBack.Visible = true;
            buttonForward.Text = treatAddApproveStr;
            buttonForward.Click += buttonTreatAddApprove_EventHandler;
            buttonForward.Visible = true;
        }
        private void treatAddHide()
        {
            label1.Visible = false;
            comboBoxTreat.Visible = false;
            buttonBack.Click -= buttonTreatAddCancel_EventHandler;
            buttonBack.Visible = false;
            buttonForward.Click -= buttonTreatAddApprove_EventHandler;
            buttonForward.Visible = false;
        }
        private void formClear()
        {
            //AMCheckResultStr = "";
            //AMCheckResultCol = Color.Black;
            AMConnected = false;
            AMCheckShowFirst = true;
            amData.SNum = 0;
            amData.Maxi = 0;
            amData.MaxiSet = 0;
            comboBoxTreat.ResetText();
        }

        private void progressBarAMCheckProgress_Callback(object sender, EventArgs e)
        {
            SerialPortEventArgs args = e as SerialPortEventArgs;
            progressBarAMCheck.Maximum = args.maximum;
            progressBarAMCheck.Value = args.progress;
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            comboBoxTreat.Items.AddRange(new string[] { "50", "100", "150", "200", "250", "300" });
            //comboBoxTreat.Text = comboBoxTreat.Items[0].ToString();
            amData.SNum = 0x123;
            // if failed
            FormNotify formNotify = new FormNotify(notifyLoginCapStr, notifyLoginTextStr, NotifyButtons.OK);
            formNotify.ShowDialog();
            formNotify.Dispose();
            // if ok
            loginHide();
            AMCheckShow();
        }

        private async void buttonAMCheck_Click(object sender, EventArgs e)
        {
            buttonBack.Enabled = false;
            buttonForward.Enabled = false;
            progressBarAMCheck.Visible = true;
            amData.serialPortProgressEvent += new EventHandler(progressBarAMCheckProgress_Callback);
            int errcode = await amData.AMDataRead();
            progressBarAMCheck.Value = progressBarAMCheck.Minimum;
            progressBarAMCheck.Visible = false;
            if (errcode == 0)
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
                FormNotify formNotify = new FormNotify(notifyAMCheckCapStr, notifyAMCheckTextStr, NotifyButtons.OK);
                formNotify.ShowDialog();
                formNotify.Dispose();
            }
            buttonBack.Enabled = true;
        }

        private void AMDisconnectedShow()
        {
            //AMConnected = false;
            //AMCheckResultCol = Color.Red;
            //AMCheckResultStr = AMCheckFaultStr;
            labelAMCheckSts.Text = AMCheckFaultStr;
            labelAMCheckSts.ForeColor = Color.Red;
            labelAMCheckSts.Visible = true;
            buttonForward.Enabled = false;
        }

        private void AMConnectedShow()
        {
            //AMConnected = true;
            //AMCheckResultStr = AMCheckOKStr;
            //AMCheckResultCol = Color.Green;
            labelAMCheckSts.Text = AMCheckOKStr;
            labelAMCheckSts.ForeColor = Color.Green;
            labelAMCheckSts.Visible = true;
            buttonForward.Enabled = true;
        }

        private void buttonAMCheckContinue_Click(object sender, EventArgs e)
        {
            AMCheckHide();
            AMInfoShow();
        }

        private void buttonAMInfoBack_Click(object sender, EventArgs e)
        {
            AMInfoHide();
            AMCheckShow();
        }

        private void buttonAMInfoContinue_Click(object sender, EventArgs e)
        {
            AMInfoHide();
            treatAddShow();
        }

        private void buttonTreatAddCancel_Click(object sender, EventArgs e)
        {
            FormNotify formNotify = new FormNotify(notifyTreatAddCancelCapStr, notifyTreatAddCancelTextStr,NotifyButtons.YesNo);
            formNotify.ShowDialog();
            if (formNotify.DialogResult == DialogResult.Yes)
            {
                treatAddHide();
                loginShow();
                formClear();
            }
            formNotify.Dispose();
        }

        private async void buttonTreatAddApprove_Click(object sender, EventArgs e)
        {
            comboBoxTreat.Enabled = false;
            buttonBack.Enabled = false;
            buttonForward.Enabled = false;
            progressBarAMCheck.Visible = true;
            amData.serialPortProgressEvent += new EventHandler(progressBarAMCheckProgress_Callback);
            int errcode = await amData.AMDataWrite();
            if (errcode >= 0)
            {
                errcode = await amData.AMDataRead();
            }
            progressBarAMCheck.Value = progressBarAMCheck.Minimum;
            progressBarAMCheck.Visible = false;
            if (errcode == 0)
            {
                FormNotify formNotify = new FormNotify(notifyTreatAddOKCapStr,
                    string.Format(notifyTreatAddOKTextStr, amData.MaxiSet, amData.Maxi, amData.SNum),
                    NotifyButtons.OK);
                formNotify.ShowDialog();
                formNotify.Dispose();
            }
            else if (errcode == -2)
            {
                // wrong parameters
                FormNotify formNotify = new FormNotify(notifyTreatAddFaultCapStr, notifyTreatAddWrongParameterTextStr, NotifyButtons.OK);
                formNotify.ShowDialog();
                formNotify.Dispose();
            }
            else
            {
                // if fail
                AMConnected = false;
                FormNotify formNotify = new FormNotify(notifyTreatAddFaultCapStr, notifyTreatAddFaultTextStr, NotifyButtons.OK);
                formNotify.ShowDialog();
                formNotify.Dispose();
            }
            comboBoxTreat.Enabled = true;
            buttonBack.Enabled = true;
            buttonForward.Enabled = true;
            if (!AMConnected)
            {
                treatAddHide();
                AMCheckShow();
            }
        }

        private void comboBoxTreat_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                amData.MaxiSet = int.Parse(comboBoxTreat.SelectedItem.ToString(), NumberStyles.Integer);
            }
            catch (Exception ex)
            {
                LogFile.logWrite(ex.ToString());
            }
        }
    }
}
