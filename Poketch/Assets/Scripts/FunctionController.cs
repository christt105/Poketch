using System;
using System.IO;
using System.Linq;
using SimpleJSON;
using UnityEngine;

public class FunctionController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_AuxButtonGO;

    [SerializeField]
    private Function[] m_Functions;

    private int m_FunctionIndex = -1;

    private Transform m_MyTransform;

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

    #region Unity Event Functions

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        m_MyTransform = transform;

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
                m_FunctionIndex = Array.IndexOf(m_Functions, f);
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
