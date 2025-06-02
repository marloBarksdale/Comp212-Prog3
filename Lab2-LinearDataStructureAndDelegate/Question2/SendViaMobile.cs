using System;
using System.Windows.Forms;

namespace Question2
{
    public class SendViaMobile : INotificationSender
    {
        public string PhoneNumber { get; set; }

        public SendViaMobile(string number)
        {
            PhoneNumber = number;
        }

        public void SendNotification(string message)
        {
            // Simulated SMS sending logic
            MessageBox.Show($"SMS sent to {PhoneNumber}: {message}", "SMS Notification");
        }

        public override string ToString()
        {
            return $"SMS: {PhoneNumber}";
        }
    }
}
