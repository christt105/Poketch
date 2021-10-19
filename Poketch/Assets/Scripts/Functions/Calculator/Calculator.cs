using System;
using System.Globalization;
using System.Linq;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

public class Calculator : Function
{
    private const long MaxValue = 9999999999;
    private const long MinValue = -999999999;
    private const int CharNone = 10;
    private const int CharInterrogant = 11;

    [SerializeField]
    private Transform m_ButtonsTransform;

    [SerializeField]
    private Transform m_NumbersTransform;

    [SerializeField]
    private Transform m_OperationsTransform;

    private ActualNumber m_ActualNumber = ActualNumber.Result;
    private double m_AuxiliarNumber = 0;

    private Action m_CurrentAction = Action.None;
    private KeyAction m_LastKeyAction = KeyAction.None;

    private int m_NumberOfDigits;

    private int m_NumberOfNumbers;

    private double m_Result;

    private enum ActualNumber
    {
        Result, Auxiliar,
        Invalid
    }

    #region Functions

    public override void OnCreate( JSONNode jsonObject )
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
        SetAllNumbersTo( CharNone );
    }

    #endregion

    #region Private

    private enum KeyAction
    {
        Sum, Sub, Mul, Div, Dot, Equal, Clear, None
    }

    private enum Action
    {
        Sum, Sub, Mul, Div, None
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
        m_NumberOfDigits = 0;
        m_ActualNumber = ActualNumber.Result;
        SetOperation( Action.None );
    }

    private void OnClickNumber( int number )
    {
        SoundManager.Instance.PlaySFX( SoundManager.SFX.Button );

        if ( m_NumberOfDigits >= 10 )
        {
            return;
        }

        ++m_NumberOfDigits;

        if ( m_LastKeyAction == KeyAction.Equal || m_ActualNumber == ActualNumber.Invalid )
        {
            m_Result = 0;
            m_LastKeyAction = KeyAction.None;
            m_ActualNumber = ActualNumber.Result;
        }

        if ( m_ActualNumber == ActualNumber.Result )
        {
            m_Result = m_Result * 10 + number;
        }
        else
        {
            m_AuxiliarNumber = m_AuxiliarNumber * 10 + number;
        }

        ShowResult();
    }

    private void ShowResult()
    {
        ShowResult( m_ActualNumber );
    }

    private void ShowResult( ActualNumber actualNumber )
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

        foreach ( char c in ( actualNumber == ActualNumber.Result ? m_Result : m_AuxiliarNumber ).
                            ToString( CultureInfo.InvariantCulture ).
                            Reverse() )
        {
            m_NumbersTransform.GetChild( n ).GetChild( 10 ).gameObject.SetActive( false );
            m_NumbersTransform.GetChild( n-- ).GetChild( c == '-' ? 13 : c - '0' ).gameObject.SetActive( true );
        }
    }

    private void OnClickAction( KeyAction keyAction )
    {
        SoundManager.Instance.PlaySFX( SoundManager.SFX.Button );

        switch ( keyAction )
        {
            case KeyAction.Clear:
                Reset();
                ShowResult();

                break;

            case KeyAction.Sum:
            case KeyAction.Sub:
            case KeyAction.Mul:
            case KeyAction.Div:
                if ( m_ActualNumber == ActualNumber.Invalid )
                {
                    break;
                }

                if ( m_ActualNumber == ActualNumber.Result )
                {
                    m_ActualNumber = ActualNumber.Auxiliar;
                    m_AuxiliarNumber = 0;
                    m_NumberOfDigits = 0;
                }
                else
                {
                    m_Result = Calculate( m_Result, m_AuxiliarNumber, m_CurrentAction );

                    if ( m_Result > MaxValue || m_Result < MinValue )
                    {
                        m_Result = 0;
                        m_AuxiliarNumber = 0;
                        m_ActualNumber = ActualNumber.Invalid;

                        SetAllNumbersTo( CharInterrogant );

                        break;
                    }

                    ShowResult( ActualNumber.Result );

                    m_AuxiliarNumber = 0;
                }

                m_CurrentAction = ( Action ) keyAction;

                SetOperation( m_CurrentAction );

                break;

            case KeyAction.Dot:

                break;

            case KeyAction.Equal:

                if ( m_ActualNumber == ActualNumber.Invalid )
                {
                    break;
                }

                m_Result = Calculate( m_Result, m_AuxiliarNumber, m_CurrentAction );

                SetOperation( Action.None );
                m_CurrentAction = Action.None;

                if ( m_Result > MaxValue || m_Result < MinValue )
                {
                    m_Result = 0;
                    m_AuxiliarNumber = 0;
                    m_ActualNumber = ActualNumber.Invalid;

                    SetAllNumbersTo( CharInterrogant );

                    break;
                }

                m_ActualNumber = ActualNumber.Result;
                m_NumberOfDigits = 0;

                ShowResult();

                break;

            default:
                throw new ArgumentOutOfRangeException( nameof( keyAction ), keyAction, null );
        }

        m_LastKeyAction = keyAction;
    }

    private void SetOperation( Action action )
    {
        foreach ( Transform t in m_OperationsTransform )
        {
            t.gameObject.SetActive( false );
        }

        if ( action == Action.None )
        {
            return;
        }

        m_OperationsTransform.GetChild( ( int ) action ).gameObject.SetActive( true );
    }

    private static double Calculate( double result, double auxiliarNumber, Action keyAction )
    {
        switch ( keyAction )
        {
            case Action.Sum:
                return result + auxiliarNumber;

            case Action.Sub:
                return result - auxiliarNumber;

            case Action.Mul:
                return result * auxiliarNumber;

            case Action.Div:
                if ( result == 0 && auxiliarNumber == 0 )
                {
                    return long.MaxValue;
                }

                return result / auxiliarNumber;

            default:
                throw new ArgumentOutOfRangeException( nameof( keyAction ), keyAction, null );
        }
    }

    private static KeyAction GetActionFromString( string action )
    {
        switch ( action )
        {
            case "C":
                return KeyAction.Clear;

            case "+":
                return KeyAction.Sum;

            case "-":
                return KeyAction.Sub;

            case "x":
                return KeyAction.Mul;

            case "/":
                return KeyAction.Div;

            case ".":
                return KeyAction.Dot;

            case "=":
                return KeyAction.Equal;
        }

        Debug.LogError( "No enum action for " + action );

        return KeyAction.Clear;
    }

    #endregion
}
