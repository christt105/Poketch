using System.Collections.Generic;
using SimpleJSON;
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

    public override void OnCreate( JSONObject jsonObject )
    {
        m_ColorSlider.onValueChanged.AddListener( ( color ) => ChangeColor( ( int ) color ) );

        if ( jsonObject != null )
        {
            int color = jsonObject.GetValueOrDefault("color", 0);
            SetColor( color );
            m_ColorSlider.SetValueWithoutNotify( color );
        }
        else
        {
            SetColor( 0 );
        }
    }

    private void ChangeColor( int color )
    {
        if ( color >= 0 && color < m_ColorsList.Count )
        {
            SetColor( color );

            SoundManager.Instance.PlaySFX( SoundManager.SFX.Button );

            JSONNode json = new JSONObject();
            json.Add( "color", color );
            FunctionController.Instance.SaveFunctionInfo( GetType().Name, json );
        }
    }

    private void SetColor( int color )
    {
        m_ColorMaterial.SetColor( s_Color, m_ColorsList[color] );
    }
}
