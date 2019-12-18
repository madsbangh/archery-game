using System;
using System.Collections;
using UnityEngine;

namespace MadsBangH.ArcheryGame
{
	[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(Rigidbody2D))]
	public class Target : MonoBehaviour
	{
		private static readonly int IndicateAttackHash = Animator.StringToHash("Indicate Attack");

		public bool willAttack;

		[SerializeField]
		private GameObject deathExplosionPrefab = default;

		private Animator animator;
		private Rigidbody2D rb;

		private static float AttackMovementSpeed => 0.25f + 0.1f * ArcheryGame.CurrentScore;
		private static float DelayBeforeAttack => 5f * Mathf.Pow(0.9f, ArcheryGame.CurrentScore);

		private void Awake()
		{
			animator = GetComponent<Animator>();
			rb = GetComponent<Rigidbody2D>();
		}

		private void OnEnable()
		{
			ArcheryGame.GameLost += ArcheryGame_GameLost;
		}

		private void OnDisable()
		{
			ArcheryGame.GameLost -= ArcheryGame_GameLost;
		}

		private void ArcheryGame_GameLost()
		{
			Destroy(gameObject);
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.CompareTag(Tags.Arrow))
			{
				Destroy(gameObject);
				ArcheryGame.NotifyTargetWasHit(this);
				Instantiate(deathExplosionPrefab, transform.position, Quaternion.identity);
			}
			else if (collision.CompareTag(Tags.Player))
			{
				Destroy(gameObject);
			}
		}

		private IEnumerator Start()
		{
			if (willAttack)
			{
				yield return new WaitForSeconds(DelayBeforeAttack);
				StartCoroutine(IndicateAndAttackCoroutine());
			}
		}

		private IEnumerator IndicateAndAttackCoroutine()
		{
			animator.SetTrigger(IndicateAttackHash);

			ArcheryGame.NotifyTargetIndicatedAttack(this);

			yield return new WaitForSeconds(1f);

			rb.velocity = (ArcheryPlayer.Position - rb.position).normalized * AttackMovementSpeed;
		}
	}
}