using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadsBangH.ArcheryGame
{
	[RequireComponent(typeof(AudioSource))]
	public class SoundPlayer : MonoBehaviour
	{
		[SerializeField]
		private AudioClip targetExplosion = default;
		[SerializeField]
		private AudioClip gameOver = default;

		private AudioSource audioSource;

		private void Awake()
		{
			audioSource = GetComponent<AudioSource>();
		}

		private void OnEnable()
		{
			ArcheryGame.TargetWasHit += ArcheryGame_TargetWasHit;
			ArcheryGame.GameLost += ArcheryGame_GameLost;
		}

		private void OnDisable()
		{
			ArcheryGame.TargetWasHit -= ArcheryGame_TargetWasHit;
			ArcheryGame.GameLost -= ArcheryGame_GameLost;
		}

		private void ArcheryGame_TargetWasHit(Target target)
		{
			audioSource.pitch = Random.Range(0.95f, 1.05f) + ArcheryGame.CurrentScore * 0.01f;
			audioSource.PlayOneShot(targetExplosion);
		}

		private void ArcheryGame_GameLost()
		{
			audioSource.pitch = Random.Range(0.98f, 1.02f);
			audioSource.PlayOneShot(gameOver);
		}
	}
}