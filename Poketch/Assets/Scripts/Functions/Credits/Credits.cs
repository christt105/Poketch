using SimpleJSON;
using UnityEngine;

public class Credits : Function
{
    [SerializeField]
    private CreditsInfo m_Data;

    [SerializeField]
    private GameObject m_EntryPrefab;

    [SerializeField]
    private RectTransform m_Panel;

    public override void OnCreate( JSONNode jsonObject )
    {
        foreach ( CreditsInfo.Entry n in m_Data.CreditsData )
        {
            GameObject entry = Instantiate( m_EntryPrefab, m_Panel );
            entry.GetComponent < Entry >().SetEntry( n.name, n.pokemonDex );
        }
    }
}
