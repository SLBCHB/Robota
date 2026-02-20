using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; 
public enum GameScene
{
    MainMenu,
}

public class SceneController : Singleton<SceneController>
{
    [Header("UI Elements")]
    [SerializeField] private GameObject loadingScreen;

    public event Action OnLoadStart;
    public event Action OnLoadComplete;

    protected override void Awake()
    {
        base.Awake();

        if (loadingScreen != null)
            loadingScreen.SetActive(false);
    }

    public void LoadScene(GameScene sceneToLoad)
    {
        string sceneName = sceneToLoad.ToString();
        StartCoroutine(LoadSceneAsync(sceneName));
    }


    public void LoadNextScene()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            StartCoroutine(LoadSceneAsync(nextIndex));
        }
        else
        {
            Debug.LogWarning("[SceneController] Tohle je poslední scéna v buildu!");
        }
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        OnLoadStart?.Invoke();

        if (loadingScreen != null)
            loadingScreen.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            // Unity načítá scény do hodnoty 0.9, od 0.9 do 1.0 probíhá aktivace scény
            // float progress = Mathf.Clamp01(operation.progress / 0.9f);
            // if (progressBar != null) progressBar.value = progress;

            yield return null;
        }

        if (loadingScreen != null)
            loadingScreen.SetActive(false);

        OnLoadComplete?.Invoke();
    }

    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        OnLoadStart?.Invoke();
        if (loadingScreen != null) loadingScreen.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            yield return null;
        }

        if (loadingScreen != null) loadingScreen.SetActive(false);
        OnLoadComplete?.Invoke();
    }
}