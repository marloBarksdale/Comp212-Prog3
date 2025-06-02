using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Question2
{
    public partial class ManagerSubscriptionForm : Form
    {
       
        private List<SendViaEmail> emailList;

        private List<SendViaMobile> mobileList;

       
        public event Action SubscriptionChanged;



        // Add fields for designer controls


        public ManagerSubscriptionForm()
        {
            InitializeComponent();
            emailList = new List<SendViaEmail>();
            mobileList = new List<SendViaMobile>();
        }

        public ManagerSubscriptionForm(List<SendViaEmail> emails, List<SendViaMobile> mobiles)
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
                if (!emailList.Exists(s => s.EmailAddress.Equals(email, StringComparison.OrdinalIgnoreCase)))
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
                if (!mobileList.Exists(s => s.PhoneNumber == mobile))
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
                chkEmail.Checked = false;
                chkSMS.Checked = false;
            }
        }

        

        private void btnUnsubscribe_Click(object sender, EventArgs e)
        {
            bool unsubscribed = false;

            if (chkEmail.Checked)
            {
                string email = txtEmail.Text.Trim();
                var existing = emailList.Find(s => s.EmailAddress.Equals(email, StringComparison.OrdinalIgnoreCase));
                if (existing != null)
                {
                    emailList.Remove(existing);
                    unsubscribed = true;
                }
                else
                {
                    MessageBox.Show("Email is not subscribed.");
                }
            }

            if (chkSMS.Checked)
            {
                string mobile = txtSMS.Text.Trim();
                var existing = mobileList.Find(s => s.PhoneNumber == mobile);
                if (existing != null)
                {
                    mobileList.Remove(existing);
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
