using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    [SerializeField]
    GameObject pauseMenu;
    [HideInInspector] public bool isPaused = false;
    [SerializeField]
    GameObject gameUI;
    [SerializeField]
    private Button resumeButton;
    [SerializeField]
    private Button menuButton;
    [SerializeField]
    private Button quitButton;
    [SerializeField]
    private Button recipesButton;
    [SerializeField]
    private Button backButton;
    [SerializeField]
    private GameObject recipesCanvas;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;

        if(pauseMenu != null)
            pauseMenu.SetActive(false);

        isPaused = false;
    }

    private void Update()
    {
        Pause();

        if (pauseMenu.activeSelf)
        {
            gameUI.SetActive(false);
            if (PlayerMovement.Instance != null && PlayerMovement.Instance.movementAudioSource.isPlaying)
                PlayerMovement.Instance.movementAudioSource.Stop();
        }
        else
        {
            gameUI.SetActive(true);
        }
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        isPaused = false;

        if(InventoryManager.Instance.isInventoryOpened)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }

    public void Pause()
    {
        if(InventoryManager.Instance.isDragging)
        { 
            return;
        }

        if(Player.Instance != null)
        {
            if (Player.Instance.endPanel != null && Player.Instance.deathPanel != null)
            {
                if (Player.Instance.endPanel.activeSelf || Player.Instance.deathPanel.activeSelf)
                    return;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().buildIndex != 0 && !recipesCanvas.gameObject.activeSelf)
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            if (pauseMenu.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0f;
                isPaused = true;
                recipesCanvas.gameObject.SetActive(false);
                backButton.gameObject.SetActive(false);
            }
            else
            {
                if (InventoryManager.Instance.isInventoryOpened)
                    Cursor.lockState = CursorLockMode.None;
                else
                    Cursor.lockState = CursorLockMode.Locked;

                Time.timeScale = 1f;
                isPaused = false;
            }
        }
    }

    public void Recipes()
    {
        recipesCanvas.gameObject.SetActive(true);
    }

    public void BackButton()
    {
        recipesCanvas.gameObject.SetActive(false);
    }

    public void DisableButtons()
    {
        resumeButton.gameObject.SetActive(false);
        recipesButton.gameObject.SetActive(false);
        menuButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(true);
    }

    public void EnableButtons()
    {
        resumeButton.gameObject.SetActive(true);
        recipesButton.gameObject.SetActive(true);
        menuButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        backButton.gameObject.SetActive(false);
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }

    public void Quit()
    {
        Application.Quit();
    }
}