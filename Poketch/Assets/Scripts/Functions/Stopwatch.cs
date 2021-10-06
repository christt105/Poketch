using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Stopwatch : Function
{
    public enum StopWatchState
    {
        Idle = 0,
        PushedIdle = 1,
        Counting = 2,
        Explode = 3,
        PushedCount = 4,

        None = -1
    }

    [SerializeField] NumberController[] numbers;
    [SerializeField] Animator anim;
    [HideInInspector] public StopWatchState currentState = StopWatchState.Idle;

    float time = 0;

    public override void OnExit()
    {
        ChangeState((int)StopWatchState.Idle);
    }

    // Function to change the Animator's State
    // It uses an int as parameter to allow to call this function from
    // Animation Callbacks
    public void ChangeState(int newState)
    {
        anim.SetInteger("State", newState);
        currentState = (StopWatchState)newState;
    }

    // Function called to Reset the clock
    // It's used from an Animation Callback
    public void ResetClock()
    {
        time = 0;
        SetClockNumbers();
    }

    private void Update()
    {
        if (currentState == StopWatchState.Counting)
        {
            time += Time.deltaTime;
            SetClockNumbers();
        }
    }

    // Function to set the time counted to the UI numbers
    void SetClockNumbers()
    {
        // Conversion to calculate hours (3600 seconds per hour)
        int hours = Mathf.FloorToInt(time / 3600);

        if (hours > 99)
        {
            time = 0;
            SetClockNumbers();
            return;
        }

        // Conversion to calculate minutes
        int minutes = Mathf.FloorToInt(time / 60 - hours * 60);

        //Conversion to calculate seconds
        int seconds = Mathf.FloorToInt(time - minutes * 60 - hours * 3600);

        // Conversion to get seconds
        int miliseconds = Mathf.FloorToInt((time - Mathf.FloorToInt(time)) * 100);
        
        // Setting UI numbers
        numbers[0].SetNumber(hours);
        numbers[1].SetNumber(minutes);
        numbers[2].SetNumber(seconds);
        numbers[3].SetNumber(miliseconds);
    }
}
