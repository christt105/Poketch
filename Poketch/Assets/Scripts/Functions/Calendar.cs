using System;
using UnityEngine;
using SimpleJSON;
using UnityEngine.UI;

public class Calendar : Function
{
    public GameObject daysContainer;
    public NumberController m_monthNumber;
    public Sprite actualDaySprite;

    private Vector2Int m_LastDate = Vector2Int.one * -1;

    public override void OnCreate( JSONNode jsonObject )
    {
        UpdateCalendarMonthDays();
    }
    public override void OnChange()
    {
        CancelInvoke();
        InvokeRepeating(nameof(UpdateCalendar), 0f, 2f);
    }
    // Update is called once per frame
    private void UpdateCalendar()
    {
        int day = DateTime.Now.Day;
        int month = DateTime.Now.Month;

        if(m_LastDate.x != month)
        {
            m_monthNumber.SetNumber(month);
            m_LastDate.x = month;

            UpdateCalendarMonthDays();
        }

        if (m_LastDate.y != day)
        {
            m_LastDate.y = day;
        }

    }

    private void UpdateCalendarMonthDays()
    {
        DateTime firstMonthDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        int firstMonthDayWeekDay = (int)firstMonthDay.DayOfWeek;
        int firstWeekEmptyDays = firstMonthDayWeekDay - 1;

        int daysInCurrentMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

        int currentDaysChecked = 0;


        for (int i = 0; i < daysContainer.transform.childCount; ++i) 
        {
            daysContainer.transform.GetChild(i).GetComponent<Image>().sprite = null;

            if (i > firstWeekEmptyDays && i - firstWeekEmptyDays < daysInCurrentMonth + 1)
            {
                currentDaysChecked++;
                daysContainer.transform.GetChild(i).GetComponent<NumberController>().SetNumber(currentDaysChecked);
            }
        }

        daysContainer.transform.GetChild(DateTime.Now.Day - 1 + firstWeekEmptyDays).GetComponent<Image>().sprite = actualDaySprite;
    }
}
