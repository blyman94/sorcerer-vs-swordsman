using UnityEngine.InputSystem;

namespace Game.Control
{
    /// <summary>
    /// Interface specifying response methods to all acceptable player input.
    /// Assumes the New Unity Input System is used to handle player input.
    /// </summary>
    public interface IPlayerController
    {
        /// <summary>
        /// Response to player Attack (button) input.
        /// </summary>
        /// <param name="context">New Unity Input System callback 
        /// context.</param>
        void OnAttack(InputAction.CallbackContext context);

        /// <summary>
        /// Response to player crouch (button) input.
        /// </summary>
        /// <param name="context">New Unity Input System callback 
        /// context.</param>
        void OnCrouch(InputAction.CallbackContext context);

        /// <summary>
        /// Response to player jump (button) input.
        /// </summary>
        /// <param name="context">New Unity Input System callback 
        /// context.</param>
        void OnJump(InputAction.CallbackContext context);

        /// <summary>
        /// Response to player movement (2D axis composite) input.
        /// </summary>
        /// <param name="context">New Unity Input System callback 
        /// context.</param>
        void OnMovement(InputAction.CallbackContext context);
    }
}
