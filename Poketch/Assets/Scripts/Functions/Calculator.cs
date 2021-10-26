using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Calculator : Function
{
    //private const long MaxValue = 9999999999;
    //private const long MinValue = -999999999;
    //private const int CharNone = 10;
    //private const int CharInterrogant = 11;
    private const int MaxDigits = 10;

    [SerializeField]
    private Transform m_ButtonsTransform;

    [SerializeField]
    private Transform m_NumbersTransform;

    [SerializeField]
    private Transform m_OperationsTransform;

    //Debug
    [SerializeField]
    private string m_Result;

    [SerializeField]
    private string m_Auxiliar;

    [SerializeField]
    private char m_CurrentOperation;

    [SerializeField]
    private Stage m_Stage = Stage.AddToResult;

    private readonly Dictionary < char, int > m_NumberIndex = new Dictionary < char, int >()
    {
        { '0', 0 },
        { '1', 1 },
        { '2', 2 },
        { '3', 3 },
        { '4', 4 },
        { '5', 5 },
        { '6', 6 },
        { '7', 7 },
        { '8', 8 },
        { '9', 9 },
        { 'n', 10 },
        { '?', 11 },
        { '.', 12 },
        { '-', 13 },
    };

    private readonly Dictionary < char, int > m_OperationIndex = new Dictionary < char, int >()
    {
        { '+', 0 }, { '-', 1 }, { '*', 2 }, { '/', 3 },
    };

    private void Start()
    {
        Clear();

        foreach ( Button b in m_ButtonsTransform.GetComponentsInChildren < Button >() )
        {
            b.onClick.AddListener( () => SetKey( b.name[0] ) );
        }
    }

    private void SetKey( char key )
    {
        switch ( key )
        {
            case 'c':
                Clear();
                Display( m_Result );

                break;

            case '+':
            case '-':
            case '*':
            case '/':
                if ( m_Stage == Stage.InvalidResult )
                {
                    break;
                }

                if ( m_Stage == Stage.AddToResult )
                {
                    if ( m_Result != "" )
                    {
                        m_Stage = Stage.AddToAuxiliar;
                        m_Auxiliar = "";
                    }
                }
                else if ( m_Stage == Stage.AddToAuxiliar && m_Auxiliar == "" )
                {

                }
                else if ( m_Stage == Stage.ShowResult )
                {
                    m_Stage = Stage.AddToAuxiliar;
                    m_Auxiliar = "";
                }
                else
                {
                    Calculate();
                    Display( m_Result );
                    m_Stage = Stage.AddToAuxiliar;
                    m_Auxiliar = "";
                }

                foreach ( Transform t in m_OperationsTransform )
                {
                    t.gameObject.SetActive( false );
                }

                m_OperationsTransform.GetChild( m_OperationIndex[key] ).gameObject.SetActive( true );

                m_CurrentOperation = key;

                break;

            case '=':
                if (m_Stage == Stage.InvalidResult)
                {
                    break;
                }

                if ( m_CurrentOperation != '\0' )
                {
                    Calculate();
                    Display( m_Result );

                    foreach ( Transform t in m_OperationsTransform )
                    {
                        t.gameObject.SetActive( false );
                    }
                }

                break;

            default:
                if ( m_Stage == Stage.ShowResult || m_Stage == Stage.InvalidResult )
                {
                    m_Result = "0";
                    m_Stage = Stage.AddToResult;
                }

                if ( m_Stage == Stage.AddToResult )
                {
                    m_Auxiliar = "";
                    AddChar( ref m_Result, key );
                }
                else if ( m_Stage == Stage.AddToAuxiliar )
                {
                    AddChar( ref m_Auxiliar, key );
                }

                break;
        }
    }

    private void AddChar( ref string result, char key )
    {
        if ( result.Length >= MaxDigits )
        {
            return;
        }

        if ( result == "0" && key != '.' )
        {
            result = "";
        }

        if ( result.Contains( "." ) && key == '.' )
        {
            return;
        }

        result += key;

        Display( result );
    }

    private void Calculate()
    {
        double result = double.Parse( m_Result, CultureInfo.InvariantCulture );
        double auxiliar = double.Parse( m_Auxiliar, CultureInfo.InvariantCulture );

        switch ( m_CurrentOperation )
        {
            case '+':
                m_Result = ( result + auxiliar ).ToString( CultureInfo.InvariantCulture );

                break;

            case '-':
                m_Result = ( result - auxiliar ).ToString( CultureInfo.InvariantCulture );

                break;

            case '*':
                m_Result = ( result * auxiliar ).ToString( CultureInfo.InvariantCulture );

                break;

            case '/':
                if ( result == 0d && auxiliar == 0d )
                {
                    Debug.LogWarning( "Cannot divide 0/0" );
                    m_Result = "aaaaaaaaaaaaaaaaaaa";
                }
                else
                {
                    m_Result = ( result / auxiliar ).ToString( CultureInfo.InvariantCulture );
                }

                break;
        }

        if ( !m_Result.Contains( "." ) )
        {
            if ( m_Result.Length > MaxDigits )
            {
                m_Result = "??????????";
                m_Stage = Stage.InvalidResult;
                m_CurrentOperation = '\0';

                return;
            }
        }
        else
        {
        }

        m_Stage = Stage.ShowResult;
    }

    private void Display( string result )
    {
        foreach ( Transform t in m_NumbersTransform )
        {
            foreach ( Transform number in t )
            {
                number.gameObject.SetActive( false );
            }

            t.GetChild( 10 ).gameObject.SetActive( true );
        }

        int n = m_NumbersTransform.childCount - 1;

        foreach ( char c in result.Reverse() )
        {
            Transform number = m_NumbersTransform.GetChild( n-- );
            number.GetChild( 10 ).gameObject.SetActive( false );
            number.GetChild( m_NumberIndex[c] ).gameObject.SetActive( true );
        }
    }

    private void Clear()
    {
        m_Result = "0";
        m_Auxiliar = "";

        m_Stage = 0;

        foreach ( Transform t in m_OperationsTransform )
        {
            t.gameObject.SetActive( false );
        }
    }

    private enum Stage
    {
        AddToResult,
        AddToAuxiliar,
        InvalidResult,
        ShowResult
    }
}
