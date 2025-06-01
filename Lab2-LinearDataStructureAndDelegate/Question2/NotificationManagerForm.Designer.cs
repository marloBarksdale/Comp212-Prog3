namespace Question2
{
    partial class NotificationManagerForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnManageSubscriptions = new Button();
            btnPublishNotification = new Button();
            btnExit = new Button();
            SuspendLayout();
            // 
            // btnManageSubscriptions
            // 
            btnManageSubscriptions.Location = new Point(12, 27);
            btnManageSubscriptions.Name = "btnManageSubscriptions";
            btnManageSubscriptions.Size = new Size(132, 23);
            btnManageSubscriptions.TabIndex = 0;
            btnManageSubscriptions.Text = "Manage Subscription";
            btnManageSubscriptions.UseVisualStyleBackColor = true;
            btnManageSubscriptions.Click += this.btnManageSubscriptions_Click;
            // 
            // btnPublishNotification
            // 
            btnPublishNotification.Enabled = false;
            btnPublishNotification.Location = new Point(150, 27);
            btnPublishNotification.Name = "btnPublishNotification";
            btnPublishNotification.Size = new Size(129, 23);
            btnPublishNotification.TabIndex = 1;
            btnPublishNotification.Text = "Publish Notification";
            btnPublishNotification.UseVisualStyleBackColor = true;
            // 
            // btnExit
            // 
            btnExit.Location = new Point(285, 27);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(87, 23);
            btnExit.TabIndex = 2;
            btnExit.Text = "Exit";
            btnExit.UseVisualStyleBackColor = true;
            // 
            // NotificationManagerForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(384, 81);
            Controls.Add(btnExit);
            Controls.Add(btnPublishNotification);
            Controls.Add(btnManageSubscriptions);
            Name = "NotificationManagerForm";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button btnManageSubscriptions;
        private Button btnPublishNotification;
        private Button btnExit;
    }
}
