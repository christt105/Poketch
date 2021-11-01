using PedometerU;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Android;

public class PokePedometer : Function
{
    [SerializeField]
    private NumberController m_StepsDigits;

    private int m_CurrentSteps = 0;
    private int m_InitialSteps = 0;

    private Pedometer m_Pedometer;

    public override void OnCreate( JSONNode jsonObject )
    {
        Permission.RequestUserPermission( "android.permission.ACTIVITY_RECOGNITION" );

        m_Pedometer = new Pedometer( OnStep );

        if ( jsonObject != null )
        {
            m_InitialSteps = jsonObject.GetValueOrDefault( "steps", 0 );

            m_StepsDigits.SetNumber( m_InitialSteps );
        }
    }

    public override void OnChange()
    {
        m_StepsDigits.SetNumber( m_CurrentSteps );
        SaveSteps();
    }

    private void OnStep( int steps, double distance )
    {
        m_CurrentSteps = m_InitialSteps + steps;

        if ( m_CurrentSteps > 99999 )
        {
            m_CurrentSteps = m_InitialSteps = 0;

            m_Pedometer.Dispose();
            m_Pedometer = new Pedometer( OnStep );
        }

        m_StepsDigits.SetNumber( m_CurrentSteps );
        SaveSteps();
    }

    public void ResetSteps()
    {
        SoundManager.Instance.PlaySFX( SoundManager.SFX.Button );

        m_StepsDigits.SetNumber( m_CurrentSteps );

        m_Pedometer.Dispose();
        m_Pedometer = new Pedometer( OnStep );

        SaveSteps();
    }

    private void SaveSteps()
    {
        JSONNode json = new JSONObject();
        json.Add( "steps", m_CurrentSteps );
        FunctionController.Instance.SaveFunctionInfo( GetType().Name, json );
    }
}
