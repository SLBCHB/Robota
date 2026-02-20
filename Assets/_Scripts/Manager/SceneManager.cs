using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameScene
{
    MainMenu,
    Loading,
}

public class SceneController : Singleton<SceneController>
{
    public event Action OnLoadStart;
    public event Action OnLoadComplete;

    public float LoadingProgress { get; private set; }

    public void LoadScene(GameScene sceneToLoad)
    {
        StartCoroutine(LoadSceneSequence(sceneToLoad.ToString()));
    }

    public void LoadNextScene()
    {

        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            string nextScenePath = SceneUtility.GetScenePathByBuildIndex(nextIndex);
            string nextSceneName = System.IO.Path.GetFileNameWithoutExtension(nextScenePath);
            StartCoroutine(LoadSceneSequence(nextSceneName));
        }
        else
        {
            Debug.LogWarning("[SceneController] Tohle je poslední scéna v buildu!");
        }
    }

    private IEnumerator LoadSceneSequence(string targetSceneName)
    {
        OnLoadStart?.Invoke();
        LoadingProgress = 0f;

        SceneManager.LoadScene(GameScene.Loading.ToString());

        yield return null;

        AsyncOperation operation = SceneManager.LoadSceneAsync(targetSceneName);

        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            LoadingProgress = Mathf.Clamp01(operation.progress / 0.9f);

            if (operation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(1f);
                operation.allowSceneActivation = true;
            }

            yield return null;
        }

        OnLoadComplete?.Invoke();
    }
}