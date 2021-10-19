using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent( typeof( Image ) )]
public class ShowPokemonButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private Button.ButtonClickedEvent m_Action;

    [SerializeField]
    private Sprite m_OnClickedSprite;

    private Sprite m_DefaultSprite;

    private Image m_Image;

    private bool m_OnClicked = false;

    private void Awake()
    {
        m_Image = GetComponent < Image >();
        m_DefaultSprite = m_Image.sprite;
    }

    public void OnPointerDown( PointerEventData eventData )
    {
        m_Image.sprite = m_OnClickedSprite;
        m_OnClicked = true;
        StartCoroutine( OnClicked() );
        m_Action?.Invoke();
        SoundManager.Instance.PlaySFX( SoundManager.SFX.Button );
    }

    public void OnPointerUp( PointerEventData eventData )
    {
        m_Image.sprite = m_DefaultSprite;
        m_OnClicked = false;
    }

    private IEnumerator OnClicked()
    {
        float buttonTimer = Time.time;
        float buttonDelay = 1f;

        while ( m_OnClicked )
        {
            if ( Time.time - buttonTimer >= buttonDelay )
            {
                for ( int i = 0; i < 10; ++i )
                {
                    m_Action?.Invoke();
                }

                buttonDelay = 0.25f;
                SoundManager.Instance.PlaySFX( SoundManager.SFX.Button );
                buttonTimer = Time.time;
            }

            yield return null;
        }
    }
}
