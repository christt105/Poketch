using SimpleJSON;
using UnityEngine;
using System;

public class AnalogClock : Function
{
    [Header("Game Objects")]
    [SerializeField] RectTransform shortHand;
    [SerializeField] RectTransform longHand;

    Vector2 time = Vector2.zero;

    private void Update()
    {
        UpdateClock();
    }

    void UpdateClock()
    {
        int hour = DateTime.Now.Hour;
        int minutes = DateTime.Now.Minute;

        if (time == new Vector2(hour, minutes)) return;

        int angleH = hour >= 12 ? hour - 12 : hour;
        float angle = angleH * 30 + minutes / 2;
        shortHand.rotation = Quaternion.Euler(new Vector3(0, 0, -angle));

        angle = (float)minutes * 6;
        longHand.rotation = Quaternion.Euler(new Vector3(0, 0, -angle));

        time = new Vector2(hour, minutes);
    }
}
