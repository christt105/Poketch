using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

public class MemoPad : Function
{
    // Reference to the erase button.
    [SerializeField]
    private Button m_ButtonErase;
    // Reference to the paint button.
    [SerializeField]
    private Button m_ButtonPaint;

    // Enum that has the different possible actions.
    public enum ACTION_STATE
    {
        NONE = 0,
        PAINTING,
        ERASING
    }
    // Variable that has the current state of the action we are performing.
    public ACTION_STATE current_state = ACTION_STATE.PAINTING;

    // This function is used as a Start function.
    // @_jsonObject has information of the JSON file.
    public override void OnCreate(JSONObject _jsonObject)
    {
        m_ButtonErase.onClick.AddListener(() => ChangeState(ACTION_STATE.ERASING));
        m_ButtonPaint.onClick.AddListener(() => ChangeState(ACTION_STATE.PAINTING));
        m_ButtonPaint.Select();
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
}
