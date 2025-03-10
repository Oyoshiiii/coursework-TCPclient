﻿using CourseworkGameLib;
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
        Robert robert = new Robert();

        List<string> coctails = null;
        List<string> recipies = null;

        List<string> images = new List<string>
        {
            "bar.jpg",
            //картинка бара с лилит
            "lilith.jpg",
            //картинка бара с питером
            "robert.jpg"
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
            "* спустя двадцать минут в бар зашел промокший от дождя парень, который был явно не в духе *",
            "* парень забрал заказ и ушел *"
        };
        List<string> playerAnswers = new List<string>()
        {
            //часть игры с Лилит
            //0 1
            "Да",
            "Нет",
            //2 3
            "А как обычно - это как?..",
            "Конечно, 'как обычно', каждый день такое готовлю",
            //4 5
            "Ничего страшного",
            "Ну, смешно однако...",
            //6 7
            "Хорошо, сейчас сделаем",
            "Простите, может вы еще вспомните пару деталей коктейля?",
            // 8
            "...может.. ещё детали вспомните?...",
            
            //часть игры с Робертом
            //9 10
            "Извините?. Я хоть и пару дней тут, но ничем не хуже Джилл или Джона",
            "Не переживайте, я приготовлю все в лучшем виде",
            //11 12
            "Пару минут",
            "Серьёзно? Тут каждый второй напиток на любителя..."
        };

        public Form1()
        {
            InitializeComponent();
            coctails = new List<string>()
            {
                lilith.Coctails[0],
                "Кисельные грёзы",
                robert.Coctails[0]
            };
            recipies = new List<string>()
            {
                lilith.Recipies[0],
                "Альдегид, лаймовый сироп",
                robert.Recipies[0]
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

                            byte[] data = Encoding.UTF8.GetBytes(sendCode);
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
        } //обработчик закрытия игры
        private void NewGameButton_Click(object sender, EventArgs e)
        {
            AutosaveCode = 0;
            SendCodeToServerAsync();
        } //обработчик кнопки Новая игра

        private void ContinueButton_Click(object sender, EventArgs e)
        {
            AutosaveCode = -1;
            SendCodeToServerAsync();
        }    //обработчик кнопки Продолжить

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
                        plotCode = 6;
                        NewGameButton.Hide();
                        ContinueButton.Hide();

                        menuText.Visible = true;
                        visual.Visible = true;
                        textLines.Visible = true;
                        AnswersList.Visible = true;
                        NextReplika.Visible = true;
                        Coctails.Visible = true;

                        if (AutosaveCode == 16) { plotCode = 6; }
                        visual.Image = Image.FromFile(images[0]);
                        textLines.Text = "Нажми на кнопку ->";
                    }
                }

                player.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }  //отправка кода на сервер

        private void NextReplika_Click(object sender, EventArgs e)
        {
            if (AutosaveCode == 0) { StartGame(); }
            else if (AutosaveCode < 16 && AutosaveCode > -17) { GamePartLilith(); }
            else if (AutosaveCode == 16 && plotCode < 9) { GameMiddlePart(); }
            else if (AutosaveCode < 30) { GamePartRobert(); }
            else { EndGame(); }
        } //обработчик кнопки перехода на следующую реплику

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

            if (AutosaveCode == -8 && AnswersList.Items.Count > 0)
            {
                if (AnswersList.SelectedIndex == 0)
                {
                    Coctails.Enabled = true;
                    NextReplika.Enabled = false;
                }
            }

            if (AutosaveCode == -17 && AnswersList.Items.Count > 0)
            {
                if (AnswersList.SelectedIndex == 0)
                {
                    AutosaveCode = 18;
                    NextReplika.Enabled = true;
                }
                if (AnswersList.SelectedIndex == 1)
                {
                    AutosaveCode = 19;
                    NextReplika.Enabled = true;
                }
            }

            if ((AutosaveCode == -21 || AutosaveCode == 26 || AutosaveCode == 23 || AutosaveCode == 27 || AutosaveCode == 22) && AnswersList.Items.Count > 0)
            {
                if (AnswersList.SelectedIndex == 0)
                {
                    Coctails.Enabled = true;
                    NextReplika.Enabled = false;
                    advice = true;
                    AutosaveCode = -21;
                }
                if (AnswersList.SelectedIndex == 1)
                {
                    Coctails.Enabled = false;
                    NextReplika.Enabled = true;
                    advice = false;
                    Coctails.SelectedIndex = -1;
                    AutosaveCode = 22;
                }
            }

            if (AutosaveCode == -22 && AnswersList.Items.Count > 0)
            {
                if (AnswersList.SelectedIndex == 0)
                {
                    Coctails.Enabled = true;
                    NextReplika.Enabled = false;
                    AutosaveCode = -22;
                }
            }
        } //обработчик изменения выбранных элементов листа ответов
        //методы игры и обработчик изменения выбранных элементов в списке напитков
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
                if (AutosaveCode < 16 && AutosaveCode > -16) { AutosaveCode = 9; }
                else { AutosaveCode = 26;}
            }
            if (Coctails.SelectedIndex == 1)
            {
                MessageBox.Show(recipies[1], "Описание коктейля", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (AutosaveCode < 16 && AutosaveCode > -16) { AutosaveCode = 15; }
                else { AutosaveCode = 26; }
            }
            if (Coctails.SelectedIndex == 2)
            {
                MessageBox.Show(recipies[2], "Описание коктейля", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (AutosaveCode < 16 && AutosaveCode > -16) { AutosaveCode = 15; }
                else
                {
                    if (advice) { AutosaveCode = 23; }
                    else { AutosaveCode = 25;}
                }
            }
            if (Coctails.SelectedIndex == 3)
            {
                MessageBox.Show(recipies[3], "Описание коктейля", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AutosaveCode = 27;
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
            if (visual.Image != Image.FromFile(images[1]))
            {
                visual.Image = Image.FromFile(images[1]);
            }

            Coctails.Enabled = false;
            AnswersList.Items.Clear();
            AnswersList.Update();
            AnswersList.EndUpdate();

            Coctails.SelectedIndex = -1;
            AnswersList.SelectedIndex = -1;

            if (Coctails.Items.Count == 0)
            {
                foreach (var coctail in coctails)
                {
                    Coctails.Items.Add(coctail);
                    Coctails.Update();
                }
                Coctails.EndUpdate();
            }
            if (recipie && Coctails.Items.Count < 4 && AutosaveCode != 15)
            {
                coctails.Add(lilith.Coctails[lilith.CoctailGiftNum]);
                recipies.Add(lilith.Recipies[lilith.CoctailGiftNum]);
                Coctails.Items.Add(coctails[coctails.Count - 1]);
                Coctails.Update();
                Coctails.EndUpdate();
            }

            switch (AutosaveCode)
            {
                case -2:
                    AutosaveCode = 4;
                    break;
                case -3:
                    AutosaveCode = 4;
                    break;
                case -10:
                    AutosaveCode = 11;
                    break;
                case -11:
                    AutosaveCode = 16;
                    break;
                case -12:
                    AutosaveCode = 13;
                    break;
                case -13:
                    AutosaveCode = 16;
                    break;
                case -14:
                    AutosaveCode = 16;
                    break;
                case -15:
                    AutosaveCode = 16;
                    break;
            }
            
            if (AutosaveCode < 16)
            {
                textLines.Text = GameLine.MainGameLine(AutosaveCode);
            }
            else
            {
                visual.Image = Image.FromFile(images[0]);
                textLines.Text = plot[plotCode]; 
                plotCode++;
            }

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
                    AutosaveCode = -2;
                    break;
                case 3:
                    AutosaveCode = -3;
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
                    AutosaveCode = -10;
                    break;
                case 11:
                    MessageBox.Show($"Добавлен новый напиток: {lilith.Coctails[lilith.CoctailGiftNum]}", $"Подарок от {lilith.Name}", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    AutosaveCode = -11;
                    break;
                case 12:
                    AutosaveCode = -12;
                    break;
                case 13:
                    AutosaveCode = -13;
                    break;
                case 14:
                    AutosaveCode = -14;
                    break;
                case 15:
                    recipie = false;
                    AutosaveCode = -15;
                    break;
            }
        }

        private void GameMiddlePart()
        {
            if (visual.Image != Image.FromFile(images[0]))
            {
                visual.Image = Image.FromFile(images[0]);
            }

            if (advice) { advice = false; }

            if (plotCode < 9)
            {
                textLines.Text = plot[plotCode];
                textLines.Update();
                if (plotCode == 8) { AutosaveCode = 17; }
            }

            plotCode++;
        }

        private void GamePartRobert()
        {
            if (visual.Image != Image.FromFile(images[2]) && AutosaveCode < 28 && AutosaveCode > 0)
            {
                visual.Image = Image.FromFile(images[2]);
            }

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
                    coctails.Add(lilith.Coctails[lilith.CoctailGiftNum]);
                    recipies.Add(lilith.Recipies[lilith.CoctailGiftNum]);
                }
                foreach (var coctail in coctails)
                {
                    Coctails.Items.Add(coctail);
                    Coctails.Update();
                }
                Coctails.EndUpdate();
            }

            switch (AutosaveCode)
            {
                case -18:
                    AutosaveCode = 20;
                    break;
                case -19:
                    AutosaveCode = 20;
                    break;
                case -20:
                    AutosaveCode = 21;
                    break;
                case -23:
                    AutosaveCode = 24;
                    break;
                case -24:
                    AutosaveCode = 28;
                    break;
                case -25:
                    AutosaveCode = 28;
                    break;
                case -26:
                    AutosaveCode = 28;
                    break;
                case -27:
                    AutosaveCode = 28;
                    break;
                case -28:
                    AutosaveCode = 30;
                    textLines.Text = "...";
                    break;
            }

            if (AutosaveCode < 28) { textLines.Text = GameLine.MainGameLine(AutosaveCode); }

            switch (AutosaveCode)
            {
                case 17:
                    AnswersList.Items.Add(playerAnswers[9]);
                    AnswersList.Items.Add(playerAnswers[10]);
                    AnswersList.Update();
                    AnswersList.EndUpdate();

                    AutosaveCode = -17;
                    NextReplika.Enabled = false;
                    break;
                case 18:
                    AutosaveCode = -18;
                    break;
                case 19:
                    AutosaveCode = -19;
                    break;
                case 20:
                    AutosaveCode = -20;
                    break;
                case 21:
                    AnswersList.Items.Add(playerAnswers[11]);
                    AnswersList.Items.Add(playerAnswers[12]);
                    AnswersList.Update();
                    AnswersList.EndUpdate();

                    AutosaveCode = -21;
                    NextReplika.Enabled = false;
                    break;
                case 22:
                    AnswersList.Items.Add(playerAnswers[11]);
                    AnswersList.Update();
                    AnswersList.EndUpdate();

                    AutosaveCode = -22;
                    NextReplika.Enabled = false;
                    break;
                case 23:
                    AutosaveCode = -23;
                    break;
                case 24:
                    AutosaveCode = -24;
                    break;
                case 25:
                    AutosaveCode = -25;
                    break;
                case 26:
                    AutosaveCode = -26;
                    break;
                case 27:
                    AutosaveCode = -27;
                    break;
                case 28:
                    visual.Image = Image.FromFile(images[0]);
                    textLines.Text = plot[plot.Count - 1];
                    AutosaveCode = -28;
                    break;
            }
        }
    
        private void EndGame()
        {
            NewGameButton.Visible = true;
            ContinueButton.Visible = true;

            menuText.Hide();
            visual.Hide();
            textLines.Hide();
            AnswersList.Hide();
            NextReplika.Hide();
            Coctails.Hide();

            MessageBox.Show("Спасибо за прохождение этой короткой визуальной новеллы, " +
                "основанной на игре Va 11 Hall-a!", "Конец", MessageBoxButtons.OK, MessageBoxIcon.Information);

            var answ = MessageBox.Show("Вам понравилось?", "???", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if(answ == DialogResult.Yes)
            {
                MessageBox.Show("(ﾉ◕ヮ◕)ﾉ*:･ﾟ✧");
            }
            else { MessageBox.Show("(╯︵╰,)"); }

            AutosaveCode = 0;
        }
    }
}