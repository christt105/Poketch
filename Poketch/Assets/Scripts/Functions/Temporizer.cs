using System;
using System.Collections;
using UnityEngine.UI;
using SimpleJSON;
using UnityEngine;

public class Temporizer : Function
{
    enum Buttons
    {
        START,
        STOP,
        RESET
    }
    
    enum TimePos
    {
        SEC,
        TENTH_SEC,
        MIN,
        TENTH_MIN
    }

    [Serializable]
    public struct SnorlaxTime
    {
        public Transform[] slotTime;
    }
    [SerializeField]
    private SnorlaxTime[] m_ArraySpritesTime;

    [Serializable]
    public struct Button
    {
        public Transform button;
        public Text buttonText;
        public bool m_is_selected;
        [Range(0, 50)]
        public float offset_pushed;
    }
    [SerializeField]
    private Button[] m_ArrayButtons;

    private int[] m_ArrayActualTime;

    private bool m_enable_timer;

    [SerializeField]
    private Transform[] m_ArrayArrows;

    [SerializeField]
    private Sprite m_imgArrow;

    private bool enable_arrows;

    [SerializeField] [Range(0f, 3f)]
    private float arrow_anim_duration;

    private float m_totalTimeSec;


    #region Override Functions

    public override void OnCreate(JSONObject jsonObject)
    {
        m_ArrayActualTime = new int[4];
        for (int i = 0; i < m_ArrayActualTime.Length; i++)
            m_ArrayActualTime[i] = 0;

        ChangeStateButton((int)Buttons.STOP, true);


        enable_arrows = true;
    }

    public override void OnChange()
    {
        for (int i = 0; i < m_ArrayActualTime.Length; i++)
            m_ArrayActualTime[i] = 0;

    }

    #endregion
    #region Private

    private void Start()
    {
        StartCoroutine(ArrowAnim());
    }

    private void Update()
    {
        if (m_enable_timer)
        {
            m_totalTimeSec -= Time.deltaTime;

            if (m_totalTimeSec < 0.1f)
            {
                m_totalTimeSec = 0;
                m_enable_timer = false;
                ChangeStateButton((int)Buttons.START, false);

                ResetTimer();
            }
            ShowTimerCountDown();
        }
    }

    private void ShowTimerCountDown()
    {
        float minutes = Mathf.Floor(m_totalTimeSec / 60);
        double seconds = Mathf.RoundToInt(m_totalTimeSec % 60);

        DisableAllSprites();

        m_ArrayActualTime[3] = (int)minutes / 10;
        m_ArrayActualTime[2] = (int)minutes % 10;
        m_ArrayActualTime[1] = (int)seconds / 10;
        m_ArrayActualTime[0] = (int)seconds % 10;

        m_ArraySpritesTime[3].slotTime[m_ArrayActualTime[3]].gameObject.SetActive(true);
        m_ArraySpritesTime[2].slotTime[m_ArrayActualTime[2]].gameObject.SetActive(true);
        m_ArraySpritesTime[1].slotTime[m_ArrayActualTime[1]].gameObject.SetActive(true);
        m_ArraySpritesTime[0].slotTime[m_ArrayActualTime[0]].gameObject.SetActive(true);

    }

    private void ResetTimer()
    {
        DisableAllSprites();
        for (int i = 0; i < m_ArrayActualTime.Length; i++)
        {
            m_ArraySpritesTime[i].slotTime[m_ArrayActualTime[i]--].gameObject.SetActive(false);
            m_ArrayActualTime[i] = 0;
            m_ArraySpritesTime[i].slotTime[m_ArrayActualTime[i]].gameObject.SetActive(true);
        }

        ChangeStateButton((int)Buttons.STOP, true);
        ChangeStateButton((int)Buttons.START, false);


        enable_arrows = true;
        m_enable_timer = false;
    }

    private void DisableAllSprites()
    {
        for (int i = 0; i < m_ArrayActualTime.Length; i++) // Set all actual sprites to false
            for (int j = 0; j < m_ArraySpritesTime[(int)TimePos.SEC].slotTime.Length; j++)
                m_ArraySpritesTime[i].slotTime[j].gameObject.SetActive(false);
    }

    private void ChangeStateButton(int position, bool value)
    {
        if (!m_ArrayButtons[position].m_is_selected && value)
            m_ArrayButtons[position].button.position = new Vector3(m_ArrayButtons[position].button.position.x, m_ArrayButtons[position].button.position.y - m_ArrayButtons[position].offset_pushed, m_ArrayButtons[position].button.position.z);
        else if (m_ArrayButtons[position].m_is_selected && !value)
            m_ArrayButtons[position].button.position = new Vector3(m_ArrayButtons[position].button.position.x, m_ArrayButtons[position].button.position.y + m_ArrayButtons[position].offset_pushed, m_ArrayButtons[position].button.position.z);

        m_ArrayButtons[position].m_is_selected = value;
    }

    private int CalculateTimeInSeconds() { return (m_ArrayActualTime[(int)TimePos.SEC] + m_ArrayActualTime[(int)TimePos.TENTH_SEC] * 10 + m_ArrayActualTime[(int)TimePos.MIN] * 60 + m_ArrayActualTime[(int)TimePos.TENTH_MIN] * 600); }

    #endregion
    #region Button Functions

    public void NextTimer(int position)
    {
        m_ArraySpritesTime[position].slotTime[m_ArrayActualTime[position]].gameObject.SetActive(false);
        m_ArrayActualTime[position]++;

        if (m_ArrayActualTime[(int)TimePos.SEC] > 9 || m_ArrayActualTime[(int)TimePos.TENTH_SEC] > 9 || m_ArrayActualTime[(int)TimePos.TENTH_SEC] > 5 || m_ArrayActualTime[(int)TimePos.TENTH_MIN] > 5)
            m_ArrayActualTime[position] = 0;


        m_ArraySpritesTime[position].slotTime[m_ArrayActualTime[position]].gameObject.SetActive(true);
    }

    public void PrevTimer(int position)
    {
        m_ArraySpritesTime[position].slotTime[m_ArrayActualTime[position]].gameObject.SetActive(false);
        m_ArrayActualTime[position]--;

        if (m_ArrayActualTime[(int)TimePos.SEC] < 0 || m_ArrayActualTime[(int)TimePos.MIN] < 0 || m_ArrayActualTime[(int)TimePos.TENTH_MIN] < 0) 
            m_ArrayActualTime[position] = 9;
        else if (m_ArrayActualTime[(int)TimePos.TENTH_SEC] < 0) 
            m_ArrayActualTime[position] = 5;

        m_ArraySpritesTime[position].slotTime[m_ArrayActualTime[position]].gameObject.SetActive(true);
    }

    public void StartButton()
    {
        if(!m_ArrayButtons[(int)Buttons.START].m_is_selected && !(m_ArrayActualTime[(int)TimePos.SEC] == 0 && m_ArrayActualTime[(int)TimePos.TENTH_SEC] == 0 && m_ArrayActualTime[(int)TimePos.MIN] == 0 && m_ArrayActualTime[(int)TimePos.TENTH_MIN] == 0))
        {
            ChangeStateButton((int)Buttons.START, true);
            ChangeStateButton((int)Buttons.STOP, false);

            enable_arrows = false;
            m_enable_timer = true;

            m_totalTimeSec = CalculateTimeInSeconds();
            Debug.Log("--- Total time: " + m_totalTimeSec + " ---");
        }
        
    }

    public void StopButton()
    {
        if(!m_ArrayButtons[(int)Buttons.STOP].m_is_selected)
        {
            ChangeStateButton((int)Buttons.STOP, true);
            ChangeStateButton((int)Buttons.START, false);

            enable_arrows = true;
            m_enable_timer = false;
        }

    }

    public void ResetButton()
    {
        ResetTimer();
        StartCoroutine(ResetButtonAnim());
    }

    #endregion
    #region Corutines

    private IEnumerator ResetButtonAnim()
    {
        float anim_duration = .2f;
        m_ArrayButtons[(int)Buttons.RESET].button.position = new Vector3(m_ArrayButtons[(int)Buttons.RESET].button.position.x, m_ArrayButtons[(int)Buttons.RESET].button.position.y - m_ArrayButtons[(int)Buttons.RESET].offset_pushed, m_ArrayButtons[(int)Buttons.RESET].button.position.z);

        yield return new WaitForSeconds(anim_duration);
        m_ArrayButtons[(int)Buttons.RESET].button.position = new Vector3(m_ArrayButtons[(int)Buttons.RESET].button.position.x, m_ArrayButtons[(int)Buttons.RESET].button.position.y + m_ArrayButtons[(int)Buttons.RESET].offset_pushed, m_ArrayButtons[(int)Buttons.RESET].button.position.z);
    }

    private IEnumerator ArrowAnim()
    {
        if (enable_arrows)
        {
            foreach (Transform arrow in m_ArrayArrows)
                arrow.gameObject.SetActive(false);

            yield return new WaitForSeconds(arrow_anim_duration);


            foreach (Transform arrow in m_ArrayArrows)
                arrow.gameObject.SetActive(true);
        }
        else
        {
            foreach (Transform arrow in m_ArrayArrows)
            {
                arrow.gameObject.SetActive(false);
            }
        }

        yield return new WaitForSeconds(arrow_anim_duration);

        StartCoroutine(ArrowAnim());
    }
    #endregion
}
