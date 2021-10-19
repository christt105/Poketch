using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(RawImage))]
public class PaintTexture : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isPainting = false;
    private Vector2 previousPixel = new Vector2Int(-1, -1);
    private Texture2D textureContainer;
    private Vector2Int textureRect;
    private Color[] pixelColors;
    private List<Vector2> neighbours = new List<Vector2>() { new Vector2(1.0f, 0.0f), new Vector2(0.0f, -1.0f), new Vector2(1.0f, -1.0f) };

    private void OnEnable()
    {
        Signals.onPaint += PaintToScreen;
        Signals.onResetTexture += ResetTexture;
        Signals.onInitializeValues += InitializeVariables;
    }

    private void OnDisable()
    {
        Signals.onPaint -= PaintToScreen;
        Signals.onResetTexture -= ResetTexture;
        Signals.onInitializeValues -= InitializeVariables;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
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
            Signals.SignalOnScreenTouched();
            yield return null;
        }
    }

    private void InitializeVariables(Texture2D renderTexture, Vector2Int textureRect, Color[] pixelColors)
    {
        textureContainer = renderTexture; // Reference to the texture used to modify it
        this.textureRect = textureRect; // The size we want to paint. It is not necessary to match with the texture size.
        this.pixelColors = pixelColors;
        GetComponent<RawImage>().texture = textureContainer; // Every time we modify textureContainer, the raw image will change automatically.
    }

    private void PaintToScreen(Color colorToPaint)
    {
        Vector2 pixelPosition = CalculatePixelPosition();

        // Firt time dragging
        if (previousPixel == new Vector2Int(-1, -1))
        {
            CreateBrushSize(colorToPaint, pixelPosition);
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

            while (step <= 1.0f)
            {
                Vector2 newPixelPos = Vector2.Lerp(initialPos, endPos, step);

                CreateBrushSize(colorToPaint, newPixelPos);

                step += 1.0f / dist;
            }
        }
        previousPixel = pixelPosition;
    }

    private void CreateBrushSize(Color colorToPaint, Vector2 pixelPosition)
    {
        // Paint the first pixel that we allways want to paint no matter if we are erasing or painting.
        PaintPixel(colorToPaint, pixelPosition);

        if (colorToPaint == Color.white) // We need to calculate 3 neighbour pixels because we need to erase 4x4 pixels
        {
            foreach(Vector2 position in neighbours)
            {
                PaintPixel(colorToPaint, pixelPosition + position);
            }
        }
        textureContainer.Apply(); 
    }

    private void PaintPixel(Color colorToPaint, Vector2 pixelPosition)
    {
        // Check if the pixel is inside the boundaries and if it is already painted to avoid painting it. If not, paint it.
        if (pixelPosition.x >= 0 && pixelPosition.x <= textureRect.x && pixelPosition.y >= 0 && pixelPosition.y <= textureRect.y)
        {
            if (textureContainer.GetPixel((int)pixelPosition.x, (int)pixelPosition.y) != colorToPaint)
            {
                textureContainer.SetPixel((int)pixelPosition.x, (int)pixelPosition.y, colorToPaint);
            }
        }
    }

    private Vector2 CalculatePixelPosition()
    {
        // Calculate the pixel position, local to the texture we want to paint
        // P.e. If the texture is 150x150, no matter where it is in the screen, if the mouse is in the middle of that texture, you will get (75,75)
        Vector2 localCursor;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), Input.mousePosition, null, out localCursor))
            return Vector2Int.zero;
        localCursor.x += textureContainer.width;
        localCursor.y += textureContainer.height;
        localCursor *= 0.5f;

        return localCursor;
    }

    private void ResetTexture()
    {
        textureContainer.SetPixels(pixelColors);
        textureContainer.Apply();
    }
}
