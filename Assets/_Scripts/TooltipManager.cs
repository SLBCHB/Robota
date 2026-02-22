using UnityEngine;
using TMPro;
using System.Collections;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;

    [Header("UI References")]
    [Tooltip("The main UI object containing the background and text")]
    public RectTransform tooltipContainer;
    [Tooltip("The TextMeshPro component")]
    public TextMeshProUGUI tooltipText;
    [Tooltip("CanvasGroup for smooth fading")]
    public CanvasGroup canvasGroup;

    [Header("Settings")]
    public Vector2 screenOffset = new Vector2(0, 50f);
    public float fadeSpeed = 5f;

    private Transform _currentTarget;
    private Camera _mainCam;
    private Coroutine _activeRoutine;

    private void Awake()
    {
        Instance = this;
        _mainCam = Camera.main;
        
        canvasGroup.alpha = 0f;
        tooltipContainer.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_currentTarget != null && tooltipContainer.gameObject.activeSelf)
        {
            UpdatePosition();
        }
    }
    
    public void ShowDialogue(string message, Transform target, float duration = 2f)
    {
        if (_activeRoutine != null) StopCoroutine(_activeRoutine);
        
        _currentTarget = target;
        tooltipText.text = message;
        
        Canvas.ForceUpdateCanvases(); 
        
        UpdatePosition();
        _activeRoutine = StartCoroutine(DialogueRoutine(duration));
    }

    public void HideDialogueInstantly()
    {
        if (_activeRoutine != null) StopCoroutine(_activeRoutine);
        _currentTarget = null;
        canvasGroup.alpha = 0f;
        tooltipContainer.gameObject.SetActive(false);
    }

    private void UpdatePosition()
    {
        if (_currentTarget == null || _mainCam == null) return;

        Vector2 screenPos = _mainCam.WorldToScreenPoint(_currentTarget.position);
        tooltipContainer.position = screenPos + screenOffset;
    }

    private IEnumerator DialogueRoutine(float duration)
    {
        tooltipContainer.gameObject.SetActive(true);

        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }

        yield return new WaitForSeconds(duration);

        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }

        tooltipContainer.gameObject.SetActive(false);
        _currentTarget = null;
    }
}