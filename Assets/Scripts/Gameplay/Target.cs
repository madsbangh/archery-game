﻿using System;
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
		private string ArrowTag = default;

		private Animator animator;
		private Rigidbody2D rb;

		private static float AttackMovementSpeed => 0.25f + 0.1f * ArcheryGame.CurrentScore;
		private static float DelayBeforeAttack => 5f * Mathf.Pow(0.9f, ArcheryGame.CurrentScore);

		private void Awake()
		{
			animator = GetComponent<Animator>();
			rb = GetComponent<Rigidbody2D>();
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.CompareTag(ArrowTag))
			{
				Destroy(gameObject);
				ArcheryGame.NotifyTargetWasHit(this);
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

			yield return new WaitForSeconds(1f);

			rb.velocity = (ArcheryPlayer.Position - rb.position).normalized * AttackMovementSpeed;
		}
	}
}