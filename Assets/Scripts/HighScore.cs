using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScore : MonoBehaviour
{
    static public int knifeHighScore = 0;
    static public int stageHighScore = 0;
    

    private void Awake()
    {
        if (PlayerPrefs.HasKey("knifeHighScore"))
        {
            knifeHighScore = PlayerPrefs.GetInt("knifeHighScore");
        }

        if (PlayerPrefs.HasKey("stageHighScore"))
        {
            stageHighScore = PlayerPrefs.GetInt("stageHighScore");
        }
    }


    static public void SetKnifeHighScore(int knifeScore)
    {
        if (knifeScore > knifeHighScore)
        {
            PlayerPrefs.SetInt("knifeHighScore", knifeScore);
            knifeHighScore = knifeScore;
        }
    }


    static public void SetStageHighScore(int stageScore)
    {
        if (stageScore > stageHighScore)
        {
            PlayerPrefs.SetInt("stageHighScore", stageScore);
            stageHighScore = stageScore;
        }            
    }


    static public void ResetScore()
    {
        PlayerPrefs.SetInt("knifeHighScore", 0);
        knifeHighScore = 0;

        PlayerPrefs.SetInt("stageHighScore", 0);
        stageHighScore = 0;
    }
}
