using System;
using System.Collections;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

public class DotArtist : Function
{
    private const string InitialPaint =
        "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAwAAAwMDAAMDAwADAwMAAwADAAMAAAAAAwAAAwADAAMAAwADAAAAAwADAAAAAAAAAwAAAwMDAAMAAwADAwMAAwMDAAMAAAAAAwAAAAAAAAAAAAAAAAAAAwAAAAMAAAAAAwAAAAAAAAAAAAAAAAAAAAAAAAMAAAMDAwMDAAAAAAAAAAAAAAAAAAAAAAMAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";
    [SerializeField]
    private Vector2Int m_TextureSize;

    [SerializeField]
    private RawImage m_RawImage;

    [SerializeField]
    private EventPointRectTransform m_PointerZone;

    [SerializeField]
    private Color[] m_Colors = new Color[4];

    private DotTexture m_DotTexture;

    private bool m_Painting = false;

    private byte[] m_Values;

    private void Reset()
    {
        m_DotTexture.Reset( Color.white );

        for ( int i = 0; i < m_Values.Length; ++i )
        {
            m_Values[i] = 0;
        }
    }

    private void OnEnable()
    {
        m_PointerZone.OnDown += StartPainting;
        m_PointerZone.OnUp += StopPainting;
    }

    private void OnDisable()
    {
        m_PointerZone.OnDown -= StartPainting;
        m_PointerZone.OnUp -= StopPainting;
    }

    private void StopPainting()
    {
        m_Painting = false;

        JSONNode json = new JSONObject();
        json["data"] = Convert.ToBase64String(m_Values);

        FunctionController.Instance.SaveFunctionInfo( GetType().Name, json );
    }

    public override void OnCreate( JSONNode jsonObject )
    {
        m_DotTexture = new DotTexture();
        m_RawImage.texture = m_DotTexture.CreateTexture( m_TextureSize.x, m_TextureSize.y );
        m_Values = new byte [m_DotTexture.Area];

        Reset();

        if ( jsonObject != null )
        {
            m_Values = Convert.FromBase64String(jsonObject["data"]);

            for ( int i = 0; i < m_Values.Length; i++ )
            {
                Vector2Int coord = GetCoordsFromIndex( i, m_TextureSize.x );
                m_DotTexture.Paint( coord.x, coord.y, m_Colors[m_Values[i]] );
            }
        }
        else
        {
            m_Values = Convert.FromBase64String( InitialPaint );

            for (int i = 0; i < m_Values.Length; i++)
            {
                Vector2Int coord = GetCoordsFromIndex(i, m_TextureSize.x);
                m_DotTexture.Paint(coord.x, coord.y, m_Colors[m_Values[i]]);
            }
        }
    }

    public override void OnAuxButton()
    {
        Reset();
    }

    private void StartPainting()
    {
        m_Painting = true;
        StartCoroutine( Paint() );
    }

    private IEnumerator Paint()
    {
        Vector2Int lastPainted = Vector2Int.one * -1;

        while ( m_Painting )
        {
            Vector2Int pixel = Vector2Int.FloorToInt( m_PointerZone.GetRelativeMousePosition() * m_TextureSize );

            if ( CheckBoundaries( pixel ) && pixel != lastPainted )
            {
                m_DotTexture.Paint(
                    pixel.x,
                    pixel.y,
                    m_Colors[GetNextValue( pixel.x, pixel.y )] );

                lastPainted = pixel;
            }

            yield return null;
        }
    }

    private bool CheckBoundaries( Vector2Int pixel )
    {
        return pixel.x >= 0 && pixel.x < m_TextureSize.x && pixel.y >= 0 && pixel.y < m_TextureSize.y;
    }

    private int GetNextValue( int x, int y )
    {
        int index = GetIndexOf2DArray( x, y, m_TextureSize.x );

        if ( ++m_Values[index] > 3 )
        {
            m_Values[index] = 0;
        }

        return m_Values[index];
    }

    private static int GetIndexOf2DArray( int x, int y, int width )
    {
        return y * width + x;
    }

    private static Vector2Int GetCoordsFromIndex( int index, int width )
    {
        return new Vector2Int( index % width, index / width );
    }
}
