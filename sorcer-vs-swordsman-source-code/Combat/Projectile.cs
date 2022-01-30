using Game.Entity;
using UnityEngine;

namespace Game.Combat
{
    /// <summary>
    /// Projectile behaviour. Includes OnTriggerEnter2D and RotateTowardsTarget
    /// methods.
    /// </summary>
    public class Projectile : MonoBehaviour
    {
        /// <summary>
        /// Damage projectile inflicts upon valid targets.
        /// </summary>
        public float ProjectileDamage { get; set; }

        public AudioSource ProjectileAudio;

        public AudioClip FireballSnuffClip;
        public AudioClip FireballImpactClip;

        public float impactAudioVolume;

        private void PlayImpactClip()
        {
            AudioSource.PlayClipAtPoint(FireballSnuffClip,transform.position,0.25f);
        }

        /// <summary>
        /// Rotates the projectile toward a passed target vector.
        /// </summary>
        /// <param name="target">Target to rotate towards.</param>
        public void RotateTowardsTarget(Vector2 target)
        {
            Vector2 projectilePos = (Vector2)transform.position;
            target.x = target.x - projectilePos.x;
            target.y = target.y - projectilePos.y;
            float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<Player>().CombatTarget.TakeDamage(ProjectileDamage);
                other.GetComponent<Player>().PlayImpactAudio();
            }
            PlayImpactClip();
            gameObject.SetActive(false);
        }
    }
}
