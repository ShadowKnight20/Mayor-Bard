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