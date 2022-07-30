using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneManagement : MonoBehaviour
{
    private float StartSceneLoadDelay = 1f;
    private float LevelLoadDelay = 1f;
    private float LevelRestartDelay = 1f;

    private int currentSceneIndex;


    private void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

    }

    public void LoadNextScene()
    {
        StartCoroutine(LoadNextLevel());
    }


    public void LoadStartScene()
    {
        StartCoroutine(LoadStartMenu());
    }

    IEnumerator LoadStartMenu()
    {
        yield return new WaitForSecondsRealtime(StartSceneLoadDelay);
        SceneManager.LoadScene("StartScene");
    }

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSecondsRealtime(LevelLoadDelay);
        Time.timeScale = 1f;

        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void RestartTheLevel()
    {
        
        StartCoroutine(RestartLevel());
        
    }

    public void AdRestartLevel()
    {
        StartCoroutine(RestartLevel());
    }

    IEnumerator RestartLevel()
    {
        yield return new WaitForSecondsRealtime(LevelRestartDelay);
        Time.timeScale = 1f;

        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ButtonLoadNextScene()
    {
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void LoadSpecificLevel(int Index)
    {
        SceneManager.LoadScene(Index + 1);
    }
}
