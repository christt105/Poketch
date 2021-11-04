using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NumberController : MonoBehaviour
{
    [SerializeField]
    private bool m_HideZero = false;

    [SerializeField]
    private bool m_InitializeToZero = false;

    private bool m_Init = false;
    private int m_MaxNumberLength = 0;
    private int[] m_Numbers;
    private Digit[] m_NumbersTransforms;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        m_MaxNumberLength = transform.childCount;
        m_NumbersTransforms = new Digit[m_MaxNumberLength];
        m_Numbers = new int[m_MaxNumberLength];

        for ( int i = 0; i < m_MaxNumberLength; ++i )
        {
            m_NumbersTransforms[i] = new Digit( transform.GetChild( i ) );
            m_Numbers[i] = 0;
        }

        if ( m_InitializeToZero )
        {
            foreach ( Digit digit in m_NumbersTransforms )
            {
                foreach ( Image d in digit.Digits )
                {
                    d.enabled = false;
                }

                digit.Digits[0].enabled = !m_HideZero;
            }
        }

        m_Init = true;
    }

    public void SetNumber( int number )
    {
        if ( !m_Init )
        {
            Init();
        }

        if ( GetNumberDigits( number ) > m_MaxNumberLength )
        {
            Debug.LogError(
                "Tried to set " + number + " on a " + m_MaxNumberLength + " length number container" );

            return;
        }

        int[] newNumber = GetIntArray( number, m_MaxNumberLength );
        int lengthNewNnumber = newNumber.Length;

        bool foundZero = false;

        for ( int i = 0; i < m_MaxNumberLength; ++i )
        {
            if ( i > lengthNewNnumber && m_Numbers[i] != 0 )
            {
                m_NumbersTransforms[i].Digits[m_Numbers[i]].enabled = false;
                m_NumbersTransforms[i].Digits[0].enabled = !m_HideZero || foundZero;

                m_Numbers[i] = newNumber[i];
            }
            else if ( newNumber[i] != m_Numbers[i] )
            {
                m_NumbersTransforms[i].Digits[m_Numbers[i]].enabled = false;
                m_NumbersTransforms[i].Digits[newNumber[i]].enabled = newNumber[i] != 0 || (!m_HideZero || foundZero);
                m_Numbers[i] = newNumber[i];
            }
            else if(newNumber[i] == 0)
            {
                m_NumbersTransforms[i].Digits[m_Numbers[i]].enabled = !m_HideZero || foundZero;
            }

            if ( m_Numbers[i] != 0 )
            {
                foundZero = true;
            }
        }
    }

    private static int[] GetIntArray( int num, int maxNumber )
    {
        int[] listOfInts = new int[maxNumber];

        if ( num == 0 )
        {
            return listOfInts;
        }

        int n = 0;

        while ( num > 0 )
        {
            if ( n >= listOfInts.Length )
            {
                Debug.LogError( "number (" + num + ") is bigger than the max number allowed (" + maxNumber + ")" );

                break;
            }

            listOfInts[n++] = num % 10;
            num /= 10;
        }

        return listOfInts.Reverse().ToArray();
    }

    /// <summary>
    ///     Returns the number digits length
    /// </summary>
    /// <param name="n">number</param>
    /// <returns></returns>
    public static int GetNumberDigits( int n )
    {
        return n == 0 ? 1 : ( n > 0 ? 1 : 2 ) + ( int ) Math.Log10( Math.Abs( ( double ) n ) );
    }

    private readonly struct Digit
    {
        public Digit( Component t )
        {
            Digits = t.GetComponentsInChildren < Image >();
        }

        public Image[] Digits { get; }
    }
}
