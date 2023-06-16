using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagement : MonoBehaviour
{
    [SerializeField] private Image transparentImage;
    [SerializeField] private ParticleSystem baths;

    private float StartSceneLoadDelay = 0.5f;
    private float LevelLoadDelay = 1.7f;
    private float LevelRestartDelay = 1f;

    private int currentSceneIndex;

    private float tp = 0;

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
        Time.timeScale = 1f;
        StartCoroutine(LoadStartMenu());
    }

    IEnumerator LoadStartMenu()
    {
        yield return new WaitForSecondsRealtime(StartSceneLoadDelay);
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator LoadNextLevel()
    {
        baths.Play();

        StartCoroutine(ChangeColor());
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

    public void LoadSceneWithName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


    private IEnumerator ChangeColor()
    {

        yield return new WaitForSeconds(0.5f);

        for (float f = 0f; f <= 255; f += 0.03f)
        {
            yield return new WaitForSeconds(0.02f);
            transparentImage.color = new Color(0, 0, 0, f);
        }
        yield return null;


    }

}
