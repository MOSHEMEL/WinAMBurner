
namespace WinAMBurner
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBoxAuthorize = new System.Windows.Forms.PictureBox();
            this.richTextBoxUsername = new System.Windows.Forms.RichTextBox();
            this.richTextBoxPassword = new System.Windows.Forms.RichTextBox();
            this.buttonLogin = new System.Windows.Forms.Button();
            this.labelAMCheckWellcom = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonBack = new System.Windows.Forms.Button();
            this.buttonForward = new System.Windows.Forms.Button();
            this.labelAMInfoTreatNum = new System.Windows.Forms.Label();
            this.labelAMInfoSNum = new System.Windows.Forms.Label();
            this.comboBoxTreat = new System.Windows.Forms.ComboBox();
            this.progressBarAMCheck = new System.Windows.Forms.ProgressBar();
            this.labelAMCheckSts = new System.Windows.Forms.Label();
            this.labelLoginForgot = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAuthorize)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxAuthorize
            // 
            this.pictureBoxAuthorize.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBoxAuthorize.Image = global::WinAMBurner.Properties.Resources.ARmentaSmall;
            this.pictureBoxAuthorize.Location = new System.Drawing.Point(555, 90);
            this.pictureBoxAuthorize.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBoxAuthorize.Name = "pictureBoxAuthorize";
            this.pictureBoxAuthorize.Size = new System.Drawing.Size(304, 130);
            this.pictureBoxAuthorize.TabIndex = 0;
            this.pictureBoxAuthorize.TabStop = false;
            // 
            // richTextBoxUsername
            // 
            this.richTextBoxUsername.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.richTextBoxUsername.Location = new System.Drawing.Point(438, 406);
            this.richTextBoxUsername.Margin = new System.Windows.Forms.Padding(4);
            this.richTextBoxUsername.Name = "richTextBoxUsername";
            this.richTextBoxUsername.Size = new System.Drawing.Size(538, 42);
            this.richTextBoxUsername.TabIndex = 1;
            this.richTextBoxUsername.Text = "Username";
            // 
            // richTextBoxPassword
            // 
            this.richTextBoxPassword.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.richTextBoxPassword.Location = new System.Drawing.Point(438, 494);
            this.richTextBoxPassword.Margin = new System.Windows.Forms.Padding(4);
            this.richTextBoxPassword.Name = "richTextBoxPassword";
            this.richTextBoxPassword.Size = new System.Drawing.Size(538, 42);
            this.richTextBoxPassword.TabIndex = 2;
            this.richTextBoxPassword.Text = "Password";
            // 
            // buttonLogin
            // 
            this.buttonLogin.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonLogin.Location = new System.Drawing.Point(587, 736);
            this.buttonLogin.Margin = new System.Windows.Forms.Padding(4);
            this.buttonLogin.Name = "buttonLogin";
            this.buttonLogin.Size = new System.Drawing.Size(240, 46);
            this.buttonLogin.TabIndex = 3;
            this.buttonLogin.Text = "Login";
            this.buttonLogin.UseVisualStyleBackColor = true;
            this.buttonLogin.Click += new System.EventHandler(this.buttonLogin_Click);
            // 
            // labelAMCheckWellcom
            // 
            this.labelAMCheckWellcom.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelAMCheckWellcom.AutoSize = true;
            this.labelAMCheckWellcom.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelAMCheckWellcom.Location = new System.Drawing.Point(438, 310);
            this.labelAMCheckWellcom.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelAMCheckWellcom.Name = "labelAMCheckWellcom";
            this.labelAMCheckWellcom.Size = new System.Drawing.Size(91, 38);
            this.labelAMCheckWellcom.TabIndex = 4;
            this.labelAMCheckWellcom.Text = "label0";
            this.labelAMCheckWellcom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelAMCheckWellcom.Visible = false;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(438, 409);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 30);
            this.label1.TabIndex = 5;
            this.label1.Text = "label1";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.Visible = false;
            // 
            // buttonBack
            // 
            this.buttonBack.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonBack.Location = new System.Drawing.Point(256, 736);
            this.buttonBack.Margin = new System.Windows.Forms.Padding(4);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(240, 46);
            this.buttonBack.TabIndex = 6;
            this.buttonBack.Text = "buttonBack";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Visible = false;
            // 
            // buttonForward
            // 
            this.buttonForward.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonForward.Location = new System.Drawing.Point(899, 736);
            this.buttonForward.Margin = new System.Windows.Forms.Padding(4);
            this.buttonForward.Name = "buttonForward";
            this.buttonForward.Size = new System.Drawing.Size(240, 46);
            this.buttonForward.TabIndex = 7;
            this.buttonForward.Text = "buttonForward";
            this.buttonForward.UseVisualStyleBackColor = true;
            this.buttonForward.Visible = false;
            // 
            // labelAMInfoTreatNum
            // 
            this.labelAMInfoTreatNum.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelAMInfoTreatNum.AutoSize = true;
            this.labelAMInfoTreatNum.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelAMInfoTreatNum.ForeColor = System.Drawing.Color.Black;
            this.labelAMInfoTreatNum.Location = new System.Drawing.Point(438, 581);
            this.labelAMInfoTreatNum.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelAMInfoTreatNum.Name = "labelAMInfoTreatNum";
            this.labelAMInfoTreatNum.Size = new System.Drawing.Size(218, 30);
            this.labelAMInfoTreatNum.TabIndex = 9;
            this.labelAMInfoTreatNum.Text = "labelAMInfoTreatNum";
            this.labelAMInfoTreatNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelAMInfoTreatNum.Visible = false;
            // 
            // labelAMInfoSNum
            // 
            this.labelAMInfoSNum.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelAMInfoSNum.AutoSize = true;
            this.labelAMInfoSNum.Location = new System.Drawing.Point(438, 497);
            this.labelAMInfoSNum.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelAMInfoSNum.Name = "labelAMInfoSNum";
            this.labelAMInfoSNum.Size = new System.Drawing.Size(68, 30);
            this.labelAMInfoSNum.TabIndex = 14;
            this.labelAMInfoSNum.Text = "label2";
            this.labelAMInfoSNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelAMInfoSNum.Visible = false;
            // 
            // comboBoxTreat
            // 
            this.comboBoxTreat.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.comboBoxTreat.FormattingEnabled = true;
            this.comboBoxTreat.Location = new System.Drawing.Point(437, 494);
            this.comboBoxTreat.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxTreat.Name = "comboBoxTreat";
            this.comboBoxTreat.Size = new System.Drawing.Size(538, 38);
            this.comboBoxTreat.TabIndex = 17;
            this.comboBoxTreat.Visible = false;
            this.comboBoxTreat.SelectedIndexChanged += new System.EventHandler(this.comboBoxTreat_SelectedIndexChanged);
            // 
            // progressBarAMCheck
            // 
            this.progressBarAMCheck.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.progressBarAMCheck.Location = new System.Drawing.Point(438, 579);
            this.progressBarAMCheck.Margin = new System.Windows.Forms.Padding(4);
            this.progressBarAMCheck.Name = "progressBarAMCheck";
            this.progressBarAMCheck.Size = new System.Drawing.Size(540, 24);
            this.progressBarAMCheck.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarAMCheck.TabIndex = 18;
            this.progressBarAMCheck.Visible = false;
            // 
            // labelAMCheckSts
            // 
            this.labelAMCheckSts.AutoSize = true;
            this.labelAMCheckSts.Location = new System.Drawing.Point(438, 497);
            this.labelAMCheckSts.Name = "labelAMCheckSts";
            this.labelAMCheckSts.Size = new System.Drawing.Size(172, 30);
            this.labelAMCheckSts.TabIndex = 19;
            this.labelAMCheckSts.Text = "labelAMCheckSts";
            this.labelAMCheckSts.Visible = false;
            // 
            // labelLoginForgot
            // 
            this.labelLoginForgot.AutoSize = true;
            this.labelLoginForgot.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point);
            this.labelLoginForgot.ForeColor = System.Drawing.Color.Blue;
            this.labelLoginForgot.Location = new System.Drawing.Point(443, 581);
            this.labelLoginForgot.Name = "labelLoginForgot";
            this.labelLoginForgot.Size = new System.Drawing.Size(166, 30);
            this.labelLoginForgot.TabIndex = 20;
            this.labelLoginForgot.Text = "Forgot password";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1414, 1013);
            this.Controls.Add(this.labelLoginForgot);
            this.Controls.Add(this.labelAMCheckSts);
            this.Controls.Add(this.comboBoxTreat);
            this.Controls.Add(this.labelAMInfoSNum);
            this.Controls.Add(this.labelAMInfoTreatNum);
            this.Controls.Add(this.buttonForward);
            this.Controls.Add(this.buttonBack);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelAMCheckWellcom);
            this.Controls.Add(this.buttonLogin);
            this.Controls.Add(this.richTextBoxPassword);
            this.Controls.Add(this.richTextBoxUsername);
            this.Controls.Add(this.pictureBoxAuthorize);
            this.Controls.Add(this.progressBarAMCheck);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pulse’s Updater";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAuthorize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxAuthorize;
        private System.Windows.Forms.RichTextBox richTextBoxUsername;
        private System.Windows.Forms.RichTextBox richTextBoxPassword;
        private System.Windows.Forms.Button buttonLogin;
        private System.Windows.Forms.Label labelAMCheckWellcom;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.Button buttonForward;
        private System.Windows.Forms.Label labelAMInfoTreatNum;
        private System.Windows.Forms.Label labelAMInfoSNum;
        private System.Windows.Forms.ComboBox comboBoxTreat;
        private System.Windows.Forms.ProgressBar progressBarAMCheck;
        private System.Windows.Forms.Label labelAMCheckSts;
        private System.Windows.Forms.Label labelLoginForgot;
    }
}