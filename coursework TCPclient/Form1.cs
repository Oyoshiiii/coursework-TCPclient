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
        bool recipie = false;
        bool advice = false;

        string ip = "127.0.0.1";
        int port = 8888;
        int plotCode = 0;

        Lilith lilith = new Lilith();
        Peter peter = new Peter();

        List<string> coctails = null;
        List<string> recipies = null;
        List<string> images = new List<string>
        {
            "bar.jpg"
        };
        List<string> plot = new List<string>()
        {
            "*|  Поздравляю, сегодня уже второй рабочий день на твоей новой работе",
            "*|  На улице пасмурно, иногда покрапывает дождь...",
            "*|  Ну ладно, похоже кто-то пришёл, желаю удачи",
            "*|  Готова?",
            "*|  Отлично",
            "* к барной стойке подошла высокая девушка, которая попутно закрывала свой зонтик *",
            "* девушка ушла и вы смогли какое-то время побыть наедине с самой собой *",
            "...",
            "* спустя двадцать минут в бар зашел промокший от дождя парень, который был явно не в духе *"
        };

        List<string> playerAnswers = new List<string>()
        {
            //0 1
            "Да",
            "Нет",
            //2 3
            "А как обычно - это как?..",
            "Конечно, как обычно, каждый день такое готовлю",
            //4 5
            "Ничего страшного",
            "Ну, смешно однако...",
            //6 7
            "Хорошо, сейчас сделаем",
            "Простите, может вы еще вспомните пару деталей коктейля?",
            // 8
            "...может.. ещё детали вспомните?..."
            
        };



        public Form1()
        {
            InitializeComponent();
            coctails = new List<string>()
            {
                lilith.Coctails[0],
                "Кисельные грёзы",
                peter.Coctails[0]
            };
            recipies = new List<string>()
            {
                lilith.Recipies[0],
                "Альдегид, лаймовый сироп",
                peter.Recipies[0]
            };
        }
        private async void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((Coctails.Enabled || AnswersList.SelectedIndex != -1) && AutosaveCode != 0 && AutosaveCode != -1)
            {
                MessageBox.Show("Сначала перейдите к следующей реплике", "Ошибка автосохранения", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                e.Cancel = true;
            }
            else
            {
                var msg = MessageBox.Show("Вы точно хотите выйти из игры?", "Выход", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (msg == DialogResult.Yes)
                {
                    if (AutosaveCode == 0 || AutosaveCode == -1)
                    {
                        e.Cancel = false;
                    }
                    else
                    {
                        try
                        {
                            TcpClient player = new TcpClient(ip, port);
                            NetworkStream stream = player.GetStream();

                            if (AutosaveCode < 0) { AutosaveCode *= -1; }

                            string sendCode = $"{AutosaveCode}";
                            if (recipie)
                            {
                                sendCode += "00";
                            }

                            byte[] data = Encoding.UTF8.GetBytes(AutosaveCode.ToString());
                            stream.Write(data, 0, data.Length);

                            player.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ошибка при закрытии: " + ex.Message);
                        }
                    }
                }
                else { e.Cancel = true; }
            }
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

                    visual.Image = Image.FromFile(images[0]);
                    visual.Update();

                    MessageBox.Show("Данная игра создана по мотивам игры Va 11 Hall-a\nВы играете от лица барменши, которая" +
                        " только устроилась в бар", "Вступление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MessageBox.Show("Приятной вам игры!", "Вступление", MessageBoxButtons.OK);

                    textLines.Text = "Нажми на кнопку ->";
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
                        int HasGift = lastCode / 100;
                        if (HasGift != 0) { recipie = true; lastCode /= 100; }
                        AutosaveCode = lastCode;

                        NewGameButton.Hide();
                        ContinueButton.Hide();

                        menuText.Visible = true;
                        visual.Visible = true;
                        textLines.Visible = true;
                        AnswersList.Visible = true;
                        NextReplika.Visible = true;
                        Coctails.Visible = true;

                        textLines.Text = "Нажми на кнопку ->";
                    }
                }

                player.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void NextReplika_Click(object sender, EventArgs e)
        {
            if (AutosaveCode == 0) { StartGame(); }
            else if (AutosaveCode < 16) { GamePartLilith(); }
            else if (AutosaveCode == 17 && plotCode < 9) { GameMiddlePart(); }
            else { GamePartPeter(); }
        }

        private void AnswersList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AnswersList.SelectedIndex != -1)
            {
                NextReplika.Enabled = true;
            }

            if (AutosaveCode == -1 && AnswersList.Items.Count > 0)
            {
                if (AnswersList.SelectedIndex == 0) { AutosaveCode = 2; }
                if (AnswersList.SelectedIndex == 1) { AutosaveCode = 3; }
            }

            if (AutosaveCode == -4 && AnswersList.Items.Count > 0)
            {
                if (AnswersList.SelectedIndex == 0) { AutosaveCode = 5; }
                if (AnswersList.SelectedIndex == 1) { AutosaveCode = 6; }
            }

            if ((AutosaveCode == -5 || AutosaveCode == 7 || AutosaveCode == 9 || AutosaveCode == 15) && AnswersList.Items.Count > 0)
            {
                if (AnswersList.SelectedIndex == 0)
                {
                    Coctails.Enabled = true;
                    NextReplika.Enabled = false;
                    recipie = true;
                    AutosaveCode = -5;
                }
                if (AnswersList.SelectedIndex == 1)
                {
                    Coctails.Enabled = false;
                    Coctails.SelectedIndex = -1;
                    NextReplika.Enabled = true;
                    recipie = false;
                    AutosaveCode = 7;
                }
            }

            if (AutosaveCode == -7 && AnswersList.Items.Count > 0)
            {
                if (AnswersList.SelectedIndex == 0)
                {
                    Coctails.Enabled = true;
                    NextReplika.Enabled = false;
                    advice = true;
                    AutosaveCode = -7;
                }
                if (AnswersList.SelectedIndex == 1)
                {
                    Coctails.Enabled = false;
                    NextReplika.Enabled = true;
                    advice = false;
                    Coctails.SelectedIndex = -1;
                    AutosaveCode = 8;
                }
            }
        }

        private void Coctails_SelectedIndexChanged(Object sender, EventArgs e)
        {
            if (Coctails.Enabled)
            {
                if (Coctails.SelectedIndex == -1)
                {
                    NextReplika.Enabled = false;
                }
                else NextReplika.Enabled = true;
            }
            else Coctails.SelectedIndex = -1;
            if (Coctails.SelectedIndex == 0)
            {
                MessageBox.Show(recipies[0], "Описание коктейля", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (AutosaveCode < 16) { AutosaveCode = 9; }
            }
            if (Coctails.SelectedIndex == 1)
            {
                MessageBox.Show(recipies[1], "Описание коктейля", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (AutosaveCode < 16) { AutosaveCode = 15; }
            }
            if (Coctails.SelectedIndex == 2)
            {
                MessageBox.Show(recipies[2], "Описание коктейля", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (AutosaveCode < 16) { AutosaveCode = 15; }
            }
            if (Coctails.SelectedIndex == 3)
            {
                MessageBox.Show(recipies[3], "Описание коктейля", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        private void StartGame()
        {
            if (plotCode < 6)
            {
                textLines.Text = plot[plotCode];
                textLines.Update();

                AnswersList.Items.Clear();
                AnswersList.Update();
                AnswersList.EndUpdate();

                if (plotCode == 3)
                {
                    AnswersList.Items.Add(playerAnswers[0]);
                    AnswersList.Update();
                    AnswersList.EndUpdate();
                    NextReplika.Enabled = false;
                }

                if (plotCode == 5)
                {
                    AutosaveCode = 1;
                }
                plotCode++;
            }
        }

        private void GamePartLilith()
        {
            Coctails.Enabled = false;
            AnswersList.Items.Clear();
            AnswersList.Update();
            AnswersList.EndUpdate();

            Coctails.SelectedIndex = -1;
            AnswersList.SelectedIndex = -1;

            if (Coctails.Items.Count == 0)
            {
                if (recipie)
                {
                    coctails.Add(lilith.Coctails[1]);
                    recipies.Add(lilith.Recipies[1]);
                }
                foreach (var coctail in coctails)
                {
                    Coctails.Items.Add(coctail);
                    Coctails.Update();
                }
                Coctails.EndUpdate();
            }

            textLines.Text = GameLine.MainGameLine(AutosaveCode);

            switch (AutosaveCode)
            {
                case 1:
                    AnswersList.Items.Add(playerAnswers[2]);
                    AnswersList.Items.Add(playerAnswers[3]);
                    AnswersList.Update();
                    AnswersList.EndUpdate();

                    AutosaveCode = -1;
                    NextReplika.Enabled = false;
                    break;
                case 2:
                    AutosaveCode = 4;
                    break;
                case 3:
                    AutosaveCode = 4;
                    break;
                case 4:
                    AnswersList.Items.Add(playerAnswers[4]);
                    AnswersList.Items.Add(playerAnswers[5]);
                    AnswersList.Update();
                    AnswersList.EndUpdate();

                    AutosaveCode = -4;
                    NextReplika.Enabled = false;
                    break;
                case 5:
                    AnswersList.Items.Add(playerAnswers[6]);
                    AnswersList.Items.Add(playerAnswers[7]);
                    AnswersList.Update();
                    AnswersList.EndUpdate();

                    AutosaveCode = -5;
                    NextReplika.Enabled = false;
                    break;
                case 6:
                    AnswersList.Items.Add(playerAnswers[6]);
                    AnswersList.Items.Add(playerAnswers[7]);
                    AnswersList.Update();
                    AnswersList.EndUpdate();

                    AutosaveCode = -5;
                    NextReplika.Enabled = false;
                    break;
                case 7:
                    AnswersList.Items.Add(playerAnswers[6]);
                    AnswersList.Items.Add(playerAnswers[8]);
                    AnswersList.Update();
                    AnswersList.EndUpdate();

                    AutosaveCode = -7;
                    NextReplika.Enabled = false;
                    break;
                case 8:
                    AnswersList.Items.Add(playerAnswers[6]);
                    AnswersList.Update();
                    AnswersList.EndUpdate();

                    AutosaveCode = -8;
                    NextReplika.Enabled = false;
                    break;
                case 9:
                    if (recipie) { AutosaveCode = 10; }
                    else
                    {
                        if (advice) { AutosaveCode = 12; }
                        else { AutosaveCode = 14; }
                    }
                    break;
                case 10:
                    AutosaveCode = 11;
                    Coctails.Items.Add(lilith.Coctails[1]);
                    Coctails.Update();
                    Coctails.EndUpdate();

                    coctails.Add(lilith.Coctails[1]);
                    recipies.Add(lilith.Recipies[1]);
                    break;
                case 11:
                    AutosaveCode = 16;
                    break;
                case 12:
                    AutosaveCode = 13;
                    break;
                case 13:
                    AutosaveCode = 16;
                    break;
                case 14:
                    AutosaveCode = 16;
                    break;
                case 15:
                    recipie = false;
                    AutosaveCode = 16;
                    break;
            }
        }

        private void GameMiddlePart()
        {
            if (plotCode < 9)
            {
                textLines.Text = plot[plotCode];
                textLines.Update();
                plotCode++;
            }
        }

        private void GamePartPeter()
        {

        }
    }
}
