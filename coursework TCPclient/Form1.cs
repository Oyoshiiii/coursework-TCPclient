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
             ���������� ����� TCPclient � ��������������� ���������� � ��������
            �� ������ ������������ ��������� 'newgame' � ������ ������� ������ ��������������, ������������ ������� ���
            �������������� 0, 
            ����� ���������� �� ���������� GameLibrary ����������� ����� ������ GameLine
            
            ����� �������� ���������, �� ������ ������������ ��������� 
             */

        }

        private void ContinueButton_Click(object sender, EventArgs e)
        {
            /*
             ���������� ����� TCPclient � ��������������� ���������� � ��������
            �� ������ ������������ ��������� 'contgame'
            ���� ������ �������������� ����, �� ������ ���������� ��������� 'noautosaves' � ���������� �����������
            messageBox � ���������� � ���, ��� ��� ��������� ��������������
            ���� ������ �������������� �� ����, ������ ���������� ��� ���������� ��������������, ������� ��������� � ����������
            AutosaveCode
            ����� ���������� �� ���������� GameLibrary ����������� ����� ������ GameLine
             */
        }
    }
}
