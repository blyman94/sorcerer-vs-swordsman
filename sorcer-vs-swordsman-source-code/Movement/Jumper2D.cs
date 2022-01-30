using Game.Core;
using UnityEngine;

namespace Game.Movement
{
    /// <summary>
    /// The jump behaviour of an entity. Utilizes Unity's Physics2D system to
    /// propel the entity upward. Variable jump height is implemented by 
    /// manipulating the gravity scale of the entity's rigidbody during jump
    /// phases.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class Jumper2D : MonoBehaviour
    {
        public delegate void JumpStarted();
        public JumpStarted jumpStarted;

        [Header("Sensors")]

        [Tooltip("Sensor to determine if the entity is grounded")]
        public GroundSensor2D GroundSensor2D;

        [Header("Jump Parameters")]

        [SerializeField]
        [Tooltip("Upward force applied to the entity when a jump is started.")]
        private float jumpForce = 5f;

        [SerializeField]
        [Tooltip("Gravitational force applied to the entity. This value is " +
            "scaled by the gravitational constant before acting upon the " +
            "entity.")]
        private float gravityScale = 1.0f;

        [SerializeField]
        [Tooltip("Max height delta the entity may travel before " +
            "Gravity Scale is reapplied to the entity.")]
        private float maxJumpHeight = 5.5f;

        private float disableMovementTimer;

        /// <summary>
        /// The starting position of a jump.
        /// </summary>
        private float startY;

        /// <summary>
        /// Determines if jump input has been recieved.
        /// </summary>
        [HideInInspector]
        public bool pressedJump;

        /// <summary>
        /// Determines if stop jump input has been recieved.
        /// </summary>
        [HideInInspector]
        public bool releasedJump;

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
            jumpStarted = null;
        }

        private void FixedUpdate()
        {
            if (pressedJump)
            {
                StartJump();
            }

            if (releasedJump || (transform.position.y - startY) > maxJumpHeight)
            {
                StopJump();
            }
        }
        private void Update()
        {
            disableMovementTimer -= Time.deltaTime;
        }

        /// <summary>
        /// Initiates a jump by applying upward force to the Rigidbody2D and 
        /// zeroing-out it's gravity scale. This may only occur if the
        /// GroundSensor2D is active (grounded).
        /// </summary>
        private void StartJump()
        {
            if (GroundSensor2D.Active && disableMovementTimer <= 0.0f)
            {
                startY = transform.position.y;
                rb2D.gravityScale = 0.0f;
                rb2D.AddForce(new Vector2(0, jumpForce),
                    ForceMode2D.Impulse);
                pressedJump = false;
                jumpStarted?.Invoke();
            }
            else
            {
                pressedJump = false;
            }
        }

        /// <summary>
        /// Cancles the jump by reapplying the gravity scaled to the 
        /// Rigidbody2D.
        /// </summary>
        private void StopJump()
        {
            rb2D.gravityScale = gravityScale;
            releasedJump = false;
        }

        public void SetDisableMovementTimer(float duration)
        {
            disableMovementTimer = duration;
        }
    }
}
