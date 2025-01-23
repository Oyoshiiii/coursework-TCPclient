namespace coursework_TCPclient
{
    public partial class Form1 : Form
    {
        int AutosaveCode;
        public Form1()
        {
            InitializeComponent();
            AutosaveCode = 0;
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            /*
             вызывается метод TCPclient и устанавливается соединение с сервером
            на сервер отправляется сообщение 'newgame' и сервер очищает список автосохранений, устанавливая текущий код
            автосохранения 0, 
            далее вызывается из бибилотеки GameLibrary статический метод класса GameLine
            
            после закрытия программы, на сервер отправляется сообщение 
             */

        }

        private void ContinueButton_Click(object sender, EventArgs e)
        {
            /*
             вызывается метод TCPclient и устанавливается соединение с сервером
            на сервер отправляется сообщение 'contgame'
            если список автосохранений пуст, то сервер возвращает сообщение 'noautosaves' и приложение высвечивает
            messageBox с сообщением о том, что нет последних автосохранений
            если список автосохранений не пуст, сервер возвращает код последнего автосохранения, который заносится в переменную
            AutosaveCode
            далее вызывается из бибилотеки GameLibrary статический метод класса GameLine
             */
        }
    }
}
