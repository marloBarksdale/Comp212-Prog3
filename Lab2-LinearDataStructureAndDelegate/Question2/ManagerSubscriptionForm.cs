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

        public ManagerSubscriptionForm()
        {
            InitializeComponent();
            ValidateUnsubscribeEligibility();
            emailList = new List<SendViaEmail>();
            mobileList = new List<SendViaMobile>();

            txtEmail.TextChanged += InputFields_TextChanged;
            txtSMS.TextChanged += InputFields_TextChanged;
 

            btnSubscribe.Enabled = false;
            btnUnsubscribe.Enabled = false;
           

            //UpdateButtonStates();
        }

        public ManagerSubscriptionForm(List<SendViaEmail> emails, List<SendViaMobile> mobiles)
        {
            InitializeComponent();
            ValidateUnsubscribeEligibility();
            emailList = emails;
            mobileList = mobiles;
            btnSubscribe.Enabled = false;
            btnUnsubscribe.Enabled = false;
            txtEmail.TextChanged += InputFields_TextChanged;
            txtSMS.TextChanged += InputFields_TextChanged;
       



            //UpdateButtonStates();
        }

        private void chkEmail_CheckedChanged(object sender, EventArgs e)
        {
            txtEmail.Enabled = chkEmail.Checked;
            InputFields_TextChanged(null, null);
        }

        private void chkSMS_CheckedChanged(object sender, EventArgs e)
        {
            txtSMS.Enabled = chkSMS.Checked;
            InputFields_TextChanged(null, null);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void btnSubscribe_Click(object sender, EventArgs e)
        {
            lblEmailError.Visible = false;
            lblPhoneError.Visible = false;

            bool subscribed = false;

            if (chkEmail.Checked)
            {
                string email = txtEmail.Text.Trim();
                if (!IsValidEmail(email))
                {
                    lblEmailError.Visible = true;
                }
                else
                {
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
            }

            if (chkSMS.Checked)
            {
                string mobile = txtSMS.Text.Trim();
                if (!IsValidPhone(mobile))
                {
                    lblPhoneError.Visible = true;
                }
                else
                {
                    var mobileSub = new SendViaMobile(mobile);
                    if (!mobileList.Exists(s => s.PhoneNumber == mobile))
                    {
                        mobileList.Add(mobileSub);
                        subscribed = true;
                    }
                    else
                    {
                        MessageBox.Show("Mobile number already subscribed.");
                    }
                }
            }

            if (subscribed)
            {
                txtEmail.Clear();
                txtSMS.Clear();
                btnSubscribe.Enabled = false;
                lblEmailError.Visible = false;
                lblPhoneError.Visible = false;
                SubscriptionChanged?.Invoke();
                MessageBox.Show("Subscription successful.");
                ValidateUnsubscribeEligibility();
            }
        }


        private void btnUnsubscribe_Click(object sender, EventArgs e)
        {
            bool unsubscribed = false;

            if (chkEmail.Checked)
            {
                string email = txtEmail.Text.Trim();
                if (IsValidEmail(email))
                {
                    var subscriber = emailList.Find(s => s.EmailAddress.Equals(email, StringComparison.OrdinalIgnoreCase));
                    if (subscriber != null)
                    {
                        emailList.Remove(subscriber);
                        unsubscribed = true;
                    }
                    else
                    {
                        MessageBox.Show("Email not found in subscription list.");
                    }
                }
            }

            if (chkSMS.Checked)
            {
                string phone = txtSMS.Text.Trim();
                if (IsValidPhone(phone))
                {
                    var subscriber = mobileList.Find(s => s.PhoneNumber == phone);
                    if (subscriber != null)
                    {
                        mobileList.Remove(subscriber);
                        unsubscribed = true;
                    }
                    else
                    {
                        MessageBox.Show("Mobile number not found in subscription list.");
                    }
                }
            }

            if (unsubscribed)
            {
                MessageBox.Show("Unsubscribed successfully.");
                txtEmail.Clear();
                txtSMS.Clear();
                SubscriptionChanged?.Invoke();
                btnSubscribe.Enabled = false;
                lblEmailError.Visible = false;
                lblPhoneError.Visible = false;

                ValidateUnsubscribeEligibility();
            }
        }



        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private bool IsValidPhone(string phone)
        {
            return Regex.IsMatch(phone, @"^\d{10}$");
        }

        private void InputFields_TextChanged(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string phone = txtSMS.Text.Trim();

            bool isEmailChecked = chkEmail.Checked;
            bool isPhoneChecked = chkSMS.Checked;

            bool isValidEmail = IsValidEmail(email);
            bool isValidPhone = IsValidPhone(phone);

            bool emailAlreadyExists = isValidEmail &&
                emailList.Exists(s => s.EmailAddress.Equals(email, StringComparison.OrdinalIgnoreCase));
            bool phoneAlreadyExists = isValidPhone &&
                mobileList.Exists(s => s.PhoneNumber == phone);

            // 👀 Show validation errors while typing
            lblEmailError.Visible = isEmailChecked && !string.IsNullOrEmpty(email) && !isValidEmail;
            lblPhoneError.Visible = isPhoneChecked && !string.IsNullOrEmpty(phone) && !isValidPhone;

            bool enableSubscribe = false;

            // 🔒 Handle combinations
            if (isEmailChecked && isPhoneChecked)
            {
                enableSubscribe = isValidEmail && isValidPhone &&
                                  !emailAlreadyExists && !phoneAlreadyExists;
            }
            else if (isEmailChecked)
            {
                enableSubscribe = isValidEmail && !emailAlreadyExists;
            }
            else if (isPhoneChecked)
            {
                enableSubscribe = isValidPhone && !phoneAlreadyExists;
            }

            btnSubscribe.Enabled = enableSubscribe;

            //  Keep strict unsubscribe logic
            ValidateUnsubscribeEligibility();
        }








        private void ValidateUnsubscribeEligibility()
        {
            string email = txtEmail.Text.Trim();
            string phone = txtSMS.Text.Trim();

            bool canUnsubscribe = false;

            if (chkEmail.Checked && IsValidEmail(email))
            {
                canUnsubscribe = emailList.Exists(s => s.EmailAddress.Equals(email, StringComparison.OrdinalIgnoreCase));
            }

            if (!canUnsubscribe && chkSMS.Checked && IsValidPhone(phone))
            {
                canUnsubscribe = mobileList.Exists(s => s.PhoneNumber == phone);
            }

            btnUnsubscribe.Enabled = canUnsubscribe;
        }

    }
}
