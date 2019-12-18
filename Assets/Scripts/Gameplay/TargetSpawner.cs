using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadsBangH.ArcheryGame
{
	/// <summary>
	/// This component spawns targets at the positions of immediate children.
	/// It spawns a target which does not attach at the given first target spawn point.
	/// </summary>
	public class TargetSpawner : MonoBehaviour
	{
		[SerializeField]
		private Transform firstTargetSpawnPoint = default;
		[SerializeField]
		private Target targetPrefab = default;

		private bool willSpawnTargets = default;

		private static float SpawnInterval => 4f * Mathf.Pow(0.95f, ArcheryGame.CurrentScore);

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.magenta;
			for (int i = 0; i < transform.childCount; i++)
			{
				Gizmos.DrawWireSphere(transform.GetChild(i).position, 1f);
			}

			Gizmos.color = Color.cyan;
			Gizmos.DrawWireSphere(firstTargetSpawnPoint.position, 1f);
		}

		private void OnEnable()
		{
			ArcheryGame.NewGameStarted += ArcheryGame_NewGameStarted;
			ArcheryGame.GameLost += ArcheryGame_GameLost;
			ArcheryGame.FirstTargetWasShot += ArcheryGame_FirstTargetWasShot;
		}

		private void OnDisable()
		{
			ArcheryGame.NewGameStarted -= ArcheryGame_NewGameStarted;
			ArcheryGame.GameLost -= ArcheryGame_GameLost;
			ArcheryGame.FirstTargetWasShot -= ArcheryGame_FirstTargetWasShot;
		}

		private void ArcheryGame_NewGameStarted()
		{
			Target firstTarget = Instantiate(targetPrefab, firstTargetSpawnPoint.position, Quaternion.identity);
			firstTarget.willAttack = false;
		}

		private void ArcheryGame_GameLost()
		{
			willSpawnTargets = false;
		}

		private void ArcheryGame_FirstTargetWasShot()
		{
			willSpawnTargets = true;
			StartCoroutine(SpawnTargetsCoroutine());
		}

		private IEnumerator SpawnTargetsCoroutine()
		{
			// Wait a little bit after hitting the first target until we spawn the second one
			yield return new WaitForSeconds(1f);

			while (willSpawnTargets)
			{
				Target spawnedTarget = Instantiate(targetPrefab, GetRandomSpawnPosition(), Quaternion.identity);
				spawnedTarget.willAttack = true;
				ArcheryGame.NotifyTargetWasSpawned(spawnedTarget);

				yield return new WaitForSeconds(SpawnInterval);
			}
		}

		private Vector3 GetRandomSpawnPosition()
		{
			int randomIndex = UnityEngine.Random.Range(0, transform.childCount);
			return transform.GetChild(randomIndex).position;
		}
	}
}