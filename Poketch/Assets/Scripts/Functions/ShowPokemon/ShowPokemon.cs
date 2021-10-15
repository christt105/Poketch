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

    public int DexNumber { get; private set; } = 1;

    public override void OnCreate( JSONNode jsonObject )
    {
        SetPokemon();
        m_InputField.onEndEdit.AddListener( SetNumber );
    }

    private void SetPokemon()
    {
        if ( DexNumber < 1 )
        {
            DexNumber = 1;
        }

        if ( DexNumber > PokemonDataBase.Instance.NumberOfPokemon )
        {
            DexNumber = PokemonDataBase.Instance.NumberOfPokemon;
        }

        m_PokemonImage.sprite = PokemonDataBase.Instance.GetPokemonSprite( DexNumber );

        string pokemonName = PokemonDataBase.Instance.GetPokemonNameFromIndex( DexNumber );
        m_PokemonName.text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase( pokemonName );

        m_InputField.text = DexNumber.ToString();
    }

    private void SetNumber( string number )
    {
        if ( int.TryParse( number, out int numberInt ) )
        {
            DexNumber = numberInt;
            SetPokemon();
        }
    }

    public void Add()
    {
        ++DexNumber;
        SetPokemon();
    }

    public void Sub()
    {
        --DexNumber;
        SetPokemon();
    }
}
