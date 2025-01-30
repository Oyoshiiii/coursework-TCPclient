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
            var msg = MessageBox.Show("Вы точно хотите выйти из игры?", "Выход", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (msg == DialogResult.Yes) { Cancle = true; e.Cancel = true; }
            else { Cancle = false; }
        }
        private async void NewGameButton_Click(object sender, EventArgs e)
        {
            NewGameButton.Hide();
            ContinueButton.Hide();

            visual.Visible = true;
            textLines.Visible = true;
            AnswersList.Visible = true;
            NextReplika.Visible = true;
            
            AutosaveCode = 0;
            await TcpPlayer();
            MessageBox.Show("Начата новая игра, все предыдущие автосохранения, если они были, стерты", "Новая игра",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async Task ContinueButton_Click(object sender, EventArgs e)
        {
            AutosaveCode = -1;
            await TcpPlayer();

            if (AutosaveCode == 0)
            {
                MessageBox.Show("У вас нет последних автосохранений", "Ошибка загрузки", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                ContinueButton.Enabled = false;
            }

            else
            {
                NewGameButton.Hide();
                ContinueButton.Hide();

                visual.Visible = true;
                textLines.Visible = true;
                AnswersList.Visible = true;
                NextReplika.Visible = true;
            }
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            while (true)
            {
                try
                {
                    int code = AutosaveCode;
                    AutosaveCode = Convert.ToInt32(await reader.ReadLineAsync());
                    if (AutosaveCode !=  code)
                    {
                        break;
                    }
                    while (true)
                    {
                        AutosaveCode = Convert.ToInt32(await reader.ReadLineAsync());
                    }
                }
                catch { break; }
            }
        }

        private void NextReplika_Click(object sender, EventArgs e)
        {

        }
    }
}
