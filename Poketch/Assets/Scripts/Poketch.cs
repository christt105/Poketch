using UnityEngine;
using UnityEngine.UI;

public class Poketch : MonoBehaviour
{
    private static readonly int s_Previous = Animator.StringToHash( "previous" );
    private static readonly int s_ChangeScreen = Animator.StringToHash( "changeScreen" );

    [SerializeField]
    private Button m_UpButton;

    [SerializeField]
    private Button m_DownButton;

    [SerializeField]
    private Animator m_ScreenAnimator;

    public static Poketch Instance { get; private set; }

    #region private

    private void Next( bool previous = false )
    {
        m_ScreenAnimator.SetTrigger( s_ChangeScreen );
        m_ScreenAnimator.SetBool( s_Previous, previous );
        SoundManager.Instance.PlaySFX( SoundManager.SFX.ChangeScreen );
    }

    #endregion

    #region Unity Event Functions

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        m_UpButton.onClick.AddListener( () => Next( true ) );
        m_DownButton.onClick.AddListener( () => Next() );
    }

    #endregion
}
