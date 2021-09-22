using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keckleon : Function
{
    [SerializeField]
    private List < Color > m_ColorsList;

    [SerializeField]
    private Slider m_ColorSlider;

    [SerializeField]
    private Material m_ColorMaterial;

    public override void OnCreate()
    {
        m_ColorSlider.onValueChanged.AddListener( ( float color ) => ChangeColor( ( int ) color ) );
    }

    private void ChangeColor( int color )
    {
        if ( color >= 0 && color < m_ColorsList.Count )
        {
            m_ColorMaterial.SetColor( "_Color", m_ColorsList[color] );
            Debug.Log( $"Changed to {color}" );
        }
    }
}
