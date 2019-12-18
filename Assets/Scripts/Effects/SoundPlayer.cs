using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadsBangH.ArcheryGame
{
	/// <summary>
	/// This class plays some of the one-off sounds which are cumbersome to play at their source
	/// </summary>
	[RequireComponent(typeof(AudioSource))]
	public class SoundPlayer : MonoBehaviour
	{
		[SerializeField]
		private AudioClip targetExplosion = default;
		[SerializeField]
		private AudioClip gameOver = default;
		[SerializeField]
		private AudioClip gameIntro = default;
		[SerializeField]
		private AudioClip targetSpawn = default;
		[SerializeField]
		private AudioClip targetAttack = default;

		private AudioSource audioSource;

		private void Awake()
		{
			audioSource = GetComponent<AudioSource>();
		}

		private void OnEnable()
		{
			ArcheryGame.FirstTargetWasShot += ArcheryGame_FirstTargetWasShot;
			ArcheryGame.TargetWasHit += ArcheryGame_TargetWasHit;
			ArcheryGame.GameLost += ArcheryGame_GameLost;
			ArcheryGame.TargetSpawned += ArcheryGame_TargetSpawned;
			ArcheryGame.TargetIndicatedAttack += ArcheryGame_TargetIndicatedAttack;
		}

		private void OnDisable()
		{
			ArcheryGame.FirstTargetWasShot -= ArcheryGame_FirstTargetWasShot;
			ArcheryGame.TargetWasHit -= ArcheryGame_TargetWasHit;
			ArcheryGame.GameLost -= ArcheryGame_GameLost;
			ArcheryGame.TargetSpawned -= ArcheryGame_TargetSpawned;
			ArcheryGame.TargetIndicatedAttack -= ArcheryGame_TargetIndicatedAttack;
		}

		private void ArcheryGame_FirstTargetWasShot()
		{
			audioSource.PlayOneShot(gameIntro);
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

		private void ArcheryGame_TargetSpawned(Target target)
		{
			audioSource.pitch = Random.Range(0.9f, 1.1f);
			audioSource.PlayOneShot(targetSpawn, 0.5f);
		}

		private void ArcheryGame_TargetIndicatedAttack(Target target)
		{
			audioSource.pitch = Random.Range(0.9f, 1.1f);
			audioSource.PlayOneShot(targetAttack, 0.5f);
		}
	}
}