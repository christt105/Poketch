using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

[ RequireComponent( typeof(RawImage))]
public class PaintTexture : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private MemoPad memoPad;

    private bool isPainting = false;

    public delegate void OnScreenTouched();
    public static event OnScreenTouched onScreenTouched;

    private void Start()
    {
        MemoPad.onPaint += Paint;
        MemoPad.onResetTexture += ResetTexture;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPainting = true;
        StartCoroutine(OnMousePressing());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPainting = false;
    }

    IEnumerator OnMousePressing()
    {
        while (isPainting)
        {
            onScreenTouched();
            yield return null; 
        }
    }

    private void Paint(Color colorToPaint)
    {
        Vector2 localCursor;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), Input.mousePosition, null, out localCursor))
            return;
        localCursor.x += memoPad.m_renderer_texture.width * 0.5f;
        localCursor.y += memoPad.m_renderer_texture.height * 0.5f;

        if (localCursor.x >= 0 && localCursor.x <= MemoPad.width && localCursor.y >= 0 && localCursor.y <= MemoPad.height)
        {
            if (memoPad.m_renderer_texture.GetPixel((int)localCursor.x, (int)localCursor.y) != colorToPaint)
            {
                memoPad.m_renderer_texture.SetPixel((int)localCursor.x, (int)localCursor.y, colorToPaint);
                memoPad.m_renderer_texture.Apply();
            }
        }
    }

    private void ResetTexture()
    {
        memoPad.m_renderer_texture.SetPixels(memoPad.pixelColors);
        memoPad.m_renderer_texture.Apply();
    }

   
}
