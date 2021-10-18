using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicPlayer : Function
{
    [Header("Music Player")]
    [SerializeField] AudioClip[] songs;
    [SerializeField] Text songName;
    [SerializeField] Button playButton;
    [SerializeField] Slider songProgression;
    [SerializeField] Text currentTimeText;
    [SerializeField] Text songText;

    [SerializeField] Sprite play;
    [SerializeField] Sprite pause;

    AudioSource player;
    int currentSong = 0;
    float currentSongTime = 0;
    float completeTime = 0;

    public override void OnCreate(JSONNode jsonObject)
    {
        if (jsonObject != null) currentSong = jsonObject.GetValueOrDefault("currentSong", 0);

        player = GetComponent<AudioSource>();

        player.clip = songs[currentSong];
        completeTime = player.clip.length;
        songName.text = player.clip.name;
        ApplyTimeToUI(songText, completeTime);
    }

    private void Update()
    {
        if (!player.isPlaying) return;

        currentSongTime += Time.deltaTime;
        ApplyTimeToUI(currentTimeText, currentSongTime);

        songProgression.value = currentSongTime / completeTime;

        if (currentSongTime >= completeTime)
        {
            NextTrack();
        }
    }

    public void PlayPressed()
    {
        playButton.image.sprite = playButton.image.sprite == play ? pause : play;

        if (player.isPlaying)
        {
            player.Stop();
        }
        else
        {
            player.Play();
        }
    }

    public void NextTrack()
    {
        ChangeTrack(1, true);
    }

    public void PreviousTrack()
    {
        if (currentSongTime >= 2)
        {
            currentSongTime = 0;
            ApplyTimeToUI(currentTimeText, currentSongTime);
            player.time = 0;
        }
        else
        {
            ChangeTrack(-1, player.isPlaying);
        }
    }

    void ChangeTrack(int change, bool play)
    {
        currentSongTime = 0;
        ApplyTimeToUI(currentTimeText, currentSongTime);

        currentSong += change;

        if (currentSong < 0) currentSong = songs.Length - 1;
        if (currentSong >= songs.Length) currentSong = 0;

        player.clip = songs[currentSong];
        completeTime = player.clip.length;
        ApplyTimeToUI(songText, completeTime);
        songName.text = player.clip.name;
        SaveLastSong();

        if (play && !player.isPlaying)
        {
            playButton.image.sprite = pause;
            player.Play();
        }
    }

    void ApplyTimeToUI(Text text, float seconds)
    {
        int minutes = Mathf.FloorToInt(seconds / 60);
        int secs = Mathf.FloorToInt(seconds - minutes * 60);
        string minutesS = minutes > 9 ? minutes.ToString() : "0" + minutes;
        string secsS = secs > 9 ? secs.ToString() : "0" + secs;

        text.text = minutesS + ":" + secsS;
    }

    public void SliderChange()
    {
        if (Mathf.Approximately(songProgression.value, currentSongTime / completeTime)) return;

        currentSongTime = completeTime * songProgression.value;
        player.time = songProgression.value;
        ApplyTimeToUI(currentTimeText, currentSongTime);
    }

    void SaveLastSong()
    {
        JSONNode json = new JSONObject();
        json.Add("currentSong", currentSong);
        FunctionController.Instance.SaveFunctionInfo(GetType().Name, json);
    }
}
