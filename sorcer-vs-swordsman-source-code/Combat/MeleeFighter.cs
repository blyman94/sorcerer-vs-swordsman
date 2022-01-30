using Game.Combat.Utilities;
using Game.Core;
using Game.Stats;
using UnityEngine;

namespace Game.Combat
{
    /// <summary>
    /// MeleeFighters can damage enemies at close range by swinging a weapon.
    /// </summary>
    public class MeleeFighter : MonoBehaviour
    {
        public event AttackStarted AttackStarted;

        public event DisableMove DisableMove;

        public MeleeWeapon MeleeWeapon;
        public EntityStatsObject Stats { get; set; }
        public float SlamAttackVelocity;
        public float AirSlamAttackDisableMovementTime = 0.8f;
        public float AttackDisableMovementTime;

        [Header("Sensors")]

        [Tooltip("Sensor to determine if the entity is grounded")]
        public GroundSensor2D GroundSensor2D;

        private int currentAttack = 0;
        private float timeSinceLastAttack = 0.0f;
        private float minTimeBetweenAttacks = 0.2f;
        private float maxComboTime = 1.0f;
        private float yDir;

        /// <summary>
        /// Rigidbody of the entity to be modified by the dash action.
        /// </summary>
        private Rigidbody2D rb2D;

        private void Awake()
        {
            rb2D = GetComponent<Rigidbody2D>();
        }

        private void OnDisable()
        {
            AttackStarted = null;
            DisableMove = null;
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
        }

        public void Attack()
        {
            if (timeSinceLastAttack >= minTimeBetweenAttacks)
            {
                if (!GroundSensor2D.Active)
                {
                    if (yDir > 0.5f)
                    {
                        AirAttackUp();
                    }
                    else if (yDir < -0.5f)
                    {
                        AirSlamAttack();
                    }
                    else
                    {
                        AirAttack();
                    }
                }
                else // Grounded attacks stop entity movement.
                {
                    rb2D.velocity =
                        new Vector2(0.0f, rb2D.velocity.y);

                    if (yDir > 0.5f)
                    {
                        BasicAttackUp();
                    }
                    else
                    {
                        BasicAttack();
                    }
                }
            }
        }

        private void AirAttack()
        {
            AttackStarted?.Invoke("Air");
            timeSinceLastAttack = 0.0f;
        }

        private void AirAttackUp()
        {
            AttackStarted?.Invoke("AirUp");
            timeSinceLastAttack = 0.0f;
        }

        private void AirSlamAttack()
        {
            AttackStarted?.Invoke("Slam");
            rb2D.velocity = new Vector2(0.0f, -SlamAttackVelocity);
            DisableMove?.Invoke(AirSlamAttackDisableMovementTime);
            timeSinceLastAttack = 0.0f;
        }

        private void BasicAttack()
        {
            currentAttack++;
            MeleeWeapon.currentAttack++;

            if (currentAttack > 2)
            {
                currentAttack = 1;
                MeleeWeapon.currentAttack = 1;
            }

            if (timeSinceLastAttack > maxComboTime)
            {
                currentAttack = 1;
                MeleeWeapon.currentAttack = 1;
            }

            AttackStarted?.Invoke(currentAttack.ToString());

            DisableMove?.Invoke(AttackDisableMovementTime);

            timeSinceLastAttack = 0.0f;
        }

        private void BasicAttackUp()
        {
            AttackStarted?.Invoke("Up");
            DisableMove?.Invoke(AttackDisableMovementTime);
            timeSinceLastAttack = 0.0f;
        }

        /// <summary>
        /// Sets the move direction to 1, -1, or 0 based on direction of
        /// moveInput.
        /// </summary>
        /// <param name="moveInput">Movement input from controller or 
        /// other source.</param>
        public void UpdateYDir(Vector2 moveInput)
        {
            yDir = moveInput.y;
        }
    }
}
