using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent( typeof( AudioSource ) )]
public class SoundManager : MonoBehaviour

{
    public enum SFX
    {
        ChangeScreen,
        Button,
        CoinStart,
        CoinBound,
        ResetCounter,
        Radar,
        Refresh,
        Alarm,
        ButtonDeny,
        SnorlaxTemporizer,
        ExplodeStopwatch,
        AlarmClock,
        RouletteStop,
        Zahori
    }

    [SerializeField]
    private List < SFXTuple > m_SFXList;

    private AudioSource m_AudioSource;

    private Dictionary < SFX, AudioClip > m_SfxDictionary;

    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        m_AudioSource = GetComponent < AudioSource >();
        m_SfxDictionary = m_SFXList.ToDictionary( sfx => sfx.key, sfx => sfx.value );
    }

    public void PlaySFX( SFX sfx, float volumeScale = 1f)
    {
        m_AudioSource.PlayOneShot( m_SfxDictionary[sfx], volumeScale );
    }

    public void PlaySFX( AudioClip audioClip, float volumeScale = 1f )
    {
        m_AudioSource.PlayOneShot( audioClip, volumeScale );
    }

    [Serializable]
    private struct SFXTuple
    {
        public SFX key;
        public AudioClip value;
    }
}
