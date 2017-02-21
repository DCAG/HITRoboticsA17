namespace IOT
{
    partial class IOTPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IOTPanel));
            this.ActuatorsGroupBox = new System.Windows.Forms.GroupBox();
            this.StepMotorTextBox = new System.Windows.Forms.TextBox();
            this.RGBLedGroupBox = new System.Windows.Forms.GroupBox();
            this.WhiteButton = new System.Windows.Forms.Button();
            this.RgbTextBox = new System.Windows.Forms.TextBox();
            this.YellowButton = new System.Windows.Forms.Button();
            this.TurnOffButton = new System.Windows.Forms.Button();
            this.GreenButton = new System.Windows.Forms.Button();
            this.PinkButton = new System.Windows.Forms.Button();
            this.BlueButton = new System.Windows.Forms.Button();
            this.AzureButton = new System.Windows.Forms.Button();
            this.RedrButton = new System.Windows.Forms.Button();
            this.LEDTextBox = new System.Windows.Forms.TextBox();
            this.ServoMotorTextBox = new System.Windows.Forms.TextBox();
            this.StepMotorButton = new System.Windows.Forms.Button();
            this.LedButton = new System.Windows.Forms.Button();
            this.ServoMotorButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ReedSensorTextBox = new System.Windows.Forms.TextBox();
            this.ButtonSensorTextBox = new System.Windows.Forms.TextBox();
            this.DTHSensorTextBox = new System.Windows.Forms.TextBox();
            this.LightSensorTextBox = new System.Windows.Forms.TextBox();
            this.reedSensorButton = new System.Windows.Forms.Button();
            this.buttonSensorButton = new System.Windows.Forms.Button();
            this.DTHSensorButton = new System.Windows.Forms.Button();
            this.lightSensorButton = new System.Windows.Forms.Button();
            this.startSmartChainButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.whiteCubeConnectionStateLabel = new System.Windows.Forms.Label();
            this.arduinoConnectionStateLabel = new System.Windows.Forms.Label();
            this.resetButton = new System.Windows.Forms.Button();
            this.RunTimeLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ActuatorsGroupBox.SuspendLayout();
            this.RGBLedGroupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ActuatorsGroupBox
            // 
            this.ActuatorsGroupBox.Controls.Add(this.StepMotorTextBox);
            this.ActuatorsGroupBox.Controls.Add(this.RGBLedGroupBox);
            this.ActuatorsGroupBox.Controls.Add(this.LEDTextBox);
            this.ActuatorsGroupBox.Controls.Add(this.ServoMotorTextBox);
            this.ActuatorsGroupBox.Controls.Add(this.StepMotorButton);
            this.ActuatorsGroupBox.Controls.Add(this.LedButton);
            this.ActuatorsGroupBox.Controls.Add(this.ServoMotorButton);
            this.ActuatorsGroupBox.Location = new System.Drawing.Point(542, 103);
            this.ActuatorsGroupBox.Name = "ActuatorsGroupBox";
            this.ActuatorsGroupBox.Size = new System.Drawing.Size(527, 488);
            this.ActuatorsGroupBox.TabIndex = 1;
            this.ActuatorsGroupBox.TabStop = false;
            this.ActuatorsGroupBox.Text = "Actuators";
            // 
            // StepMotorTextBox
            // 
            this.StepMotorTextBox.ForeColor = System.Drawing.SystemColors.InfoText;
            this.StepMotorTextBox.Location = new System.Drawing.Point(355, 427);
            this.StepMotorTextBox.Name = "StepMotorTextBox";
            this.StepMotorTextBox.ReadOnly = true;
            this.StepMotorTextBox.Size = new System.Drawing.Size(100, 22);
            this.StepMotorTextBox.TabIndex = 8;
            this.StepMotorTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // RGBLedGroupBox
            // 
            this.RGBLedGroupBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("RGBLedGroupBox.BackgroundImage")));
            this.RGBLedGroupBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.RGBLedGroupBox.Controls.Add(this.WhiteButton);
            this.RGBLedGroupBox.Controls.Add(this.RgbTextBox);
            this.RGBLedGroupBox.Controls.Add(this.YellowButton);
            this.RGBLedGroupBox.Controls.Add(this.TurnOffButton);
            this.RGBLedGroupBox.Controls.Add(this.GreenButton);
            this.RGBLedGroupBox.Controls.Add(this.PinkButton);
            this.RGBLedGroupBox.Controls.Add(this.BlueButton);
            this.RGBLedGroupBox.Controls.Add(this.AzureButton);
            this.RGBLedGroupBox.Controls.Add(this.RedrButton);
            this.RGBLedGroupBox.Location = new System.Drawing.Point(23, 258);
            this.RGBLedGroupBox.Name = "RGBLedGroupBox";
            this.RGBLedGroupBox.Size = new System.Drawing.Size(250, 192);
            this.RGBLedGroupBox.TabIndex = 6;
            this.RGBLedGroupBox.TabStop = false;
            this.RGBLedGroupBox.Text = "RGB Led";
            // 
            // WhiteButton
            // 
            this.WhiteButton.BackColor = System.Drawing.Color.White;
            this.WhiteButton.Location = new System.Drawing.Point(95, 96);
            this.WhiteButton.Name = "WhiteButton";
            this.WhiteButton.Size = new System.Drawing.Size(58, 23);
            this.WhiteButton.TabIndex = 7;
            this.WhiteButton.Text = "White";
            this.WhiteButton.UseVisualStyleBackColor = false;
            this.WhiteButton.Click += new System.EventHandler(this.WhiteButton_Click);
            // 
            // RgbTextBox
            // 
            this.RgbTextBox.Location = new System.Drawing.Point(78, 169);
            this.RgbTextBox.Name = "RgbTextBox";
            this.RgbTextBox.ReadOnly = true;
            this.RgbTextBox.Size = new System.Drawing.Size(100, 22);
            this.RgbTextBox.TabIndex = 7;
            this.RgbTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // YellowButton
            // 
            this.YellowButton.BackColor = System.Drawing.Color.Yellow;
            this.YellowButton.Location = new System.Drawing.Point(47, 76);
            this.YellowButton.Name = "YellowButton";
            this.YellowButton.Size = new System.Drawing.Size(56, 23);
            this.YellowButton.TabIndex = 6;
            this.YellowButton.Text = "Yellow";
            this.YellowButton.UseVisualStyleBackColor = false;
            this.YellowButton.Click += new System.EventHandler(this.YellowButton_Click);
            // 
            // TurnOffButton
            // 
            this.TurnOffButton.BackColor = System.Drawing.Color.LightGray;
            this.TurnOffButton.Location = new System.Drawing.Point(5, 30);
            this.TurnOffButton.Name = "TurnOffButton";
            this.TurnOffButton.Size = new System.Drawing.Size(53, 23);
            this.TurnOffButton.TabIndex = 5;
            this.TurnOffButton.Text = "Off";
            this.TurnOffButton.UseVisualStyleBackColor = false;
            this.TurnOffButton.Click += new System.EventHandler(this.TurnOffRGBLedButton_Click);
            // 
            // GreenButton
            // 
            this.GreenButton.BackColor = System.Drawing.Color.Lime;
            this.GreenButton.Location = new System.Drawing.Point(10, 124);
            this.GreenButton.Name = "GreenButton";
            this.GreenButton.Size = new System.Drawing.Size(75, 23);
            this.GreenButton.TabIndex = 4;
            this.GreenButton.Text = "Green";
            this.GreenButton.UseVisualStyleBackColor = false;
            this.GreenButton.Click += new System.EventHandler(this.GreenButton_Click);
            // 
            // PinkButton
            // 
            this.PinkButton.BackColor = System.Drawing.Color.Fuchsia;
            this.PinkButton.Location = new System.Drawing.Point(147, 68);
            this.PinkButton.Name = "PinkButton";
            this.PinkButton.Size = new System.Drawing.Size(43, 31);
            this.PinkButton.TabIndex = 3;
            this.PinkButton.Text = "Pink";
            this.PinkButton.UseVisualStyleBackColor = false;
            this.PinkButton.Click += new System.EventHandler(this.PinkButton_Click);
            // 
            // BlueButton
            // 
            this.BlueButton.BackColor = System.Drawing.Color.Blue;
            this.BlueButton.Location = new System.Drawing.Point(165, 120);
            this.BlueButton.Name = "BlueButton";
            this.BlueButton.Size = new System.Drawing.Size(48, 32);
            this.BlueButton.TabIndex = 2;
            this.BlueButton.Text = "Blue";
            this.BlueButton.UseVisualStyleBackColor = false;
            this.BlueButton.Click += new System.EventHandler(this.BlueButton_Click);
            // 
            // AzureButton
            // 
            this.AzureButton.BackColor = System.Drawing.Color.Aqua;
            this.AzureButton.Location = new System.Drawing.Point(95, 129);
            this.AzureButton.Name = "AzureButton";
            this.AzureButton.Size = new System.Drawing.Size(60, 39);
            this.AzureButton.TabIndex = 1;
            this.AzureButton.Text = "Azure";
            this.AzureButton.UseVisualStyleBackColor = false;
            this.AzureButton.Click += new System.EventHandler(this.AzureButton_Click);
            // 
            // RedrButton
            // 
            this.RedrButton.BackColor = System.Drawing.Color.Red;
            this.RedrButton.Location = new System.Drawing.Point(78, 21);
            this.RedrButton.Name = "RedrButton";
            this.RedrButton.Size = new System.Drawing.Size(72, 23);
            this.RedrButton.TabIndex = 0;
            this.RedrButton.Text = "Red";
            this.RedrButton.UseVisualStyleBackColor = false;
            this.RedrButton.Click += new System.EventHandler(this.RedButton_Click);
            // 
            // LEDTextBox
            // 
            this.LEDTextBox.ForeColor = System.Drawing.SystemColors.InfoText;
            this.LEDTextBox.Location = new System.Drawing.Point(355, 207);
            this.LEDTextBox.Name = "LEDTextBox";
            this.LEDTextBox.ReadOnly = true;
            this.LEDTextBox.Size = new System.Drawing.Size(100, 22);
            this.LEDTextBox.TabIndex = 4;
            this.LEDTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ServoMotorTextBox
            // 
            this.ServoMotorTextBox.Location = new System.Drawing.Point(101, 207);
            this.ServoMotorTextBox.Name = "ServoMotorTextBox";
            this.ServoMotorTextBox.ReadOnly = true;
            this.ServoMotorTextBox.Size = new System.Drawing.Size(100, 22);
            this.ServoMotorTextBox.TabIndex = 4;
            // 
            // StepMotorButton
            // 
            this.StepMotorButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("StepMotorButton.BackgroundImage")));
            this.StepMotorButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.StepMotorButton.Location = new System.Drawing.Point(319, 259);
            this.StepMotorButton.Name = "StepMotorButton";
            this.StepMotorButton.Size = new System.Drawing.Size(168, 191);
            this.StepMotorButton.TabIndex = 3;
            this.StepMotorButton.Text = "StepMotor";
            this.StepMotorButton.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.StepMotorButton.UseVisualStyleBackColor = true;
            this.StepMotorButton.Click += new System.EventHandler(this.StepMotorButton_Click);
            // 
            // LedButton
            // 
            this.LedButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.LedButton.Location = new System.Drawing.Point(319, 38);
            this.LedButton.Name = "LedButton";
            this.LedButton.Size = new System.Drawing.Size(168, 192);
            this.LedButton.TabIndex = 4;
            this.LedButton.Text = "Led";
            this.LedButton.UseVisualStyleBackColor = true;
            this.LedButton.Click += new System.EventHandler(this.LEDButton_Click);
            // 
            // ServoMotorButton
            // 
            this.ServoMotorButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ServoMotorButton.BackgroundImage")));
            this.ServoMotorButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ServoMotorButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.ServoMotorButton.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.ServoMotorButton.Location = new System.Drawing.Point(29, 38);
            this.ServoMotorButton.Name = "ServoMotorButton";
            this.ServoMotorButton.Size = new System.Drawing.Size(244, 192);
            this.ServoMotorButton.TabIndex = 0;
            this.ServoMotorButton.Text = "ServoMotor";
            this.ServoMotorButton.UseVisualStyleBackColor = true;
            this.ServoMotorButton.Click += new System.EventHandler(this.ServoMotorButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ReedSensorTextBox);
            this.groupBox1.Controls.Add(this.ButtonSensorTextBox);
            this.groupBox1.Controls.Add(this.DTHSensorTextBox);
            this.groupBox1.Controls.Add(this.LightSensorTextBox);
            this.groupBox1.Controls.Add(this.reedSensorButton);
            this.groupBox1.Controls.Add(this.buttonSensorButton);
            this.groupBox1.Controls.Add(this.DTHSensorButton);
            this.groupBox1.Controls.Add(this.lightSensorButton);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.Desktop;
            this.groupBox1.Location = new System.Drawing.Point(20, 103);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(461, 488);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sensors";
            // 
            // ReedSensorTextBox
            // 
            this.ReedSensorTextBox.Location = new System.Drawing.Point(287, 409);
            this.ReedSensorTextBox.Multiline = true;
            this.ReedSensorTextBox.Name = "ReedSensorTextBox";
            this.ReedSensorTextBox.ReadOnly = true;
            this.ReedSensorTextBox.Size = new System.Drawing.Size(102, 40);
            this.ReedSensorTextBox.TabIndex = 7;
            this.ReedSensorTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ButtonSensorTextBox
            // 
            this.ButtonSensorTextBox.Location = new System.Drawing.Point(70, 410);
            this.ButtonSensorTextBox.Multiline = true;
            this.ButtonSensorTextBox.Name = "ButtonSensorTextBox";
            this.ButtonSensorTextBox.ReadOnly = true;
            this.ButtonSensorTextBox.Size = new System.Drawing.Size(100, 39);
            this.ButtonSensorTextBox.TabIndex = 6;
            this.ButtonSensorTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // DTHSensorTextBox
            // 
            this.DTHSensorTextBox.Location = new System.Drawing.Point(287, 187);
            this.DTHSensorTextBox.Multiline = true;
            this.DTHSensorTextBox.Name = "DTHSensorTextBox";
            this.DTHSensorTextBox.ReadOnly = true;
            this.DTHSensorTextBox.Size = new System.Drawing.Size(102, 41);
            this.DTHSensorTextBox.TabIndex = 5;
            this.DTHSensorTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // LightSensorTextBox
            // 
            this.LightSensorTextBox.Location = new System.Drawing.Point(70, 187);
            this.LightSensorTextBox.Multiline = true;
            this.LightSensorTextBox.Name = "LightSensorTextBox";
            this.LightSensorTextBox.ReadOnly = true;
            this.LightSensorTextBox.Size = new System.Drawing.Size(100, 41);
            this.LightSensorTextBox.TabIndex = 4;
            this.LightSensorTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // reedSensorButton
            // 
            this.reedSensorButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("reedSensorButton.BackgroundImage")));
            this.reedSensorButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.reedSensorButton.Location = new System.Drawing.Point(251, 258);
            this.reedSensorButton.Name = "reedSensorButton";
            this.reedSensorButton.Size = new System.Drawing.Size(170, 192);
            this.reedSensorButton.TabIndex = 3;
            this.reedSensorButton.Text = "ReedSensor";
            this.reedSensorButton.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.reedSensorButton.UseVisualStyleBackColor = true;
            // 
            // buttonSensorButton
            // 
            this.buttonSensorButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonSensorButton.BackgroundImage")));
            this.buttonSensorButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonSensorButton.Location = new System.Drawing.Point(35, 258);
            this.buttonSensorButton.Name = "buttonSensorButton";
            this.buttonSensorButton.Size = new System.Drawing.Size(170, 192);
            this.buttonSensorButton.TabIndex = 2;
            this.buttonSensorButton.Text = "ButtonSensor";
            this.buttonSensorButton.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.buttonSensorButton.UseVisualStyleBackColor = true;
            // 
            // DTHSensorButton
            // 
            this.DTHSensorButton.BackColor = System.Drawing.SystemColors.ControlDark;
            this.DTHSensorButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("DTHSensorButton.BackgroundImage")));
            this.DTHSensorButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.DTHSensorButton.Location = new System.Drawing.Point(251, 37);
            this.DTHSensorButton.Name = "DTHSensorButton";
            this.DTHSensorButton.Size = new System.Drawing.Size(170, 192);
            this.DTHSensorButton.TabIndex = 1;
            this.DTHSensorButton.Text = "TempSensor";
            this.DTHSensorButton.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.DTHSensorButton.UseVisualStyleBackColor = false;
            // 
            // lightSensorButton
            // 
            this.lightSensorButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("lightSensorButton.BackgroundImage")));
            this.lightSensorButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.lightSensorButton.ForeColor = System.Drawing.SystemColors.InfoText;
            this.lightSensorButton.Location = new System.Drawing.Point(35, 37);
            this.lightSensorButton.Name = "lightSensorButton";
            this.lightSensorButton.Size = new System.Drawing.Size(170, 192);
            this.lightSensorButton.TabIndex = 0;
            this.lightSensorButton.Text = "LightSensor";
            this.lightSensorButton.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lightSensorButton.UseVisualStyleBackColor = true;
            // 
            // startSmartChainButton
            // 
            this.startSmartChainButton.Location = new System.Drawing.Point(542, 12);
            this.startSmartChainButton.Name = "startSmartChainButton";
            this.startSmartChainButton.Size = new System.Drawing.Size(140, 76);
            this.startSmartChainButton.TabIndex = 3;
            this.startSmartChainButton.Text = "Start Smart Chain";
            this.startSmartChainButton.UseVisualStyleBackColor = true;
            this.startSmartChainButton.Click += new System.EventHandler(this.startSmartChainButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(217, 32);
            this.label1.TabIndex = 4;
            this.label1.Text = "Arduino State   :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label2.Location = new System.Drawing.Point(12, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(219, 32);
            this.label2.TabIndex = 5;
            this.label2.Text = "WhiteBox State:";
            // 
            // whiteCubeConnectionStateLabel
            // 
            this.whiteCubeConnectionStateLabel.AutoSize = true;
            this.whiteCubeConnectionStateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.whiteCubeConnectionStateLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.whiteCubeConnectionStateLabel.Location = new System.Drawing.Point(235, 51);
            this.whiteCubeConnectionStateLabel.Name = "whiteCubeConnectionStateLabel";
            this.whiteCubeConnectionStateLabel.Size = new System.Drawing.Size(188, 32);
            this.whiteCubeConnectionStateLabel.TabIndex = 7;
            this.whiteCubeConnectionStateLabel.Text = "Disconnected";
            // 
            // arduinoConnectionStateLabel
            // 
            this.arduinoConnectionStateLabel.AutoSize = true;
            this.arduinoConnectionStateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.arduinoConnectionStateLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.arduinoConnectionStateLabel.Location = new System.Drawing.Point(235, 9);
            this.arduinoConnectionStateLabel.Name = "arduinoConnectionStateLabel";
            this.arduinoConnectionStateLabel.Size = new System.Drawing.Size(188, 32);
            this.arduinoConnectionStateLabel.TabIndex = 6;
            this.arduinoConnectionStateLabel.Text = "Disconnected";
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(929, 12);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(140, 76);
            this.resetButton.TabIndex = 8;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // RunTimeLabel
            // 
            this.RunTimeLabel.AutoSize = true;
            this.RunTimeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.RunTimeLabel.Location = new System.Drawing.Point(706, 56);
            this.RunTimeLabel.Name = "RunTimeLabel";
            this.RunTimeLabel.Size = new System.Drawing.Size(127, 32);
            this.RunTimeLabel.TabIndex = 9;
            this.RunTimeLabel.Text = "00:00:00";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label4.Location = new System.Drawing.Point(706, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(145, 32);
            this.label4.TabIndex = 10;
            this.label4.Text = "Run Time:";
            // 
            // IOTPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1099, 610);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.RunTimeLabel);
            this.Controls.Add(this.resetButton);
            this.Controls.Add(this.whiteCubeConnectionStateLabel);
            this.Controls.Add(this.arduinoConnectionStateLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.startSmartChainButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ActuatorsGroupBox);
            this.ForeColor = System.Drawing.SystemColors.InfoText;
            this.Name = "IOTPanel";
            this.Text = "IOT";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.IOTPanel_FormClosing);
            this.ActuatorsGroupBox.ResumeLayout(false);
            this.ActuatorsGroupBox.PerformLayout();
            this.RGBLedGroupBox.ResumeLayout(false);
            this.RGBLedGroupBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox ActuatorsGroupBox;
        private System.Windows.Forms.Button LedButton;
        private System.Windows.Forms.Button StepMotorButton;
        private System.Windows.Forms.Button ServoMotorButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button reedSensorButton;
        private System.Windows.Forms.Button buttonSensorButton;
        private System.Windows.Forms.Button DTHSensorButton;
        private System.Windows.Forms.Button lightSensorButton;
        private System.Windows.Forms.Button startSmartChainButton;
        private System.Windows.Forms.TextBox LEDTextBox;
        private System.Windows.Forms.TextBox DTHSensorTextBox;
        private System.Windows.Forms.TextBox LightSensorTextBox;
        private System.Windows.Forms.TextBox ReedSensorTextBox;
        private System.Windows.Forms.TextBox ButtonSensorTextBox;
        private System.Windows.Forms.TextBox ServoMotorTextBox;
        private System.Windows.Forms.GroupBox RGBLedGroupBox;
        private System.Windows.Forms.Button WhiteButton;
        private System.Windows.Forms.Button YellowButton;
        private System.Windows.Forms.Button TurnOffButton;
        private System.Windows.Forms.Button GreenButton;
        private System.Windows.Forms.Button PinkButton;
        private System.Windows.Forms.Button BlueButton;
        private System.Windows.Forms.Button AzureButton;
        private System.Windows.Forms.Button RedrButton;
        private System.Windows.Forms.TextBox RgbTextBox;
        private System.Windows.Forms.TextBox StepMotorTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label whiteCubeConnectionStateLabel;
        private System.Windows.Forms.Label arduinoConnectionStateLabel;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.Label RunTimeLabel;
        private System.Windows.Forms.Label label4;
    }
}

