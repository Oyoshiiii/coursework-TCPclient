using CourseworkGameLib;
using System.Net.Security;
using System.Net.Sockets;
using System.Reflection.Metadata;
namespace coursework_TCPclient
{
    public partial class Form1 : Form
    {
        int AutosaveCode = 0;
        string ip = "127.0.0.1";
        int port = 8888;
        bool Cancle = false;
        int playerAnswerCode = 0;
        int plotCode = 0;

        TcpClient player = new TcpClient();
        StreamReader? Reader = null;
        StreamWriter? Writer = null;
        List<Image> images = new List<Image>()
        {

        };
        List<string> plot = new List<string>()
        {
            "*|  Поздравляю, сегодня уже второй рабочий день на твоей новой работе",
            "*|  На улице пасмурно, иногда покрапывает дождь...",
            "*|  Ну ладно, похоже кто-то пришёл, желаю удачи",
            "*|  Готова?",
            "*|  Отлично"
        };

        List<string> playerAnswers = new List<string>()
        {
            "Да",
            "Нет"
        };

        public Form1()
        {
            InitializeComponent();
            AutosaveCode = 1;
            FormClosing += Form1_FormClosing;
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            var msg = MessageBox.Show("Вы точно хотите выйти из игры?", "Выход", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (msg == DialogResult.Yes) { Cancle = true; }
            else { Cancle = false; e.Cancel = false; }
        }
        private async void NewGameButton_Click(object sender, EventArgs e)
        {
            AutosaveCode = 0;
            await TcpPlayer();
            MessageBox.Show("Начата новая игра, все предыдущие автосохранения, если они были, стерты", "Новая игра",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            NewGameButton.Hide();
            ContinueButton.Hide();

            menuText.Visible = true;
            visual.Visible = true;
            textLines.Visible = true;
            AnswersList.Visible = true;
            NextReplika.Visible = true;
            Coctails.Visible = true;

            textLines.Text = plot[0];
        }

        private async void ContinueButton_Click(object sender, EventArgs e)
        {
            AutosaveCode = -1;
            await TcpPlayer();

            if (AutosaveCode != 0)
            {
                NewGameButton.Hide();
                ContinueButton.Hide();

                menuText.Visible = true;
                visual.Visible = true;
                textLines.Visible = true;
                AnswersList.Visible = true;
                NextReplika.Visible = true;
                Coctails.Visible = true;
            }
        }

        private async Task TcpPlayer()
        {
            try
            {
                player.Connect(ip, port);

                Reader = new StreamReader(player.GetStream());
                Writer = new StreamWriter(player.GetStream());


                Task.Run(() => ReceiveServerMessageAsync(Reader));
                await SendServerMessageAsync(Writer);
                //написать дополнительный метод отправки именно последнего кода, после отправки самого первого кода, tcpclient должен отключаться
                //и в конце игры вновь подключаться, отправляя последний код и снова отключаться
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

            while (true)
            {
                if (Cancle == true)
                {
                    await writer.WriteLineAsync(AutosaveCode.ToString());
                    await writer.FlushAsync();
                    break;
                }
                else continue;
            }
        }

        private async Task ReceiveServerMessageAsync(StreamReader reader)
        {
            while (true)
            {
                try
                {
                    int code = Convert.ToInt32(await reader.ReadLineAsync());
                    MessageBox.Show($"{code}");

                    if (code == 0 && AutosaveCode != 0)
                    {
                        MessageBox.Show("У вас нет последних автосохранений", "Ошибка загрузки",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ContinueButton.Enabled = false;
                    }
                }
                catch { break; }
            }
        }

        private void NextReplika_Click(object sender, EventArgs e)
        {
            Game();
        }

        private void Game()
        {
            if (AutosaveCode == 0 && plotCode < 5)
            {
                if (plotCode == 3)
                {
                    NextReplika.Enabled = false;
                    AnswersList.Items.Add(playerAnswers[0]);
                    AnswersList.EndUpdate();

                    while (AnswersList.SelectedIndex != 0)
                    {
                        NextReplika.Enabled = false;
                    }

                    NextReplika.Enabled = true;
                }
                else
                {
                    textLines.Text = plot[plotCode];
                }

                textLines.Update();
                plotCode++;
            }
        }
    }
}
