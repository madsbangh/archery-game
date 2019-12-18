using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadsBangH.ArcheryGame
{
	public class ArcheryGame : MonoBehaviour
	{
		public static event Action ScoreIncreased;
		public static event Action GameLost;
		public static event Action NewGameStarted;
		public static event Action FirstTargetWasShot;
		public static event Action<Target> TargetWasHit;

		private static bool isWaitingForFirstTargetShot;
		private static bool startNewGameOnTap;

		public static int CurrentScore { get; private set; }
		public static int Highscore { get; private set; }

		private void Start()
		{
			StartNewGame();
		}

		private void Update()
		{
			if (startNewGameOnTap && Input.GetMouseButtonUp(0))
			{
				startNewGameOnTap = false;
				StartNewGame();
			}
		}

		public static void StartNewGame()
		{
			isWaitingForFirstTargetShot = true;
			NewGameStarted?.Invoke();
		}

		public static void NotifyTargetWasHit(Target target)
		{
			TargetWasHit?.Invoke(target);

			CurrentScore++;
			ScoreIncreased?.Invoke();

			if (isWaitingForFirstTargetShot)
			{
				isWaitingForFirstTargetShot = false;
				FirstTargetWasShot?.Invoke();
			}
		}

		public static void NotifyPlayerWasHit()
		{
			if (CurrentScore > Highscore)
			{
				Highscore = CurrentScore;
			}

			GameLost?.Invoke();

			CurrentScore = 0;

			startNewGameOnTap = true;
		}
	}
}