using System.Data;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System;

// �߰������� �غ��ؾ� �� ��
// Virtual Serial Ports Emulator�� ��ġ�ϰ� Virtual Connector�� ���� �ø��� ��Ʈ�� �����ؾ� ��

namespace DeviceEmulator
{
    public partial class DeviceEmulatorForm : Form
    {
        int rowCount = -1;
        List<SerialPortEmulator> serialPortEmulators = new List<SerialPortEmulator>();
        List<Thread> emulatorThreads = new List<Thread>();

        public DeviceEmulatorForm()
        {
            InitializeComponent();

            RestrictButtons(false);

            // 1��° DataGridView
            dataGridView_PortConfiguration.Columns["DeviceName"].ValueType = typeof(string);
            dataGridView_PortConfiguration.Columns["PortName"].ValueType = typeof(string);
            dataGridView_PortConfiguration.Columns["BaudRate"].ValueType = typeof(int);
            dataGridView_PortConfiguration.Columns["Parity"].ValueType = typeof(int);
            dataGridView_PortConfiguration.Columns["DataBits"].ValueType = typeof(int);
            dataGridView_PortConfiguration.Columns["StopBits"].ValueType = typeof(int);

            dataGridView_PortConfiguration.Rows.Add("FirmwareController", "COM6", 115200, (int)System.IO.Ports.Parity.None, 8, (int)System.IO.Ports.StopBits.One);
            //dataGridView_PortConfiguration.Rows.Add("WaterLevelChecker", "COM6", 38400, (int)System.IO.Ports.Parity.None, 8, (int)System.IO.Ports.StopBits.One);

            // 2��° DataGridView
            dataGridView_ProtocolList.Columns["DeviceNameAssociated"].ValueType = typeof(string);
            dataGridView_ProtocolList.Columns["Description"].ValueType = typeof(string);
            dataGridView_ProtocolList.Columns["ReceiveText"].ValueType = typeof(string);
            dataGridView_ProtocolList.Columns["SendText"].ValueType = typeof(string);

            dataGridView_ProtocolList.Rows.Add("FirmwareController", "��� ����", "7E11", "succ");
            dataGridView_ProtocolList.Rows.Add("FirmwareController", "������ ����", "7E12", "0.0");
            dataGridView_ProtocolList.Rows.Add("FirmwareController", "������ �ӵ� ����", "7E13", "succ");
            dataGridView_ProtocolList.Rows.Add("FirmwareController", "������ �̵� (Feed = 0)", "7E10", "succ");
            dataGridView_ProtocolList.Rows.Add("FirmwareController", "������ �̵� (Feed = 1)", "7E10", "succ");
            dataGridView_ProtocolList.Rows.Add("FirmwareController", "������ ��ġ", "7E11", "0.0");
            dataGridView_ProtocolList.Rows.Add("FirmwareController", "���� ��ũ ����", "7E06", "succ");
            dataGridView_ProtocolList.Rows.Add("FirmwareController", "���� ��ũ �ӵ� ����", "7E07", "succ");
            dataGridView_ProtocolList.Rows.Add("FirmwareController", "���� ��ũ �̵� (Feed = 0)", "7E03", "succ");
            dataGridView_ProtocolList.Rows.Add("FirmwareController", "���� ��ũ �̵� (Feed = 1)", "7E03", "succ");
            dataGridView_ProtocolList.Rows.Add("FirmwareController", "���� ��ũ ��ġ", "7E05", "0.0");
            dataGridView_ProtocolList.Rows.Add("FirmwareController", "����Ʈ ���̵� �ӵ� ����", "7E24", "2");
            dataGridView_ProtocolList.Rows.Add("FirmwareController", "����Ʈ ���̵� ������ (Feed = 0)", "7E25", "0");
            dataGridView_ProtocolList.Rows.Add("FirmwareController", "����Ʈ ���̵� ������ (Feed = 1)", "7E27", "0");
            dataGridView_ProtocolList.Rows.Add("FirmwareController", "����Ʈ ���̵� �ڷ� (Feed = 0)", "7E26", "1");
            dataGridView_ProtocolList.Rows.Add("FirmwareController", "����Ʈ ���̵� �ڷ� (Feed = 1)", "7E28", "1");
            dataGridView_ProtocolList.Rows.Add("FirmwareController", "�÷�Ʈ ���̵� �ӵ� ����", "7E34", "2");
            dataGridView_ProtocolList.Rows.Add("FirmwareController", "�÷�Ʈ ���̵� ������ (Feed = 0)", "7E35", "0");
            dataGridView_ProtocolList.Rows.Add("FirmwareController", "�÷�Ʈ ���̵� ������ (Feed = 1)", "7E37", "0");
            dataGridView_ProtocolList.Rows.Add("FirmwareController", "�÷�Ʈ ���̵� �ڷ� (Feed = 0)", "7E36", "1");
            dataGridView_ProtocolList.Rows.Add("FirmwareController", "�÷�Ʈ ���̵� �ڷ� (Feed = 1)", "7E38", "1");

            //dataGridView_ProtocolList.Rows.Add("WaterLevelChecker", "��� ����", "%01#RMD**\r", "%03$RMD-0123456**\r");  // -0123456 : -12.3456mm ��� �� ?
        }

        public void LogWriteLine(string message)
        {
            this.logText.Text += message + "\n";
        }

        public void RestrictButtons(bool isConnected)
        {
            if (isConnected)
            {
                this.buttonConnect.Enabled = false;
                this.buttonDisconnect.Enabled = true;
            }
            else
            {
                this.buttonConnect.Enabled = true;
                this.buttonDisconnect.Enabled = false;
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            buttonDisconnect.PerformClick();
            this.Close();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            RestrictButtons(true);

            rowCount = -1;

            for (int i = 0; i < dataGridView_PortConfiguration.RowCount; i++)
            {
                if ((string)dataGridView_PortConfiguration.Rows[i].Cells[1].Value != "")
                    rowCount++;
            }

            // ��� ����
            for (int i = 0; i < rowCount; i++)
            {
                string portName = (string)dataGridView_PortConfiguration.Rows[i].Cells[1].Value;
                int baudRate = (int)dataGridView_PortConfiguration.Rows[i].Cells[2].Value;
                Parity parity = (Parity)dataGridView_PortConfiguration.Rows[i].Cells[3].Value;
                int dataBits = (int)dataGridView_PortConfiguration.Rows[i].Cells[4].Value;
                StopBits stopBits = (StopBits)dataGridView_PortConfiguration.Rows[i].Cells[5].Value;

                SerialPortEmulator newEmulator = new(this, portName, baudRate, parity, dataBits, stopBits);
                newEmulator.SetDataGridView(dataGridView_PortConfiguration, dataGridView_ProtocolList);

                Thread newThread = new Thread(newEmulator.Start);
                newThread.Start();

                serialPortEmulators.Add(newEmulator);
                emulatorThreads.Add(newThread);
            }
        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            RestrictButtons(false);

            // ��� ����
            for (int i = 0; i < rowCount; i++)
            {
                serialPortEmulators[i].Stop();
                emulatorThreads[i].Join();
            }

            serialPortEmulators.Clear();
            emulatorThreads.Clear();
        }

        private void DeviceEmulatorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            buttonDisconnect.PerformClick();
            this.Close();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            logText.Text = "";
        }
    }

    public class SerialPortEmulator
    {
        private DeviceEmulatorForm mainForm;

        int rowCount_PortConfiguration = -1;
        int rowCount_ProtocolList = -1;

        private SerialPort serialPort;
        private bool isRunning = false;
        private List<KeyValuePair<string, string>> commandSet = new List<KeyValuePair<string, string>>();
        private string foundDeviceName;

        public SerialPortEmulator(DeviceEmulatorForm mainForm, string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            this.mainForm = mainForm;
            serialPort = new(portName, baudRate, parity, dataBits, stopBits);
        }

        public void SetDataGridView(DataGridView dataGridView_PortConfiguration, DataGridView dataGridView_ProtocolList)
        {
            for (int i = 0; i < dataGridView_PortConfiguration.RowCount; i++)
            {
                if ((string)dataGridView_PortConfiguration.Rows[i].Cells[1].Value != "")
                    rowCount_PortConfiguration++;
            }

            for (int i = 0; i < dataGridView_ProtocolList.RowCount; i++)
            {
                if ((string)dataGridView_ProtocolList.Rows[i].Cells[0].Value != "")
                    rowCount_ProtocolList++;
            }

            // serialPort.PortName�� ���� DeviceName�� ã��
            for (int i = 0; i < rowCount_PortConfiguration; i++)
            {
                if ((string)dataGridView_PortConfiguration.Rows[i].Cells[1].Value == serialPort.PortName)
                    foundDeviceName = (string)dataGridView_PortConfiguration.Rows[i].Cells[0].Value;
            }

            // �ش� DeviceName�� Ŀ�ǵ� ������ �����ؼ� ������
            for (int i = 0; i < rowCount_ProtocolList; i++)
            {
                if ((string)dataGridView_ProtocolList.Rows[i].Cells[0].Value == foundDeviceName)
                {
                    // dataGridView_ProtocolList���� ������ DeviceName�� row�� ã�Ƽ� KeyValuePair<ReceiveText, SendText>�� commandSet�� �߰���
                    commandSet.Add(new KeyValuePair<string, string>((string)dataGridView_ProtocolList.Rows[i].Cells[2].Value, (string)dataGridView_ProtocolList.Rows[i].Cells[3].Value));
                }
            }
        }

        public void Start()
        {
            serialPort.DataReceived += SerialPort_DataReceived;

            serialPort.Open();
            isRunning = true;

            while (isRunning)
            {
                Thread.Sleep(100);
            }
        }

        private static StringBuilder buffer = new StringBuilder();

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (serialPort.BytesToRead > 0)
            {
                string receivedData = serialPort.ReadExisting();
                buffer.Append(receivedData);

                if (receivedData.EndsWith("\r") || receivedData.EndsWith("\n") || receivedData.EndsWith("\r\n") || receivedData.EndsWith("\0"))
                {
                    string message = buffer.ToString().Trim().ToUpper();

                    // ���� �����͸� ������� ���� ����
                    string responseData = ProcessReceivedData(message);

                    // ���� ����
                    serialPort.Write(responseData + "\r\n");

                    mainForm.Invoke(new Action(() =>
                    {
                        mainForm.LogWriteLine($"{message}: {responseData}");
                    }));

                    // ���� �ʱ�ȭ
                    buffer.Clear();
                }
            }
        }

        private string ProcessReceivedData(string receivedData)
        {
            for (int i = 0; i < commandSet.Count; i++)
            {
                if (receivedData.Trim().StartsWith(commandSet[i].Key))
                    return commandSet[i].Value;
            }

            return "ERROR";
        }

        public void Stop()
        {
            isRunning = false;

            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
                serialPort.Dispose();
            }
        }

        public static string MakeCommandWithChecksum(string dataPart)
        {
            if (string.IsNullOrEmpty(dataPart))
                return string.Empty;

            // �빮�ڷ� ����
            dataPart = dataPart.ToUpper();

            byte xorVal = 0;

            // XOR ���
            foreach (char c in dataPart)
            {
                xorVal ^= (byte)c;
            }

            // 2�ڸ� 16������ ��ȯ
            string checksumStr = xorVal.ToString("X2");

            // ���� ��� + ��� ����� ���� ��ȯ
            return dataPart + checksumStr;
        }
    }
}
