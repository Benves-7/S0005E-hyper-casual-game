using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Highscore : MonoBehaviour
{
    public Text[] scoreFields;
    public Text[] nameFields;
    public string[] keys = { "Score1", "Score2", "Score3", "Score4", "Score5", "Score6", "Score7", "Score8", "Score9", "Score10" };
    public string[] nameKeys = { "Name1", "Name2", "Name3", "Name4", "Name5", "Name6", "Name7", "Name8", "Name9", "Name10" };

    private int count;
    private int[] highscores = new int[10];
    private string[] names = new string[10];
    private int index;


    void OnEnable()
    {
        /// If Highscore has ever been saved.
        if (PlayerPrefs.HasKey("Highscore"))
        {
            // Highscore contains the amount of saved scores.
            count = PlayerPrefs.GetInt("Highscore");
            highscores = new int[10];
            names = new string[10];

            /// Extract saved scores to highscore array.
            for (int i = 0; i < count; i++)
            {
                if (PlayerPrefs.HasKey(Keys.keys[i]))
                {
                    highscores[i] = PlayerPrefs.GetInt(Keys.keys[i]);
                }
                if (PlayerPrefs.HasKey(Keys.nameKeys[i]))
                {
                    names[i] = PlayerPrefs.GetString(Keys.nameKeys[i]);
                }
            }

            /// Fill in the highscore into the textfields.
            for (int i = 0; i < scoreFields.Length; i++)
            {
                scoreFields[i].text = highscores[i].ToString();
            }
            for (int i = 0; i < nameFields.Length; i++)
            {
                nameFields[i].text = names[i];
            }
        }
    }

    public int AddScore(int score)
    {
        index = -1;
        /// Add and sort new score.
        for (int i = highscores.Length-1; i >= 0; i--)
        {
            // If current highscore is bigger then new score and current highscore is not the last highscore add the new score one step bellow current highscore.
            if (highscores[i] > score)
            {
                if (i == 9)
                {
                    break;
                }
                else
                {
                    highscores[i + 1] = score;
                    index = i + 1;
                    break;
                }
            }
            // If current highscore is less then new score.
            else
            {
                // If current highscore is last highscore, replace it with score.
                if (i == 9)
                {
                    highscores[i] = score;
                }
                else if (i == 0)
                {
                    highscores[i + 1] = highscores[i];
                    names[i + 1] = names[i];
                    highscores[i] = score;
                    index = i;
                    break;
                }
                // If current highscore is not last highscore, move that score one step and then place new score in its place.
                else
                {
                    highscores[i + 1] = highscores[i];
                    names[i + 1] = names[i];
                    highscores[i] = score;
                    index = i;
                }
            }
        }
        /// Fill in the highscore into the textfields.
        for (int i = 0; i < scoreFields.Length; i++)
        {
            scoreFields[i].text = highscores[i].ToString();
        }
        for (int i = 0; i < nameFields.Length; i++)
        {
            if (i == index)
            {
                nameFields[i].text = "-";
            }
            else
            {
                nameFields[i].text = names[i];
            }
        }
        /// Return the index (-1 means score to low)
        return index;
    }

    public void SubmitScore(string name)
    {
        for (int i = 0; i < highscores.Length - 1; i++)
        {
            PlayerPrefs.SetInt(Keys.keys[i], highscores[i]);
            PlayerPrefs.SetString(Keys.nameKeys[i], names[i]);
        }
        PlayerPrefs.SetString(Keys.nameKeys[index], name);
        if (count < 10)
        {
            PlayerPrefs.SetInt("Highscore", count + 1);
        }
    }

    public void ResetHighscore()
    {
        PlayerPrefs.SetInt("Highscore", 0);

        foreach (string key in Keys.keys)
        {
            if (PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.DeleteKey(key);
            }
        }
        foreach (string key in Keys.nameKeys)
        {
            if (PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.DeleteKey(key);
            }
        }
    }
}
