using UnityEngine;
using UnityEngine.UI;

public class PokemonListSetting : MonoBehaviour
{
    [SerializeField]
    private InputField m_DexNumber;

    [SerializeField]
    private Slider m_LifeSlider;

    [SerializeField]
    private Toggle m_HasObject;

    [SerializeField]
    private Toggle m_HasStatus;

    public int DexNumber => m_DexNumber.text == "" ? 0 : int.Parse( m_DexNumber.text );

    public float Life => m_LifeSlider.value;

    public bool HasObject => m_HasObject.isOn;

    public bool HasStatus => m_HasStatus.isOn;

    public void SetPokemon( int index )
    {
        m_DexNumber.text = index.ToString();
    }

    public void SetLife( int life )
    {
        m_LifeSlider.value = life / 100f;
    }

    public void SetObject( bool has )
    {
        m_HasObject.isOn = has;
    }

    public void SetStatus( bool has )
    {
        m_HasStatus.isOn = has;
    }
}
