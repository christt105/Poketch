using SimpleJSON;
using UnityEngine;

public class Zahori : Function
{
    [SerializeField]
    private GameObject m_Circle;

    [SerializeField]
    private GameObject m_Spot;

    private Canvas m_Canvas;

    private Vector2 m_FunctionSize;
    private Vector2 m_SpotSize;

    private void OnEnable()
    {
        Signals.onScreenTouched += ScreenTouched;
        Signals.onCircleExpanded += SpawnSpot;
    }

    private void OnDisable()
    {
        Signals.onScreenTouched -= ScreenTouched;
        Signals.onCircleExpanded -= SpawnSpot;
    }

    public override void OnCreate( JSONNode jsonObject )
    {
        m_Canvas = FindObjectOfType < Canvas >();

        m_FunctionSize = GetComponent < RectTransform >().sizeDelta;
        m_SpotSize = m_Spot.GetComponent < RectTransform >().sizeDelta;
    }

    public override void OnChange()
    {
        m_Circle.SetActive( false );
        m_Spot.SetActive( false );
    }

    private void ScreenTouched()
    {
        if ( !m_Circle.activeSelf )
        {
            ActivateCircle();
        }
    }

    private void ActivateCircle()
    {
        m_Circle.SetActive( true );

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            m_Canvas.transform as RectTransform,
            Input.mousePosition,
            m_Canvas.worldCamera,
            out Vector2 pos );

        m_Circle.transform.position = m_Canvas.transform.TransformPoint( pos );

        SoundManager.Instance.PlaySFX( SoundManager.SFX.Zahori );
    }

    private void SpawnSpot()
    {
        if ( Random.Range( 0, 6 ) == 0 )
        {
            m_Spot.SetActive( true );

            m_Spot.GetComponent < RectTransform >().anchoredPosition = new Vector2(
                Random.Range( 0f, m_FunctionSize.x ) - m_SpotSize.x,
                Random.Range( 0f, m_FunctionSize.y ) - m_SpotSize.y );
        }
    }
}
