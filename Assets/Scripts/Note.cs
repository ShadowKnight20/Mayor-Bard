using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    double timeInstantiated;
    public float assignedTime;

    // Start is called before the first frame update
    void Start()
    {
        timeInstantiated = SongManager.GetAudioSourceTime();
        GetComponent<SpriteRenderer>().enabled = true; // Enable the sprite renderer once

        Debug.DrawLine(
        new Vector3(transform.position.x, SongManager.instance.noteSpawnY, transform.position.z),
        new Vector3(transform.position.x, SongManager.instance.noteDespawnY, transform.position.z),
        Color.red,5f);
    }

    // Update is called once per frame
    //    void Update()
    //    {
    //        // Time since this note was instantiated
    //        double timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated;

    //        // Normalize the time to calculate the position along the path
    //        float t = (float)(timeSinceInstantiated / SongManager.instance.noteTime);

    //        // Destroy the note if it has passed the despawn position
    //        if (t > 1)
    //        {
    //            Destroy(gameObject);
    //        }
    //        else
    //        {
    //            // Interpolate the position between spawn and despawn
    //            transform.localPosition = Vector3.Lerp(
    //                Vector3.up * SongManager.instance.noteSpawnY,
    //                Vector3.up * SongManager.instance.noteDespawnY,t);
    //        }
    //    }
    //}

    //    void Update()
    //    {
    //        // Time since the note was instantiated
    //        double timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated;

    //        // Normalize time for movement (0 to 1)
    //        float t = (float)(timeSinceInstantiated / SongManager.instance.noteTime);

    //        // Destroy the note if it reaches or exceeds despawn point
    //        if (t > 1)
    //        {
    //            Destroy(gameObject);
    //        }
    //        else
    //        {
    //            // Interpolate the note's position along the Z-axis
    //            transform.localPosition = Vector3.Lerp(
    //                new Vector3(transform.localPosition.x, transform.localPosition.y, SongManager.instance.noteSpawnZ),
    //                new Vector3(transform.localPosition.x, transform.localPosition.y, SongManager.instance.noteDespawnZ),
    //                t
    //            );
    //        }
    //    }
    //}
    void Update()
    {
        // Time since the note was instantiated
        double timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated;

        // Normalize time for movement (0 to 1)
        float t = (float)(timeSinceInstantiated / SongManager.instance.noteTime);

        // Destroy the note if it reaches or exceeds the despawn point
        if (t > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            // Interpolate the note's position along the Y-axis
            transform.localPosition = Vector3.Lerp(
                new Vector3(transform.localPosition.x, SongManager.instance.noteSpawnY, transform.localPosition.z),
                new Vector3(transform.localPosition.x, SongManager.instance.noteDespawnY, transform.localPosition.z),
                t
            );
        }
    }
}