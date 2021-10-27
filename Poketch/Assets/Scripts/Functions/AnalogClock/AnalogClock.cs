using SimpleJSON;
using UnityEngine;
using System;

public class AnalogClock : Function
{
    [Header("Game Objects")]
    [SerializeField] RectTransform shortHand;
    [SerializeField] RectTransform longHand;

    Vector2Int time = Vector2Int.zero;

    private void Update()
    {
        UpdateClock();
    }

    void UpdateClock()
    {
        int hour = DateTime.Now.Hour;
        int minutes = DateTime.Now.Minute;

        if (time.y == minutes && time.x == hour) return;

        // Addapt 24h format to 12h format
        int angleH = hour >= 12 ? hour - 12 : hour;

        // 30 comes from 360 divided by 12. Degrees for each hour.
        // Minutes * 0.5f is for adjusting the hour hand to the current minute.
        float angle = angleH * 30 + minutes * 0.5f;
        shortHand.rotation = Quaternion.Euler(new Vector3(0, 0, -angle));

        // 6 comes from dividing 360 by 60. Degrees for each minute.
        angle = minutes * 6;
        longHand.rotation = Quaternion.Euler(new Vector3(0, 0, -angle));

        time.Set(hour, minutes);
    }
}
