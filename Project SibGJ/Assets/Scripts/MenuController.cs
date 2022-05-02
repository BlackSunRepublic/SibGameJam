using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class MenuController : MonoBehaviour
{

    public GameObject menuButtonsPanel;
    public GameObject aboutGamesPanel;
    public GameObject aboutAuthorsPanel;

    public int sceneIndex = 1;

    void Start()
    {
        menuButtonsPanel.SetActive(true);
        aboutGamesPanel.SetActive(false);
        aboutAuthorsPanel.SetActive(false);
    }

    public void ClickOnStartGame()
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void ClickOnAboutGame()
    {
        menuButtonsPanel.SetActive(false);
        aboutGamesPanel.SetActive(true);
    }

    public void ClickOnCloseAboutGame()
    {
        aboutGamesPanel.SetActive(false);
        menuButtonsPanel.SetActive(true);
    }

    public void ClickOnAboutAuthors()
    {
        menuButtonsPanel.SetActive(false);
        aboutAuthorsPanel.SetActive(true);
    }

    public void ClickOnCloseAboutAuthors()
    {
        aboutAuthorsPanel.SetActive(false);
        menuButtonsPanel.SetActive(true);
    }

    public void ClickOnExitGame()
    {
        Application.Quit();
    }
}
