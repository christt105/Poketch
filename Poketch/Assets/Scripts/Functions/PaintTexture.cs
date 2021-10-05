using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using SimpleJSON;

[ RequireComponent( typeof(RawImage))]
public class PaintTexture : Function, IPointerDownHandler
{
    [SerializeField]
    private MemoPad memoPad;
    private Texture2D m_renderer_texture;
    private Color[] pixelColors = new Color[156*150];
    public override void OnCreate(JSONObject jsonObject)
    {
        for (int i = 0; i < 156 * 150; ++i)
        {
            pixelColors[i] = Color.white;
        }
        m_renderer_texture = new Texture2D(156, 150);
        GetComponent<RawImage>().texture = m_renderer_texture;
    }

    public override void OnChange()
    {
        m_renderer_texture.SetPixels(pixelColors);
        m_renderer_texture.Apply();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (memoPad.current_state == MemoPad.ACTION_STATE.PAINTING)
        {
            Paint(eventData);
        }
        else
        {
            Erase(eventData);
        }
    }



    private void Paint(PointerEventData eventData)
    {
        Vector2 localCursor;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), Input.mousePosition, eventData.pressEventCamera, out localCursor))
            return;
        localCursor.x += m_renderer_texture.width * 0.5f;
        localCursor.y += m_renderer_texture.height * 0.5f;
        m_renderer_texture.SetPixel((int)localCursor.x, (int)localCursor.y, Color.black);
        m_renderer_texture.Apply();
    }

    private void Erase(PointerEventData eventData)
    {

    }

   
}
