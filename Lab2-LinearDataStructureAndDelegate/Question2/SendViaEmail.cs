using System;
using System.Windows.Forms;

namespace Question2
{
    public class SendViaEmail : INotificationSender
    {
        public string EmailAddress { get; set; }

        public SendViaEmail(string email)
        {
            EmailAddress = email;
        }

        public void SendNotification(string message)
        {
            // Simulated email sending logic
            MessageBox.Show($"Email sent to {EmailAddress}: {message}", "Email Notification");
        }

        public override string ToString()
        {
            return $"Email: {EmailAddress}";
        }
    }
}
