namespace PDJaya.Kiosk.UI
{
    partial class FormKonfigurasi
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.BtnConfig = new System.Windows.Forms.Button();
            this.TxtDeviceNo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.BtnConfig);
            this.groupBox1.Controls.Add(this.TxtDeviceNo);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(9, 10);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Size = new System.Drawing.Size(278, 128);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Data Device";
            // 
            // BtnConfig
            // 
            this.BtnConfig.Location = new System.Drawing.Point(103, 56);
            this.BtnConfig.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.BtnConfig.Name = "BtnConfig";
            this.BtnConfig.Size = new System.Drawing.Size(170, 25);
            this.BtnConfig.TabIndex = 2;
            this.BtnConfig.Text = "Get Configuration From Server";
            this.BtnConfig.UseVisualStyleBackColor = true;
            // 
            // TxtDeviceNo
            // 
            this.TxtDeviceNo.Location = new System.Drawing.Point(70, 33);
            this.TxtDeviceNo.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.TxtDeviceNo.Name = "TxtDeviceNo";
            this.TxtDeviceNo.Size = new System.Drawing.Size(204, 20);
            this.TxtDeviceNo.TabIndex = 1;
            this.TxtDeviceNo.Text = "D001";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 36);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Device No :";
            // 
            // FormKonfigurasi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(296, 145);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "FormKonfigurasi";
            this.Text = "FormKonfigurasi";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button BtnConfig;
        private System.Windows.Forms.TextBox TxtDeviceNo;
        private System.Windows.Forms.Label label1;
    }
}