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

    enum SnorlaxPose
    {
        TIME_OUT,
        TIME_ON,
        TIME_OF
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

    [SerializeField]
    private Transform[] m_ArrayArrows;

    [SerializeField]
    private Sprite m_imgArrow;

    [SerializeField]
    private Transform[] m_ArraySnorlaxPose;

    [SerializeField] [Range(0f, 3f)]
    private float arrow_anim_duration;

    [SerializeField] [Range(0f, 3f)]
    private float snorlax_anim_duration;

    private float m_totalTimeSec;

    private bool m_enable_arrows;

    private int[] m_ArrayActualTime;

    private bool m_enable_timer;

    private bool m_enable_snorlax_anim;

    private bool m_snorlax_paused;

    #region Override Functions

    public override void OnCreate(JSONNode jsonObject)
    {
        m_ArrayActualTime = new int[4];
        for (int i = 0; i < m_ArrayActualTime.Length; i++)
            m_ArrayActualTime[i] = 0;

        ChangeStateButton((int)Buttons.STOP, true);

        DisableSnorlaxAnimations();
        m_ArraySnorlaxPose[(int)SnorlaxPose.TIME_OF].gameObject.SetActive(true);


        m_enable_arrows = true;
    }

    public override void OnChange()
    {
        for (int i = 0; i < m_ArrayActualTime.Length; i++)
            m_ArrayActualTime[i] = 0;

        DisableSnorlaxAnimations();
        m_ArraySnorlaxPose[(int)SnorlaxPose.TIME_OF].gameObject.SetActive(true);

        ResetTimer();
        StartCoroutine(ArrowAnim());
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

            if (m_totalTimeSec <= 0f)
            {
                m_totalTimeSec = 0f;

                DisableSnorlaxAnimations();
                m_ArraySnorlaxPose[(int)SnorlaxPose.TIME_OUT].gameObject.SetActive(true);
                
                m_enable_snorlax_anim = true;
                StartCoroutine(SnorlaxAnim());
                m_enable_timer = false;

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


        m_enable_arrows = true;
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

    private void DisableSnorlaxAnimations()
    {
        for (int i = 0; i < m_ArraySnorlaxPose.Length; i++)
            m_ArraySnorlaxPose[i].gameObject.SetActive(false);

        m_snorlax_paused = false;
        m_enable_snorlax_anim = false;
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
        if(!m_ArrayButtons[(int)Buttons.START].m_is_selected && (!(m_ArrayActualTime[(int)TimePos.SEC] == 0 && m_ArrayActualTime[(int)TimePos.TENTH_SEC] == 0 && m_ArrayActualTime[(int)TimePos.MIN] == 0 && m_ArrayActualTime[(int)TimePos.TENTH_MIN] == 0) || m_snorlax_paused))
        {
            SoundManager.Instance.PlaySFX(SoundManager.SFX.Button);

            ChangeStateButton((int)Buttons.START, true);
            ChangeStateButton((int)Buttons.STOP, false);

            DisableSnorlaxAnimations();
            m_ArraySnorlaxPose[(int)SnorlaxPose.TIME_ON].gameObject.SetActive(true);

            m_snorlax_paused = false;
            m_enable_snorlax_anim = true;
            m_enable_arrows = false;
            m_enable_timer = true;

            m_totalTimeSec = CalculateTimeInSeconds();
            Debug.Log("--- Total time: " + m_totalTimeSec + " ---");
        }
        else
            SoundManager.Instance.PlaySFX(SoundManager.SFX.ButtonDeny);

    }

    public void StopButton()
    {
        if(!m_ArrayButtons[(int)Buttons.STOP].m_is_selected)
        {
            SoundManager.Instance.PlaySFX(SoundManager.SFX.Button);

            ChangeStateButton((int)Buttons.STOP, true);
            ChangeStateButton((int)Buttons.START, false);

            m_enable_snorlax_anim = false;
            m_enable_arrows = false;
            m_enable_timer = false;
        }
        else
            SoundManager.Instance.PlaySFX(SoundManager.SFX.ButtonDeny);
    }

    public void ResetButton()
    {
        SoundManager.Instance.PlaySFX(SoundManager.SFX.Button);

        DisableSnorlaxAnimations();
        m_ArraySnorlaxPose[(int)SnorlaxPose.TIME_OF].gameObject.SetActive(true);

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

    private IEnumerator SnorlaxAnim()
    {
        m_ArraySnorlaxPose[(int)SnorlaxPose.TIME_OUT].transform.localScale = new Vector3(-m_ArraySnorlaxPose[(int)SnorlaxPose.TIME_OUT].transform.localScale.x, m_ArraySnorlaxPose[(int)SnorlaxPose.TIME_OUT].transform.localScale.y, m_ArraySnorlaxPose[(int)SnorlaxPose.TIME_OUT].transform.localScale.z);
        SoundManager.Instance.PlaySFX(SoundManager.SFX.SnorlaxTemporizer);

        yield return new WaitForSeconds(snorlax_anim_duration);
        m_ArraySnorlaxPose[(int)SnorlaxPose.TIME_OUT].transform.localScale = new Vector3(m_ArraySnorlaxPose[(int)SnorlaxPose.TIME_OUT].transform.localScale.x, m_ArraySnorlaxPose[(int)SnorlaxPose.TIME_OUT].transform.localScale.y, m_ArraySnorlaxPose[(int)SnorlaxPose.TIME_OUT].transform.localScale.z);
        SoundManager.Instance.PlaySFX(SoundManager.SFX.SnorlaxTemporizer);

        m_snorlax_paused = true;
        if(m_enable_snorlax_anim)
            StartCoroutine(SnorlaxAnim());
        else
            StopCoroutine(SnorlaxAnim());
    }

    private IEnumerator ArrowAnim()
    {
        if (m_enable_arrows)
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
