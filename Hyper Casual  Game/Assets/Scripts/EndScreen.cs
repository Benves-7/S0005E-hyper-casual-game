using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public GameObject Canvas;
    public Text stateText;
    public Text pointText;
    public Text[] HighscoreTexts;


    private const int highscoreSize = 10;
    private int[] highscores; 
    public int count;

    public Button resetButton;
    public Button retryButton;
    public Button exitButton;

    private Movement playerMovementController;
    private bool setup;
    private string[] keys = { "Score1", "Score2", "Score3", "Score4", "Score5", "Score6", "Score7", "Score8", "Score9", "Score10" };


    private void Awake()
    {
        playerMovementController = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
    }

    private void Start()
    {
        resetButton.onClick.AddListener(resetHighScore);
        retryButton.onClick.AddListener(retry);
        exitButton.onClick.AddListener(Exit);
        highscores = new int[highscoreSize];
    }

    private void Update()
    {
        if (playerMovementController.stop && !setup)
        {
            checkHighScore();

            // activate endscreen.
            Canvas.SetActive(true);

            // set state to correct gamestate.
            if (playerMovementController.state == 0)
            {
                stateText.text = "Game Over";
            }
            else if (playerMovementController.state == 1)
            {
                stateText.text = "Congratulations";
            }

            // display points
            pointText.text = playerMovementController.points.ToString();

            setup = true;
        }
    }

    private void checkHighScore()
    {
        int score = playerMovementController.points;

        /// If Highscore has ever been saved.
        if (PlayerPrefs.HasKey("Highscore"))
        {
            // Highscore contains the amount of saved scores.
            count = PlayerPrefs.GetInt("Highscore");
            
            /// Extract saved scores to highscore array.
            for (int i = 0; i < count; i++)
            {
                if (PlayerPrefs.HasKey(keys[i]))
                {
                    highscores[i] = PlayerPrefs.GetInt(keys[i]);
                }
            }

            /// Add and sort new score.
            for (int i = highscoreSize - 1; i >= 0; i--)
            {
                // If current highscore is bigger then new score and current highscore is not the last highscore add the new score one step bellow current highscore.
                if (highscores[i] > score && !(i == highscoreSize - 1))
                {
                    highscores[i + 1] = score;
                    break;
                }
                // If current highscore is less then new score.
                else
                {
                    // If current highscore is last highscore, replace it with score.
                    if (i == highscoreSize - 1)
                    {
                        highscores[i] = score;
                    }
                    // If current highscore is not last highscore, move that score one step and then place new score in its place.
                    else
                    {
                        highscores[i + 1] = highscores[i];
                        highscores[i] = score;
                    }
                }
            }

            /// Fill in the highscore into the textfields.
            for (int i = 0; i < HighscoreTexts.Length; i++)
            {
                // If the score is 0, don't print 0.
                if (highscores[i] == 0)
                {
                    HighscoreTexts[i].text = "-";
                }
                // Else just print the score.
                else
                {
                    HighscoreTexts[i].text = highscores[i].ToString();
                }
            }

            // If number of saved highscore is less than maximum number of saved highscore increase the integer in PlayerPrefs.
            if (count < highscoreSize)
            {
                PlayerPrefs.SetInt("Highscore", count + 1);
            }
            // Save all the new highscores.
            for (int i = 0; i < count; i++)
            {
                PlayerPrefs.SetInt(keys[i], highscores[i]);
            }
        }
        /// Create Highscore
        else
        {
            PlayerPrefs.SetInt("Highscore", 1);
            PlayerPrefs.SetInt(keys[0], score);
            HighscoreTexts[0].text = score.ToString();
        }
    }

    private void resetHighScore()
    {
        /// Delete all keys that relates to Highscores.
        if (PlayerPrefs.HasKey("Highscore"))
        {
            PlayerPrefs.DeleteKey("Highscore");
        }
        foreach (string key in keys)
        {
            if (PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.DeleteKey(key);
            }
        }
        foreach (Text text in HighscoreTexts)
        {
            text.text = "-";
        }
        // run the highscore check again.
        checkHighScore();
    }

    private void retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Exit()
    {
        SceneManager.LoadScene("Main Menu");
    }
}