using System.Collections.Generic;

public class PokemonTypeChart
{
    private readonly byte[,] m_TypeChart = new byte[( byte ) PokemonType.Max, ( byte ) PokemonType.Max];

    #region Public

    public PokemonTypeChart()
    {
        InitTypeChart();
    }

    public float GetEffectiveness(
        PokemonType attackType,
        PokemonType pokemonType,
        PokemonType pokemonType2 = PokemonType.None )
    {
        return m_TypeChart[( int ) pokemonType, ( int ) attackType] *
               0.5f *
               ( pokemonType2 == PokemonType.None ? 1f : m_TypeChart[( int ) pokemonType2, ( int ) attackType] * 0.5f );
    }

    public override string ToString()
    {
        string sr = string.Empty;

        string header = "\t";

        for ( int i = 0; i < ( int ) PokemonType.Max; ++i )
        {
            header += ( ( PokemonType ) i ).ToString().
                                            Substring(
                                                0,
                                                ( ( PokemonType ) i ).ToString().Length < 5
                                                    ? ( ( PokemonType ) i ).ToString().Length
                                                    : 5 ) +
                      "\t";
        }

        sr += header + "\n";

        for ( int i = 0; i < ( int ) PokemonType.Max; ++i )
        {
            string line = ( ( PokemonType ) i ).ToString().
                                                Substring(
                                                    0,
                                                    ( ( PokemonType ) i ).ToString().Length < 5
                                                        ? ( ( PokemonType ) i ).ToString().Length
                                                        : 5 ) +
                          "\t";

            for ( int j = 0; j < ( int ) PokemonType.Max; ++j )
            {
                line += GetEffectiveness( ( PokemonType ) j, ( PokemonType ) i ) + "\t";
            }

            sr += line + "\n";
        }

        return sr;
    }

    #endregion

    #region Private

    private void InitTypeChart()
    {
        for ( int i = 0; i < ( int ) PokemonType.Max; i += 1 )
        {
            for ( int j = 0; j < ( int ) PokemonType.Max; j += 1 )
            {
                m_TypeChart[i, j] = 2;
            }
        }

        foreach ( KeyValuePair < PokemonType, PokemonTypeEffectiveness > pokemonType in PokemonTypesEffectivinesses.
            Effectivenesses )
        {
            foreach ( PokemonType deb in pokemonType.Value.effectivenesses )
            {
                m_TypeChart[( int ) deb, ( int ) pokemonType.Key] = 4;
            }

            foreach ( PokemonType deb in pokemonType.Value.weaknesses )
            {
                m_TypeChart[( int ) deb, ( int ) pokemonType.Key] = 1;
            }

            foreach ( PokemonType deb in pokemonType.Value.immune )
            {
                m_TypeChart[( int ) deb, ( int ) pokemonType.Key] = 0;
            }
        }
    }

    #endregion
}
