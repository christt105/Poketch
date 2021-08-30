using UnityEngine;

public class FunctionController : MonoBehaviour
{
    private int m_FunctionIndex = -1;

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

        foreach ( Transform t in m_MyTransform )
        {
            if ( !t.gameObject.activeSelf )
            {
                continue;
            }

            if ( m_FunctionIndex != -1 )
            {
                t.gameObject.SetActive( false );
            }
            else
            {
                m_FunctionIndex = t.GetSiblingIndex();
            }
        }
    }

    #endregion

    #region Public

    public void Next()
    {
        m_MyTransform.GetChild( m_FunctionIndex ).gameObject.SetActive( false );

        if ( ++m_FunctionIndex > m_MyTransform.childCount - 1 )
        {
            m_FunctionIndex = 0;
        }

        m_MyTransform.GetChild( m_FunctionIndex ).gameObject.SetActive( true );
    }

    public void Previous()
    {
        m_MyTransform.GetChild( m_FunctionIndex ).gameObject.SetActive( false );

        if ( --m_FunctionIndex < 0 )
        {
            m_FunctionIndex = m_MyTransform.childCount - 1;
        }

        m_MyTransform.GetChild( m_FunctionIndex ).gameObject.SetActive( true );
    }

    #endregion
}
