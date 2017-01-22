using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace MasterMind
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
    {
        private IContainer components;
		private System.Windows.Forms.Button ScoreButton;
		private System.Windows.Forms.Button UndoButton;
		private Board MainBoard;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.Button AutoButton;

		enum Level {beginner=0, intermediate=1, expert=2};

		private Level TheLevel;

		private int[, ] LevelInfo = new int[3,2];

		private const int kGuessPopulation = 0;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem NewGame;
		private System.Windows.Forms.MenuItem ExitMenu;
        private Label label1;
        private Panel panel1;
        private Panel panel2;
        private RadioButton radioButtonSecretPattern;
        private RadioButton radioButtonBoard;
        private CheckBox checkBoxDrawSecure;
        private Button buttonBFS;
        private Button buttonDFS;
        private Button buttonGrad;
        private Button buttonNodeEdge;
		private const int kGuessGeneration = 1;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			SetupLevelInformation();
			InitializeMenu();
			ResetBoard();
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		private void ResetBoard()
		{
			MainBoard = new Board(ClientRectangle);
		}

		private void InitializeMenu()
		{
			menuItem4.Checked = true; // set to expert level
			TheLevel = Level.expert;
		}


		public void SetupLevelInformation()
		{
			// beginner
			LevelInfo[0, 0] = 10;
 			LevelInfo[0, 1] = 25;

			// intermediate
			LevelInfo[1, 0] = 20;
			LevelInfo[1, 1] = 50;

			// expert
			LevelInfo[2, 0] = 1000;
			LevelInfo[2, 1] = 10;

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.ScoreButton = new System.Windows.Forms.Button();
            this.UndoButton = new System.Windows.Forms.Button();
            this.AutoButton = new System.Windows.Forms.Button();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.NewGame = new System.Windows.Forms.MenuItem();
            this.ExitMenu = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.checkBoxDrawSecure = new System.Windows.Forms.CheckBox();
            this.radioButtonSecretPattern = new System.Windows.Forms.RadioButton();
            this.radioButtonBoard = new System.Windows.Forms.RadioButton();
            this.buttonBFS = new System.Windows.Forms.Button();
            this.buttonDFS = new System.Windows.Forms.Button();
            this.buttonGrad = new System.Windows.Forms.Button();
            this.buttonNodeEdge = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ScoreButton
            // 
            this.ScoreButton.Location = new System.Drawing.Point(177, 411);
            this.ScoreButton.Name = "ScoreButton";
            this.ScoreButton.Size = new System.Drawing.Size(75, 23);
            this.ScoreButton.TabIndex = 0;
            this.ScoreButton.Text = "Score";
            this.ScoreButton.Click += new System.EventHandler(this.ScoreButton_Click);
            this.ScoreButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ScoreButton_MouseDown);
            // 
            // UndoButton
            // 
            this.UndoButton.Location = new System.Drawing.Point(89, 411);
            this.UndoButton.Name = "UndoButton";
            this.UndoButton.Size = new System.Drawing.Size(75, 23);
            this.UndoButton.TabIndex = 1;
            this.UndoButton.Text = "Undo";
            this.UndoButton.Click += new System.EventHandler(this.UndoButton_Click);
            // 
            // AutoButton
            // 
            this.AutoButton.Location = new System.Drawing.Point(9, 411);
            this.AutoButton.Name = "AutoButton";
            this.AutoButton.Size = new System.Drawing.Size(64, 24);
            this.AutoButton.TabIndex = 2;
            this.AutoButton.Text = "Auto";
            this.AutoButton.Click += new System.EventHandler(this.AutoButton_Click);
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem5,
            this.menuItem1});
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 0;
            this.menuItem5.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.NewGame,
            this.ExitMenu});
            this.menuItem5.Text = "File";
            // 
            // NewGame
            // 
            this.NewGame.Index = 0;
            this.NewGame.Text = "New Game...";
            this.NewGame.Click += new System.EventHandler(this.NewGame_Click);
            // 
            // ExitMenu
            // 
            this.ExitMenu.Index = 1;
            this.ExitMenu.Text = "Exit...";
            this.ExitMenu.Click += new System.EventHandler(this.ExitMenu_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 1;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem2,
            this.menuItem3,
            this.menuItem4});
            this.menuItem1.RadioCheck = true;
            this.menuItem1.Text = "Level";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 0;
            this.menuItem2.RadioCheck = true;
            this.menuItem2.Text = "Beginner";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 1;
            this.menuItem3.RadioCheck = true;
            this.menuItem3.Text = "Intermediate";
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 2;
            this.menuItem4.RadioCheck = true;
            this.menuItem4.Text = "Expert";
            this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Загаданный код";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(9, 346);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(155, 59);
            this.panel1.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.checkBoxDrawSecure);
            this.panel2.Controls.Add(this.radioButtonSecretPattern);
            this.panel2.Controls.Add(this.radioButtonBoard);
            this.panel2.Location = new System.Drawing.Point(177, 346);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(91, 59);
            this.panel2.TabIndex = 5;
            // 
            // checkBoxDrawSecure
            // 
            this.checkBoxDrawSecure.AutoSize = true;
            this.checkBoxDrawSecure.Location = new System.Drawing.Point(3, 39);
            this.checkBoxDrawSecure.Name = "checkBoxDrawSecure";
            this.checkBoxDrawSecure.Size = new System.Drawing.Size(86, 17);
            this.checkBoxDrawSecure.TabIndex = 2;
            this.checkBoxDrawSecure.Text = "Отобразить";
            this.checkBoxDrawSecure.UseVisualStyleBackColor = true;
            this.checkBoxDrawSecure.CheckedChanged += new System.EventHandler(this.checkBoxDrawSecure_CheckedChanged);
            // 
            // radioButtonSecretPattern
            // 
            this.radioButtonSecretPattern.AutoSize = true;
            this.radioButtonSecretPattern.Location = new System.Drawing.Point(3, 20);
            this.radioButtonSecretPattern.Name = "radioButtonSecretPattern";
            this.radioButtonSecretPattern.Size = new System.Drawing.Size(44, 17);
            this.radioButtonSecretPattern.TabIndex = 1;
            this.radioButtonSecretPattern.TabStop = true;
            this.radioButtonSecretPattern.Text = "Код";
            this.radioButtonSecretPattern.UseVisualStyleBackColor = true;
            // 
            // radioButtonBoard
            // 
            this.radioButtonBoard.AutoSize = true;
            this.radioButtonBoard.Location = new System.Drawing.Point(3, 6);
            this.radioButtonBoard.Name = "radioButtonBoard";
            this.radioButtonBoard.Size = new System.Drawing.Size(51, 17);
            this.radioButtonBoard.TabIndex = 0;
            this.radioButtonBoard.TabStop = true;
            this.radioButtonBoard.Text = "Поле";
            this.radioButtonBoard.UseVisualStyleBackColor = true;
            // 
            // buttonBFS
            // 
            this.buttonBFS.Location = new System.Drawing.Point(390, 12);
            this.buttonBFS.Name = "buttonBFS";
            this.buttonBFS.Size = new System.Drawing.Size(75, 23);
            this.buttonBFS.TabIndex = 6;
            this.buttonBFS.Text = "В ширину";
            this.buttonBFS.UseVisualStyleBackColor = true;
            this.buttonBFS.Click += new System.EventHandler(this.buttonBFS_Click);
            // 
            // buttonDFS
            // 
            this.buttonDFS.Location = new System.Drawing.Point(390, 41);
            this.buttonDFS.Name = "buttonDFS";
            this.buttonDFS.Size = new System.Drawing.Size(75, 23);
            this.buttonDFS.TabIndex = 7;
            this.buttonDFS.Text = "В глубину";
            this.buttonDFS.UseVisualStyleBackColor = true;
            this.buttonDFS.Click += new System.EventHandler(this.buttonDFS_Click);
            // 
            // buttonGrad
            // 
            this.buttonGrad.Location = new System.Drawing.Point(390, 70);
            this.buttonGrad.Name = "buttonGrad";
            this.buttonGrad.Size = new System.Drawing.Size(75, 23);
            this.buttonGrad.TabIndex = 8;
            this.buttonGrad.Text = "Градиентный";
            this.buttonGrad.UseVisualStyleBackColor = true;
            this.buttonGrad.Click += new System.EventHandler(this.buttonGrad_Click);
            // 
            // buttonNodeEdge
            // 
            this.buttonNodeEdge.Location = new System.Drawing.Point(390, 100);
            this.buttonNodeEdge.Name = "buttonNodeEdge";
            this.buttonNodeEdge.Size = new System.Drawing.Size(75, 23);
            this.buttonNodeEdge.TabIndex = 9;
            this.buttonNodeEdge.Text = "Стратегия ветвей и границ";
            this.buttonNodeEdge.UseVisualStyleBackColor = true;
            this.buttonNodeEdge.Click += new System.EventHandler(this.buttonNodeEdge_Click);
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(468, 443);
            this.Controls.Add(this.buttonNodeEdge);
            this.Controls.Add(this.buttonGrad);
            this.Controls.Add(this.buttonDFS);
            this.Controls.Add(this.buttonBFS);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.AutoButton);
            this.Controls.Add(this.UndoButton);
            this.Controls.Add(this.ScoreButton);
            this.Menu = this.mainMenu1;
            this.Name = "Form1";
            this.Text = "MasterMind";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void Form1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
		  Graphics g = e.Graphics;
		  g.FillRectangle(Brushes.LightGray, ClientRectangle);
		  MainBoard.Draw(g);
          Graphics g1 = panel1.CreateGraphics();
          MainBoard.DrawSecretPattern(g1, checkBoxDrawSecure.Checked);
          g1.Dispose();
		}

		private void ScoreButton_Click(object sender, System.EventArgs e)
		{
			int numberOfBlackPegs = MainBoard.CalcScore();
			Invalidate();

			if (numberOfBlackPegs == 4)
			{
				MessageBox.Show("You Win in " + (MainBoard.CurrentRow + 1).ToString() + " tries!");
				Application.Exit();
			}

			if (MainBoard.AdvanceRow() == false)
			{
				MessageBox.Show("You Lose!");
				Application.Exit();
			}
		}

		private void ScoreButton_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
		}

		private void Form1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
            Board.destination dest = Board.destination.board;
            if (radioButtonSecretPattern.Checked) dest = Board.destination.secure_code;
            if (radioButtonBoard.Checked) dest = Board.destination.board;
			MainBoard.PlacePeg(e.X, e.Y, dest);
			Invalidate();
		}

		private void UndoButton_Click(object sender, System.EventArgs e)
		{
			MainBoard.UndoLastPeg();
			Invalidate();
		}

		private void AutoButton_Click(object sender, System.EventArgs e)
		{
          int numberOfBlackPegs = 0;
		  this.Cursor = Cursors.WaitCursor;
		  switch (TheLevel)
			{
			  case Level.beginner:
                    numberOfBlackPegs = MainBoard.LetComputerGuess(LevelInfo[(int)Level.beginner, kGuessPopulation], 
					LevelInfo[(int)Level.beginner, kGuessGeneration]);
				break;
			  case Level.intermediate:
                numberOfBlackPegs = MainBoard.LetComputerGuess(LevelInfo[(int)Level.intermediate, kGuessPopulation], 
					  LevelInfo[(int)Level.intermediate, kGuessGeneration]);
				  break;
			  case Level.expert:
                  numberOfBlackPegs = MainBoard.LetComputerGuess(LevelInfo[(int)Level.expert, kGuessPopulation], 
					  LevelInfo[(int)Level.expert, kGuessGeneration]);
				  break;
			  default:
                  numberOfBlackPegs = MainBoard.LetComputerGuess(LevelInfo[(int)Level.expert, kGuessPopulation], 
					  LevelInfo[(int)Level.expert, kGuessGeneration]);
				  break;
			}

		  this.Cursor = Cursors.Arrow;
		  Invalidate();

          if (numberOfBlackPegs == 4)
          {
              MessageBox.Show("Auto Win in " + (MainBoard.CurrentRow).ToString() + " tries!");
              Application.Exit();
          }

          if (MainBoard.CurrentRow > 9)
          {
              MessageBox.Show("You Lose!");
              Application.Exit();
          }
		}

		private void ClearMenuItems()
		{
			menuItem2.Checked = false;
			menuItem3.Checked = false;
			menuItem4.Checked = false;
		}
		private void menuItem2_Click(object sender, System.EventArgs e)
		{
			ClearMenuItems();
		  ((MenuItem)sender).Checked = true;
			TheLevel = Level.beginner;
			Invalidate();
		}

		private void menuItem3_Click(object sender, System.EventArgs e)
		{
			ClearMenuItems();
			((MenuItem)sender).Checked = true;
			TheLevel = Level.intermediate;
			Invalidate();
		}

		private void menuItem4_Click(object sender, System.EventArgs e)
		{
			ClearMenuItems();
			((MenuItem)sender).Checked = true;
			TheLevel = Level.expert;
			Invalidate();
		}

		private void NewGame_Click(object sender, System.EventArgs e)
		{
			this.ResetBoard();
			Invalidate();
		}

		private void ExitMenu_Click(object sender, System.EventArgs e)
		{
			Application.Exit();
		}

        private void checkBoxDrawSecure_CheckedChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void buttonBFS_Click(object sender, EventArgs e)
        {
            MainBoard.bfs.FindDecision_BFS();
        }

        private void buttonDFS_Click(object sender, EventArgs e)
        {
            MainBoard.dfs.FindDecision_DFS();
        }

        private void buttonNodeEdge_Click(object sender, EventArgs e)
        {
            MainBoard.edgeBounds.SearchSolution();
            Invalidate();
        }

        private void buttonGrad_Click(object sender, EventArgs e)
        {
            MainBoard.grad.FindDecision_Grad();
        }
	}
}
