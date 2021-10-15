using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PedometerU;
using SimpleJSON;

public class PokePedometer : Function
{
    Pedometer pedometer;

    [SerializeField] NumberController stepsDigits;

    int currentSteps = 0;
    int lastSteps = 0;
    public override void OnCreate(JSONNode jsonObject)
    {
        pedometer = new Pedometer(OnStep);

        if (jsonObject != null)
        {
            currentSteps = jsonObject.GetValueOrDefault("steps", 0);

            stepsDigits.SetNumber(currentSteps);
        }
    }

    public override void OnExit()
    {
        lastSteps = currentSteps;
    }

    public override void OnChange()
    {
        if (lastSteps != currentSteps)
        {
            lastSteps = currentSteps;

            stepsDigits.SetNumber(currentSteps);
            SaveSteps();
        }
    }

    void OnStep(int steps, double distance)
    {
        currentSteps = currentSteps + steps > 9999 ? 9999 : currentSteps + steps;

        if (gameObject.activeSelf)
        {
            stepsDigits.SetNumber(currentSteps);
            SaveSteps();
        }
    }

    public void ResetSteps()
    {
        SoundManager.Instance.PlaySFX(SoundManager.SFX.Button);
        currentSteps = lastSteps = 0;
        stepsDigits.SetNumber(currentSteps);

        SaveSteps();
    }

    void SaveSteps()
    {
        JSONNode json = new JSONObject();
        json.Add("steps", currentSteps);
        FunctionController.Instance.SaveFunctionInfo(GetType().Name, json);
    }
}
