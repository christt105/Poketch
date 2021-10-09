using System.Collections;
using SimpleJSON;
using UnityEngine;
using UnityEngine.EventSystems;

public class Mark : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool m_Dragging = false;
    private MarkingMap m_MarkingMap;
    private RectTransform m_MyRectTransform;
    private RectTransform m_ParentRectTransform;
    private Vector2 m_MinPoint;
    private Vector2 m_MaxPoint;

    public void OnPointerDown( PointerEventData eventData )
    {
        m_MyRectTransform.localScale = Vector3.one * 1.5f;

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
            Vector2 localMousePosition = m_ParentRectTransform.InverseTransformPoint( Input.mousePosition );

            m_MyRectTransform.localPosition = CheckBoundaries( localMousePosition );

            yield return null;
        }
    }

    private Vector2 CheckBoundaries( Vector2 localMousePosition )
    {
        if ( localMousePosition.x < m_MinPoint.x )
        {
            localMousePosition.x = m_MinPoint.x;
        }
        

        if ( localMousePosition.x > m_MaxPoint.x )
        {
            localMousePosition.x = m_MaxPoint.x;
        }


        if ( localMousePosition.y < m_MinPoint.y)
        {
            localMousePosition.y = m_MinPoint.y;
        }

        if ( localMousePosition.y > m_MaxPoint.y)
        {
            localMousePosition.y = m_MaxPoint.y;
        }

        return localMousePosition;
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
