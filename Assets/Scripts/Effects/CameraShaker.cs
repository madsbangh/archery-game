using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadsBangH.ArcheryGame
{
	public class CameraShaker : MonoBehaviour
	{
		[SerializeField]
		private float springForce = default;
		[SerializeField]
		private float damping = default;
		[SerializeField]
		private float targetImpactShakeFactor = default;
		[SerializeField]
		private float playerImpactShakeFactor = default;

		private Vector2 velocity;

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
			velocity += (ArcheryPlayer.Position - (Vector2)target.transform.position).normalized * targetImpactShakeFactor;
		}

		private void ArcheryGame_GameLost()
		{
			velocity += Vector2.up * playerImpactShakeFactor;
		}

		private void FixedUpdate()
		{
			Vector2 position = transform.position;

			velocity += springForce * -position;
			velocity *= damping;

			position += velocity;

			transform.position = new Vector3(position.x, position.y, transform.position.z);
		}
	}
}