using Game.Combat;
using Game.Combat.Reserves;
using Game.Combat.Utilities;
using Game.Stats;
using Game.UI;
using UnityEngine;

namespace Game.Entity
{
    [RequireComponent(typeof(HealthReserve))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Enemy : MonoBehaviour, IEnemy, ICombatTarget
    {
        public event DamageTaken DamageTaken;
        public event Died Died;

        public GameObject DamageIndicatorPrefab;

        [SerializeField]
        private EntityStatsObject enemyStats;

        public EntityStatsObject Stats
        {
            get
            {
                return enemyStats;
            }
            set
            {
                enemyStats = value;
            }
        }

        public IReserve Health
        {
            get
            {
                return (IReserve)healthReserve;
            }

            set
            {
                healthReserve = (HealthReserve)value;
            }
        }

        /// <summary>
        /// The CombatTarget's health reserve.
        /// </summary>
        private HealthReserve healthReserve;

        private Rigidbody2D rb2D;

        private void Awake()
        {
            Health = GetComponent<HealthReserve>();
            rb2D = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            Health.Max = Stats.MaxHealth;
        }

        public void OnEnable()
        {
            Health.Empty += Die;
        }
        public void OnDisable()
        {
            Health.Empty -= Die;
        }

        private void FixedUpdate()
        {
            // TODO: This behaviour should be specific to stationary enemies.
            if (Mathf.Abs(rb2D.velocity.x) > 0.1f ||
                Mathf.Abs(rb2D.velocity.y) > 0.1f)
            {
                float newVelX = Mathf.Lerp(rb2D.velocity.x,
                0.0f, 5.0f * Time.fixedDeltaTime); //TODO: NO HARDCODING
                if (Mathf.Abs(newVelX) < 0.1f)
                {
                    newVelX = 0.0f;
                }

                float newVelY = Mathf.Lerp(rb2D.velocity.y,
                    0.0f, 5.0f * Time.fixedDeltaTime); //TODO: NO HARDCODING
                if (Mathf.Abs(newVelY) < 0.1f)
                {
                    newVelY = 0.0f;
                }
                rb2D.velocity = new Vector2(newVelX, newVelY);
            }
        }

        public void Die()
        {
            Destroy(gameObject);
            Died?.Invoke();
        }

        public void TakeDamage(float howMuchDamage)
        {
            Health.Modify(-howMuchDamage);
            DamageTaken?.Invoke();

            DamageIndicator damageIndicator = 
                Instantiate(DamageIndicatorPrefab, transform.position,
                Quaternion.identity).GetComponent<DamageIndicator>();
            damageIndicator.SetDamageText((int)Mathf.Floor(howMuchDamage));
        }
    }
}
