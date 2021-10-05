using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberController : MonoBehaviour
{
    private int m_MaxNumberLength = 0;
    private int[] m_Numbers;
    private Digit[] m_NumbersTransforms;

    private void Start()
    {
        m_MaxNumberLength = transform.childCount;
        m_NumbersTransforms = new Digit[m_MaxNumberLength];
        m_Numbers = new int[m_MaxNumberLength];

        for ( int i = 0; i < m_MaxNumberLength; ++i )
        {
            m_NumbersTransforms[i] = new Digit( transform.GetChild( i ) );
            m_Numbers[i] = 0;
        }
    }

    public void SetNumber( int number )
    {
        if ( GetNumberDigits( number ) > m_MaxNumberLength )
        {
            Debug.LogError(
                "Tried to set " + number + " on a " + m_MaxNumberLength + " length number container" );

            return;
        }

        int[] newNumber = GetIntArray( number );
        int lengthNewNnumber = newNumber.Length;

        for ( int i = 0; i < m_MaxNumberLength; ++i )
        {
            if ( i > lengthNewNnumber && m_Numbers[i] != 0 )
            {
                m_NumbersTransforms[i].Digits[m_Numbers[i]].enabled = false;
                m_NumbersTransforms[i].Digits[0].enabled = true;
                m_Numbers[i] = newNumber[i];
            }
            else if ( newNumber[i] != m_Numbers[i] )
            {
                m_NumbersTransforms[i].Digits[m_Numbers[i]].enabled = false;
                m_NumbersTransforms[i].Digits[newNumber[i]].enabled = true;
                m_Numbers[i] = newNumber[i];
            }
        }
    }

    private static int[] GetIntArray( int num )
    {
        if ( num == 0 )
        {
            return new int[] { 0 };
        }

        List < int > listOfInts = new List < int >();

        while ( num > 0 )
        {
            listOfInts.Add( num % 10 );
            num /= 10;
        }

        listOfInts.Reverse();

        return listOfInts.ToArray();
    }

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
