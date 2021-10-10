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

    private void OnEnable()
    {
        MemoPad.onPaint += PaintToScreen;
        MemoPad.onResetTexture += ResetTexture;
    }

    private void OnDisable()
    {
        MemoPad.onPaint -= PaintToScreen;
        MemoPad.onResetTexture -= ResetTexture;
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

    private void PaintToScreen(Color colorToPaint)
    {
        PaintPixel(colorToPaint, CalculatePixelPosition());
    }

    private void PaintPixel(Color colorToPaint, Vector2 pixelPosition)
    {
        if (pixelPosition.x >= 0 && pixelPosition.x <= MemoPad.width && pixelPosition.y >= 0 && pixelPosition.y <= MemoPad.height)
        {
            if (memoPad.m_renderer_texture.GetPixel((int)pixelPosition.x, (int)pixelPosition.y) != colorToPaint)
            {
                memoPad.m_renderer_texture.SetPixel((int)pixelPosition.x, (int)pixelPosition.y, colorToPaint);
                memoPad.m_renderer_texture.Apply();
            }
        }
    }

    private Vector2 CalculatePixelPosition()
    {
        Vector2 localCursor;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), Input.mousePosition, null, out localCursor))
            return Vector2.zero;
        localCursor.x += memoPad.m_renderer_texture.width;
        localCursor.y += memoPad.m_renderer_texture.height;
        localCursor *= 0.5f;

        return localCursor;
    }

    private void ResetTexture()
    {
        memoPad.m_renderer_texture.SetPixels(memoPad.pixelColors);
        memoPad.m_renderer_texture.Apply();
    }

   
}
