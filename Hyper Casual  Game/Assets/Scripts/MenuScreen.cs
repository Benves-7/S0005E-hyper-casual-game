﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScreen : MonoBehaviour
{
    public GameObject menuScreen, levelSelectScreen, optionsScreen;
    public Button playButton, optionsButton, exitButton, tutorialButton, scoreGameButton;
    public GameObject optionsObject;
    public Options options;


    private void Awake()
    {
        playButton.onClick.AddListener(Play);
        optionsButton.onClick.AddListener(Options);
        exitButton.onClick.AddListener(Back);
        tutorialButton.onClick.AddListener(Tutorial);
        scoreGameButton.onClick.AddListener(ScoreGame);

        optionsObject = GameObject.FindGameObjectWithTag("Options");
        options = optionsObject.GetComponent<Options>();
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