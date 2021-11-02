using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

public class TeamList : Function
{
    [SerializeField]
    private Transform m_GridContainerTransform;

    [SerializeField]
    private Transform m_GridSettingsTransform;

    [SerializeField]
    private GameObject m_PokemonPrefab;

    [SerializeField]
    private Button m_SettingsButton;

    private PokemonListEntry[] m_PokemonList;

    private bool m_ShowingList = true;

    public override void OnCreate( JSONNode jsonObject )
    {
        m_SettingsButton.onClick.AddListener( ClickSettings );

        m_GridSettingsTransform.gameObject.SetActive( false );

        m_PokemonList = m_GridContainerTransform.GetComponentsInChildren < PokemonListEntry >();

        if ( jsonObject != null )
        {
            for ( int i = 0; i < m_PokemonList.Length; i++ )
            {
                m_PokemonList[i].Set( jsonObject.AsArray[i] );
            }
        }
        else
        {
            foreach ( PokemonListEntry poke in m_PokemonList )
            {
                poke.SetRandomPokemon();
            }
        }
    }

    public override void OnAuxButton()
    {
        foreach ( PokemonListEntry poke in m_PokemonList )
        {
            poke.SetRandomPokemon();
        }

        Save();
    }

    private void ClickSettings()
    {
        m_ShowingList = !m_ShowingList;

        m_GridContainerTransform.gameObject.SetActive( m_ShowingList );
        m_GridSettingsTransform.gameObject.SetActive( !m_ShowingList );

        if ( m_ShowingList )
        {
            foreach ( PokemonListEntry pokemonListEntry in m_PokemonList )
            {
                pokemonListEntry.SetFromSettings();
            }

            Save();
        }
    }

    private void Save()
    {
        JSONArray json = new JSONArray();

        foreach ( PokemonListEntry pokemonListEntry in m_PokemonList )
        {
            json.Add( pokemonListEntry.ToJson() );
        }

        FunctionController.Instance.SaveFunctionInfo( GetType().Name, json );
    }
}
