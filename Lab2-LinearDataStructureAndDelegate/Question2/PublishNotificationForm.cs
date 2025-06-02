using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Question2
{
    public partial class PublishNotificationForm : Form
    {
        private List<INotificationSender> subscribers;

        public PublishNotificationForm(List<SendViaEmail> emailSubscribers, List<SendViaMobile> mobileSubscribers)
        {
            InitializeComponent();

            this.subscribers = new List<INotificationSender>();
            this.subscribers.AddRange(emailSubscribers);
            this.subscribers.AddRange(mobileSubscribers);

            btnPublish.Enabled = subscribers.Count > 0;
        }

        private void btnPublish_Click(object sender, EventArgs e)
        {
            string message = textBox1.Text.Trim();

            if (string.IsNullOrEmpty(message))
            {
                MessageBox.Show("Please enter a message to publish.");
                return;
            }

            foreach (var subscriber in subscribers)
            {
                subscriber.SendNotification(message);
            }

            MessageBox.Show("Notification sent to all subscribers!");
            textBox1.Clear();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
