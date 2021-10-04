using System;
using System.Collections;
using UnityEngine.UI;
using SimpleJSON;
using UnityEngine;

public class Temporizer : Function
{

    [Serializable]
    public struct SnorlaxTime
    {
        public Transform[] slotTime;
    }

    [SerializeField]
    private SnorlaxTime[] m_ArraySpritesTime;
    private int[] m_ArrayActualTime;
    private bool m_enable_timer;

    [SerializeField]
    private Transform[] m_ArrayButtons;
    [SerializeField]
    private Sprite m_imgButton;
    [SerializeField]
    private Sprite m_imgButtonPressed;

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

        enable_arrows = true;
    }

    public override void OnChange()
    {
        for (int i = 0; i < m_ArrayActualTime.Length; i++)
            m_ArrayActualTime[i] = 0;

        StartCoroutine(ArrowAnim());
    }

    #endregion
    #region Private

    private void Update()
    {
        if (m_enable_timer)
        {
            m_totalTimeSec -= Time.deltaTime;

            if (m_totalTimeSec < 0.1f)
            {
                m_totalTimeSec = 0;
                m_enable_timer = false;
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

        ResetSpriteButtonPressed();

        enable_arrows = true;
        m_enable_timer = false;
    }

    private void DisableAllSprites()
    {
        for (int i = 0; i < m_ArrayActualTime.Length; i++) // Set all actual sprites to false
            for (int j = 0; j < m_ArraySpritesTime[0].slotTime.Length; j++)
                m_ArraySpritesTime[i].slotTime[j].gameObject.SetActive(false);
    }

    private void ResetSpriteButtonPressed()
    {
        for (int i = 0; i < m_ArrayButtons.Length; i++)
            m_ArrayButtons[i].GetComponent<Image>().sprite = m_imgButton;
    }

    private int CalculateTimeInSeconds() { return (m_ArrayActualTime[0] + m_ArrayActualTime[1] * 10 + m_ArrayActualTime[2] * 60 + m_ArrayActualTime[3] * 600); }

    #endregion
    #region Button Functions

    public void NextTimer(int position)
    {
        m_ArraySpritesTime[position].slotTime[m_ArrayActualTime[position]].gameObject.SetActive(false);
        m_ArrayActualTime[position]++;

        if (m_ArrayActualTime[0] > 9 || m_ArrayActualTime[2] > 9 || m_ArrayActualTime[1] > 5 || m_ArrayActualTime[3] > 5) //TODO: Magic Numbers :)
            m_ArrayActualTime[position] = 0;


        m_ArraySpritesTime[position].slotTime[m_ArrayActualTime[position]].gameObject.SetActive(true);
    }

    public void PrevTimer(int position)
    {
        m_ArraySpritesTime[position].slotTime[m_ArrayActualTime[position]].gameObject.SetActive(false);
        m_ArrayActualTime[position]--;

        if (m_ArrayActualTime[0] < 0 || m_ArrayActualTime[2] < 0) //TODO: Magic Numbers :)
            m_ArrayActualTime[position] = 9;
        else if (m_ArrayActualTime[1] < 0 || m_ArrayActualTime[3] < 0) //TODO: Magic Numbers :)
            m_ArrayActualTime[position] = 5;

        m_ArraySpritesTime[position].slotTime[m_ArrayActualTime[position]].gameObject.SetActive(true);
    }

    public void StartButton()
    {
        ResetSpriteButtonPressed();
        m_ArrayButtons[0].GetComponent<Image>().sprite = m_imgButtonPressed;

        enable_arrows = false;
        m_enable_timer = true;

        m_totalTimeSec = CalculateTimeInSeconds();
        Debug.Log("--- Total time: " + m_totalTimeSec + " ---");
    }

    public void StopButton()
    {
        ResetSpriteButtonPressed();
        m_ArrayButtons[1].GetComponent<Image>().sprite = m_imgButtonPressed;
        enable_arrows = true;
        m_enable_timer = false;
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
        m_ArrayButtons[2].GetComponent<Image>().sprite = m_imgButtonPressed; // "2" Position of ResetButton in the SerializField array
        yield return new WaitForSeconds(anim_duration);
        m_ArrayButtons[2].GetComponent<Image>().sprite = m_imgButton;
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
