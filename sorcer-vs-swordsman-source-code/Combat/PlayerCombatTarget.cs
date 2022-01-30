using Game.Combat.Reserves;
using Game.Combat.Utilities;
using Game.Core;
using Game.Stats;
using Game.UI;
using UnityEngine;

namespace Game.Combat
{
    /// <summary>
    /// CombatTargets can take damage and die.
    /// </summary>
    [RequireComponent(typeof(HealthReserve))]
    public class PlayerCombatTarget : MonoBehaviour, ICombatTarget
    {
        public event DamageTaken DamageTaken;
        public event Died Died;
        public event DisableMove DisableMove;
        public static event RequestDamagePopup RequestDamagePopup;

        public EntityStatsObject Stats { get; set; }

        public float HurtDisableMovementTime = 0.1f;

        [Header("Sensors")]

        [Tooltip("Sensor to determine if the entity is touching a wall to " +
            "its left.")]
        public WallSensor2D LeftWallSensor2D;

        [Tooltip("Sensor to determine if the entity is touching a wall to " +
            "its right.")]
        public WallSensor2D RightWallSensor2D;

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

        private void Awake()
        {
            Health = GetComponent<HealthReserve>();

            Health.Empty += Die;
        }

        private void OnDisable()
        {
            DamageTaken = null;
            Died = null;
            DisableMove = null;
        }

        private void Start()
        {
            Health.Max = Stats.MaxHealth;
        }

        public void TakeDamage(float howMuchDamage)
        {
            Health.Modify(-howMuchDamage);
            DisableMove?.Invoke(HurtDisableMovementTime);
            DisableWallSensors();
            if (GameManager.Instance.State == GameState.Running)
            {
                DamageTaken?.Invoke();
            }
            RequestDamagePopup?.Invoke(transform.position, (int)Mathf.Floor(howMuchDamage), true);
        }

        public void Die()
        {
            if (GameManager.Instance.State == GameState.Running)
            {
                DisableWallSensors();
                Died?.Invoke();
            }
        }

        private void DisableWallSensors()
        {
            if (LeftWallSensor2D != null)
            {
                LeftWallSensor2D.Disable(0.8f);
            }
            if (RightWallSensor2D != null)
            {
                RightWallSensor2D.Disable(0.8f);
            }
        }
    }
}
