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
        if(Player.Instance != null)
        {
            if (Player.Instance.endPanel != null && Player.Instance.deathPanel != null)
            {
                if (Player.Instance.endPanel.activeSelf || Player.Instance.deathPanel.activeSelf)
                    return;
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().buildIndex != 0)
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            if(pauseMenu.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0f;
                isPaused = true;
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