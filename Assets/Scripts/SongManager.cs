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

    //public float noteSpawnZ;  // Where notes spawn (far from the tap zone)
    //public float noteTapZ;    // Where notes should be tapped
    //public float noteDespawnZ
    //{
    //    get
    //    {
    //        return noteTapZ - (noteSpawnZ - noteTapZ); // Symmetrical despawn position
    //    }
    //}

    public static MidiFile midiFile;

    void Start()
    {
        instance = this;
        if(Application.streamingAssetsPath.StartsWith("http://") || Application.streamingAssetsPath.StartsWith("https://"))
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
        midiFile = MidiFile.Read(Application.streamingAssetsPath +"/"+fileLocation);
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
    }
    public static double GetAudioSourceTime()
    {
        return (double)instance.audioSource.timeSamples / instance.audioSource.clip.frequency;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
