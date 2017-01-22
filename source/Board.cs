using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using GeneticAlgorithm;


namespace MasterMind
{
	/// <summary>
	/// Summary description for Board.
	/// </summary>
	/// 
	public class Board
	{
        public enum destination{board, secure_code};
		private int[,] Grid = new int[4,10];
		private Rectangle BoardFrame;
		private   Rectangle PegHole = new Rectangle(0, 0, 20, 20);
		private   Rectangle Peg = new Rectangle(0, 0, 25, 25);
		private const int SPACING = 12;
		private const int MARGIN = 10;
		public ScoreBoard TheScore = new ScoreBoard(SPACING, MARGIN + 150,MARGIN + 3);
		public ColorPanel ThePanel = new ColorPanel(200, 140);
		public int CurrentRow = 0;
		public int CurrentPosition = 0;
		public int[] GuessingRow = new int[4];
		private AutoPlayer TheComputer;
        public BFS bfs; // ����� ������ � ������
        public DFS dfs; // ����� ������ � �������
        public EdgeBounds edgeBounds; // ��������� ������ � ������
        public Grad grad;   // ����� ������������ ������

		public Board(Rectangle dimensions)
		{
			//
			// TODO: Add constructor logic here
			//
            bfs = new BFS();
            bfs.board = this;

            dfs = new DFS();
            dfs.board = this;

            edgeBounds = new EdgeBounds();
            edgeBounds.board = this;

            grad = new Grad();
            grad.board = this;

			InitializeBoard();
			BoardFrame = dimensions;

			// set up computer player
			AutoPlayer.SetBoardInfo(this, TheScore);
			TheComputer = new AutoPlayer();
            
		}

        /// <summary>
        /// ����������� ����������� ����
        /// </summary>
        /// <param name="g"></param>
        public void DrawSecretPattern(Graphics g, bool drawSecure)
        {
            Rectangle r;
            const int j = 0;
            for (int i = 0; i < Grid.GetLength(0); i++)
            {
                if (drawSecure)
                {
                    // there is a peg here, draw the colored peg
                    r = new Rectangle(Peg.Left, Peg.Top, Peg.Width, Peg.Height);
                    r.Offset(i * (PegHole.Width + SPACING), j * (PegHole.Height + SPACING) + 20);
                    r.Offset(MARGIN, MARGIN);

                    Brush aBrush = Brushes.Black;

                    // This routine retrieves the brush matching
                    // the integer at the grid point
                    aBrush = GetBrush(GuessingRow[i]);

                    // draw the colored peg with the determined brush

                    Pen aPen = new Pen(aBrush, 1);
                    g.FillEllipse(aBrush, r);
                    g.DrawEllipse(Pens.Black, r);

                    aPen.Dispose();

                }
                else
                {
                    // ���������� ���������
                    //r = new Rectangle(PegHole.Left, PegHole.Top, PegHole.Width, PegHole.Height);
                    r = new Rectangle(Peg.Left, Peg.Top, Peg.Width, Peg.Height);
                    r.Offset(i * (PegHole.Width + SPACING), j * (PegHole.Height + SPACING) + 20);
                    r.Offset(MARGIN, MARGIN);
                    g.DrawEllipse(Pens.Black, r);
                    g.FillEllipse(Brushes.Black, r);

                }
            }
        }

        /// <summary>
        /// ����������� �������� ����
        /// </summary>
        /// <param name="g"></param>
		public void Draw(Graphics g)
		{
			// ���� �� ������� Grid � ����������
			// ���������� �������� � �������� �����
			for (int i = 0; i < Grid.GetLength(0); i++)
			{
				for (int j = 0; j < Grid.GetLength(1); j++)
				{
					// a Grid value of 0 indicates an empty slot
					// draw an empty hole
					if (Grid[i,j] == 0)
					{
						Rectangle r = new Rectangle(PegHole.Left, PegHole.Top, PegHole.Width, PegHole.Height);
						r.Offset(i* (PegHole.Width + SPACING), j* (PegHole.Height + SPACING));
						r.Offset(MARGIN, MARGIN);
						g.DrawEllipse(Pens.Black, r);
						g.FillEllipse(Brushes.Black, r);
					}
					else
					{
						// there is a peg here, draw the colored peg

						Rectangle r = new Rectangle(Peg.Left, Peg.Top, Peg.Width, Peg.Height);
						r.Offset(i* (PegHole.Width + SPACING), j* (PegHole.Height + SPACING));
						r.Offset(MARGIN, MARGIN);

						Brush aBrush = Brushes.Black;

						// This routine retrieves the brush matching
						// the integer at the grid point
						aBrush = GetBrush(Grid[i,j]);

						// draw the colored peg with the determined brush

						Pen aPen = new Pen(aBrush,1);
						g.FillEllipse(aBrush, r);
						g.DrawEllipse(Pens.Black, r);

						aPen.Dispose();
					}
				}
			}

			// draw the score column
			TheScore.Draw(g);

			// draw the color peg choice panel
			ThePanel.Draw(g);
		}

		public static Brush GetBrush(int index)
		{
			Brush aBrush = Brushes.Black;
			switch (index)
			{
				case 1:
					aBrush = Brushes.Aqua;
					break;
				case 2:
					aBrush = Brushes.DarkGreen;
					break;
				case 3:
					aBrush = Brushes.Yellow;
					break;
				case 4:
					aBrush = Brushes.Blue;
					break;
				case 5:
					aBrush = Brushes.Red;
					break;
				case 6:
					aBrush = Brushes.Pink;
					break;
				case 7:
					aBrush = Brushes.DarkOrange;
					break;
				case 8:
					aBrush = Brushes.DarkViolet;
					break;
				default:
					break;
			}

			return aBrush;
		}

		public static Color GetColor(int index)
		{
			Color aBrush = Color.Black;
			switch (index)
			{
				case 1:
					aBrush = Color.Aqua;
					break;
				case 2:
					aBrush = Color.DarkGreen;
					break;
				case 3:
					aBrush = Color.Yellow;
					break;
				case 4:
					aBrush = Color.Blue;
					break;
				case 5:
					aBrush = Color.Red;
					break;
				case 6:
					aBrush = Color.Pink;
					break;
				case 7:
					aBrush = Color.DarkOrange;
					break;
				case 8:
					aBrush = Color.DarkViolet;
					break;
				default:
					break;
			}

			return aBrush;
		}


		public void InitializeBoard()
		{
			for (int i = 0; i < Grid.GetLength(0); i++)
			{
				for (int j = 0; j < Grid.GetLength(1); j++)
				{
					Grid[i,j] = 0;
				}
			}

			CalculateGuessingRow();

            //byte[] pat = new byte[4];
            //for (int i = 0; i < 4; i++)
            //{
            //    pat[i] = (byte)(GuessingRow[i] - 1);
            //}
           
            bfs.BuildScoresTable(); // ������ ������� ����������� ����� ����� ����� ������������
            dfs.scores = bfs.scores;
            grad.scores = bfs.scores;
            MM_base.GameTreeNode tree = bfs.BuildTree(); // ������ ������, ������������ ��� ������� ���� �������� � ������ � �������
            dfs.GameTree = tree;                         // ���� � ���� ������ ������������ ��� ��������� ������� ������
            dfs.number_of_nodes = bfs.number_of_nodes;   // ��������������, ����� ���������� ������ � ������ ���������


		}

		public void CalculateGuessingRow()
		{
			Random r = new Random();
			for (int i = 0; i < 4; i++)
			{
				GuessingRow[i] = r.Next(8) + 1;
			}
		}

		public void PlacePeg(int x, int y, destination dest)
		{
           int myColor = ThePanel.GetColorAt(x,y);
			if (myColor != -1)
			{
			  AddPeg(myColor, dest);
			}
		}

        /// <summary>
        /// ���������� ������� ��������
        /// </summary>
        /// <param name="aColor"></param>
		public void AddPeg(int aColor, destination dest)
		{
            switch (dest)
            {
                case destination.board:
                    Grid[CurrentPosition, CurrentRow] = aColor;
                    break;
                case destination.secure_code:
                    GuessingRow[CurrentPosition] = aColor;
                    break;
            }
		  CurrentPosition ++;
		  if  (CurrentPosition == 4)
			  CurrentPosition = 0;  // ��������� ����������
		}

		public bool AdvanceRow()
		{
			CurrentRow++;
			CurrentPosition = 0;
			if (CurrentRow > 9)
				return false;

			return true;
		}

		public void UndoLastPeg()
		{
			if (CurrentPosition > 0)
			{
			
				CurrentPosition -= 1;
				Grid[CurrentPosition, CurrentRow] = 0;
			}
		}

		public int LetComputerGuess(int nPopulation, int nGenerations)
		{
			int[] rowOfColors = TheComputer.CalculateGeneration(nPopulation, nGenerations);
            int numberOfBlackPegs = 0;
            //int[] rowOfColors = bfs.MakeGuess();
			foreach (int theColor in rowOfColors)
			{
			   this.AddPeg(theColor, destination.board);
			}

            numberOfBlackPegs = CalcScore();

			this.AdvanceRow();
            return numberOfBlackPegs;
		}

		public int CalcScore()
		{
         int nExact = 0;
		 int nThere = 0;
		 int nCount = 0;
			int[] places = new int[4]{-1, -1, -1, -1};
			int[] places2 = new int[4]{-1, -1, -1, -1};

            //if (null != bfs)
            //{
            //    byte[] pat = new byte[4];
            //    for (int i = 0; i < 4; i++)
            //    {
            //        pat[i] = (byte)(GuessingRow[i]-1);
            //    }
            //    bfs.secret_code = bfs.FindPattern(pat);
            //    bfs.Score(pat, ref bfs.CurGuesses[bfs.CurGuessCount]);
            //}

			// do exact first
			for (int i = 0; i < 4; i++)
			{
				if (GuessingRow[i] == Grid[i, CurrentRow])
				{
				  nExact++;
				  TheScore.AddBlackPeg(CurrentRow, nCount);
				  nCount++;
				  places[i] = 1;
				  places2[i] = 1;
				}
			}

			if (nExact == 4)
			{
				return nExact;
			}

			// now do there
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					if ((i != j) && (places[i] != 1) && (places2[j] != 1))
					{
						if (GuessingRow[i] == Grid[j, CurrentRow])
						{
							nThere++;
							TheScore.AddWhitePeg(CurrentRow, nCount);
							nCount++;
							places[i] = 1;
							places2[j] = 1;
							j = 5;  // force a break
						}
					}
				}
			}

			return nExact;

		}

		public int[] CalcScore(int[] autoGuess, int row)
		{
			// used by auto player to calculate score based on guessed row
			int nExact = 0;
			int nThere = 0;
			int nCount = 0;
			int[] places = new int[4]{-1, -1, -1, -1};
			int[] places2 = new int[4]{-1, -1, -1, -1};
			int[] result = new int[4]{0,0,0,0};

			// do exact first
			for (int i = 0; i < 4; i++)
			{
				if (autoGuess[i] == Grid[i, row])
				{
					nExact++;
					result[nCount] = 1;
					nCount++;
					places[i] = 1;
					places2[i] = 1;
				}
			}

			if (nExact == 4)
			{
				return result;
			}

			// now do there
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					if ((i != j) && (places[i] != 1) && (places2[j] != 1))
					{
						if (autoGuess[i] == Grid[j, row])
						{
							nThere++;
							result[nCount] = 2;
							nCount++;
							places[i] = 1;
							places2[j] = 1;
							j = 5;  // force a break
						}
					}
				}
			}

			return result;

		}



	}
}
