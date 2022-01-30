using Game.Core;
using UnityEngine;

namespace Game.Movement
{
    /// <summary>
    /// The wall jump behaviour of the entity. When in contact with a wall, the
    /// entity will slide down the wall at a configurable speed. The entity can
    /// jump off the wall in 3
    /// </summary>
    public class WallJumper2D : MonoBehaviour
    {
        public delegate void JumpStarted();
        public JumpStarted jumpStarted;

        public delegate void DirectionChanged(int newDir);
        public DirectionChanged directionChanged;

        [Header("Sensors")]

        [Tooltip("Sensor to determine if the entity is grounded")]
        public GroundSensor2D GroundSensor2D;

        [Tooltip("Sensor to determine if the entity is touching a wall to " +
            "its left.")]
        public WallSensor2D LeftWallSensor2D;

        [Tooltip("Sensor to determine if the entity is touching a wall to " +
            "its right.")]
        public WallSensor2D RightWallSensor2D;

        [Header("Wall Slide Parameters")]

        [Tooltip("Max speed at which the entity will slide down a wall.")]
        public float WallSlideSpeed;

        [Header("Wall Jump Parameters")]
        [Tooltip("Determines launch angle of the Wall Jump Off behaviour.")]
        public Vector2 WallJumpOffVelocity;

        [Tooltip("Determines launch angle of the Wall Jump Climb behaviour.")]
        public Vector2 WallJumpClimbVelocity;

        /// <summary>
        /// Determines if jump input has been recieved.
        /// </summary>
        [HideInInspector]
        public bool pressedJump;

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
            if (LeftWallSensor2D.Active || RightWallSensor2D.Active)
            {
                if (pressedJump)
                {
                    StartWallJump();
                    pressedJump = false;
                }
                if ((rb2D.velocity.y < 0) && 
                    Mathf.Abs(rb2D.velocity.y) > WallSlideSpeed)
                {
                    rb2D.velocity = new Vector2(rb2D.velocity.x,
                        -WallSlideSpeed);
                }
            }
            else
            {
                pressedJump = false;
            }
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
        }

        /// <summary>
        /// Determines the direction of the wall the entity is on.
        /// </summary>
        /// <returns>Direction of the wall the entity is on.</returns>
        private int GetWallDir()
        {
            if (RightWallSensor2D.Active)
            {
                return 1;
            }
            else if (LeftWallSensor2D.Active)
            {
                return -1;
            }
            return 0;
        }

        /// <summary>
        /// Executes one of two wall jumps. If the current move direction is 
        /// towards the wall the entity is currently on, they will hop up the 
        /// wall to climb it. Otherwise, they will launch off the wall at an
        /// angle determined by the WallJumpOffVelocity vector.
        /// </summary>
        private void StartWallJump()
        {
            int wallDirX = GetWallDir();
            if (!GroundSensor2D.Active)
            {
                if (moveDir == wallDirX)
                {
                    rb2D.velocity =
                        new Vector2(-wallDirX * WallJumpClimbVelocity.x,
                        WallJumpClimbVelocity.y);
                }
                else
                {
                    rb2D.velocity =
                        new Vector2(-wallDirX * WallJumpOffVelocity.x,
                        WallJumpOffVelocity.y);
                    directionChanged?.Invoke(-wallDirX);
                }
                jumpStarted?.Invoke();
            }
        }
    }
}
