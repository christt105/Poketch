using UnityEngine;
using UnityEngine.UI;

public class Counter : Function
{
    [SerializeField]
    private Button m_CountButton;

    [SerializeField]
    private Transform m_NumberPanelTransform;
    private Transform m_Thousands;
    private Transform m_Hundreds;
    private Transform m_Tens;
    private Transform m_Units;

    private int m_Count = 0;

    #region Unity Event Functions

    private void Awake()
    {
        m_CountButton.onClick.AddListener( Add );

        m_Thousands = m_NumberPanelTransform.GetChild( 0 );
        m_Hundreds = m_NumberPanelTransform.GetChild( 1 );
        m_Tens = m_NumberPanelTransform.GetChild( 2 );
        m_Units = m_NumberPanelTransform.GetChild( 3 );
    }

    #endregion

    #region Function

    public override void OnChange()
    {
        m_Count = 0;
        UpdateCounter();
    }

    #endregion

    #region Private

    private void Add() //TODO: Optimize
    {
        if( ++m_Count > 9999 )
            m_Count = 0;

        UpdateCounter();

        Poketch.Instance.PlayButton();
    }

    private void UpdateCounter()
    {
        foreach ( Transform number in m_NumberPanelTransform )
        {
            ResetNumbers( number );
        }

        m_Thousands.GetChild( m_Count / 1000 ).gameObject.SetActive( true );
        m_Hundreds.GetChild( ( m_Count / 100 ) % 10 ).gameObject.SetActive( true );
        m_Tens.GetChild( ( m_Count / 10 ) % 10 ).gameObject.SetActive( true );
        m_Units.GetChild( m_Count % 10 ).gameObject.SetActive( true );
    }

    private static void ResetNumbers( Transform numberContainer )
    {
        foreach ( Transform n in numberContainer )
        {
            n.gameObject.SetActive( false );
        }
    }

    #endregion
}
