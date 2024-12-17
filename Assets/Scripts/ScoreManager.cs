using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public AudioSource hitSFX;
    public AudioSource missSFX;
    public Text scoreText;
    static int comboScore;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        comboScore = 0;
    }
    public static void Hit()
    {
        comboScore += 1;
        instance.hitSFX.Play();
    }
    public static void Miss()
    {
        comboScore = 0;
        instance.missSFX.Play();
    }
    // Update is called once per frame
    private void Update()
    {
        scoreText.text = comboScore.ToString();
    }
}
