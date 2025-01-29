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
            AutosaveCode = 0;
            await TcpPlayer();
            /*
             вызывается метод TCPclient и устанавливается соединение с сервером
            на сервер отправляется сообщение 0 и сервер очищает список автосохранений, устанавливая текущий код
            автосохранения 0, 
            далее вызывается из бибилотеки GameLibrary статический метод класса GameLine
            после закрытия программы, на сервер отправляется сообщение 
             */
        }

        private void ContinueButton_Click(object sender, EventArgs e)
        {
            /*
             вызывается метод TCPclient и устанавливается соединение с сервером
            на сервер отправляется сообщение '-1'
            если список автосохранений пуст, то сервер возвращает сообщение 'noautosaves' и приложение высвечивает
            messageBox с сообщением о том, что нет последних автосохранений
            если список автосохранений не пуст, сервер возвращает код последнего автосохранения, который заносится в переменную
            AutosaveCode
            далее вызывается из бибилотеки GameLibrary статический метод класса GameLine
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
