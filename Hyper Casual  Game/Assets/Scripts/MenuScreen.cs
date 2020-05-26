using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuScreen : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject menuScreen;
    public GameObject levelSelectScreen;
    public GameObject optionsScreen;
    public GameObject highscoreScreen;
    public GameObject aboutScreen;

    [Header("General Buttons")]
    public Button exitButton;

    [Header("Main Menu Buttons")]
    public Button playButton;
    public Button optionsButton;
    public Button highscoreButton;
    public Button aboutButton;

    [Header("Level Select Buttons")]
    public Button tutorialButton;
    public Button scoreGameButton;

    [Header("HighscoreButton")]
    public Button resetHighscoreButton;
    private Highscore highscoreScript;

    [Header("references")]
    public GameObject optionsObject;
    public Options options;


    private void Awake()
    {
        playButton.onClick.AddListener(Play);
        optionsButton.onClick.AddListener(Options);
        exitButton.onClick.AddListener(Back);
        tutorialButton.onClick.AddListener(Tutorial);
        scoreGameButton.onClick.AddListener(ScoreGame);
        highscoreButton.onClick.AddListener(Highscore);
        resetHighscoreButton.onClick.AddListener(ResetHighscore);
        aboutButton.onClick.AddListener(About);

        optionsObject = GameObject.FindGameObjectWithTag("Options");
        options = optionsObject.GetComponent<Options>();
        highscoreScript = GetComponentInChildren<Highscore>();
        DontDestroyOnLoad(optionsObject);
    }

    // Start is called before the first frame update
    private void Start()
    {
        menuScreen.SetActive(true);
    }
    void Back()
    {
        if (menuScreen.activeSelf)
        {
            // save any game data here..
            Quit();
        }
        else if (levelSelectScreen.activeSelf)
        {
            levelSelectScreen.SetActive(false);
            menuScreen.SetActive(true);
            exitButton.GetComponentInChildren<Text>().text = "Exit";
        }
        else if (optionsScreen.activeSelf)
        {
            optionsScreen.SetActive(false);
            menuScreen.SetActive(true);
            exitButton.GetComponentInChildren<Text>().text = "Exit";
        }
        else if (highscoreScreen.activeSelf)
        {
            highscoreScreen.SetActive(false);
            menuScreen.SetActive(true);
            exitButton.GetComponentInChildren<Text>().text = "Exit";
        }
        else if (aboutScreen.activeSelf)
        {
            aboutScreen.SetActive(false);
            menuScreen.SetActive(true);
            exitButton.GetComponentInChildren<Text>().text = "Exit";
        }
    }
    void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
    void Options()
    {
        menuScreen.SetActive(false);
        optionsScreen.SetActive(true);
        exitButton.GetComponentInChildren<Text>().text = "Back";
    }
    void Play()
    {
        menuScreen.SetActive(false);
        levelSelectScreen.SetActive(true);
        exitButton.GetComponentInChildren<Text>().text = "Back";
    }
    void Highscore()
    {
        menuScreen.SetActive(false);
        highscoreScreen.SetActive(true);
        exitButton.GetComponentInChildren<Text>().text = "Back";
    }
    void ResetHighscore()
    {
        highscoreScript.ResetHighscore();
        Back();
        Highscore();
    }    
    void About()
    {
        menuScreen.SetActive(false);
        aboutScreen.SetActive(true);
        exitButton.GetComponentInChildren<Text>().text = "Back";
    }    
    void Tutorial()
    {
        options.mode = MapLoader.RunMode.tutorial;
        SceneManager.LoadScene("GameScene");
    }
    void ScoreGame()
    {
        options.mode = MapLoader.RunMode.proceduralGeneration;
        SceneManager.LoadScene("GameScene");
    }


}
