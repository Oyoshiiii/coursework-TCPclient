namespace coursework_TCPclient
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            NewGameButton = new Button();
            ContinueButton = new Button();
            textLines = new RichTextBox();
            visual = new PictureBox();
            NextReplika = new Button();
            AnswersList = new ListBox();
            Coctails = new ListBox();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)visual).BeginInit();
            SuspendLayout();
            // 
            // NewGameButton
            // 
            NewGameButton.BackColor = Color.LightBlue;
            NewGameButton.Location = new Point(321, 131);
            NewGameButton.Name = "NewGameButton";
            NewGameButton.Size = new Size(157, 62);
            NewGameButton.TabIndex = 0;
            NewGameButton.Text = "Новая игра";
            NewGameButton.UseVisualStyleBackColor = false;
            NewGameButton.Click += NewGameButton_Click;
            // 
            // ContinueButton
            // 
            ContinueButton.BackColor = SystemColors.InactiveCaption;
            ContinueButton.Location = new Point(321, 220);
            ContinueButton.Name = "ContinueButton";
            ContinueButton.Size = new Size(157, 58);
            ContinueButton.TabIndex = 1;
            ContinueButton.Text = "Продолжить";
            ContinueButton.UseVisualStyleBackColor = false;
            ContinueButton.Click += ContinueButton_Click;
            // 
            // textLines
            // 
            textLines.Location = new Point(22, 286);
            textLines.Name = "textLines";
            textLines.Size = new Size(423, 144);
            textLines.TabIndex = 2;
            textLines.Text = "";
            textLines.Visible = false;
            // 
            // visual
            // 
            visual.Location = new Point(215, 12);
            visual.Name = "visual";
            visual.Size = new Size(559, 266);
            visual.TabIndex = 3;
            visual.TabStop = false;
            visual.Visible = false;
            // 
            // NextReplika
            // 
            NextReplika.Location = new Point(709, 320);
            NextReplika.Name = "NextReplika";
            NextReplika.Size = new Size(79, 83);
            NextReplika.TabIndex = 4;
            NextReplika.Text = "->";
            NextReplika.UseVisualStyleBackColor = true;
            NextReplika.Visible = false;
            NextReplika.Click += NextReplika_Click;
            // 
            // AnswersList
            // 
            AnswersList.FormattingEnabled = true;
            AnswersList.ItemHeight = 25;
            AnswersList.Location = new Point(451, 286);
            AnswersList.Name = "AnswersList";
            AnswersList.Size = new Size(252, 154);
            AnswersList.TabIndex = 5;
            AnswersList.Visible = false;
            // 
            // Coctails
            // 
            Coctails.FormattingEnabled = true;
            Coctails.ItemHeight = 25;
            Coctails.Location = new Point(22, 37);
            Coctails.Name = "Coctails";
            Coctails.Size = new Size(180, 229);
            Coctails.TabIndex = 6;
            Coctails.Visible = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(22, 9);
            label1.Name = "label1";
            label1.Size = new Size(59, 25);
            label1.TabIndex = 7;
            label1.Text = "Меню напитков бара";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveBorder;
            ClientSize = new Size(800, 450);
            Controls.Add(label1);
            Controls.Add(Coctails);
            Controls.Add(AnswersList);
            Controls.Add(NextReplika);
            Controls.Add(visual);
            Controls.Add(textLines);
            Controls.Add(ContinueButton);
            Controls.Add(NewGameButton);
            Name = "Form1";
            Text = "MainWindow";
            ((System.ComponentModel.ISupportInitialize)visual).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button NewGameButton;
        private Button ContinueButton;
        private RichTextBox textLines;
        private PictureBox visual;
        private Button NextReplika;
        private ListBox AnswersList;
        private ListBox Coctails;
        private Label label1;
    }
}
