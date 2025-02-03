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
            menuText = new Label();
            ((System.ComponentModel.ISupportInitialize)visual).BeginInit();
            SuspendLayout();
            // 
            // NewGameButton
            // 
            NewGameButton.BackColor = Color.MediumPurple;
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
            ContinueButton.BackColor = Color.MediumOrchid;
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
            textLines.BackColor = Color.LightSlateGray;
            textLines.Location = new Point(22, 284);
            textLines.Name = "textLines";
            textLines.Size = new Size(681, 87);
            textLines.TabIndex = 2;
            textLines.Text = "";
            textLines.Visible = false;
            // 
            // visual
            // 
            visual.BackColor = Color.Indigo;
            visual.Location = new Point(229, 12);
            visual.Name = "visual";
            visual.Size = new Size(512, 266);
            visual.TabIndex = 3;
            visual.TabStop = false;
            visual.Visible = false;
            // 
            // NextReplika
            // 
            NextReplika.BackColor = Color.Orchid;
            NextReplika.Location = new Point(709, 320);
            NextReplika.Name = "NextReplika";
            NextReplika.Size = new Size(79, 83);
            NextReplika.TabIndex = 4;
            NextReplika.Text = "->";
            NextReplika.UseVisualStyleBackColor = false;
            NextReplika.Visible = false;
            NextReplika.Click += NextReplika_Click;
            // 
            // AnswersList
            // 
            AnswersList.BackColor = Color.Indigo;
            AnswersList.ForeColor = Color.FloralWhite;
            AnswersList.FormattingEnabled = true;
            AnswersList.HorizontalScrollbar = true;
            AnswersList.ItemHeight = 25;
            AnswersList.Location = new Point(22, 377);
            AnswersList.Name = "AnswersList";
            AnswersList.Size = new Size(681, 54);
            AnswersList.TabIndex = 5;
            AnswersList.Visible = false;
            AnswersList.SelectedIndexChanged += AnswersList_SelectedIndexChanged;
            // 
            // Coctails
            // 
            Coctails.BackColor = Color.DarkMagenta;
            Coctails.FormattingEnabled = true;
            Coctails.ItemHeight = 25;
            Coctails.Location = new Point(22, 37);
            Coctails.Name = "Coctails";
            Coctails.Size = new Size(180, 229);
            Coctails.TabIndex = 6;
            Coctails.Visible = false;
            Coctails.SelectedIndexChanged += Coctails_SelectedIndexChanged;
            // 
            // menuText
            // 
            menuText.AutoSize = true;
            menuText.ForeColor = SystemColors.ActiveCaption;
            menuText.Location = new Point(22, 9);
            menuText.Name = "menuText";
            menuText.Size = new Size(187, 25);
            menuText.TabIndex = 7;
            menuText.Text = "Меню напитков бара";
            menuText.Visible = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DarkSlateBlue;
            ClientSize = new Size(800, 450);
            Controls.Add(menuText);
            Controls.Add(Coctails);
            Controls.Add(AnswersList);
            Controls.Add(NextReplika);
            Controls.Add(visual);
            Controls.Add(textLines);
            Controls.Add(ContinueButton);
            Controls.Add(NewGameButton);
            Name = "Form1";
            Text = "MainWindow";
            FormClosing += Form1_FormClosing;
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
        private Label menuText;
    }
}
