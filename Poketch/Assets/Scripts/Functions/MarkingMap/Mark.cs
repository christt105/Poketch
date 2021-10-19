using System.Collections;
using SimpleJSON;
using UnityEngine;
using UnityEngine.EventSystems;

public class Mark : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool m_Dragging = false;
    private MarkingMap m_MarkingMap;
    private Vector2 m_MaxPoint;
    private Vector2 m_MinPoint;
    private RectTransform m_MyRectTransform;
    private RectTransform m_ParentRectTransform;

    public void OnPointerDown( PointerEventData eventData )
    {
        m_MyRectTransform.localScale = Vector3.one * 2f;

        m_Dragging = true;
        StartCoroutine( Drag() );
    }

    public void OnPointerUp( PointerEventData eventData )
    {
        m_MyRectTransform.localScale = Vector3.one;
        m_Dragging = false;

        m_MarkingMap.Save();
    }

    private IEnumerator Drag()
    {
        while ( m_Dragging )
        {
            m_MyRectTransform.localPosition = CheckBoundaries( GetRelativeMousePosition() );

            yield return null;
        }
    }

    private Vector2 GetRelativeMousePosition()
    {
        return m_ParentRectTransform.InverseTransformPoint( Input.mousePosition );
    }

    private Vector2 CheckBoundaries( Vector2 localMousePosition )
    {
        return Vector2.Max( Vector2.Min( localMousePosition, m_MaxPoint ), m_MinPoint );
    }

    public Vector2 GetPosition()
    {
        return m_MyRectTransform.localPosition;
    }

    public void Set( JSONNode jsonNode, MarkingMap markingMap )
    {
        m_MarkingMap = markingMap;

        m_MyRectTransform = GetComponent < RectTransform >();
        m_ParentRectTransform = m_MyRectTransform.parent.GetComponent < RectTransform >();

        m_MinPoint = m_MyRectTransform.sizeDelta;
        m_MaxPoint = m_ParentRectTransform.sizeDelta - m_MinPoint;

        if ( jsonNode == null )
        {
            return;
        }

        Vector3 localPosition = m_MyRectTransform.localPosition;

        localPosition = new Vector3(
            jsonNode.GetValueOrDefault( "x", localPosition.x ),
            jsonNode.GetValueOrDefault( "y", localPosition.y ) );

        m_MyRectTransform.localPosition = localPosition;
    }
}
