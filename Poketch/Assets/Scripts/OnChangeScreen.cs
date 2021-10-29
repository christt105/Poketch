using UnityEngine;

public class OnChangeScreen : StateMachineBehaviour
{
    private static readonly int s_ChangeScreen = Animator.StringToHash( "changeScreen" );
    private static readonly int s_HoldChange = Animator.StringToHash( "holdChange" );
    private static readonly int s_Previous = Animator.StringToHash( "previous" );
    private float m_Timer = 0f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter( Animator animator, AnimatorStateInfo stateInfo, int layerIndex )
    {
        bool previous = animator.GetBool( s_Previous );
        NextFunction( previous );

        if ( animator.GetBool( s_ChangeScreen ) )
        {
            animator.SetBool( s_ChangeScreen, false );
            animator.SetBool( s_HoldChange, true );
            FunctionController.Instance.SetFunctionText( previous );
            m_Timer = Time.time;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate( Animator animator, AnimatorStateInfo stateInfo, int layerIndex )
    {
        if ( animator.GetBool( s_HoldChange ) )
        {
            if ( Time.time - m_Timer >= FunctionController.Instance.FastTimeChange )
            {
                bool previous = animator.GetBool( s_Previous );
                NextFunction( previous );

                if ( animator.GetBool( s_ChangeScreen ) )
                {
                    animator.SetBool( s_ChangeScreen, false );
                    animator.SetBool( s_HoldChange, true );
                    FunctionController.Instance.SetFunctionText( previous );
                    m_Timer = Time.time;
                }
                else
                {
                    animator.SetBool( s_HoldChange, false );
                    FunctionController.Instance.HideFunctionText();
                }
            }
        }
    }

    private static void NextFunction( bool previous )
    {
        if ( previous )
        {
            FunctionController.Instance.Previous();
        }
        else
        {
            FunctionController.Instance.Next();
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
