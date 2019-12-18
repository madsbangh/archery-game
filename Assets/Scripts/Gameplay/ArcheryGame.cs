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
		public static event Action<Target> TargetSpawned;
		public static event Action<Target> TargetIndicatedAttack;

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
			if (startNewGameOnTap && Input.GetMouseButtonDown(0))
			{
				startNewGameOnTap = false;
				StartNewGame();
			}
		}

		private void OnEnable()
		{
			GameLost += ArcheryGame_GameLost;
		}

		private void OnDisable()
		{
			GameLost -= ArcheryGame_GameLost;
		}

		private void ArcheryGame_GameLost()
		{
			// Enable tap to restart after 2 seconds when game is lost
			StartCoroutine(PerformActionAfterSecondsCoroutine(() => startNewGameOnTap = true, 2f));
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
		}

		public static void NotifyTargetWasSpawned(Target target)
		{
			TargetSpawned?.Invoke(target);
		}

		public static void NotifyTargetIndicatedAttack(Target target)
		{
			TargetIndicatedAttack?.Invoke(target);
		}

		private IEnumerator PerformActionAfterSecondsCoroutine(Action action, float seconds)
		{
			yield return new WaitForSeconds(seconds);
			action?.Invoke();
		}
	}
}