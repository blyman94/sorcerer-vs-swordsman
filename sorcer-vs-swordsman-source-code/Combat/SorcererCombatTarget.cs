using System.Collections;
using System.Collections.Generic;
using Game.Combat.Reserves;
using Game.Combat.Utilities;
using Game.Stats;
using UnityEngine;

namespace Game.Combat
{
    [RequireComponent(typeof(HealthReserve))]
    public class SorcererCombatTarget : MonoBehaviour, ICombatTarget
    {
        public event DamageTaken DamageTaken;
        public event Died Died;
        public static event RequestDamagePopup RequestDamagePopup;

        public EntityStatsObject Stats { get; set; }

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

        public void Awake()
        {
            Health = GetComponent<HealthReserve>();

            Health.Empty += Die;
        }

        public void OnDisable()
        {
            DamageTaken = null;
            Died = null;
        }

		private void Start()
        {
            Health.Max = Stats.MaxHealth;
        }

        public void Die()
        {
            if (GameManager.Instance.State == GameState.Running)
            {
                Died?.Invoke();
            }
        }

        public void TakeDamage(float howMuchDamage)
        {
            Health.Modify(-howMuchDamage);
            if (GameManager.Instance.State == GameState.Running)
            {
                DamageTaken?.Invoke();
            }
            RequestDamagePopup?.Invoke(transform.position, (int)Mathf.Floor(howMuchDamage), false);
        }
    }
}

