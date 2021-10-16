using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventPointRectTransform : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform PointerZone { get; private set; }

    public delegate void PointerDown();
    public event PointerDown OnDown;

    public delegate void PointerUp();
    public event PointerUp OnUp;

    private void Start()
    {
        PointerZone = GetComponent < RectTransform >();
    }

    public void OnPointerDown( PointerEventData eventData )
    {
        OnDown?.Invoke();
    }

    public void OnPointerUp( PointerEventData eventData )
    {
        OnUp?.Invoke();
    }

    public Vector2 GetRelativeMousePosition()
    {
        return PointerZone.InverseTransformPoint(Input.mousePosition) / PointerZone.sizeDelta;
    }
}
