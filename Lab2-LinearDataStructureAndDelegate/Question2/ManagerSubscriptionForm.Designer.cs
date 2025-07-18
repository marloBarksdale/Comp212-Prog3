﻿
namespace Question2
{
    partial class ManagerSubscriptionForm  
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) // Fix CS0115 by ensuring Dispose method matches the base class signature
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            chkEmail = new CheckBox();
            txtEmail = new TextBox();
            lblEmailError = new Label();
            chkSMS = new CheckBox();
            txtSMS = new TextBox();
            btnSubscribe = new Button();
            btnUnsubscribe = new Button();
            btnCancel = new Button();
            lblPhoneError = new Label();
            SuspendLayout();
            // 
            // chkEmail
            // 
            chkEmail.AutoSize = true;
            chkEmail.Checked = true;
            chkEmail.CheckState = CheckState.Checked;
            chkEmail.Location = new Point(44, 40);
            chkEmail.Name = "chkEmail";
            chkEmail.Size = new Size(163, 19);
            chkEmail.TabIndex = 0;
            chkEmail.Text = "Notification Sent by Email";
            chkEmail.UseVisualStyleBackColor = true;
            chkEmail.CheckedChanged += chkEmail_CheckedChanged;
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(243, 36);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(201, 23);
            txtEmail.TabIndex = 1;
            // 
            // lblEmailError
            // 
            lblEmailError.AutoSize = true;
            lblEmailError.ForeColor = Color.Red;
            lblEmailError.Location = new Point(246, 69);
            lblEmailError.Name = "lblEmailError";
            lblEmailError.Size = new Size(117, 15);
            lblEmailError.TabIndex = 2;
            lblEmailError.Text = "Invalid email address";
            lblEmailError.Visible = false;
            // 
            // chkSMS
            // 
            chkSMS.AutoSize = true;
            chkSMS.Location = new Point(44, 127);
            chkSMS.Name = "chkSMS";
            chkSMS.Size = new Size(157, 19);
            chkSMS.TabIndex = 3;
            chkSMS.Text = "Notification Sent by SMS";
            chkSMS.UseVisualStyleBackColor = true;
            chkSMS.CheckedChanged += chkSMS_CheckedChanged;
            // 
            // txtSMS
            // 
            txtSMS.Enabled = false;
            txtSMS.Location = new Point(243, 123);
            txtSMS.Name = "txtSMS";
            txtSMS.Size = new Size(198, 23);
            txtSMS.TabIndex = 4;
            // 
            // btnSubscribe
            // 
            btnSubscribe.Location = new Point(49, 183);
            btnSubscribe.Name = "btnSubscribe";
            btnSubscribe.Size = new Size(75, 23);
            btnSubscribe.TabIndex = 5;
            btnSubscribe.Text = "Subscribe";
            btnSubscribe.UseVisualStyleBackColor = true;
            btnSubscribe.Click += btnSubscribe_Click;
            // 
            // btnUnsubscribe
            // 
            btnUnsubscribe.Location = new Point(171, 183);
            btnUnsubscribe.Name = "btnUnsubscribe";
            btnUnsubscribe.Size = new Size(87, 23);
            btnUnsubscribe.TabIndex = 6;
            btnUnsubscribe.Text = "Unsubscribe";
            btnUnsubscribe.UseVisualStyleBackColor = true;
            btnUnsubscribe.Click += btnUnsubscribe_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(299, 183);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(86, 23);
            btnCancel.TabIndex = 7;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // lblPhoneError
            // 
            lblPhoneError.AutoSize = true;
            lblPhoneError.ForeColor = Color.Red;
            lblPhoneError.Location = new Point(243, 149);
            lblPhoneError.Name = "lblPhoneError";
            lblPhoneError.Size = new Size(87, 15);
            lblPhoneError.TabIndex = 8;
            lblPhoneError.Text = "Invalid number";
            lblPhoneError.Visible = false;
            // 
            // ManagerSubscriptionForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(516, 236);
            Controls.Add(lblPhoneError);
            Controls.Add(btnCancel);
            Controls.Add(btnUnsubscribe);
            Controls.Add(btnSubscribe);
            Controls.Add(txtSMS);
            Controls.Add(chkSMS);
            Controls.Add(lblEmailError);
            Controls.Add(txtEmail);
            Controls.Add(chkEmail);
            Name = "ManagerSubscriptionForm";
            Text = "Manage Subscription";
            ResumeLayout(false);
            PerformLayout();
        }

        //private void SuspendLayout()
        //{
        //    throw new NotImplementedException();
        //}

        #endregion

        private CheckBox chkEmail;
        private TextBox txtEmail;
        private Label lblEmailError;
        private CheckBox chkSMS;
        private TextBox txtSMS;
        private Button btnSubscribe;
        private Button btnUnsubscribe;
        private Button btnCancel;
        private Label lblPhoneError;
    }
}