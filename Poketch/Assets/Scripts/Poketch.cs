using UnityEngine;
using UnityEngine.UI;

public class Poketch : MonoBehaviour
{
    public static Poketch Instance { get; private set; }

    [SerializeField]
    private FunctionController m_FunctionsController;

    [SerializeField]
    private Button m_UpButton;
    [SerializeField]
    private Button m_DownButton;

    [SerializeField]
    private Animator m_ScreenAnimator;

    private AudioSource m_AudioSource;

    [Header("Audio Clips")] //TODO: ScriptableObject
    [SerializeField] private AudioClip m_ChangeScreenAudioClip;
    [SerializeField] private AudioClip m_ButtonAudioClip;
    [SerializeField] private AudioClip m_CoinStartAudioClip;
    [SerializeField] private AudioClip m_CoinBoundAudioClip;
    [SerializeField] private AudioClip m_RefreshCounterAudioClip;
    [SerializeField] private AudioClip m_RadarAudioClip;
    [SerializeField] private AudioClip m_RefreshAudioClip;

    #region Unity Event Functions

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        m_AudioSource = GetComponent < AudioSource >();

        m_UpButton.onClick.AddListener( () => Next(true) );
        m_DownButton.onClick.AddListener( () => Next() );
    }

    #endregion

    #region private

    private void Next(bool previous = false)
    {
        m_ScreenAnimator.SetTrigger( "changeScreen" );
        m_ScreenAnimator.SetBool( "previous", previous );
        m_AudioSource.PlayOneShot( m_ChangeScreenAudioClip );
    }

    #endregion

    public void PlayButton()
    {
        m_AudioSource.PlayOneShot( m_ButtonAudioClip );
    }
}
