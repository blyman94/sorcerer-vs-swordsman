using Game.Entity;
using System.Collections;
using UnityEngine;

namespace Game.UI
{
    public class CameraShake : MonoBehaviour
    {
        /// <summary>
		/// Duration of the camera shake.
		/// </summary>
		public float shakeDuration = 0.1f;

		/// <summary>
		/// Magnitude of screen position deviation during shake.
		/// </summary>
		public float shakeIntensity = 0.1f;

		/// <summary>
		/// Position of the camera before any shaking.
		/// </summary>
		private Vector3 startPos;

        [SerializeField]
        private Player player;
		private void Awake()
		{
			startPos = transform.position;
		}

		private void OnEnable()
		{
			player.MeleeFighter.MeleeWeapon.AttackSuccessful += StartCameraShake;
            player.CombatTarget.DamageTaken += StartCameraShake;
		}

		private void OnDisable()
		{
			player.MeleeFighter.MeleeWeapon.AttackSuccessful -= StartCameraShake;
            player.CombatTarget.DamageTaken -= StartCameraShake;

		}

		/// <summary>
		/// Starts the camera shake routine on successful attack.
		/// </summary>
		/// <param name="grounded">UNUSED.</param>
		private void StartCameraShake(bool grounded)
		{
            StartCoroutine(CameraShakeRoutine());
		}
        /// <summary>
		/// Starts the camera shake routine on player hit.
		/// </summary>
		/// <param name="grounded">UNUSED.</param>
		private void StartCameraShake()
		{
            StartCoroutine(CameraShakeRoutine());
		}

		/// <summary>
		/// Coroutine to shake the camera driven by Random.insideUnitSphere.
		/// </summary>
		/// <returns>Yield returns null over the shake duration.</returns>
		private IEnumerator CameraShakeRoutine()
		{
			float elapsedTime = 0.0f;
			while (elapsedTime < shakeDuration)
			{
                Vector2 newRandomPos = Random.insideUnitCircle * shakeIntensity;
				transform.position = startPos + new Vector3(newRandomPos.x,newRandomPos.y, startPos.z);
				elapsedTime += Time.deltaTime;
				yield return null;
			}
			transform.position = startPos;
		}
    }
}
