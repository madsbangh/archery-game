using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadsBangH.ArcheryGame
{
	public class ArcheryPlayer : MonoBehaviour
	{
		private const float ShotForceMultiplier = 16f;
		private const float ShotForceBase = 4f;
		private const float ReloadCooldownSeconds = 0.5f;
		private const float MinDragDistance = 0.25f;
		private const float MaxDragDistance = 2f;

		private static readonly int StretchAmountHash = Animator.StringToHash("Stretch Amount");

		[SerializeField]
		private Transform bow = default;
		[SerializeField]
		private Animator bowAnimator = default;
		[SerializeField]
		private GameObject arrowOnBow = default;
		[SerializeField]
		private Arrow arrowProjectilePrefab = default;

		private bool isPullingBow;
		private float pullStartDistance;
		private bool isReloaded;
		private float lastShotTime;

		private bool allowShooting;

		private static ArcheryPlayer instance;
		public static Vector2 Position
		{
			get
			{
				if (instance == null)
				{
					instance = FindObjectOfType<ArcheryPlayer>();
					if (instance == null)
					{
						return Vector2.zero;
					}
				}
				return instance.transform.position;
			}
		}

		private void Start()
		{
			isPullingBow = false;
			isReloaded = true;
		}

		private void OnEnable()
		{
			ArcheryGame.NewGameStarted += ArcheryGame_NewGameStarted;
			ArcheryGame.GameLost += ArcheryGame_GameLost;
		}

		private void OnDisable()
		{
			ArcheryGame.NewGameStarted -= ArcheryGame_NewGameStarted;
			ArcheryGame.GameLost -= ArcheryGame_GameLost;
		}

		private void ArcheryGame_NewGameStarted()
		{
			allowShooting = true;
		}

		private void ArcheryGame_GameLost()
		{
			allowShooting = false;
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.CompareTag(Tags.Target))
			{
				ArcheryGame.NotifyPlayerWasHit();
			}
		}

		private void Update()
		{
			if (!allowShooting)
			{
				isPullingBow = false;
				arrowOnBow.SetActive(false);
				return;
			}

			// Reload if cooldown has passed
			if (Time.time > lastShotTime + ReloadCooldownSeconds)
			{
				isReloaded = true;
				arrowOnBow.SetActive(true);
			}

			Vector2 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 position2D = new Vector2(transform.position.x, transform.position.y);
			Vector2 pullVector = position2D - worldMousePosition;
			if (isPullingBow)
			{
				float angle = Mathf.Atan2(pullVector.y, pullVector.x) * Mathf.Rad2Deg;
				float pullMagnitudeDifference = pullVector.magnitude - pullStartDistance;
				float pullAmount = Mathf.Clamp01(Mathf.InverseLerp(MinDragDistance, MaxDragDistance, pullMagnitudeDifference));
				UpdateBowGraphics(angle, pullAmount);

				if (isReloaded && Input.GetMouseButtonUp(0))
				{
					PerformShot(angle, pullAmount);
					isPullingBow = false;
				}
			}
			else
			{
				if (Input.GetMouseButtonDown(0))
				{
					isPullingBow = true;
					pullStartDistance = pullVector.magnitude;
				}
			}
		}

		private void UpdateBowGraphics(float angle, float stretchAmount)
		{
			bow.eulerAngles = new Vector3(0f, 0f, angle);

			bowAnimator.SetFloat(StretchAmountHash, stretchAmount);
		}

		private void PerformShot(float angle, float stretchAmount)
		{
			isReloaded = false;
			lastShotTime = Time.time;

			arrowOnBow.SetActive(false);
			bowAnimator.SetFloat(StretchAmountHash, 0f);

			var arrowProjectile = Instantiate(arrowProjectilePrefab);
			float speed = (ShotForceBase + stretchAmount * ShotForceMultiplier);
			arrowProjectile.SetPositionRotationAndSpeed(arrowOnBow.transform.position, angle, speed);
		}
	}
}
