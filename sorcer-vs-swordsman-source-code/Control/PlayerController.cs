using Game.Entity;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Control
{
    /// <summary>
    /// Describes all methods used to control the player behaviour.
    /// </summary>
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        /// <summary>
        /// Player to control.
        /// </summary>
        private Player player;

        public Camera MainCamera;

        public LayerMask WhatIsBlock;

        /// <summary>
        /// Current position of the player's mouse.
        /// </summary>        
        public Vector2 MousePosition { get; set; }

        private void Awake()
        {
            player = GetComponent<Player>();
        }

        /// <summary>
        /// Returns the infinite ray that runs from the camera through the
        /// mouse's current position on screen.
        /// </summary>
        /// <returns>The infinite ray that runs from the camera through the
        /// mouse's current position on screen.</returns>
        public Vector2 GetMouseRay()
        {
            return MainCamera.ScreenToWorldPoint(MousePosition);
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (GameManager.Instance != null)
            {
                if (GameManager.Instance.State == GameState.Running)
                {
                    if (context.started)
                    {
                        player.MeleeFighter.Attack();
                    }
                }
            }
            else
            {
                if (context.started)
                {
                    player.MeleeFighter.Attack();
                }
            }
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            if (GameManager.Instance != null)
            {
                if (GameManager.Instance.State == GameState.Running)
                {
                    if (context.started)
                    {
                        player.Croucher2D.pressedCrouch = true;
                    }
                }
            }
            else
            {
                if (context.started)
                {
                    player.Croucher2D.pressedCrouch = true;
                }
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (GameManager.Instance != null)
            {
                if (GameManager.Instance.State == GameState.Running)
                {
                    if (context.started)
                    {
                        player.Jumper2D.pressedJump = true;
                        player.WallJumper2D.pressedJump = true;
                    }
                    else if (context.canceled)
                    {
                        player.Jumper2D.releasedJump = true;
                    }
                }
            }
            else
            {
                if (context.started)
                {
                    player.Jumper2D.pressedJump = true;
                    player.WallJumper2D.pressedJump = true;
                }
                else if (context.canceled)
                {
                    player.Jumper2D.releasedJump = true;
                }
            }
        }

        /// <summary>
        /// Responds to Interact event from the New Input System.
        /// </summary>
        /// <param name="context">Input action from which to read input.</param>
        public void OnInteract(InputAction.CallbackContext context)
        {
            if (GameManager.Instance != null)
            {
                if (GameManager.Instance.State == GameState.Running)
                {
                    if (context.started)
                    {
                        RaycastHit2D[] hitInfos = Physics2D.RaycastAll(GetMouseRay(), Vector2.zero, Mathf.Infinity, WhatIsBlock);
                        Debug.Log(hitInfos.Length);
                        foreach (RaycastHit2D hitInfo in hitInfos)
                        {
                            if (hitInfo.collider != null)
                            {
                                if (hitInfo.transform.CompareTag("Block"))
                                {
                                    Block block = hitInfo.transform.GetComponent<Block>();
                                    player.InteractWithBlock(block);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (context.started)
                {
                    RaycastHit2D[] hitInfos = Physics2D.RaycastAll(GetMouseRay(), Vector2.zero, Mathf.Infinity, WhatIsBlock);
                    Debug.Log(hitInfos.Length);
                    foreach (RaycastHit2D hitInfo in hitInfos)
                    {
                        if (hitInfo.collider != null)
                        {
                            if (hitInfo.transform.CompareTag("Block"))
                            {
                                Block block = hitInfo.transform.GetComponent<Block>();
                                player.InteractWithBlock(block);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Responds to mouse movement from the New Input System.
        /// </summary>
        /// <param name="context">Input action from which to read input.</param>
        public void OnMouseMove(InputAction.CallbackContext context)
        {
            MousePosition = context.ReadValue<Vector2>();
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            if (GameManager.Instance != null)
            {
                if (GameManager.Instance.State == GameState.Running)
                {
                    if (context.performed)
                    {
                        Vector2 moveInput = context.ReadValue<Vector2>();
                        player.MeleeFighter.UpdateYDir(moveInput);
                        player.Mover2D.UpdateMoveDir(moveInput);
                        player.Dasher2D.UpdateMoveInput(moveInput);
                        player.WallJumper2D.UpdateMoveDir(moveInput);
                    }
                    else if (context.canceled)
                    {
                        Vector2 moveInput = new Vector2(0, 0);
                        player.MeleeFighter.UpdateYDir(moveInput);
                        player.Dasher2D.UpdateMoveInput(moveInput);
                        player.Mover2D.UpdateMoveDir(moveInput);
                        player.WallJumper2D.UpdateMoveDir(moveInput);
                    }
                }
            }
            else
            {
                if (context.performed)
                {
                    Vector2 moveInput = context.ReadValue<Vector2>();
                    player.MeleeFighter.UpdateYDir(moveInput);
                    player.Mover2D.UpdateMoveDir(moveInput);
                    player.Dasher2D.UpdateMoveInput(moveInput);
                    player.WallJumper2D.UpdateMoveDir(moveInput);
                }
                else if (context.canceled)
                {
                    Vector2 moveInput = new Vector2(0, 0);
                    player.MeleeFighter.UpdateYDir(moveInput);
                    player.Dasher2D.UpdateMoveInput(moveInput);
                    player.Mover2D.UpdateMoveDir(moveInput);
                    player.WallJumper2D.UpdateMoveDir(moveInput);
                }
            }
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (GameManager.Instance != null)
            {
                if (GameManager.Instance.State == GameState.Running)
                {
                    if (context.started)
                    {
                        GameManager.Instance.TogglePauseState();
                    }
                }
            }
        }
    }
}
