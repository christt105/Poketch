using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(RawImage))]
public class PaintTexture : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private MemoPad memoPad;

    private bool isPainting = false;
    private Vector2 previousPixel = new Vector2Int(-1, -1);

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
        Debug.Log("pointer down");
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
        if (previousPixel == new Vector2Int(-1, -1))
        {
            PaintPixel(colorToPaint, CalculatePixelPosition());
        }

        if (previousPixel != new Vector2Int(-1, -1) && previousPixel != CalculatePixelPosition())
        {
            PaintPixel(colorToPaint, CalculatePixelPosition());
        }
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

    private Vector2Int CalculatePixelPosition()
    {
        Vector2 localCursor;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), Input.mousePosition, null, out localCursor))
            return Vector2Int.zero;
        localCursor.x += memoPad.m_renderer_texture.width;
        localCursor.y += memoPad.m_renderer_texture.height;
        localCursor *= 0.5f;

        return new Vector2Int((int)localCursor.x, (int)localCursor.y);
    }

    private void ResetTexture()
    {
        memoPad.m_renderer_texture.SetPixels(memoPad.pixelColors);
        memoPad.m_renderer_texture.Apply();
    }


}