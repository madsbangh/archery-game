using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadsBangH.ArcheryGame
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class Arrow : MonoBehaviour
	{
		private const float DestroyDelay = 1f;

		[SerializeField]
		private Transform arrowHead = default;
		[SerializeField]
		private float arrowHeadDownForce = default;

		private Rigidbody2D rb;

		private void Awake()
		{
			rb = GetComponent<Rigidbody2D>();
		}

		private void FixedUpdate()
		{
			rb.AddForceAtPosition(Vector2.down * arrowHeadDownForce, arrowHead.position);
		}

		private void OnBecameInvisible()
		{
			Destroy(gameObject);
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.CompareTag(Tags.Target))
			{
				Destroy(gameObject);
			}
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
