namespace Question2
{
    partial class PublishNotificationForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            lblMessage = new Label();
            textBox1 = new TextBox();
            btnPublish = new Button();
            btnExit = new Button();
            SuspendLayout();
            // 
            // lblMessage
            // 
            lblMessage.AutoSize = true;
            lblMessage.Location = new Point(28, 51);
            lblMessage.Name = "lblMessage";
            lblMessage.Size = new Size(116, 15);
            lblMessage.TabIndex = 0;
            lblMessage.Text = "Notification Content";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(150, 41);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(300, 25);
            textBox1.TabIndex = 1;
            // 
            // btnPublish
            // 
            btnPublish.Enabled = false;
            btnPublish.Location = new Point(30, 170);
            btnPublish.Name = "btnPublish";
            btnPublish.Size = new Size(75, 23);
            btnPublish.TabIndex = 2;
            btnPublish.Text = "Publish";
            btnPublish.UseVisualStyleBackColor = true;
            btnPublish.Click += new EventHandler(this.btnPublish_Click);
            // 
            // btnExit
            // 
            btnExit.Location = new Point(337, 170);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(95, 23);
            btnExit.TabIndex = 3;
            btnExit.Text = "Exit";
            btnExit.UseVisualStyleBackColor = true;
            btnExit.Click += new EventHandler(this.btnExit_Click);
            // 
            // PublishNotificationForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(503, 220);
            Controls.Add(btnExit);
            Controls.Add(btnPublish);
            Controls.Add(textBox1);
            Controls.Add(lblMessage);
            Name = "PublishNotificationForm";
            Text = "Publish Notification";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblMessage;
        private TextBox textBox1;
        private Button btnPublish;
        private Button btnExit;
    }
}
