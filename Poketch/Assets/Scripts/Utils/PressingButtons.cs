using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PressingButtons : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [Header("Config")]
    [SerializeField] float timeBetweenCalls = 0;

    [Header("Events")]
    [SerializeField] UnityEngine.Events.UnityEvent myEvent;

    bool isPressed = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPressed = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }

    float timeSinceLastCall = 0;

    // Update is called once per frame
    void Update()
    {
        if (isPressed && timeSinceLastCall >= timeBetweenCalls)
        {
            myEvent.Invoke();
            timeSinceLastCall = 0;
        }

        timeSinceLastCall += Time.deltaTime;
    }
}
