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
		private const float MinDragDistance = 1f;
		private const float MaxDragDistance = 3f;
		private const float dragThreshold = 0.1f;

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
		private bool isReloaded;
		private float lastShotTime;

		private void Start()
		{
			isPullingBow = false;
			isReloaded = true;
		}

		private void Update()
		{
			// Reload if cooldown has passed
			if (Time.time > lastShotTime + ReloadCooldownSeconds)
			{
				isReloaded = true;
				arrowOnBow.SetActive(true);
			}

			if (isPullingBow)
			{
				Vector2 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				Vector2 position2D = new Vector2(transform.position.x, transform.position.y);
				Vector2 pullVector = position2D - worldMousePosition;
				float angle = Mathf.Atan2(pullVector.y, pullVector.x) * Mathf.Rad2Deg;
				float stretchAmount = Mathf.Clamp01(Mathf.InverseLerp(MinDragDistance, MaxDragDistance, pullVector.magnitude));
				if (stretchAmount > dragThreshold)
				{
					UpdateBowGraphics(angle, stretchAmount);
				}

				if (Input.GetMouseButtonUp(0))
				{
					if (stretchAmount > dragThreshold)
					{
						PerformShot(angle, stretchAmount);
					}
					isPullingBow = false;
				}
			}
			else
			{
				if (Input.GetMouseButtonDown(0))
				{
					isPullingBow = true;
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
			lastShotTime = Time.time;
			isReloaded = false;

			arrowOnBow.SetActive(false);
			bowAnimator.SetFloat(StretchAmountHash, 0f);

			var arrowProjectile = Instantiate(arrowProjectilePrefab);
			float speed = (ShotForceBase + stretchAmount * ShotForceMultiplier);
			arrowProjectile.SetPositionRotationAndSpeed(arrowOnBow.transform.position, angle, speed);
		}
	}
}
