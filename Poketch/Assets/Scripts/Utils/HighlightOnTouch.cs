using UnityEngine;
using UnityEngine.EventSystems;

public class HighlightOnTouch : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private static readonly int s_Highlight = Shader.PropertyToID( "_Highlight" );

    [SerializeField]
    private Material m_ColorMaterial;

    public void OnPointerDown( PointerEventData eventData )
    {
        m_ColorMaterial.SetInt( s_Highlight, 1 );
    }

    public void OnPointerUp( PointerEventData eventData )
    {
        m_ColorMaterial.SetInt( s_Highlight, 0 );
    }
}
