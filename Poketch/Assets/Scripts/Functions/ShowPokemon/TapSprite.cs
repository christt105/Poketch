using UnityEngine;
using UnityEngine.EventSystems;
using Utils;

[RequireComponent( typeof( Animator ) )]
public class TapSprite : MonoBehaviour, IPointerDownHandler
{
    private static readonly int s_Tap = Animator.StringToHash( "Tap" );

    [SerializeField]
    private ShowPokemon m_ShowPokemon;

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

        SoundManager.Instance.PlaySFX( PokemonDataBase.Instance.GetPokemonAudioClip( m_ShowPokemon.DexNumber ), 0.1f );
        m_Animator.SetTrigger( s_Tap );
    }

    public void FinishAnimation()
    {
        m_OnAnimation = false;
    }
}
