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
            Vector2 localMousePosition = m_ParentRectTransform.InverseTransformPoint( Input.mousePosition );

            //TODO: boundaries
            if ( localMousePosition.x < 0f )
            {
                localMousePosition.x = 0f;
            }

            if ( localMousePosition.x > m_ParentRectTransform.sizeDelta.x )
            {
                localMousePosition.x = m_ParentRectTransform.sizeDelta.x;
            }

            if ( localMousePosition.y < 0f )
            {
                localMousePosition.y = 0f;
            }

            if ( localMousePosition.y > m_ParentRectTransform.sizeDelta.y )
            {
                localMousePosition.y = m_ParentRectTransform.sizeDelta.y;
            }

            m_MyRectTransform.localPosition = localMousePosition;

            yield return null;
        }
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
