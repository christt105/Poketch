using System.Globalization;
using SimpleJSON;
using UnityEngine;
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

#if UNITY_EDITOR
    private void Update()
    {
        if ( Input.GetKeyDown( KeyCode.KeypadPlus ) )
        {
            Add();
        }

        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            Sub();
        }
    }
#endif

    private void SetPokemon()
    {
        if ( DexNumber < 1 )
        {
            DexNumber = PokemonDataBase.Instance.NumberOfPokemon;
        }

        if ( DexNumber > PokemonDataBase.Instance.NumberOfPokemon )
        {
            DexNumber = 1;
        }

        m_PokemonImage.sprite = PokemonDataBase.Instance.GetPokemonSprite( DexNumber );

        string pokemonName = PokemonDataBase.Instance.GetPokemonNameFromIndex( DexNumber );
        m_PokemonName.text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase( pokemonName );

        m_InputField.text = DexNumber.ToString( "D3" );
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
