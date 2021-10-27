using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

public class Counter : Function
{
    [SerializeField]
    private Button m_CountButton;

    [SerializeField]
    private NumberController m_NumberUI;

    private int m_Count = 0;

    #region Private

    private void Add()
    {
        if ( ++m_Count > 9999 )
        {
            m_Count = 0;
        }

        m_NumberUI.SetNumber( m_Count );

        SoundManager.Instance.PlaySFX( SoundManager.SFX.ResetCounter );
    }

    #endregion

    #region Function

    public override void OnCreate( JSONNode jsonObject )
    {
        m_CountButton.onClick.AddListener( Add );
    }

    public override void OnChange()
    {
        m_Count = 0;
        m_NumberUI.SetNumber( 0 );
    }

    #endregion
}
