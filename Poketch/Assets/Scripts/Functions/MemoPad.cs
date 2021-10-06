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

    public static readonly int width = 156 / 2;
    public static readonly int height = 150 / 2;

    public Color[] pixelColors { get; private set; } = new Color[width * height];
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

    public delegate void OnPaint(Color colorToPaint);
    public static event OnPaint onPaint;
    public delegate void OnResetTexture();
    public static event OnResetTexture onResetTexture;

    // This function is used as a Start function.
    // @_jsonObject has information of the JSON file.

    public override void OnCreate(JSONObject _jsonObject)
    {
        m_ButtonErase.onClick.AddListener(() => ChangeState(ACTION_STATE.ERASING));
        m_ButtonPaint.onClick.AddListener(() => ChangeState(ACTION_STATE.PAINTING));
        m_ButtonPaint.Select();

        // Connect the ScreenTouched() method to the PaintTexture signal.
        PaintTexture.onScreenTouched += ScreenTouched;
      

        for (int i = 0; i < width * height; ++i)
        {
            pixelColors[i] = Color.white;
        }
        m_renderer_texture = new Texture2D(width, height);
        m_renderer_texture.filterMode = FilterMode.Point;
        m_ImageToPaint.texture = m_renderer_texture;
    }

    public override void OnChange()
    {
        onResetTexture();
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
            onPaint(Color.black);
        }
        else
        {
            onPaint(Color.white);
        }
    }

}
