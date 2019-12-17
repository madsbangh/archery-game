using UnityEngine;

namespace MadsBangH.ArcheryGame
{
	public class Target : MonoBehaviour
	{
		[SerializeField]
		private string ArrowTag = default;

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.CompareTag(ArrowTag))
			{
				Destroy(gameObject);
				ArcheryGame.NotifyTargetWasHit(this);
			}
		}
	}
}