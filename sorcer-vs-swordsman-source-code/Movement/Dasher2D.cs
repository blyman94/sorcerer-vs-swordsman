using Game.Core;
using UnityEngine;

public class Dasher2D : MonoBehaviour
{
    public delegate void DashStarted();
    public DashStarted dashStarted;

    [Tooltip("How powerful the dash is.")]
    public float DashPower;

    [Header("Sensors")]

    [Tooltip("Sensor to determine if the entity is grounded")]
    public GroundSensor2D GroundSensor2D;

    [Tooltip("Sensor to determine if the entity is touching a wall to " +
        "its left.")]
    public WallSensor2D LeftWallSensor2D;

    [Tooltip("Sensor to determine if the entity is touching a wall to " +
        "its right.")]
    public WallSensor2D RightWallSensor2D;

    /// <summary>
    /// Determines if dash input has been recieved.
    /// </summary>
    [HideInInspector]
    public bool pressedDash;

    /// <summary>
    /// Determines if the entity can dash. To reset a dash, the entity must 
    /// touch a wall or the ground.
    /// </summary>
    private bool canDash;

    /// <summary>
    /// Direction of movement.
    /// </summary>
    private Vector2 moveInput;

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
        if (!canDash)
        {
            if (GroundSensor2D.Active || RightWallSensor2D.Active ||
                LeftWallSensor2D.Active)
            {
                canDash = true;
            }
        }

        if (canDash)
        {
            if (pressedDash && moveInput != Vector2.zero)
            {
                // Note: Mover2D will drastically reduce the effect of dashes
                // that are in a purely horizontal direction. Doubling the
                // dash power of the x component maintains the intended feel
                // of the dash.
                rb2D.velocity =
                    new Vector2(moveInput.x * 2.0f * DashPower,
                    moveInput.y * DashPower);
                canDash = false;
                pressedDash = false;
                dashStarted?.Invoke();
            }
            else
            {
                pressedDash = false;
            }
        }
        else
        {
            pressedDash = false;
        }
    }

    /// <summary>
    /// Sets the move input to given Vector2.
    /// </summary>
    /// <param name="moveInput">Movement input from controller or 
    /// other source.</param>
    public void UpdateMoveInput(Vector2 moveInput)
    {
        this.moveInput = moveInput;
    }
}
