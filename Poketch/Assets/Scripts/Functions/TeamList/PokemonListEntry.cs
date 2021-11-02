using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class PokemonListEntry : MonoBehaviour
{
    private static readonly int s_Highlight = Shader.PropertyToID( "_Highlight" );

    [SerializeField]
    private PokemonListSetting m_Settings;

    [SerializeField]
    private Image m_PokemonImage;

    [SerializeField]
    private Slider m_Slider;

    [SerializeField]
    private GameObject m_Object;

    private int m_DexNumber = 0;
    private bool m_HasStatus;

    private void Start()
    {
        TapSprite tapSprite = GetComponentInChildren < TapSprite >();
        tapSprite.OnTap += OnTapSprite;

        m_PokemonImage.material = new Material( m_PokemonImage.material );
    }

    private void OnTapSprite()
    {
        SoundManager.Instance.PlaySFX( PokemonDataBase.Instance.GetPokemonAudioClip( m_DexNumber ), 0.1f );
    }

    public void SetPokemon( int index )
    {
        m_DexNumber = index;
        m_PokemonImage.sprite = PokemonDataBase.Instance.GetPokemonSprite( index );
        m_Settings.SetPokemon( index );
    }

    /// <summary>
    /// </summary>
    /// <param name="life">from 0 to 100</param>
    public void SetLife( int life )
    {
        m_Slider.value = life;
        m_Settings.SetLife( life );
    }

    public void HasObject( bool has )
    {
        m_Object.SetActive( has );
        m_Settings.SetObject( has );
    }

    public void SetRandomPokemon()
    {
        SetPokemon( Random.Range( 0, PokemonDataBase.Instance.NumberOfPokemon ) );
        SetLife( Random.Range( 0, 101 ) );
        HasObject( Random.Range( 0, 2 ) == 1 );
        HasStatus( Random.Range( 0, 2 ) == 1 );

        CheckStatus();
    }

    private void CheckStatus()
    {
        m_PokemonImage.material.SetInt( s_Highlight, Mathf.FloorToInt( m_Slider.value ) == 0 || m_HasStatus ? 2 : 0 );
    }

    private void HasStatus( bool has )
    {
        m_HasStatus = has;
        m_Settings.SetStatus( has );
    }

    public void SetFromSettings()
    {
        m_DexNumber = m_Settings.DexNumber;

        if ( m_DexNumber <= 0 || m_DexNumber >= PokemonDataBase.Instance.NumberOfPokemon )
        {
            gameObject.SetActive( false );

            return;
        }

        gameObject.SetActive( true );
        SetPokemon( m_DexNumber );
        SetLife( Mathf.FloorToInt( m_Settings.Life * 101 ) );
        HasObject( m_Settings.HasObject );
        HasStatus( m_Settings.HasStatus );

        CheckStatus();
    }

    public JSONNode ToJson()
    {
        JSONObject json = new JSONObject();

        json["DexNumber"] = m_DexNumber;
        json["Life"] = m_Slider.value;
        json["HasObject"] = m_Object.activeSelf;
        json["HasStatus"] = m_HasStatus;

        return json;
    }

    public void Set( JSONNode pokemonJson )
    {
        SetPokemon( pokemonJson["DexNumber"] );
        SetLife( pokemonJson["Life"] );
        HasObject( pokemonJson["HasObject"] );
        HasStatus( pokemonJson["HasStatus"] );

        CheckStatus();
    }
}
