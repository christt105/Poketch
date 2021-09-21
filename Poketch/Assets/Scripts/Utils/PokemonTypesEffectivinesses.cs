using System.Collections.Generic;

public struct PokemonTypeEffectiveness
{
    public List < PokemonType > effectivenesses;
    public List < PokemonType > weaknesses;
    public List < PokemonType > immune;

    #region Public

    public PokemonTypeEffectiveness(
        List < PokemonType > effectivenesses, // x2
        List < PokemonType > weaknesses,      // x0.5
        List < PokemonType > inmune )         // x0
    {
        this.weaknesses = weaknesses;
        this.effectivenesses = effectivenesses;
        immune = inmune;
    }

    #endregion
}

public static class PokemonTypesEffectivinesses
{
    public static Dictionary < PokemonType, PokemonTypeEffectiveness >
        Effectivenesses =>
        new Dictionary < PokemonType, PokemonTypeEffectiveness >
        {
            {
                PokemonType.Normal, new PokemonTypeEffectiveness(
                    new List < PokemonType >(),
                    new List < PokemonType >() { PokemonType.Rock, PokemonType.Steel },
                    new List < PokemonType >() { PokemonType.Ghost }
                )
            },
            {
                PokemonType.Fighting, new PokemonTypeEffectiveness(
                    new List < PokemonType >()
                    {
                        PokemonType.Normal,
                        PokemonType.Rock,
                        PokemonType.Steel,
                        PokemonType.Ice,
                        PokemonType.Dark
                    },
                    new List < PokemonType >()
                    {
                        PokemonType.Flying,
                        PokemonType.Poison,
                        PokemonType.Bug,
                        PokemonType.Psychic,
                        PokemonType.Fairy
                    },
                    new List < PokemonType >() { PokemonType.Ghost } )
            },
            {
                PokemonType.Flying, new PokemonTypeEffectiveness(
                    new List < PokemonType >() { PokemonType.Fighting, PokemonType.Bug, PokemonType.Grass },
                    new List < PokemonType >() { PokemonType.Rock, PokemonType.Steel, PokemonType.Electric },
                    new List < PokemonType >() { } )
            },
            {
                PokemonType.Poison, new PokemonTypeEffectiveness(
                    new List < PokemonType >() { PokemonType.Grass, PokemonType.Fairy },
                    new List < PokemonType >()
                    {
                        PokemonType.Poison, PokemonType.Ground, PokemonType.Rock, PokemonType.Ghost
                    },
                    new List < PokemonType >() { PokemonType.Steel } )
            },
            {
                PokemonType.Ground, new PokemonTypeEffectiveness(
                    new List < PokemonType >()
                    {
                        PokemonType.Poison,
                        PokemonType.Rock,
                        PokemonType.Steel,
                        PokemonType.Fire,
                        PokemonType.Electric
                    },
                    new List < PokemonType >() { PokemonType.Bug, PokemonType.Grass },
                    new List < PokemonType >() { PokemonType.Flying } )
            },
            {
                PokemonType.Rock, new PokemonTypeEffectiveness(
                    new List < PokemonType >()
                        {
                            PokemonType.Flying, PokemonType.Bug, PokemonType.Fire, PokemonType.Ice
                        },
                    new List < PokemonType >() { PokemonType.Fighting, PokemonType.Ground, PokemonType.Steel },
                    new List < PokemonType >() { } )
            },
            {
                PokemonType.Bug, new PokemonTypeEffectiveness(
                    new List < PokemonType >() { PokemonType.Grass, PokemonType.Psychic, PokemonType.Dark },
                    new List < PokemonType >()
                    {
                        PokemonType.Fighting,
                        PokemonType.Flying,
                        PokemonType.Poison,
                        PokemonType.Ghost,
                        PokemonType.Steel,
                        PokemonType.Fire,
                        PokemonType.Fairy
                    },
                    new List < PokemonType >() { } )
            },
            {
                PokemonType.Ghost, new PokemonTypeEffectiveness(
                    new List < PokemonType >() { PokemonType.Ghost, PokemonType.Psychic },
                    new List < PokemonType >() { PokemonType.Dark },
                    new List < PokemonType >() { PokemonType.Normal } )
            },
            {
                PokemonType.Steel, new PokemonTypeEffectiveness(
                    new List < PokemonType >() { PokemonType.Rock, PokemonType.Ice, PokemonType.Fairy },
                    new List < PokemonType >()
                    {
                        PokemonType.Steel, PokemonType.Fire, PokemonType.Water, PokemonType.Electric
                    },
                    new List < PokemonType >() { } )
            },
            {
                PokemonType.Fire, new PokemonTypeEffectiveness(
                    new List < PokemonType >()
                        {
                            PokemonType.Bug, PokemonType.Steel, PokemonType.Grass, PokemonType.Ice
                        },
                    new List < PokemonType >()
                    {
                        PokemonType.Rock, PokemonType.Fire, PokemonType.Water, PokemonType.Dragon
                    },
                    new List < PokemonType >() { } )
            },
            {
                PokemonType.Water, new PokemonTypeEffectiveness(
                    new List < PokemonType >() { PokemonType.Ground, PokemonType.Rock, PokemonType.Fire },
                    new List < PokemonType >() { PokemonType.Water, PokemonType.Grass, PokemonType.Dragon },
                    new List < PokemonType >() { } )
            },
            {
                PokemonType.Grass, new PokemonTypeEffectiveness(
                    new List < PokemonType >() { PokemonType.Ground, PokemonType.Rock, PokemonType.Water },
                    new List < PokemonType >()
                    {
                        PokemonType.Flying,
                        PokemonType.Poison,
                        PokemonType.Bug,
                        PokemonType.Steel,
                        PokemonType.Fire,
                        PokemonType.Grass,
                        PokemonType.Dragon
                    },
                    new List < PokemonType >() { } )
            },
            {
                PokemonType.Electric, new PokemonTypeEffectiveness(
                    new List < PokemonType >() { PokemonType.Flying, PokemonType.Water },
                    new List < PokemonType >() { PokemonType.Grass, PokemonType.Electric, PokemonType.Dragon },
                    new List < PokemonType >() { PokemonType.Ground } )
            },
            {
                PokemonType.Psychic, new PokemonTypeEffectiveness(
                    new List < PokemonType >() { PokemonType.Fighting, PokemonType.Poison },
                    new List < PokemonType >() { PokemonType.Steel, PokemonType.Psychic },
                    new List < PokemonType >() { PokemonType.Dark } )
            },
            {
                PokemonType.Ice, new PokemonTypeEffectiveness(
                    new List < PokemonType >()
                    {
                        PokemonType.Flying, PokemonType.Ground, PokemonType.Grass, PokemonType.Dragon
                    },
                    new List < PokemonType >()
                        {
                            PokemonType.Steel, PokemonType.Fire, PokemonType.Water, PokemonType.Ice
                        },
                    new List < PokemonType >() { } )
            },
            {
                PokemonType.Dragon, new PokemonTypeEffectiveness(
                    new List < PokemonType >() { PokemonType.Dragon },
                    new List < PokemonType >() { PokemonType.Steel },
                    new List < PokemonType >() { PokemonType.Fairy } )
            },
            {
                PokemonType.Dark, new PokemonTypeEffectiveness(
                    new List < PokemonType >() { PokemonType.Ghost, PokemonType.Psychic },
                    new List < PokemonType >() { PokemonType.Fighting, PokemonType.Dark, PokemonType.Fairy },
                    new List < PokemonType >() { } )
            },
            {
                PokemonType.Fairy, new PokemonTypeEffectiveness(
                    new List < PokemonType >() { PokemonType.Fighting, PokemonType.Dragon, PokemonType.Dark },
                    new List < PokemonType >() { PokemonType.Poison, PokemonType.Steel, PokemonType.Fire },
                    new List < PokemonType >() { } )
            },
        };
}
