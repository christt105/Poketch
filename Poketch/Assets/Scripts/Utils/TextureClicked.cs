using UnityEngine;
using UnityEngine.EventSystems;

public class TextureClicked : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Signals.SignalOnScreenTouched();
    }

}
