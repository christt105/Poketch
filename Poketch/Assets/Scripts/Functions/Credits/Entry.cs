using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class Entry : MonoBehaviour
{
    [SerializeField]
    private Text m_Text;

    [SerializeField]
    private Image m_PokemonImage;

    public void SetEntry( string name, int pokemon )
    {
        m_Text.text = name;
        m_PokemonImage.sprite = PokemonDataBase.Instance.GetPokemonSprite( pokemon );
    }
}
