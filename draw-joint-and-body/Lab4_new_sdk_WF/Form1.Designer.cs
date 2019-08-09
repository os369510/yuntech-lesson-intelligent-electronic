namespace Lab4_new_sdk_WF
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.StartButtonTimer = new System.Windows.Forms.Timer(this.components);
            this.GameTimer = new System.Windows.Forms.Timer(this.components);
            this.Status = new System.Windows.Forms.TextBox();
            this.StatusValue = new System.Windows.Forms.TextBox();
            this.GameTime = new System.Windows.Forms.TextBox();
            this.GameTimeValue = new System.Windows.Forms.TextBox();
            this.Title = new System.Windows.Forms.TextBox();
            this.TextChangeTimer = new System.Windows.Forms.Timer(this.components);
            this.ClearStart = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(8, 45);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(640, 480);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // serialPort1
            // 
            this.serialPort1.PortName = "COM6";
            // 
            // StartButtonTimer
            // 
            this.StartButtonTimer.Interval = 3000;
            this.StartButtonTimer.Tick += new System.EventHandler(this.StartButtonTimer_Tick);
            // 
            // GameTimer
            // 
            this.GameTimer.Interval = 1000;
            this.GameTimer.Tick += new System.EventHandler(this.GameTimer_Tick);
            // 
            // Status
            // 
            this.Status.BackColor = System.Drawing.Color.Yellow;
            this.Status.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Status.Cursor = System.Windows.Forms.Cursors.No;
            this.Status.Font = new System.Drawing.Font("文鼎勘亭流", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Status.ForeColor = System.Drawing.Color.Red;
            this.Status.Location = new System.Drawing.Point(329, 9);
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            this.Status.Size = new System.Drawing.Size(65, 27);
            this.Status.TabIndex = 1;
            this.Status.Text = "狀態";
            // 
            // StatusValue
            // 
            this.StatusValue.BackColor = System.Drawing.Color.White;
            this.StatusValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.StatusValue.Cursor = System.Windows.Forms.Cursors.No;
            this.StatusValue.Font = new System.Drawing.Font("Ruach LET", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StatusValue.Location = new System.Drawing.Point(390, 6);
            this.StatusValue.Name = "StatusValue";
            this.StatusValue.ReadOnly = true;
            this.StatusValue.Size = new System.Drawing.Size(117, 35);
            this.StatusValue.TabIndex = 2;
            // 
            // GameTime
            // 
            this.GameTime.BackColor = System.Drawing.Color.Yellow;
            this.GameTime.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.GameTime.Cursor = System.Windows.Forms.Cursors.No;
            this.GameTime.Font = new System.Drawing.Font("文鼎勘亭流", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.GameTime.ForeColor = System.Drawing.Color.Red;
            this.GameTime.Location = new System.Drawing.Point(513, 9);
            this.GameTime.Name = "GameTime";
            this.GameTime.ReadOnly = true;
            this.GameTime.Size = new System.Drawing.Size(63, 27);
            this.GameTime.TabIndex = 3;
            this.GameTime.Text = "時間";
            // 
            // GameTimeValue
            // 
            this.GameTimeValue.BackColor = System.Drawing.Color.White;
            this.GameTimeValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.GameTimeValue.Cursor = System.Windows.Forms.Cursors.No;
            this.GameTimeValue.Font = new System.Drawing.Font("新細明體", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.GameTimeValue.Location = new System.Drawing.Point(573, 6);
            this.GameTimeValue.Name = "GameTimeValue";
            this.GameTimeValue.ReadOnly = true;
            this.GameTimeValue.Size = new System.Drawing.Size(75, 33);
            this.GameTimeValue.TabIndex = 4;
            // 
            // Title
            // 
            this.Title.BackColor = System.Drawing.SystemColors.Control;
            this.Title.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Title.Cursor = System.Windows.Forms.Cursors.No;
            this.Title.Font = new System.Drawing.Font("微軟正黑體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Title.ForeColor = System.Drawing.Color.Black;
            this.Title.Location = new System.Drawing.Point(8, 9);
            this.Title.Name = "Title";
            this.Title.ReadOnly = true;
            this.Title.Size = new System.Drawing.Size(315, 32);
            this.Title.TabIndex = 5;
            this.Title.Text = "Move & Push 逃離粉色牢籠";
            // 
            // TextChangeTimer
            // 
            this.TextChangeTimer.Interval = 500;
            this.TextChangeTimer.Tick += new System.EventHandler(this.TextChangeTimer_Tick);
            // 
            // ClearStart
            // 
            this.ClearStart.Interval = 1000;
            this.ClearStart.Tick += new System.EventHandler(this.ClearStart_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(657, 531);
            this.Controls.Add(this.Title);
            this.Controls.Add(this.GameTimeValue);
            this.Controls.Add(this.GameTime);
            this.Controls.Add(this.StatusValue);
            this.Controls.Add(this.Status);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Timer StartButtonTimer;
        private System.Windows.Forms.Timer GameTimer;
        private System.Windows.Forms.TextBox Status;
        private System.Windows.Forms.TextBox StatusValue;
        private System.Windows.Forms.TextBox GameTime;
        private System.Windows.Forms.TextBox GameTimeValue;
        private System.Windows.Forms.TextBox Title;
        private System.Windows.Forms.Timer TextChangeTimer;
        private System.Windows.Forms.Timer ClearStart;
    }
}

