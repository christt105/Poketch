using UnityEngine;
using UnityEngine.EventSystems;

public class CoinFlip : MonoBehaviour, IPointerDownHandler
{
    private static readonly int s_Reset = Animator.StringToHash( "Reset" );
    private static readonly int s_Flip = Animator.StringToHash( "Flip" );
    private static readonly int s_IsFace = Animator.StringToHash( "IsFace" );

    public bool m_AllowClick = true;

    private Animator m_Animator;

    #region Unity Event Functions

    private void Awake()
    {
        m_Animator = GetComponent < Animator >();
    }

    #endregion

    public void Reset()
    {
        m_Animator.SetTrigger( s_Reset );
        m_AllowClick = true;
    }

    public void OnPointerDown( PointerEventData eventData )
    {
        if ( !m_AllowClick )
        {
            return;
        }

        m_Animator.SetTrigger( s_Flip );
        m_Animator.SetBool( s_IsFace, Random.Range( 0, 2 ) == 1 );
        SoundManager.Instance.PlaySFX( SoundManager.SFX.CoinStart );
    }

    public void PlayBoundSound()
    {
        SoundManager.Instance.PlaySFX( SoundManager.SFX.CoinBound );
    }
}
