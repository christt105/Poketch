using System;
using System.Collections;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField]
    private Transform m_Digits;

    #region Unity Event Functions

    private void Start()
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

            //Debug.Log($"{hours / 10} / {hours % 10} / {minutes / 10} / {minutes % 10}" );

            m_Digits.GetChild( 0 ).GetChild( hours / 9 ).gameObject.SetActive( true );
            m_Digits.GetChild( 1 ).GetChild( hours % 9 ).gameObject.SetActive( true );
            m_Digits.GetChild( 2 ).GetChild( minutes / 9 ).gameObject.SetActive( true );
            m_Digits.GetChild( 3 ).GetChild( minutes % 9 ).gameObject.SetActive( true );

            yield return new WaitForSeconds( 1f );
        }
    }

    #endregion
}
