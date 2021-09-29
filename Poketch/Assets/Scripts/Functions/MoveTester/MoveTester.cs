using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

public class MoveTester : Function
{
    [SerializeField]
    private Transform m_ExclamationsTransform;

    [SerializeField]
    private TypeButton m_AttackType;

    [SerializeField]
    private TypeButton m_PokemonType1;

    [SerializeField]
    private TypeButton m_PokemonType2;

    [SerializeField]
    private Text m_EffectivinessText;

    private readonly PokemonTypeChart m_TypeChart = new PokemonTypeChart();
    private GameObject[] m_Exclamations;

    #region Private

    private int GetEffectivenessIndex( float effectiveness )
    {
        int index = ( int ) ( effectiveness * 100f / 25f );

        switch ( index )
        {
            case 0:
            case 1:
            case 2:
                return index;

            case 4:
                return 3;

            case 8:
                return 4;

            case 16:
                return 5;
        }

        Debug.LogError( $"Effectiveness {index.ToString()} not compatible" );

        return -1;
    }

    #endregion

    public void OnTypeChanged()
    {
        int exclamations = GetEffectivenessIndex(
            m_TypeChart.GetEffectiveness(
                m_AttackType.Type,
                m_PokemonType1.Type,
                m_PokemonType1.Type == m_PokemonType2.Type ? PokemonType.None : m_PokemonType2.Type ) );

        for ( int i = 0; i < m_Exclamations.Length; i++ )
        {
            m_Exclamations[i].gameObject.SetActive( i < exclamations );
        }

        m_EffectivinessText.text = GetEffectivenessTextByIndex( exclamations );
    }

    private static string GetEffectivenessTextByIndex( int exclamations )
    {
        switch ( exclamations )
        {
            case 0:
                return "No effect";
            case 1:
            case 2:
                return "Not very effective";
            case 3:
                return "Regularly effective";
            case 4:
            case 5:
                return "Super effective";
        }

        return string.Empty;
    }

    #region Functions

    public override void OnCreate( JSONObject jsonObject )
    {
        m_Exclamations = new GameObject[m_ExclamationsTransform.childCount];

        for ( int i = 0; i < m_Exclamations.Length; i++ )
        {
            m_Exclamations[i] = m_ExclamationsTransform.GetChild( i ).gameObject;
        }
    }

    public override void OnStart()
    {
        Reset();
    }

    public override void OnChange()
    {
        Reset();
    }

    private void Reset()
    {
        m_AttackType.Reset();
        m_PokemonType1.Reset();
        m_PokemonType2.Reset();

        OnTypeChanged();
    }

    #endregion
}
