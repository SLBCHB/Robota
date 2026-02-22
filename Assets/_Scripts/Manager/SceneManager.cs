using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Required for Image
using UnityEngine.SceneManagement;

public enum GameScene
{
    MainMenu,
    loopVjck,
    Loading,
    Intro,
    johny,
    Map,
    GoodEnd,
    BadEnd
}

public class SceneController : Singleton<SceneController>
{
    public event Action OnLoadStart;
    public event Action OnLoadComplete;


    public float LoadingProgress { get; private set; }

    // Fade Settings
    public CanvasGroup _fadeCanvasGroup;
    private float _fadeDuration = 0.5f;

    protected override void Awake()
    {
        base.Awake();
        SetupFadeUI();


    }

    private void SetupFadeUI()
    {
        // Create Canvas for the fade overlay
        GameObject canvasObj = new GameObject("FadeOverlay");
        canvasObj.transform.SetParent(this.transform);
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999; // Ensure it is on top

        _fadeCanvasGroup = canvasObj.AddComponent<CanvasGroup>();
        _fadeCanvasGroup.alpha = 0f;
        _fadeCanvasGroup.blocksRaycasts = false;

        // Create the Black Background
        GameObject imgObj = new GameObject("FadeImage");
        imgObj.transform.SetParent(canvasObj.transform);
        Image img = imgObj.AddComponent<Image>();
        img.color = Color.black;

        // Stretch to fill screen
        RectTransform rect = img.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.one;

       
    }


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

        // 1. Fade to Black (Current Scene)
        yield return StartCoroutine(Fade(1f));

        // 2. Load the Loading "Sandwich" Scene
        yield return SceneManager.LoadSceneAsync(GameScene.Loading.ToString());

        // 3. Reveal the Loading Scene
        yield return StartCoroutine(Fade(0f));

        // Start loading the actual target in the background
        AsyncOperation operation = SceneManager.LoadSceneAsync(targetSceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            LoadingProgress = Mathf.Clamp01(operation.progress / 0.9f);

            if (operation.progress >= 0.9f)
            {
                // 4. Fade to Black again (Hiding Loading Scene)
                yield return StartCoroutine(Fade(1f));

                operation.allowSceneActivation = true;
            }

            yield return null;
        }

        // 5. Final Reveal (New Scene)
        yield return StartCoroutine(Fade(0f));

        OnLoadComplete?.Invoke();
    }

    public IEnumerator Fade(float targetAlpha)
    {
        _fadeCanvasGroup.blocksRaycasts = true;
        float startAlpha = _fadeCanvasGroup.alpha;
        float time = 0;

        while (time < _fadeDuration)
        {
            time += Time.deltaTime;
            _fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / _fadeDuration);
            yield return null;
        }

        _fadeCanvasGroup.alpha = targetAlpha;
        if (targetAlpha == 0) _fadeCanvasGroup.blocksRaycasts = false;
    }
}