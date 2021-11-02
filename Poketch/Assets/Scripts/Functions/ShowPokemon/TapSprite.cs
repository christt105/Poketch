using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent( typeof( Animator ) )]
public class TapSprite : MonoBehaviour, IPointerDownHandler
{
    public delegate void Tap();

    private static readonly int s_Tap = Animator.StringToHash( "Tap" );

    private Animator m_Animator;

    private bool m_OnAnimation = false;

    private void Awake()
    {
        m_Animator = GetComponent < Animator >();
    }

    public void OnPointerDown( PointerEventData eventData )
    {
        if ( m_OnAnimation )
        {
            return;
        }

        m_OnAnimation = true;

        OnTap?.Invoke();

        m_Animator.SetTrigger( s_Tap );
    }

    public event Tap OnTap;

    public void FinishAnimation()
    {
        m_OnAnimation = false;
    }
}
