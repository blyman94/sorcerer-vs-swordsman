using UnityEngine;

namespace Game.Animation
{
    public class AttackAirSlamBehaviour : StateMachineBehaviour
    {
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.GetComponent<PlayerAnimEvents>().BT_EndSlamStrike();
        }
    }
}
