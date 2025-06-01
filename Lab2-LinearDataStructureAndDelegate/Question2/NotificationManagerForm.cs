using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Question2
{
    public partial class NotificationManagerForm : Form
    {
        // Subscriber lists
        private List<SendViaEmail> emailSubscribers = new List<SendViaEmail>();
        private List<SendViaMobile> mobileSubscribers = new List<SendViaMobile>();

        public NotificationManagerForm()
        {
            InitializeComponent();

            // Disable publish button on startup
            btnPublishNotification.Enabled = false;
        }

        private void btnManageSubscriptions_Click(object sender, EventArgs e)
        {
            var manageForm = new ManageSubscriptionsForm(emailSubscribers, mobileSubscribers);

            // When subscribers change, update publish button
            manageForm.SubscriptionChanged += UpdatePublishButtonState;
            manageForm.ShowDialog();
        }

        private void btnPublishNotification_Click(object sender, EventArgs e)
        {
            var publishForm = new PublishNotificationForm(emailSubscribers, mobileSubscribers);
            publishForm.ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void UpdatePublishButtonState()
        {
            // Enable publish button if there's at least one subscriber
            btnPublishNotification.Enabled = emailSubscribers.Count > 0 || mobileSubscribers.Count > 0;
        }
    }
}
