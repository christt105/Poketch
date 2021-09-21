using System.Collections.Generic;
using UnityEngine;

public class FunctionController : MonoBehaviour
{
    private int m_FunctionIndex = -1;

    private Function[] m_Functions;

    private Transform m_MyTransform;

    public static FunctionController Instance { get; private set; }

    #region Unity Event Functions

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        m_MyTransform = transform;

        m_Functions = m_MyTransform.GetComponentsInChildren < Function >(true);

        foreach ( Function f in m_Functions )
        {
            //TODO: Load from json

            f.OnCreate();

            if ( !f.gameObject.activeSelf )
            {
                continue;
            }

            if ( m_FunctionIndex != -1 )
            {
                f.gameObject.SetActive( false );
            }
            else
            {
                m_FunctionIndex = f.transform.GetSiblingIndex();
                f.OnStart();
            }
        }

        if ( m_FunctionIndex == -1 )
        {
            m_FunctionIndex = 0;
            m_Functions[m_FunctionIndex].gameObject.SetActive( true );
        }
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

        m_Functions[m_FunctionIndex].gameObject.SetActive(true);
        m_Functions[m_FunctionIndex].OnChange();
    }

    public void Previous()
    {
        m_Functions[m_FunctionIndex].gameObject.SetActive(false);
        m_Functions[m_FunctionIndex].OnExit();

        if ( --m_FunctionIndex < 0 )
        {
            m_FunctionIndex = m_Functions.Length - 1;
        }

        m_Functions[m_FunctionIndex].gameObject.SetActive(true);
        m_Functions[m_FunctionIndex].OnChange();
    }

    #endregion
}
