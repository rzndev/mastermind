using System;

namespace MasterMind
{
	/// <summary>
	/// Summary description for AutoPlayer.
	/// </summary>
	public class AutoPlayer
	{
		private int NumPegs = 5;
		GeneticAlgorithm.Population TheGenePopulation = new MasterMind.MasterMindPopulation();
		public AutoPlayer()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		static public void SetBoardInfo(Board theBoard, ScoreBoard theScore)
		{
			MasterMind.MastermindGenome.TheBoard = theBoard;
			MasterMind.MastermindGenome.TheScore  = theScore;
		}

		public int[] CalculateGeneration(int nPopulation, int nGeneration)
		{
			MasterMind.MasterMindPopulation TestPopulation = new MasterMindPopulation(nPopulation);
//			TestPopulation.WriteNextGeneration();
			for (int i = 0; i < nGeneration; i++)
			{
				TestPopulation.NextGeneration();
//				TestPopulation.WriteNextGeneration();
			} 

			int[] bestGenome = ((MastermindGenome)TestPopulation.GetHighestScoreGenome()).ToArray();
			return bestGenome;
			
		}
	}
}
