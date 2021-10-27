using UnityEngine;

public class CircleExpansion : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SetActive(false);
        Signals.SignalOnCircleExpanded();
    }
}
