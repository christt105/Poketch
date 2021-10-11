using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AlarmClock : Function
{
    public enum State
    {
        Clock,
        Alarm,
    }

    State currentState = State.Alarm;

    [Header("Game Objects")]
    [SerializeField] Image background;
    [SerializeField] Button clockButton;
    [SerializeField] Button alarmButton;
    [SerializeField] NumberController hours;
    [SerializeField] NumberController minutes;
    [SerializeField] GameObject arrowsParent;
    [SerializeField] Image[] arrowButtons;

    [Header("Sprites")]
    [SerializeField] Sprite pressedButton;
    [SerializeField] Sprite idleButton;

    [Header("Backgrounds")]
    [SerializeField] Sprite alarmSetBackground;
    [SerializeField] Sprite clockBackground;
    [SerializeField] Sprite[] alarmSoundBackgrounds;

    Vector2Int lastTime = Vector2Int.zero;
    Vector2Int alarm = Vector2Int.zero;

    float timeBG = 0;
    float timeButtons = 0;

    public override void OnCreate(JSONNode jsonObject)
    {
        SetState((int)State.Alarm);
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Clock:

                if (lastTime.y == DateTime.Now.Minute && lastTime.x == DateTime.Now.Minute && lastTime != alarm) return;

                lastTime.Set(DateTime.Now.Hour, DateTime.Now.Minute);
                hours.SetNumber(lastTime.x);
                minutes.SetNumber(lastTime.y);

                if (alarm == lastTime)
                {
                    if (background.sprite == clockBackground) background.sprite = alarmSoundBackgrounds[0];

                    timeBG += Time.deltaTime;
                    if (timeBG >= 0.1f)
                    {
                        background.sprite = background.sprite == alarmSoundBackgrounds[0] ? alarmSoundBackgrounds[1] : alarmSoundBackgrounds[0];
                        timeBG = 0;
                    }
                }
                else
                {
                    if (background.sprite != clockBackground) background.sprite = clockBackground;
                }

                break;
            case State.Alarm:

                if (timeButtons >= 0.5f)
                {
                    for (int i = 0; i < arrowButtons.Length; ++i)
                    {
                        Color c = arrowButtons[i].color;
                        c.a = c.a == 1 ? 0 : 1;
                        arrowButtons[i].color = c;
                    }

                    timeButtons = 0;
                }

                timeButtons += Time.deltaTime;

                break;
        }
    }

    public void SetState(int newState)
    {
        arrowsParent.SetActive((State)newState == State.Alarm);
        
        switch ((State)newState)
        {
            case State.Clock:

                background.sprite = clockBackground;
                clockButton.image.sprite = pressedButton;
                alarmButton.image.sprite = idleButton;

                timeBG = 0;

                break;
            case State.Alarm:

                background.sprite = alarmSetBackground;
                clockButton.image.sprite = idleButton;
                alarmButton.image.sprite = pressedButton;

                hours.SetNumber(alarm.x);
                minutes.SetNumber(alarm.y);

                for (int i = 0; i < arrowButtons.Length; ++i)
                {
                    arrowButtons[i].color = Color.white;
                }

                timeButtons = 0;

                break;
        }

        currentState = (State)newState;
    }

    public void ChangeAlarmHour(int quantity)
    {
        alarm.x += quantity;

        if (alarm.x < 0) alarm.x = 24 + alarm.x;
        if (alarm.x > 23) alarm.x = alarm.x - 24;

        hours.SetNumber(alarm.x);
    }

    public void ChangeAlarmMinute(int quantity)
    {
        alarm.y += quantity;

        if (alarm.y < 0) alarm.y = 60 + alarm.y;
        if (alarm.y > 59) alarm.y = alarm.y - 60;

        minutes.SetNumber(alarm.y);
    }
}
