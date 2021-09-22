using UnityEngine;
using UnityEngine.UI;

public class TypeButton : MonoBehaviour
{
    [SerializeField]
    private Button m_PreviousButton;

    [SerializeField]
    private Button m_NextButton;

    [SerializeField]
    private Text m_Text;

    [SerializeField]
    private MoveTester m_MoveTester;

    [SerializeField]
    private bool m_AllowNone = false;

    private PokemonType m_Type = PokemonType.Normal;

    public PokemonType Type => m_Type;

    public void Reset()
    {
        SetType( m_AllowNone ? PokemonType.None : PokemonType.Normal );
    }

    private void Start()
    {
        m_NextButton.onClick.AddListener( NextType );
        m_PreviousButton.onClick.AddListener( PrevType );
    }

    private void SetType( PokemonType pokemonType )
    {
        m_Type = pokemonType;

        m_Text.text = m_Type.ToString().ToUpperInvariant();
    }

    private void NextType()
    {
        if ( ++m_Type == PokemonType.Max )
        {
            m_Type = m_AllowNone ? PokemonType.None : PokemonType.None + 1;
        }

        OnTypeChange();
    }

    private void PrevType()
    {
        if ( --m_Type == ( m_AllowNone ? PokemonType.None - 1 : PokemonType.None ) )
        {
            m_Type = PokemonType.Max - 1;
        }

        OnTypeChange();
    }

    private void OnTypeChange()
    {
        m_Text.text = m_Type.ToString().ToUpperInvariant();
        m_MoveTester.OnTypeChanged();
        SoundManager.Instance.PlaySFX(SoundManager.SFX.Button);
    }
}
