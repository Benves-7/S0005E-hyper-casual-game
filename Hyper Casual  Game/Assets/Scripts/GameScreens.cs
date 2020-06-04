using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class GameScreens : MonoBehaviour
{
    [Header("Screens")]
    public GameObject endScreenPanel;
    public GameObject hudPanel;
    public GameObject pausePanel;
    public GameObject startPanel;
    public GameObject optionsPanel;

    [Header("Pause menu - Referenses")]
    public Text pointDisplayPause;
    public Button resumeButton;
    public Button restartButton;
    public Button optionsButton;
    public Button exitButtonPause;

    [Header("Options menu - Referenses")]
    public Button backButton;

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

    [Header("Referenses")]
    public Highscore highscoreScript;
    private Movement player;
    private MapLoader map;
    private Options options;
    private GameObject cam;

    public OptionsScreen ops;


    private void Awake()
    {
        try
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
            map = GameObject.FindGameObjectWithTag("Maploader").GetComponent<MapLoader>();
            cam = GameObject.FindGameObjectWithTag("Camera");
            options = GameObject.FindGameObjectWithTag("Options").GetComponent<Options>();
        }
        catch (System.Exception)
        {
            print("ERROR: Missing references.");
        }        
        hudPanel.SetActive(true);
        startPanel.SetActive(true);
        pausePanel.SetActive(false);
        endScreenPanel.SetActive(false);
        optionsPanel.SetActive(false);

        map.stop = true;
        player.stop = true;

        time = 3f;
    }

    private void Start()
    {
        retryButton.onClick.AddListener(Retry);
        exitButton.onClick.AddListener(Exit);
        submitButton.onClick.AddListener(Submit);

        resumeButton.onClick.AddListener(Resume);
        restartButton.onClick.AddListener(Retry);
        optionsButton.onClick.AddListener(Options);
        exitButtonPause.onClick.AddListener(Exit);
        backButton.onClick.AddListener(Back);

        ops.GetSound();
    }

    private void Update()
    {
        if (player.dead && !setup)
        {
            // deactivate playercamera and stop the map.
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


            // if score is on highscore..
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
        if (Input.GetButtonDown("Cancel") && !player.dead && !startPanel.activeSelf && !optionsPanel.activeSelf && !endScreenPanel.activeSelf)
        {
            if (pausePanel.activeSelf)
            {
                Resume();
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

    private void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Exit()
    {
        options.sequence = new List<int>();
        SceneManager.LoadScene("Main Menu");
    }

    private void Resume()
    {
        pausePanel.SetActive(false);
        hudPanel.SetActive(true);
        player.stop = false;
        map.stop = false;
        cam.SetActive(true);
    }

    private void Options()
    {
        pausePanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void Back()
    {
        pausePanel.SetActive(true);
        optionsPanel.SetActive(false);
    }

    private void Submit()
    {
        if (inputField.text.Length > 0)
        {
            highscoreScript.SubmitScore(inputField.text);

            highscoreScript.nameFields[highscoreIndex].text = inputField.text;
            retryButton.interactable = true;
            exitButton.interactable = true;
        }
    }
}