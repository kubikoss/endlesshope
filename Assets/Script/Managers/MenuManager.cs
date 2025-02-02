using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Map");
    }

    public void Settings()
    {
        Debug.Log("settings");
    }

    public void Recipes()
    {
        Debug.Log("recipes");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
