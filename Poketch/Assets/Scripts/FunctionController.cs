using System;
using System.IO;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

public class FunctionController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_AuxButtonGO;

    [SerializeField]
    private Function[] m_Functions;

    [SerializeField]
    private GameObject m_FunctionTextGameObject;

    [SerializeField]
    private float m_TimeToFastChange = 0.5f;

    private int m_FunctionIndex = -1;

    private Text m_FunctionText;

    private Transform m_MyTransform;

    public float FastTimeChange => m_TimeToFastChange;

    private static string DataPath => Application.persistentDataPath + "/data.json";

    public static FunctionController Instance { get; private set; }

    public void SaveFunctionInfo( string functionName, JSONNode json )
    {
        JSONNode file = File.Exists( DataPath ) ? JSON.Parse( File.ReadAllText( DataPath ) ) : new JSONObject();

        file["Functions"][functionName] = json;

        File.WriteAllText( DataPath, file.ToString( 1 ) );
    }

    private void SaveActualFunction( int index )
    {
        JSONNode file = File.Exists( DataPath ) ? JSON.Parse( File.ReadAllText( DataPath ) ) : new JSONObject();

        file["Actual"] = index;

        File.WriteAllText( DataPath, file.ToString() );
    }

    public void SetFunctionText( bool previous )
    {
        if ( !m_FunctionTextGameObject.activeSelf )
        {
            m_FunctionTextGameObject.SetActive( true );
        }

        int index = m_FunctionIndex + ( previous ? -1 : 1 );

        if ( index < 0 )
        {
            index = m_Functions.Length - 1;
        }
        else if ( index >= m_Functions.Length )
        {
            index = 0;
        }

        m_FunctionText.text = ( index + 1 ).ToString( "00" ) + " - " + m_Functions[index].GetType().Name;
    }

    public void HideFunctionText()
    {
        m_FunctionTextGameObject.SetActive( false );
    }

    #region Unity Event Functions

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        m_MyTransform = transform;
        m_FunctionText = m_FunctionTextGameObject.GetComponent < Text >();

        JSONNode file = File.Exists( DataPath ) ? JSON.Parse( File.ReadAllText( DataPath ) ) : null;

#if !UNITY_EDITOR
        m_FunctionIndex = file != null ? file.GetValueOrDefault( "Actual", -1 ).AsInt : -1;
        if ( m_FunctionIndex < 0 || m_FunctionIndex >= m_Functions.Length )
        {
            m_FunctionIndex = 0;
        }
#endif

        file = file?.GetValueOrDefault( "Functions", null );

        foreach ( Function f in m_Functions )
        {
            f.OnCreate( file?[f.GetType().Name] );

            if ( !f.gameObject.activeSelf )
            {
                continue;
            }

            if ( m_FunctionIndex == -1 )
            {
                m_FunctionIndex = Array.IndexOf( m_Functions, f );
            }

            f.gameObject.SetActive( false );
        }

        if ( m_FunctionIndex == -1 )
        {
            m_FunctionIndex = 0;
        }

        if ( m_FunctionIndex >= m_Functions.Length )
        {
            m_FunctionIndex = m_Functions.Length - 1;
        }

        m_Functions[m_FunctionIndex].gameObject.SetActive( true );
        m_Functions[m_FunctionIndex].OnChange();
        m_AuxButtonGO.SetActive( m_Functions[m_FunctionIndex].UseAuxButton );
    }

    #endregion

    #region Public

    public void OnAuxButton()
    {
        m_Functions[m_FunctionIndex].OnAuxButton();
    }

    public void Next()
    {
        m_Functions[m_FunctionIndex].gameObject.SetActive( false );
        m_Functions[m_FunctionIndex].OnExit();

        if ( ++m_FunctionIndex > m_Functions.Length - 1 )
        {
            m_FunctionIndex = 0;
        }

        SaveActualFunction( m_FunctionIndex );
        m_AuxButtonGO.SetActive( m_Functions[m_FunctionIndex].UseAuxButton );

        m_Functions[m_FunctionIndex].gameObject.SetActive( true );
        m_Functions[m_FunctionIndex].OnChange();
    }

    public void Previous()
    {
        m_Functions[m_FunctionIndex].gameObject.SetActive( false );
        m_Functions[m_FunctionIndex].OnExit();

        if ( --m_FunctionIndex < 0 )
        {
            m_FunctionIndex = m_Functions.Length - 1;
        }

        SaveActualFunction( m_FunctionIndex );
        m_AuxButtonGO.SetActive( m_Functions[m_FunctionIndex].UseAuxButton );

        m_Functions[m_FunctionIndex].gameObject.SetActive( true );
        m_Functions[m_FunctionIndex].OnChange();
    }

    #endregion
}
