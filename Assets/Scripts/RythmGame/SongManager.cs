using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using UnityEngine.Networking;
using System;

public class SongManager : MonoBehaviour
{
    public static SongManager instance;
    public AudioSource audioSource;
    public Lane[] lanes;
    public float songDelayInSeconds;

    public double marginOfError; //in seconds

    public int inputDelayInMilliseconds;

    public string fileLocation;
    public float noteTime;

    public float noteSpawnY;  // Spawn position (higher in Y)
    public float noteTapY;    // Tap position (near the bottom)
    public float noteDespawnY
    {
        get
        {
            return noteTapY - (noteSpawnY - noteTapY); // Symmetrical despawn below tap
        }
    }

    public static MidiFile midiFile;

    void Start()
    {
        instance = this;
        if (Application.streamingAssetsPath.StartsWith("http://") || Application.streamingAssetsPath.StartsWith("https://"))
        {
            StartCoroutine(ReadFromWebsite());
        }
        else
        {
            ReadFromFile();
        }
    }

    private IEnumerator ReadFromWebsite()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(Application.streamingAssetsPath + "/" + fileLocation))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                byte[] results = www.downloadHandler.data;
                using (var stream = new MemoryStream(results))
                {
                    midiFile = MidiFile.Read(stream);
                    GetDataFromMidi();
                }
            }
        }
    }

    private void ReadFromFile()
    {
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + fileLocation);
        GetDataFromMidi();
    }

    public void GetDataFromMidi()
    {
        var notes = midiFile.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(array, 0);

        foreach (var lane in lanes) lane.SetTimeStamps(array);

        Invoke(nameof(StartSong), songDelayInSeconds);
    }

    public void StartSong()
    {
        audioSource.Play();
        Debug.Log("Song started.");
    }

    public static double GetAudioSourceTime()
    {
        return (double)instance.audioSource.timeSamples / instance.audioSource.clip.frequency;
    }

    void Update()
    {
        // Check if the song has ended
        if (!audioSource.isPlaying && audioSource.time >= audioSource.clip.length)
        {
            Debug.Log("Song has ended. Disabling SongManager.");
            gameObject.SetActive(false); // Disable the SongManager GameObject
        }
    }

    void OnEnable()
    {
        // Restart the song when the object is enabled
        Debug.Log("SongManager enabled. Reinitializing...");
        audioSource.Stop();
        audioSource.time = 0; // Reset the song time
        if (midiFile != null)
        {
            GetDataFromMidi();
        }
        else
        {
            Debug.LogWarning("MIDI file not loaded. Ensure it's loaded before re-enabling.");
        }
    }
}
