using Game.Core;
using UnityEngine;

namespace Game.Movement
{
    /// <summary>
    /// The horizontal movement behaviour of an entity.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class Mover2D : MonoBehaviour
    {
        public delegate void DirectionChanged(int newDir);
        public DirectionChanged directionChanged;

        [Header("Sensors")]

        [Tooltip("Sensor to determine if the entity is grounded")]
        public GroundSensor2D GroundSensor2D;

        [Header("Movement Parameters")]

        [SerializeField]
        [Tooltip("Maximum X velocity.")]
        public float MaxSpeed = 3.0f;

        [SerializeField]
        [Tooltip("Rate at which MaxSpeed is approached.")]
        private float xAccelRate = 5.0f;

        private float disableMovementTimer;

        /// <summary>
        /// Direction of movement.
        /// </summary>
        private int moveDir;

        /// <summary>
        /// Rigidbody of the entity to be modified by the dash action.
        /// </summary>
        private Rigidbody2D rb2D;

        private void Awake()
        {
            rb2D = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (moveDir == 0)
            {
                if (GroundSensor2D.Active)
                {
                    rb2D.velocity =
                        new Vector2(0.0f, rb2D.velocity.y);
                }
            }
            else if (moveDir != 0 && disableMovementTimer <= 0.0f)
            {
                float newVelX = Mathf.Lerp(rb2D.velocity.x,
                    MaxSpeed * moveDir, xAccelRate * Time.fixedDeltaTime);
                rb2D.velocity =
                    new Vector2(newVelX, rb2D.velocity.y);
            }
        }

        private void Update()
        {
            disableMovementTimer -= Time.deltaTime;
        }

        /// <summary>
        /// Sets the move direction to 1, -1, or 0 based on direction of
        /// moveInput.
        /// </summary>
        /// <param name="moveInput">Movement input from controller or 
        /// other source.</param>
        public void UpdateMoveDir(Vector2 moveInput)
        {
            moveDir = 0;

            if (moveInput.x != 0)
            {
                moveDir = (int)Mathf.Sign(moveInput.x);
            }

            directionChanged?.Invoke(moveDir);
        }

        public void SetDisableMovementTimer(float duration)
        {
            disableMovementTimer = duration;
        }
    }
}
