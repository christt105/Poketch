using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Roulette : Function
{
    public enum State
    {
        Stopped,
        Spinning,
        Stopping,
    }

    State currentState = State.Stopped;

    [Header("Game Objects")]
    [SerializeField] Button playButton;
    [SerializeField] Button stopButton;
    [SerializeField] Button resetButton;
    [SerializeField] RectTransform rouletteArrow;
    [SerializeField] RoulettePaintTexture paintTexture;

    Texture2D tex;
    Color[] pixelColors = new Color[70 * 70];

    float velocity = 0;
    float maxVelocity = 10;

    bool firstFrame = true;

    public override void OnCreate(JSONNode jsonObject)
    {
        tex = new Texture2D(70, 70);
        tex.filterMode = FilterMode.Point;

        ChangeState((int)State.Stopped);
        
        for (int i = 0; i < tex.width * tex.height; ++i)
        {
            pixelColors[i] = Color.white;
        }
    }

    private void OnEnable()
    {
        Signals.onScreenTouched += Paint;
    }

    private void OnDisable()
    {
        Signals.onScreenTouched -= Paint;
    }

    public override void OnChange()
    {
        Signals.SignalOnInitializeValues(tex, new Vector2Int(tex.width, tex.height), pixelColors);
        Signals.SignalOnResetTexture();
    }

    private void Update()
    {
        if (firstFrame) firstFrame = false;

        switch (currentState)
        {
            case State.Stopped:



                break;
            case State.Spinning:

                if (velocity < maxVelocity)
                {
                    velocity = Mathf.Min(velocity + maxVelocity * 0.5f * Time.deltaTime, maxVelocity);
                }

                rouletteArrow.Rotate(Vector3.forward, -velocity);

                break;
            case State.Stopping:

                if (velocity > 0)
                {
                    velocity = Mathf.Max(velocity - maxVelocity * 0.25f * Time.deltaTime, 0);
                }

                rouletteArrow.Rotate(Vector3.forward, -velocity);

                if (velocity == 0)
                {
                    ChangeState((int)State.Stopped);
                }

                break;
        }
    }

    public void ChangeState(int newState)
    {
        switch ((State)newState)
        {
            case State.Stopped:

                if (!firstFrame) SoundManager.Instance.PlaySFX(SoundManager.SFX.RouletteStop);

                stopButton.interactable = false;
                playButton.interactable = true;
                resetButton.interactable = true;

                break;
            case State.Spinning:

                SoundManager.Instance.PlaySFX(SoundManager.SFX.Button);

                stopButton.interactable = true;
                playButton.interactable = false;
                resetButton.interactable = false;

                break;
            case State.Stopping:

                SoundManager.Instance.PlaySFX(SoundManager.SFX.Button);

                stopButton.interactable = false;

                break;
        }

        currentState = (State)newState;
    }

    public void ResetCanvas()
    {
        Signals.SignalOnResetTexture();
        SoundManager.Instance.PlaySFX(SoundManager.SFX.Button);
    }

    void Paint()
    {
        if (currentState == State.Stopped)
        {
            Signals.SignalOnPaint(Color.black);
        }
    }
}
