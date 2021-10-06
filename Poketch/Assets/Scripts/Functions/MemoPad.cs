using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MemoPad : Function
{
    // Reference to the erase button.
    [SerializeField]
    private Button m_ButtonErase;
    // Reference to the paint button.
    [SerializeField]
    private Button m_ButtonPaint;
    [SerializeField]
    private RawImage m_ImageToPaint;

    private Color[] pixelColors = new Color[156 * 150];
    public Texture2D m_renderer_texture { get; private set; }

    // Enum that has the different possible actions.
    public enum ACTION_STATE
    {
        NONE = 0,
        PAINTING,
        ERASING
    }
    // Variable that has the current state of the action we are performing.
    public ACTION_STATE current_state = ACTION_STATE.PAINTING;

    public delegate void OnPaint();
    public static event OnPaint onPaint;
    public delegate void OnErase();
    public static event OnErase onErase;

    // This function is used as a Start function.
    // @_jsonObject has information of the JSON file.

    public override void OnCreate(JSONObject _jsonObject)
    {
        m_ButtonErase.onClick.AddListener(() => ChangeState(ACTION_STATE.ERASING));
        m_ButtonPaint.onClick.AddListener(() => ChangeState(ACTION_STATE.PAINTING));
        m_ButtonPaint.Select();

        // Connect the ScreenTouched() method to the PaintTexture signal.
        PaintTexture.onScreenTouched += ScreenTouched;
      

        for (int i = 0; i < 156 * 150; ++i)
        {
            pixelColors[i] = Color.white;
        }
        m_renderer_texture = new Texture2D(156, 150);
        m_ImageToPaint.texture = m_renderer_texture;
    }

    public override void OnChange()
    {
        m_renderer_texture.SetPixels(pixelColors);
        m_renderer_texture.Apply();
    }

    // Method that change the current action state and perform the logic and graphic part.
    // This method is called when one of the two buttons is clicked.
    // @action is an ACTION_STATE variable used to know which action we want to perform.
    private void ChangeState(ACTION_STATE action)
    {
        if (action != current_state)
        {
            current_state = action;
            SoundManager.Instance.PlaySFX(SoundManager.SFX.Button);
        }
        else
        {
            //bad
        }
    }

    private void ScreenTouched()
    {
        if (current_state == ACTION_STATE.PAINTING)
        {
            onPaint();
        }
        else
        {
            onErase();
        }
    }

}
