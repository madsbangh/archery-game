using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MadsBangH.ArcheryGame
{
	public class GameCanvasController : MonoBehaviour
	{
		private static readonly int ScoreBumpedHash = Animator.StringToHash("Score Bumped");

		[SerializeField]
		private TMP_Text scoreLabel = default;
		[SerializeField]
		private Animator scoreLabelAnimator = default;

		private void OnEnable()
		{
			ArcheryGame.ScoreIncreased += ArcheryGame_ScoreChanged;
		}

		private void OnDisable()
		{
			ArcheryGame.ScoreIncreased -= ArcheryGame_ScoreChanged;
		}

		private void ArcheryGame_ScoreChanged()
		{
			scoreLabel.text = ArcheryGame.CurrentScore.ToString();
			scoreLabelAnimator.SetTrigger(ScoreBumpedHash);
		}
	}
}
