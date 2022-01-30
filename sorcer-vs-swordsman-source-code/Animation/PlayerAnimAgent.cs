using Game.Core;
using Game.Entity;
using Game.Stats;
using UnityEngine;

namespace Game.Animation
{
    /// <summary>
    /// Listens to delegate events (or other changes in player component
    /// values) and updates the player animator accordingly.
    /// </summary>
    public class PlayerAnimAgent : MonoBehaviour
    {
        [Tooltip("Player to animate.")]
        public Player Player;

        [Header("Sensors")]

        [Tooltip("Sensor to determine if the player is grounded.")]
        public GroundSensor2D GroundSensor2D;

        [Tooltip("Sensor to determine if the entity is touching a wall to " +
            "its left.")]
        public WallSensor2D LeftWallSensor2D;

        [Tooltip("Sensor to determine if the entity is touching a wall to " +
            "its right.")]
        public WallSensor2D RightWallSensor2D;

        /// <summary>
        /// Animator controlling player animations.
        /// </summary>
        private Animator animator;

        /// <summary>
        /// Sprite renderer representing the player.
        /// </summary>
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            ManageCroucher2DEvents(subscribe: true);
            ManageDasher2DEvents(subscribe: true);
            ManageGroundSensorEvents(subscribe: true);
            ManageJumper2DEvents(subscribe: true);
            ManageMeleeFighterEvents(subscribe: true);
            ManageMover2DEvents(subscribe: true);
            ManagePlayerHealthEvents(subscribe: true);
            ManageWallJumper2DEvents(subscribe: true);
        }

        private void OnDisable()
        {
            ManageCroucher2DEvents(subscribe: false);
            ManageDasher2DEvents(subscribe: false);
            ManageGroundSensorEvents(subscribe: false);
            ManageJumper2DEvents(subscribe: false);
            ManageMeleeFighterEvents(subscribe: false);
            ManageMover2DEvents(subscribe: false);
            ManagePlayerHealthEvents(subscribe: false);
            ManageWallJumper2DEvents(subscribe: false);
        }

        private void Update()
        {
            UpdateMovingParam();
            UpdateYVelocityParam();
            UpdateWallSlidingParam();
        }

        #region Event Subscription Management

        /// <summary>
        /// Subscribes/unsubscribes from Croucher2D events.
        /// </summary>
        /// <param name="subscribe">Subscribes to events if true,
        /// unsubscribes from events if false.</param>
        private void ManageCroucher2DEvents(bool subscribe)
        {
            if (subscribe)
            {
                Player.Croucher2D.crouchStarted += TriggerCrouch;
                Player.Croucher2D.crouchEnded += TriggerStandUp;
            }
            else
            {
                Player.Croucher2D.crouchStarted -= TriggerCrouch;
                Player.Croucher2D.crouchEnded -= TriggerStandUp;
            }
        }

        /// <summary>
        /// Subscribes/unsubscribes from Dasher2D events.
        /// </summary>
        /// <param name="subscribe">Subscribes to events if true,
        /// unsubscribes from events if false.</param>
        private void ManageDasher2DEvents(bool subscribe)
        {
            if (subscribe)
            {
                Player.Dasher2D.dashStarted += TriggerDash;
            }
            else
            {
                Player.Dasher2D.dashStarted -= TriggerDash;
            }
        }

        /// <summary>
        /// Subscribes/unsubscribes from GroundSensor2D events.
        /// </summary>
        /// <param name="subscribe">Subscribes to events if true,
        /// unsubscribes from events if false.</param>
        private void ManageGroundSensorEvents(bool subscribe)
        {
            if (subscribe)
            {
                GroundSensor2D.sensorStateChanged += UpdateGroundedParam;
            }
            else
            {
                GroundSensor2D.sensorStateChanged -= UpdateGroundedParam;
            }
        }

        /// <summary>
        /// Subscribes/unsubscribes from Jumper2D events.
        /// </summary>
        /// <param name="subscribe">Subscribes to events if true,
        /// unsubscribes from events if false.</param>
        private void ManageJumper2DEvents(bool subscribe)
        {
            if (subscribe)
            {
                Player.Jumper2D.jumpStarted += TriggerJump;
            }
            else
            {
                Player.Jumper2D.jumpStarted -= TriggerJump;
            }
        }

        /// <summary>
        /// Subscribes/unsubscribes from MeleeFighter events.
        /// </summary>
        /// <param name="subscribe">Subscribes to events if true,
        /// unsubscribes from events if false.</param>
        private void ManageMeleeFighterEvents(bool subscribe)
        {
            if (subscribe)
            {
                Player.MeleeFighter.AttackStarted += TriggerAttack;
            }
            else
            {
                Player.MeleeFighter.AttackStarted -= TriggerAttack;
            }
        }

        /// <summary>
        /// Subscribes/unsubscribes from Mover2D events.
        /// </summary>
        /// <param name="subscribe">Subscribes to events if true,
        /// unsubscribes from events if false.</param>
        private void ManageMover2DEvents(bool subscribe)
        {
            if (subscribe)
            {
                Player.Mover2D.directionChanged += UpdateTransformDirection;
            }
            else
            {
                Player.Mover2D.directionChanged -= UpdateTransformDirection;
            }
        }

        /// <summary>
        /// Subscribes/unsubscribes from Player.Health events.
        /// </summary>
        /// <param name="subscribe">Subscribes to events if true,
        /// unsubscribes from events if false.</param>
        private void ManagePlayerHealthEvents(bool subscribe)
        {
            if (subscribe)
            {
                Player.CombatTarget.DamageTaken += TriggerHurt;
                Player.CombatTarget.Died += TriggerDeath;
            }
            else
            {
                Player.CombatTarget.DamageTaken -= TriggerHurt;
                Player.CombatTarget.Died -= TriggerDeath;
            }
        }

        /// <summary>
        /// Subscribes/unsubscribes from WallJumper2D events.
        /// </summary>
        /// <param name="subscribe">Subscribes to events if true,
        /// unsubscribes from events if false.</param>
        private void ManageWallJumper2DEvents(bool subscribe)
        {
            if (subscribe)
            {
                Player.WallJumper2D.jumpStarted += TriggerJump;
                Player.WallJumper2D.directionChanged +=
                    UpdateTransformDirection;
            }
            else
            {
                Player.WallJumper2D.jumpStarted -= TriggerJump;
                Player.WallJumper2D.directionChanged -=
                    UpdateTransformDirection;
            }
        }

        #endregion

        /// <summary>
        /// Triggers one of two ground attack animations.
        /// </summary>
        /// <param name="attackType">Integer of the attack to be triggered.
        /// </param>
        private void TriggerAttack(string attackType)
        {
            animator.SetTrigger("attack" + attackType + "_t");
        }

        /// <summary>
        /// Triggers one of two ground attack animations.
        /// </summary>
        /// <param name="attackInt">Integer of the attack to be triggered.
        /// </param>
        private void TriggerUpAttack()
        {
            animator.SetTrigger("attackUp_t");
        }

        /// <summary>
        /// Triggers the crouch animation.
        /// </summary>
        private void TriggerCrouch()
        {
            animator.SetTrigger("crouch_t");
            animator.ResetTrigger("standUp_t");
        }

        /// <summary>
        /// Triggers the dash animation.
        /// </summary>
        private void TriggerDash()
        {
            animator.SetTrigger("dash_t");
        }

        /// <summary>
        /// Triggers the death animation.
        /// </summary>
        private void TriggerDeath()
        {
            animator.SetTrigger("die_t");
        }

        /// <summary>
        /// Triggers the hurt animation.
        /// </summary>
        /// <param name="newAmount">UNUSED</param>
        /// <param name="damage">Whether or not the health change is 
        /// damage. Animation only plays if health changes in the negative
        /// direction.</param>
        private void TriggerHurt()
        {
            animator.SetTrigger("hurt_t");
        }

        /// <summary>
        /// Triggers the jump animation.
        /// </summary>
        private void TriggerJump()
        {
            animator.SetTrigger("jump_t");
        }

        /// <summary>
        /// Triggers the stand up animation.
        /// </summary>
        private void TriggerStandUp()
        {
            animator.SetTrigger("standUp_t");
        }

        /// <summary>
        /// Updates the grounded parameter (bool) in response to changes in the 
        /// GroundSensor2D's state.
        /// </summary>
        /// <param name="newState">Current state of the GroundSensor2D.</param>
        private void UpdateGroundedParam(bool newState)
        {
            animator.SetBool("grounded_b", newState);
        }

        /// <summary>
        /// Updates the moving parameter (bool) in response to changes in the 
        /// player's Rigidbody2D horizontal velocity.
        /// </summary>
        /// <param name="newState">Current state of the GroundSensor2D.</param>
        private void UpdateMovingParam()
        {
            if (Mathf.Abs(Player.Rigidbody2D.velocity.x) > 0.01f)
            {
                animator.SetBool("moving_b", true);
            }
            else
            {
                animator.SetBool("moving_b", false);
            }
        }

        /// <summary>
        /// Updates the wall sliding parameter (bool) in response to changes in
        /// the wall sensors' states.
        /// </summary>
        /// <param name="newState">Current state of the GroundSensor2D.</param>
        private void UpdateWallSlidingParam()
        {
            if ((LeftWallSensor2D.Active || RightWallSensor2D.Active) &&
                Player.Rigidbody2D.velocity.y < 0.0f)
            {
                animator.SetBool("wallSliding_b", true);
            }
            else
            {
                animator.SetBool("wallSliding_b", false);
            }
        }

        /// <summary>
        /// Updates the vertical velocity parameter (float) in response to 
        /// changes in the player's Rigidbody2D vertical velocity.
        /// </summary>
        /// <param name="newState">Current state of the GroundSensor2D.</param>
        private void UpdateYVelocityParam()
        {
            animator.SetFloat("yVelocity_f", Player.Rigidbody2D.velocity.y);
        }

        /// <summary>
        /// Flips the sprite along the x direction in response to new move 
        /// direction input in the Mover2D component.
        /// </summary>
        /// <param name="newMoveDir"></param>
        private void UpdateTransformDirection(int newMoveDir)
        {
            switch (newMoveDir)
            {
                case 0:
                    return;
                case 1:
                    spriteRenderer.flipX = false;
                    break;
                case -1:
                    spriteRenderer.flipX = true;
                    break;
                default:
                    Debug.LogError("[PlayerAnimationHandler.cs] Unrecognized " +
                        "movement direction passed to " +
                        "UpdateTransformDirection(int newMoveDir).");
                    break;
            }
        }
    }
}
