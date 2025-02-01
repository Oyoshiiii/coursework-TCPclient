using CourseworkGameLib;
using System.Net.Security;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Text;
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
        private async void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            var msg = MessageBox.Show("Вы точно хотите выйти из игры?", "Выход", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (msg == DialogResult.Yes)
            {
                try
                {
                    TcpClient player = new TcpClient(ip, port);
                    NetworkStream stream = player.GetStream();

                    byte[] data = Encoding.UTF8.GetBytes(AutosaveCode.ToString());
                    stream.Write(data, 0, data.Length);

                    player.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при закрытии: " + ex.Message);
                }
            }
            else { Cancle = false; e.Cancel = false; }
        }
        private void NewGameButton_Click(object sender, EventArgs e)
        {
            AutosaveCode = 0;
            SendCodeToServerAsync();
        }

        private void ContinueButton_Click(object sender, EventArgs e)
        {
            AutosaveCode = -1;
            SendCodeToServerAsync();
        }
        
        private void SendCodeToServerAsync()
        {
            try
            {
                TcpClient player = new TcpClient(ip, port);
                NetworkStream stream = player.GetStream();

                byte[] code = Encoding.UTF8.GetBytes(AutosaveCode.ToString());
                stream.Write(code, 0, code.Length);
                
                if (AutosaveCode == 0)
                {
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

                else
                {
                    byte[] data = new byte[256];
                    int bytesRead = stream.Read(data, 0, data.Length);
                    int lastCode = Convert.ToInt32(Encoding.UTF8.GetString(data, 0, bytesRead));

                    if (lastCode == 0)
                    {
                        MessageBox.Show("У вас нет последних автосохранений", "Ошибка загрузки",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ContinueButton.Enabled = false;
                    }

                    else
                    {
                        AutosaveCode = lastCode;
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

                player.Close();
            }
            catch(Exception ex) 
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
