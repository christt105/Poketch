using System;
using System.Linq;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

public class Calculator : Function
{
    [SerializeField]
    private Transform m_ButtonsTransform;

    [SerializeField]
    private Transform m_NumbersTransform;

    [SerializeField]
    private Transform m_OperationsTransform;

    private Action m_Action = Action.None;
    private long m_AuxiliarNumber = 0;

    private int m_NumberOfDigits;

    private int m_NumberOfNumbers;

    private long m_Result;

    #region Functions

    public override void OnCreate( JSONObject jsonObject )
    {
        foreach ( Transform t in m_ButtonsTransform )
        {
            Button b = t.GetComponent < Button >();

            if ( int.TryParse( t.name, out int number ) )
            {
                b.onClick.AddListener( () => OnClickNumber( number ) );
            }
            else
            {
                b.onClick.AddListener( () => OnClickAction( GetActionFromString( t.name ) ) );
            }
        }

        m_NumberOfNumbers = m_NumbersTransform.childCount;
    }

    public override void OnChange()
    {
        Reset();
        SetAllNumbersTo( 10 );
    }

    #endregion

    #region Private

    private enum Action
    {
        Sum, Sub, Mul, Div, Dot, Equal, Clear, None
    }

    private void SetAllNumbersTo( int character )
    {
        foreach ( Transform t in m_NumbersTransform )
        {
            foreach ( Transform number in t )
            {
                number.gameObject.SetActive( false );
            }

            t.GetChild( character ).gameObject.SetActive( true );
        }
    }

    private void Reset()
    {
        m_Result = 0;
        m_AuxiliarNumber = 0;
        m_Action = Action.None;
        m_NumberOfDigits = 0;

        foreach ( Transform t in m_OperationsTransform )
        {
            t.gameObject.SetActive( false );
        }
    }

    private void OnClickNumber( int number )
    {
        SoundManager.Instance.PlaySFX( SoundManager.SFX.Button );

        if ( m_NumberOfDigits == 10 || m_Result == 0 && number == 0 )
        {
            return;
        }

        if ( m_Action == Action.Equal && m_Result != 0 )
        {
            m_Result = 0;
            m_Action = Action.None;
        }

        if ( m_Action == Action.None )
        {
            m_Result = m_Result * 10 + number;
        }
        else
        {
            m_AuxiliarNumber = m_AuxiliarNumber * 10 + number;
        }

        m_NumberOfDigits++;

        ShowResult();
    }

    private void ShowResult( bool result = false )
    {
        int n = m_NumberOfNumbers - 1;

        foreach ( Transform t in m_NumbersTransform )
        {
            foreach ( Transform number in t )
            {
                number.gameObject.SetActive( false );
            }

            t.GetChild( 10 ).gameObject.SetActive( true );
        }

        foreach ( char c in result || m_Action == Action.None
            ? m_Result.ToString().Reverse()
            : m_AuxiliarNumber.ToString().Reverse() )
        {
            m_NumbersTransform.GetChild( n ).GetChild( 10 ).gameObject.SetActive( false );
            m_NumbersTransform.GetChild( n-- ).GetChild( c - '0' ).gameObject.SetActive( true );
        }
    }

    private void OnClickAction( Action action )
    {
        SoundManager.Instance.PlaySFX( SoundManager.SFX.Button );

        switch ( action )
        {
            case Action.Clear:
                Reset();
                ShowResult();

                break;

            case Action.Sum:
            case Action.Sub:
            case Action.Mul:
            case Action.Div:
                if ( m_Action >= Action.Sum && m_Action <= Action.Div )
                {
                    m_Result = Calculate( m_Result, m_AuxiliarNumber, m_Action );
                    m_AuxiliarNumber = 0;
                    m_NumberOfDigits = 0;
                    ShowResult( true );
                }

                foreach ( Transform t in m_OperationsTransform )
                {
                    t.gameObject.SetActive( false );
                }

                m_OperationsTransform.GetChild( ( int ) action ).gameObject.SetActive( true );

                m_NumberOfDigits = 0;
                m_Action = action;

                break;

            case Action.Dot:
                break;

            case Action.Equal:
                m_Result = Calculate( m_Result, m_AuxiliarNumber, m_Action );

                m_AuxiliarNumber = 0;
                m_NumberOfDigits = 0;

                foreach ( Transform t in m_OperationsTransform )
                {
                    t.gameObject.SetActive( false );
                }

                if ( m_Result > 9999999999 )
                {
                    SetAllNumbersTo( 11 );
                    m_Result = 0;

                    return;
                }

                ShowResult(true);

                break;

            default:
                throw new ArgumentOutOfRangeException( nameof( action ), action, null );
        }
    }

    private static long Calculate( long result, long auxiliarNumber, Action action )
    {
        switch ( action )
        {
            case Action.Sum:
                return result + auxiliarNumber;

            case Action.Sub:
                return result - auxiliarNumber;

            case Action.Mul:
                return result * auxiliarNumber;

            case Action.Div:
                return result / auxiliarNumber;

            default:
                throw new ArgumentOutOfRangeException( nameof( action ), action, null );
        }
    }

    private static Action GetActionFromString( string action )
    {
        switch ( action )
        {
            case "C":
                return Action.Clear;

            case "+":
                return Action.Sum;

            case "-":
                return Action.Sub;

            case "x":
                return Action.Mul;

            case "/":
                return Action.Div;

            case ".":
                return Action.Dot;

            case "=":
                return Action.Equal;
        }

        Debug.LogError( "No enum action for " + action );

        return Action.None;
    }

    #endregion
}
