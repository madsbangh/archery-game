using System.Collections;
using UnityEngine;

namespace MadsBangH.ArcheryGame
{
	public class DestroyAfterSeconds : MonoBehaviour
	{
		[SerializeField]
		private float seconds;

		private IEnumerator Start()
		{
			yield return new WaitForSeconds(seconds);

			Destroy(gameObject);
		}
	}
}