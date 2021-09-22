using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keckleon : Function
{
    private static readonly int s_Color = Shader.PropertyToID( "_Color" );

    [SerializeField]
    private List < Color > m_ColorsList;

    [SerializeField]
    private Slider m_ColorSlider;

    [SerializeField]
    private Material m_ColorMaterial;

    public override void OnCreate()
    {
        m_ColorSlider.onValueChanged.AddListener( ( color ) => ChangeColor( ( int ) color ) );
    }

    private void ChangeColor( int color )
    {
        if ( color >= 0 && color < m_ColorsList.Count )
        {
            m_ColorMaterial.SetColor( s_Color, m_ColorsList[color] );
            Poketch.Instance.PlayButton();
        }
    }
}
