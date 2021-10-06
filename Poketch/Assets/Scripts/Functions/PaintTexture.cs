using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using SimpleJSON;

[ RequireComponent( typeof(RawImage))]
public class PaintTexture : MonoBehaviour, IDragHandler
{
    [SerializeField]
    private MemoPad memoPad;

    private PointerEventData m_pointerEventData;

    public delegate void OnScreenTouched();
    public static event OnScreenTouched onScreenTouched;

    private void Start()
    {
        MemoPad.onPaint += Paint;
        MemoPad.onErase += Erase;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0))
        {
            m_pointerEventData = eventData;
            onScreenTouched();
        }
    }


    private void Paint()
    {
        Vector2 localCursor;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), Input.mousePosition, m_pointerEventData.pressEventCamera, out localCursor))
            return;
        localCursor.x += memoPad.m_renderer_texture.width * 0.5f;
        localCursor.y += memoPad.m_renderer_texture.height * 0.5f;

        if (localCursor.x >= 0 && localCursor.x <= 156 && localCursor.y >= 0 && localCursor.y <= 150)
        {
            memoPad.m_renderer_texture.SetPixel((int)localCursor.x, (int)localCursor.y, Color.black);
            memoPad.m_renderer_texture.Apply();
        }
    }

    private void Erase()
    {

    }






   

}
