using System.Globalization;
using SimpleJSON;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using Utils;

public class ShowPokemon : Function
{
    [SerializeField]
    private Image m_PokemonImage;

    [SerializeField]
    private Text m_PokemonName;

    [SerializeField]
    private InputField m_InputField;

    private int m_DexNumber = 1;

    public override void OnCreate( JSONNode jsonObject )
    {
        SetPokemon();
        m_InputField.onEndEdit.AddListener( SetNumber );
    }

    private void SetPokemon()
    {
        if ( m_DexNumber < 1 )
        {
            m_DexNumber = 1;
        }

        if ( m_DexNumber > PokemonDataBase.Instance.NumberOfPokemon )
        {
            m_DexNumber = PokemonDataBase.Instance.NumberOfPokemon;
        }

        m_PokemonImage.sprite = PokemonDataBase.Instance.GetPokemonSprite( m_DexNumber );

        string pokemonName = PokemonDataBase.Instance.GetPokemonNameFromIndex( m_DexNumber );
        m_PokemonName.text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase( pokemonName );

        m_InputField.text = m_DexNumber.ToString();
    }

    private void SetNumber( string number )
    {
        if ( int.TryParse( number, out int numberInt ) )
        {
            m_DexNumber = numberInt;
            SetPokemon();
        }
    }

    public void Add()
    {
        ++m_DexNumber;
        SetPokemon();
    }

    public void Sub()
    {
        --m_DexNumber;
        SetPokemon();
    }
}
