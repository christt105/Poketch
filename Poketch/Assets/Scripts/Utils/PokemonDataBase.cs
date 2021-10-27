using System.Collections.Generic;
using System.IO;
using System.Linq;
using SimpleJSON;
using UnityEngine;
using UnityEngine.U2D;

namespace Utils
{

public class PokemonDataBase : MonoBehaviour
{
    public static PokemonDataBase Instance;

    [SerializeField]
    private TextAsset m_JsonData;

    [SerializeField]
    private SpriteAtlas m_SpriteAtlas;

    private Dictionary < int, string > m_PokemonDictionary;

    private Dictionary < int, string > PokemonDictionary
    {
        get
        {
            if ( m_PokemonDictionary == null )
            {
                LoadDictionary();
            }

            return m_PokemonDictionary;
        }
    }

    public int NumberOfPokemon => PokemonDictionary.Count;

    private void Awake()
    {
        Instance = this;
    }

    #region Private

    private void LoadDictionary()
    {
        m_PokemonDictionary = new Dictionary < int, string >();

        JSONNode jsonInfo = JSON.Parse( m_JsonData.text );

        foreach ( KeyValuePair < string, JSONNode > pokemon in jsonInfo )
        {
            if ( !int.TryParse( pokemon.Key, out int dexNumber ) )
            {
                Debug.LogWarning(
                    "Error parsing the number " + pokemon.Key + " of Pokémon " + pokemon.Value );

                continue;
            }

            m_PokemonDictionary.Add( dexNumber, pokemon.Value );
        }

        //Check
#if false
        int i = 1;
        foreach ( KeyValuePair < int, string > p in PokemonDictionary )
        {
            if ( GetPokemonSprite( p.Value ) == null )
            {
                Debug.LogError( "Sprite of Pokemon " + p.Value + " not found" );
            }

            if ( p.Key != i++ )
            {
                Debug.LogError( "Missing " + ( i - 1 ) + " Pokemon" );
            }
        }
#endif
    }

    //TODO: separate on a tool
    private void GeneratePokedexJSON()
    {
        JSONNode jsonInfo = JSON.Parse( m_JsonData.text );

        JSONNode jsonSave = new JSONObject();

        foreach ( KeyValuePair < string, JSONNode > pokemon in jsonInfo )
        {
            if ( !int.TryParse( pokemon.Key, out int dexNumber ) )
            {
                Debug.LogWarning(
                    "Error parsing the number " + pokemon.Key + " of Pokémon " + pokemon.Value["name"]["eng"] );

                continue;
            }

            jsonSave.Add( dexNumber.ToString(), pokemon.Value["slug"]["eng"] );
        }

        File.WriteAllText( "Assets/Data/dex.json", jsonSave.ToString( 1 ) );
    }

    #endregion

    #region Public

    public string GetPokemonNameFromIndex( int index )
    {
        return PokemonDictionary.ContainsKey( index ) ? PokemonDictionary[index] : "none";
    }

    public int GetPokemonIndexFromName( string name )
    {
        return PokemonDictionary.FirstOrDefault( x => x.Value == name ).Key;
    }

    public Sprite GetPokemonSprite( string name )
    {
        return m_SpriteAtlas.GetSprite( name.ToLower() );
    }

    public Sprite GetPokemonSprite( int index )
    {
        return GetPokemonSprite( GetPokemonNameFromIndex( index ) );
    }

    public AudioClip GetPokemonAudioClip( int index )
    {
        return Resources.Load < AudioClip >( "PokemonCries/" + index );
    }

    public AudioClip GetPokemonAudioClip( string name )
    {
        return Resources.Load < AudioClip >( "PokemonCries/" + GetPokemonIndexFromName( name ) );
    }

    #endregion
}

}
