using System;
using SimpleJSON;
using UnityEngine;

public class Calendar : Function
{
    [SerializeField]
    private GameObject m_DaysContainer;

    [SerializeField]
    private NumberController m_MonthNumber;

    [SerializeField]
    private RectTransform m_ActualDayRect;

    [SerializeField]
    private Color m_NormalColor;

    [SerializeField]
    private Color m_SpecialColor;

    private byte[] m_ActivatedDays = new byte[7 * 6];

    private int m_ActualMonth = -1;

    public override void OnCreate( JSONNode jsonObject )
    {
        m_ActualMonth = DateTime.Now.Month;

        for ( int i = 0; i < m_ActivatedDays.Length; i++ )
        {
            m_ActivatedDays[i] = 0;
        }

        if ( jsonObject != null && jsonObject["actualMonth"].AsInt == m_ActualMonth )
        {
            m_ActivatedDays = jsonObject["activatedDays"].AsByteArray;
        }
    }

    public override void OnChange()
    {
        UpdateCalendarMonthDays();
    }

    private void UpdateCalendarMonthDays()
    {
        DateTime today = DateTime.Today;
        DateTime firstDayOfMonth = new DateTime( today.Year, today.Month, 1 );
        int dayOfWeek = ( int ) firstDayOfMonth.DayOfWeek;
        int daysInMonth = DateTime.DaysInMonth( today.Year, today.Month );

        if ( m_ActualMonth != today.Month )
        {
            for ( int i = 0; i < m_ActivatedDays.Length; i++ )
            {
                m_ActivatedDays[i] = 0;
            }

            m_ActualMonth = today.Month;
        }

        m_MonthNumber.SetNumber( today.Month );

        ClickDay[] days = m_DaysContainer.GetComponentsInChildren < ClickDay >();

        bool firstDayFound = false;
        int day = 0;

        for ( int i = 0; i < days.Length; i++ )
        {
            if ( !firstDayFound )
            {
                if ( i < dayOfWeek )
                {
                    days[i].Hide();

                    continue;
                }

                firstDayFound = true;
            }

            if ( day >= daysInMonth )
            {
                for ( int j = i; j < days.Length; ++j )
                {
                    days[j].Hide();
                }

                break;
            }

            days[i].Unhide();
            days[i].NumberController.SetNumber( ++day );

            if ( day == today.Day )
            {
                m_ActualDayRect.anchoredPosition = days[i].GetComponent < RectTransform >().anchoredPosition;
            }

            if ( m_ActivatedDays[i] == 1 )
            {
                days[i].Select();
            }

            if ( i % 7 == 0 )
            {
                days[i].SetSpecial();
                days[i].SetNumbersColor( m_SpecialColor );
            }
            else
            {
                days[i].SetNumbersColor( m_NormalColor );
            }
        }
    }

    public void Save( int index, bool imageEnabled )
    {
        JSONObject json = new JSONObject();

        m_ActivatedDays[index] = ( byte ) ( imageEnabled ? 1 : 0 );

        json["activatedDays"] = m_ActivatedDays;
        json["actualMonth"] = m_ActualMonth;

        FunctionController.Instance.SaveFunctionInfo( GetType().Name, json );
    }
}
