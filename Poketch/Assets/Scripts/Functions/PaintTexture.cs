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
        previousPixel = new Vector2Int(-1, -1);
    }

    IEnumerator OnMousePressing()
    {
        // Paint the pixel every frame we are pressing the screen
        while (isPainting)
        {
            onScreenTouched();
            yield return null;
        }
    }

    private void PaintToScreen(Color colorToPaint)
    {
        Vector2 pixelPosition = CalculatePixelPosition();

        // Firt time dragging
        if (previousPixel == new Vector2Int(-1, -1))
        {
            PaintPixel(colorToPaint, pixelPosition);
        }

        // If the pixel position is not (-1,-1) and is different from the previous position, do a Lerp to interpolate between the two points
        // so we can paint all the pixels between.
        if (previousPixel != new Vector2Int(-1, -1) && previousPixel != pixelPosition)
        {
            // Lerp from previous to pixelPosition
            Vector2 initialPos = previousPixel;
            Vector2 endPos = pixelPosition;
            float dist = (endPos - initialPos).magnitude;
            float step = 0.0f;

            while(step <= 1.0f)
            {
                Vector2 newPixelPos = Vector2.Lerp(initialPos, endPos, step);

                PaintPixel(colorToPaint, newPixelPos);

                step += 1.0f / dist;
            }

        }
        previousPixel = pixelPosition;
    }

    private void PaintPixel(Color colorToPaint, Vector2 pixelPosition)
    {
        // Check if the pixel is inside the boundaries and if it is, check if the pixel was already paint to avoid painting the same pixel
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
        // Calculate the pixel position, local to the texture we want to paint
        Vector2 localCursor;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), Input.mousePosition, null, out localCursor))
            return Vector2Int.zero;
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