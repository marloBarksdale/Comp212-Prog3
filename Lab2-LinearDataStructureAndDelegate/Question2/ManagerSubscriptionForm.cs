using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Question2
{
    public partial class ManageSubscriptionsForm : Form
    {
        private List<SendViaEmail> emailList;
        private List<SendViaMobile> mobileList;

        public delegate void SubscriptionChangedHandler();
        public event SubscriptionChangedHandler? SubscriptionChanged;

        public ManageSubscriptionsForm(List<SendViaEmail> emails, List<SendViaMobile> mobiles)
        {
            InitializeComponent();
            emailList = emails;
            mobileList = mobiles;
        }

        private void chkEmail_CheckedChanged(object sender, EventArgs e)
        {
            txtEmail.Enabled = chkEmail.Checked;
        }

        private void chkSMS_CheckedChanged(object sender, EventArgs e)
        {
            txtSMS.Enabled = chkSMS.Checked;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSubscribe_Click(object sender, EventArgs e)
        {
            lblEmailError.Visible = false;
            bool subscribed = false;

            if (chkEmail.Checked)
            {
                string email = txtEmail.Text.Trim();
                if (!IsValidEmail(email))
                {
                    lblEmailError.Visible = true;
                    return;
                }

                var emailSub = new SendViaEmail(email);
                if (!emailList.Contains(emailSub))
                {
                    emailList.Add(emailSub);
                    subscribed = true;
                }
                else
                {
                    MessageBox.Show("Email already subscribed.");
                }
            }

            if (chkSMS.Checked)
            {
                string mobile = txtSMS.Text.Trim();
                if (!IsValidMobile(mobile))
                {
                    MessageBox.Show("Invalid mobile number.");
                    return;
                }

                var smsSub = new SendViaMobile(mobile);
                if (!mobileList.Contains(smsSub))
                {
                    mobileList.Add(smsSub);
                    subscribed = true;
                }
                else
                {
                    MessageBox.Show("Mobile already subscribed.");
                }
            }

            if (subscribed)
            {
                SubscriptionChanged?.Invoke();
                MessageBox.Show("Subscribed successfully.");
                txtEmail.Clear();
                txtSMS.Clear();
            }
        }

        private void btnUnsubscribe_Click(object sender, EventArgs e)
        {
            bool unsubscribed = false;

            if (chkEmail.Checked)
            {
                var email = new SendViaEmail(txtEmail.Text.Trim());
                if (emailList.Contains(email))
                {
                    emailList.Remove(email);
                    unsubscribed = true;
                }
                else
                {
                    MessageBox.Show("Email is not subscribed.");
                }
            }

            if (chkSMS.Checked)
            {
                var mobile = new SendViaMobile(txtSMS.Text.Trim());
                if (mobileList.Contains(mobile))
                {
                    mobileList.Remove(mobile);
                    unsubscribed = true;
                }
                else
                {
                    MessageBox.Show("Mobile is not subscribed.");
                }
            }

            if (unsubscribed)
            {
                SubscriptionChanged?.Invoke();
                MessageBox.Show("Unsubscribed successfully.");
                txtEmail.Clear();
                txtSMS.Clear();
            }
        }

        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private bool IsValidMobile(string mobile)
        {
            return Regex.IsMatch(mobile, @"^\d{10}$");
        }
    }
}
