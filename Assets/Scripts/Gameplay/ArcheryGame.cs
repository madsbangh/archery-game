using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadsBangH.ArcheryGame
{
	public static class ArcheryGame
	{
		public static event Action ScoreChanged;
		public static event Action GameLost;

		private static int currentScore;
		public static int CurrentScore
		{
			get => currentScore;
			private set
			{
				currentScore = value;
				ScoreChanged?.Invoke();
			}
		}

		public static int Highscore { get; private set; }

		public static void NotifyTargetWasHit(Target target)
		{
			CurrentScore++;
		}

		public static void NotifyPlayerWasHit()
		{
			if (CurrentScore > Highscore)
			{
				Highscore = CurrentScore;
			}

			GameLost?.Invoke();

			CurrentScore = 0;
		}
	}
}