using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class StopwatchVoltorb : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField] Stopwatch stopwatch;

    // Function called by Unity when the mouse button is pressed 
    // It decides what to do depending on the current state
    public void OnPointerDown(PointerEventData eventData)
    {
        switch (stopwatch.currentState)
        {
            case Stopwatch.StopWatchState.Idle:
                stopwatch.ChangeState((int)Stopwatch.StopWatchState.PushedIdle);
                break;
            case Stopwatch.StopWatchState.PushedIdle:
                Debug.LogError("What? Shouldn't be possible xd");
                break;
            case Stopwatch.StopWatchState.Counting:
                stopwatch.ChangeState((int)Stopwatch.StopWatchState.PushedCount);
                break;
            case Stopwatch.StopWatchState.Explode:
                // Nothing to do here
                break;
            case Stopwatch.StopWatchState.PushedCount:
                Debug.LogError("What? Shouldn't be possible xd");
                break;
            case Stopwatch.StopWatchState.None:
                Debug.LogError("What? Shouldn't be possible xd");
                break;
        }
    }

    // Function called by Unity when the mouse button is released 
    // It decides what to do depending on the current state
    public void OnPointerUp(PointerEventData eventData)
    {
        switch (stopwatch.currentState)
        {
            case Stopwatch.StopWatchState.Idle:
                // Nothing to do here
                break;
            case Stopwatch.StopWatchState.PushedIdle:
                stopwatch.ChangeState((int)Stopwatch.StopWatchState.Counting);
                break;
            case Stopwatch.StopWatchState.Counting:
                Debug.LogError("What? Shouldn't be possible xd");
                break;
            case Stopwatch.StopWatchState.Explode:
                // Nothing to do here
                break;
            case Stopwatch.StopWatchState.PushedCount:
                stopwatch.ChangeState((int)Stopwatch.StopWatchState.Idle);
                break;
            case Stopwatch.StopWatchState.None:
                Debug.LogError("What? Shouldn't be possible xd");
                break;
        }
    }

    // Function called by Unity when the cursor goes outside the image 
    // It decides what to do depending on the current state
    public void OnPointerExit(PointerEventData eventData)
    {
        switch (stopwatch.currentState)
        {
            case Stopwatch.StopWatchState.PushedIdle:
                stopwatch.ChangeState((int)Stopwatch.StopWatchState.Idle);
                break;
            case Stopwatch.StopWatchState.PushedCount:
                stopwatch.ChangeState((int)Stopwatch.StopWatchState.Idle);
                break;
        }
    }

    // Repeated Functions because I had to addapt the scripts
    // to the Poketch Functions Structure
    public void ChangeState(int newState)
    {
        stopwatch.ChangeState(newState);
    }

    public void ResetClock()
    {
        stopwatch.ResetClock();
    }
}
