using System;
using System.Collections;
using System.Xml.Serialization;
using UnityEngine;

public class Clock : Function
{
    [SerializeField]
    private Transform m_Digits;

    #region Functions

    public override void OnStart()
    {
        StartCoroutine(UpdateClock());
    }

    public override void OnChange()
    {
        StartCoroutine( UpdateClock() );
    }

    #endregion

    #region Private

    private IEnumerator UpdateClock()
    {
        while ( true )
        {
            int hours = DateTime.Now.Hour;
            int minutes = DateTime.Now.Minute;

            foreach ( Transform digit in m_Digits )
            {
                foreach ( Transform d in digit )
                {
                    d.gameObject.SetActive( false );
                }
            }

            m_Digits.GetChild( 0 ).GetChild( hours / 10 ).gameObject.SetActive( true );
            m_Digits.GetChild( 1 ).GetChild( hours % 10 ).gameObject.SetActive( true );
            m_Digits.GetChild( 2 ).GetChild( minutes / 10 ).gameObject.SetActive( true );
            m_Digits.GetChild( 3 ).GetChild( minutes % 10 ).gameObject.SetActive( true );

            yield return new WaitForSeconds( 0.5f );
        }
    }

    #endregion
}
