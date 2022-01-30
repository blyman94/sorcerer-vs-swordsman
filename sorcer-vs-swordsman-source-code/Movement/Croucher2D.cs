using Game.Core;
using System.Collections;
using UnityEngine;

public class Croucher2D : MonoBehaviour
{
    public delegate void CrouchStarted();
    public CrouchStarted crouchStarted;

    public delegate void CrouchEnded();
    public CrouchEnded crouchEnded;

    [Header("Sensors")]

    [Tooltip("Sensor to determine if the entity is grounded")]
    public GroundSensor2D GroundSensor2D;

    [Header("Crouch Parameters")]

    [Tooltip("The window of time an entity crouches for.")]
    public float CrouchTime;

    [Header("Standing Collider Parameters")]

    [Tooltip("Offset of the box collider of the entity when standing.")]
    public Vector2 StandingColliderOffset;

    [Tooltip("Size of the box collider of the entity when standing.")]
    public Vector2 StandingColliderSize;

    [Header("Crouching Collider Parameters")]

    [Tooltip("Offset of the box collider of the entity when crouching or " +
        "dodging. While crouching or dodging, the entities collider " +
        "should shrink drastically so the action can be used to dodge " +
        "attacks. Should be visually aligned with a crouch/dodge animation.")]
    public Vector2 CrouchingColliderOffset;

    [Tooltip("Size of the box collider of the entity when crouching or " +
        "dodging. While crouching or dodging, the entities collider " +
        "should shrink drastically so the action can be used to dodge " +
        "attacks. Should be visually aligned with a crouch/dodge animation.")]
    public Vector2 CrouchingColliderSize;

    /// <summary>
    /// Determines if the entity can crouch (i.e., it is not already crouching).
    /// </summary>
    [HideInInspector]
    public bool canCrouch;

    /// <summary>
    /// Determines if crouch input has been recieved.
    /// </summary>
    [HideInInspector]
    public bool pressedCrouch;

    /// <summary>
    /// BoxCollider2D of the entity to be modified by the crouch action.
    /// </summary>
    private BoxCollider2D boxCollider2D;

    /// <summary>
    /// Variable to store the converted WaitForSeconds object for the crouch
    /// routine. This way, a new WaitForSeconds object is not created every 
    /// time the routine is called.
    /// </summary>
    private WaitForSeconds crouchWFS;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        crouchWFS = new WaitForSeconds(CrouchTime);
        canCrouch = true;
    }

    private void FixedUpdate()
    {
        if (canCrouch)
        {
            if (pressedCrouch && GroundSensor2D.Active)
            {
                StartCrouch();
            }
            else
            {
                pressedCrouch = false;
            }
        }
        else
        {
            pressedCrouch = false;
        }
    }

    /// <summary>
    /// Start the crouching behaviour by shrinking the size of the entity's
    /// collider.
    /// </summary>
    private void StartCrouch()
    {
        canCrouch = false;
        boxCollider2D.offset = CrouchingColliderOffset;
        boxCollider2D.size = CrouchingColliderSize;
        StartCoroutine(CrouchRoutine());
        crouchStarted?.Invoke();
    }

    /// <summary>
    /// Routine to have the player remained crouched for CrouchTime seconds.
    /// </summary>
    private IEnumerator CrouchRoutine()
    {
        yield return crouchWFS;
        EndCrouch();
    }

    /// <summary>
    /// End the crouching behaviour by restoring the original collider size of
    /// the entity.
    /// </summary>
    private void EndCrouch()
    {
        boxCollider2D.offset = StandingColliderOffset;
        boxCollider2D.size = StandingColliderSize;
        crouchEnded?.Invoke();
        canCrouch = true;
    }
}
