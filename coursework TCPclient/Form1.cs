using CourseworkGameLib;
using System.Net.Sockets;
namespace coursework_TCPclient
{
    public partial class Form1 : Form
    {
        int AutosaveCode;
        string ip = "127.0.0.1";
        int port = 8888;
        bool Cancle = false;
        
        TcpClient player = new TcpClient();
        StreamReader? Reader = null;
        StreamWriter? Writer = null;

        public Form1()
        {
            InitializeComponent();
            AutosaveCode = 1;
            FormClosing += Form1_FormClosing;
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            var msg = MessageBox.Show("�� ����� ������ ����� �� ����?", "�����", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (msg == DialogResult.Yes) { Cancle = true; e.Cancel = true; }
            else { Cancle = false; }
        }
        private async void NewGameButton_Click(object sender, EventArgs e)
        {
            AutosaveCode = 0;
            await TcpPlayer();
            /*
             ���������� ����� TCPclient � ��������������� ���������� � ��������
            �� ������ ������������ ��������� 0 � ������ ������� ������ ��������������, ������������ ������� ���
            �������������� 0, 
            ����� ���������� �� ���������� GameLibrary ����������� ����� ������ GameLine
            ����� �������� ���������, �� ������ ������������ ��������� 
             */
        }

        private void ContinueButton_Click(object sender, EventArgs e)
        {
            /*
             ���������� ����� TCPclient � ��������������� ���������� � ��������
            �� ������ ������������ ��������� '-1'
            ���� ������ �������������� ����, �� ������ ���������� ��������� 'noautosaves' � ���������� �����������
            messageBox � ���������� � ���, ��� ��� ��������� ��������������
            ���� ������ �������������� �� ����, ������ ���������� ��� ���������� ��������������, ������� ��������� � ����������
            AutosaveCode
            ����� ���������� �� ���������� GameLibrary ����������� ����� ������ GameLine
             */
        }

        private async Task TcpPlayer()
        {
            try
            {
                player.Connect(ip, port);

                Reader = new StreamReader(player.GetStream());
                Writer = new StreamWriter(player.GetStream());

                if (Writer == null || Reader == null) return;

                Task.Run(() => ReceiveServerMessageAsync(Reader));
                await SendServerMessageAsync(Writer);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Writer?.Close();
            Reader?.Close();
        }

        private async Task SendServerMessageAsync(StreamWriter writer)
        {
            await writer.WriteLineAsync(AutosaveCode.ToString());
            await writer.FlushAsync();

            if (Cancle == true)
            {
                await writer.WriteLineAsync(AutosaveCode.ToString());
                await writer.FlushAsync();
            }
        }

        private async Task ReceiveServerMessageAsync(StreamReader reader)
        {
            while(true)
            {
                try
                {
                    AutosaveCode = Convert.ToInt32(await reader.ReadLineAsync());
                }
                catch { break; }
            }
        }
    }
}
