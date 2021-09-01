using UnityEngine;
using UnityEngine.UI;

public class Calculator : Function
{
    [SerializeField]
    private Transform m_ButtonsTransform;

    #region Unity Event Functions

    private void Start()
    {
        foreach ( Transform t in m_ButtonsTransform )

        {
            Button b = t.GetComponent < Button >();

            if ( int.TryParse( t.name, out int number ) )
            {
                b.onClick.AddListener( () => OnClickNumber( number ) );
            }
            else
            {
                b.onClick.AddListener( () => OnClickAction( t.name ) );
            }
        }
    }

    #endregion

    private void OnClickNumber( int number )
    {
        Poketch.Instance.PlayButton();
        Debug.Log( "Pressed " + number );
    }

    private void OnClickAction( string action )
    {
        Poketch.Instance.PlayButton();
        Debug.Log( "Pressed " + action );
    }
}
