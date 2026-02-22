using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(CinemachineCamera))]
public class CameraController : MonoBehaviour
{   
    public static CameraController Instance; 

    public enum ToolType { Claw, Scanner, Fingerprint }
    
    [Header("Tool Settings")]
    public ToolType activeTool = ToolType.Claw;

    [Header("Camera Settings")] 
    public Vector2 shiftStrength = new Vector2(2f, 2f);
    public float smoothSpeed = 5f;

    [Header("Cursor Settings")]
    public Canvas cursorCanvas; 
    public GameObject defaultCursorPrefab;
    public GameObject pointerCursorPrefab;
    public string interactableTag = "interactable";
    
    [Header("Screen Zones")]
    public float tableHeightRatio = 0.45f;

    public event Action<List<GameObject>> OnCameraClickEvent;

    private CinemachineCamera _vcam;
    private CinemachineFollow _followComponent;
    private Vector3 _initialOffset;

    private RectTransform _cursorDefaultRT;
    private RectTransform _cursorPointerRT;
    private Camera _mainCam;
    
    private GraphicRaycaster _uiRaycaster;
    private EventSystem _eventSystem;
    private PointerEventData _pointerEventData;
    private List<RaycastResult> _raycastResults = new List<RaycastResult>(); 

    private Vector2 _currentMousePos;
    private bool _isOverUI;
    
    private CameraMoveController _moveController;
    private List<GameObject> _hoveredWorldObject;

    private void Awake() => Instance = this;

    private void Start()
    {
        _vcam = GetComponent<CinemachineCamera>();
        _followComponent = _vcam.GetComponent<CinemachineFollow>();
        _mainCam = Camera.main;
        _uiRaycaster = cursorCanvas.GetComponent<GraphicRaycaster>();
        _eventSystem = EventSystem.current;
        
        _pointerEventData = new PointerEventData(_eventSystem);
        
        if (_followComponent != null)
            _initialOffset = _followComponent.FollowOffset;

        Cursor.visible = false;
        
        _moveController = FindFirstObjectByType<CameraMoveController>();
        
        GameObject defCursorObj = Instantiate(defaultCursorPrefab, cursorCanvas.transform);
        GameObject ptrCursorObj = Instantiate(pointerCursorPrefab, cursorCanvas.transform);
        
        _cursorDefaultRT = defCursorObj.GetComponent<RectTransform>();
        _cursorPointerRT = ptrCursorObj.GetComponent<RectTransform>();

        _cursorDefaultRT.pivot = new Vector2(0f, 1f);
        _cursorPointerRT.pivot = new Vector2(0f, 1f);

        CanvasGroup defGroup = defCursorObj.AddComponent<CanvasGroup>();
        defGroup.interactable = false;
        defGroup.blocksRaycasts = false;

        CanvasGroup ptrGroup = ptrCursorObj.AddComponent<CanvasGroup>();
        ptrGroup.interactable = false;
        ptrGroup.blocksRaycasts = false;

        _cursorPointerRT.gameObject.SetActive(false);

        InputManager.Instance.CameraClickEvent += HandleInputClick;
    }

    private void OnDestroy()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.CameraClickEvent -= HandleInputClick;
    }

    private void Update()
    {
        Vector2 mousePos = InputManager.Instance.MousePosition;
        _currentMousePos = mousePos;

        _isOverUI = CheckUI(_currentMousePos);
        _hoveredWorldObject = GetWorldObjectUnderMouse(_currentMousePos);
        
        bool isAtGate = _moveController != null ? _moveController.isAtGate : true;

        if (isAtGate)
        {
            HandleCameraShift(_currentMousePos);
        }

        UpdateCursorVisuals(_currentMousePos, isAtGate);
        HandleCameraShift(_currentMousePos);
    }

    public void SelectClawTool()
    {
        activeTool = ToolType.Claw;
        Debug.Log("Equipped the Claw!");
    }

    public void SelectScannerTool()
    {
        activeTool = ToolType.Scanner;
        Debug.Log("Equipped the Light Scanner!");
    }
    
    public void ToggleFingerprintTool()
    { 
        if (activeTool == ToolType.Fingerprint)
        {
            activeTool = ToolType.Claw;
            
            Debug.Log("Retracted the Fingerprint Scanner.");
        }
        else
        {
            activeTool = ToolType.Fingerprint;
            Debug.Log("Deployed the Fingerprint Scanner!");
        }
    }

    private void HandleInputClick()
    {
        if (_moveController != null && !_moveController.isAtGate) return;
        
        if (_isOverUI)
        {
            if (_raycastResults.Count > 0)
            {
                Debug.Log($"Manager: Hovering over UI element {_raycastResults[0].gameObject.name}, letting EventSystem click it.");
            }
            return; 
        }

        OnCameraClickEvent?.Invoke(_hoveredWorldObject);
    }

    private void UpdateCursorVisuals(Vector2 mousePos, bool isAtGate)
    {
        _cursorDefaultRT.position = mousePos;
        _cursorPointerRT.position = mousePos;

        bool isOverInteractable = false;
        foreach(GameObject go in _hoveredWorldObject)
        {
            isOverInteractable = go.CompareTag(interactableTag);
        }
        
        bool shouldShowPointer = _isOverUI || isOverInteractable;
        bool isOverTable = (mousePos.y / Screen.height) <= tableHeightRatio;

        bool showFingerprint = isAtGate && (activeTool == ToolType.Fingerprint);
        bool showClaw = isAtGate && (!isOverTable && activeTool == ToolType.Claw);
        bool showScanner = isAtGate && (!isOverTable && activeTool == ToolType.Scanner);
        
        bool showDefaultCursor = (!isAtGate) || (isOverTable && !showFingerprint); 

        if (FingerprintController.Instance != null) FingerprintController.Instance.SetActiveState(showFingerprint);
        if (ClawController.Instance != null) ClawController.Instance.SetActiveState(showClaw);
        if (ScannerController.Instance != null) ScannerController.Instance.SetActiveState(showScanner);

        if (showDefaultCursor)
        {
            _cursorDefaultRT.gameObject.SetActive(!shouldShowPointer);
            _cursorPointerRT.gameObject.SetActive(shouldShowPointer);
        }
        else
        {
            _cursorDefaultRT.gameObject.SetActive(false);
            _cursorPointerRT.gameObject.SetActive(false);
        }
    }
    
    private Vector2 GetClampedMousePos()
    {
        Vector2 raw = InputManager.Instance.MousePosition;
        return new Vector2(Mathf.Clamp(raw.x, 0, Screen.width), Mathf.Clamp(raw.y, 0, Screen.height));
    }

    private void HandleCameraShift(Vector2 mousePos)
    {
        float mouseX = (mousePos.x / Screen.width) * 2f - 1f;
        float mouseY = (mousePos.y / Screen.height) * 2f - 1f;
        Vector3 targetShift = new Vector3(mouseX * shiftStrength.x, mouseY * shiftStrength.y, 0f);
        _followComponent.FollowOffset = Vector3.Lerp(_followComponent.FollowOffset, _initialOffset + targetShift, Time.deltaTime * smoothSpeed);
    }

    private bool CheckUI(Vector2 mousePos)
    {
        if (_eventSystem == null) return false;
        
        _pointerEventData.position = mousePos;
        _raycastResults.Clear();
        _uiRaycaster.Raycast(_pointerEventData, _raycastResults);
        
        return _raycastResults.Count > 0;
    }

    private List<GameObject> GetWorldObjectUnderMouse(Vector2 mousePos)
    {
        float dist = Mathf.Abs(_mainCam.transform.position.z);
        Vector3 worldPos = _mainCam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, dist));
        worldPos.z = 0;

        Collider2D[] hits = Physics2D.OverlapCircleAll(worldPos, 0.2f);

        List<GameObject> hitObjects = new List<GameObject>();

        foreach (Collider2D hit in hits)
        {
            hitObjects.Add(hit.gameObject);
        }

        return hitObjects;
    }
}