using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameScreens : MonoBehaviour
{
    [Header("Screens")]
    public GameObject endScreenPanel;
    public GameObject hudPanel;
    public GameObject pausePanel;
    public GameObject startPanel;

    [Header("Pause menu - Referenses")]
    public Text pointDisplayPause;
    public Button resumeButton;
    public Button restartButton;
    public Button optionsButton;
    public Button exitButtonPause;

    [Header("Hud - Referenses")]
    public Text pointDisplay;

    [Header("EndScreen - Texts")]
    public Text stateText;
    public Text pointText;

    [Header("EndScreen - Buttons.")]
    public Button retryButton;
    public Button exitButton;
    public Button submitButton;

    [Header("EndScreen - Highscore - Referenses")]
    public int highscoreIndex;
    public InputField inputField;

    [Header("StartScreen - TextField")]
    public Text countdown;
    private float time;

    private bool setup;                             // Bool to check if setup is done.
    //private int[] highscores;
    //private string[] names;
    //private int count;
    //private string[] keys = { "Score1", "Score2", "Score3", "Score4", "Score5", "Score6", "Score7", "Score8", "Score9", "Score10" };
    //private string[] nameKeys = { "Name1", "Name2", "Name3", "Name4", "Name5", "Name6", "Name7", "Name8", "Name9", "Name10" };

    [Header("Referenses")]
    public Highscore highscoreScript;
    private Movement player;
    private MapLoader map;
    private Options options;
    private GameObject cam;


    private void Awake()
    {
        try
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
            map = GameObject.FindGameObjectWithTag("Maploader").GetComponent<MapLoader>();
            options = GameObject.FindGameObjectWithTag("Options").GetComponent<Options>();
            cam = GameObject.FindGameObjectWithTag("Camera");
        }
        catch (System.Exception)
        {
            print("ERROR: Missing references.");
        }        
        hudPanel.SetActive(true);
        startPanel.SetActive(true);
        pausePanel.SetActive(false);
        endScreenPanel.SetActive(false);

        map.stop = true;
        player.stop = true;

        time = 3f;
    }

    private void Start()
    {
        retryButton.onClick.AddListener(retry);
        exitButton.onClick.AddListener(Exit);
        submitButton.onClick.AddListener(Submit);
    }

    private void Update()
    {
        if (player.dead && !setup)
        {
            cam.SetActive(false);
            map.stop = true;

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

            highscoreIndex = highscoreScript.AddScore(player.points);

            if (highscoreIndex == -1)
            {
                inputField.interactable = false;
                submitButton.interactable = false;
                retryButton.interactable = true;
                exitButton.interactable = true;
            }
            else
            {
                inputField.interactable = true;
                submitButton.interactable = true;
                retryButton.interactable = false;
                exitButton.interactable = false;
            }

            setup = true;
        }
        if (Input.GetButtonDown("Cancel") && !player.dead)
        {
            if (pausePanel.activeSelf)
            {
                pausePanel.SetActive(false);
                hudPanel.SetActive(true);
                player.stop = false;
                map.stop = false;
                cam.SetActive(true);
            }
            else
            {
                pausePanel.SetActive(true);
                hudPanel.SetActive(false);
                player.stop = true;
                map.stop = true;
                cam.SetActive(false);
                pointDisplayPause.text = pointDisplay.text;
            }
        }
        if (startPanel.activeSelf)
        {
            time -= Time.deltaTime;
            countdown.text = ((int)time).ToString();
            if ((int)time <= 0)
            {
                startPanel.SetActive(false);
                map.stop = false;
                player.stop = false;
            }
        }


        pointDisplay.text = player.points.ToString();
    }

    private void retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Exit()
    {
        options.sequence = new List<int>();
        SceneManager.LoadScene("Main Menu");
    }

    private void Submit()
    {
        if (inputField.text.Length > 0)
        {
            PlayerPrefs.SetString(highscoreScript.nameKeys[highscoreIndex], inputField.text);
            highscoreScript.nameFields[highscoreIndex].text = inputField.text;
            retryButton.interactable = true;
            exitButton.interactable = true;
        }
    }
}