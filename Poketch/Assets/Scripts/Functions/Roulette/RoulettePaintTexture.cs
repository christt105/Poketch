using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RoulettePaintTexture : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    bool isPainting = false;
    bool cursorInside = false;
    Texture2D tex;
    Color[] colorWhite = new Color[140 * 140];

    public void OnPointerDown(PointerEventData eventData)
    {
        isPainting = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPainting = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        cursorInside = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cursorInside = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        tex = new Texture2D(140, 140);
        tex.filterMode = FilterMode.Point;

        for (int i = 0; i < colorWhite.Length; ++i)
        {
            colorWhite[i] = Color.white;
            colorWhite[i].a = 0;
        }

        tex.SetPixels(0, 0, 140, 140, colorWhite);
        tex.Apply();
        GetComponent<RawImage>().texture = tex;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPainting && cursorInside)
        {
            Vector2Int texPixelPos = CalculatePixelPosition();
            tex.SetPixel(texPixelPos.x, texPixelPos.y, Color.black);
            tex.Apply();
        }
    }

    Vector2Int CalculatePixelPosition()
    {
        // Get from https://answers.unity.com/questions/1709599/find-which-part-of-an-image-was-clicked.html

        //Get the screen size
        Vector2Int screenSize = new Vector2Int(Screen.width, Screen.height);

        //Get the texture size
        Vector2Int textureSize = new Vector2Int(tex.width, tex.height);

        //Get the screen position of the texture (This will be the center of the image)
        Vector2 screenPos = GameManager.get.mainCam.WorldToScreenPoint(gameObject.transform.position);
        Debug.Log(screenPos/screenSize);
        Vector2Int textureScreenPosition = new Vector2Int(Mathf.RoundToInt(screenPos.x), Mathf.RoundToInt(screenPos.y));

        //Get the 0,0 position of the texture:
        Vector2Int textureStartPosition = textureScreenPosition - textureSize / 2;

        //Subtract the 0,0 position of the texture from the mouse click position.
        return new Vector2Int(Mathf.RoundToInt(Input.mousePosition.x), Mathf.RoundToInt(Input.mousePosition.y)) - textureStartPosition;
    }

    public void ClearTexture()
    {
        tex.SetPixels(0, 0, 140, 140, colorWhite);
        tex.Apply();
    }
}
