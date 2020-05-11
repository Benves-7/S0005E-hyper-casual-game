using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameScreens : MonoBehaviour
{
    public GameObject endScreenPanel;
    public GameObject hudPanel;
    public GameObject pausePanel;

    // PauseScreen.
    public Text pointDisplayPause;
    public Button resumeButton, restartButton, optionsButton, exitButtonPause;

    // HudScreen.
    public Text pointDisplay;

    // EndScreen.
    public Text stateText;
    public Text pointText;
    public Text[] HighscoreTexts;
    private bool setup;                             // Bool to check if setup is done.
    // EndScreen - Highscore.
    private const int highscoreSize = 10;
    private int[] highscores; 
    public int count;
    private string[] keys = { "Score1", "Score2", "Score3", "Score4", "Score5", "Score6", "Score7", "Score8", "Score9", "Score10" };
    // EndScreen - Buttons.
    public Button resetButton;
    public Button retryButton;
    public Button exitButton;

    // References.
    private Movement player;
    private MapLoader map;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        map = GameObject.FindGameObjectWithTag("Maploader").GetComponent<MapLoader>();

        hudPanel.SetActive(true);
        pausePanel.SetActive(false);
        endScreenPanel.SetActive(false);
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
        if (player.stop && !pausePanel.activeSelf && !setup)
        {
            checkHighScore();

            // activate endscreen.
            endScreenPanel.SetActive(true);
            hudPanel.SetActive(false);

            // set state to correct gamestate.
            if (player.state == 0)
            {
                stateText.text = "Game Over";
            }
            else if (player.state == 1)
            {
                stateText.text = "Congratulations";
            }

            // display points
            pointText.text = player.points.ToString();

            setup = true;
        }
        if (Input.GetButtonDown("Cancel"))
        {
            if (pausePanel.activeSelf)
            {
                pausePanel.SetActive(false);
                hudPanel.SetActive(true);
                player.stop = false;
                map.stop = false;
            }
            else
            {
                pausePanel.SetActive(true);
                hudPanel.SetActive(false);
                player.stop = true;
                pointDisplayPause.text = pointDisplay.text;
            }
        }
        pointDisplay.text = player.points.ToString();
    }

    private void checkHighScore()
    {
        int score = player.points;

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