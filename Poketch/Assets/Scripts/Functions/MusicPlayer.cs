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
    [SerializeField] GameObject shuffleUI;
    [SerializeField] GameObject loopUI;

    [SerializeField] Sprite play;
    [SerializeField] Sprite pause;

    AudioSource player;
    int currentSong = 0;
    float completeTime = 0;

    bool shuffle = false;
    bool looping = false;

    List<int> randomSongs = new List<int>();

    public override void OnCreate(JSONNode jsonObject)
    {
        if (jsonObject != null)
        {
            currentSong = jsonObject.GetValueOrDefault("currentSong", 0);

            shuffle = jsonObject.GetValueOrDefault("shuffle", false);
            shuffleUI.SetActive(shuffle);

            looping = jsonObject.GetValueOrDefault("looping", false);
            loopUI.SetActive(looping);
        }

        player = GetComponent<AudioSource>();

        player.clip = songs[currentSong];
        completeTime = player.clip.length;
        songName.text = player.clip.name;
        ApplyTimeToUI(songText, completeTime);
    }

    private void Update()
    {
        if (player.isPlaying)
        {
            ApplyTimeToUI(currentTimeText, player.time);

            songProgression.value = player.time / completeTime;
        }

        if (player.time >= completeTime)
        {
            NextTrack();
        }
    }

    public void PlayPressed()
    {
        playButton.image.sprite = playButton.image.sprite == play ? pause : play;

        if (player.isPlaying)
        {
            player.Pause();
        }
        else
        {
            player.Play();
        }
    }

    public void NextTrack()
    {
        if (looping)
        {
            player.time = 0;
            return;
        }

        if (shuffle)
        {
            if (randomSongs.Count == songs.Length)
            {
                randomSongs.Clear();
            }

            int randomSong = Random.Range(0, songs.Length);

            while (randomSongs.Contains(randomSong) && randomSong != currentSong)
            {
                randomSong = Random.Range(0, songs.Length);
            }

            randomSongs.Add(randomSong);
            currentSong = randomSong;
            ChangeTrack(0, true);

            return;
        }

        ChangeTrack(1, true);
    }

    public void PreviousTrack()
    {
        if (player.time >= 2)
        {
            player.time = 0;
            ApplyTimeToUI(currentTimeText, player.time);
        }
        else
        {
            ChangeTrack(-1, player.isPlaying);
        }
    }

    void ChangeTrack(int change, bool play)
    {
        player.time = 0;
        ApplyTimeToUI(currentTimeText, player.time);

        currentSong += change;

        if (currentSong < 0) currentSong = songs.Length - 1;
        if (currentSong >= songs.Length) currentSong = 0;

        player.clip = songs[currentSong];
        completeTime = player.clip.length;
        ApplyTimeToUI(songText, completeTime);
        songName.text = player.clip.name;
        SaveInfo();

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
        if (Mathf.Approximately(songProgression.value, player.time / completeTime)) return;

        player.time = songProgression.value * completeTime;
        ApplyTimeToUI(currentTimeText, player.time);
    }

    void SaveInfo()
    {
        JSONNode json = new JSONObject();
        json.Add("currentSong", currentSong);
        json.Add("looping", looping);
        json.Add("shuffle", shuffle);
        FunctionController.Instance.SaveFunctionInfo(GetType().Name, json);
    }

    public void ActivateShuffle()
    {
        shuffle = !shuffle;
        shuffleUI.SetActive(shuffle);

        if (!shuffle)
        {
            randomSongs.Clear();
        }

        SaveInfo();
    }

    public void ActiveLoop()
    {
        looping = !looping;
        loopUI.SetActive(looping);
        player.loop = looping;
        SaveInfo();
    }
}
