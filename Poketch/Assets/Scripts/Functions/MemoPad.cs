using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

public class MemoPad : Function
{
    // Global variables for the different actions we have.
    private const string PAINT = "paint";
    private const string ERASE = "erase";

    // Reference to the erase button.
    [SerializeField]
    private Button m_ButtonErase;
    // Reference to the paint button.
    [SerializeField]
    private Button m_ButtonPaint;

    // Enum that has the different possible actions.
    enum ACTION_STATE
    {
        NONE = 0,
        PAINTING,
        ERASING
    }
    // Variable that has the current state of the action we are performing.
    private ACTION_STATE m_current_state = ACTION_STATE.PAINTING;

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
        switch (m_current_state)
        {
            case ACTION_STATE.PAINTING:
                {
                    if(action == ACTION_STATE.ERASING)
                    {
                        // We are able to erase.
                        m_current_state = ACTION_STATE.ERASING;
                        //TODO: Deseleccionar el botó de paint i seleccionar el botó de erase.
                        //TODO: Afegir so?
                    }
                    else
                    {
                        //TODO: No pots desseleccionar ell mateix. Afegir so de que no pot.
                    }
                    break;
                }
            case ACTION_STATE.ERASING:
                {
                    if (action == ACTION_STATE.PAINTING)
                    {
                        // We are able to erase.
                        m_current_state = ACTION_STATE.PAINTING;
                        //TODO: Deseleccionar el botó de erase i seleccionar el botó de paint.
                        //TODO: Afegir so?
                    }
                    else
                    {
                        //TODO: No pots desseleccionar ell mateix. Afegir so de que no pot.
                    }
                    break;
                }
        }
    }

    private void TouchedScreen()
    {
        //TODO: Canviar el nom de la funció. Aquí es decideix quina acció es fa i es crida Paint() o Erase().
    }

    private void Paint() //TODO: Optimize
    {
       
    }

    private void Erase() //TODO: Optimize
    {
    }
}
