using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DotTexture
{
    private Vector2Int m_Size = new Vector2Int( 1, 1 );

    public int Area => m_Size.x * m_Size.y;

    private Texture2D m_Texture;

    public Texture2D CreateTexture(int x, int y)
    {
        m_Size.Set( x, y );

        m_Texture = new Texture2D( m_Size.x, m_Size.y ) { filterMode = FilterMode.Point };

        return m_Texture;
    }

    public void Reset(Color32 color)
    {
        Color32[] newColor = new Color32[Area];
        
        for ( int i = 0; i < newColor.Length; ++i )
        {
            newColor[i] = color;
        }

        m_Texture.SetPixels32(newColor);
        m_Texture.Apply();
    }

    public void Paint( int pixelX, int pixelY, Color color )
    {
        m_Texture.SetPixel( pixelX, pixelY, color );
        m_Texture.Apply();
    }
}
