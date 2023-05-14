namespace VeriketApp.Form
{
    partial class ShowForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShowForm));
            list_Log = new ListBox();
            btnLog = new Button();
            SuspendLayout();
            // 
            // list_Log
            // 
            list_Log.Dock = DockStyle.Top;
            list_Log.FormattingEnabled = true;
            list_Log.ItemHeight = 20;
            list_Log.Location = new Point(0, 0);
            list_Log.Name = "list_Log";
            list_Log.Size = new Size(565, 244);
            list_Log.TabIndex = 0;
            // 
            // btnLog
            // 
            btnLog.BackColor = SystemColors.ActiveCaptionText;
            btnLog.Dock = DockStyle.Top;
            btnLog.Font = new Font("Arial Black", 13.8F, FontStyle.Bold, GraphicsUnit.Point);
            btnLog.ForeColor = SystemColors.ButtonHighlight;
            btnLog.Location = new Point(0, 244);
            btnLog.Name = "btnLog";
            btnLog.Size = new Size(565, 64);
            btnLog.TabIndex = 1;
            btnLog.Text = "Logları İzle";
            btnLog.UseVisualStyleBackColor = false;
            btnLog.Click += btnLog_Click;
            // 
            // ShowForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(565, 307);
            Controls.Add(btnLog);
            Controls.Add(list_Log);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "ShowForm";
            Text = "ShowForm";
            ResumeLayout(false);
        }

        #endregion
        private Button btnLog;
        public ListBox list_Log;
    }
}