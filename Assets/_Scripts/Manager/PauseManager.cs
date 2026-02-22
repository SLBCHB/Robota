using UnityEngine;

public class PauseManager : Singleton<PauseManager>
{
    [SerializeField] private GameObject pauseMenuPrefab;
    private GameObject _pauseMenuInstance;
    private bool _isPaused = false;

    private void Start()
    {
        // Subscribe to your InputManager's event
        InputManager.Instance.PauseEvent += HandlePauseInput;
    }

    private void HandlePauseInput()
    {
        Debug.Log("Pause input received!");
        // Check if we are currently fading (from SceneController)
        //if (SceneController.Instance.IsFading) return;

        TogglePause();
    }

    public void TogglePause()
    {
        _isPaused = !_isPaused;
        Time.timeScale = _isPaused ? 0f : 1f;

        if (_pauseMenuInstance != null) _pauseMenuInstance.SetActive(_isPaused);

        if (_isPaused)
        {
            // Use your InputManager's built-in switching methods!
            InputManager.Instance.SwitchToUI();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            InputManager.Instance.SwitchToPlayer();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void InitializeUI(Transform canvasTransform)
    {
        if (pauseMenuPrefab != null && _pauseMenuInstance == null)
        {
            _pauseMenuInstance = Instantiate(pauseMenuPrefab, canvasTransform);
            _pauseMenuInstance.SetActive(false);
            _pauseMenuInstance.transform.SetAsFirstSibling();
        }
    }
}