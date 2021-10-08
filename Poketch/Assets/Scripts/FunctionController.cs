using System.IO;
using SimpleJSON;
using UnityEngine;

public class FunctionController : MonoBehaviour
{
    private int m_FunctionIndex = -1;

    private Function[] m_Functions;

    private Transform m_MyTransform;

    private static string DataPath => Application.persistentDataPath + "/data.json";

    public static FunctionController Instance { get; private set; }

    public void SaveFunctionInfo( string functionName, JSONNode json )
    {
        JSONNode file = File.Exists( DataPath ) ? JSON.Parse( File.ReadAllText( DataPath ) ) : new JSONObject();

        file["Functions"][functionName] = json;

        File.WriteAllText( DataPath, file.ToString() );
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

        m_Functions = m_MyTransform.GetComponentsInChildren < Function >( true );
        JSONNode file = File.Exists( DataPath ) ? JSON.Parse( File.ReadAllText( DataPath ) ) : null;

#if !UNITY_EDITOR
        m_FunctionIndex = file != null ? file.GetValueOrDefault( "Actual", -1 ).AsInt : -1;
#endif

        file = file?["Functions"];

        foreach ( Function f in m_Functions )
        {
            f.OnCreate( file?[f.GetType().Name]?.AsObject );

            if ( !f.gameObject.activeSelf )
            {
                continue;
            }

            if ( m_FunctionIndex == -1 )
            {
                m_FunctionIndex = f.transform.GetSiblingIndex();
            }

            f.gameObject.SetActive( false );
        }

        if ( m_FunctionIndex == -1 )
        {
            m_FunctionIndex = 0;
        }

        m_Functions[m_FunctionIndex].gameObject.SetActive( true );
        m_Functions[m_FunctionIndex].OnChange();
    }

    #endregion

    #region Public

    public void Next()
    {
        m_Functions[m_FunctionIndex].gameObject.SetActive( false );
        m_Functions[m_FunctionIndex].OnExit();

        if ( ++m_FunctionIndex > m_Functions.Length - 1 )
        {
            m_FunctionIndex = 0;
        }

        SaveActualFunction( m_FunctionIndex );

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
        m_Functions[m_FunctionIndex].gameObject.SetActive( true );
        m_Functions[m_FunctionIndex].OnChange();
    }

    #endregion
}
