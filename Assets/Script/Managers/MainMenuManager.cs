using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance { get; private set; }

    [SerializeField]
    Button playButton;
    [SerializeField]
    Button settingsButton;
    [SerializeField]
    Button recipesButton;
    [SerializeField]
    Button exitButton;
    [SerializeField]
    Button backButton;
    [SerializeField]
    GameObject recipesCanvas;
    [SerializeField]
    GameObject settingsCanvas;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        Screen.fullScreen = true;
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }

    public void Settings()
    {
        settingsCanvas.gameObject.SetActive(true);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void Recipes()
    {
        recipesCanvas.gameObject.SetActive(true);
    }

    public void BackButton()
    {
        recipesCanvas.gameObject.SetActive(false);
        settingsCanvas.gameObject.SetActive(false);
    }

    public void DisableButtons()
    {
        backButton.gameObject.SetActive(true);
        playButton.gameObject.SetActive(false);
        settingsButton.gameObject.SetActive(false);
        recipesButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
    }

    public void EnableButtons()
    {
        backButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(true);
        settingsButton.gameObject.SetActive(true);
        recipesButton.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }
}