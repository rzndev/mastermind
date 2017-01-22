using System;
using System.Collections;
using GeneticAlgorithm;

namespace MasterMind
{


	/// <summary>
	/// 
	/// </summary>
	public class MastermindGenome : GeneticAlgorithm.ListGenome
	{

		public static MasterMind.Board TheBoard;
		public static MasterMind.ScoreBoard TheScore;

		public MastermindGenome()
		{
			// 
			// TODO: Add constructor logic here
			//
		}

		public MastermindGenome(long length, object min, object max) : base(length, min, max)
		{
			//
			// TODO: Add constructor logic here
			//
			
		}


		private int[] GetIntArray(ArrayList a)
		{
			int[] result = new int[4]{0,0,0,0};
			for (int i = 0; i < a.Count; i++)
			{
				result[i] = (int)a[i];
			}

			return result;
		}


		private int   CompareToScore(int nCompareRow, int[] testpegs)
		{
			int nScoreMatch = 0;
			for (int i = 0; i < 4; i++)
			{
				int nPeg = TheScore.GetPeg(i, nCompareRow);
				if (testpegs[i] == nPeg)
				{
					nScoreMatch++;
				}
			}

			return nScoreMatch;
		}
		private float CalculateFromMastermindBoard()
		{

			float fFitnessScore = 0.0f;
			for (int i = 0; i < TheBoard.CurrentRow; i++)
			{
				int[] result = TheBoard.CalcScore(GetIntArray(TheArray), i); 	
				int numCorrectInRow = CompareToScore(i, result);
				fFitnessScore += ((float)numCorrectInRow)/4.0f;
			}

			fFitnessScore += .02f;

			return fFitnessScore;
		}

		public override float CalculateFitness()
		{
		CurrentFitness = CalculateFromMastermindBoard();
			//			CurrentFitness = CalculateClosestRatioToPi();
			//			CurrentFitness = CalculateNumbersProducingProductsWithSameDigitsAsFirst();
			//			CurrentFitness = CalculateClosestProductSum();
			//			CurrentFitness =  CalculateClosestSumTo10();
			return CurrentFitness;
		}

		public override string ToString()
		{
			string strResult = "";
			for (int i = 0; i < Length; i++)
			{
				strResult = strResult + Board.GetColor(((int)TheArray[i])).ToString() + " ";
			}

			strResult += "-->" + CurrentFitness.ToString();

			return strResult;
		}

		public override void CopyGeneInfo(Genome dest)
		{
			MastermindGenome theGene = (MastermindGenome)dest;
			theGene.Length = Length;
			theGene.TheMin = TheMin;
			theGene.TheMax = TheMax;
		}


		public override Genome Crossover(Genome g)
		{
			MastermindGenome aGene1 = new MastermindGenome();
			MastermindGenome aGene2 = new MastermindGenome();
			g.CopyGeneInfo(aGene1);
			g.CopyGeneInfo(aGene2);


			MastermindGenome CrossingGene = (MastermindGenome)g;
			for (int i = 0; i < CrossoverPoint; i++)
			{
				aGene1.TheArray.Add(CrossingGene.TheArray[i]);
				aGene2.TheArray.Add(TheArray[i]);
			}

			for (int j = CrossoverPoint; j < Length; j++)
			{
				aGene1.TheArray.Add(TheArray[j]);
				aGene2.TheArray.Add(CrossingGene.TheArray[j]);
			}

			// 50/50 chance of returning gene1 or gene2
			MastermindGenome aGene = null;
			if (TheSeed.Next(2) == 1)			
			{
				aGene = aGene1;
			}
			else
			{
				aGene = aGene2;
			}

			return aGene;
		}



	}
}
