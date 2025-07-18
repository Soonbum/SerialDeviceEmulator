namespace DeviceEmulator
{
    partial class DeviceEmulatorForm
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
            dataGridView_PortConfiguration = new DataGridView();
            DeviceName = new DataGridViewTextBoxColumn();
            PortName = new DataGridViewTextBoxColumn();
            BaudRate = new DataGridViewTextBoxColumn();
            Parity = new DataGridViewTextBoxColumn();
            DataBits = new DataGridViewTextBoxColumn();
            StopBits = new DataGridViewTextBoxColumn();
            labelPortConfiguration = new Label();
            dataGridView_ProtocolList = new DataGridView();
            DeviceNameAssociated = new DataGridViewTextBoxColumn();
            Description = new DataGridViewTextBoxColumn();
            ReceiveText = new DataGridViewTextBoxColumn();
            SendText = new DataGridViewTextBoxColumn();
            buttonClose = new Button();
            buttonConnect = new Button();
            buttonDisconnect = new Button();
            logText = new RichTextBox();
            buttonClear = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView_PortConfiguration).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView_ProtocolList).BeginInit();
            SuspendLayout();
            // 
            // dataGridView_PortConfiguration
            // 
            dataGridView_PortConfiguration.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView_PortConfiguration.Columns.AddRange(new DataGridViewColumn[] { DeviceName, PortName, BaudRate, Parity, DataBits, StopBits });
            dataGridView_PortConfiguration.Location = new Point(12, 35);
            dataGridView_PortConfiguration.Name = "dataGridView_PortConfiguration";
            dataGridView_PortConfiguration.Size = new Size(756, 247);
            dataGridView_PortConfiguration.TabIndex = 0;
            // 
            // DeviceName
            // 
            DeviceName.HeaderText = "장비 이름";
            DeviceName.Name = "DeviceName";
            DeviceName.Width = 200;
            // 
            // PortName
            // 
            PortName.HeaderText = "포트번호";
            PortName.Name = "PortName";
            // 
            // BaudRate
            // 
            BaudRate.HeaderText = "Baud Rate";
            BaudRate.Name = "BaudRate";
            // 
            // Parity
            // 
            Parity.HeaderText = "Parity";
            Parity.Name = "Parity";
            // 
            // DataBits
            // 
            DataBits.HeaderText = "DataBits";
            DataBits.Name = "DataBits";
            // 
            // StopBits
            // 
            StopBits.HeaderText = "StopBits";
            StopBits.Name = "StopBits";
            // 
            // labelPortConfiguration
            // 
            labelPortConfiguration.AutoSize = true;
            labelPortConfiguration.Location = new Point(12, 9);
            labelPortConfiguration.Name = "labelPortConfiguration";
            labelPortConfiguration.Size = new Size(59, 15);
            labelPortConfiguration.TabIndex = 1;
            labelPortConfiguration.Text = "포트 설정";
            // 
            // dataGridView_ProtocolList
            // 
            dataGridView_ProtocolList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView_ProtocolList.Columns.AddRange(new DataGridViewColumn[] { DeviceNameAssociated, Description, ReceiveText, SendText });
            dataGridView_ProtocolList.Location = new Point(12, 297);
            dataGridView_ProtocolList.Name = "dataGridView_ProtocolList";
            dataGridView_ProtocolList.Size = new Size(948, 400);
            dataGridView_ProtocolList.TabIndex = 2;
            // 
            // DeviceNameAssociated
            // 
            DeviceNameAssociated.HeaderText = "연관된 장비 이름";
            DeviceNameAssociated.Name = "DeviceNameAssociated";
            DeviceNameAssociated.Width = 200;
            // 
            // Description
            // 
            Description.HeaderText = "Description";
            Description.Name = "Description";
            Description.Width = 200;
            // 
            // ReceiveText
            // 
            ReceiveText.HeaderText = "ReceiveText";
            ReceiveText.Name = "ReceiveText";
            ReceiveText.Width = 250;
            // 
            // SendText
            // 
            SendText.HeaderText = "SendText";
            SendText.Name = "SendText";
            SendText.Width = 250;
            // 
            // buttonClose
            // 
            buttonClose.Location = new Point(870, 97);
            buttonClose.Name = "buttonClose";
            buttonClose.Size = new Size(90, 25);
            buttonClose.TabIndex = 3;
            buttonClose.Text = "닫기";
            buttonClose.UseVisualStyleBackColor = true;
            buttonClose.Click += buttonClose_Click;
            // 
            // buttonConnect
            // 
            buttonConnect.Location = new Point(870, 35);
            buttonConnect.Name = "buttonConnect";
            buttonConnect.Size = new Size(90, 25);
            buttonConnect.TabIndex = 4;
            buttonConnect.Text = "통신 연결";
            buttonConnect.UseVisualStyleBackColor = true;
            buttonConnect.Click += buttonConnect_Click;
            // 
            // buttonDisconnect
            // 
            buttonDisconnect.Location = new Point(870, 66);
            buttonDisconnect.Name = "buttonDisconnect";
            buttonDisconnect.Size = new Size(90, 25);
            buttonDisconnect.TabIndex = 5;
            buttonDisconnect.Text = "통신 끊기";
            buttonDisconnect.UseVisualStyleBackColor = true;
            buttonDisconnect.Click += buttonDisconnect_Click;
            // 
            // logText
            // 
            logText.Location = new Point(774, 128);
            logText.Name = "logText";
            logText.Size = new Size(186, 154);
            logText.TabIndex = 6;
            logText.Text = "";
            // 
            // buttonClear
            // 
            buttonClear.Location = new Point(774, 97);
            buttonClear.Name = "buttonClear";
            buttonClear.Size = new Size(90, 25);
            buttonClear.TabIndex = 7;
            buttonClear.Text = "Clear";
            buttonClear.UseVisualStyleBackColor = true;
            buttonClear.Click += buttonClear_Click;
            // 
            // DeviceEmulatorForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(984, 842);
            Controls.Add(buttonClear);
            Controls.Add(logText);
            Controls.Add(buttonDisconnect);
            Controls.Add(buttonConnect);
            Controls.Add(buttonClose);
            Controls.Add(dataGridView_ProtocolList);
            Controls.Add(labelPortConfiguration);
            Controls.Add(dataGridView_PortConfiguration);
            Name = "DeviceEmulatorForm";
            Text = "Serial Device Emulator";
            FormClosing += DeviceEmulatorForm_FormClosing;
            ((System.ComponentModel.ISupportInitialize)dataGridView_PortConfiguration).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView_ProtocolList).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView_PortConfiguration;
        private Label labelPortConfiguration;
        private DataGridView dataGridView_ProtocolList;
        private Button buttonClose;
        private DataGridViewTextBoxColumn DeviceName;
        private DataGridViewTextBoxColumn PortName;
        private DataGridViewTextBoxColumn BaudRate;
        private DataGridViewTextBoxColumn Parity;
        private DataGridViewTextBoxColumn DataBits;
        private DataGridViewTextBoxColumn StopBits;
        private DataGridViewTextBoxColumn DeviceNameAssociated;
        private DataGridViewTextBoxColumn Description;
        private DataGridViewTextBoxColumn ReceiveText;
        private DataGridViewTextBoxColumn SendText;
        private Button buttonConnect;
        private Button buttonDisconnect;
        private RichTextBox logText;
        private Button buttonClear;
    }
}
