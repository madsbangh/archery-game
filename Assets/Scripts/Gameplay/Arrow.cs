﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadsBangH.ArcheryGame
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class Arrow : MonoBehaviour
	{
		private Rigidbody2D rb;

		private void Awake()
		{
			rb = GetComponent<Rigidbody2D>();
		}

		// Destroy self after a while
		private IEnumerator Start()
		{
			yield return new WaitForSeconds(3f);
			Destroy(gameObject);
		}

		public void SetPositionRotationAndSpeed(Vector3 position, float rotation, float speed)
		{
			rb.position = position;
			rb.rotation = rotation - 90f;
			rb.velocity = Vector2.zero;
			rb.AddRelativeForce(Vector2.up * speed, ForceMode2D.Impulse);

			transform.SetPositionAndRotation(rb.position, Quaternion.Euler(0f, 0f, rb.rotation));
		}
	}
}
