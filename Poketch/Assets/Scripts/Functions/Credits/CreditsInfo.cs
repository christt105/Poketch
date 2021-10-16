using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Credits", order = 1)]
public class CreditsInfo : ScriptableObject
{
    [System.Serializable]
    public struct Entry
    {
        public string name;
        public int pokemonDex;
    }

    [SerializeField]
    private List < Entry > m_Entries;

    public List < Entry > CreditsData => m_Entries;
}
