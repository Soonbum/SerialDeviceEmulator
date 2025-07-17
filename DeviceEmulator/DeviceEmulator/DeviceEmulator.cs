using System.Data;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System;

// 추가적으로 준비해야 할 것
// Virtual Serial Ports Emulator를 설치하고 Virtual Connector로 가상 시리얼 포트를 생성해야 함

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

            // 1번째 DataGridView
            dataGridView_PortConfiguration.Columns["DeviceName"].ValueType = typeof(string);
            dataGridView_PortConfiguration.Columns["PortName"].ValueType = typeof(string);
            dataGridView_PortConfiguration.Columns["BaudRate"].ValueType = typeof(int);
            dataGridView_PortConfiguration.Columns["Parity"].ValueType = typeof(int);
            dataGridView_PortConfiguration.Columns["DataBits"].ValueType = typeof(int);
            dataGridView_PortConfiguration.Columns["StopBits"].ValueType = typeof(int);

            dataGridView_PortConfiguration.Rows.Add("FirmwareController", "COM6", 115200, (int)System.IO.Ports.Parity.None, 8, (int)System.IO.Ports.StopBits.One);
            //dataGridView_PortConfiguration.Rows.Add("WaterLevelChecker", "COM6", 38400, (int)System.IO.Ports.Parity.None, 8, (int)System.IO.Ports.StopBits.One);

            // 2번째 DataGridView
            dataGridView_ProtocolList.Columns["DeviceNameAssociated"].ValueType = typeof(string);
            dataGridView_ProtocolList.Columns["Description"].ValueType = typeof(string);
            dataGridView_ProtocolList.Columns["ReceiveText"].ValueType = typeof(string);
            dataGridView_ProtocolList.Columns["SendText"].ValueType = typeof(string);

            dataGridView_ProtocolList.Rows.Add("FirmwareController", "통신 연결", "7E11", "succ");
            dataGridView_ProtocolList.Rows.Add("FirmwareController", "조형판 리셋", "7E12", "0.0");
            dataGridView_ProtocolList.Rows.Add("FirmwareController", "조형판 속도 설정", "7E13", "succ");
            dataGridView_ProtocolList.Rows.Add("FirmwareController", "조형판 이동 (Feed = 0)", "7E10", "succ");
            dataGridView_ProtocolList.Rows.Add("FirmwareController", "조형판 이동 (Feed = 1)", "7E10", "succ");
            dataGridView_ProtocolList.Rows.Add("FirmwareController", "조형판 위치", "7E11", "0.0");
            dataGridView_ProtocolList.Rows.Add("FirmwareController", "레벨 탱크 리셋", "7E06", "succ");
            dataGridView_ProtocolList.Rows.Add("FirmwareController", "레벨 탱크 속도 설정", "7E07", "succ");
            dataGridView_ProtocolList.Rows.Add("FirmwareController", "레벨 탱크 이동 (Feed = 0)", "7E03", "succ");
            dataGridView_ProtocolList.Rows.Add("FirmwareController", "레벨 탱크 이동 (Feed = 1)", "7E03", "succ");
            dataGridView_ProtocolList.Rows.Add("FirmwareController", "레벨 탱크 위치", "7E05", "0.0");
            dataGridView_ProtocolList.Rows.Add("FirmwareController", "프린트 블레이드 속도 설정", "7E24", "2");
            dataGridView_ProtocolList.Rows.Add("FirmwareController", "프린트 블레이드 앞으로 (Feed = 0)", "7E25", "0");
            dataGridView_ProtocolList.Rows.Add("FirmwareController", "프린트 블레이드 앞으로 (Feed = 1)", "7E27", "0");
            dataGridView_ProtocolList.Rows.Add("FirmwareController", "프린트 블레이드 뒤로 (Feed = 0)", "7E26", "1");
            dataGridView_ProtocolList.Rows.Add("FirmwareController", "프린트 블레이드 뒤로 (Feed = 1)", "7E28", "1");
            dataGridView_ProtocolList.Rows.Add("FirmwareController", "컬렉트 블레이드 속도 설정", "7E34", "2");
            dataGridView_ProtocolList.Rows.Add("FirmwareController", "컬렉트 블레이드 앞으로 (Feed = 0)", "7E35", "0");
            dataGridView_ProtocolList.Rows.Add("FirmwareController", "컬렉트 블레이드 앞으로 (Feed = 1)", "7E37", "0");
            dataGridView_ProtocolList.Rows.Add("FirmwareController", "컬렉트 블레이드 뒤로 (Feed = 0)", "7E36", "1");
            dataGridView_ProtocolList.Rows.Add("FirmwareController", "컬렉트 블레이드 뒤로 (Feed = 1)", "7E38", "1");

            //dataGridView_ProtocolList.Rows.Add("WaterLevelChecker", "통신 연결", "%01#RMD**\r", "%03$RMD-0123456**\r");  // -0123456 : -12.3456mm 라는 뜻 ?
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

            // 통신 연결
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

            // 통신 해제
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

            // serialPort.PortName을 가진 DeviceName을 찾음
            for (int i = 0; i < rowCount_PortConfiguration; i++)
            {
                if ((string)dataGridView_PortConfiguration.Rows[i].Cells[1].Value == serialPort.PortName)
                    foundDeviceName = (string)dataGridView_PortConfiguration.Rows[i].Cells[0].Value;
            }

            // 해당 DeviceName의 커맨드 정보를 추출해서 저장함
            for (int i = 0; i < rowCount_ProtocolList; i++)
            {
                if ((string)dataGridView_ProtocolList.Rows[i].Cells[0].Value == foundDeviceName)
                {
                    // dataGridView_ProtocolList에서 동일한 DeviceName인 row를 찾아서 KeyValuePair<ReceiveText, SendText>를 commandSet에 추가함
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

                    // 받은 데이터를 기반으로 응답 생성
                    string responseData = ProcessReceivedData(message);

                    // 응답 전송
                    serialPort.Write(responseData + "\r\n");

                    mainForm.Invoke(new Action(() =>
                    {
                        mainForm.LogWriteLine($"{message}: {responseData}");
                    }));

                    // 버퍼 초기화
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

            // 대문자로 통일
            dataPart = dataPart.ToUpper();

            byte xorVal = 0;

            // XOR 계산
            foreach (char c in dataPart)
            {
                xorVal ^= (byte)c;
            }

            // 2자리 16진수로 변환
            string checksumStr = xorVal.ToString("X2");

            // 기존 명령 + 계산 결과를 합쳐 반환
            return dataPart + checksumStr;
        }
    }
}
