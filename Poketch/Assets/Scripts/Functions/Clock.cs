using System;
using SimpleJSON;
using UnityEngine;

public class Clock : Function
{
    [SerializeField]
    private Transform m_Digits;

    private float m_Timer = 0f;

    public override void OnCreate( JSONObject jsonObject )
    {
        UpdateClock();
        m_Timer = 0f;
    }

    public override void OnChange()
    {
        UpdateClock();
        m_Timer = 0f;
    }

    #region Private

    private void Update() //TODO: Coroutine (?)
    {
        m_Timer += Time.deltaTime;

        if ( m_Timer < 0.5f )
        {
            return;
        }

        UpdateClock();

        m_Timer = 0f;
    }

    private void UpdateClock()
    {
        int hours = DateTime.Now.Hour;
        int minutes = DateTime.Now.Minute;

        foreach ( Transform digit in m_Digits ) //TODO: Only change if number changed
        {
            foreach ( Transform d in digit )
            {
                d.gameObject.SetActive( false );
            }
        }

        m_Digits.GetChild( 0 ).GetChild( hours / 10 ).gameObject.SetActive( true );
        m_Digits.GetChild( 1 ).GetChild( hours % 10 ).gameObject.SetActive( true );
        m_Digits.GetChild( 2 ).GetChild( minutes / 10 ).gameObject.SetActive( true );
        m_Digits.GetChild( 3 ).GetChild( minutes % 10 ).gameObject.SetActive( true );
    }

    #endregion
}
