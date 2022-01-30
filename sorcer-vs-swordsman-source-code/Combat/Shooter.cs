using Game.Data;
using Game.Stats;
using UnityEngine;
using System.Collections;

namespace Game.Combat
{
    /// <summary>
    /// Utilizes the PlayerPosition data object to fire projectiles at the
    /// player, dealing damage if collision is successful.
    /// </summary>
    public class Shooter : MonoBehaviour
    {
        public delegate void ShotFired();
        public ShotFired shotFired;

        public delegate void MegaShot();
        public MegaShot megaShot;

        public EntityStatsObject Stats { get; set; }

        [Tooltip("Time in between shots.")]
        public float ShotCooldown;

        [Tooltip("Activated when shot is fired.")]
        public GameObject ProjectilePrefab;
        public GameObject LargeProjectilePrefab;

        [Tooltip("Speed of the projectile when fired.")]
        public float ProjectileSpeed;

        [Tooltip("Projectile pool parent transform.")]
        public Transform ProjectilePoolTransform;

        [Tooltip("Number of projectiles to instatiate at startup. For best " +
            "performance, use the minimum count required by the firing behaviour.")]
        public int ProjectilePoolSize;

        [Header("Audio")]
        public AudioSource ShooterAudio;
        public AudioClip FireballCastClip;
        public AudioClip MegaAttackClip;

        /// <summary>
        /// Object pool for projectiles.
        /// </summary>
        private GameObject[] projectilePool;

        private GameObject[] largeProjectilePool;

        private Vector3[] directions;

        private void Start()
        {
            directions = new Vector3[8]
            {
                new Vector3(0,0,90), new Vector3(0,0,45), Vector3.zero,
                new Vector3(0,0,-45), new Vector3(0,0,-90), new Vector3(0,0,-135),
                new Vector3(0,0,-180), new Vector3(0,0,-225)
            };
            InitializeProjectilePools();
        }

        private void FireProjectile()
        {
            int randomAttackIndex = Random.Range(0, 3);
            switch (randomAttackIndex)
            {
                case 0:
                    StartCoroutine(FireProjectileRoutine());
                    break;
                case 1:
                    StartCoroutine(MegaAttackRoutine());
                    break;
                case 2:
                    StartCoroutine(BurstFireProjectileRoutine());
                    break;
            }
        }

        private void PlayFireballCastAudio()
        {
            ShooterAudio.PlayOneShot(FireballCastClip, 0.75f);
        }

        private void PlayMegaAttackAudio()
        {
            ShooterAudio.PlayOneShot(MegaAttackClip, 0.75f);
        }

        /// <summary>
        /// Attemps to retrieve a projectile from the pool. If successful,
        /// activates, rotates, and fires the projectile toward the player
        /// position.
        /// </summary>
        private IEnumerator FireProjectileRoutine()
        {
            shotFired?.Invoke();
            PlayFireballCastAudio();
            yield return new WaitForSeconds(0.35f);
            GameObject projectileObject = GetProjectile();
            if (projectileObject != null)
            {
                projectileObject.transform.position = transform.position;
                Projectile projectile = projectileObject.GetComponent<Projectile>();
                projectile.RotateTowardsTarget(PlayerPosition.Current);
                projectile.GetComponent<Projectile>().ProjectileDamage =
                    Stats.Damage;
                Rigidbody2D projectileRb = projectileObject.GetComponent<Rigidbody2D>();
                projectileObject.SetActive(true);
                projectileRb.velocity =
                    projectileObject.transform.right * ProjectileSpeed;
            }
            else
            {
                yield break;
            }
        }

        /// <summary>
        /// Attemps to retrieve a projectile from the pool. If successful,
        /// activates, rotates, and fires the projectile toward the player
        /// position.
        /// </summary>
        private IEnumerator BurstFireProjectileRoutine()
        {
            for (int i = 0; i < 3; i++)
            {
                shotFired?.Invoke();
                PlayFireballCastAudio();
                yield return new WaitForSeconds(0.35f);
                GameObject projectileObject = GetProjectile();
                if (projectileObject != null)
                {
                    projectileObject.transform.position = transform.position;
                    Projectile projectile = projectileObject.GetComponent<Projectile>();
                    projectile.RotateTowardsTarget(PlayerPosition.Current);
                    projectile.GetComponent<Projectile>().ProjectileDamage =
                        Stats.Damage;
                    Rigidbody2D projectileRb = projectileObject.GetComponent<Rigidbody2D>();
                    projectileObject.SetActive(true);
                    projectileRb.velocity =
                        projectileObject.transform.right * ProjectileSpeed;
                }
                else
                {
                    yield break;
                }
            }

        }

        private IEnumerator MegaAttackRoutine()
        {
            megaShot?.Invoke();
            PlayMegaAttackAudio();
            yield return new WaitForSeconds(0.35f);
            for (int i = 0; i < largeProjectilePool.Length; i++)
            {
                largeProjectilePool[i].transform.position = transform.position;
                Projectile projectile = largeProjectilePool[i].GetComponent<Projectile>();
                projectile.transform.rotation = Quaternion.Euler(directions[i]);
                projectile.GetComponent<Projectile>().ProjectileDamage =
                    Stats.Damage * 2;
                Rigidbody2D projectileRb = largeProjectilePool[i].GetComponent<Rigidbody2D>();
                largeProjectilePool[i].SetActive(true);
                projectileRb.velocity =
                    largeProjectilePool[i].transform.right * ProjectileSpeed * .25f;
                yield return null;
            }


        }

        /// <summary>
        /// Creates an object pool of projectiles.
        /// </summary>
        private void InitializeProjectilePools()
        {
            projectilePool = new GameObject[ProjectilePoolSize];
            largeProjectilePool = new GameObject[ProjectilePoolSize];

            for (int i = 0; i < ProjectilePoolSize; i++)
            {
                GameObject projectileObject =
                    Instantiate(ProjectilePrefab, ProjectilePoolTransform);
                projectileObject.SetActive(false);
                projectilePool[i] = projectileObject;

                GameObject largeProjectileObject =
                    Instantiate(LargeProjectilePrefab, ProjectilePoolTransform);
                largeProjectileObject.SetActive(false);
                largeProjectilePool[i] = largeProjectileObject;
            }
        }

        /// <summary>
        /// Returns a reference to a projectile in the projectile pool if it is not
        /// active (and therefore available to use for the shot).
        /// </summary>
        /// <returns></returns>
        private GameObject GetProjectile()
        {
            for (int i = 0; i < ProjectilePoolSize; i++)
            {
                if (projectilePool[i].activeInHierarchy)
                {
                    continue;
                }
                else
                {
                    return projectilePool[i];
                }
            }
            return null;
        }
        private GameObject GetLargeProjectile()
        {
            for (int i = 0; i < ProjectilePoolSize; i++)
            {
                if (largeProjectilePool[i].activeInHierarchy)
                {
                    continue;
                }
                else
                {
                    return largeProjectilePool[i];
                }
            }
            return null;
        }
    }
}
